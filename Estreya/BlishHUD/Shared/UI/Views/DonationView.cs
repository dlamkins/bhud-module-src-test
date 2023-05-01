using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public class DonationView : BaseView
	{
		private IFlurlClient _flurlClient;

		private Texture2D _kofiLogo;

		public DonationView(IFlurlClient flurlClient, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, BitmapFont font = null)
			: base(apiManager, iconService, translationService, font)
		{
			_flurlClient = flurlClient;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_FlowDirection((ControlFlowDirection)3);
			Rectangle contentRegion = ((Container)parent).get_ContentRegion();
			((Control)val).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			((Panel)val).set_CanScroll(true);
			FlowPanel sectionsPanel = val;
			BuildDonationSection(sectionsPanel);
		}

		private void BuildDonationSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			Panel sectionPanel = val;
			FormattedLabel label = new FormattedLabelBuilder().SetWidth(((Container)sectionPanel).get_ContentRegion().Width - 50).AutoSizeHeight().Wrap()
				.CreatePart("You enjoy my work on these modules and want to support it? Feels free to choose a donation method you like.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetFontSize((FontSize)20);
				})
				.CreatePart("Donations are always optional and never expected to use my modules!", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetFontSize((FontSize)16);
					builder.MakeItalic();
				})
				.Build();
			((Control)label).set_Parent((Container)(object)sectionPanel);
			((Control)label).set_Location(new Point(30, 30));
			Button button = RenderButton(sectionPanel, "Ko-fi", delegate
			{
				Process.Start("https://ko-fi.com/estreya");
			});
			((Control)button).set_Left(((Control)label).get_Left());
			((Control)button).set_Top(((Control)label).get_Bottom() + 20);
			button.Icon = AsyncTexture2D.op_Implicit(_kofiLogo);
			((Control)button).set_Height(48);
			((Control)button).set_Width(150);
			button.Font = GameService.Content.get_DefaultFont18();
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			try
			{
				Bitmap bitmap = ImageUtil.ResizeImage(Image.FromStream(await _flurlClient.Request("https://storage.ko-fi.com/cdn/nav-logo-stroke.png").GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0)), 48, 32);
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
							_kofiLogo = Texture2D.FromStream(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), (Stream)memoryStream);
						}
						finally
						{
							((GraphicsDeviceContext)(ref val)).Dispose();
						}
					});
					return true;
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
				return false;
			}
		}
	}
}
