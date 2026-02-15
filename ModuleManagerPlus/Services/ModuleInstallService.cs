using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;
using Flurl.Http;
using ModuleManagerPlus.Data;

namespace ModuleManagerPlus.Services
{
	internal class ModuleInstallService
	{
		private static readonly Logger Logger = Logger.GetLogger<ModuleInstallService>();

		public ModuleManager FindInstalledModule(Module module)
		{
			return GameService.Module.get_Modules().FirstOrDefault((ModuleManager m) => string.Equals(m.get_Manifest().get_Namespace(), module.Namespace, StringComparison.OrdinalIgnoreCase));
		}

		public ModuleInstallState GetInstallState(Module module)
		{
			ModuleManager installed = FindInstalledModule(module);
			if (installed == null)
			{
				return ModuleInstallState.NotInstalled;
			}
			Release latestRelease = (from r in module.Releases?.Where((Release r) => !r.IsPrerelease)
				orderby r.TypedVersion descending
				select r).FirstOrDefault();
			if (latestRelease != null && latestRelease.TypedVersion > installed.get_Manifest().get_Version())
			{
				return ModuleInstallState.UpdateAvailable;
			}
			return ModuleInstallState.Installed;
		}

		private string GetModulesDirectory()
		{
			ModuleManager anyModule = GameService.Module.get_Modules().FirstOrDefault();
			if (anyModule != null)
			{
				return Path.GetDirectoryName(anyModule.get_DataReader().get_PhysicalPath());
			}
			Logger.Warn("Could not determine modules directory — no modules are loaded.");
			return null;
		}

		public async Task<(bool Success, string Error)> InstallModule(Module module, Release release, IProgress<string> progress = null)
		{
			string modulesDir = GetModulesDirectory();
			if (modulesDir == null)
			{
				return (false, "Could not determine modules directory.");
			}
			string moduleName = module.Namespace + "_" + release.Version + ".bhm";
			string fullPath = Path.Combine(modulesDir, moduleName);
			if (File.Exists(fullPath))
			{
				return (false, "Module already exists at " + fullPath + ".");
			}
			try
			{
				progress?.Report("Downloading module...");
				byte[] downloadedModule = await release.DownloadUrl.GetBytesAsync(default(CancellationToken), (HttpCompletionOption)0);
				progress?.Report("Saving module...");
				File.WriteAllBytes(fullPath, downloadedModule);
				Logger.Info("Module saved to '" + fullPath + "'.");
				progress?.Report("Registering module...");
				if (GameService.Module.RegisterPackedModule(fullPath) == null)
				{
					TryDeleteFile(fullPath);
					return (false, "Module registration failed.");
				}
				progress?.Report("");
				Logger.Info("Module '" + module.Name + "' v" + release.Version + " installed successfully.");
				return (true, string.Empty);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to install module '" + module.Name + "'.");
				TryDeleteFile(fullPath);
				return (false, "Install failed: " + ex.Message);
			}
		}

		public async Task<(bool Success, string Error)> UpdateModule(Module module, Release release, IProgress<string> progress = null)
		{
			ModuleManager existing = FindInstalledModule(module);
			if (existing == null)
			{
				return await InstallModule(module, release, progress);
			}
			bool wasEnabled = existing.get_Enabled();
			if (wasEnabled)
			{
				progress?.Report("Disabling current version...");
				existing.Disable();
			}
			string modulesDir = GetModulesDirectory();
			if (modulesDir == null)
			{
				return (false, "Could not determine modules directory.");
			}
			string moduleName = module.Namespace + "_" + release.Version + ".bhm";
			string fullPath = Path.Combine(modulesDir, moduleName);
			try
			{
				progress?.Report("Downloading update...");
				byte[] downloadedModule = await release.DownloadUrl.GetBytesAsync(default(CancellationToken), (HttpCompletionOption)0);
				progress?.Report("Saving update...");
				File.WriteAllBytes(fullPath, downloadedModule);
				progress?.Report("Removing old version...");
				existing.DeleteModule();
				progress?.Report("Registering update...");
				if (GameService.Module.RegisterPackedModule(fullPath) == null)
				{
					TryDeleteFile(fullPath);
					return (false, "Module registration failed after update.");
				}
				if (wasEnabled)
				{
					GameService.Module.get_ModuleStates().get_Value()[module.Namespace].set_Enabled(true);
					GameService.Settings.Save(false);
				}
				progress?.Report("");
				Logger.Info("Module '" + module.Name + "' updated to v" + release.Version + ".");
				return (true, string.Empty);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to update module '" + module.Name + "'.");
				return (false, "Update failed: " + ex.Message);
			}
		}

		public (bool Success, string Error) UninstallModule(Module module)
		{
			ModuleManager existing = FindInstalledModule(module);
			if (existing == null)
			{
				return (false, "Module is not installed.");
			}
			try
			{
				Logger.Info("Uninstalling module '" + module.Name + "'...");
				existing.DeleteModule();
				Logger.Info("Module '" + module.Name + "' uninstalled.");
				return (true, string.Empty);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to uninstall module '" + module.Name + "'.");
				return (false, "Uninstall failed: " + ex.Message);
			}
		}

		private void TryDeleteFile(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed to clean up file '" + path + "'.");
			}
		}
	}
}
