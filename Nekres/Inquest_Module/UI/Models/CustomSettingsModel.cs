using System.Collections.Generic;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Inquest_Module.UI.Models
{
	public class CustomSettingsModel
	{
		public enum Social
		{
			KoFi
		}

		public string PolicyMacrosAndMacroUse = "https://help.guildwars2.com/hc/articles/360013762153-Policy-Macros-and-Macro-Use";

		private readonly Dictionary<Social, string> _socialUrls;

		private readonly Dictionary<Social, Texture2D> _socialLogos;

		public SettingCollection Settings { get; private set; }

		private ContentsManager ContentsManager => InquestModule.ModuleInstance.ContentsManager;

		public CustomSettingsModel(SettingCollection settings)
		{
			Settings = settings;
			_socialUrls = new Dictionary<Social, string> { 
			{
				Social.KoFi,
				"https://ko-fi.com/TypoTiger"
			} };
			_socialLogos = new Dictionary<Social, Texture2D> { 
			{
				Social.KoFi,
				ContentsManager.GetTexture("socials\\ko-fi-logo.png")
			} };
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