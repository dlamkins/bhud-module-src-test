using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Enums;
using Kenedia.Modules.BuildsManager.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager.Controls
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

		private readonly CustomTooltip CustomTooltip;

		private readonly Texture2D _NoWaterTexture;

		private readonly int _SkillSize = 55;

		public Skill_Control Skill_Control;

		public List<API.Skill> _Skills = new List<API.Skill>();

		private List<SelectionSkill> _SelectionSkills = new List<SelectionSkill>();

		public bool Aquatic;

		private readonly BitmapFont Font;

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
			_NoWaterTexture = Texture2DExtension.GetRegion(BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.NoWaterTexture), 16, 16, 96, 96);
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
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_0406: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_0536: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_0546: Unknown result type (might be due to invalid IL or missing references)
			//IL_057c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0581: Unknown result type (might be due to invalid IL or missing references)
			//IL_058b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0616: Unknown result type (might be due to invalid IL or missing references)
			//IL_061b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0625: Unknown result type (might be due to invalid IL or missing references)
			//IL_062b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0697: Unknown result type (might be due to invalid IL or missing references)
			//IL_069c: Unknown result type (might be due to invalid IL or missing references)
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
			string text2;
			switch (Skill_Control.Slot)
			{
			case SkillSlots.Heal:
				text2 = "Healing Skills";
				break;
			case SkillSlots.Elite:
				text2 = "Elite Skills";
				break;
			case SkillSlots.AquaticLegend1:
			case SkillSlots.AquaticLegend2:
			case SkillSlots.TerrestrialLegend1:
			case SkillSlots.TerrestrialLegend2:
				text2 = "Legends";
				break;
			default:
				text2 = "Utility Skills";
				break;
			}
			string text = text2;
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
						CustomTooltip.TooltipContent = new List<string> { entry.Skill.Description };
						CustomTooltip.HeaderColor = new Color(255, 204, 119, 255);
						((Control)CustomTooltip).set_Visible(true);
					}
				}
			}
		}
	}
}
