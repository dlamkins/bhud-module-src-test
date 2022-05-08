using System;
using System.Collections.Generic;
using System.Threading;
using Blish_HUD;
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
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			CustomTooltip customTooltip = new CustomTooltip(((Control)this).get_Parent());
			((Control)customTooltip).set_ClipsBounds(false);
			customTooltip.HeaderColor = new Color(255, 204, 119, 255);
			CustomTooltip = customTooltip;
			ContentService cnt = new ContentService();
			Font = cnt.GetFont((FontFace)0, (FontSize)18, (FontStyle)0);
			((Control)this).set_Size(new Point(20 + 4 * _SkillSize, _SkillSize * (int)Math.Ceiling((double)Skills.Count / 4.0)));
			((Control)this).set_ClipsBounds(false);
			_NoWaterTexture = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.NoWaterTexture), 16, 16, 96, 96);
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnGlobalClick);
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			((Control)CustomTooltip).Dispose();
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
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_054a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_0588: Unknown result type (might be due to invalid IL or missing references)
			//IL_058d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05da: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0656: Unknown result type (might be due to invalid IL or missing references)
			//IL_065b: Unknown result type (might be due to invalid IL or missing references)
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
			text = Skill_Control.Slot switch
			{
				SkillSlots.Heal => "Healing Skills", 
				SkillSlots.Elite => "Elite Skills", 
				_ => "Utility Skills", 
			};
			RectangleF sRect = Font.GetStringRectangle(text);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, Font, new Rectangle((bounds.Width - (int)sRect.Width) / 2, 0, (int)sRect.Width, 20), Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			((Control)CustomTooltip).set_Visible(false);
			foreach (SelectionSkill entry in _SelectionSkills)
			{
				bool noUnderwater = Aquatic && entry.Skill.Flags.Contains("NoUnderwater");
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, entry.Skill.Icon.Texture, entry.Bounds, (Rectangle?)entry.Skill.Icon.Texture.get_Bounds(), noUnderwater ? Color.get_Gray() : Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
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

		public void SetTemplate()
		{
		}
	}
}
