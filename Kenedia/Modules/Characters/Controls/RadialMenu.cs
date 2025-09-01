using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Extensions;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class RadialMenu : Control
	{
		private readonly Data _data;

		private readonly AsyncTexture2D _dummy = AsyncTexture2D.FromAssetId(1128572);

		private readonly CharacterTooltip _tooltip;

		private readonly Settings _settings;

		private readonly ObservableCollection<Character_Model> _characters;

		private readonly Func<Character_Model> _currentCharacter;

		private List<Character_Model> _displayedCharacters;

		private int _iconSize;

		private readonly List<RadialMenuSection> _sections = new List<RadialMenuSection>();

		private Character_Model? _selected;

		private Vector2 _center;

		private GraphicsDevice _graphicsDevice;

		private BasicEffect _effect;

		public RadialMenu(Settings settings, ObservableCollection<Character_Model> characters, Container parent, Func<Character_Model> currentCharacter, Data data, TextureManager textureManager)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Expected O, but got Unknown
			_settings = settings;
			_characters = characters;
			_currentCharacter = currentCharacter;
			_data = data;
			base.Parent = parent;
			_tooltip = new CharacterTooltip(currentCharacter, textureManager, data, settings)
			{
				Parent = GameService.Graphics.SpriteScreen,
				ZIndex = 1073741824,
				Size = new Point(300, 50),
				Visible = false
			};
			base.BackgroundColor = Color.get_Transparent();
			foreach (Character_Model character in _characters)
			{
				character.Updated += new EventHandler(Character_Updated);
			}
			base.Parent.Resized += Parent_Resized;
			Control.Input.Keyboard.KeyPressed += Keyboard_KeyPressed;
			_graphicsDevice = GameService.Graphics.LendGraphicsDeviceContext().GraphicsDevice;
			BasicEffect val = new BasicEffect(_graphicsDevice);
			val.set_VertexColorEnabled(true);
			Viewport viewport = _graphicsDevice.get_Viewport();
			float num = ((Viewport)(ref viewport)).get_Width();
			viewport = _graphicsDevice.get_Viewport();
			val.set_Projection(Matrix.CreateOrthographicOffCenter(0f, num, (float)((Viewport)(ref viewport)).get_Height(), 0f, 0f, 1f));
			_effect = val;
		}

		public bool HasDisplayedCharacters()
		{
			if (_characters != null)
			{
				_displayedCharacters = ((_characters.Count > 0) ? _characters.Where((Character_Model e) => e.ShowOnRadial).ToList() : new List<Character_Model>());
			}
			return _displayedCharacters.Count() > 0;
		}

		private void Keyboard_KeyPressed(object sender, KeyboardEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			if ((int)e.Key == 27)
			{
				Hide();
			}
		}

		private void Parent_Resized(object sender, ResizedEventArgs e)
		{
			RecalculateLayout();
		}

		private void Character_Updated(object sender, EventArgs e)
		{
			RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (base.Parent != null)
			{
				_displayedCharacters = ((_characters.Count > 0) ? _characters.Where((Character_Model e) => e.ShowOnRadial).ToList() : new List<Character_Model>());
				_displayedCharacters.Sort((Character_Model a, Character_Model b) => a.Name.CompareTo(b.Name));
				int num = (int)((float)Math.Min(base.Parent.Width, base.Parent.Height) * _settings.Radial_Scale.Value);
				base.Size = new Point(base.Parent.Width, base.Parent.Height);
				_sections.Clear();
				int amount = _displayedCharacters.Count;
				_ = Math.PI * 2.0 / (double)amount;
				int radius = num / 2;
				_iconSize = Math.Min(105, (int)((double)radius * (Math.Sqrt(2.0) / (double)amount)) * 3);
				if (amount > 2)
				{
					List<Vector2> points = new List<Vector2>();
					Vector2 c = _center;
					Vector2 p2 = default(Vector2);
					for (int j = 0; j < amount; j++)
					{
						((Vector2)(ref p2))._002Ector(_center.X - (float)(int)((double)radius * Math.Sin(Math.PI * 2.0 * (double)j / (double)amount)), _center.Y - (float)(int)((double)radius * Math.Cos(Math.PI * 2.0 * (double)j / (double)amount)));
						points.Add(p2);
					}
					Vector2 v = default(Vector2);
					for (int i = 0; i < points.Count; i++)
					{
						Vector2 a2 = points[i];
						Vector2 b2 = ((i == points.Count - 1) ? points[0] : points[i + 1]);
						Triangle t = new Triangle(a2, b2, c);
						((Vector2)(ref v))._002Ector((a2.X + b2.X + c.X) / 3f, (a2.Y + b2.Y + c.Y) / 3f);
						Point p = ((Vector2)(ref v)).ToPoint();
						Math.Min(((Vector2)(ref a2)).ToPoint().Distance2D(((Vector2)(ref v)).ToPoint()), Math.Min(((Vector2)(ref b2)).ToPoint().Distance2D(((Vector2)(ref v)).ToPoint()), ((Vector2)(ref c)).ToPoint().Distance2D(((Vector2)(ref v)).ToPoint())));
						_sections.Add(new RadialMenuSection
						{
							Character = _displayedCharacters[i],
							Triangle = t,
							Lines = t.DrawingPoints(),
							IconPos = v,
							IconRectangle = new Rectangle(p.X - _iconSize / 2, p.Y - _iconSize / 2, _iconSize, _iconSize)
						});
					}
				}
			}
			if (_graphicsDevice != null)
			{
				float dpiScale = (float)_graphicsDevice.get_PresentationParameters().get_BackBufferWidth() / (float)GameService.Graphics.SpriteScreen.Size.X;
				Point position = GameService.Input.Mouse.Position;
				Vector2 mouse = ((Point)(ref position)).ToVector2() * dpiScale;
				Rectangle absBounds = base.AbsoluteBounds;
				_center = new Vector2(mouse.X - (float)absBounds.X, mouse.Y - (float)absBounds.Y);
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			base.OnMouseMoved(e);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			base.DoUpdate(gameTime);
			if (!Control.Input.Keyboard.KeysDown.Contains(_settings.RadialKey.Value.PrimaryKey))
			{
				Hide();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			DrawRadialMenu2(spriteBatch, bounds);
		}

		private void DrawRadialMenu2(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			_tooltip.Hide();
			float dpiScale = (float)_graphicsDevice.get_PresentationParameters().get_BackBufferWidth() / (float)GameService.Graphics.SpriteScreen.Size.X;
			Point val = GameService.Input.Mouse.Position;
			Vector2 mouse = ((Point)(ref val)).ToVector2() * dpiScale;
			float radius = (float)base.Height * _settings.Radial_Scale.Value / 2f;
			int count = _displayedCharacters.Count;
			if (count == 0)
			{
				return;
			}
			float angleStep = (float)Math.PI * 2f / (float)count;
			for (int i = 0; i < count; i++)
			{
				Character_Model character = _displayedCharacters[i];
				float startAngle = (float)i * angleStep;
				float endAngle = startAngle + angleStep;
				float midAngle = (startAngle + endAngle) / 2f;
				Vector2 dir = mouse - _center;
				float num = ((Vector2)(ref dir)).Length();
				float angle = (float)Math.Atan2(dir.Y, dir.X);
				if (angle < 0f)
				{
					angle += (float)Math.PI * 2f;
				}
				bool num2 = num <= radius && angle >= startAngle && angle <= endAngle;
				if (!_settings.Radial_UseProfessionColor.Value)
				{
					_ = _settings.Radial_IdleColor.Value;
				}
				else
				{
					_ = character.Profession.GetData(_data.Professions).Color * 0.7f;
				}
				Color borderColor = (num2 ? _settings.Radial_HoveredBorderColor.Value : _settings.Radial_IdleBorderColor.Value) * 0.5f;
				Color backgroundColor = (num2 ? _settings.Radial_HoveredColor.Value : _settings.Radial_IdleColor.Value) * 0.8f;
				float innerRadius = radius * 0.5f;
				DrawSlice(_center, radius, startAngle, endAngle, backgroundColor);
				DrawBorder(_center, radius, startAngle, endAngle, _settings.Radial_IdleColor.Value * 1.5f);
				if (num2)
				{
					DrawSliceBorder(_center, innerRadius, radius, startAngle, endAngle, borderColor);
				}
				AsyncTexture2D texture = (_settings.Radial_UseProfessionIcons.Value ? character.ProfessionIcon : character.Icon);
				float iconRadius = (radius + innerRadius) / 2f;
				float scale = (float)_iconSize / (float)texture.Width;
				Vector2 val2 = _center / dpiScale;
				Rectangle bounds2 = texture.Bounds;
				val = ((Rectangle)(ref bounds2)).get_Size();
				Vector2 iconCenter = val2 - ((Point)(ref val)).ToVector2() * scale / 2f + new Vector2((float)Math.Cos(midAngle), (float)Math.Sin(midAngle)) * iconRadius;
				spriteBatch.Draw((Texture2D)texture, iconCenter, (Rectangle?)texture.Bounds, Color.get_White(), 0f, Vector2.get_Zero(), scale, (SpriteEffects)0, 1f);
				if (num2)
				{
					if (_settings.Radial_ShowAdvancedTooltip.Value)
					{
						_selected = character;
						_tooltip.Character = character;
						_tooltip.Show();
					}
					else
					{
						base.BasicTooltipText = character.Name;
					}
				}
			}
		}

		private void DrawBorder(Vector2 center, float radius, float startAngle, float endAngle, Color color, int segments = 30)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			if (_graphicsDevice == null || _effect == null)
			{
				return;
			}
			segments = Math.Max(2, segments);
			float angleStep = (endAngle - startAngle) / (float)segments;
			VertexPositionColor[] vertices = (VertexPositionColor[])(object)new VertexPositionColor[segments + 1];
			for (int i = 0; i <= segments; i++)
			{
				float angle = startAngle + (float)i * angleStep;
				Vector2 pos = center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
				vertices[i] = new VertexPositionColor(new Vector3(pos, 0f), color);
			}
			Enumerator enumerator = ((Effect)_effect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					_graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)3, vertices, 0, segments);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		private void DrawSliceBorder(Vector2 center, float innerRadius, float outerRadius, float startAngle, float endAngle, Color color, int segments = 30)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			if (_graphicsDevice == null || _effect == null)
			{
				return;
			}
			segments = Math.Max(2, segments);
			float angleStep = (endAngle - startAngle) / (float)segments;
			VertexPositionColor[] outerVertices = (VertexPositionColor[])(object)new VertexPositionColor[segments + 1];
			for (int j = 0; j <= segments; j++)
			{
				float angle = startAngle + (float)j * angleStep;
				Vector2 pos = center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * outerRadius;
				outerVertices[j] = new VertexPositionColor(new Vector3(pos, 0f), color);
			}
			VertexPositionColor[] innerVertices = (VertexPositionColor[])(object)new VertexPositionColor[segments + 1];
			for (int i = 0; i <= segments; i++)
			{
				float angle2 = startAngle + (float)i * angleStep;
				Vector2 pos2 = center + new Vector2((float)Math.Cos(angle2), (float)Math.Sin(angle2)) * innerRadius;
				innerVertices[i] = new VertexPositionColor(new Vector3(pos2, 0f), color);
			}
			Enumerator enumerator = ((Effect)_effect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					_graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)2, (VertexPositionColor[])(object)new VertexPositionColor[4]
					{
						new VertexPositionColor(new Vector3(center, 0f), color),
						new VertexPositionColor(new Vector3(center + new Vector2((float)Math.Cos(startAngle), (float)Math.Sin(startAngle)) * outerRadius, 0f), color),
						new VertexPositionColor(new Vector3(center, 0f), color),
						new VertexPositionColor(new Vector3(center + new Vector2((float)Math.Cos(endAngle), (float)Math.Sin(endAngle)) * outerRadius, 0f), color)
					}, 0, 2);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		private void DrawSlice(Vector2 center, float radius, float startAngle, float endAngle, Color color, int segments = 30)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			float angleStep = (endAngle - startAngle) / (float)segments;
			VertexPositionColor[] vertices = (VertexPositionColor[])(object)new VertexPositionColor[segments * 3];
			for (int i = 0; i < segments; i++)
			{
				float angle1 = startAngle + (float)i * angleStep;
				float angle2 = startAngle + (float)(i + 1) * angleStep;
				Vector2 p1 = center + new Vector2((float)Math.Cos(angle1), (float)Math.Sin(angle1)) * radius;
				Vector2 p2 = center + new Vector2((float)Math.Cos(angle2), (float)Math.Sin(angle2)) * radius;
				vertices[i * 3] = new VertexPositionColor(new Vector3(center, 0f), color);
				vertices[i * 3 + 1] = new VertexPositionColor(new Vector3(p1, 0f), color);
				vertices[i * 3 + 2] = new VertexPositionColor(new Vector3(p2, 0f), color);
			}
			Enumerator enumerator = ((Effect)_effect).get_CurrentTechnique().get_Passes().GetEnumerator();
			try
			{
				while (((Enumerator)(ref enumerator)).MoveNext())
				{
					((Enumerator)(ref enumerator)).get_Current().Apply();
					_graphicsDevice.DrawUserPrimitives<VertexPositionColor>((PrimitiveType)0, vertices, 0, segments);
				}
			}
			finally
			{
				((IDisposable)(Enumerator)(ref enumerator)).Dispose();
			}
		}

		protected override async void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (_selected != null)
			{
				Hide();
				if (await ExtendedInputService.WaitForNoKeyPressed())
				{
					_selected!.Swap();
				}
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			RecalculateLayout();
			_selected = null;
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			_tooltip.Character = null;
			_tooltip.Hide();
		}

		protected override void DisposeControl()
		{
			if (base.Parent != null)
			{
				base.Parent.Resized -= Parent_Resized;
			}
			foreach (Character_Model character in _characters)
			{
				character.Updated -= new EventHandler(Character_Updated);
			}
			_tooltip?.Dispose();
			base.DisposeControl();
		}
	}
}
