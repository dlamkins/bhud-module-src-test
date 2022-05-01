using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Universal_Search_Module.Strings;

namespace Universal_Search_Module.Controls
{
	public class SkillTooltip : Tooltip
	{
		private const int MAX_WIDTH = 400;

		private readonly Skill _skill;

		public SkillTooltip(Skill skill)
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
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			//IL_0146: Expected O, but got Unknown
			_skill = skill;
			Label val = new Label();
			val.set_Text(_skill.Name);
			val.set_Font(Control.get_Content().get_DefaultFont18());
			val.set_TextColor(Colors.Chardonnay);
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Parent((Container)(object)this);
			Label skillTitle = val;
			Label categoryText = null;
			string description = _skill.Description;
			if (_skill.Categories != null)
			{
				Label val2 = new Label();
				val2.set_Text(string.Join(", ", skill.Categories));
				val2.set_Font(Control.get_Content().get_DefaultFont16());
				val2.set_AutoSizeHeight(true);
				val2.set_AutoSizeWidth(true);
				((Control)val2).set_Location(new Point(0, ((Control)skillTitle).get_Bottom() + 5));
				val2.set_TextColor(Colors.ColonialWhite);
				((Control)val2).set_Parent((Container)(object)this);
				categoryText = val2;
				description = description.Substring(description.IndexOf(".") + 1).Trim();
			}
			Label val3 = new Label();
			val3.set_Text(description);
			val3.set_Font(Control.get_Content().get_DefaultFont16());
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Location(new Point(0, ((categoryText == null) ? ((Control)skillTitle).get_Bottom() : ((Control)categoryText).get_Bottom()) + 5));
			((Control)val3).set_Parent((Container)(object)this);
			LabelUtil.HandleMaxWidth(val3, 400);
			Control lastFact = (Control)val3;
			Control lastTopRightCornerControl = null;
			if (_skill.Facts != null)
			{
				SkillFactRecharge rechargeFact = null;
				foreach (SkillFact fact in _skill.Facts!)
				{
					SkillFactRecharge skillFactRecharge = fact as SkillFactRecharge;
					if (skillFactRecharge != null)
					{
						rechargeFact = skillFactRecharge;
					}
					else
					{
						lastFact = CreateFact(fact, lastFact);
					}
				}
				if (rechargeFact != null)
				{
					lastTopRightCornerControl = CreateRechargeFact(rechargeFact);
				}
			}
			if (skill.Professions.Contains<string>("Revenant") && skill.Cost.HasValue)
			{
				lastTopRightCornerControl = CreateEnergyDisplay(lastTopRightCornerControl);
			}
			if (skill.Professions.Contains<string>("Thief") && skill.Initiative.HasValue)
			{
				lastTopRightCornerControl = CreateInitiativeDisplay(lastTopRightCornerControl);
			}
			if (skill.Flags.ToArray().FirstOrDefault((ApiEnum<SkillFlag> x) => x.ToEnum() == SkillFlag.NoUnderwater) != null)
			{
				lastTopRightCornerControl = CreateNonUnderwaterDisplay(lastTopRightCornerControl);
			}
		}

		private Control CreateFact(SkillFact fact, Control lastFact)
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
			if (fact is SkillFactDamage)
			{
				return lastFact;
			}
			RenderUrl? icon = fact.Icon;
			SkillFactPrefixedBuff prefixedBuff = fact as SkillFactPrefixedBuff;
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

