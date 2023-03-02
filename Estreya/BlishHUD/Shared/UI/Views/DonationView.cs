using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.State;
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

		public DonationView(IFlurlClient flurlClient, Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, BitmapFont font = null)
			: base(apiManager, iconState, translationState, font)
		{
			_flurlClient = flurlClient;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected O, but got Unknown
			FormattedLabel label = new FormattedLabelBuilder().SetWidth(((Container)parent).get_ContentRegion().Width - 50).AutoSizeHeight().Wrap()
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
			((Control)label).set_Parent((Container)(object)parent);
			((Control)label).set_Location(new Point(30, 30));
			StandardButton val = new StandardButton();
			((Control)val).set_Left(((Control)label).get_Left());
			((Control)val).set_Top(((Control)label).get_Bottom() + 20);
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Icon(AsyncTexture2D.op_Implicit(_kofiLogo));
			val.set_Text("Ko-fi");
			((Control)val).set_Height(48);
			((Control)val).set_Width(150);
			StandardButton kofiSupport = val;
			((object)kofiSupport).GetType().GetField("_font", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(kofiSupport, GameService.Content.get_DefaultFont18());
			((Control)kofiSupport).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					Process.Start("https://ko-fi.com/estreya");
				}
				catch (Exception)
				{
				}
			});
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			try
			{
				Bitmap bitmap = ResizeImage(Image.FromStream(await _flurlClient.Request("https://storage.ko-fi.com/cdn/nav-logo-stroke.png").GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0)), 48, 32);
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

		private static Bitmap ResizeImage(Image image, int width, int height)
		{
			Rectangle destRect = new Rectangle(0, 0, width, height);
			Bitmap destImage = new Bitmap(width, height);
			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			using Graphics graphics = Graphics.FromImage(destImage);
			graphics.CompositingMode = CompositingMode.SourceCopy;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			using ImageAttributes wrapMode = new ImageAttributes();
			wrapMode.SetWrapMode(WrapMode.TileFlipXY);
			graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
			return destImage;
		}
	}
}
