using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Raids
{
	public class MentorProgressPopupPanel : Panel
	{
		private const int IconSize = 48;

		private const int Padding = 8;

		private readonly Image _icon;

		private readonly Label _bossNameLabel;

		private readonly Label _progressLabel;

		private readonly Label _deltaLabel;

		public MentorProgressPopupPanel(string bossName, int current, int max, int delta, int iconAssetId)
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Expected O, but got Unknown
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Expected O, but got Unknown
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Expected O, but got Unknown
			((Control)this).set_Size(new Point(300, 72));
			((Panel)this).set_BackgroundTexture(Service.Textures?.DatAsset(156112));
			((Panel)this).set_ShowBorder(false);
			((Panel)this).set_ShowTint(false);
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Size(new Point(48, 48));
			((Control)val).set_Location(new Point(8, (((Control)this).get_Height() - 48) / 2));
			val.set_Texture((AsyncTexture2D)((iconAssetId > 0 && Service.Textures != null) ? ((object)Service.Textures!.DatAsset(iconAssetId)) : ((object)AsyncTexture2D.op_Implicit(Textures.get_Pixel()))));
			_icon = val;
			int contentLeft = 64;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(contentLeft, 8));
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			val2.set_Font(Control.get_Content().get_DefaultFont16());
			val2.set_TextColor(Color.get_LightGoldenrodYellow());
			val2.set_Text(bossName);
			_bossNameLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(contentLeft, ((Control)_bossNameLabel).get_Bottom() + 2));
			val3.set_AutoSizeWidth(true);
			val3.set_Font(Control.get_Content().get_DefaultFont18());
			val3.set_TextColor(new Color(218, 165, 32));
			val3.set_Text($"+{delta}");
			_deltaLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(((Control)_deltaLabel).get_Right() + 4, ((Control)_bossNameLabel).get_Bottom() + 4));
			val4.set_AutoSizeWidth(true);
			val4.set_Font(Control.get_Content().get_DefaultFont14());
			val4.set_TextColor(Color.get_LightGoldenrodYellow());
			val4.set_Text($"{current} / {max}");
			_progressLabel = val4;
			((Control)this).add_Click((EventHandler<MouseEventArgs>)OnAnyClick);
			AutoCloseAfterDelayAsync();
		}

		private void OnAnyClick(object sender, MouseEventArgs e)
		{
			((Control)this).Dispose();
		}

		private async Task AutoCloseAfterDelayAsync()
		{
			try
			{
				await Task.Delay(TimeSpan.FromSeconds(5.0));
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
				{
					try
					{
						((Control)this).Dispose();
					}
					catch (Exception)
					{
					}
				});
			}
			catch
			{
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)OnAnyClick);
			((Panel)this).DisposeControl();
		}
	}
}
