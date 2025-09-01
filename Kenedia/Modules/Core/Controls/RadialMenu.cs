using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
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

		public BasicEffect Effect { get; protected set; }

		public GraphicsDevice GraphicsDevice { get; protected set; }

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
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			GraphicsDevice = GameService.Graphics.LendGraphicsDeviceContext().GraphicsDevice;
			BasicEffect val = new BasicEffect(GraphicsDevice);
			val.set_VertexColorEnabled(true);
			Viewport viewport = GraphicsDevice.get_Viewport();
			float num = ((Viewport)(ref viewport)).get_Width();
			viewport = GraphicsDevice.get_Viewport();
			val.set_Projection(Matrix.CreateOrthographicOffCenter(0f, num, (float)((Viewport)(ref viewport)).get_Height(), 0f, 0f, 1f));
			Effect = val;
			DpiScale = (float)GraphicsDevice.get_PresentationParameters().get_BackBufferWidth() / (float)GameService.Graphics.SpriteScreen.Size.X;
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

		private void DrawRadialBorder(Vector2 Center, float innerR, float outerR, float angle, float thickness, Color color)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0416: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_052a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_0548: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0552: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_056f: Unknown result type (might be due to invalid IL or missing references)
			//IL_057d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_058b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05df: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0600: Unknown result type (might be due to invalid IL or missing references)
			//IL_0607: Unknown result type (might be due to invalid IL or missing references)
			//IL_060c: Unknown result type (might be due to invalid IL or missing references)
			//IL_060d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0612: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Unknown result type (might be due to invalid IL or missing references)
			//IL_063b: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06db: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0706: Unknown result type (might be due to invalid IL or missing references)
			//IL_070b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0710: Unknown result type (might be due to invalid IL or missing references)
			//IL_0713: Unknown result type (might be due to invalid IL or missing references)
			//IL_072a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0731: Unknown result type (might be due to invalid IL or missing references)
			//IL_0736: Unknown result type (might be due to invalid IL or missing references)
			//IL_073b: Unknown result type (might be due to invalid IL or missing references)
			//IL_073e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0755: Unknown result type (might be due to invalid IL or missing references)
			//IL_075c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0761: Unknown result type (might be due to invalid IL or missing references)
			//IL_0766: Unknown result type (might be due to invalid IL or missing references)
			//IL_0771: Unknown result type (might be due to invalid IL or missing references)
			//IL_0778: Unknown result type (might be due to invalid IL or missing references)
			//IL_077d: Unknown result type (might be due to invalid IL or missing references)
			//IL_077e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0783: Unknown result type (might be due to invalid IL or missing references)
			//IL_0791: Unknown result type (might be due to invalid IL or missing references)
			//IL_0798: Unknown result type (might be due to invalid IL or missing references)
			//IL_079d: Unknown result type (might be due to invalid IL or missing references)
			//IL_079e: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_07be: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_07de: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0803: Unknown result type (might be due to invalid IL or missing references)
			//IL_0811: Unknown result type (might be due to invalid IL or missing references)
			//IL_0818: Unknown result type (might be due to invalid IL or missing references)
			//IL_081d: Unknown result type (might be due to invalid IL or missing references)
			//IL_081e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0823: Unknown result type (might be due to invalid IL or missing references)
			//IL_0847: Unknown result type (might be due to invalid IL or missing references)
			//IL_084c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0892: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_08da: Unknown result type (might be due to invalid IL or missing references)
			if (GraphicsDevice == null || Effect == null || segments < 2)
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
			for (int i = 0; i < segments; i++)
			{
				float a1 = startAngle + (float)i * angleStep;
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
			Enumerator enumerator = ((Effect)Effect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					GraphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, solidVerts, 0, segments * 2);
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
				for (int j = 0; j < segments; j++)
				{
					float a3 = startAngle + (float)j * angleStep;
					float a7 = a3 + angleStep;
					Vector2 i4 = Center + new Vector2((float)Math.Cos(a3), (float)Math.Sin(a3)) * solidRadius;
					Vector2 o3 = Center + new Vector2((float)Math.Cos(a3), (float)Math.Sin(a3)) * fadeRadius;
					Vector2 i8 = Center + new Vector2((float)Math.Cos(a7), (float)Math.Sin(a7)) * solidRadius;
					Vector2 o8 = Center + new Vector2((float)Math.Cos(a7), (float)Math.Sin(a7)) * fadeRadius;
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(i4, 0f), solidColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(o3, 0f), edgeFadeColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(o8, 0f), edgeFadeColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(i4, 0f), solidColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(o8, 0f), edgeFadeColor);
					fadeVerts[idx++] = new VertexPositionColor(new Vector3(i8, 0f), solidColor);
				}
				enumerator = ((Effect)Effect).get_CurrentTechnique().get_Passes().GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator)).MoveNext())
					{
						((Enumerator)(ref enumerator)).get_Current().Apply();
						GraphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, fadeVerts, 0, segments * 2);
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
					float a8 = a4 + angleStep;
					Vector2 i5 = Center + new Vector2((float)Math.Cos(a4), (float)Math.Sin(a4)) * innerBorderR2;
					Vector2 o4 = Center + new Vector2((float)Math.Cos(a4), (float)Math.Sin(a4)) * outerBorderR2;
					Vector2 i9 = Center + new Vector2((float)Math.Cos(a8), (float)Math.Sin(a8)) * innerBorderR2;
					Vector2 o7 = Center + new Vector2((float)Math.Cos(a8), (float)Math.Sin(a8)) * outerBorderR2;
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(i5, 0f), solidColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(o4, 0f), edgeFadeColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(o7, 0f), edgeFadeColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(i5, 0f), solidColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(o7, 0f), edgeFadeColor);
					borderVerts2[idx++] = new VertexPositionColor(new Vector3(i9, 0f), solidColor);
				}
				enumerator = ((Effect)Effect).get_CurrentTechnique().get_Passes().GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator)).MoveNext())
					{
						((Enumerator)(ref enumerator)).get_Current().Apply();
						GraphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, borderVerts2, 0, segments * 2);
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
				for (int k = 0; k < segments; k++)
				{
					float a2 = startAngle + (float)k * angleStep;
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
				enumerator = ((Effect)Effect).get_CurrentTechnique().get_Passes().GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator)).MoveNext())
					{
						((Enumerator)(ref enumerator)).get_Current().Apply();
						GraphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, borderVerts, 0, segments * 2);
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator)).Dispose();
				}
			}
			if (ShowSliceBorder)
			{
				DrawRadialBorder(Center, clearRadius + (ShowInnerBorder ? innerBorderThickness : 0f), fadeRadius, startAngle, SliceBorderThickness, solidColor);
				DrawRadialBorder(Center, clearRadius + (ShowInnerBorder ? innerBorderThickness : 0f), fadeRadius, endAngle, SliceBorderThickness, solidColor);
			}
		}
	}
}
