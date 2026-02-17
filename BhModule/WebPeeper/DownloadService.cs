using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BhModule.WebPeeper
{
	internal class DownloadService
	{
		private static readonly string[] _exludeExtractExts = new string[2] { ".pdb", ".xml" };

		private int _downloadingCount;

		private int _downloadedCount;

		private readonly HashSet<CefPkgVersion> _downloadingVersions = new HashSet<CefPkgVersion>();

		private readonly IProgress<float> _progress;

		private static CefService CefService => WebPeeperModule.Instance.CefService;

		public float ProgressPercentage { get; private set; }

		public bool Downloading => _downloadingCount > 0;

		public IReadOnlyCollection<CefPkgVersion> DownloadingVersions => _downloadingVersions;

		public DownloadService()
		{
			_progress = new Progress<float>(delegate(float val)
			{
				ProgressPercentage = val;
			});
		}

		public bool CheckCefLib(CefPkgVersion version)
		{
			if (version == CefService.DefaultVersion)
			{
				return true;
			}
			string[] array = new string[7] { "CefSharp.OffScreen.dll", "CefSharp.dll", "CefSharp.Core.dll", "CefSharp.Core.Runtime.dll", "CefSharp.BrowserSubprocess.exe", "CefSharp.BrowserSubprocess.Core.dll", "libcef.dll" };
			foreach (string path in array)
			{
				if (!File.Exists(Path.Combine(CefService.GetCefSharpFolder(version), path)))
				{
					return false;
				}
			}
			return true;
		}

		public void Delete(CefPkgVersion version)
		{
			bool num = CefService.LibLoadStarted && version == CefService.CurrentVersion;
			bool flag = version == CefService.DefaultVersion;
			if (!(num || flag))
			{
				try
				{
					Directory.Delete(CefService.GetCefSharpFolder(version), recursive: true);
				}
				catch
				{
				}
			}
		}

		public async Task Download(CefPkgVersion cefPkgVersion)
		{
			WebPeeperModule.Logger.Debug("DownloadService.Download: try download if didnt download.");
			try
			{
				bool num = CheckCefLib(cefPkgVersion);
				bool flag = _downloadingVersions.Contains(cefPkgVersion);
				if (num || flag)
				{
					return;
				}
				WebPeeperModule.Logger.Debug($"DownloadService.Download: downloading cef {cefPkgVersion}.");
				if (_downloadingCount == 0)
				{
					_progress.Report(0f);
				}
				HttpClient client = new HttpClient();
				try
				{
					NugetResource nugetResource = JsonSerializer.Deserialize<NugetIndex>(await client.GetStringAsync("https://api.nuget.org/v3/index.json"), (JsonSerializerOptions)null).Resources.First((NugetResource r) => r.Type?.Contains("PackageBaseAddress") ?? false);
					string nugetHostUrl = nugetResource.Id;
					Package[] packages = new Package[3]
					{
						new Package("CefSharp.OffScreen", cefPkgVersion.CefSharp, new string[1] { "lib/net462/" }),
						new Package("CefSharp.Common", cefPkgVersion.CefSharp, new string[2] { "CefSharp/x64/", "lib/net462/" }),
						new Package("chromiumembeddedframework.runtime.win-x64", cefPkgVersion.Cef, new string[2] { "CEF/win-x64/", "runtimes/win-x64/native/" })
					};
					_downloadingCount += packages.Length;
					_downloadingVersions.Add(cefPkgVersion);
					CefVersionSettingView.UpdateView?.Invoke();
					Package[] array = packages;
					foreach (Package package in array)
					{
						string text = package.Name.ToLowerInvariant();
						string text2 = $"{nugetHostUrl}{text}/{package.Version}/{text}.{package.Version}.nupkg";
						HttpResponseMessage nupkgUrlResp = await client.GetAsync(text2, (HttpCompletionOption)1);
						try
						{
							int nupkgFileSize = (int)nupkgUrlResp.get_Content().get_Headers().get_ContentLength()
								.GetValueOrDefault();
							using Stream nupkgFileStream = await nupkgUrlResp.get_Content().ReadAsStreamAsync();
							using MemoryStream downloadedStream = new MemoryStream();
							nupkgFileStream.CopyToAsync(downloadedStream);
							while (downloadedStream.Length < nupkgFileSize)
							{
								await Task.Delay(50);
								float num2 = ((float)downloadedStream.Length / (float)nupkgFileSize + (float)_downloadedCount) / (float)packages.Length;
								if (num2 < 1f)
								{
									_progress.Report(num2);
								}
							}
							ZipArchive zip = new ZipArchive((Stream)downloadedStream, (ZipArchiveMode)0);
							try
							{
								foreach (ZipArchiveEntry entry in zip.get_Entries())
								{
									if (_exludeExtractExts.Contains(Path.GetExtension(entry.get_Name()).ToLower()))
									{
										continue;
									}
									string[] array2 = package.PendingFiles.ToArray();
									foreach (string path in array2)
									{
										if (entry.get_FullName().IndexOf(path) != 0)
										{
											continue;
										}
										await Task.Run(delegate
										{
											string path2 = Path.Combine(CefService.GetCefSharpFolder(cefPkgVersion), entry.get_FullName().Replace(path, ""));
											Directory.CreateDirectory(Path.GetDirectoryName(path2));
											using Stream stream = entry.Open();
											using FileStream destination = new FileStream(path2, FileMode.Create, FileAccess.Write);
											stream.CopyTo(destination);
										});
									}
								}
								_downloadedCount++;
							}
							finally
							{
								((IDisposable)zip)?.Dispose();
							}
						}
						finally
						{
							((IDisposable)nupkgUrlResp)?.Dispose();
						}
					}
					if (_downloadedCount == _downloadingCount)
					{
						_downloadedCount = 0;
						_downloadingCount = 0;
						_downloadingVersions.Clear();
						_progress.Report(1f);
						WebPeeperModule.Logger.Debug("DownloadService.Download: all end.");
					}
				}
				finally
				{
					((IDisposable)client)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				WebPeeperModule.Logger.Error(ex.Message);
				throw;
			}
		}
	}
}
