using System.Collections.Generic;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Stream_Out.UI.Models
{
	public class CustomSettingsModel
	{
		public enum Social
		{
			KoFi,
			Discord,
			GitHub,
			Instagram,
			Twitch,
			Twitter,
			YouTube
		}

		private readonly Dictionary<Social, string> _socialUrls;

		private readonly Dictionary<Social, Texture2D> _socialLogos;

		public SettingCollection Settings { get; }

		private ContentsManager ContentsManager => StreamOutModule.ModuleInstance.ContentsManager;

		public CustomSettingsModel(SettingCollection settings)
		{
			Settings = settings;
			_socialUrls = new Dictionary<Social, string>
			{
				{
					Social.KoFi,
					"https://ko-fi.com/TypoTiger"
				},
				{
					Social.Discord,
					"discord://https://discord.gg/Ch3vz4pDyV"
				},
				{
					Social.GitHub,
					"https://github.com/agaertner"
				},
				{
					Social.Instagram,
					"https://www.instagram.com/typo_tiger"
				},
				{
					Social.Twitch,
					"https://www.twitch.tv/TypoTiger"
				},
				{
					Social.Twitter,
					"https://twitter.com/TypoTiger"
				},
				{
					Social.YouTube,
					"https://www.youtube.com/channel/UCfCefS0ouN40thChtwi-IGg"
				}
			};
			_socialLogos = new Dictionary<Social, Texture2D>
			{
				{
					Social.KoFi,
					ContentsManager.GetTexture("socials\\ko-fi-logo.png")
				},
				{
					Social.Discord,
					ContentsManager.GetTexture("socials\\discord-logo.png")
				},
				{
					Social.GitHub,
					ContentsManager.GetTexture("socials\\github-logo.png")
				},
				{
					Social.Instagram,
					ContentsManager.GetTexture("socials\\instagram-logo.png")
				},
				{
					Social.Twitch,
					ContentsManager.GetTexture("socials\\twitch-logo.png")
				},
				{
					Social.Twitter,
					ContentsManager.GetTexture("socials\\twitter-logo.png")
				},
				{
					Social.YouTube,
					ContentsManager.GetTexture("socials\\youtube-logo.png")
				}
			};
		}

		public Texture2D GetSocialLogo(Social social)
		{
			return _socialLogos[social];
		}

		public string GetSocialUrl(Social social)
		{
			return _socialUrls[social];
		}
	}
}
