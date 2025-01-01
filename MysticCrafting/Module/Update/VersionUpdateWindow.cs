using System;
using System.IO;
using System.Reflection;
using System.Text;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;
using SemVer;

namespace MysticCrafting.Module.Update
{
	internal class VersionUpdateWindow : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<VersionUpdateWindow>();

		private readonly Texture2D _windowTexture = AsyncTexture2D.op_Implicit(ServiceContainer.TextureRepository.GetRefTexture("updateview.png"));

		private readonly Texture2D _heroReleaseTexture = AsyncTexture2D.op_Implicit(ServiceContainer.TextureRepository.GetRefTexture("versionupdatehero.png"));

		private readonly Texture2D _titleBannerTexture = AsyncTexture2D.op_Implicit(ServiceContainer.TextureRepository.Textures.TitleBanner);

		private readonly FlowPanel _changePanel;

		private readonly StandardButton _bttnDismiss;

		private readonly GlowButton _bttnHide;

		private readonly LoadingSpinner _loadingSpinner;

		private readonly Label _lblProgressMessage;

		private Manifest _manifest;

		private string GetVersionChangelog()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string resourceName = "MysticCrafting.Module.changelog.txt";
			using Stream stream = executingAssembly.GetManifestResourceStream(resourceName);
			using StreamReader reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}

		public VersionUpdateWindow(Manifest manifest)
			: this()
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Expected O, but got Unknown
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Expected O, but got Unknown
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Expected O, but got Unknown
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Expected O, but got Unknown
			_manifest = manifest;
			Rectangle bounds = _windowTexture.get_Bounds();
			((Control)this).set_Size(((Rectangle)(ref bounds)).get_Size());
			((Container)this).set_ContentRegion(new Rectangle(187, 35, 632, 824));
			((Control)this).set_Visible(false);
			Label val = new Label();
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Height(82);
			((Control)val).set_Top(25);
			val.set_Text($"Mystic Crafting - Version {manifest.get_Version()}");
			val.set_Font(GameService.Content.get_DefaultFont32());
			val.set_StrokeText(true);
			val.set_VerticalAlignment((VerticalAlignment)1);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val).set_Parent((Container)(object)this);
			Label val2 = new Label();
			((Control)val2).set_Top(390);
			((Control)val2).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)val2).set_Height(82);
			val2.set_TextColor(Colors.Chardonnay);
			val2.set_Text("Changelog");
			val2.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0));
			val2.set_StrokeText(true);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val2).set_Parent((Container)(object)this);
			Label subLabel = val2;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Top(((Control)subLabel).get_Bottom());
			((Control)val3).set_Height(290);
			((Control)val3).set_Width(474);
			((Control)val3).set_Left(((Container)this).get_ContentRegion().Width / 2 - 237);
			val3.set_ControlPadding(new Vector2(10f, 2f));
			((Control)val3).set_Padding(new Thickness(3f));
			((Control)val3).set_Parent((Container)(object)this);
			((Panel)val3).set_ShowBorder(true);
			((Panel)val3).set_CanScroll(true);
			_changePanel = val3;
			StringBuilder bodyBuffer = new StringBuilder();
			string[] array = GetVersionChangelog().Split('\n');
			foreach (string line in array)
			{
				if (line.StartsWith("#"))
				{
					CreateBodyLabel(bodyBuffer);
					CreateTitleLabel(line.TrimStart('#', ' '));
				}
				else
				{
					bodyBuffer.AppendLine(line);
				}
				void CreateBodyLabel(StringBuilder text)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_0046: Unknown result type (might be due to invalid IL or missing references)
					if (!string.IsNullOrEmpty(text.ToString()))
					{
						Label val8 = new Label();
						val8.set_AutoSizeHeight(true);
						((Control)val8).set_Width(((Control)_changePanel).get_Width() - 30);
						val8.set_WrapText(true);
						val8.set_Text($"\n{text}");
						val8.set_Font(GameService.Content.get_DefaultFont16());
						((Control)val8).set_Parent((Container)(object)_changePanel);
						text.Clear();
					}
				}
				void CreateTitleLabel(string text)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					//IL_0005: Unknown result type (might be due to invalid IL or missing references)
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					//IL_0014: Unknown result type (might be due to invalid IL or missing references)
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					Label val7 = new Label();
					val7.set_AutoSizeHeight(true);
					val7.set_AutoSizeWidth(true);
					val7.set_TextColor(Colors.Chardonnay);
					val7.set_Text(text ?? "");
					val7.set_Font(GameService.Content.get_DefaultFont18());
					((Control)val7).set_Parent((Container)(object)_changePanel);
				}
			}
			CreateBodyLabel(bodyBuffer);
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Top(((Control)_changePanel).get_Bottom() + 32);
			((Control)val4).set_Width(128);
			val4.set_Text(Common.Dismiss);
			((Control)val4).set_Left(((Container)this).get_ContentRegion().Width / 2 - 64);
			((Control)val4).set_Parent((Container)(object)this);
			_bttnDismiss = val4;
			LoadingSpinner val5 = new LoadingSpinner();
			((Control)val5).set_Location(new Point(((Container)this).get_ContentRegion().Width / 2 - 32, ((Control)_changePanel).get_Bottom() - ((Control)_changePanel).get_Height() / 2 - 70));
			((Control)val5).set_Visible(false);
			((Control)val5).set_Parent((Container)(object)this);
			_loadingSpinner = val5;
			Label val6 = new Label();
			val6.set_AutoSizeHeight(true);
			((Control)val6).set_Width(((Control)_changePanel).get_Width());
			((Control)val6).set_Left(((Container)this).get_ContentRegion().Width / 2 - ((Control)_changePanel).get_Width() / 2);
			((Control)val6).set_Top(((Control)_loadingSpinner).get_Bottom() + 6);
			val6.set_HorizontalAlignment((HorizontalAlignment)1);
			val6.set_VerticalAlignment((VerticalAlignment)0);
			((Control)val6).set_Visible(false);
			val6.set_WrapText(false);
			((Control)val6).set_Parent((Container)(object)this);
			_lblProgressMessage = val6;
			((Control)_bttnDismiss).add_Click((EventHandler<MouseEventArgs>)DismissClick);
		}

		public void AcknowledgeUpdate(Version version)
		{
			MysticCraftingModule.Settings.LastAcknowledgedUpdate.set_Value(version);
		}

		private async void DismissClick(object sender, MouseEventArgs e)
		{
			AcknowledgeUpdate(_manifest.get_Version());
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<VersionUpdateWindow>(this, (object)new
			{
				Opacity = 0
			}, 0.5f, 0f, true);
			((Control)this).Hide();
		}

		private void HideClick(object sender, MouseEventArgs e)
		{
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<VersionUpdateWindow>(this, (object)new
			{
				Opacity = 0
			}, 0.5f, 0f, true);
		}

		private void SetDisplayMode(bool isPending, bool showChanges)
		{
			((Control)_changePanel).set_Visible(showChanges);
			((Control)_bttnDismiss).set_Visible(isPending);
			((Control)_lblProgressMessage).set_Visible(!showChanges);
			((Control)_loadingSpinner).set_Visible(!isPending);
			((Control)_lblProgressMessage).set_Top(((Control)_loadingSpinner).get_Visible() ? (((Control)_loadingSpinner).get_Bottom() + 6) : (((Control)_loadingSpinner).get_Top() + ((Control)_loadingSpinner).get_Height() / 2));
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)GameService.Overlay.get_BlishHudWindow()).Hide();
			((Control)this).set_Opacity(0f);
			GameService.Content.PlaySoundEffectByName("hero-open");
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<VersionUpdateWindow>(this, (object)new
			{
				Opacity = 1
			}, 1f, 0f, true);
			((Control)this).set_ZIndex(9999);
			((Control)this).OnShown(e);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - 512, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2 - 512));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _windowTexture, bounds);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _titleBannerTexture, RectangleExtension.OffsetBy(_titleBannerTexture.get_Bounds(), 0, 0));
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _heroReleaseTexture, RectangleExtension.OffsetBy(_heroReleaseTexture.get_Bounds(), 112, 157));
		}

		protected override void DisposeControl()
		{
			((Control)this).set_Visible(false);
			((Container)this).DisposeControl();
		}
	}
}
