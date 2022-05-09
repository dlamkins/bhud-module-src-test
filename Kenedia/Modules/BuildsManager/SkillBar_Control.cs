using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class SkillBar_Control : Control
	{
		private CustomTooltip CustomTooltip;

		private List<Skill_Control> _Skills_Aquatic;

		private List<Skill_Control> _Skills_Terrestial;

		private Texture2D _AquaTexture;

		private Texture2D _TerrestialTexture;

		public SkillSelector_Control SkillSelector;

		private double _Scale = 1.0;

		private int _SkillSize = 55;

		public int _Width = 643;

		public Template Template => BuildsManager.ModuleInstance.Selected_Template;

		public double Scale
		{
			get
			{
				return _Scale;
			}
			set
			{
				_Scale = value;
				foreach (Skill_Control item in _Skills_Aquatic)
				{
					item.Scale = value;
				}
				foreach (Skill_Control item2 in _Skills_Terrestial)
				{
					item2.Scale = value;
				}
			}
		}

		public SkillBar_Control(Container parent)
			: this()
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			CustomTooltip customTooltip = new CustomTooltip(((Control)this).get_Parent());
			((Control)customTooltip).set_ClipsBounds(false);
			customTooltip.HeaderColor = new Color(255, 204, 119, 255);
			CustomTooltip = customTooltip;
			_TerrestialTexture = BuildsManager.TextureManager.getControlTexture(_Controls.Land);
			_AquaTexture = BuildsManager.TextureManager.getControlTexture(_Controls.Water);
			Enum.GetValues(typeof(SkillSlots));
			_Skills_Aquatic = new List<Skill_Control>();
			foreach (API.Skill item in Template.Build.Skills_Aquatic)
			{
				_ = item;
				List<Skill_Control> skills_Aquatic = _Skills_Aquatic;
				Skill_Control skill_Control = new Skill_Control(((Control)this).get_Parent());
				((Control)skill_Control).set_Location(new Point(27 + _Skills_Aquatic.Count * (_SkillSize + 1), 0));
				skill_Control.Skill = Template.Build.Skills_Aquatic[_Skills_Aquatic.Count];
				skill_Control.Slot = (SkillSlots)_Skills_Aquatic.Count;
				skill_Control.Aquatic = true;
				skills_Aquatic.Add(skill_Control);
				Skill_Control control2 = _Skills_Aquatic[_Skills_Aquatic.Count - 1];
				((Control)control2).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0065: Unknown result type (might be due to invalid IL or missing references)
					//IL_0076: Unknown result type (might be due to invalid IL or missing references)
					//IL_007b: Unknown result type (might be due to invalid IL or missing references)
					if (!((Control)SkillSelector).get_Visible() || SkillSelector.currentObject != control2)
					{
						((Control)SkillSelector).set_Visible(true);
						SkillSelector.Skill_Control = control2;
						((Control)SkillSelector).set_Location(((Control)control2).get_Location().Add(new Point(2, ((Control)control2).get_Height())));
						List<API.Skill> list2 = new List<API.Skill>();
						if (Template.Build.Profession != null)
						{
							foreach (API.Skill iSkill2 in (from e in Template.Build.Profession.Skills
								orderby e.Specialization, (e.Categories.Count <= 0) ? "Unkown" : e.Categories[0]
								select e).ToList())
							{
								if (iSkill2.Specialization == 0 || Template.Build.SpecLines.Find((SpecLine e) => e.Specialization != null && e.Specialization.Id == iSkill2.Specialization) != null)
								{
									switch (control2.Slot)
									{
									case SkillSlots.Heal:
										if (iSkill2.Slot == API.skillSlot.Heal)
										{
											list2.Add(iSkill2);
										}
										break;
									case SkillSlots.Elite:
										if (iSkill2.Slot == API.skillSlot.Elite)
										{
											list2.Add(iSkill2);
										}
										break;
									default:
										if (iSkill2.Slot == API.skillSlot.Utility)
										{
											list2.Add(iSkill2);
										}
										break;
									}
								}
							}
						}
						SkillSelector.Skills = list2;
						SkillSelector.Aquatic = true;
						SkillSelector.currentObject = control2;
					}
					else
					{
						((Control)SkillSelector).set_Visible(false);
					}
				});
				BuildsManager.ModuleInstance.Selected_Template_Changed += ApplyBuild;
			}
			int p = _Width - _Skills_Aquatic.Count * (_SkillSize + 1);
			_Skills_Terrestial = new List<Skill_Control>();
			foreach (API.Skill item2 in Template.Build.Skills_Terrestial)
			{
				_ = item2;
				List<Skill_Control> skills_Terrestial = _Skills_Terrestial;
				Skill_Control skill_Control2 = new Skill_Control(((Control)this).get_Parent());
				((Control)skill_Control2).set_Location(new Point(p + _Skills_Terrestial.Count * (_SkillSize + 1), 0));
				skill_Control2.Skill = Template.Build.Skills_Terrestial[_Skills_Terrestial.Count];
				skill_Control2.Slot = (SkillSlots)_Skills_Terrestial.Count;
				skills_Terrestial.Add(skill_Control2);
				Skill_Control control = _Skills_Terrestial[_Skills_Terrestial.Count - 1];
				((Control)control).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0065: Unknown result type (might be due to invalid IL or missing references)
					//IL_0077: Unknown result type (might be due to invalid IL or missing references)
					//IL_007c: Unknown result type (might be due to invalid IL or missing references)
					if (!((Control)SkillSelector).get_Visible() || SkillSelector.currentObject != control)
					{
						((Control)SkillSelector).set_Visible(true);
						SkillSelector.Skill_Control = control;
						((Control)SkillSelector).set_Location(((Control)control).get_Location().Add(new Point(-2, ((Control)control).get_Height())));
						List<API.Skill> list = new List<API.Skill>();
						if (Template.Build.Profession != null)
						{
							foreach (API.Skill iSkill in (from e in Template.Build.Profession.Skills
								orderby e.Specialization, (e.Categories.Count <= 0) ? "Unkown" : e.Categories[0]
								select e).ToList())
							{
								if (iSkill.Specialization == 0 || Template.Build.SpecLines.Find((SpecLine e) => e.Specialization != null && e.Specialization.Id == iSkill.Specialization) != null)
								{
									switch (control.Slot)
									{
									case SkillSlots.Heal:
										if (iSkill.Slot == API.skillSlot.Heal)
										{
											list.Add(iSkill);
										}
										break;
									case SkillSlots.Elite:
										if (iSkill.Slot == API.skillSlot.Elite)
										{
											list.Add(iSkill);
										}
										break;
									default:
										if (iSkill.Slot == API.skillSlot.Utility)
										{
											list.Add(iSkill);
										}
										break;
									}
								}
							}
						}
						SkillSelector.Skills = list;
						SkillSelector.Aquatic = false;
						SkillSelector.currentObject = control;
					}
					else
					{
						((Control)SkillSelector).set_Visible(false);
					}
				});
			}
			SkillSelector_Control skillSelector_Control = new SkillSelector_Control();
			((Control)skillSelector_Control).set_Parent(((Control)this).get_Parent());
			((Control)skillSelector_Control).set_Visible(false);
			((Control)skillSelector_Control).set_ZIndex(((Control)this).get_ZIndex() + 3);
			SkillSelector = skillSelector_Control;
			SkillSelector.SkillChanged += OnSkillChanged;
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGlobalClick);
		}

		public void ApplyBuild(object sender, EventArgs e)
		{
			SetTemplate();
		}

		private void OnSkillChanged(object sender, SkillChangedEvent e)
		{
			if (e.Skill_Control.Aquatic)
			{
				foreach (Skill_Control skill_Control2 in _Skills_Aquatic)
				{
					if (skill_Control2.Skill == e.Skill)
					{
						skill_Control2.Skill = null;
					}
				}
				for (int j = 0; j < Template.Build.Skills_Aquatic.Count; j++)
				{
					if (Template.Build.Skills_Aquatic[j] == e.Skill)
					{
						Template.Build.Skills_Aquatic[j] = null;
					}
				}
				Template.Build.Skills_Aquatic[(int)e.Skill_Control.Slot] = e.Skill;
				e.Skill_Control.Skill = e.Skill;
			}
			else
			{
				foreach (Skill_Control skill_Control in _Skills_Terrestial)
				{
					if (skill_Control.Skill == e.Skill)
					{
						skill_Control.Skill = null;
					}
				}
				for (int i = 0; i < Template.Build.Skills_Terrestial.Count; i++)
				{
					if (Template.Build.Skills_Terrestial[i] == e.Skill)
					{
						Template.Build.Skills_Terrestial[i] = null;
					}
				}
				Template.Build.Skills_Terrestial[(int)e.Skill_Control.Slot] = e.Skill;
				e.Skill_Control.Skill = e.Skill;
			}
			Template.SetChanged();
		}

		private void OnGlobalClick(object sender, MouseEventArgs m)
		{
			if (!((Control)this).get_MouseOver() && !((Control)SkillSelector).get_MouseOver())
			{
				((Control)SkillSelector).Hide();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _AquaTexture, ClassExtensions.Scale(new Rectangle(new Point(0, 0), new Point(25, 25)), Scale), (Rectangle?)_AquaTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _TerrestialTexture, ClassExtensions.Scale(new Rectangle(new Point(_Width - (_Skills_Aquatic.Count * (_SkillSize + 1) + 28), 0), new Point(25, 25)), Scale), (Rectangle?)_TerrestialTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
		}

		protected override void DisposeControl()
		{
			BuildsManager.ModuleInstance.Selected_Template_Changed -= ApplyBuild;
			((Control)this).DisposeControl();
		}

		public void SetTemplate()
		{
			_ = BuildsManager.ModuleInstance.Selected_Template;
			int i = 0;
			i = 0;
			foreach (Skill_Control item in _Skills_Aquatic)
			{
				item.Skill = Template.Build.Skills_Aquatic[i];
				i++;
			}
			i = 0;
			foreach (Skill_Control item2 in _Skills_Terrestial)
			{
				item2.Skill = Template.Build.Skills_Terrestial[i];
				i++;
			}
		}
	}
}
