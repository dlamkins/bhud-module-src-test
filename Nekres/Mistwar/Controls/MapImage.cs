using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.Controls
{
	internal class MapImage : Container
	{
		public IEnumerable<WvwObjectiveEntity> WvwObjectives;

		protected AsyncTexture2D _texture;

		private SpriteEffects _spriteEffects;

		private Rectangle? _sourceRectangle;

		private Color _tint = Color.get_White();

		private float _grayscaleIntensity;

		private Effect _grayscaleEffect;

		private MapImageDynamic _dynamicLayer;

		public AsyncTexture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				((Control)this).SetProperty<AsyncTexture2D>(ref _texture, value, false, "Texture");
			}
		}

		public SpriteEffects SpriteEffects
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _spriteEffects;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<SpriteEffects>(ref _spriteEffects, value, false, "SpriteEffects");
			}
		}

		public Rectangle SourceRectangle
		{
			get
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				return (Rectangle)(((_003F?)_sourceRectangle) ?? _texture.get_Texture().get_Bounds());
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Rectangle?>(ref _sourceRectangle, (Rectangle?)value, false, "SourceRectangle");
			}
		}

		public Color Tint
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _tint;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _tint, value, false, "Tint");
			}
		}

		public float GrayscaleIntensity
		{
			get
			{
				return _grayscaleIntensity;
			}
			set
			{
				((Control)this).SetProperty<float>(ref _grayscaleIntensity, value, false, "GrayscaleIntensity");
				_grayscaleEffect.get_Parameters().get_Item("Intensity").SetValue(MathHelper.Clamp(value, 0f, 1f));
			}
		}

		public float ScaleRatio { get; private set; } = MathHelper.Clamp(MistwarModule.ModuleInstance.ScaleRatioSetting.get_Value() / 100f, 0f, 1f);


		public float TextureOpacity { get; private set; }

		public MapImage()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			Texture = new AsyncTexture2D();
			Rectangle bounds = _texture.get_Texture().get_Bounds();
			((Control)this).set_Size(((Rectangle)(ref bounds)).get_Size());
			if (_grayscaleEffect == null)
			{
				_grayscaleEffect = MistwarModule.ModuleInstance.ContentsManager.GetEffect<Effect>("effects\\grayscale.mgfx");
			}
			SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val.set_Effect(_grayscaleEffect);
			((Control)this)._spriteBatchParameters = val;
			_texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
			MapImageDynamic mapImageDynamic = new MapImageDynamic(this);
			((Control)mapImageDynamic).set_Size(((Control)this).get_Size());
			_dynamicLayer = mapImageDynamic;
			MistwarModule.ModuleInstance.ScaleRatioSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnScaleRatioChanged);
		}

		internal void SetOpacity(float opacity)
		{
			TextureOpacity = opacity;
			_grayscaleEffect.get_Parameters().get_Item("Opacity").SetValue(opacity);
		}

		private void OnScaleRatioChanged(object o, ValueChangedEventArgs<float> e)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			ScaleRatio = MathHelper.Clamp(e.get_NewValue() / 100f, 0f, 1f);
			if (_texture.get_HasTexture())
			{
				Rectangle bounds = _texture.get_Texture().get_Bounds();
				((Control)this).set_Size(PointExtensions.ResizeKeepAspect(((Rectangle)(ref bounds)).get_Size(), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width()), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height()), false));
				((Control)this).set_Location(new Point(((Control)((Control)this).get_Parent()).get_Size().X / 2 - ((Control)this).get_Size().X / 2, ((Control)((Control)this).get_Parent()).get_Size().Y / 2 - ((Control)this).get_Size().Y / 2));
				((Control)_dynamicLayer).set_Size(((Control)this).get_Size());
			}
		}

		private void Dispose()
		{
			MapImageDynamic dynamicLayer = _dynamicLayer;
			if (dynamicLayer != null)
			{
				((Control)dynamicLayer).Dispose();
			}
			Texture.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
			MistwarModule.ModuleInstance.ScaleRatioSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnScaleRatioChanged);
			((Control)this).Dispose();
		}

		private void OnTextureSwapped(object o, ValueChangedEventArgs<Texture2D> e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			SourceRectangle = e.get_NewValue().get_Bounds();
			Rectangle bounds = e.get_NewValue().get_Bounds();
			((Control)this).set_Size(PointExtensions.ResizeKeepAspect(((Rectangle)(ref bounds)).get_Size(), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width()), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height()), false));
			((Control)this).set_Location(new Point(((Control)((Control)this).get_Parent()).get_Size().X / 2 - ((Control)this).get_Size().X / 2, ((Control)((Control)this).get_Parent()).get_Size().Y / 2 - ((Control)this).get_Size().Y / 2));
			((Control)_dynamicLayer).set_Size(((Control)this).get_Size());
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			if (_texture.get_HasTexture() && WvwObjectives != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texture), bounds, (Rectangle?)SourceRectangle, _tint, 0f, Vector2.get_Zero(), _spriteEffects);
			}
		}
	}
}
