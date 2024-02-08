using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Audio;

namespace MysticCrafting.Module.Services
{
	public class AudioService : IAudioService
	{
		private SoundEffect _menuItemClickSfx;

		private IReadOnlyList<SoundEffect> _menuClicks;

		private readonly ContentsManager _contentsManager;

		public AudioService(ContentsManager contentsManager)
		{
			_contentsManager = contentsManager;
		}

		public void Load()
		{
			if (_contentsManager == null)
			{
				throw new NullReferenceException("Content Manager was not initialized correctly");
			}
			_menuItemClickSfx = _contentsManager.GetSound("audio\\menu-item-click.wav");
			_menuClicks = new List<SoundEffect>
			{
				_contentsManager.GetSound("audio\\menu-click-1.wav"),
				_contentsManager.GetSound("audio\\menu-click-2.wav"),
				_contentsManager.GetSound("audio\\menu-click-3.wav"),
				_contentsManager.GetSound("audio\\menu-click-4.wav")
			};
		}

		public void PlayMenuItemClick()
		{
			_menuItemClickSfx.Play(GameService.GameIntegration.Audio.Volume, 0f, 0f);
		}

		public void PlayMenuClick()
		{
			_menuClicks[RandomUtil.GetRandom(0, 3)].Play(GameService.GameIntegration.Audio.Volume, 0f, 0f);
		}
	}
}
