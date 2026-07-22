using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls
{
	public class RunIndicator : FramedContainer
	{
		private readonly Kenedia.Modules.Core.Controls.LoadingSpinner _loadingSpinner;

		private readonly ChoyaSpinner _choyaSpinner;

		private readonly Kenedia.Modules.Core.Controls.Label _titleText;

		private readonly Kenedia.Modules.Core.Controls.Label _statusText;

		private readonly Kenedia.Modules.Core.Controls.Label _disclaimerText;

		private Point _screenPartionSize;

		private readonly CharacterSorting _characterSorting;

		private readonly CharacterSwapping _characterSwapping;

		private readonly SettingEntry<bool> _isEnabled;

		private readonly TextureManager _textureManager;

		private readonly SettingEntry<bool> _showChoya;

		public RunIndicator(CharacterSorting characterSorting, CharacterSwapping characterSwapping, SettingEntry<bool> isEnabled, TextureManager textureManager, SettingEntry<bool> showChoya)
		{
			_characterSorting = characterSorting;
			_characterSwapping = characterSwapping;
			_isEnabled = isEnabled;
			_textureManager = textureManager;
			_showChoya = showChoya;
			_screenPartionSize = new Point(Math.Min(640, GameService.Graphics.SpriteScreen.Size.X / 5), Math.Min(360, GameService.Graphics.SpriteScreen.Size.Y / 5));
			int x = (GameService.Graphics.SpriteScreen.Size.X - _screenPartionSize.X) / 2;
			int y = (GameService.Graphics.SpriteScreen.Size.Y - _screenPartionSize.Y) / 2;
			base.Parent = GameService.Graphics.SpriteScreen;
			base.Size = _screenPartionSize;
			base.Location = new Point(x, y);
			base.BackgroundImage = AsyncTexture2D.FromAssetId(1863949);
			base.TextureRectangle = new Rectangle(30, 0, base.BackgroundImage.Width - 30, base.BackgroundImage.Height);
			base.Visible = false;
			base.BackgroundImageColor = Color.White * 0.9f;
			_titleText = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Location = new Point(0, 10),
				Text = BaseModule<Characters, MainWindow, Settings, PathCollection, StaticHosting>.ModuleName,
				AutoSizeHeight = true,
				HorizontalAlignment = HorizontalAlignment.Center,
				Font = GameService.Content.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size36, ContentService.FontStyle.Regular),
				TextColor = Color.White,
				Width = base.Width
			};
			int spinnerSize = Math.Max(_screenPartionSize.Y / 2, 96);
			_loadingSpinner = new Kenedia.Modules.Core.Controls.LoadingSpinner
			{
				Parent = this,
				Size = new Point(spinnerSize, spinnerSize),
				Location = new Point((_screenPartionSize.X - spinnerSize) / 2, (_screenPartionSize.Y - spinnerSize) / 2 - 20),
				Visible = !_showChoya.Value
			};
			_choyaSpinner = new ChoyaSpinner(_textureManager)
			{
				Parent = this,
				Size = new Point(_screenPartionSize.X - 20, spinnerSize),
				Location = new Point(10, (_screenPartionSize.Y - spinnerSize) / 2 - 20),
				Visible = _showChoya.Value
			};
			_statusText = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Text = "Doing something very fancy right now ...",
				Height = 100,
				HorizontalAlignment = HorizontalAlignment.Center,
				Font = GameService.Content.DefaultFont18,
				Width = base.Width,
				Location = new Point(0, base.Height - 125)
			};
			_disclaimerText = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Text = "Any Key or Mouse press will cancel the current action!",
				Height = 50,
				HorizontalAlignment = HorizontalAlignment.Center,
				Font = GameService.Content.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size14, ContentService.FontStyle.Regular),
				Width = base.Width,
				Location = new Point(0, base.Height - 50)
			};
			_characterSwapping.StatusChanged += new EventHandler(CharacterSwapping_StatusChanged);
			_characterSorting.StatusChanged += new EventHandler(CharacterSorting_StatusChanged);
			_characterSwapping.Started += new EventHandler(ShowIndicator);
			_characterSorting.Started += new EventHandler(ShowIndicator);
			_characterSwapping.Finished += new EventHandler(HideIndicator);
			_characterSorting.Finished += new EventHandler(HideIndicator);
			_showChoya.SettingChanged += ShowChoya_SettingChanged;
		}

		private void ShowChoya_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			_loadingSpinner.Visible = !e.NewValue;
			_choyaSpinner.Visible = e.NewValue;
		}

		private void HideIndicator(object sender, EventArgs e)
		{
			Hide();
		}

		private void ShowIndicator(object sender, EventArgs e)
		{
			if (_isEnabled.Value)
			{
				_screenPartionSize = new Point(Math.Min(640, GameService.Graphics.SpriteScreen.Size.X / 5), Math.Min(360, GameService.Graphics.SpriteScreen.Size.Y / 5));
				int x = (GameService.Graphics.SpriteScreen.Size.X - _screenPartionSize.X) / 2;
				int y = (GameService.Graphics.SpriteScreen.Size.Y - _screenPartionSize.Y) / 2;
				base.Location = new Point(x, y);
				base.Size = _screenPartionSize;
				_titleText.Width = base.Width;
				_statusText.Width = base.Width;
				_statusText.Location = new Point(0, base.Height - 125);
				int spinnerSize = Math.Min(_screenPartionSize.Y / 2, 96);
				_loadingSpinner.Size = new Point(spinnerSize, spinnerSize);
				_loadingSpinner.Location = new Point((_screenPartionSize.X - spinnerSize) / 2, (_screenPartionSize.Y - spinnerSize) / 2 - 20);
				_choyaSpinner.Size = new Point(_screenPartionSize.X - 20, spinnerSize);
				_choyaSpinner.Location = new Point(10, (_screenPartionSize.Y - spinnerSize) / 2 - 20);
				_disclaimerText.Width = base.Width;
				_disclaimerText.Location = new Point(0, base.Height - 50);
				Invalidate();
				Show();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_showChoya.SettingChanged -= ShowChoya_SettingChanged;
			_characterSwapping.StatusChanged -= new EventHandler(CharacterSwapping_StatusChanged);
			_characterSorting.StatusChanged -= new EventHandler(CharacterSorting_StatusChanged);
			_characterSwapping.Started -= new EventHandler(ShowIndicator);
			_characterSorting.Started -= new EventHandler(ShowIndicator);
			_characterSwapping.Finished -= new EventHandler(HideIndicator);
			_characterSorting.Finished -= new EventHandler(HideIndicator);
		}

		private void CharacterSorting_StatusChanged(object sender, EventArgs e)
		{
			_statusText.Text = _characterSorting.Status;
		}

		private void CharacterSwapping_StatusChanged(object sender, EventArgs e)
		{
			_statusText.Text = _characterSwapping.Status;
		}
	}
}