		private string GetTextForFact(SkillFact fact)
		{
			SkillFactAttributeAdjust attributeAdjust = fact as SkillFactAttributeAdjust;
			if (attributeAdjust == null)
			{
				SkillFactBuff buff = fact as SkillFactBuff;
				if (buff == null)
				{
					SkillFactComboField comboField = fact as SkillFactComboField;
					if (comboField == null)
					{
						SkillFactComboFinisher comboFinisher = fact as SkillFactComboFinisher;
						if (comboFinisher == null)
						{
							SkillFactDamage damage = fact as SkillFactDamage;
							if (damage == null)
							{
								SkillFactDistance distance = fact as SkillFactDistance;
								if (distance == null)
								{
									SkillFactDuration duration = fact as SkillFactDuration;
									if (duration == null)
									{
										SkillFactHeal heal = fact as SkillFactHeal;
										if (heal == null)
										{
											SkillFactHealingAdjust healingAdjust = fact as SkillFactHealingAdjust;
											if (healingAdjust == null)
											{
												if (!(fact is SkillFactNoData))
												{
													SkillFactNumber skillFactNumber = fact as SkillFactNumber;
													if (skillFactNumber == null)
													{
														SkillFactPercent skillFactPercent = fact as SkillFactPercent;
														if (skillFactPercent == null)
														{
															SkillFactPrefixedBuff skillFactPrefixedBuff = fact as SkillFactPrefixedBuff;
															if (skillFactPrefixedBuff == null)
															{
																SkillFactRadius skillFactRadius = fact as SkillFactRadius;
																if (skillFactRadius == null)
																{
																	SkillFactRange skillFactRange = fact as SkillFactRange;
																	if (skillFactRange == null)
																	{
																		if (!(fact is SkillFactStunBreak))
																		{
																			SkillFactTime skillFactTime = fact as SkillFactTime;
																			if (skillFactTime == null)
																			{
																				if (!(fact is SkillFactUnblockable))
																				{
																				}
																				return fact.Text;
																			}
																			return $"{skillFactTime.Text}: {skillFactTime.Duration}s";
																		}
																		return Common.SkillTooltip_BreaksStun;
																	}
																	return $"{skillFactRange.Text}: {skillFactRange.Value}";
																}
																return $"{skillFactRadius.Text}: {skillFactRadius.Distance}";
															}
															return $"{skillFactPrefixedBuff.ApplyCount}x {skillFactPrefixedBuff.Status} ({skillFactPrefixedBuff.Duration}s): {skillFactPrefixedBuff.Description}";
														}
														return $"{skillFactPercent.Text}: {skillFactPercent.Percent}%";
													}
													return $"{skillFactNumber.Text}: {skillFactNumber.Value}";
												}
												return Common.SkillTooltip_CombatOnly;
											}
											return $"{healingAdjust.HitCount}x {healingAdjust.Text}";
										}
										return $"{heal.HitCount}x {heal.Text}";
									}
									return $"{duration.Text}: {duration.Duration}s";
								}
								return $"{distance.Text}: {distance.Distance}";
							}
							return $"{damage.Text}({damage.HitCount}x): {damage.Text}";
						}
						return $"{comboFinisher.Text}: {comboFinisher.Type} ({comboFinisher.Percent}% {Common.SkillTooltip_Chance})";
					}
					return comboField.Text + ": " + comboField.FieldType.ToEnumString();
				}
				string applyCountText = ((buff.ApplyCount.HasValue && buff.ApplyCount != 1) ? (buff.ApplyCount + "x ") : string.Empty);
				string durationText = ((buff.Duration != 0) ? $" ({buff.Duration}s) " : string.Empty);
				return applyCountText + buff.Status + durationText + ": " + buff.Description;
			}
			return $"{attributeAdjust.Text}: {attributeAdjust.Value}";
		}

		private Control CreateRechargeFact(SkillFactRecharge skillFactRecharge)
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
			return (Control)(object)cooldownText;
		}

		private Control CreateInitiativeDisplay(Control lastControl)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			Image val = new Image();
			val.set_Texture(AsyncTexture2D.op_Implicit(UniversalSearchModule.ModuleInstance.ContentsManager.GetTexture("textures\\156649.png")));
			((Control)val).set_Visible(true);
			((Control)val).set_Size(new Point(16, 16));
			((Control)val).set_Parent((Container)(object)this);
			Image initiativeImage = val;
			if (lastControl == null)
			{
				((Control)initiativeImage).set_Location(new Point(((Control)this).get_Width() - ((Control)initiativeImage).get_Width(), 1));
			}
			else
			{
				((Control)initiativeImage).set_Location(new Point(lastControl.get_Left() - ((Control)initiativeImage).get_Width() - 5, 0));
			}
			Label val2 = new Label();
			val2.set_Text(_skill.Initiative.ToString());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)this);
			Label initiativeText = val2;
			((Control)initiativeText).set_Location(new Point(((Control)initiativeImage).get_Left() - ((Control)initiativeText).get_Width() - 2, 0));
			return (Control)(object)initiativeText;
		}

		private Control CreateEnergyDisplay(Control lastControl)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			Image val = new Image();
			val.set_Texture(AsyncTexture2D.op_Implicit(UniversalSearchModule.ModuleInstance.ContentsManager.GetTexture("textures\\156647.png")));
			((Control)val).set_Visible(true);
			((Control)val).set_Size(new Point(16, 16));
			((Control)val).set_Parent((Container)(object)this);
			Image energyImage = val;
			if (lastControl == null)
			{
				((Control)energyImage).set_Location(new Point(((Control)this).get_Width() - ((Control)energyImage).get_Width(), 1));
			}
			else
			{
				((Control)energyImage).set_Location(new Point(lastControl.get_Left() - ((Control)energyImage).get_Width() - 5, 0));
			}
			Label val2 = new Label();
			val2.set_Text(_skill.Cost.ToString());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Parent((Container)(object)this);
			Label energyText = val2;
			((Control)energyText).set_Location(new Point(((Control)energyImage).get_Left() - ((Control)energyText).get_Width() - 2, 0));
			return (Control)(object)energyText;
		}

		private Control CreateNonUnderwaterDisplay(Control lastControl)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			Image val = new Image();
			val.set_Texture(AsyncTexture2D.op_Implicit(UniversalSearchModule.ModuleInstance.ContentsManager.GetTexture("textures\\358417.png")));
			((Control)val).set_Visible(true);
			((Control)val).set_Size(new Point(16, 16));
			((Control)val).set_Parent((Container)(object)this);
			Image underwaterImage = val;
			if (lastControl == null)
			{
				((Control)underwaterImage).set_Location(new Point(((Control)this).get_Width() - ((Control)underwaterImage).get_Width(), 1));
			}
			else
			{
				((Control)underwaterImage).set_Location(new Point(lastControl.get_Left() - ((Control)underwaterImage).get_Width() - 5, 0));
			}
			return (Control)(object)underwaterImage;
		}
	}
}
