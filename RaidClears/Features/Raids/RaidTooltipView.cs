using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RaidClears.Features.Raids.Services;
using RaidClears.Features.Shared.Models;
using RaidClears.Localization;
using RaidClears.Settings.Services;
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

		private readonly Image _mentorIcon;

		private readonly Label _mentorLabel;

		private BossEncounter _encounter = new BossEncounter();

		public BossEncounter Encounter
		{
			get
			{
				return _encounter;
			}
			set
			{
				Common.SetProperty(ref _encounter, value ?? new BossEncounter(), new ValueChangedEventHandler<BossEncounter>(ApplyEncounter));
			}
		}

		public RaidTooltipView()
			: this()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Expected O, but got Unknown
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Expected O, but got Unknown
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Expected O, but got Unknown
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Expected O, but got Unknown
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Expected O, but got Unknown
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Expected O, but got Unknown
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Expected O, but got Unknown
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Expected O, but got Unknown
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_045e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0495: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Expected O, but got Unknown
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
			Image val10 = new Image();
			((Control)val10).set_Parent((Container)(object)this);
			((Control)val10).set_Height(20);
			((Control)val10).set_Width(20);
			val10.set_Texture(AsyncTexture2D.op_Implicit(Textures.get_Pixel()));
			location = new Point
			{
				X = 0,
				Y = 0
			};
			((Control)val10).set_Location(location);
			((Control)val10).set_Visible(false);
			_mentorIcon = val10;
			Label val11 = new Label();
			((Control)val11).set_Parent((Container)(object)this);
			((Control)val11).set_Height(Control.get_Content().get_DefaultFont12().get_LineHeight());
			val11.set_AutoSizeWidth(true);
			location = new Point
			{
				X = 0,
				Y = 0
			};
			((Control)val11).set_Location(location);
			val11.set_Font(Control.get_Content().get_DefaultFont12());
			val11.set_TextColor(Color.get_White() * 0.85f);
			((Control)val11).set_Visible(false);
			_mentorLabel = val11;
		}

		private void ApplyEncounter(object sender, ValueChangedEventArgs<BossEncounter> e)
		{
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Unknown result type (might be due to invalid IL or missing references)
			if (e.NewValue == null)
			{
				return;
			}
			BossEncounter enc = e.NewValue;
			_title.set_Text(enc.Name);
			_id.set_Text(enc.IsStrike ? ("(" + Service.StrikeSettings.GetEncounterLabel(enc) + ")") : ("(" + Service.RaidSettings.GetEncounterLabel(enc) + ")"));
			if (enc.AssetId > 0)
			{
				_icon.set_Texture(Service.Textures!.DatAsset(enc.AssetId));
			}
			int yOffset = ((Control)_icon).get_Bottom() + 5;
			int xOffset = ((Control)_icon).get_Left();
			RaidData raidData = Service.RaidData;
			if (raidData != null)
			{
				if (enc.PowerFavored)
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
				if (enc.CondiFavored)
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
				if (enc.NeedsDefianceBreak && raidData.DefianceAssetId > 0)
				{
					_defianceIcon.set_Texture(Service.Textures!.DatAsset(raidData.DefianceAssetId));
					((Control)_defianceIcon).set_Location(new Point(xOffset, yOffset));
					((Control)_defianceIcon).set_Visible(true);
					((Control)_defianceLabel).set_Location(new Point(((Control)_defianceIcon).get_Right() + 5, yOffset));
					((Control)_defianceLabel).set_Visible(true);
					yOffset += 25;
				}
				else
				{
					((Control)_defianceIcon).set_Visible(false);
					((Control)_defianceLabel).set_Visible(false);
				}
				SettingService settings = Service.Settings;
				bool num = settings != null && (settings.RaidSettings?.RaidPanelMentorProgress?.get_Value()).GetValueOrDefault();
				bool hasOtherCallouts = ((Control)_powerIcon).get_Visible() || ((Control)_condiIcon).get_Visible() || ((Control)_defianceIcon).get_Visible();
				if (num)
				{
					int? mentorAchievementId = enc.MentorAchievementId;
					if (mentorAchievementId.HasValue)
					{
						int mentorId = mentorAchievementId.GetValueOrDefault();
						if (hasOtherCallouts)
						{
							yOffset += 12;
						}
						IReadOnlyDictionary<int, MentorAchievementProgressEntry> progress = Service.MentorAchievementProgress?.Progress;
						((Control)_mentorIcon).set_Visible(raidData.MentorAssetId > 0);
						if (((Control)_mentorIcon).get_Visible())
						{
							_mentorIcon.set_Texture(Service.Textures!.DatAsset(raidData.MentorAssetId));
							((Control)_mentorIcon).set_Location(new Point(xOffset, yOffset));
						}
						if (progress != null && progress.TryGetValue(mentorId, out var entry))
						{
							_mentorLabel.set_Text(entry.Done ? Strings.Tooltip_MentorDone : string.Format(Strings.Tooltip_MentorProgress, entry.Current, entry.Max));
							((Control)_mentorLabel).set_Location(((Control)_mentorIcon).get_Visible() ? new Point(((Control)_mentorIcon).get_Right() + 5, yOffset) : new Point(xOffset + 5, yOffset));
							((Control)_mentorLabel).set_Visible(true);
						}
						else
						{
							_mentorLabel.set_Text(string.Format(Strings.Tooltip_MentorProgress, 0, "?"));
							((Control)_mentorLabel).set_Location(((Control)_mentorIcon).get_Visible() ? new Point(((Control)_mentorIcon).get_Right() + 5, yOffset) : new Point(xOffset + 5, yOffset));
							((Control)_mentorLabel).set_Visible(true);
						}
						yOffset += 25;
						goto IL_04d5;
					}
				}
				((Control)_mentorIcon).set_Visible(false);
				((Control)_mentorLabel).set_Visible(false);
			}
			else
			{
				((Control)_powerIcon).set_Visible(false);
				((Control)_powerLabel).set_Visible(false);
				((Control)_condiIcon).set_Visible(false);
				((Control)_condiLabel).set_Visible(false);
				((Control)_defianceIcon).set_Visible(false);
				((Control)_defianceLabel).set_Visible(false);
				((Control)_mentorIcon).set_Visible(false);
				((Control)_mentorLabel).set_Visible(false);
			}
			goto IL_04d5;
			IL_04d5:
			((Control)this).Invalidate();
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
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
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
			if (_mentorIcon != null && ((Control)_mentorIcon).get_Visible())
			{
				if (indicatorCount > 0)
				{
					height += 12;
					contentHeight += 12;
				}
				height += 25;
				contentHeight += 25;
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
