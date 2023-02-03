using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Enums;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SkillBar_Control : Control
	{
		private readonly CustomTooltip CustomTooltip;

		private readonly List<Skill_Control> _Legends_Aquatic;

		private readonly List<Skill_Control> _Legends_Terrestrial;

		private readonly List<Skill_Control> _Skills_Aquatic;

		private readonly List<Skill_Control> _Skills_Terrestrial;

		private readonly Texture2D _AquaTexture;

		private readonly Texture2D _TerrestrialTexture;

		private readonly Texture2D _SwapTexture;

		public SkillSelector_Control SkillSelector;

		public SkillSelector_Control LegendSelector;

		public SkillSelector_Control PetSelector;

		private bool CanClick = true;

		private bool ShowProfessionSkills;

		private double _Scale = 1.0;

		private readonly int _SkillSize = 55;

		public int _Width = 643;

		private Point _OGLocation;

		public Template Template => BuildsManager.s_moduleInstance.Selected_Template;

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
				foreach (Skill_Control item2 in _Skills_Terrestrial)
				{
					item2.Scale = value;
				}
			}
		}

		public Point _Location
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return ((Control)this).get_Location();
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).get_Location();
				if (((Control)this).get_Location() == Point.get_Zero())
				{
					_OGLocation = value;
				}
				((Control)this).set_Location(value);
			}
		}

		public SkillBar_Control(Container parent)
			: this()
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			CustomTooltip customTooltip = new CustomTooltip(((Control)this).get_Parent());
			((Control)customTooltip).set_ClipsBounds(false);
			customTooltip.HeaderColor = new Color(255, 204, 119, 255);
			CustomTooltip = customTooltip;
			_TerrestrialTexture = BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.Land);
			_AquaTexture = BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.Water);
			_SwapTexture = BuildsManager.s_moduleInstance.TextureManager.getIcon(Icons.Refresh);
			Enum.GetValues(typeof(SkillSlots));
			_Skills_Aquatic = new List<Skill_Control>();
			foreach (API.Skill item in Template.Build.Skills_Aquatic)
			{
				_ = item;
				List<Skill_Control> skills_Aquatic = _Skills_Aquatic;
				Skill_Control skill_Control = new Skill_Control(((Control)this).get_Parent());
				((Control)skill_Control).set_Location(new Point(27 + _Skills_Aquatic.Count * (_SkillSize + 1), 51));
				skill_Control.Skill = Template.Build.Skills_Aquatic[_Skills_Aquatic.Count];
				skill_Control.Slot = (SkillSlots)_Skills_Aquatic.Count;
				skill_Control.Aquatic = true;
				skills_Aquatic.Add(skill_Control);
				((Control)_Skills_Aquatic[_Skills_Aquatic.Count - 1]).add_Click((EventHandler<MouseEventArgs>)Control_Click);
			}
			int p = _Width - _Skills_Aquatic.Count * (_SkillSize + 1);
			_Skills_Terrestrial = new List<Skill_Control>();
			foreach (API.Skill item2 in Template.Build.Skills_Terrestrial)
			{
				_ = item2;
				List<Skill_Control> skills_Terrestrial = _Skills_Terrestrial;
				Skill_Control skill_Control2 = new Skill_Control(((Control)this).get_Parent());
				((Control)skill_Control2).set_Location(new Point(p + _Skills_Terrestrial.Count * (_SkillSize + 1), 51));
				skill_Control2.Skill = Template.Build.Skills_Terrestrial[_Skills_Terrestrial.Count];
				skill_Control2.Slot = (SkillSlots)_Skills_Terrestrial.Count;
				skills_Terrestrial.Add(skill_Control2);
				((Control)_Skills_Terrestrial[_Skills_Terrestrial.Count - 1]).add_Click((EventHandler<MouseEventArgs>)Control_Click);
			}
			SkillSelector_Control skillSelector_Control = new SkillSelector_Control();
			((Control)skillSelector_Control).set_Parent(((Control)this).get_Parent());
			((Control)skillSelector_Control).set_Visible(false);
			((Control)skillSelector_Control).set_ZIndex(((Control)this).get_ZIndex() + 3);
			SkillSelector = skillSelector_Control;
			_Legends_Aquatic = new List<Skill_Control>();
			List<Skill_Control> legends_Aquatic = _Legends_Aquatic;
			Skill_Control obj = new Skill_Control(((Control)this).get_Parent())
			{
				Skill = Template.Build.Legends_Aquatic[0].Skill,
				Slot = SkillSlots.AquaticLegend1,
				Aquatic = true,
				Scale = 0.8
			};
			((Control)obj).set_Location(new Point(27, 0));
			legends_Aquatic.Add(obj);
			List<Skill_Control> legends_Aquatic2 = _Legends_Aquatic;
			Skill_Control obj2 = new Skill_Control(((Control)this).get_Parent())
			{
				Skill = Template.Build.Legends_Aquatic[1].Skill,
				Slot = SkillSlots.AquaticLegend1,
				Aquatic = true,
				Scale = 0.8
			};
			((Control)obj2).set_Location(new Point(89, 0));
			legends_Aquatic2.Add(obj2);
			((Control)_Legends_Aquatic[0]).add_Click((EventHandler<MouseEventArgs>)Legend);
			((Control)_Legends_Aquatic[1]).add_Click((EventHandler<MouseEventArgs>)Legend);
			_Legends_Terrestrial = new List<Skill_Control>();
			List<Skill_Control> legends_Terrestrial = _Legends_Terrestrial;
			Skill_Control obj3 = new Skill_Control(((Control)this).get_Parent())
			{
				Skill = Template.Build.Legends_Terrestrial[0].Skill,
				Slot = SkillSlots.TerrestrialLegend1,
				Aquatic = false,
				Scale = 0.8
			};
			((Control)obj3).set_Location(new Point(p, 0));
			legends_Terrestrial.Add(obj3);
			List<Skill_Control> legends_Terrestrial2 = _Legends_Terrestrial;
			Skill_Control obj4 = new Skill_Control(((Control)this).get_Parent())
			{
				Skill = Template.Build.Legends_Terrestrial[1].Skill,
				Slot = SkillSlots.TerrestrialLegend1,
				Aquatic = false,
				Scale = 0.8
			};
			((Control)obj4).set_Location(new Point(p + 36 + 26, 0));
			legends_Terrestrial2.Add(obj4);
			((Control)_Legends_Terrestrial[0]).add_Click((EventHandler<MouseEventArgs>)Legend);
			((Control)_Legends_Terrestrial[1]).add_Click((EventHandler<MouseEventArgs>)Legend);
			SkillSelector_Control skillSelector_Control2 = new SkillSelector_Control();
			((Control)skillSelector_Control2).set_Parent(((Control)this).get_Parent());
			((Control)skillSelector_Control2).set_Visible(false);
			((Control)skillSelector_Control2).set_ZIndex(((Control)this).get_ZIndex() + 3);
			LegendSelector = skillSelector_Control2;
			LegendSelector.SkillChanged += LegendSelector_SkillChanged;
			SkillSelector.SkillChanged += OnSkillChanged;
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGlobalClick);
			BuildsManager.s_moduleInstance.Selected_Template_Changed += ApplyBuild;
		}

		private void Control_Click(object sender, MouseEventArgs mouse)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			Skill_Control control = (Skill_Control)sender;
			if (!CanClick)
			{
				return;
			}
			if (!((Control)SkillSelector).get_Visible() || SkillSelector.currentObject != control)
			{
				((Control)SkillSelector).set_Visible(true);
				SkillSelector.Skill_Control = control;
				((Control)SkillSelector).set_Location(((Control)control).get_Location().Add(new Point(-2, ((Control)control).get_Height())));
				List<API.Skill> Skills = new List<API.Skill>();
				if (Template.Build.Profession != null)
				{
					if (Template.Build.Profession.Id == "Revenant")
					{
						API.Legend legend = (control.Aquatic ? Template.Build.Legends_Aquatic[0] : Template.Build.Legends_Terrestrial[0]);
						if (legend != null && legend.Utilities != null)
						{
							switch (control.Slot)
							{
							case SkillSlots.Heal:
								Skills.Add(legend?.Heal);
								break;
							case SkillSlots.Elite:
								Skills.Add(legend?.Elite);
								break;
							default:
								foreach (API.Skill iSkill2 in legend?.Utilities)
								{
									Skills.Add(iSkill2);
								}
								break;
							}
						}
					}
					else
					{
						foreach (API.Skill iSkill in (from e in Template.Build.Profession.Skills
							orderby e.Specialization, (e.Categories.Count <= 0) ? "Unkown" : e.Categories[0]
							select e).ToList())
						{
							if (iSkill.Specialization != 0 && Template.Build.SpecLines.Find((SpecLine e) => e.Specialization != null && e.Specialization.Id == iSkill.Specialization) == null)
							{
								continue;
							}
							switch (control.Slot)
							{
							case SkillSlots.Heal:
								if (iSkill.Slot == API.skillSlot.Heal)
								{
									Skills.Add(iSkill);
								}
								break;
							case SkillSlots.Elite:
								if (iSkill.Slot == API.skillSlot.Elite)
								{
									Skills.Add(iSkill);
								}
								break;
							default:
								if (iSkill.Slot == API.skillSlot.Utility)
								{
									Skills.Add(iSkill);
								}
								break;
							}
						}
					}
				}
				SkillSelector.Skills = Skills;
				SkillSelector.Aquatic = control.Aquatic;
				SkillSelector.currentObject = control;
			}
			else
			{
				((Control)SkillSelector).set_Visible(false);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnClick(e);
			Rectangle rect0 = RectangleExtension.Scale(new Rectangle(new Point(36, 15), new Point(25, 25)), Scale);
			Rectangle rect1 = RectangleExtension.Scale(new Rectangle(new Point(_Width - (_Skills_Aquatic.Count * (_SkillSize + 1) + 28) + 63, 15), new Point(25, 25)), Scale);
			if (((Rectangle)(ref rect0)).Contains(((Control)this).get_RelativeMousePosition()) || ((Rectangle)(ref rect1)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				Template.Build?.SwapLegends();
				SetTemplate();
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			if (!CanClick)
			{
				CanClick = true;
			}
		}

		private void LegendSelector_SkillChanged(object sender, SkillChangedEvent e)
		{
			Skill_Control ctrl = e.Skill_Control;
			API.Legend legend = Template.Build.Legends_Terrestrial[0];
			if (ctrl == _Legends_Terrestrial[0])
			{
				Template.Build.Legends_Terrestrial[0] = Template.Profession.Legends.Find((API.Legend leg) => leg.Skill.Id == e.Skill.Id);
				legend = Template.Build.Legends_Terrestrial[0];
				if (legend != null)
				{
					Template.Build.Skills_Terrestrial[0] = legend.Heal;
					Template.Build.Skills_Terrestrial[1] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.Skills_Terrestrial[1]?.PaletteId);
					Template.Build.Skills_Terrestrial[2] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.Skills_Terrestrial[2]?.PaletteId);
					Template.Build.Skills_Terrestrial[3] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.Skills_Terrestrial[3]?.PaletteId);
					Template.Build.Skills_Terrestrial[4] = legend.Elite;
				}
			}
			else if (ctrl == _Legends_Terrestrial[1])
			{
				Template.Build.Legends_Terrestrial[1] = Template.Profession.Legends.Find((API.Legend leg) => leg.Skill.Id == e.Skill.Id);
				legend = Template.Build.Legends_Terrestrial[1];
				if (legend != null)
				{
					Template.Build.InactiveSkills_Terrestrial[0] = legend.Heal;
					Template.Build.InactiveSkills_Terrestrial[1] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.InactiveSkills_Terrestrial[1]?.PaletteId);
					Template.Build.InactiveSkills_Terrestrial[2] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.InactiveSkills_Terrestrial[2]?.PaletteId);
					Template.Build.InactiveSkills_Terrestrial[3] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.InactiveSkills_Terrestrial[3]?.PaletteId);
					Template.Build.InactiveSkills_Terrestrial[4] = legend.Elite;
				}
			}
			else if (ctrl == _Legends_Aquatic[0])
			{
				Template.Build.Legends_Aquatic[0] = Template.Profession.Legends.Find((API.Legend leg) => leg.Skill.Id == e.Skill.Id);
				legend = Template.Build.Legends_Aquatic[0];
				if (legend != null)
				{
					Template.Build.Skills_Aquatic[0] = legend.Heal;
					Template.Build.Skills_Aquatic[1] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.Skills_Aquatic[1]?.PaletteId);
					Template.Build.Skills_Aquatic[2] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.Skills_Aquatic[2]?.PaletteId);
					Template.Build.Skills_Aquatic[3] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.Skills_Aquatic[3]?.PaletteId);
					Template.Build.Skills_Aquatic[4] = legend.Elite;
				}
			}
			else if (ctrl == _Legends_Aquatic[1])
			{
				Template.Build.Legends_Aquatic[1] = Template.Profession.Legends.Find((API.Legend leg) => leg.Skill.Id == e.Skill.Id);
				legend = Template.Build.Legends_Aquatic[1];
				if (legend != null)
				{
					Template.Build.InactiveSkills_Aquatic[0] = legend.Heal;
					Template.Build.InactiveSkills_Aquatic[1] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.InactiveSkills_Aquatic[1]?.PaletteId);
					Template.Build.InactiveSkills_Aquatic[2] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.InactiveSkills_Aquatic[2]?.PaletteId);
					Template.Build.InactiveSkills_Aquatic[3] = legend.Utilities.Find((API.Skill skill) => skill.PaletteId == Template.Build.InactiveSkills_Aquatic[3]?.PaletteId);
					Template.Build.InactiveSkills_Aquatic[4] = legend.Elite;
				}
			}
			ctrl.Skill = e.Skill;
			SetTemplate();
			Template.SetChanged();
			((Control)LegendSelector).set_Visible(false);
			CanClick = false;
		}

		private void Legend(object sender, MouseEventArgs e)
		{
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			Skill_Control ctrl = (Skill_Control)sender;
			if (!(Template.Profession?.Id == "Revenant"))
			{
				return;
			}
			List<API.Skill> legends = new List<API.Skill>();
			foreach (API.Legend legend in Template.Profession.Legends)
			{
				if (legend.Specialization == 0 || Template.Specialization?.Id == legend.Specialization)
				{
					legends.Add(legend.Skill);
				}
			}
			LegendSelector.Skills = legends;
			((Control)LegendSelector).set_Visible(true);
			LegendSelector.Aquatic = false;
			LegendSelector.currentObject = ctrl;
			LegendSelector.Skill_Control = ctrl;
			((Control)LegendSelector).set_Location(((Control)ctrl).get_Location().Add(new Point(-2, (int)((double)((Control)ctrl).get_Height() * ctrl.Scale))));
			CanClick = false;
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
				foreach (Skill_Control skill_Control in _Skills_Terrestrial)
				{
					if (skill_Control.Skill == e.Skill)
					{
						skill_Control.Skill = null;
					}
				}
				for (int i = 0; i < Template.Build.Skills_Terrestrial.Count; i++)
				{
					if (Template.Build.Skills_Terrestrial[i] == e.Skill)
					{
						Template.Build.Skills_Terrestrial[i] = null;
					}
				}
				Template.Build.Skills_Terrestrial[(int)e.Skill_Control.Slot] = e.Skill;
				e.Skill_Control.Skill = e.Skill;
			}
			Template.SetChanged();
		}

		private void OnGlobalClick(object sender, MouseEventArgs m)
		{
			if (!((Control)SkillSelector).get_MouseOver())
			{
				((Control)SkillSelector).Hide();
			}
			if (!((Control)LegendSelector).get_MouseOver())
			{
				((Control)LegendSelector).Hide();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _AquaTexture, RectangleExtension.Scale(new Rectangle(new Point(0, 50), new Point(25, 25)), Scale), (Rectangle?)_AquaTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			if (ShowProfessionSkills)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _SwapTexture, RectangleExtension.Scale(new Rectangle(new Point(63, 15), new Point(25, 25)), Scale), (Rectangle?)_SwapTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _TerrestrialTexture, RectangleExtension.Scale(new Rectangle(new Point(_Width - (_Skills_Aquatic.Count * (_SkillSize + 1) + 28), 50), new Point(25, 25)), Scale), (Rectangle?)_TerrestrialTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			if (ShowProfessionSkills)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _SwapTexture, RectangleExtension.Scale(new Rectangle(new Point(_Width - (_Skills_Aquatic.Count * (_SkillSize + 1) + 28) + 63, 15), new Point(25, 25)), Scale), (Rectangle?)_SwapTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		protected override void DisposeControl()
		{
			BuildsManager.s_moduleInstance.Selected_Template_Changed -= ApplyBuild;
			LegendSelector.SkillChanged -= LegendSelector_SkillChanged;
			SkillSelector.SkillChanged -= OnSkillChanged;
			Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGlobalClick);
			((Control)_Legends_Terrestrial[0]).remove_Click((EventHandler<MouseEventArgs>)Legend);
			((Control)_Legends_Terrestrial[1]).remove_Click((EventHandler<MouseEventArgs>)Legend);
			((Control)_Legends_Aquatic[0]).remove_Click((EventHandler<MouseEventArgs>)Legend);
			((Control)_Legends_Aquatic[1]).remove_Click((EventHandler<MouseEventArgs>)Legend);
			foreach (Skill_Control item in _Skills_Terrestrial)
			{
				((Control)item).remove_Click((EventHandler<MouseEventArgs>)Control_Click);
			}
			foreach (Skill_Control item2 in _Skills_Aquatic)
			{
				((Control)item2).remove_Click((EventHandler<MouseEventArgs>)Control_Click);
			}
			((IEnumerable<IDisposable>)_Skills_Terrestrial).DisposeAll();
			((IEnumerable<IDisposable>)_Skills_Aquatic).DisposeAll();
			((Control)CustomTooltip).Dispose();
			((Control)this).DisposeControl();
		}

		public void SetTemplate()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			ShowProfessionSkills = Template.Profession?.Id == "Revenant";
			if (!ShowProfessionSkills)
			{
				((Control)this).set_Location(_OGLocation.Add(new Point(0, -16)));
			}
			else
			{
				((Control)this).set_Location(_OGLocation);
			}
			int i = 0;
			foreach (Skill_Control item in _Skills_Aquatic)
			{
				item.Skill = Template.Build.Skills_Aquatic[i];
				((Control)item).set_Location(new Point(((Control)item).get_Location().X, ShowProfessionSkills ? 51 : 30));
				i++;
			}
			i = 0;
			foreach (Skill_Control item2 in _Skills_Terrestrial)
			{
				item2.Skill = Template.Build.Skills_Terrestrial[i];
				((Control)item2).set_Location(new Point(((Control)item2).get_Location().X, ShowProfessionSkills ? 51 : 30));
				i++;
			}
			_Legends_Terrestrial[0].Skill = Template.Build.Legends_Terrestrial[0]?.Skill;
			_Legends_Terrestrial[1].Skill = Template.Build.Legends_Terrestrial[1]?.Skill;
			_Legends_Aquatic[0].Skill = Template.Build.Legends_Aquatic[0]?.Skill;
			_Legends_Aquatic[1].Skill = Template.Build.Legends_Aquatic[1]?.Skill;
			((Control)_Legends_Terrestrial[0]).set_Visible(ShowProfessionSkills);
			((Control)_Legends_Terrestrial[1]).set_Visible(ShowProfessionSkills);
			((Control)_Legends_Aquatic[0]).set_Visible(ShowProfessionSkills);
			((Control)_Legends_Aquatic[1]).set_Visible(ShowProfessionSkills);
		}
	}
}
