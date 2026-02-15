using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModuleManagerPlus.Data;
using ModuleManagerPlus.Utility;

namespace ModuleManagerPlus.UI
{
	internal class ModuleCard : Container
	{
		private const int IMAGE_LENGTH = 315;

		private const int DESCRIPTION_HEIGHT = 99;

		private const int FOOTER_HEIGHT = 45;

		private const int INTERACT_HEIGHT = 27;

		private const int PADDING = 15;

		private Texture2D _backgroundMask;

		private Texture2D _avatarMask;

		private Texture2D _lightBackground;

		private Texture2D _defaultBackground;

		private Texture2D _selectedBackground;

		private Effect _maskEffect;

		private AsyncTexture2D _heroTexture;

		private AsyncTexture2D _authorTexture;

		private static readonly RasterizerState _scissorOn;

		public Module Model { get; set; }

		public ModuleCard(Module model, TextureLoader textureLoader)
			: this()
		{
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			Model = model;
			_maskEffect = ModuleManagerPlus.MaskEffect;
			_lightBackground = textureLoader.LoadTextureFromRef("textures/lightcarousel-tile_default.png");
			_backgroundMask = textureLoader.LoadTextureFromRef("textures/blackcarousel-tile_default.png");
			_avatarMask = textureLoader.LoadTextureFromRef("textures/avatar_mask.png");
			_defaultBackground = textureLoader.LoadTextureFromRef("textures/darkcarousel-tile_default.png");
			_selectedBackground = textureLoader.LoadTextureFromRef("textures/darkcarousel-tile_default.png");
			if (model.HeroUrl != null)
			{
				_heroTexture = textureLoader.LoadTextureFromWeb(model.HeroUrl);
			}
			_authorTexture = textureLoader.LoadTextureFromWeb(model.HeroUrl);
			((Control)this).set_Size(new Point(315, 459));
			((Control)this).set_BasicTooltipText(model.Description);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			Process.Start("https://blishhud.com/modules/?module=" + Model.Namespace);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			int offset = (((Control)this).get_MouseOver() ? (-2) : 0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, ((Control)this).get_MouseOver() ? _selectedBackground : _defaultBackground, ((Control)this).get_MouseOver() ? RectangleExtension.OffsetBy(bounds, 0, offset) : bounds, Color.get_White() * (((Control)this).get_MouseOver() ? 0.4f : 0.8f));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Model.Name, GameService.Content.get_DefaultFont18(), new Rectangle(15, 330 + offset, 315, 15), Color.get_White(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			spriteBatch.DrawStringOnCtrl((Control)(object)this, Model.Description, GameService.Content.get_DefaultFont14(), new Rectangle(15, 355 + offset, 285, 54), Color.get_LightGray(), wrap: true, stroke: false, 1, (HorizontalAlignment)0, (VerticalAlignment)0, (Rectangle?)new Rectangle(15, 355, 285, 54));
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Freesnow", GameService.Content.get_DefaultFont18(), new Rectangle(52, 414 + offset, ((Control)this).get_Width(), 30), Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			spriteBatch.End();
			spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.LinearWrap, (DepthStencilState)null, _scissorOn, _maskEffect, (Matrix?)GameService.Graphics.get_UIScaleTransform());
			if (_heroTexture != null && _heroTexture.get_HasTexture())
			{
				_maskEffect.get_Parameters().get_Item("Mask").SetValue((Texture)(object)_backgroundMask);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_heroTexture), new Rectangle(0, offset, 315, 315), Color.get_White());
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, new Rectangle(125, 125 + offset, 64, 64));
			}
			if (_authorTexture != null && _authorTexture.get_HasTexture())
			{
				_maskEffect.get_Parameters().get_Item("Mask").SetValue((Texture)(object)_avatarMask);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_authorTexture), new Rectangle(15, 414 + offset, 30, 30), Color.get_White());
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, new Rectangle(15, 414 + offset, 30, 30));
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
		}

		static ModuleCard()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			RasterizerState val = new RasterizerState();
			val.set_CullMode((CullMode)0);
			val.set_ScissorTestEnable(true);
			_scissorOn = val;
		}
	}
}
