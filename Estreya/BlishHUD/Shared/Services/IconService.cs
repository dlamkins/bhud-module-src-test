using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Services
{
	public class IconService : ManagedService
	{
		public enum IconSource
		{
			Core,
			Module,
			RenderAPI,
			DAT,
			Wiki,
			Unknown
		}

		public const string RENDER_API_URL = "https://render.guildwars2.com/file/";

		public const string WIKI_URL = "https://wiki.guildwars2.com/images/";

		private static readonly WebClient _webclient = new WebClient();

		private static readonly Regex _regexRenderServiceSignatureFileIdPair = new Regex("(.{40})\\/(\\d+)(?>\\..*)?$", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex _regexWiki = new Regex("(\\d{1}\\/\\d{1}\\w{1}\\/.+\\.png)", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex _regexDat = new Regex("(\\d+)\\.png", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex _regexModuleRef = new Regex("(.+\\.png)", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex _regexCore = new Regex("(\\d+)", RegexOptions.Compiled | RegexOptions.Singleline);

		private readonly ContentsManager _contentsManager;

		public IconService(ServiceConfiguration configuration, ContentsManager contentsManager)
			: base(configuration)
		{
			_contentsManager = contentsManager;
		}

		protected override Task Initialize()
		{
			return Task.CompletedTask;
		}

		protected override void InternalUnload()
		{
		}

		protected override void InternalUpdate(GameTime gameTime)
		{
		}

		protected override Task Load()
		{
			return Task.CompletedTask;
		}

		public AsyncTexture2D GetIcon(string identifier)
		{
			List<IconSource> iconSources = DetermineIconSources(identifier);
			return GetIcon(identifier, iconSources);
		}

		private AsyncTexture2D GetIcon(string identifier, List<IconSource> iconSources)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			if (iconSources == null || iconSources.Count == 0)
			{
				return AsyncTexture2D.op_Implicit(Textures.get_Error());
			}
			AsyncTexture2D icon = new AsyncTexture2D();
			foreach (IconSource source in iconSources)
			{
				string sourceIdentifier = ParseIdentifierBySource(identifier, source);
				if (string.IsNullOrWhiteSpace(sourceIdentifier))
				{
					Logger.Warn($"Can't load texture by {identifier} with source {source}");
					continue;
				}
				try
				{
					switch (source)
					{
					case IconSource.Core:
					{
						Texture2D coreTexture = GameService.Content.GetTexture(sourceIdentifier);
						if (coreTexture != Textures.get_Error())
						{
							icon.SwapTexture(coreTexture);
						}
						break;
					}
					case IconSource.Module:
					{
						Texture2D moduleTexture = _contentsManager.GetTexture(sourceIdentifier);
						if (moduleTexture != Textures.get_Error())
						{
							icon.SwapTexture(moduleTexture);
						}
						break;
					}
					case IconSource.RenderAPI:
						icon = GameService.Content.GetRenderServiceTexture(sourceIdentifier);
						break;
					case IconSource.DAT:
						icon = AsyncTexture2D.FromAssetId(Convert.ToInt32(sourceIdentifier));
						break;
					case IconSource.Wiki:
					{
						string wikiUrl = ((!identifier.StartsWith("https://wiki.guildwars2.com/images/")) ? ("https://wiki.guildwars2.com/images/" + sourceIdentifier) : sourceIdentifier);
						icon = AsyncTexture2D.op_Implicit(TextureUtil.FromStreamPremultiplied((Stream)new MemoryStream(_webclient.DownloadData(wikiUrl))));
						break;
					}
					case IconSource.Unknown:
						break;
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Could not load icon {0}:", new object[1] { identifier });
				}
				if (icon == null)
				{
					continue;
				}
				break;
			}
			if (icon == null)
			{
				return AsyncTexture2D.op_Implicit(Textures.get_Error());
			}
			return icon;
		}

		public Task<AsyncTexture2D> GetIconAsync(string identifier, CancellationToken cancellationToken)
		{
			return Task.Run(() => GetIcon(identifier), cancellationToken);
		}

		private string ParseIdentifierBySource(string identifier, IconSource source)
		{
			switch (source)
			{
			case IconSource.Core:
				return _regexCore.Match(identifier).Groups[1].Value;
			case IconSource.Module:
				return _regexModuleRef.Match(identifier).Groups[1].Value;
			case IconSource.RenderAPI:
			{
				Match renderApiMatch = _regexRenderServiceSignatureFileIdPair.Match(identifier);
				return renderApiMatch.Groups[1].Value + "/" + renderApiMatch.Groups[2].Value;
			}
			case IconSource.DAT:
				return _regexDat.Match(identifier).Groups[1].Value;
			case IconSource.Wiki:
				return _regexWiki.Match(identifier).Groups[1].Value;
			default:
				return null;
			}
		}

		private List<IconSource> DetermineIconSources(string identifier)
		{
			List<IconSource> iconSources = new List<IconSource>();
			if (string.IsNullOrEmpty(identifier))
			{
				return iconSources;
			}
			if (_regexRenderServiceSignatureFileIdPair.IsMatch(identifier))
			{
				iconSources.Add(IconSource.RenderAPI);
			}
			if (_regexWiki.IsMatch(identifier))
			{
				iconSources.Add(IconSource.Wiki);
			}
			if (_regexDat.IsMatch(identifier))
			{
				iconSources.Add(IconSource.DAT);
			}
			if (_regexModuleRef.IsMatch(identifier))
			{
				iconSources.Add(IconSource.Module);
			}
			if (_regexCore.IsMatch(identifier))
			{
				iconSources.Add(IconSource.Core);
			}
			if (iconSources.Count == 0)
			{
				iconSources.Add(IconSource.Unknown);
			}
			return iconSources;
		}
	}
}
