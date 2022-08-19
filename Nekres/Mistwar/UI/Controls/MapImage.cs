using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.UI.Controls
{
	internal class MapImage : Container
	{
		public IEnumerable<WvwObjectiveEntity> WvwObjectives;

		protected AsyncTexture2D _texture;

		private SpriteEffects _spriteEffects;

		private Rectangle? _sourceRectangle;

		private Color _tint = Color.get_White();

		private Effect _grayscaleEffect;

		private SpriteBatchParameters _grayscaleSpriteBatchParams;

		private BitmapFont _font;

		public float TextureOpacity { get; private set; }

		public float ScaleRatio { get; private set; } = MathHelper.Clamp(MistwarModule.ModuleInstance.ScaleRatioSetting.get_Value() / 100f, 0f, 1f);


		public AsyncTexture2D Texture
		{
			get
			{
				return _texture;
			}
			private init
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
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
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

		public MapImage()
			: this()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			_font = Control.get_Content().GetFont((FontFace)0, (FontSize)24, (FontStyle)0);
			((Control)this)._spriteBatchParameters = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			_grayscaleEffect = MistwarModule.ModuleInstance.ContentsManager.GetEffect<Effect>("effects\\grayscale.mgfx");
			SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val.set_Effect(_grayscaleEffect);
			_grayscaleSpriteBatchParams = val;
			Texture = new AsyncTexture2D();
			Texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
			MistwarModule.ModuleInstance.ScaleRatioSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnScaleRatioChanged);
		}

		public void Toggle(float tDuration = 0.1f, bool silent = false)
		{
			if (((Control)this)._visible)
			{
				((Control)this)._visible = false;
				if (silent)
				{
					((Control)this).Hide();
					return;
				}
				GameService.Content.PlaySoundEffectByName("window-close");
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MapImage>(this, (object)new
				{
					Opacity = 0f
				}, tDuration, 0f, true).OnComplete((Action)((Control)this).Hide);
				return;
			}
			((Control)this)._visible = true;
			((Control)this).Show();
			if (!silent)
			{
				GameService.Content.PlaySoundEffectByName("page-open-" + RandomUtil.GetRandom(1, 3));
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MapImage>(this, (object)new
				{
					Opacity = 1f
				}, 0.35f, 0f, true);
			}
		}

		internal void SetOpacity(float opacity)
		{
			TextureOpacity = opacity;
			_grayscaleEffect.get_Parameters().get_Item("Opacity").SetValue(opacity);
		}

		public void SetColorIntensity(float colorIntensity)
		{
			_grayscaleEffect.get_Parameters().get_Item("Intensity").SetValue(MathHelper.Clamp(colorIntensity, 0f, 1f));
		}

		private void OnScaleRatioChanged(object o, ValueChangedEventArgs<float> e)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			ScaleRatio = MathHelper.Clamp(e.get_NewValue() / 100f, 0f, 1f);
			if (_texture.get_HasTexture())
			{
				Rectangle bounds = _texture.get_Texture().get_Bounds();
				((Control)this).set_Size(PointExtensions.ResizeKeepAspect(((Rectangle)(ref bounds)).get_Size(), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width()), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height()), false));
				((Control)this).set_Location(new Point(((Control)((Control)this).get_Parent()).get_Size().X / 2 - ((Control)this).get_Size().X / 2, ((Control)((Control)this).get_Parent()).get_Size().Y / 2 - ((Control)this).get_Size().Y / 2));
			}
		}

		protected override void DisposeControl()
		{
			if (_texture != null)
			{
				_texture.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
				_texture.Dispose();
			}
			Effect grayscaleEffect = _grayscaleEffect;
			if (grayscaleEffect != null)
			{
				((GraphicsResource)grayscaleEffect).Dispose();
			}
			MistwarModule.ModuleInstance.ScaleRatioSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnScaleRatioChanged);
			((Container)this).DisposeControl();
		}

		private void OnTextureSwapped(object o, ValueChangedEventArgs<Texture2D> e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			SourceRectangle = e.get_NewValue().get_Bounds();
			Rectangle bounds = e.get_NewValue().get_Bounds();
			((Control)this).set_Size(PointExtensions.ResizeKeepAspect(((Rectangle)(ref bounds)).get_Size(), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width()), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height()), false));
			((Control)this).set_Location(new Point(((Control)((Control)this).get_Parent()).get_Size().X / 2 - ((Control)this).get_Size().X / 2, ((Control)((Control)this).get_Parent()).get_Size().Y / 2 - ((Control)this).get_Size().Y / 2));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Invalid comparison between Unknown and I4
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_Visible() || !_texture.get_HasTexture() || WvwObjectives == null)
			{
				return;
			}
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _grayscaleSpriteBatchParams);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texture), bounds, (Rectangle?)SourceRectangle, _tint, 0f, Vector2.get_Zero(), _spriteEffects);
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this)._spriteBatchParameters);
			float widthRatio = (float)bounds.Width / (float)SourceRectangle.Width;
			float heightRatio = (float)bounds.Height / (float)SourceRectangle.Height;
			if (MistwarModule.ModuleInstance.DrawSectorsSetting.get_Value())
			{
				foreach (WvwObjectiveEntity wvwObjective in WvwObjectives)
				{
					Color teamColor = wvwObjective.TeamColor.GetColorBlindType(MistwarModule.ModuleInstance.ColorTypeSetting.get_Value(), (int)(TextureOpacity * 255f));
					Vector2[] sectorBounds = wvwObjective.Bounds.Select((Func<Point, Vector2>)delegate(Point p)
					{
						//IL_0020: Unknown result type (might be due to invalid IL or missing references)
						//IL_002b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0030: Unknown result type (might be due to invalid IL or missing references)
						//IL_0035: Unknown result type (might be due to invalid IL or missing references)
						//IL_0036: Unknown result type (might be due to invalid IL or missing references)
						//IL_003d: Unknown result type (might be due to invalid IL or missing references)
						//IL_0044: Unknown result type (might be due to invalid IL or missing references)
						Point val = PointExtensions.ToBounds(new Point((int)(widthRatio * (float)p.X), (int)(heightRatio * (float)p.Y)), ((Control)this).get_AbsoluteBounds());
						return new Vector2((float)val.X, (float)val.Y);
					}).ToArray();
					ShapeExtensions.DrawPolygon(spriteBatch, new Vector2(0f, 0f), (IReadOnlyList<Vector2>)sectorBounds, teamColor, 3f, 0f);
				}
			}
			Rectangle dest = default(Rectangle);
			foreach (WvwObjectiveEntity objectiveEntity in WvwObjectives)
			{
				if (objectiveEntity.Icon != null && (MistwarModule.ModuleInstance.DrawRuinMapSetting.get_Value() || (int)objectiveEntity.Type != 6))
				{
					int width = (int)(ScaleRatio * (float)objectiveEntity.Icon.get_Width());
					int height = (int)(ScaleRatio * (float)objectiveEntity.Icon.get_Height());
					((Rectangle)(ref dest))._002Ector((int)(widthRatio * (float)objectiveEntity.Center.X), (int)(heightRatio * (float)objectiveEntity.Center.Y), width, height);
					spriteBatch.DrawWvwObjectiveOnCtrl((Control)(object)this, objectiveEntity, dest, 1f, _font);
				}
			}
		}
	}
}
