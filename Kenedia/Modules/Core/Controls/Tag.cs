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
	public class Tag : FlowPanel, IFontControl
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
				return ((Label)_text).get_Font();
			}
			set
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				if (value != null && ((Label)_text).get_Font() != value)
				{
					((Control)_dummy).set_Size(new Point(value.get_LineHeight(), value.get_LineHeight()));
					((Control)_delete).set_Size(new Point(value.get_LineHeight(), value.get_LineHeight()));
					((Label)_text).set_Font(value);
					((Control)_text).set_Height(Math.Max(20, value.get_LineHeight() + 4));
					((Control)this).set_Height(Math.Max(20, value.get_LineHeight() + 4) + 5);
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
					_background.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)CreateDisabledBackground);
				}
			}
		}

		public bool ShowDelete
		{
			get
			{
				return ((Control)_delete).get_Visible();
			}
			set
			{
				if (_delete != null)
				{
					((Control)_delete).set_Visible(value);
					((Control)_dummy).set_Visible(!value);
					((Control)this).Invalidate();
				}
			}
		}

		public string Text
		{
			get
			{
				Label text = _text;
				if (text == null)
				{
					return null;
				}
				return ((Label)text).get_Text();
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
					((Label)_text).set_Text(value);
					((Control)_text).set_Width((int)Font.MeasureString(value).Width + 4);
					((Control)this).set_Width((int)Font.MeasureString(value).Width + ((Control)_delete).get_Width() + (int)((FlowPanel)this).get_OuterControlPadding().X + ((Container)this).get_AutoSizePadding().X + (int)((FlowPanel)this).get_ControlPadding().X);
				}
			}
		}

		public event EventHandler Deleted;

		public event EventHandler ActiveChanged;

		public Tag()
			: this()
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
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)2);
			((FlowPanel)this).set_OuterControlPadding(new Vector2(3f, 3f));
			((FlowPanel)this).set_ControlPadding(new Vector2(4f, 0f));
			((Container)this).set_AutoSizePadding(new Point(5, 0));
			ImageButton imageButton = new ImageButton();
			((Control)imageButton).set_Parent((Container)(object)this);
			imageButton.Texture = AsyncTexture2D.FromAssetId(156012);
			imageButton.HoveredTexture = AsyncTexture2D.FromAssetId(156011);
			imageButton.TextureRectangle = new Rectangle(4, 4, 24, 24);
			((Control)imageButton).set_Size(new Point(20, 20));
			((Control)imageButton).set_BasicTooltipText(string.Format(strings_common.DeleteX, strings_common.Tag));
			_delete = imageButton;
			((Control)_delete).add_Click((EventHandler<MouseEventArgs>)Delete_Click);
			ImageButton imageButton2 = new ImageButton();
			((Control)imageButton2).set_Parent((Container)(object)this);
			imageButton2.Texture = AsyncTexture2D.FromAssetId(156025);
			imageButton2.TextureRectangle = new Rectangle(44, 48, 43, 46);
			((Control)imageButton2).set_Size(new Point(20, 20));
			((Control)imageButton2).set_Visible(false);
			_dummy = imageButton2;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)this);
			((Control)label).set_Height(Math.Max(20, Control.get_Content().get_DefaultFont14().get_LineHeight() + 4));
			((Label)label).set_VerticalAlignment((VerticalAlignment)1);
			((Label)label).set_Text("Tag");
			_text = label;
			((Control)this).set_Height(Math.Max(20, Font.get_LineHeight() + 4) + 5);
			((Control)this).set_Width((int)Font.MeasureString("Tag").Width + ((Control)_delete).get_Width() + (int)((FlowPanel)this).get_OuterControlPadding().X + ((Container)this).get_AutoSizePadding().X + (int)((FlowPanel)this).get_ControlPadding().X);
			((Control)_text).set_Width((int)Font.MeasureString("Tag").Width + 4);
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
				AsyncTexture2D texture = (Active ? _background : ((_disabledBackground != null) ? AsyncTexture2D.op_Implicit(_disabledBackground) : _background));
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(texture), bounds, (Rectangle?)bounds, Active ? (Color.get_White() * 0.98f) : (_disabledColor * 0.8f));
			}
			Color color = Color.get_Black();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (!((Control)_delete).get_MouseOver())
			{
				((Panel)this).OnClick(e);
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
			((Control)this).Dispose();
		}

		private void CreateDisabledBackground(object sender, ValueChangedEventArgs<Texture2D> e)
		{
			_disabledBackground = _background.get_Texture().ToGrayScaledPalettable();
			_background.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)CreateDisabledBackground);
		}
	}
}
