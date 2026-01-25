using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RaidClears.Features.Raids.Models;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Strikes.Models;
using RaidClears.Localization;
using RaidClears.Utils.Kenedia;

namespace RaidClears.Features.Raids
{
	public class RaidTooltipView : Tooltip
	{
		private readonly Label _title;

		private readonly Label _id;

		private readonly Image _icon;

		private readonly Image _powerIcon;

		private readonly Label _powerLabel;

		private readonly Image _condiIcon;

		private readonly Label _condiLabel;

		private readonly Image _defianceIcon;

		private readonly Label _defianceLabel;

		private RaidEncounter _encounter = new RaidEncounter();

		private StrikeMission _strikeMission = new StrikeMission();

		public RaidEncounter Encoutner
		{
			get
			{
				return _encounter;
			}
			set
			{
				Common.SetProperty(ref _encounter, value, new ValueChangedEventHandler<RaidEncounter>(ApplyEncounter));
			}
		}

		public StrikeMission StrikeMission
		{
			get
			{
				return _strikeMission;
			}
			set
			{
				Common.SetProperty(ref _strikeMission, value, new ValueChangedEventHandler<StrikeMission>(ApplyStrikeMission));
			}
		}

		public RaidTooltipView()
			: this()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Expected O, but got Unknown
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Expected O, but got Unknown
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Expected O, but got Unknown
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Expected O, but got Unknown
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Expected O, but got Unknown
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Expected O, but got Unknown
			Image val = new Image();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Height(48);
			((Control)val).set_Width(48);
			val.set_Texture(AsyncTexture2D.op_Implicit(Textures.get_Pixel()));
			Point location = new Point
			{
				X = 0,
				Y = 0
			};
			((Control)val).set_Location(location);
			_icon = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Height(Control.get_Content().get_DefaultFont16().get_LineHeight());
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(((Control)_icon).get_Right() + 5, ((Control)_icon).get_Top() + 5));
			val2.set_Font(Control.get_Content().get_DefaultFont16());
			val2.set_TextColor(Colors.Chardonnay);
			_title = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Height(Control.get_Content().get_DefaultFont12().get_LineHeight());
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Location(new Point(((Control)_title).get_Left(), ((Control)_title).get_Bottom()));
			val3.set_Font(Control.get_Content().get_DefaultFont12());
			val3.set_TextColor(Color.get_White() * 0.8f);
			_id = val3;
			Image val4 = new Image();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Height(20);
			((Control)val4).set_Width(20);
			val4.set_Texture(AsyncTexture2D.op_Implicit(Textures.get_Pixel()));
			location = new Point
			{
				X = 0,
				Y = 0
			};
			((Control)val4).set_Location(location);
			((Control)val4).set_Visible(false);
			_powerIcon = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Height(Control.get_Content().get_DefaultFont12().get_LineHeight());
			val5.set_AutoSizeWidth(true);
			location = new Point
			{
				X = 0,
				Y = 0
			};
			((Control)val5).set_Location(location);
			val5.set_Font(Control.get_Content().get_DefaultFont12());
			val5.set_TextColor(Color.get_White() * 0.9f);
			val5.set_Text(Strings.Tooltip_PowerDamage);
			((Control)val5).set_Visible(false);
			_powerLabel = val5;
			Image val6 = new Image();
			((Control)val6).set_Parent((Container)(object)this);
			((Control)val6).set_Height(20);
			((Control)val6).set_Width(20);
			val6.set_Texture(AsyncTexture2D.op_Implicit(Textures.get_Pixel()));
			location = new Point
			{
				X = 0,
				Y = 0
			};
			((Control)val6).set_Location(location);
			((Control)val6).set_Visible(false);
			_condiIcon = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)this);
			((Control)val7).set_Height(Control.get_Content().get_DefaultFont12().get_LineHeight());
			val7.set_AutoSizeWidth(true);
			location = new Point
			{
				X = 0,
				Y = 0
			};
			((Control)val7).set_Location(location);
			val7.set_Font(Control.get_Content().get_DefaultFont12());
			val7.set_TextColor(Color.get_White() * 0.9f);
			val7.set_Text(Strings.Tooltip_ConditionDamage);
			((Control)val7).set_Visible(false);
			_condiLabel = val7;
			Image val8 = new Image();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Height(20);
			((Control)val8).set_Width(20);
			val8.set_Texture(AsyncTexture2D.op_Implicit(Textures.get_Pixel()));
			location = new Point
			{
				X = 0,
				Y = 0
			};
			((Control)val8).set_Location(location);
			((Control)val8).set_Visible(false);
			_defianceIcon = val8;
			Label val9 = new Label();
			((Control)val9).set_Parent((Container)(object)this);
			((Control)val9).set_Height(Control.get_Content().get_DefaultFont12().get_LineHeight());
			val9.set_AutoSizeWidth(true);
			location = new Point
			{
				X = 0,
				Y = 0
			};
			((Control)val9).set_Location(location);
			val9.set_Font(Control.get_Content().get_DefaultFont12());
			val9.set_TextColor(new Color(57, 172, 161));
			val9.set_Text(Strings.Tooltip_DefianceBreak);
			((Control)val9).set_Visible(false);
			_defianceLabel = val9;
		}

		private void ApplyEncounter(object sender, ValueChangedEventArgs<RaidEncounter> e)
		{
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			if (e.NewValue == null)
			{
				return;
			}
			_title.set_Text(e.NewValue!.Name ?? "");
			_id.set_Text("(" + Service.RaidSettings.GetEncounterLabel(e.NewValue!.ApiId) + ")");
			if (e.NewValue!.AssetId > 0)
			{
				_icon.set_Texture(Service.Textures!.DatAsset(e.NewValue!.AssetId));
			}
			RaidData raidData = Service.RaidData;
			int yOffset = ((Control)_icon).get_Bottom() + 5;
			int xOffset = ((Control)_icon).get_Left();
			if (raidData != null)
			{
				if (e.NewValue!.PowerFavored)
				{
					_powerIcon.set_Texture(Service.Textures!.DatAsset(raidData.PowerDamageAssetId));
					((Control)_powerIcon).set_Location(new Point(xOffset, yOffset));
					((Control)_powerIcon).set_Visible(true);
					((Control)_powerLabel).set_Location(new Point(((Control)_powerIcon).get_Right() + 5, yOffset));
					((Control)_powerLabel).set_Visible(true);
					yOffset += 25;
				}
				else
				{
					((Control)_powerIcon).set_Visible(false);
					((Control)_powerLabel).set_Visible(false);
				}
				if (e.NewValue!.CondiFavored)
				{
					_condiIcon.set_Texture(Service.Textures!.DatAsset(raidData.CondiDamageAssetId));
					((Control)_condiIcon).set_Location(new Point(xOffset, yOffset));
					((Control)_condiIcon).set_Visible(true);
					((Control)_condiLabel).set_Location(new Point(((Control)_condiIcon).get_Right() + 5, yOffset));
					((Control)_condiLabel).set_Visible(true);
					yOffset += 25;
				}
				else
				{
					((Control)_condiIcon).set_Visible(false);
					((Control)_condiLabel).set_Visible(false);
				}
				if (e.NewValue!.NeedsDefianceBreak && raidData.DefianceAssetId > 0)
				{
					_defianceIcon.set_Texture(Service.Textures!.DatAsset(raidData.DefianceAssetId));
					((Control)_defianceIcon).set_Location(new Point(xOffset, yOffset));
					((Control)_defianceIcon).set_Visible(true);
					((Control)_defianceLabel).set_Location(new Point(((Control)_defianceIcon).get_Right() + 5, yOffset));
					((Control)_defianceLabel).set_Visible(true);
				}
				else
				{
					((Control)_defianceIcon).set_Visible(false);
					((Control)_defianceLabel).set_Visible(false);
				}
			}
			else
			{
				((Control)_powerIcon).set_Visible(false);
				((Control)_powerLabel).set_Visible(false);
				((Control)_condiIcon).set_Visible(false);
				((Control)_condiLabel).set_Visible(false);
				((Control)_defianceIcon).set_Visible(false);
				((Control)_defianceLabel).set_Visible(false);
			}
			((Control)this).Invalidate();
		}

		private void ApplyStrikeMission(object sender, ValueChangedEventArgs<StrikeMission> e)
		{
			if (e.NewValue != null)
			{
				_title.set_Text(e.NewValue!.Name ?? "");
				_id.set_Text("(" + Service.StrikeSettings.GetEncounterLabel(e.NewValue!.Id) + ")");
				if (e.NewValue!.AssetId > 0)
				{
					_icon.set_Texture(Service.Textures!.DatAsset(e.NewValue!.AssetId));
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Draw(spriteBatch, drawBounds, scissor);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((Tooltip)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		public override void RecalculateLayout()
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			int height = 58;
			int contentHeight = 48;
			int indicatorCount = 0;
			if (_powerIcon != null && ((Control)_powerIcon).get_Visible())
			{
				indicatorCount++;
			}
			if (_condiIcon != null && ((Control)_condiIcon).get_Visible())
			{
				indicatorCount++;
			}
			if (_defianceIcon != null && ((Control)_defianceIcon).get_Visible())
			{
				indicatorCount++;
			}
			if (indicatorCount > 0)
			{
				height += indicatorCount * 25;
				contentHeight += indicatorCount * 25;
			}
			((Control)this).set_Size(new Point(230, height));
			((Container)this).set_ContentRegion(new Rectangle(5, 5, 220, contentHeight));
		}

		protected override void DisposeControl()
		{
			((Tooltip)this).DisposeControl();
		}
	}
}
