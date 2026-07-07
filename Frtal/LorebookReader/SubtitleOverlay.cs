using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.LorebookReader
{
	public sealed class SubtitleOverlay : Control
	{
		public const string SampleText = "Lorebook Reader — náhled titulků. Přetáhni mě myší.";

		private static readonly Logger Logger = Logger.GetLogger<SubtitleOverlay>();

		private readonly TextRenderer _textRenderer;

		private float _fontSize = 24f;

		private int _boxWidth = 600;

		private string _rawText = "";

		private readonly List<string> _lines = new List<string>();

		private bool _editMode;

		private bool _dragging;

		private Point _dragOffset;

		public float FontSize
		{
			get
			{
				return _fontSize;
			}
			set
			{
				if (!(Math.Abs(_fontSize - value) < 0.1f))
				{
					_fontSize = value;
					Reflow();
				}
			}
		}

		public int BoxWidth
		{
			get
			{
				return _boxWidth;
			}
			set
			{
				int w = Math.Max(100, value);
				if (_boxWidth != w)
				{
					_boxWidth = w;
					Reflow();
				}
			}
		}

		public string SubtitleText
		{
			get
			{
				return _rawText;
			}
			set
			{
				value = value ?? "";
				if (!(_rawText == value))
				{
					_rawText = value;
					Reflow();
				}
			}
		}

		public bool EditMode
		{
			get
			{
				return _editMode;
			}
			set
			{
				if (_editMode != value)
				{
					_editMode = value;
					if (_editMode)
					{
						SubtitleText = "Lorebook Reader — náhled titulků. Přetáhni mě myší.";
						((Control)this).set_Visible(true);
					}
					else
					{
						StopDrag(fireEvent: false);
						SubtitleText = "";
						((Control)this).set_Visible(false);
					}
				}
			}
		}

		public event EventHandler<EventArgs> PositionEdited;

		public SubtitleOverlay(TextRenderer textRenderer)
			: this()
		{
			_textRenderer = textRenderer;
			((Control)this).set_Visible(false);
		}

		protected override CaptureType CapturesInput()
		{
			if (_editMode)
			{
				return (CaptureType)4;
			}
			return (CaptureType)0;
		}

		private void Reflow()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_lines.Clear();
				if (string.IsNullOrEmpty(_rawText))
				{
					((Control)this).set_Size(new Point(_boxWidth, 1));
					return;
				}
				_lines.AddRange(_textRenderer.WrapText(_rawText, _fontSize, _boxWidth - 16));
				int height = (int)Math.Ceiling(_textRenderer.LineHeight(_fontSize) * (float)_lines.Count) + 10;
				((Control)this).set_Size(new Point(_boxWidth, Math.Max(1, height)));
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Subtitle layout failed.");
				_lines.Clear();
				((Control)this).set_Size(new Point(_boxWidth, 1));
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			if (_editMode && !_dragging)
			{
				_dragging = true;
				_dragOffset = new Point(GameService.Input.get_Mouse().get_Position().X - ((Control)this).get_Location().X, GameService.Input.get_Mouse().get_Position().Y - ((Control)this).get_Location().Y);
				GameService.Input.get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)OnGlobalMouseMoved);
				GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
			}
		}

		private void OnGlobalMouseMoved(object sender, MouseEventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			if (_dragging)
			{
				Point position = GameService.Input.get_Mouse().get_Position();
				Point sprite = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
				int x = position.X - _dragOffset.X;
				int y = position.Y - _dragOffset.Y;
				((Control)this).set_Location(new Point(Math.Max(0, Math.Min(x, sprite.X - ((Control)this).get_Width())), Math.Max(0, Math.Min(y, sprite.Y - ((Control)this).get_Height()))));
			}
		}

		private void OnGlobalMouseReleased(object sender, MouseEventArgs e)
		{
			StopDrag(fireEvent: true);
		}

		private void StopDrag(bool fireEvent)
		{
			if (_dragging)
			{
				_dragging = false;
				GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)OnGlobalMouseMoved);
				GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
				if (fireEvent)
				{
					this.PositionEdited?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (_editMode)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, new Color(0, 0, 0, 120));
				}
				if (_lines.Count == 0)
				{
					return;
				}
				float lh = _textRenderer.LineHeight(_fontSize);
				float y = bounds.Y + 5;
				Color white = Color.get_White() * ((Control)this).get_Opacity();
				Color shadow = default(Color);
				((Color)(ref shadow))._002Ector(0, 0, 0, (int)(210f * ((Control)this).get_Opacity()));
				foreach (string line in _lines)
				{
					if (line.Length == 0)
					{
						y += lh;
						continue;
					}
					Texture2D shadowTex = _textRenderer.RenderLine(line, _fontSize, shadow);
					Texture2D textTex = _textRenderer.RenderLine(line, _fontSize, white);
					if (textTex == null)
					{
						y += lh;
						continue;
					}
					int x = bounds.X + (bounds.Width - textTex.get_Width()) / 2;
					if (shadowTex != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, shadowTex, new Rectangle(x + 2, (int)y + 2, shadowTex.get_Width(), shadowTex.get_Height()));
					}
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, textTex, new Rectangle(x, (int)y, textTex.get_Width(), textTex.get_Height()));
					y += lh;
				}
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Subtitle paint failed; hiding overlay.");
				((Control)this).set_Visible(false);
			}
		}

		protected override void DisposeControl()
		{
			StopDrag(fireEvent: false);
			((Control)this).DisposeControl();
		}
	}
}
