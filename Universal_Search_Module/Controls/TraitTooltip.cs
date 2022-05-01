using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Universal_Search_Module.Strings;

namespace Universal_Search_Module.Controls
{
	public class TraitTooltip : Tooltip
	{
		private const int MAX_WIDTH = 400;

		private readonly Trait _trait;

		public TraitTooltip(Trait trait)
			: this()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00b6: Expected O, but got Unknown
			_trait = trait;
			Label val = new Label();
			val.set_Text(_trait.Name);
			val.set_Font(Control.get_Content().get_DefaultFont18());
			val.set_TextColor(Colors.Chardonnay);
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Parent((Container)(object)this);
			Label traitTitle = val;
			Label val2 = new Label();
			val2.set_Text(StringUtil.SanitizeTraitDescription(_trait.Description));
			val2.set_Font(Control.get_Content().get_DefaultFont16());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(0, ((Control)traitTitle).get_Bottom() + 5));
			((Control)val2).set_Parent((Container)(object)this);
			LabelUtil.HandleMaxWidth(val2, 400);
			Control lastFact = (Control)val2;
			if (_trait.Facts == null)
			{
				return;
			}
			TraitFactRecharge rechargeFact = null;
			foreach (TraitFact fact in _trait.Facts!)
			{
				TraitFactRecharge traitFactRecharge = fact as TraitFactRecharge;
				if (traitFactRecharge != null)
				{
					rechargeFact = traitFactRecharge;
				}
				else
				{
					lastFact = CreateFact(fact, lastFact);
				}
			}
			if (rechargeFact != null)
			{
				CreateRechargeFact(rechargeFact);
			}
		}

		private Control CreateFact(TraitFact fact, Control lastFact)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected O, but got Unknown
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Expected O, but got Unknown
			if (fact is TraitFactDamage)
			{
				return lastFact;
			}
			RenderUrl? icon = fact.Icon;
			TraitFactPrefixedBuff prefixedBuff = fact as TraitFactPrefixedBuff;
			if (prefixedBuff != null)
			{
				icon = prefixedBuff.Prefix.Icon;
			}
			Image val = new Image();
			AsyncTexture2D texture;
			if (!icon.HasValue)
			{
				texture = AsyncTexture2D.op_Implicit(Textures.get_Error());
			}
			else
			{
				ContentService content = Control.get_Content();
				RenderUrl? renderUrl = icon;
				texture = content.GetRenderServiceTexture(renderUrl.HasValue ? ((string)renderUrl.GetValueOrDefault()) : null);
			}
			val.set_Texture(texture);
			((Control)val).set_Size(new Point(32, 32));
			((Control)val).set_Location(new Point(0, lastFact.get_Bottom() + 5));
			((Control)val).set_Parent((Container)(object)this);
			Image factImage = val;
			Label val2 = new Label();
			val2.set_Text(GetTextForFact(fact));
			val2.set_Font(Control.get_Content().get_DefaultFont16());
			val2.set_TextColor(new Color(161, 161, 161));
			((Control)val2).set_Height(((Control)factImage).get_Height());
			val2.set_VerticalAlignment((VerticalAlignment)1);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Location(new Point(((Control)factImage).get_Width() + 5, lastFact.get_Bottom() + 5));
			((Control)val2).set_Parent((Container)(object)this);
			Label factDescription = val2;
			LabelUtil.HandleMaxWidth(factDescription, 400, ((Control)factImage).get_Width(), delegate
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				factDescription.set_AutoSizeHeight(true);
				((Control)factDescription).RecalculateLayout();
				((Control)factImage).set_Location(new Point(0, ((Control)factDescription).get_Location().Y + (((Control)factDescription).get_Height() / 2 - ((Control)factImage).get_Height() / 2)));
			});
			return (Control)(object)factDescription;
		}

		private string GetTextForFact(TraitFact fact)
		{
			TraitFactAttributeAdjust attributeAdjust = fact as TraitFactAttributeAdjust;
			if (attributeAdjust == null)
			{
				TraitFactBuff buff = fact as TraitFactBuff;
				if (buff == null)
				{
					TraitFactBuffConversion buffConversion = fact as TraitFactBuffConversion;
					if (buffConversion == null)
					{
						TraitFactComboField comboField = fact as TraitFactComboField;
						if (comboField == null)
						{
							TraitFactComboFinisher comboFinisher = fact as TraitFactComboFinisher;
							if (comboFinisher == null)
							{
								TraitFactDamage damage = fact as TraitFactDamage;
								if (damage == null)
								{
									TraitFactDistance distance = fact as TraitFactDistance;
									if (distance == null)
									{
										if (!(fact is TraitFactNoData))
										{
											TraitFactNumber number = fact as TraitFactNumber;
											if (number == null)
											{
												TraitFactPercent percent = fact as TraitFactPercent;
												if (percent == null)
												{
													TraitFactPrefixedBuff prefixedBuff = fact as TraitFactPrefixedBuff;
													if (prefixedBuff == null)
													{
														TraitFactRadius radius = fact as TraitFactRadius;
														if (radius == null)
														{
															TraitFactRange range = fact as TraitFactRange;
															if (range == null)
															{
																TraitFactTime time = fact as TraitFactTime;
																if (time == null)
																{
																	if (!(fact is TraitFactUnblockable))
																	{
																	}
																	return fact.Text;
																}
																return $"{time.Text}: {time.Duration}s";
															}
															return $"{range.Text}: {range.Value}";
														}
														return $"{radius.Text}: {radius.Distance}";
													}
													return $"{prefixedBuff.ApplyCount}x {prefixedBuff.Status} ({prefixedBuff.Duration}s): {prefixedBuff.Description}";
												}
												return $"{percent.Text}: {percent.Percent}%";
											}
											return $"{number.Text}: {number.Value}";
										}
										return Common.TraitTooltip_CombatOnly;
									}
									return $"{distance.Text}: {distance.Distance}";
								}
								return $"{damage.Text}({damage.HitCount}x): {damage.Text}";
							}
							return $"{comboFinisher.Text}: {comboFinisher.Type} ({comboFinisher.Percent} {Common.TraitTooltip_Chance})";
						}
						return comboField.Text + ": " + comboField.FieldType.ToEnumString();
					}
					return string.Format(Common.TraitTooltip_BuffConversion, buffConversion.Target, buffConversion.Source, buffConversion.Percent);
				}
				string applyCountText = ((buff.ApplyCount.HasValue && buff.ApplyCount != 1) ? (buff.ApplyCount + "x ") : string.Empty);
				string durationText = ((buff.Duration != 0) ? $" ({buff.Duration}s) " : string.Empty);
				return applyCountText + buff.Status + durationText + ": " + buff.Description;
			}
			return $"{attributeAdjust.Text}: {attributeAdjust.Value}";
		}

		private void CreateRechargeFact(TraitFactRecharge skillFactRecharge)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected O, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			Image val = new Image();
			AsyncTexture2D texture;
			if (!skillFactRecharge.Icon.HasValue)
			{
				texture = AsyncTexture2D.op_Implicit(Textures.get_Error());
			}
			else
			{
				ContentService content = Control.get_Content();
				RenderUrl? icon = skillFactRecharge.Icon;
				texture = content.GetRenderServiceTexture(icon.HasValue ? ((string)icon.GetValueOrDefault()) : null);
			}
			val.set_Texture(texture);
			((Control)val).set_Visible(true);
			((Control)val).set_Size(new Point(16, 16));
			((Control)val).set_Parent((Container)(object)this);
			Image cooldownImage = val;
			((Control)cooldownImage).set_Location(new Point(((Control)this).get_Width() - ((Control)cooldownImage).get_Width(), 1));
			Label val2 = new Label();
			val2.set_Text(skillFactRecharge.Value.ToString());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)this);
			Label cooldownText = val2;
			((Control)cooldownText).set_Location(new Point(((Control)cooldownImage).get_Left() - ((Control)cooldownText).get_Width() - 2, 0));
		}
	}
}
