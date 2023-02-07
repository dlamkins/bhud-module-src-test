using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Controls
{
	public class BaseDialog : FramedContainer
	{
		private readonly Panel _modalBackground;

		private readonly AsyncTexture2D _backgroundImage = AsyncTexture2D.FromAssetId(156003);

		private readonly AsyncTexture2D _alertImage = AsyncTexture2D.FromAssetId(222246);

		private readonly (Button Button, DialogResult Result)[] _buttons;

		private readonly FlowPanel _buttonPanel;

		private DialogResult _dialogResult;

		private readonly EventWaitHandle _waitHandle;

		private Rectangle _titleBounds;

		private Rectangle _alertBounds;

		private Rectangle _titleTextBounds;

		private Rectangle _messageTextBounds;

		private string _message;

		public string Title { get; private set; }

		public string Message { get; private set; }

		private BitmapFont TitleFont { get; set; } = GameService.Content.get_DefaultFont32();


		private BitmapFont Font { get; set; } = GameService.Content.get_DefaultFont16();


		public ButtonDefinition[] Buttons { get; }

		public int SelectedButtonIndex { get; private set; }

		public bool AutoSize { get; set; }

		public Color? ModalColor
		{
			get
			{
				return _modalBackground.BackgroundColor;
			}
			set
			{
				_modalBackground.BackgroundColor = value;
			}
		}

		public int DesiredWidth { get; set; }

		public BaseDialog(string title, string message, ButtonDefinition[] buttons = null)
		{
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			(Button, DialogResult)[] array = new(Button, DialogResult)[2];
			Button button = new Button();
			((StandardButton)button).set_Text("OK");
			array[0] = (button, DialogResult.OK);
			Button button2 = new Button();
			((StandardButton)button2).set_Text("Cancel");
			array[1] = (button2, DialogResult.Cancel);
			_buttons = array;
			FlowPanel flowPanel = new FlowPanel();
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)2);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)1);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			_buttonPanel = flowPanel;
			_waitHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);
			AutoSize = true;
			base._002Ector();
			Title = title;
			Message = message;
			Buttons = buttons;
			if (buttons != null)
			{
				_buttons = buttons.Select(delegate(ButtonDefinition x)
				{
					Button button3 = new Button();
					((StandardButton)button3).set_Text(x.Title);
					return (button3, x.Result);
				}).ToArray();
			}
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)panel).set_ZIndex(2147483646);
			panel.BackgroundColor = Color.get_White() * 0.2f;
			((Container)panel).set_WidthSizingMode((SizingMode)2);
			((Container)panel).set_HeightSizingMode((SizingMode)2);
			((Control)panel).set_Visible(false);
			_modalBackground = panel;
			BuildButtons();
			SelectButton();
			base.BackgroundImage = _backgroundImage;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_ZIndex(int.MaxValue);
			base.BorderColor = Color.get_Black();
			base.BorderWidth = new RectangleDimensions(3);
			base.ContentPadding = new RectangleDimensions(5);
			((Control)this).set_Height(115);
			((Control)this).set_Width(300);
			((Control)this).set_Visible(false);
			((Control)_buttonPanel).set_Parent((Container)(object)this);
			((Control)_buttonPanel).add_Resized((EventHandler<ResizedEventArgs>)ButtonPanel_Resized);
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)Keyboard_KeyPressed);
			((Control)((Control)this).get_Parent()).add_Resized((EventHandler<ResizedEventArgs>)Parent_Resized);
		}

		private void ButtonPanel_Resized(object sender, ResizedEventArgs e)
		{
			((Control)this).RecalculateLayout();
		}

		private void Parent_Resized(object sender, ResizedEventArgs e)
		{
			((Control)this).RecalculateLayout();
		}

		private void SelectButton(int direction = 0)
		{
			switch (direction)
			{
			case -1:
				if (SelectedButtonIndex > 0)
				{
					SelectedButtonIndex--;
				}
				break;
			case 1:
				if (SelectedButtonIndex < _buttons.Length - 1)
				{
					SelectedButtonIndex++;
				}
				break;
			}
			_buttons.ToList().ForEach(delegate((Button Button, DialogResult Result) b)
			{
				b.Button.Selected = false;
			});
			_buttons[SelectedButtonIndex].Button.Selected = true;
		}

		private void BuildButtons()
		{
			(Button, DialogResult)[] buttons = _buttons;
			for (int i = 0; i < buttons.Length; i++)
			{
				(Button Button, DialogResult Result) button = buttons[i];
				((Control)button.Button).set_Parent((Container)(object)_buttonPanel);
				button.Button.ClickAction = delegate
				{
					_dialogResult = _buttons.Where(delegate((Button Button, DialogResult Result) b)
					{
						var (button2, dialogResult) = b;
						var (button3, dialogResult2) = button;
						return button2 == button3 && dialogResult == dialogResult2;
					}).First().Result;
					_waitHandle.Set();
				};
			}
		}

		private void Keyboard_KeyPressed(object sender, KeyboardEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Invalid comparison between Unknown and I4
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Invalid comparison between Unknown and I4
			Keys key = e.get_Key();
			if ((int)key <= 27)
			{
				if ((int)key != 13)
				{
					if ((int)key == 27)
					{
						_dialogResult = DialogResult.None;
					}
					return;
				}
			}
			else if ((int)key != 32)
			{
				if ((int)key != 37)
				{
					if ((int)key == 39)
					{
						SelectButton(1);
					}
				}
				else
				{
					SelectButton(-1);
				}
				return;
			}
			_dialogResult = _buttons[SelectedButtonIndex].Result;
		}

		public async Task<DialogResult> ShowDialog(CancellationToken cancellationToken = default(CancellationToken))
		{
			return await ShowDialog(TimeSpan.FromMinutes(5.0), cancellationToken);
		}

		public async Task<DialogResult> ShowDialog(TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
		{
			((Control)this).Show();
			if (!(await _waitHandle.WaitOneAsync(timeout, cancellationToken)))
			{
				_dialogResult = DialogResult.None;
			}
			((Control)this).Hide();
			return _dialogResult;
		}

		public override void RecalculateLayout()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (((Control)this).get_Parent() != null)
			{
				((Control)this).set_Location(new Point((((Control)((Control)this).get_Parent()).get_Width() - ((Control)this).get_Width()) / 2, (((Control)((Control)this).get_Parent()).get_Height() - ((Control)this).get_Height()) / 2));
			}
			base.TextureRectangle = new Rectangle(30, 30, Math.Min(((Control)this).get_Width(), _backgroundImage.get_Width() - 60), Math.Min(((Control)this).get_Height(), _backgroundImage.get_Height() - 60));
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			int left = ((Rectangle)(ref contentRegion)).get_Left();
			contentRegion = ((Container)this).get_ContentRegion();
			_titleBounds = new Rectangle(left, ((Rectangle)(ref contentRegion)).get_Top(), ((Container)this).get_ContentRegion().Width, TitleFont.get_LineHeight() + 2);
			_alertBounds = new Rectangle(((Rectangle)(ref _titleBounds)).get_Left(), ((Rectangle)(ref _titleBounds)).get_Top(), _titleBounds.Height, _titleBounds.Height);
			_titleTextBounds = new Rectangle(((Rectangle)(ref _titleBounds)).get_Left() + _alertBounds.Width + 10, ((Rectangle)(ref _alertBounds)).get_Top(), _titleBounds.Width - _alertBounds.Width * 2, _titleBounds.Height);
			_messageTextBounds = new Rectangle(((Rectangle)(ref _titleBounds)).get_Left(), ((Rectangle)(ref _titleBounds)).get_Bottom() + 5, _titleBounds.Width, ((Control)_buttonPanel).get_Top() - ((Rectangle)(ref _titleBounds)).get_Bottom() + 5);
			if (AutoSize)
			{
				_message = DrawUtil.WrapText(Font, Message, (float)((Container)this).get_ContentRegion().Width);
				((Control)this).set_Height(base.BorderWidth.Vertical + TitleFont.get_LineHeight() + 2 + 5 + (int)Font.GetStringRectangle(_message).Height + 15 + ((Control)_buttonPanel).get_Height() + base.ContentPadding.Vertical);
				((Control)this).set_Width(Math.Max(DesiredWidth, base.ContentPadding.Horizontal + _alertBounds.Width * 2 + 10 + (int)TitleFont.GetStringRectangle(Title).Width));
			}
			((Control)_buttonPanel).set_Location(new Point((((Container)this).get_ContentRegion().Width - ((Control)_buttonPanel).get_Width()) / 2, ((Container)this).get_ContentRegion().Height - ((Control)_buttonPanel).get_Height()));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Title, TitleFont, _titleTextBounds, Colors.ColonialWhite, false, (HorizontalAlignment)1, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _message, Font, _messageTextBounds, Color.get_White(), false, (HorizontalAlignment)1, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_alertImage), _alertBounds, (Rectangle?)_alertImage.get_Bounds());
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			Panel modalBackground = _modalBackground;
			if (modalBackground != null)
			{
				((Control)modalBackground).Hide();
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Panel modalBackground = _modalBackground;
			if (modalBackground != null)
			{
				((Control)modalBackground).Show();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Panel modalBackground = _modalBackground;
			if (modalBackground != null)
			{
				((Control)modalBackground).Dispose();
			}
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)Keyboard_KeyPressed);
		}
	}
}
