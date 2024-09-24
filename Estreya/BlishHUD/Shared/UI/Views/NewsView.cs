using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public class NewsView : BaseView
	{
		private const string ESTREYA_DISCORD_INVITE = "https://discord.gg/8Yb3jdca3r";

		private const string BLISH_HUD_DISCORD_INVITE = "https://discord.gg/nGbd3kU";

		private const int DISCORD_SECTION_HEIGHT = 200;

		private static readonly Point _importantIconSize = new Point(32, 32);

		private readonly IFlurlClient _flurlClient;

		private Texture2D _discordLogo;

		private NewsService _newsService;

		public NewsView(IFlurlClient flurlClient, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, NewsService newsService)
			: base(apiManager, iconService, translationService)
		{
			_flurlClient = flurlClient;
			_newsService = newsService;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Expected O, but got Unknown
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Expected O, but got Unknown
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Location(new Point(25, 25));
			((Control)val).set_Height(((Container)parent).get_ContentRegion().Height - 200);
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - 50);
			((Panel)val).set_CanScroll(true);
			FlowPanel newsList = val;
			List<News> sortedNews = _newsService?.News?.OrderByDescending((News n) => n.Timestamp).ToList() ?? new List<News>();
			if (sortedNews.Count > 0)
			{
				foreach (News news in sortedNews)
				{
					RenderNews(newsList, news);
					RenderEmptyLine((Panel)(object)newsList);
				}
			}
			else
			{
				RenderNoNewsInfo(newsList);
			}
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Control)val2).set_Location(new Point(((Control)newsList).get_Left(), ((Control)newsList).get_Bottom()));
			((Control)val2).set_Width(((Control)newsList).get_Width());
			val2.set_ShowBorder(true);
			Panel discordSection = val2;
			((Control)discordSection).set_Height(((Container)parent).get_ContentRegion().Height - ((Control)discordSection).get_Top());
			Image val3 = new Image(AsyncTexture2D.op_Implicit(_discordLogo ?? Textures.get_TransparentPixel()));
			((Control)val3).set_Parent((Container)(object)discordSection);
			Image image = val3;
			((Control)image).set_Location(new Point(30, ((Control)discordSection).get_Height() / 2 - ((Control)image).get_Height() / 2 - 5));
			((Control)new FormattedLabelBuilder().SetWidth(((Container)discordSection).get_ContentRegion().Width).AutoSizeHeight().SetVerticalAlignment((VerticalAlignment)0)
				.SetHorizontalAlignment((HorizontalAlignment)1)
				.CreatePart("Join my Discord to stay up to date!", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetFontSize((FontSize)20).MakeUnderlined();
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("https://discord.gg/8Yb3jdca3r", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetHyperLink("https://discord.gg/8Yb3jdca3r");
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("BlishHUD:", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("\n ", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("https://discord.gg/nGbd3kU", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetHyperLink("https://discord.gg/nGbd3kU");
				})
				.Build()).set_Parent((Container)(object)discordSection);
		}

		private void RenderNoNewsInfo(FlowPanel newsList)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)newsList);
			Rectangle contentRegion = ((Container)newsList).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val.set_ShowBorder(false);
			Panel panel = val;
			FormattedLabel lbl = new FormattedLabelBuilder().SetWidth(((Container)panel).get_ContentRegion().Width).AutoSizeHeight().SetHorizontalAlignment((HorizontalAlignment)1)
				.CreatePart("There are no news.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.Build();
			((Control)lbl).set_Parent((Container)(object)panel);
			((Control)lbl).set_Top(((Control)panel).get_Height() / 2 - ((Control)lbl).get_Height() / 2);
			((Control)lbl).set_Left(((Control)panel).get_Width() / 2 - ((Control)lbl).get_Width() / 2);
		}

		private void RenderNews(FlowPanel newsList, News news)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)newsList);
			((Control)val).set_Width(((Container)newsList).get_ContentRegion().Width - 25);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel newsPanel = val;
			FontSize titleFontSize = (FontSize)20;
			BitmapFont font = GameService.Content.GetFont((FontFace)0, titleFontSize, (FontStyle)0);
			string title = news.Title;
			float titleWidth = font.MeasureString(title).Width;
			FontSize timeFontSize = (FontSize)16;
			BitmapFont timeFont = GameService.Content.GetFont((FontFace)0, timeFontSize, (FontStyle)0);
			FormattedLabelBuilder builder2 = new FormattedLabelBuilder();
			builder2.SetWidth(((Container)newsList).get_ContentRegion().Width).AutoSizeHeight().CreatePart("", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				if (news.Important)
				{
					builder.SetPrefixImage(base.IconService.GetIcon("222246.png")).SetPrefixImageSize(_importantIconSize);
				}
			})
				.CreatePart(title, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					builder.SetFontSize(titleFontSize).MakeUnderlined();
				})
				.CreatePart(GenerateTimestampAlignment(news, timeFont, ((Container)newsPanel).get_ContentRegion().Width - (int)titleWidth - 5 - (news.Important ? _importantIconSize.X : 0)), (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					builder.SetFontSize(timeFontSize);
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				});
			if (news.AsPoints)
			{
				string[] content2 = news.Content;
				foreach (string point in content2)
				{
					builder2.CreatePart("- " + point, (Action<FormattedLabelPartBuilder>)delegate
					{
					}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
					{
					});
				}
			}
			else
			{
				string content = string.Join("\n", news.Content);
				builder2.CreatePart(content, (Action<FormattedLabelPartBuilder>)delegate
				{
				}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				});
			}
			((Control)builder2.Build()).set_Parent((Container)(object)newsPanel);
		}

		private string GenerateTimestampAlignment(News news, BitmapFont font, int maxWidth)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			string text = string.Empty;
			int spaceCounter = 0;
			for (int width = 0; width < maxWidth; width = (int)font.MeasureString(text).Width)
			{
				text = "\t" + new string(' ', spaceCounter) + news.Timestamp.ToLocalTime().ToString();
				spaceCounter++;
			}
			return text;
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			await TryLoadDiscordLogo();
			return true;
		}

		private async Task TryLoadDiscordLogo()
		{
			try
			{
				Bitmap bitmap = ImageUtil.ResizeImage(Image.FromStream(await _flurlClient.Request("https://assets-global.website-files.com/6257adef93867e50d84d30e2/636e0b544a3e3c7c05753bcd_full_logo_white_RGB.png").GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0)), 200, 38);
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					bitmap.Save(memoryStream, ImageFormat.Png);
					await Task.Run(delegate
					{
						//IL_0005: Unknown result type (might be due to invalid IL or missing references)
						//IL_000a: Unknown result type (might be due to invalid IL or missing references)
						GraphicsDeviceContext val = GameService.Graphics.LendGraphicsDeviceContext();
						try
						{
							_discordLogo = Texture2D.FromStream(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), (Stream)memoryStream);
						}
						finally
						{
							((GraphicsDeviceContext)(ref val)).Dispose();
						}
					});
				}
				finally
				{
					if (memoryStream != null)
					{
						((IDisposable)memoryStream).Dispose();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		protected override void Unload()
		{
			base.Unload();
			_newsService = null;
			Texture2D discordLogo = _discordLogo;
			if (discordLogo != null)
			{
				((GraphicsResource)discordLogo).Dispose();
			}
			_discordLogo = null;
		}
	}
}
