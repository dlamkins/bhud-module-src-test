using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class Skill_Control : Control
	{
		private API.Skill _Skill;

		private int _SkillSize = 55;

		public SkillSlots Slot;

		private Texture2D _SelectorTexture;

		private Texture2D _SelectorTextureHovered;

		private Texture2D _SkillPlaceHolder;

		public bool Aquatic;

		private CustomTooltip CustomTooltip;

		private double _Scale = 1.0;

		public Template Template => BuildsManager.ModuleInstance.Selected_Template;

		public API.Skill Skill
		{
			get
			{
				return _Skill;
			}
			set
			{
				_Skill = value;
			}
		}

		public double Scale
		{
			get
			{
				return _Scale;
			}
			set
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				_Scale = value;
				((Control)this).set_Location(((Control)this).get_Location().Scale(value));
			}
		}

		public Skill_Control(Container parent)
			: this()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			((Control)this).set_Size(new Point(_SkillSize, _SkillSize + 15));
			_SelectorTexture = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.SkillSelector), 0, 2, 64, 12);
			_SelectorTextureHovered = BuildsManager.TextureManager.getControlTexture(_Controls.SkillSelector_Hovered);
			_SkillPlaceHolder = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.PlaceHolder_Traitline), 0, 0, 128, 128);
			CustomTooltip customTooltip = new CustomTooltip(((Control)this).get_Parent());
			((Control)customTooltip).set_ClipsBounds(false);
			customTooltip.HeaderColor = new Color(255, 204, 119, 255);
			CustomTooltip = customTooltip;
			((Control)this).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				if (Skill != null && Skill.Id > 0)
				{
					((Control)CustomTooltip).set_Visible(true);
					CustomTooltip.Header = Skill.Name;
					CustomTooltip.Content = new List<string> { Skill.Description };
					CustomTooltip.CurrentObject = Skill;
				}
			});
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				((Control)CustomTooltip).set_Visible(false);
			});
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			Rectangle skillRect = ClassExtensions.Scale(new Rectangle(new Point(0, 12), new Point(((Control)this).get_Width(), ((Control)this).get_Height() - 12)), Scale);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, ((Control)this).get_MouseOver() ? _SelectorTextureHovered : _SelectorTexture, ClassExtensions.Scale(new Rectangle(new Point(0, 0), new Point(((Control)this).get_Width(), 12)), Scale), (Rectangle?)_SelectorTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (Skill != null && Skill.Icon != null && Skill.Icon.Texture != null) ? Skill.Icon.Texture : _SkillPlaceHolder, skillRect, (Rectangle?)((Skill != null && Skill.Icon != null && Skill.Icon.Texture != null) ? Skill.Icon.Texture.get_Bounds() : _SkillPlaceHolder.get_Bounds()), (Color)((Skill != null && Skill.Icon != null && Skill.Icon.Texture != null) ? Color.get_White() : new Color(0, 0, 0, 155)), 0f, default(Vector2), (SpriteEffects)0);
			if (((Control)this).get_MouseOver())
			{
				Color color = Color.get_Honeydew();
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref skillRect)).get_Left(), ((Rectangle)(ref skillRect)).get_Top(), skillRect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref skillRect)).get_Left(), ((Rectangle)(ref skillRect)).get_Top(), skillRect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref skillRect)).get_Left(), ((Rectangle)(ref skillRect)).get_Bottom() - 2, skillRect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref skillRect)).get_Left(), ((Rectangle)(ref skillRect)).get_Bottom() - 1, skillRect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref skillRect)).get_Left(), ((Rectangle)(ref skillRect)).get_Top(), 2, skillRect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref skillRect)).get_Left(), ((Rectangle)(ref skillRect)).get_Top(), 1, skillRect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref skillRect)).get_Right() - 2, ((Rectangle)(ref skillRect)).get_Top(), 2, skillRect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref skillRect)).get_Right() - 1, ((Rectangle)(ref skillRect)).get_Top(), 1, skillRect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			}
		}

		public void SetTemplate()
		{
		}
	}
}
