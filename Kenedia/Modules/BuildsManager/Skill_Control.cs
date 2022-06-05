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
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				_Scale = value;
				((Control)this).set_Size(ClassExtensions.Scale(new Point(_SkillSize, _SkillSize + 15), value));
				((Control)this).set_Location(((Control)this).get_Location().Scale(value));
			}
		}

		public Skill_Control(Container parent)
			: this()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			((Control)this).set_Size(new Point(_SkillSize, _SkillSize + 15));
			_SelectorTexture = Texture2DExtension.GetRegion(BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.SkillSelector), 0, 2, 64, 12);
			_SelectorTextureHovered = BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.SkillSelector_Hovered);
			_SkillPlaceHolder = Texture2DExtension.GetRegion(BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.PlaceHolder_Traitline), 0, 0, 128, 128);
			CustomTooltip customTooltip = new CustomTooltip(((Control)this).get_Parent());
			((Control)customTooltip).set_ClipsBounds(false);
			customTooltip.HeaderColor = new Color(255, 204, 119, 255);
			CustomTooltip = customTooltip;
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			if (Skill != null && Skill.Id > 0)
			{
				((Control)CustomTooltip).set_Visible(true);
				CustomTooltip.Header = Skill.Name;
				CustomTooltip.Content = new List<string> { Skill.Description };
				CustomTooltip.CurrentObject = Skill;
			}
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			((Control)CustomTooltip).set_Visible(false);
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
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			Rectangle skillRect = ClassExtensions.Scale(new Rectangle(new Point(0, 12), new Point(((Control)this).get_Width(), ((Control)this).get_Height() - 12)), Scale);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, ((Control)this).get_MouseOver() ? _SelectorTextureHovered : _SelectorTexture, ClassExtensions.Scale(new Rectangle(new Point(0, 0), new Point(((Control)this).get_Width(), 12)), Scale), (Rectangle?)_SelectorTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, (Skill != null && Skill.Icon != null && Skill.Icon._AsyncTexture != null) ? Skill.Icon._AsyncTexture.get_Texture() : _SkillPlaceHolder, skillRect, (Rectangle?)((Skill != null && Skill.Icon != null && Skill.Icon._AsyncTexture != null) ? Skill.Icon._AsyncTexture.get_Texture().get_Bounds() : _SkillPlaceHolder.get_Bounds()), (Color)((Skill != null && Skill.Icon != null && Skill.Icon._AsyncTexture != null) ? Color.get_White() : new Color(0, 0, 0, 155)), 0f, default(Vector2), (SpriteEffects)0);
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

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			_SelectorTexture = null;
			_SelectorTextureHovered = null;
			_SkillPlaceHolder = null;
		}
	}
}
