using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blish_HUD;
using LiteDB;
using Nekres.ChatMacros.Core.Services.Data;

namespace Nekres.ChatMacros.Core.Services.Macro
{
	internal class FileMacroObserver : IDisposable
	{
		private ConcurrentDictionary<ObjectId, FileSystemWatcher> _watchers;

		public event EventHandler<ValueEventArgs<BaseMacro>> MacroUpdate;

		public FileMacroObserver()
		{
			_watchers = new ConcurrentDictionary<ObjectId, FileSystemWatcher>();
			foreach (ChatMacro macro in ChatMacros.Instance.Data.GetAllMacros())
			{
				if (!AddOrRemove(macro.Id, macro.LinkFile))
				{
					macro.LinkFile = string.Empty;
					ChatMacros.Instance.Data.Upsert(macro);
				}
			}
			ChatMacros.Instance.Data.LinkFileChange += OnLinkFileChanged;
		}

		private void OnLinkFileChanged(object sender, ValueEventArgs<BaseMacro> e)
		{
			AddOrRemove(e.get_Value().Id, e.get_Value().LinkFile);
		}

		public bool AddOrRemove(ObjectId id, string path)
		{
			if (!FileUtil.Exists(path, out var qualifiedPath, ChatMacros.Logger, ChatMacros.Instance.BasePaths.ToArray()))
			{
				Remove(id);
				return false;
			}
			try
			{
				string dir = Path.GetDirectoryName(qualifiedPath);
				string file = Path.GetFileName(qualifiedPath);
				_watchers.AddOrUpdate(id, delegate
				{
					FileSystemWatcher fileSystemWatcher2 = new FileSystemWatcher(dir)
					{
						Filter = file
					};
					RegisterEvents(fileSystemWatcher2);
					return fileSystemWatcher2;
				}, delegate(ObjectId _, FileSystemWatcher v)
				{
					v?.Dispose();
					FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(dir)
					{
						Filter = file
					};
					RegisterEvents(fileSystemWatcher);
					return fileSystemWatcher;
				});
			}
			catch (Exception e)
			{
				ChatMacros.Logger.Info(e, e.Message);
				return false;
			}
			return true;
		}

		private void RegisterEvents(FileSystemWatcher fw)
		{
			Import(_watchers.FirstOrDefault((KeyValuePair<ObjectId, FileSystemWatcher> x) => x.Value == fw).Key, Path.Combine(fw.Path, fw.Filter));
			fw.EnableRaisingEvents = true;
			fw.Changed += OnChanged;
			fw.Deleted += OnDeleted;
			fw.Created += OnChanged;
		}

		private void OnDeleted(object sender, FileSystemEventArgs e)
		{
			FileSystemWatcher fw = (FileSystemWatcher)sender;
			ObjectId id = _watchers.FirstOrDefault((KeyValuePair<ObjectId, FileSystemWatcher> x) => x.Value == fw).Key;
			ChatMacro macro = ChatMacros.Instance.Data.GetChatMacro(id);
			macro.LinkFile = string.Empty;
			ChatMacros.Instance.Data.Upsert(macro);
			Remove(id);
			this.MacroUpdate?.Invoke(this, new ValueEventArgs<BaseMacro>((BaseMacro)macro));
		}

		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			FileSystemWatcher fw = (FileSystemWatcher)sender;
			string dir = Path.GetDirectoryName(e.FullPath);
			string file = Path.GetFileName(e.FullPath);
			fw.Path = dir;
			fw.Filter = file;
			Import(_watchers.FirstOrDefault((KeyValuePair<ObjectId, FileSystemWatcher> x) => x.Value == fw).Key, e.FullPath);
		}

		private void Import(ObjectId id, string filePath)
		{
			if (id == null)
			{
				return;
			}
			ChatMacro macro = ChatMacros.Instance.Data.GetChatMacro(id);
			if (!ChatMacros.Instance.Macro.TryImportFromFile(filePath, out var lines))
			{
				return;
			}
			List<ChatLine> oldLines = macro.Lines.ToList();
			macro.Lines = lines.ToList();
			if (!macro.Lines.IsNullOrEmpty() && !ChatMacros.Instance.Data.Insert(macro.Lines.ToArray()))
			{
				macro.Lines = oldLines;
				ChatMacros.Logger.Warn($"Failed to insert lines from file for macro {macro.Id} ('{macro.Title}')");
				return;
			}
			if (!ChatMacros.Instance.Data.Upsert(macro))
			{
				macro.Lines = oldLines;
				ChatMacros.Logger.Warn($"Failed to upsert macro {macro.Id} ('{macro.Title}')");
				return;
			}
			foreach (ChatLine oldLine in oldLines)
			{
				ChatMacros.Instance.Data.Delete(oldLine);
			}
			this.MacroUpdate?.Invoke(this, new ValueEventArgs<BaseMacro>((BaseMacro)macro));
		}

		public void Remove(ObjectId id)
		{
			if (_watchers.TryRemove(id, out var watcher))
			{
				watcher?.Dispose();
			}
		}

		public void Dispose()
		{
			ChatMacros.Instance.Data.LinkFileChange -= OnLinkFileChanged;
			foreach (FileSystemWatcher value in _watchers.Values)
			{
				value?.Dispose();
			}
		}
	}
}
