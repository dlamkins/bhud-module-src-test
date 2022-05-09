using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager
{
	public class Control_TemplateEntry : Control
	{
		public Template Template;

		public Control_Build Control_Build;

		private Texture2D _EmptyTraitLine;

		private Texture2D _Template_Border;

		private BitmapFont Font;

		private Control_TemplateTooltip TemplateTooltip;

		public event EventHandler<TemplateChangedEvent> TemplateChanged;

		public Control_TemplateEntry(Container parent, Template template)
			: this()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			((Control)this).set_Parent(parent);
			Template = template;
			_EmptyTraitLine = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.PlaceHolder_Traitline), 0, 0, 647, 136);
			_Template_Border = BuildsManager.TextureManager.getControlTexture(_Controls.Template_Border);
			ContentService cnt = new ContentService();
			Font = cnt.GetFont((FontFace)0, (FontSize)14, (FontStyle)0);
		}

		private void OnTemplateChangedEvent(Template template)
		{
			this.TemplateChanged?.Invoke(this, new TemplateChangedEvent(template));
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			OnTemplateChangedEvent(Template);
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			Control_Build control_Build = Control_Build;
			if (control_Build != null)
			{
				((Control)control_Build).Dispose();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			if (TemplateTooltip != null)
			{
				((Control)TemplateTooltip).set_Visible(((Control)this).get_MouseOver());
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _Template_Border, bounds, (Rectangle?)_Template_Border.get_Bounds(), ((Control)this).get_MouseOver() ? Color.get_Gray() : Color.get_Gray(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, (Rectangle?)bounds, (BuildsManager.ModuleInstance.Selected_Template == Template) ? new Color(0, 0, 0, 200) : new Color(0, 0, 0, 145), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			Texture2D texture = _EmptyTraitLine;
			if (Template.Specialization != null)
			{
				texture = Template.Specialization.ProfessionIconBig.Texture;
			}
			else if (Template.Build.Profession != null)
			{
				texture = Template.Build.Profession.IconBig.Texture;
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, new Rectangle(2, 2, bounds.Height - 4, bounds.Height - 4), (Rectangle?)texture.get_Bounds(), Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			Rectangle textBounds = default(Rectangle);
			((Rectangle)(ref textBounds))._002Ector(bounds.X + bounds.Height + 5, bounds.Y, bounds.Width - (bounds.Height + 5), bounds.Height);
			Rectangle rect = Font.CalculateTextRectangle(Template.Name, textBounds);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Template.Name, Font, textBounds, (BuildsManager.ModuleInstance.Selected_Template == Template) ? Color.get_LimeGreen() : (((Control)this).get_MouseOver() ? Color.get_White() : Color.get_LightGray()), true, (HorizontalAlignment)0, (VerticalAlignment)((rect.Height <= textBounds.Height) ? 1 : 0));
			Color color = (((Control)this).get_MouseOver() ? Color.get_Honeydew() : Color.get_Transparent());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
		}
	}
}
