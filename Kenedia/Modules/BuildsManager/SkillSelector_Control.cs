using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager
{
	public class SkillSelector_Control : Control
	{
		private class SelectionSkill
		{
			public API.Skill Skill;

			public Rectangle Bounds;

			public Rectangle SelectorBounds;

			public bool Hovered;
		}

		public object currentObject;

		private CustomTooltip CustomTooltip;

		private Texture2D _NoWaterTexture;

		private int _SkillSize = 55;

		public Skill_Control Skill_Control;

		public List<API.Skill> _Skills = new List<API.Skill>();

		private List<SelectionSkill> _SelectionSkills = new List<SelectionSkill>();

		public bool Aquatic;

		private BitmapFont Font;

		public List<API.Skill> Skills
		{
			get
			{
				return _Skills;
			}
			set
			{
				_Skills = value;
				if (value == null)
				{
					return;
				}
				_SelectionSkills = new List<SelectionSkill>();
				foreach (API.Skill skill in value)
				{
					_SelectionSkills.Add(new SelectionSkill
					{
						Skill = skill
					});
				}
				UpdateLayout();
			}
		}

		public event EventHandler<SkillChangedEvent> SkillChanged;

		private void OnSkillChanged(API.Skill skill, Skill_Control skill_Control)
		{
			this.SkillChanged?.Invoke(this, new SkillChangedEvent(skill, skill_Control));
		}

		public SkillSelector_Control()
			: this()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			CustomTooltip customTooltip = new CustomTooltip(((Control)this).get_Parent());
			((Control)customTooltip).set_ClipsBounds(false);
			customTooltip.HeaderColor = new Color(255, 204, 119, 255);
			CustomTooltip = customTooltip;
			Font = GameService.Content.get_DefaultFont18();
			((Control)this).set_Size(new Point(20 + 4 * _SkillSize, _SkillSize * (int)Math.Ceiling((double)Skills.Count / 4.0)));
			((Control)this).set_ClipsBounds(false);
			_NoWaterTexture = Texture2DExtension.GetRegion(BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.NoWaterTexture), 16, 16, 96, 96);
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGlobalClick);
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			((Control)CustomTooltip).Dispose();
			Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGlobalClick);
		}

		private void OnGlobalClick(object sender, MouseEventArgs e)
		{
			if (!((Control)this).get_Visible())
			{
				return;
			}
			foreach (SelectionSkill entry in _SelectionSkills)
			{
				if (entry.Hovered && (!Aquatic || !entry.Skill.Flags.Contains("NoUnderwater")))
				{
					OnSkillChanged(entry.Skill, Skill_Control);
					((Control)CustomTooltip).Hide();
					((Control)this).Hide();
					Thread.Sleep(100);
					break;
				}
			}
		}

		private void UpdateLayout()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(20 + 4 * _SkillSize, 40 + _SkillSize * (int)Math.Ceiling((double)Skills.Count / 4.0)));
			int row = 0;
			int col = 0;
			Rectangle baseRect = default(Rectangle);
			((Rectangle)(ref baseRect))._002Ector(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height());
			Rectangle rect = default(Rectangle);
			foreach (SelectionSkill selectionSkill in _SelectionSkills)
			{
				((Rectangle)(ref rect))._002Ector(10 + col * _SkillSize, 30 + row * _SkillSize, _SkillSize, _SkillSize);
				if (!((Rectangle)(ref baseRect)).Contains(rect))
				{
					col = 0;
					row++;
					((Rectangle)(ref rect))._002Ector(10 + col * _SkillSize, 30 + row * _SkillSize, _SkillSize, _SkillSize);
				}
				selectionSkill.Bounds = rect;
				selectionSkill.Hovered = ((Rectangle)(ref selectionSkill.Bounds)).Contains(((Control)this).get_RelativeMousePosition());
				col++;
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_040f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_049d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0530: Unknown result type (might be due to invalid IL or missing references)
			//IL_0535: Unknown result type (might be due to invalid IL or missing references)
			//IL_053f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0580: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0615: Unknown result type (might be due to invalid IL or missing references)
			//IL_061a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_062a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0696: Unknown result type (might be due to invalid IL or missing references)
			//IL_069b: Unknown result type (might be due to invalid IL or missing references)
			UpdateLayout();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, (Rectangle?)bounds, new Color(0, 0, 0, 230), 0f, default(Vector2), (SpriteEffects)0);
			Color color = Color.get_Black();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			if (Skill_Control == null)
			{
				return;
			}
			string text = "";
			switch (Skill_Control.Slot)
			{
			case SkillSlots.Heal:
				text = "Healing Skills";
				break;
			case SkillSlots.Elite:
				text = "Elite Skills";
				break;
			case SkillSlots.AquaticLegend1:
			case SkillSlots.AquaticLegend2:
			case SkillSlots.TerrestrialLegend1:
			case SkillSlots.TerrestrialLegend2:
				text = "Legends";
				break;
			default:
				text = "Utility Skills";
				break;
			}
			RectangleF sRect = Font.GetStringRectangle(text);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, Font, new Rectangle((bounds.Width - (int)sRect.Width) / 2, 0, (int)sRect.Width, 20), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			((Control)CustomTooltip).set_Visible(false);
			foreach (SelectionSkill entry in _SelectionSkills)
			{
				if (entry.Skill != null)
				{
					bool noUnderwater = Aquatic && entry.Skill.Flags.Contains("NoUnderwater");
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(entry.Skill.Icon._AsyncTexture), entry.Bounds, (Rectangle?)entry.Skill.Icon._AsyncTexture.get_Texture().get_Bounds(), noUnderwater ? Color.get_Gray() : Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					if (noUnderwater)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _NoWaterTexture, entry.Bounds, (Rectangle?)_NoWaterTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					}
					if (!noUnderwater && entry.Hovered)
					{
						color = Color.get_Honeydew();
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref entry.Bounds)).get_Left(), ((Rectangle)(ref entry.Bounds)).get_Top(), entry.Bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref entry.Bounds)).get_Left(), ((Rectangle)(ref entry.Bounds)).get_Top(), entry.Bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref entry.Bounds)).get_Left(), ((Rectangle)(ref entry.Bounds)).get_Bottom() - 2, entry.Bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref entry.Bounds)).get_Left(), ((Rectangle)(ref entry.Bounds)).get_Bottom() - 1, entry.Bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref entry.Bounds)).get_Left(), ((Rectangle)(ref entry.Bounds)).get_Top(), 2, entry.Bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref entry.Bounds)).get_Left(), ((Rectangle)(ref entry.Bounds)).get_Top(), 1, entry.Bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref entry.Bounds)).get_Right() - 2, ((Rectangle)(ref entry.Bounds)).get_Top(), 2, entry.Bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref entry.Bounds)).get_Right() - 1, ((Rectangle)(ref entry.Bounds)).get_Top(), 1, entry.Bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
						CustomTooltip.CurrentObject = entry.Skill;
						CustomTooltip.Header = entry.Skill.Name;
						CustomTooltip.Content = new List<string> { entry.Skill.Description };
						CustomTooltip.HeaderColor = new Color(255, 204, 119, 255);
						((Control)CustomTooltip).set_Visible(true);
					}
				}
			}
		}
	}
}
