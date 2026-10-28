using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DeimosOilSummary.Models;

namespace DeimosOilSummary.Services
{
	public sealed class LogWatcher : IDisposable
	{
		private static readonly string[] LogExtensions = new string[2] { ".zevtc", ".evtc" };

		private readonly ConcurrentDictionary<string, byte> _seen = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);

		private FileSystemWatcher[] _watchers = Array.Empty<FileSystemWatcher>();

		private Timer _pollTimer;

		private int _parseGate;

		private bool _disposed;

		public string[] WatchedPaths { get; private set; } = Array.Empty<string>();


		public event EventHandler<OilHitSummary> SummaryReady;

		public event EventHandler<string> StatusChanged;

		public void Start()
		{
			WatchedPaths = DiscoverLogRoots();
			if (WatchedPaths.Length == 0)
			{
				RaiseStatus("No ArcDPS log folder found. Expected …/addons/arcdps/arcdps.cbtlogs");
				return;
			}
			RaiseStatus("Watching: " + string.Join("; ", WatchedPaths));
			_watchers = (from w in WatchedPaths.Select(CreateWatcher)
				where w != null
				select w).ToArray();
			string[] watchedPaths = WatchedPaths;
			for (int i = 0; i < watchedPaths.Length; i++)
			{
				foreach (string file in EnumerateLogFiles(watchedPaths[i]))
				{
					_seen.TryAdd(file, 0);
				}
			}
			_pollTimer = new Timer(delegate
			{
				PollForNewLogs();
			}, null, TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0));
		}

		public OilHitSummary ParseLatestDeimos()
		{
			string latest = WatchedPaths.SelectMany(EnumerateLogFiles).Where(IsDeimosPath).OrderByDescending(File.GetLastWriteTimeUtc)
				.FirstOrDefault();
			if (latest == null)
			{
				return new OilHitSummary
				{
					Success = false,
					ErrorMessage = "No Deimos logs found yet."
				};
			}
			return EvtcOilParser.Parse(latest);
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				_pollTimer?.Dispose();
				_pollTimer = null;
				FileSystemWatcher[] watchers = _watchers;
				foreach (FileSystemWatcher obj in watchers)
				{
					obj.EnableRaisingEvents = false;
					obj.Dispose();
				}
				_watchers = Array.Empty<FileSystemWatcher>();
			}
		}

		private FileSystemWatcher CreateWatcher(string root)
		{
			try
			{
				if (!Directory.Exists(root))
				{
					return null;
				}
				FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(root);
				fileSystemWatcher.IncludeSubdirectories = true;
				fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
				fileSystemWatcher.InternalBufferSize = 65536;
				fileSystemWatcher.Created += OnFileEvent;
				fileSystemWatcher.Changed += OnFileEvent;
				fileSystemWatcher.Renamed += delegate(object s, RenamedEventArgs e)
				{
					QueueFile(e.FullPath);
				};
				fileSystemWatcher.EnableRaisingEvents = true;
				return fileSystemWatcher;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private void OnFileEvent(object sender, FileSystemEventArgs e)
		{
			QueueFile(e.FullPath);
		}

		private void QueueFile(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			string ext = Path.GetExtension(path);
			if (!LogExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase) || !IsDeimosPath(path))
			{
				return;
			}
			Task.Run(async delegate
			{
				await Task.Delay(2500).ConfigureAwait(continueOnCapturedContext: false);
				if (WaitUntilStable(path, TimeSpan.FromSeconds(20.0)) && _seen.TryAdd(path, 0))
				{
					ParseAndRaise(path);
				}
			});
		}

		private void PollForNewLogs()
		{
			if (_disposed)
			{
				return;
			}
			string[] watchedPaths = WatchedPaths;
			for (int i = 0; i < watchedPaths.Length; i++)
			{
				foreach (string file in EnumerateLogFiles(watchedPaths[i]))
				{
					if (IsDeimosPath(file) && _seen.TryAdd(file, 0))
					{
						if (!WaitUntilStable(file, TimeSpan.FromSeconds(5.0)))
						{
							_seen.TryRemove(file, out var _);
						}
						else
						{
							ParseAndRaise(file);
						}
					}
				}
			}
		}

		private void ParseAndRaise(string path)
		{
			byte value;
			if (Interlocked.CompareExchange(ref _parseGate, 1, 0) != 0)
			{
				_seen.TryRemove(path, out value);
				return;
			}
			try
			{
				RaiseStatus("Parsing " + Path.GetFileName(path) + "…");
				OilHitSummary summary = EvtcOilParser.Parse(path);
				if (!summary.Success)
				{
					RaiseStatus(summary.ErrorMessage ?? "Parse failed.");
					return;
				}
				RaiseStatus($"Deimos oil summary ready ({summary.TotalHits} hit(s)).");
				this.SummaryReady?.Invoke(this, summary);
			}
			catch (Exception ex)
			{
				RaiseStatus("Parse error: " + ex.Message);
				_seen.TryRemove(path, out value);
			}
			finally
			{
				Interlocked.Exchange(ref _parseGate, 0);
			}
		}

		private void RaiseStatus(string message)
		{
			this.StatusChanged?.Invoke(this, message);
		}

		private static bool IsDeimosPath(string path)
		{
			string name = path ?? string.Empty;
			if (name.IndexOf("Deimos", StringComparison.OrdinalIgnoreCase) < 0)
			{
				return name.IndexOf("(17154)", StringComparison.OrdinalIgnoreCase) >= 0;
			}
			return true;
		}

		private static bool WaitUntilStable(string path, TimeSpan timeout)
		{
			DateTime deadline = DateTime.UtcNow + timeout;
			long lastSize = -1L;
			while (DateTime.UtcNow < deadline)
			{
				try
				{
					FileInfo info = new FileInfo(path);
					if (!info.Exists)
					{
						Thread.Sleep(250);
						continue;
					}
					long size = info.Length;
					if (size > 0 && size == lastSize)
					{
						using (info.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
						{
							return true;
						}
					}
					lastSize = size;
					goto IL_0060;
				}
				catch (IOException)
				{
					goto IL_0060;
				}
				IL_0060:
				Thread.Sleep(400);
			}
			return false;
		}

		private static string[] DiscoverLogRoots()
		{
			return new string[4]
			{
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "addons", "arcdps", "arcdps.cbtlogs"),
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Guild Wars 2", "addons", "arcdps.cbtlogs"),
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive", "Documents", "Guild Wars 2", "addons", "arcdps", "arcdps.cbtlogs"),
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive", "Documents", "Guild Wars 2", "addons", "arcdps.cbtlogs")
			}.Where(Directory.Exists).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
		}

		[IteratorStateMachine(typeof(_003CEnumerateLogFiles_003Ed__28))]
		private static IEnumerable<string> EnumerateLogFiles(string root)
		{
			return new _003CEnumerateLogFiles_003Ed__28(-2)
			{
				_003C_003E3__root = root
			};
		}
	}
}
