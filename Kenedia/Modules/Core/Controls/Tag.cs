using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Res;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Controls
{
	public class Tag : Blish_HUD.Controls.FlowPanel, IFontControl
	{
		private readonly Label _text;

		private readonly ImageButton _delete;

		private readonly ImageButton _dummy;

		private Color _disabledColor = new Color(156, 156, 156);

		private Texture2D _disabledBackground;

		private AsyncTexture2D _background;

		private bool _active;

		public int TagPanelIndex { get; set; }

		public Point DesiredSize => new Point((int)Font.MeasureString(Text).Width + 30, Math.Max(20, Font.get_LineHeight() + 4) + 5);

		public Action OnDeleteAction { get; set; }

		public Action OnClickAction { get; set; }

		public BitmapFont Font
		{
			get
			{
				return _text.Font;
			}
			set
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				if (value != null && _text.Font != value)
				{
					_dummy.Size = new Point(value.get_LineHeight(), value.get_LineHeight());
					_delete.Size = new Point(value.get_LineHeight(), value.get_LineHeight());
					_text.Font = value;
					_text.Height = Math.Max(20, value.get_LineHeight() + 4);
					base.Height = Math.Max(20, value.get_LineHeight() + 4) + 5;
				}
			}
		}

		public bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				if (_active != value)
				{
					_active = value;
					this.ActiveChanged?.Invoke(this, null);
				}
			}
		}

		public bool CanInteract { get; set; } = true;


		public AsyncTexture2D Background
		{
			get
			{
				return _background;
			}
			set
			{
				_background = value;
				if (value != null)
				{
					CreateDisabledBackground(null, null);
					_background.TextureSwapped += CreateDisabledBackground;
				}
			}
		}

		public bool ShowDelete
		{
			get
			{
				return _delete.Visible;
			}
			set
			{
				if (_delete != null)
				{
					_delete.Visible = value;
					_dummy.Visible = !value;
					Invalidate();
				}
			}
		}

		public string Text
		{
			get
			{
				return _text?.Text;
			}
			set
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				if (_text != null)
				{
					_text.Text = value;
					_text.Width = (int)Font.MeasureString(value).Width + 4;
					base.Width = (int)Font.MeasureString(value).Width + _delete.Width + (int)base.OuterControlPadding.X + base.AutoSizePadding.X + (int)base.ControlPadding.X;
				}
			}
		}

		public event EventHandler Deleted;

		public event EventHandler ActiveChanged;

		public Tag()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			Background = AsyncTexture2D.FromAssetId(1620622);
			base.FlowDirection = ControlFlowDirection.SingleLeftToRight;
			base.OuterControlPadding = new Vector2(3f, 3f);
			base.ControlPadding = new Vector2(4f, 0f);
			base.AutoSizePadding = new Point(5, 0);
			_delete = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(156012),
				HoveredTexture = AsyncTexture2D.FromAssetId(156011),
				TextureRectangle = new Rectangle(4, 4, 24, 24),
				Size = new Point(20, 20),
				BasicTooltipText = string.Format(strings_common.DeleteX, strings_common.Tag)
			};
			_delete.Click += Delete_Click;
			_dummy = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(156025),
				TextureRectangle = new Rectangle(44, 48, 43, 46),
				Size = new Point(20, 20),
				Visible = false
			};
			_text = new Label
			{
				Parent = this,
				Height = Math.Max(20, Control.Content.DefaultFont14.get_LineHeight() + 4),
				VerticalAlignment = VerticalAlignment.Middle,
				Text = "Tag"
			};
			base.Height = Math.Max(20, Font.get_LineHeight() + 4) + 5;
			base.Width = (int)Font.MeasureString("Tag").Width + _delete.Width + (int)base.OuterControlPadding.X + base.AutoSizePadding.X + (int)base.ControlPadding.X;
			_text.Width = (int)Font.MeasureString("Tag").Width + 4;
		}

		public void SetActive(bool active)
		{
			_active = active;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			if (_background != null)
			{
				AsyncTexture2D texture = (Active ? _background : ((_disabledBackground != null) ? ((AsyncTexture2D)_disabledBackground) : _background));
				spriteBatch.DrawOnCtrl(this, texture, bounds, bounds, Active ? (Color.get_White() * 0.98f) : (_disabledColor * 0.8f));
			}
			Color color = Color.get_Black();
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 2), Rectangle.get_Empty(), color * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 1), Rectangle.get_Empty(), color * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, 2), Rectangle.get_Empty(), color * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, 1), Rectangle.get_Empty(), color * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), Rectangle.get_Empty(), color * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), Rectangle.get_Empty(), color * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), Rectangle.get_Empty(), color * 0.5f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), Rectangle.get_Empty(), color * 0.6f);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (!_delete.MouseOver)
			{
				base.OnClick(e);
				if (CanInteract)
				{
					Active = !Active;
				}
				OnClickAction?.Invoke();
			}
		}

		private void Delete_Click(object sender, MouseEventArgs e)
		{
			this.Deleted?.Invoke(this, EventArgs.Empty);
			OnDeleteAction?.Invoke();
			Dispose();
		}

		private void CreateDisabledBackground(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			_disabledBackground = _background.Texture.ToGrayScaledPalettable();
			_background.TextureSwapped -= CreateDisabledBackground;
		}
	}
}
