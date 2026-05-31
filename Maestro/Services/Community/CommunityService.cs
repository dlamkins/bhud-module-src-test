using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Maestro.Models;
using Maestro.Services.Data;

namespace Maestro.Services.Community
{
	public class CommunityService : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<CommunityService>();

		private readonly CommunityApiClient _apiClient;

		private readonly SongStorage _songStorage;

		private readonly List<Song> _mainSongList;

		private readonly Dictionary<string, CancellationTokenSource> _activeDownloads;

		private CommunityManifest _manifest;

		private bool _isRefreshing;

		public CommunityManifest Manifest => _manifest;

		public bool IsRefreshing => _isRefreshing;

		public bool HasManifest => _manifest != null;

		public event EventHandler<DownloadProgressEventArgs> DownloadProgressChanged;

		public event EventHandler ManifestRefreshed;

		public CommunityService(SongStorage songStorage, List<Song> mainSongList)
		{
			_apiClient = new CommunityApiClient();
			_songStorage = songStorage;
			_mainSongList = mainSongList;
			_activeDownloads = new Dictionary<string, CancellationTokenSource>();
			_manifest = _songStorage.GetCachedManifest();
		}

		public async Task RefreshManifestAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (_isRefreshing)
			{
				return;
			}
			_isRefreshing = true;
			try
			{
				_manifest = await _apiClient.FetchManifestAsync(cancellationToken);
				_songStorage.SaveManifest(_manifest);
				this.ManifestRefreshed?.Invoke(this, EventArgs.Empty);
				Logger.Info($"Refreshed manifest with {_manifest.Songs.Count} songs");
			}
			catch (OperationCanceledException)
			{
				Logger.Debug("Manifest refresh cancelled");
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to refresh manifest");
				if (_manifest == null)
				{
					_manifest = _songStorage.GetCachedManifest();
				}
			}
			finally
			{
				_isRefreshing = false;
			}
		}

		public IEnumerable<CommunitySong> GetAvailableSongs()
		{
			IEnumerable<CommunitySong> enumerable = _manifest?.Songs;
			return enumerable ?? Enumerable.Empty<CommunitySong>();
		}

		public IEnumerable<CommunitySong> SearchSongs(string searchTerm, string instrumentFilter)
		{
			IEnumerable<CommunitySong> songs = GetAvailableSongs();
			if (!string.IsNullOrEmpty(instrumentFilter) && instrumentFilter != "All" && InstrumentCatalog.TryFromDisplayName(instrumentFilter, out var instrumentType))
			{
				songs = songs.Where((CommunitySong s) => s.InstrumentType == instrumentType);
			}
			if (!string.IsNullOrEmpty(searchTerm))
			{
				string term = searchTerm.ToLower();
				songs = songs.Where((CommunitySong s) => s.Name.ToLower().Contains(term) || s.Artist.ToLower().Contains(term) || s.Transcriber.ToLower().Contains(term));
			}
			return songs;
		}

		public bool IsSongDownloaded(string communityId)
		{
			return _songStorage.SongExists(communityId);
		}

		public bool IsDownloading(string communityId)
		{
			return _activeDownloads.ContainsKey(communityId);
		}

		public async Task<Song> DownloadSongAsync(CommunitySong communitySong, IProgress<int> progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (IsSongDownloaded(communitySong.Id))
			{
				Logger.Info("Song " + communitySong.Id + " already downloaded, returning cached version");
				return _songStorage.GetSong(communitySong.Id);
			}
			if (_activeDownloads.ContainsKey(communitySong.Id))
			{
				Logger.Warn("Download already in progress for " + communitySong.Id);
				return null;
			}
			CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			_activeDownloads[communitySong.Id] = cts;
			try
			{
				progress?.Report(10);
				RaiseDownloadProgress(communitySong.Id, 10, DownloadState.Downloading);
				Song song = await _apiClient.FetchSongAsync(communitySong.Id, cts.Token);
				progress?.Report(80);
				RaiseDownloadProgress(communitySong.Id, 80, DownloadState.Downloading);
				if (song == null)
				{
					RaiseDownloadProgress(communitySong.Id, 0, DownloadState.Failed);
					return null;
				}
				_songStorage.SaveSong(song);
				progress?.Report(100);
				RaiseDownloadProgress(communitySong.Id, 100, DownloadState.Completed);
				_mainSongList.Add(song);
				Logger.Info("Downloaded and added song: " + song.Name);
				return song;
			}
			catch (OperationCanceledException)
			{
				Logger.Debug("Download cancelled for " + communitySong.Id);
				RaiseDownloadProgress(communitySong.Id, 0, DownloadState.Cancelled);
				return null;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to download song " + communitySong.Id);
				RaiseDownloadProgress(communitySong.Id, 0, DownloadState.Failed);
				return null;
			}
			finally
			{
				_activeDownloads.Remove(communitySong.Id);
				cts.Dispose();
			}
		}

		public void CancelDownload(string communityId)
		{
			if (_activeDownloads.TryGetValue(communityId, out var cts))
			{
				cts.Cancel();
				Logger.Info("Cancelled download for " + communityId);
			}
		}

		public void DeleteDownloadedSong(Song song)
		{
			_songStorage.DeleteSong(song);
			_mainSongList.Remove(song);
			Logger.Info("Deleted song: " + song.Name + " (" + (song.CommunityId ?? "imported") + ")");
		}

		public async Task<List<Song>> LoadSubmittalsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			List<Song> submittals = new List<Song>();
			try
			{
				CommunityManifest pendingManifest = await _apiClient.FetchPendingManifestAsync(cancellationToken);
				if (pendingManifest?.Songs == null || pendingManifest.Songs.Count == 0)
				{
					return submittals;
				}
				CommunityManifest communityManifest = _manifest;
				if (communityManifest == null)
				{
					communityManifest = await _apiClient.FetchManifestAsync(cancellationToken);
				}
				HashSet<string> mainSongIds = new HashSet<string>(communityManifest?.Songs?.Select((CommunitySong s) => s.Id) ?? Enumerable.Empty<string>());
				List<CommunitySong> newSongs = pendingManifest.Songs.Where((CommunitySong s) => !mainSongIds.Contains(s.Id)).ToList();
				Logger.Info($"Found {newSongs.Count} submittal(s) in pending branch");
				foreach (CommunitySong communitySong in newSongs)
				{
					try
					{
						Song song = await _apiClient.FetchPendingSongAsync(communitySong.Id, cancellationToken);
						if (song != null)
						{
							song.IsSubmittal = true;
							submittals.Add(song);
							Logger.Info("Loaded submittal: " + song.Name);
						}
					}
					catch (Exception ex2)
					{
						Logger.Warn(ex2, "Failed to load submittal " + communitySong.Id + ", skipping");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to load submittals from pending branch");
			}
			return submittals;
		}

		private void RaiseDownloadProgress(string communityId, int progress, DownloadState state)
		{
			this.DownloadProgressChanged?.Invoke(this, new DownloadProgressEventArgs(communityId, progress, state));
		}

		public void Dispose()
		{
			foreach (CancellationTokenSource value in _activeDownloads.Values)
			{
				value.Cancel();
				value.Dispose();
			}
			_activeDownloads.Clear();
			_apiClient?.Dispose();
		}
	}
}
