using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using RaidClears.Utils;

namespace RaidClears.Shared.Controls
{
	public class ConfirmDialog : Container
	{
		private readonly string _title;

		private string _parsedTitle;

		private string _parsedMessage;

		private readonly string _message;

		private DialogResult _dialogResult;

		private static readonly BitmapFont _titleFont = GameService.Content.get_DefaultFont32();

		private static readonly BitmapFont _messageFont = GameService.Content.get_DefaultFont16();

		private EventWaitHandle _waitHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);

		private Texture2D _backgroundImage;

		private Rectangle _shadowRect;

		private Rectangle _confirmRect;

		private Rectangle _titleRect;

		private Rectangle _messageRect;

		private FlowPanel _buttonPanel;

		private (StandardButton Button, DialogResult Result)[] _buttons;

		private int _selectedButtonIndex;

		public DialogResult ESC_Result;

		public int SelectedButtonIndex
		{
			get
			{
				return _selectedButtonIndex;
			}
			set
			{
				_selectedButtonIndex = value;
				SelectButton();
			}
		}

		public ConfirmDialog(string title, string message, ButtonDefinition[] buttons = null)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			(StandardButton, DialogResult)[] array = new(StandardButton, DialogResult)[2];
			StandardButton val = new StandardButton();
			val.set_Text("OK");
			array[0] = (val, DialogResult.OK);
			StandardButton val2 = new StandardButton();
			val2.set_Text("Cancel");
			array[1] = (val2, DialogResult.Cancel);
			_buttons = array;
			((Container)this)._002Ector();
			_title = title;
			_parsedTitle = _title;
			_message = message;
			_parsedMessage = _message;
			if (buttons != null)
			{
				_buttons = ((IEnumerable<ButtonDefinition>)buttons).Select((Func<ButtonDefinition, (StandardButton, DialogResult)>)delegate(ButtonDefinition x)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					//IL_0005: Unknown result type (might be due to invalid IL or missing references)
					//IL_001c: Expected O, but got Unknown
					StandardButton val3 = new StandardButton();
					val3.set_Text(x.Title);
					return (val3, x.Result);
				}).ToArray();
			}
			BuildButtons();
			SelectButton();
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Width(((Control)((Control)this).get_Parent()).get_Width());
			((Control)this).set_Height(((Control)((Control)this).get_Parent()).get_Height());
			((Control)this).set_ZIndex(int.MaxValue);
			((Control)this).set_Visible(false);
			_backgroundImage = Service.Textures!.SettingWindowBackground;
			GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)Keyboard_KeyPressed);
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
						_dialogResult = ESC_Result;
						_waitHandle.Set();
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
			_waitHandle.Set();
		}

		private void SelectButton(int direction = 0)
		{
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
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
			_buttons.ToList().ForEach(delegate((StandardButton Button, DialogResult Result) b)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)b.Button).set_BackgroundColor(Color.get_Transparent());
			});
			if (_buttons[SelectedButtonIndex].Result == DialogResult.OK)
			{
				((Control)_buttons[SelectedButtonIndex].Button).set_BackgroundColor(Service.Settings.StrikeSettings.Style.Color.Cleared.get_Value().HexToXnaColor());
			}
			else
			{
				((Control)_buttons[SelectedButtonIndex].Button).set_BackgroundColor(Color.get_White());
			}
		}

		private void BuildButtons()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(10f, 0f));
			_buttonPanel = val;
			(StandardButton, DialogResult)[] buttons = _buttons;
			for (int i = 0; i < buttons.Length; i++)
			{
				(StandardButton, DialogResult) tuple = buttons[i];
				((Control)tuple.Item1).set_Parent((Container)(object)_buttonPanel);
				((Control)tuple.Item1).add_Click((EventHandler<MouseEventArgs>)Button_Click);
			}
			((Control)_buttonPanel).set_Parent((Container)(object)this);
		}

		private void Button_Click(object sender, MouseEventArgs e)
		{
			StandardButton button = (StandardButton)((sender is StandardButton) ? sender : null);
			_dialogResult = _buttons.Where(((StandardButton Button, DialogResult Result) b) => b.Button == button).First().Result;
			_waitHandle.Set();
		}

		private void CancelButton_Click(object sender, MouseEventArgs e)
		{
			_dialogResult = DialogResult.Cancel;
			_waitHandle.Set();
		}

		private void OKButton_Click(object sender, MouseEventArgs e)
		{
			_dialogResult = DialogResult.OK;
			_waitHandle.Set();
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		public async Task<DialogResult> ShowDialog(CancellationToken cancellationToken = default(CancellationToken))
		{
			return await ShowDialog(TimeSpan.FromMinutes(1.0), cancellationToken);
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
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			_shadowRect = new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height());
			int width = ((Control)this).get_Width() / 3;
			int height = ((Control)this).get_Height() / 5;
			int x = ((Control)this).get_Width() / 2 - width / 2;
			int y = ((Control)this).get_Height() / 2 - height / 2;
			_confirmRect = new Rectangle(x, y, width, height);
			int textPadding = 50;
			int maxTextWidth = _confirmRect.Width - textPadding * 2;
			_parsedTitle = DrawUtil.WrapText(_titleFont, _title, (float)maxTextWidth);
			_parsedMessage = DrawUtil.WrapText(_messageFont, _message, (float)maxTextWidth);
			_titleRect = new Rectangle(_confirmRect.X + textPadding, _confirmRect.Y + _confirmRect.Height / 5, _confirmRect.Width - textPadding * 2, (int)Math.Ceiling(_titleFont.MeasureString(_parsedTitle).Height));
			_messageRect = new Rectangle(_confirmRect.X + textPadding, ((Rectangle)(ref _titleRect)).get_Bottom() + 15, _confirmRect.Width - textPadding * 2, (int)Math.Ceiling(_messageFont.MeasureString(_parsedMessage).Height));
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			int buttonY = _confirmRect.Y + _confirmRect.Height / 2 + _confirmRect.Height / 4;
			((Control)_buttonPanel).set_Location(new Point(_confirmRect.X + _confirmRect.Width / 2 - ((Control)_buttonPanel).get_Width() / 2, buttonY));
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if (_backgroundImage != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _backgroundImage, _confirmRect);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _parsedTitle, _titleFont, _titleRect, Color.get_White(), false, (HorizontalAlignment)1, (VerticalAlignment)0);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _parsedMessage, _messageFont, _messageRect, Color.get_White(), false, (HorizontalAlignment)1, (VerticalAlignment)0);
		}

		protected override void DisposeControl()
		{
			if (_buttons != null)
			{
				(StandardButton, DialogResult)[] buttons = _buttons;
				for (int i = 0; i < buttons.Length; i++)
				{
					((Control)buttons[i].Item1).remove_Click((EventHandler<MouseEventArgs>)Button_Click);
				}
			}
			FlowPanel buttonPanel = _buttonPanel;
			if (buttonPanel != null)
			{
				((Container)buttonPanel).ClearChildren();
			}
			((Container)this).ClearChildren();
			if (_buttons != null)
			{
				(StandardButton, DialogResult)[] buttons = _buttons;
				for (int i = 0; i < buttons.Length; i++)
				{
					((Control)buttons[i].Item1).Dispose();
				}
			}
			_buttons = null;
			FlowPanel buttonPanel2 = _buttonPanel;
			if (buttonPanel2 != null)
			{
				((Control)buttonPanel2).Dispose();
			}
			_buttonPanel = null;
			GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)Keyboard_KeyPressed);
			_waitHandle?.Set();
			_waitHandle?.Dispose();
			_waitHandle = null;
			_backgroundImage = null;
		}
	}
}
