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


		public ColorGradient SliceBackground { get; set; } = new ColorGradient(Color.get_Black() * 0.5f);


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
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			SetGraphicDevice();
		}

		public void SetGraphicDevice()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			using GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
			DpiScale = (float)gdc.GraphicsDevice.get_PresentationParameters().get_BackBufferWidth() / (float)GameService.Graphics.SpriteScreen.Size.X;
			Radius = (int)((float)(Math.Min(base.Width, base.Height) / 2) * DpiScale);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			base.OnResized(e);
			Radius = (int)((float)(Math.Min(base.Width, base.Height) / 2) * DpiScale);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			float angleStep = (float)Math.PI * 2f / (float)Slices;
			float startOffset = -(float)Math.PI / 2f;
			for (int i = 0; i < Slices; i++)
			{
				float num = startOffset + (float)i * angleStep;
				float endAngle = startOffset + (float)(i + 1) * angleStep;
				_ = (num + endAngle) / 2f;
				Vector2 dir = MousePos - Center;
				float num2 = ((Vector2)(ref dir)).Length();
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
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			float scale = 64f / (float)_texture.Width;
			Vector2 val = center / DpiScale;
			Rectangle bounds = _texture.Bounds;
			Point size = ((Rectangle)(ref bounds)).get_Size();
			Vector2 relativeCenter = val - ((Point)(ref size)).ToVector2() * scale / 2f;
			spriteBatch.Draw((Texture2D)_texture, relativeCenter, (Rectangle?)_texture.Bounds, Color.get_White(), 0f, Vector2.get_Zero(), scale, (SpriteEffects)0, 1f);
		}

		public void SetCenter(Point position)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			this.SetLocation(position.X - base.Width / 2, position.Y - base.Height / 2);
			_ = ((Point)(ref position)).ToVector2() * DpiScale;
			Rectangle absoluteBounds = base.AbsoluteBounds;
			Point center = ((Rectangle)(ref absoluteBounds)).get_Center();
			Vector2 absBounds = ((Point)(ref center)).ToVector2() * DpiScale;
			Center = new Vector2(absBounds.X, absBounds.Y);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			Point position = GameService.Input.Mouse.Position;
			MousePos = ((Point)(ref position)).ToVector2() * DpiScale;
			DrawRadialMenu(spriteBatch);
		}

		private void DrawRadialMenu(SpriteBatch spriteBatch)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
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
				float num = ((Vector2)(ref dir)).Length();
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
				Color start = slice_background.Start;
				Color val = slice_background.Start;
				Color solidColor = start * ((float)(int)((Color)(ref val)).get_A() / 255f);
				Color end = slice_background.End;
				val = slice_background.End;
				DrawGradientDonutSlice(startAngle, endAngle, solidColor, end * ((float)(int)((Color)(ref val)).get_A() / 255f), DonutHolePercent, FadePercent, OuterBorderThickness, InerBorderThickness);
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
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			List<VertexPositionColor> verts = new List<VertexPositionColor>();
			Vector2 dir = default(Vector2);
			((Vector2)(ref dir))._002Ector((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 normal = new Vector2(0f - dir.Y, dir.X) * thickness;
			Vector2 i1 = Center + dir * innerR - normal;
			Vector2 i2 = Center + dir * innerR + normal;
			Vector2 o1 = Center + dir * outerR - normal;
			Vector2 o2 = Center + dir * outerR + normal;
			verts.Add(new VertexPositionColor(new Vector3(i1, 0f), color));
			verts.Add(new VertexPositionColor(new Vector3(o1, 0f), Color.get_Transparent()));
			verts.Add(new VertexPositionColor(new Vector3(o2, 0f), Color.get_Transparent()));
			verts.Add(new VertexPositionColor(new Vector3(i1, 0f), color));
			verts.Add(new VertexPositionColor(new Vector3(o2, 0f), Color.get_Transparent()));
			verts.Add(new VertexPositionColor(new Vector3(i2, 0f), color));
			Enumerator enumerator = ((Effect)Effect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					GraphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, verts.ToArray(), 0, verts.Count / 3);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		private void DrawGradientDonutSlice(float startAngle, float endAngle, Color solidColor, Color edgeFadeColor, float clearPercent = 0.2f, float fadePercent = 0.2f, float outerBorderThickness = 5f, float innerBorderThickness = 5f, int segments = 48)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_041e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_051e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_0549: Unknown result type (might be due to invalid IL or missing references)
			//IL_054e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0553: Unknown result type (might be due to invalid IL or missing references)
			//IL_0556: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0581: Unknown result type (might be due to invalid IL or missing references)
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_059f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05db: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0601: Unknown result type (might be due to invalid IL or missing references)
			//IL_0603: Unknown result type (might be due to invalid IL or missing references)
			//IL_0608: Unknown result type (might be due to invalid IL or missing references)
			//IL_0616: Unknown result type (might be due to invalid IL or missing references)
			//IL_061d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0622: Unknown result type (might be due to invalid IL or missing references)
			//IL_0623: Unknown result type (might be due to invalid IL or missing references)
			//IL_0628: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Unknown result type (might be due to invalid IL or missing references)
			//IL_063d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0642: Unknown result type (might be due to invalid IL or missing references)
			//IL_0644: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_0657: Unknown result type (might be due to invalid IL or missing references)
			//IL_065e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0663: Unknown result type (might be due to invalid IL or missing references)
			//IL_0664: Unknown result type (might be due to invalid IL or missing references)
			//IL_0669: Unknown result type (might be due to invalid IL or missing references)
			//IL_0688: Unknown result type (might be due to invalid IL or missing references)
			//IL_068d: Unknown result type (might be due to invalid IL or missing references)
			//IL_070c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0723: Unknown result type (might be due to invalid IL or missing references)
			//IL_072a: Unknown result type (might be due to invalid IL or missing references)
			//IL_072f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0734: Unknown result type (might be due to invalid IL or missing references)
			//IL_0737: Unknown result type (might be due to invalid IL or missing references)
			//IL_074e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0755: Unknown result type (might be due to invalid IL or missing references)
			//IL_075a: Unknown result type (might be due to invalid IL or missing references)
			//IL_075f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0762: Unknown result type (might be due to invalid IL or missing references)
			//IL_0779: Unknown result type (might be due to invalid IL or missing references)
			//IL_0780: Unknown result type (might be due to invalid IL or missing references)
			//IL_0785: Unknown result type (might be due to invalid IL or missing references)
			//IL_078a: Unknown result type (might be due to invalid IL or missing references)
			//IL_078d: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0800: Unknown result type (might be due to invalid IL or missing references)
			//IL_0807: Unknown result type (might be due to invalid IL or missing references)
			//IL_080c: Unknown result type (might be due to invalid IL or missing references)
			//IL_080d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0812: Unknown result type (might be due to invalid IL or missing references)
			//IL_0820: Unknown result type (might be due to invalid IL or missing references)
			//IL_0827: Unknown result type (might be due to invalid IL or missing references)
			//IL_082c: Unknown result type (might be due to invalid IL or missing references)
			//IL_082d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0832: Unknown result type (might be due to invalid IL or missing references)
			//IL_0840: Unknown result type (might be due to invalid IL or missing references)
			//IL_0847: Unknown result type (might be due to invalid IL or missing references)
			//IL_084c: Unknown result type (might be due to invalid IL or missing references)
			//IL_084d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0852: Unknown result type (might be due to invalid IL or missing references)
			//IL_0860: Unknown result type (might be due to invalid IL or missing references)
			//IL_0867: Unknown result type (might be due to invalid IL or missing references)
			//IL_086c: Unknown result type (might be due to invalid IL or missing references)
			//IL_086d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0872: Unknown result type (might be due to invalid IL or missing references)
			//IL_0891: Unknown result type (might be due to invalid IL or missing references)
			//IL_0896: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0904: Unknown result type (might be due to invalid IL or missing references)
			//IL_0925: Unknown result type (might be due to invalid IL or missing references)
			using GraphicsDeviceContext gdc = GameService.Graphics.LendGraphicsDeviceContext();
			GraphicsDevice graphicsDevice = gdc.GraphicsDevice;
			BasicEffect val = new BasicEffect(graphicsDevice);
			val.set_VertexColorEnabled(true);
			Viewport viewport = graphicsDevice.get_Viewport();
			float num = ((Viewport)(ref viewport)).get_Width();
			viewport = graphicsDevice.get_Viewport();
			val.set_Projection(Matrix.CreateOrthographicOffCenter(0f, num, (float)((Viewport)(ref viewport)).get_Height(), 0f, 0f, 1f));
			BasicEffect effect = val;
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
			VertexPositionColor[] solidVerts = (VertexPositionColor[])(object)new VertexPositionColor[segments * 6];
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
			Enumerator enumerator = ((Effect)effect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, solidVerts, 0, segments * 2);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
			if (fadePercent > 0f)
			{
				VertexPositionColor[] fadeVerts = (VertexPositionColor[])(object)new VertexPositionColor[segments * 6];
				idx = 0;
				for (int k = 0; k < segments; k++)
				{
					float a3 = startAngle + (float)k * angleStep;
					float a8 = a3 + angleStep;
					Vector2 i5 = Center + new Vector2((float)Math.Cos(a3), (float)Math.Sin(a3)) * solidRadius;
					Vector2 o4 = Center + new Vector2((float)Math.Cos(a3), (float)Math.Sin(a3)) * fadeRadius;
					Vector2 i9 = Center + new Vector2((float)Math.Cos(a8), (float)Math.Sin(a8)) * solidRadius;
					Vector2 o8 = Center + new Vector2((float)Math.Cos(a8), (float)Math.Sin(a8)) * fadeRadius;
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(i5, 0f), solidColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(o4, 0f), edgeFadeColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(o8, 0f), edgeFadeColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(i5, 0f), solidColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(o8, 0f), edgeFadeColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(i9, 0f), solidColor);
				}
				enumerator = ((Effect)effect).get_CurrentTechnique().get_Passes().GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator)).MoveNext())
					{
						((Enumerator)(ref enumerator)).get_Current().Apply();
						graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, fadeVerts, 0, segments * 2);
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator)).Dispose();
				}
			}
			if (ShowOuterBorder && outerBorderThickness > 0f)
			{
				VertexPositionColor[] borderVerts2 = (VertexPositionColor[])(object)new VertexPositionColor[segments * 6];
				idx = 0;
				float innerBorderR2 = (float)Radius - outerBorderThickness;
				float outerBorderR2 = Radius;
				for (int l = 0; l < segments; l++)
				{
					float a4 = startAngle + (float)l * angleStep;
					float a7 = a4 + angleStep;
					Vector2 i4 = Center + new Vector2((float)Math.Cos(a4), (float)Math.Sin(a4)) * innerBorderR2;
					Vector2 o3 = Center + new Vector2((float)Math.Cos(a4), (float)Math.Sin(a4)) * outerBorderR2;
					Vector2 i8 = Center + new Vector2((float)Math.Cos(a7), (float)Math.Sin(a7)) * innerBorderR2;
					Vector2 o7 = Center + new Vector2((float)Math.Cos(a7), (float)Math.Sin(a7)) * outerBorderR2;
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(i4, 0f), solidColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(o3, 0f), edgeFadeColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(o7, 0f), edgeFadeColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(i4, 0f), solidColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(o7, 0f), edgeFadeColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(i8, 0f), solidColor);
				}
				enumerator = ((Effect)effect).get_CurrentTechnique().get_Passes().GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator)).MoveNext())
					{
						((Enumerator)(ref enumerator)).get_Current().Apply();
						graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, borderVerts2, 0, segments * 2);
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator)).Dispose();
				}
			}
			if (ShowInnerBorder && innerBorderThickness > 0f)
			{
				VertexPositionColor[] borderVerts = (VertexPositionColor[])(object)new VertexPositionColor[segments * 6];
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
				enumerator = ((Effect)effect).get_CurrentTechnique().get_Passes().GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator)).MoveNext())
					{
						((Enumerator)(ref enumerator)).get_Current().Apply();
						graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, borderVerts, 0, segments * 2);
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator)).Dispose();
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
