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

		private static readonly MarkupParser.Syntax<MarkupParser.Fragment> Syntax = new MarkupParser.Syntax<MarkupParser.Fragment>
		{
			ColorTagOpenA = "<c=",
			ColorTagOpenB = '>',
			ColorTagClose = new HashSet<string> { "</c>", "<c/>" },
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
			SpecialColors = new Dictionary<string, Action<string, MarkupParser.Fragment>>()
		};

		private readonly List<(Label label, Image icon)> _topRightIcons = new List<(Label, Image)>();

		private readonly Panel _iconContainer;

		private readonly Image _icon;

		private readonly MarkupLabel _title;

		private readonly MarkupLabel _description;

		private readonly List<(Image? prefix, Image icon, Label? stacks, MarkupLabel text)> _facts = new List<(Image, Image, Label, MarkupLabel)>();

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
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Expected O, but got Unknown
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Expected O, but got Unknown
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_0459: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Expected O, but got Unknown
			SkillTooltipData data2 = data;
			((Container)this)._002Ector();
			SkillTooltip skillTooltip = this;
			((Control)this).set_Width(310);
			if (data2.SupplyCost > 0)
			{
				cornerIcon($"{data2.SupplyCost}", 2111003);
			}
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
			if (data2.EnduranceCost > 0f)
			{
				cornerIcon(TooltipUtils.FormatFraction(data2.EnduranceCost) ?? "", 156647);
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
			((Control)val).set_BackgroundColor(IconBorderColor * (float)((!StaticData.BoonAndCondi.Contains(data2.Skill.Id)) ? 1 : 0));
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
			MarkupLabel markupLabel = new MarkupLabel(Syntax);
			((Control)markupLabel).set_Parent((Container)(object)this);
			markupLabel.RawText = data2.Title;
			markupLabel.ShowShadow = true;
			markupLabel.BaseColor = TitleColor;
			markupLabel.Font = TitleFont;
			_title = markupLabel;
			MarkupLabel markupLabel2 = new MarkupLabel(Syntax);
			((Control)markupLabel2).set_Parent((Container)(object)this);
			markupLabel2.RawText = data2.Description;
			markupLabel2.ShowShadow = true;
			markupLabel2.Font = BasicFont;
			_description = markupLabel2;
			if (_description.RawText == "")
			{
				_description.RawText = "...";
			}
			using (List<BlockTooltipData>.Enumerator enumerator = data2.Blocks.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					foreach (FactTooltipData fact in enumerator.Current.Facts)
					{
						List<(Image? prefix, Image icon, Label? stacks, MarkupLabel text)> facts = _facts;
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
						MarkupLabel markupLabel3 = new MarkupLabel(Syntax);
						((Control)markupLabel3).set_Parent((Container)(object)this);
						markupLabel3.RawText = fact.Text;
						markupLabel3.ShowShadow = true;
						markupLabel3.BaseColor = FactTextColor;
						markupLabel3.Font = BasicFont;
						facts.Add(((Image)item, val4, (Label)item2, markupLabel3));
					}
				}
			}
			UpdateLayout();
			ShowIcon = data2.IconId.HasValue;
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
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
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
				((Control)icon2).set_Top(1);
				((Control)(object)label).MiddleWith((Control)(object)icon2);
				((Control)label).set_Top(((Control)label).get_Top() - 3);
			}
			((Control)_iconContainer).set_Left(0);
			((Control)_iconContainer).set_Top(0);
			((Control)_title).set_Left(((Control)_iconContainer).get_Visible() ? (((Control)_iconContainer).get_Right() + 5) : 3);
			((Control)_title).set_Top(4);
			MarkupLabel title = _title;
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
