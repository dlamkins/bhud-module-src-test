using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager
{
	public class Control_TemplateEntry : Control
	{
		public Template Template;

		private Texture2D _EmptyTraitLine;

		private Texture2D _Lock;

		private Texture2D _Template_Border;

		private BitmapFont Font;

		private BitmapFont FontItalic;

		private Control_TemplateTooltip TemplateTooltip;

		private double Tick;

		private string FeedbackPopup;

		public event EventHandler<TemplateChangedEvent> TemplateChanged;

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			if (FeedbackPopup != null)
			{
				Tick += gameTime.get_ElapsedGameTime().TotalMilliseconds;
				if (!(Tick < 350.0))
				{
					Tick = 0.0;
					FeedbackPopup = null;
				}
			}
		}

		public Control_TemplateEntry(Container parent, Template template)
			: this()
		{
			((Control)this).set_Parent(parent);
			Template = template;
			_EmptyTraitLine = Texture2DExtension.GetRegion(BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.PlaceHolder_Traitline), 0, 0, 647, 136);
			_Template_Border = BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.Template_Border);
			_Lock = BuildsManager.ModuleInstance.TextureManager.getIcon(_Icons.Lock_Locked);
			Font = GameService.Content.get_DefaultFont14();
			FontItalic = GameService.Content.GetFont((FontFace)0, (FontSize)14, (FontStyle)1);
		}

		private void OnTemplateChangedEvent(Template template)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			string code = template.Build.ParseBuildCode();
			if (code != null && code != "" && ((Enum)Control.get_Input().get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)1))
			{
				try
				{
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(template.Build.ParseBuildCode());
					FeedbackPopup = "Copied Build Code!";
				}
				catch (ArgumentException)
				{
					ScreenNotification.ShowNotification("Failed to set the clipboard text!", (NotificationType)2, (Texture2D)null, 4);
				}
				catch
				{
				}
			}
			else
			{
				this.TemplateChanged?.Invoke(this, new TemplateChangedEvent(template));
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			OnTemplateChangedEvent(Template);
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			Control_TemplateTooltip templateTooltip = TemplateTooltip;
			if (templateTooltip != null)
			{
				((Control)templateTooltip).Dispose();
			}
			Template = null;
			_EmptyTraitLine = null;
			_Lock = null;
			_Template_Border = null;
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
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_0497: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			if (TemplateTooltip != null)
			{
				((Control)TemplateTooltip).set_Visible(((Control)this).get_MouseOver());
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _Template_Border, bounds, (Rectangle?)_Template_Border.get_Bounds(), ((Control)this).get_MouseOver() ? Color.get_Gray() : Color.get_Gray(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, (Rectangle?)bounds, (BuildsManager.ModuleInstance.Selected_Template == Template) ? new Color(0, 0, 0, 200) : new Color(0, 0, 0, 145), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			Texture2D texture = _EmptyTraitLine;
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			if (Template.Specialization != null)
			{
				texture = AsyncTexture2D.op_Implicit(Template.Specialization.ProfessionIconBig._AsyncTexture);
			}
			else if (Template.Build.Profession != null)
			{
				texture = AsyncTexture2D.op_Implicit(Template.Build.Profession.IconBig._AsyncTexture);
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, new Rectangle(2, 2, bounds.Height - 4, bounds.Height - 4), (Rectangle?)texture.get_Bounds(), (Template.Profession?.Id == player.get_Profession().ToString()) ? Color.get_White() : Color.get_LightGray(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			if (Template.Path == null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _Lock, new Rectangle(bounds.Height - 14, bounds.Height - 14, 12, 12), (Rectangle?)_Lock.get_Bounds(), new Color(183, 158, 117, 255), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			}
			Rectangle textBounds = default(Rectangle);
			((Rectangle)(ref textBounds))._002Ector(bounds.X + bounds.Height + 5, bounds.Y, bounds.Width - (bounds.Height + 5), bounds.Height);
			Rectangle popupBounds = default(Rectangle);
			((Rectangle)(ref popupBounds))._002Ector(bounds.X + bounds.Height - 10, bounds.Y, bounds.Width - (bounds.Height + 5), bounds.Height);
			Rectangle rect = Font.CalculateTextRectangle(Template.Name, textBounds);
			if (FeedbackPopup != null)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, FeedbackPopup, FontItalic, popupBounds, new Color(175, 175, 175, 125), false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			else
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Template.Name, Font, textBounds, (BuildsManager.ModuleInstance.Selected_Template == Template) ? Color.get_LimeGreen() : (((Control)this).get_MouseOver() ? Color.get_White() : ((Template.Profession?.Id == player.get_Profession().ToString()) ? Color.get_LightGray() : Color.get_Gray())), true, (HorizontalAlignment)0, (VerticalAlignment)((rect.Height <= textBounds.Height) ? 1 : 0));
			}
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