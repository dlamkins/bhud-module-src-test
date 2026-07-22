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

		private AsyncTexture2D _backgroundImage = AsyncTexture2D.FromAssetId(156003);

		private AsyncTexture2D _alertImage = AsyncTexture2D.FromAssetId(222246);

		private readonly (Button Button, DialogResult Result)[] _buttons = new(Button, DialogResult)[2]
		{
			(new Button
			{
				Text = "OK",
				SelectedTint = true
			}, DialogResult.OK),
			(new Button
			{
				Text = "Cancel",
				SelectedTint = true
			}, DialogResult.Cancel)
		};

		private readonly FlowPanel _buttonPanel = new FlowPanel
		{
			FlowDirection = ControlFlowDirection.SingleLeftToRight,
			WidthSizingMode = SizingMode.AutoSize,
			HeightSizingMode = SizingMode.AutoSize
		};

		private DialogResult _dialogResult;

		private readonly EventWaitHandle _waitHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);

		private Rectangle _titleBounds;

		private Rectangle _alertBounds;

		private Rectangle _titleTextBounds;

		private Rectangle _messageTextBounds;

		private string _message;

		public string Title { get; private set; }

		public string Message { get; private set; }

		private BitmapFont TitleFont { get; set; } = GameService.Content.DefaultFont32;


		private BitmapFont Font { get; set; } = GameService.Content.DefaultFont16;


		public ButtonDefinition[] Buttons { get; }

		public int SelectedButtonIndex { get; private set; }

		public bool AutoSize { get; set; } = true;


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

		public int DesiredWidth { get; set; } = 300;


		public BaseDialog(string title, string message, ButtonDefinition[] buttons = null)
		{
			Title = title;
			Message = message;
			Buttons = buttons;
			if (buttons != null)
			{
				_buttons = buttons.Select((ButtonDefinition x) => (new Button
				{
					Text = x.Title
				}, x.Result)).ToArray();
			}
			_modalBackground = new Panel
			{
				Parent = GameService.Graphics.SpriteScreen,
				ZIndex = 2147483646,
				BackgroundColor = Color.White * 0.2f,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				Visible = false
			};
			base.BackgroundImage = _backgroundImage;
			base.Parent = GameService.Graphics.SpriteScreen;
			ZIndex = int.MaxValue;
			base.BorderColor = Color.Black;
			base.BorderWidth = new RectangleDimensions(3);
			base.ContentPadding = new RectangleDimensions(5);
			base.Height = 115;
			base.Width = 300;
			base.Visible = false;
			_buttonPanel.Parent = this;
			_buttonPanel.Resized += ButtonPanel_Resized;
			BuildButtons();
			SelectButton();
			GameService.Input.Keyboard.KeyPressed += Keyboard_KeyPressed;
			base.Parent.Resized += Parent_Resized;
		}

		private void ButtonPanel_Resized(object sender, ResizedEventArgs e)
		{
			RecalculateLayout();
		}

		private void Parent_Resized(object sender, ResizedEventArgs e)
		{
			RecalculateLayout();
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
				button.Button.Parent = _buttonPanel;
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
			switch (e.Key)
			{
			case Keys.Escape:
				_dialogResult = DialogResult.None;
				_waitHandle.Set();
				break;
			case Keys.Enter:
			case Keys.Space:
				_dialogResult = _buttons[SelectedButtonIndex].Result;
				_waitHandle.Set();
				break;
			case Keys.Left:
				SelectButton(-1);
				break;
			case Keys.Right:
				SelectButton(1);
				break;
			}
		}

		public async Task<DialogResult> ShowDialog(CancellationToken cancellationToken = default(CancellationToken))
		{
			return await ShowDialog(TimeSpan.FromMinutes(5.0), cancellationToken);
		}

		public async Task<DialogResult> ShowDialog(TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
		{
			Show();
			if (!(await _waitHandle.WaitOneAsync(timeout, cancellationToken)))
			{
				_dialogResult = DialogResult.None;
			}
			Hide();
			return _dialogResult;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			if (base.Parent != null)
			{
				base.Location = new Point((base.Parent.Width - base.Width) / 2, (base.Parent.Height - base.Height) / 2);
			}
			base.TextureRectangle = new Rectangle(30, 30, Math.Min(base.Width, _backgroundImage.Width - 60), Math.Min(base.Height, _backgroundImage.Height - 60));
			_titleBounds = new Rectangle(base.ContentRegion.Left, base.ContentRegion.Top, base.ContentRegion.Width, TitleFont.LineHeight + 2);
			_alertBounds = new Rectangle(_titleBounds.Left, _titleBounds.Top, _titleBounds.Height, _titleBounds.Height);
			_titleTextBounds = new Rectangle(_titleBounds.Left + _alertBounds.Width + 10, _alertBounds.Top, _titleBounds.Width - _alertBounds.Width * 2, _titleBounds.Height);
			_messageTextBounds = new Rectangle(_titleBounds.Left, _titleBounds.Bottom + 5, _titleBounds.Width, _buttonPanel.Top - _titleBounds.Bottom + 5);
			if (AutoSize)
			{
				_message = DrawUtil.WrapText(Font, Message, base.ContentRegion.Width);
				base.Height = base.BorderWidth.Vertical + TitleFont.LineHeight + 2 + 5 + (int)Font.GetStringRectangle(_message).Height + 15 + _buttonPanel.Height + base.ContentPadding.Vertical;
				base.Width = Math.Max(DesiredWidth, base.ContentPadding.Horizontal + _alertBounds.Width * 2 + 10 + (int)TitleFont.GetStringRectangle(Title).Width);
			}
			_buttonPanel.Location = new Point((base.ContentRegion.Width - _buttonPanel.Width) / 2, base.ContentRegion.Height - _buttonPanel.Height);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
			spriteBatch.DrawStringOnCtrl(this, Title, TitleFont, _titleTextBounds, ContentService.Colors.ColonialWhite, wrap: false, HorizontalAlignment.Center, VerticalAlignment.Top);
			spriteBatch.DrawStringOnCtrl(this, _message, Font, _messageTextBounds, Color.White, wrap: false, HorizontalAlignment.Center, VerticalAlignment.Top);
			spriteBatch.DrawOnCtrl(this, _alertImage, _alertBounds, _alertImage.Bounds);
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			_modalBackground?.Hide();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			_modalBackground?.Show();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_backgroundImage = null;
			_alertImage = null;
			_modalBackground?.Dispose();
			GameService.Input.Keyboard.KeyPressed -= Keyboard_KeyPressed;
		}
	}
}
