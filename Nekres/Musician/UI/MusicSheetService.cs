using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Microsoft.Xna.Framework.Audio;
using Nekres.Musician.Core.Models;
using Nekres.Musician.UI.Models;
using SQLite;

namespace Nekres.Musician.UI
{
	internal class MusicSheetService : IDisposable
	{
		private SQLiteAsyncConnection _db;

		private SoundEffect[] _deleteSfx;

		public string CacheDir { get; private set; }

		public SoundEffect DeleteSfx => _deleteSfx[RandomUtil.GetRandom(0, 1)];

		public event EventHandler<ValueEventArgs<MusicSheetModel>> OnSheetUpdated;

		public MusicSheetService(string cacheDir)
		{
			_deleteSfx = (SoundEffect[])(object)new SoundEffect[2]
			{
				MusicianModule.ModuleInstance.ContentsManager.GetSound("audio\\crumbling-paper-1.wav"),
				MusicianModule.ModuleInstance.ContentsManager.GetSound("audio\\crumbling-paper-2.wav")
			};
			CacheDir = cacheDir;
		}

		public async Task LoadAsync()
		{
			await LoadDatabase();
		}

		private async Task LoadDatabase()
		{
			string filePath = Path.Combine(CacheDir, "db.sqlite");
			_db = new SQLiteAsyncConnection(filePath);
			await _db.CreateTableAsync<MusicSheetModel>();
		}

		public async Task AddOrUpdate(MusicSheet musicSheet, bool silent = false)
		{
			if (await _db.Table<MusicSheetModel>().FirstOrDefaultAsync((MusicSheetModel x) => x.Id.Equals(musicSheet.Id)) == null)
			{
				await _db.InsertAsync(musicSheet.ToModel());
			}
			else
			{
				MusicSheetModel model = musicSheet.ToModel();
				await _db.UpdateAsync(model);
				this.OnSheetUpdated?.Invoke(this, new ValueEventArgs<MusicSheetModel>(model));
			}
			if (!silent)
			{
				GameService.Content.PlaySoundEffectByName("color-change");
			}
		}

		public async Task Delete(Guid key)
		{
			DeleteSfx.Play(GameService.GameIntegration.get_Audio().get_Volume(), 0f, 0f);
			await _db.Table<MusicSheetModel>().DeleteAsync((MusicSheetModel x) => x.Id.Equals(key));
		}

		public void Dispose()
		{
			SoundEffect[] deleteSfx = _deleteSfx;
			foreach (SoundEffect obj in deleteSfx)
			{
				if (obj != null)
				{
					obj.Dispose();
				}
			}
		}

		public async Task<MusicSheetModel> GetById(Guid id)
		{
			return await _db.Table<MusicSheetModel>().FirstOrDefaultAsync((MusicSheetModel x) => x.Id.Equals(id));
		}

		public async Task<IEnumerable<MusicSheetModel>> GetAll()
		{
			return await _db.Table<MusicSheetModel>().ToListAsync();
		}
	}
}
