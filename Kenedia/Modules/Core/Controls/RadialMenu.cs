using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class RadialMenu : Control
	{
		private AsyncTexture2D _texture = AsyncTexture2D.FromAssetId(102804);

		public float DpiScale { get; protected set; }

		public Vector2 Center { get; protected set; }

		public Vector2 MousePos { get; protected set; }

		public int Radius { get; protected set; }

		public int Slices { get; set; } = 8;


		public ColorGradient SliceBackground { get; set; } = new ColorGradient(Color.Black * 0.5f);


		public ColorGradient SliceHighlight { get; set; } = new ColorGradient(ContentService.Colors.ColonialWhite * 0.5f);


		public bool ShowInnerBorder { get; set; } = true;


		public bool ShowSliceBorder { get; set; } = true;


		public bool ShowOuterBorder { get; set; }

		public float DonutHolePercent { get; set; } = 0.4f;


		public float FadePercent { get; set; } = 0.2f;


		public float OuterBorderThickness { get; set; } = 5f;


		public float InerBorderThickness { get; set; } = 5f;


		public float SliceBorderThickness { get; set; } = 2f;


		public RadialMenu()
		{
			SetGraphicDevice();
		}

		public void SetGraphicDevice()
		{
			using GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
			DpiScale = (float)gdc.GraphicsDevice.PresentationParameters.BackBufferWidth / (float)GameService.Graphics.SpriteScreen.Size.X;
			Radius = (int)((float)(Math.Min(base.Width, base.Height) / 2) * DpiScale);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			base.OnResized(e);
			Radius = (int)((float)(Math.Min(base.Width, base.Height) / 2) * DpiScale);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			float angleStep = (float)Math.PI * 2f / (float)Slices;
			float startOffset = -(float)Math.PI / 2f;
			for (int i = 0; i < Slices; i++)
			{
				float num = startOffset + (float)i * angleStep;
				float endAngle = startOffset + (float)(i + 1) * angleStep;
				_ = (num + endAngle) / 2f;
				Vector2 dir = MousePos - Center;
				float num2 = dir.Length();
				float angle = (float)Math.Atan2(dir.Y, dir.X) - startOffset;
				if (angle < 0f)
				{
					angle += (float)Math.PI * 2f;
				}
				float sliceStart = (float)i * angleStep;
				float sliceEnd = (float)(i + 1) * angleStep;
				if (num2 <= (float)Radius && angle >= sliceStart && angle <= sliceEnd)
				{
					OnSliceClick(i);
				}
			}
		}

		protected virtual void OnSliceClick(int i)
		{
		}

		protected virtual void DrawSliceContent(SpriteBatch spriteBatch, bool contains_mouse, Vector2 center, float midAngle, float iconRadius, int sliceIndex)
		{
			float scale = 64f / (float)_texture.Width;
			Vector2 relativeCenter = center / DpiScale - _texture.Bounds.Size.ToVector2() * scale / 2f;
			spriteBatch.Draw(_texture, relativeCenter, _texture.Bounds, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
		}

		public void SetCenter(Point position)
		{
			this.SetLocation(position.X - base.Width / 2, position.Y - base.Height / 2);
			_ = position.ToVector2() * DpiScale;
			Vector2 absBounds = base.AbsoluteBounds.Center.ToVector2() * DpiScale;
			Center = new Vector2(absBounds.X, absBounds.Y);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			MousePos = GameService.Input.Mouse.Position.ToVector2() * DpiScale;
			DrawRadialMenu(spriteBatch);
		}

		private void DrawRadialMenu(SpriteBatch spriteBatch)
		{
			if (Slices == 0)
			{
				return;
			}
			float angleStep = (float)Math.PI * 2f / (float)Slices;
			float startOffset = -(float)Math.PI / 2f;
			bool any_contains_mouse = false;
			for (int i = 0; i < Slices; i++)
			{
				float startAngle = startOffset + (float)i * angleStep;
				float endAngle = startOffset + (float)(i + 1) * angleStep;
				float midAngle = (startAngle + endAngle) / 2f;
				Vector2 dir = MousePos - Center;
				float num = dir.Length();
				float angle = (float)Math.Atan2(dir.Y, dir.X) - startOffset;
				if (angle < 0f)
				{
					angle += (float)Math.PI * 2f;
				}
				float sliceStart = (float)i * angleStep;
				float sliceEnd = (float)(i + 1) * angleStep;
				bool contains_mouse = num <= (float)Radius && angle >= sliceStart && angle <= sliceEnd && !any_contains_mouse;
				any_contains_mouse = any_contains_mouse || contains_mouse;
				ColorGradient slice_background = GetSliceColors(i, contains_mouse);
				float innerRadius = (float)Radius * DonutHolePercent;
				Vector2 slice_center = Center + new Vector2((float)Math.Cos(midAngle), (float)Math.Sin(midAngle)) * (innerRadius + ((float)Radius - innerRadius) / 2f);
				DrawGradientDonutSlice(startAngle, endAngle, slice_background.Start * ((float)(int)slice_background.Start.A / 255f), slice_background.End * ((float)(int)slice_background.End.A / 255f), DonutHolePercent, FadePercent, OuterBorderThickness, InerBorderThickness);
				DrawSliceContent(spriteBatch, contains_mouse, slice_center, midAngle, innerRadius, i);
			}
		}

		protected virtual ColorGradient GetSliceColors(int index, bool contains_mouse)
		{
			if (!contains_mouse)
			{
				return SliceBackground;
			}
			return SliceHighlight;
		}

		private void DrawRadialBorder(GraphicsDevice GraphicsDevice, BasicEffect Effect, Vector2 Center, float innerR, float outerR, float angle, float thickness, Color color)
		{
			List<VertexPositionColor> verts = new List<VertexPositionColor>();
			Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 normal = new Vector2(0f - dir.Y, dir.X) * thickness;
			Vector2 i1 = Center + dir * innerR - normal;
			Vector2 i2 = Center + dir * innerR + normal;
			Vector2 o1 = Center + dir * outerR - normal;
			Vector2 o2 = Center + dir * outerR + normal;
			verts.Add(new VertexPositionColor(new Vector3(i1, 0f), color));
			verts.Add(new VertexPositionColor(new Vector3(o1, 0f), Color.Transparent));
			verts.Add(new VertexPositionColor(new Vector3(o2, 0f), Color.Transparent));
			verts.Add(new VertexPositionColor(new Vector3(i1, 0f), color));
			verts.Add(new VertexPositionColor(new Vector3(o2, 0f), Color.Transparent));
			verts.Add(new VertexPositionColor(new Vector3(i2, 0f), color));
			foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, verts.ToArray(), 0, verts.Count / 3);
			}
		}

		private void DrawGradientDonutSlice(float startAngle, float endAngle, Color solidColor, Color edgeFadeColor, float clearPercent = 0.2f, float fadePercent = 0.2f, float outerBorderThickness = 5f, float innerBorderThickness = 5f, int segments = 48)
		{
			using GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
			GraphicsDevice graphicsDevice = gdc.GraphicsDevice;
			BasicEffect effect = new BasicEffect(graphicsDevice)
			{
				VertexColorEnabled = true,
				Projection = Matrix.CreateOrthographicOffCenter(0f, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, 0f, 0f, 1f)
			};
			if (graphicsDevice == null || effect == null || segments < 2)
			{
				return;
			}
			clearPercent = MathHelper.Clamp(clearPercent, 0f, 0.95f);
			fadePercent = MathHelper.Clamp(fadePercent, 0f, 1f);
			float clearRadius = (float)Radius * clearPercent;
			float solidRadius = (float)Radius * (1f - fadePercent);
			float fadeRadius = Radius;
			if (solidRadius < clearRadius)
			{
				solidRadius = clearRadius;
			}
			float angleStep = (endAngle - startAngle) / (float)segments;
			VertexPositionColor[] solidVerts = new VertexPositionColor[segments * 6];
			int idx = 0;
			for (int j = 0; j < segments; j++)
			{
				float a1 = startAngle + (float)j * angleStep;
				float a5 = a1 + angleStep;
				Vector2 i2 = Center + new Vector2((float)Math.Cos(a1), (float)Math.Sin(a1)) * clearRadius;
				Vector2 o1 = Center + new Vector2((float)Math.Cos(a1), (float)Math.Sin(a1)) * solidRadius;
				Vector2 i6 = Center + new Vector2((float)Math.Cos(a5), (float)Math.Sin(a5)) * clearRadius;
				Vector2 o5 = Center + new Vector2((float)Math.Cos(a5), (float)Math.Sin(a5)) * solidRadius;
				solidVerts[idx++] = new VertexPositionColor(new Vector3(i2, 0f), solidColor);
				solidVerts[idx++] = new VertexPositionColor(new Vector3(o1, 0f), solidColor);
				solidVerts[idx++] = new VertexPositionColor(new Vector3(o5, 0f), solidColor);
				solidVerts[idx++] = new VertexPositionColor(new Vector3(i2, 0f), solidColor);
				solidVerts[idx++] = new VertexPositionColor(new Vector3(o5, 0f), solidColor);
				solidVerts[idx++] = new VertexPositionColor(new Vector3(i6, 0f), solidColor);
			}
			foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, solidVerts, 0, segments * 2);
			}
			if (fadePercent > 0f)
			{
				VertexPositionColor[] fadeVerts = new VertexPositionColor[segments * 6];
				idx = 0;
				for (int k = 0; k < segments; k++)
				{
					float a4 = startAngle + (float)k * angleStep;
					float a8 = a4 + angleStep;
					Vector2 i5 = Center + new Vector2((float)Math.Cos(a4), (float)Math.Sin(a4)) * solidRadius;
					Vector2 o4 = Center + new Vector2((float)Math.Cos(a4), (float)Math.Sin(a4)) * fadeRadius;
					Vector2 i9 = Center + new Vector2((float)Math.Cos(a8), (float)Math.Sin(a8)) * solidRadius;
					Vector2 o8 = Center + new Vector2((float)Math.Cos(a8), (float)Math.Sin(a8)) * fadeRadius;
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(i5, 0f), solidColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(o4, 0f), edgeFadeColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(o8, 0f), edgeFadeColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(i5, 0f), solidColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(o8, 0f), edgeFadeColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(i9, 0f), solidColor);
				}
				foreach (EffectPass pass2 in effect.CurrentTechnique.Passes)
				{
					pass2.Apply();
					graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, fadeVerts, 0, segments * 2);
				}
			}
			if (ShowOuterBorder && outerBorderThickness > 0f)
			{
				VertexPositionColor[] borderVerts2 = new VertexPositionColor[segments * 6];
				idx = 0;
				float innerBorderR2 = (float)Radius - outerBorderThickness;
				float outerBorderR2 = Radius;
				for (int l = 0; l < segments; l++)
				{
					float a3 = startAngle + (float)l * angleStep;
					float a7 = a3 + angleStep;
					Vector2 i4 = Center + new Vector2((float)Math.Cos(a3), (float)Math.Sin(a3)) * innerBorderR2;
					Vector2 o3 = Center + new Vector2((float)Math.Cos(a3), (float)Math.Sin(a3)) * outerBorderR2;
					Vector2 i8 = Center + new Vector2((float)Math.Cos(a7), (float)Math.Sin(a7)) * innerBorderR2;
					Vector2 o7 = Center + new Vector2((float)Math.Cos(a7), (float)Math.Sin(a7)) * outerBorderR2;
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(i4, 0f), solidColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(o3, 0f), edgeFadeColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(o7, 0f), edgeFadeColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(i4, 0f), solidColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(o7, 0f), edgeFadeColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(i8, 0f), solidColor);
				}
				foreach (EffectPass pass3 in effect.CurrentTechnique.Passes)
				{
					pass3.Apply();
					graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, borderVerts2, 0, segments * 2);
				}
			}
			if (ShowInnerBorder && innerBorderThickness > 0f)
			{
				VertexPositionColor[] borderVerts = new VertexPositionColor[segments * 6];
				idx = 0;
				float innerBorderR = clearRadius;
				float outerBorderR = clearRadius + innerBorderThickness;
				for (int i = 0; i < segments; i++)
				{
					float a2 = startAngle + (float)i * angleStep;
					float a6 = a2 + angleStep;
					Vector2 i3 = Center + new Vector2((float)Math.Cos(a2), (float)Math.Sin(a2)) * innerBorderR;
					Vector2 o2 = Center + new Vector2((float)Math.Cos(a2), (float)Math.Sin(a2)) * outerBorderR;
					Vector2 i7 = Center + new Vector2((float)Math.Cos(a6), (float)Math.Sin(a6)) * innerBorderR;
					Vector2 o6 = Center + new Vector2((float)Math.Cos(a6), (float)Math.Sin(a6)) * outerBorderR;
					borderVerts[idx++] = new VertexPositionColor(new Vector3(i3, 0f), solidColor);
					borderVerts[idx++] = new VertexPositionColor(new Vector3(o2, 0f), solidColor);
					borderVerts[idx++] = new VertexPositionColor(new Vector3(o6, 0f), solidColor);
					borderVerts[idx++] = new VertexPositionColor(new Vector3(i3, 0f), solidColor);
					borderVerts[idx++] = new VertexPositionColor(new Vector3(o6, 0f), solidColor);
					borderVerts[idx++] = new VertexPositionColor(new Vector3(i7, 0f), solidColor);
				}
				foreach (EffectPass pass4 in effect.CurrentTechnique.Passes)
				{
					pass4.Apply();
					graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, borderVerts, 0, segments * 2);
				}
			}
			if (ShowSliceBorder)
			{
				DrawRadialBorder(graphicsDevice, effect, Center, clearRadius + (ShowInnerBorder ? innerBorderThickness : 0f), fadeRadius, startAngle, SliceBorderThickness, solidColor);
				DrawRadialBorder(graphicsDevice, effect, Center, clearRadius + (ShowInnerBorder ? innerBorderThickness : 0f), fadeRadius, endAngle, SliceBorderThickness, solidColor);
			}
		}
	}
}
