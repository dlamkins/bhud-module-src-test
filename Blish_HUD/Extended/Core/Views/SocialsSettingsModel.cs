using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Blish_HUD.Extended.Core.Views
{
	public class SocialsSettingsModel
	{
		public enum SocialType
		{
			[EnumMember(Value = "kofi")]
			KoFi,
			[EnumMember(Value = "discord")]
			Discord,
			[EnumMember(Value = "github")]
			GitHub,
			[EnumMember(Value = "instagram")]
			Instagram,
			[EnumMember(Value = "patreon")]
			Patreon,
			[EnumMember(Value = "twitch")]
			Twitch,
			[EnumMember(Value = "twitter")]
			Twitter,
			[EnumMember(Value = "youtube")]
			YouTube
		}

		private sealed class Social
		{
			[JsonProperty("url")]
			public string Url;

			[JsonProperty("title")]
			public string Title;
		}

		private static readonly IReadOnlyDictionary<SocialType, Texture2D> _socialLogos;

		private IReadOnlyDictionary<SocialType, string> _socialUrls;

		private IReadOnlyDictionary<SocialType, string> _socialTexts;

		private int _timeOutSeconds;

		internal string SocialManifestUrl { get; }

		internal SettingCollection Settings { get; }

		static SocialsSettingsModel()
		{
			Dictionary<SocialType, Texture2D> socialLogos = new Dictionary<SocialType, Texture2D>();
			Assembly assembly = typeof(SocialsSettingsModel).GetTypeInfo().Assembly;
			IEnumerable<SocialType> enumerable = Enum.GetValues(typeof(SocialType)).Cast<SocialType>();
			string[] resourceNames = assembly.GetManifestResourceNames();
			foreach (SocialType social in enumerable)
			{
				string resource = resourceNames.FirstOrDefault((string x) => x.EndsWith(social.ToString().ToLowerInvariant() + "_logo.png"));
				if (resource != null)
				{
					using Stream file = assembly.GetManifestResourceStream(resource);
					socialLogos.Add(social, Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), file));
				}
			}
			_socialLogos = socialLogos;
		}

		private SocialsSettingsModel(SettingCollection settings)
		{
			Settings = settings;
		}

		public SocialsSettingsModel(SettingCollection settings, Dictionary<SocialType, string> urls, Dictionary<SocialType, string> texts)
			: this(settings)
		{
			_socialUrls = urls;
			_socialTexts = texts;
		}

		public SocialsSettingsModel(SettingCollection settings, string remoteSocialManifestUrl, int timeOutSeconds = 3)
			: this(settings)
		{
			SocialManifestUrl = remoteSocialManifestUrl;
			_timeOutSeconds = timeOutSeconds;
		}

		internal async Task<bool> LoadSocials()
		{
			if (string.IsNullOrEmpty(SocialManifestUrl))
			{
				return true;
			}
			if (_socialUrls != null && _socialTexts != null)
			{
				return true;
			}
			var (success, socials) = await TaskUtil.GetJsonResponse<Dictionary<SocialType, Social>>(SocialManifestUrl, _timeOutSeconds);
			if (!success)
			{
				return true;
			}
			_socialTexts = socials.ToDictionary((KeyValuePair<SocialType, Social> x) => x.Key, (KeyValuePair<SocialType, Social> x) => x.Value.Title);
			_socialUrls = socials.ToDictionary((KeyValuePair<SocialType, Social> x) => x.Key, (KeyValuePair<SocialType, Social> x) => x.Value.Url);
			return true;
		}

		internal Texture2D GetSocialLogo(SocialType social)
		{
			return _socialLogos[social];
		}

		internal string GetSocialUrl(SocialType social)
		{
			string val = default(string);
			if (!(_socialUrls?.TryGetValue(social, out val) ?? false))
			{
				return string.Empty;
			}
			return val;
		}

		internal string GetSocialText(SocialType social)
		{
			string val = default(string);
			if (!(_socialTexts?.TryGetValue(social, out val) ?? false))
			{
				return string.Empty;
			}
			return val;
		}

		internal IEnumerable<SocialType> GetSocials()
		{
			return _socialUrls?.Keys ?? Enumerable.Empty<SocialType>();
		}
	}
}
