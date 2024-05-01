using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Ideka.CustomCombatText
{
	public class SkillTooltip : Container
	{
		private const int IconSize = 35;

		private static readonly Color IconBorderColor = new Color(211, 211, 211);

		private static readonly Color TitleColor = new Color(255, 204, 119);

		private static readonly Color FactTextColor = new Color(170, 170, 170);

		private static readonly TemplateParser.Syntax<TemplateParser.TemplatePreFrag> Syntax = new TemplateParser.Syntax<TemplateParser.TemplatePreFrag>
		{
			ColorTagOpenA = "<c=",
			ColorTagOpenB = '>',
			ColorTagClose = "</c>",
			KnownColors = new Dictionary<string, Color>
			{
				["@abilitytype"] = new Color(255, 238, 136),
				["@flavor"] = new Color(153, 238, 221),
				["@reminder"] = new Color(170, 170, 170),
				["@warning"] = new Color(238, 0, 0),
				["@quest"] = new Color(255, 255, 255),
				["@task"] = new Color(255, 255, 255),
				["@event"] = new Color(255, 255, 255)
			},
			SpecialColors = new Dictionary<string, Action<string, TemplateParser.TemplatePreFrag>>()
		};

		private readonly List<(Label label, Image icon)> _topRightIcons = new List<(Label, Image)>();

		private readonly Panel _iconContainer;

		private readonly Image _icon;

		private readonly LabelEx _title;

		private readonly LabelEx _description;

		private readonly List<(Image? prefix, Image icon, Label? stacks, LabelEx text)> _facts = new List<(Image, Image, Label, LabelEx)>();

		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		private static BitmapFont TitleFont => CTextModule.FontAssets.Get(@internal: true, "trebuc.ttf", 19f);

		private static BitmapFont BasicFont => CTextModule.FontAssets.Get(@internal: true, "trebuc.ttf", 17f);

		public bool ShowIcon
		{
			get
			{
				return ((Control)_iconContainer).get_Visible();
			}
			set
			{
				((Control)_iconContainer).set_Visible(value);
				UpdateLayout();
			}
		}

		public SkillTooltip(SkillTooltipData data)
		{
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Expected O, but got Unknown
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Expected O, but got Unknown
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Expected O, but got Unknown
			SkillTooltipData data2 = data;
			((Container)this)._002Ector();
			SkillTooltip skillTooltip = this;
			((Control)this).set_Width(310);
			int? recharge2 = data2.Recharge;
			if (recharge2.HasValue)
			{
				int recharge = recharge2.GetValueOrDefault();
				if (recharge > 0)
				{
					cornerIcon($"{Math.Round((float)recharge / 1000f)}", 156651);
				}
			}
			if (data2.UpkeepCost != 0)
			{
				cornerIcon($"{data2.UpkeepCost}", 156058);
			}
			if (data2.EnergyCost > 0)
			{
				cornerIcon($"{data2.EnergyCost}", 156647);
			}
			if (data2.InitiativeCost > 0)
			{
				cornerIcon($"{data2.InitiativeCost}", 156649);
			}
			recharge2 = data2.Activation;
			if (recharge2.HasValue)
			{
				int activation = recharge2.GetValueOrDefault();
				if (activation > 0)
				{
					cornerIcon(TooltipUtils.FormatFraction((float)activation / 1000f) ?? "", 496252);
				}
			}
			if (data2.DisallowUnderwater)
			{
				cornerIcon("", 358417);
			}
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_BackgroundColor(IconBorderColor * (float)((!StaticData.BoonAndCondi.Contains((uint)data2.Skill.Id)) ? 1 : 0));
			((Control)val).set_Width(37);
			((Control)val).set_Height(37);
			_iconContainer = val;
			((Control)_iconContainer).set_BackgroundColor(Color.get_Transparent());
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)_iconContainer);
			val2.set_Texture(AsyncTexture2D.FromAssetId(data2.IconId.GetValueOrDefault(-1)));
			((Control)val2).set_Width(35);
			((Control)val2).set_Height(35);
			((Control)val2).set_Left(1);
			((Control)val2).set_Top(1);
			_icon = val2;
			if (data2.IconId.HasValue && CTextModule.Settings.AutocropIcons.Value)
			{
				((Func<Task>)async delegate
				{
					Image icon = skillTooltip._icon;
					icon.set_SourceRectangle(await CTextModule.IconBBoxes.GetIconBBoxAsync(data2.IconId.Value, skillTooltip._cts.Token));
				})();
			}
			LabelEx labelEx = new LabelEx(Syntax);
			((Control)labelEx).set_Parent((Container)(object)this);
			labelEx.RawText = data2.Title;
			labelEx.ShowShadow = true;
			labelEx.BaseColor = TitleColor;
			labelEx.Font = TitleFont;
			_title = labelEx;
			LabelEx labelEx2 = new LabelEx(Syntax);
			((Control)labelEx2).set_Parent((Container)(object)this);
			labelEx2.RawText = data2.Description;
			labelEx2.ShowShadow = true;
			labelEx2.Font = BasicFont;
			_description = labelEx2;
			using (List<BlockTooltipData>.Enumerator enumerator = data2.Blocks.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					foreach (FactTooltipData fact in enumerator.Current.Facts)
					{
						List<(Image? prefix, Image icon, Label? stacks, LabelEx text)> facts = _facts;
						recharge2 = fact.PrefixIconId;
						object item;
						if (recharge2.HasValue)
						{
							int prefixIconId = recharge2.GetValueOrDefault();
							Image val3 = new Image();
							item = (object)val3;
							((Control)val3).set_Parent((Container)(object)this);
							val3.set_Texture(AsyncTexture2D.FromAssetId(prefixIconId));
							((Control)val3).set_Width(35);
							((Control)val3).set_Height(35);
						}
						else
						{
							item = null;
						}
						Image val4 = new Image();
						((Control)val4).set_Parent((Container)(object)this);
						val4.set_Texture(AsyncTexture2D.FromAssetId(fact.IconId.GetValueOrDefault(-1)));
						((Control)val4).set_Width(35);
						((Control)val4).set_Height(35);
						object item2;
						if (fact.BuffApplyCount <= 1)
						{
							item2 = null;
						}
						else
						{
							Label val5 = new Label();
							item2 = (object)val5;
							((Control)val5).set_Parent((Container)(object)this);
							val5.set_Text($"{fact.BuffApplyCount}");
							val5.set_Font(BasicFont);
							val5.set_AutoSizeHeight(true);
							val5.set_AutoSizeWidth(true);
						}
						LabelEx labelEx3 = new LabelEx(Syntax);
						((Control)labelEx3).set_Parent((Container)(object)this);
						labelEx3.RawText = fact.Text;
						labelEx3.ShowShadow = true;
						labelEx3.BaseColor = FactTextColor;
						labelEx3.Font = BasicFont;
						facts.Add(((Image)item, val4, (Label)item2, labelEx3));
					}
				}
			}
			UpdateLayout();
			void cornerIcon(string text, int iconId)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Expected O, but got Unknown
				//IL_007c: Expected O, but got Unknown
				List<(Label label, Image icon)> topRightIcons = skillTooltip._topRightIcons;
				Label val6 = new Label();
				((Control)val6).set_Parent((Container)(object)skillTooltip);
				val6.set_Text(text);
				val6.set_ShowShadow(true);
				val6.set_AutoSizeHeight(true);
				val6.set_AutoSizeWidth(true);
				val6.set_HorizontalAlignment((HorizontalAlignment)2);
				val6.set_Font(BasicFont);
				Image val7 = new Image();
				((Control)val7).set_Parent((Container)(object)skillTooltip);
				val7.set_Texture(AsyncTexture2D.FromAssetId(iconId));
				((Control)val7).set_Width(16);
				((Control)val7).set_Height(16);
				topRightIcons.Add((val6, val7));
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			if (_iconContainer == null)
			{
				return;
			}
			int x2 = ((Container)this).get_ContentRegion().Width - 3;
			foreach (var (label, icon2) in _topRightIcons)
			{
				((Control)icon2).set_Right(x2);
				((Control)label).set_Right(((Control)icon2).get_Left() - 2);
				x2 = ((label.get_Text() != "") ? ((Control)label).get_Left() : ((Control)icon2).get_Left()) - 10;
			}
			((Control)_iconContainer).set_Left(0);
			((Control)_iconContainer).set_Top(0);
			((Control)_title).set_Left(((Control)_iconContainer).get_Visible() ? (((Control)_iconContainer).get_Right() + 5) : 3);
			((Control)_title).set_Top(4);
			LabelEx title = _title;
			Label item = _topRightIcons.LastOrDefault().label;
			((Control)title).set_Width(((item != null) ? ((Control)item).get_Left() : ((Container)this).get_ContentRegion().Width) - ((Control)_title).get_Left());
			if (((Control)_iconContainer).get_Visible())
			{
				((Control)(object)_title).MiddleWith((Control)(object)_iconContainer);
				((Control)_title).set_Top(Math.Max(((Control)_title).get_Top(), ((Control)_iconContainer).get_Top()));
			}
			int y2 = ((Control)_title).get_Bottom() + 7;
			if (((Control)_iconContainer).get_Visible())
			{
				y2 = Math.Max(y2, ((Control)_iconContainer).get_Bottom() + 2);
			}
			((Control)_description).set_Left(3);
			((Control)_description).set_Top(y2);
			((Control)(object)_description).WidthFillRight();
			int y = ((Control)_description).get_Bottom() + 14;
			foreach (var (prefix, icon, stacks, text) in _facts)
			{
				if (prefix != null)
				{
					((Control)prefix).set_Left(3);
					((Control)prefix).set_Top(y);
					((Control)icon).set_Left(((Control)prefix).get_Right());
				}
				else
				{
					((Control)icon).set_Left(3);
				}
				((Control)icon).set_Top(y);
				if (stacks != null)
				{
					((Control)stacks).set_Right(((Control)icon).get_Right());
					((Control)stacks).set_Bottom(((Control)icon).get_Bottom() - 2);
				}
				((Control)text).set_Left(((Control)icon).get_Right() + 1);
				((Control)(object)text).WidthFillRight();
				((Control)(object)text).MiddleWith((Control)(object)icon);
				((Control)text).set_Top(Math.Max(((Control)text).get_Top() - 2, ((Control)icon).get_Top() - 2));
				y = Math.Max(((Control)icon).get_Bottom(), ((Control)text).get_Bottom() + 3);
			}
			((Container)(object)this).SetContentRegionHeight(((IEnumerable<Control>)((Container)this).get_Children()).Where((Control x) => x.get_Visible()).Max((Control x) => x.get_Bottom()));
		}

		protected override void DisposeControl()
		{
			_cts.Cancel();
			((Container)this).DisposeControl();
		}
	}
}
