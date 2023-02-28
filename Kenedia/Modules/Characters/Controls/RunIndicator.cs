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
		private readonly LoadingSpinner _loadingSpinner;

		private readonly ChoyaSpinner _choyaSpinner;

		private readonly Label _titleText;

		private readonly Label _statusText;

		private readonly Label _disclaimerText;

		private Point _screenPartionSize;

		private readonly CharacterSorting _characterSorting;

		private readonly CharacterSwapping _characterSwapping;

		private readonly SettingEntry<bool> _isEnabled;

		private readonly TextureManager _textureManager;

		private readonly SettingEntry<bool> _showChoya;

		public RunIndicator(CharacterSorting characterSorting, CharacterSwapping characterSwapping, SettingEntry<bool> isEnabled, TextureManager textureManager, SettingEntry<bool> showChoya)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			_characterSorting = characterSorting;
			_characterSwapping = characterSwapping;
			_isEnabled = isEnabled;
			_textureManager = textureManager;
			_showChoya = showChoya;
			_screenPartionSize = new Point(Math.Min(640, ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X / 5), Math.Min(360, ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().Y / 5));
			int x = (((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X - _screenPartionSize.X) / 2;
			int y = (((Control)GameService.Graphics.get_SpriteScreen()).get_Size().Y - _screenPartionSize.Y) / 2;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Size(_screenPartionSize);
			((Control)this).set_Location(new Point(x, y));
			base.BackgroundImage = AsyncTexture2D.FromAssetId(1863949);
			base.TextureRectangle = new Rectangle(30, 0, base.BackgroundImage.get_Width() - 30, base.BackgroundImage.get_Height());
			((Control)this).set_Visible(false);
			base.BackgroundImageColor = Color.get_White() * 0.9f;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)this);
			((Control)label).set_Location(new Point(0, 10));
			((Label)label).set_Text(BaseModule<Characters, MainWindow, Settings>.ModuleName);
			((Label)label).set_AutoSizeHeight(true);
			((Label)label).set_HorizontalAlignment((HorizontalAlignment)1);
			((Label)label).set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)36, (FontStyle)0));
			((Label)label).set_TextColor(Color.get_White());
			((Control)label).set_Width(((Control)this).get_Width());
			_titleText = label;
			int spinnerSize = Math.Max(_screenPartionSize.Y / 2, 96);
			LoadingSpinner loadingSpinner = new LoadingSpinner();
			((Control)loadingSpinner).set_Parent((Container)(object)this);
			((Control)loadingSpinner).set_Size(new Point(spinnerSize, spinnerSize));
			((Control)loadingSpinner).set_Location(new Point((_screenPartionSize.X - spinnerSize) / 2, (_screenPartionSize.Y - spinnerSize) / 2 - 20));
			((Control)loadingSpinner).set_Visible(!_showChoya.get_Value());
			_loadingSpinner = loadingSpinner;
			ChoyaSpinner choyaSpinner = new ChoyaSpinner(_textureManager);
			((Control)choyaSpinner).set_Parent((Container)(object)this);
			((Control)choyaSpinner).set_Size(new Point(_screenPartionSize.X - 20, spinnerSize));
			((Control)choyaSpinner).set_Location(new Point(10, (_screenPartionSize.Y - spinnerSize) / 2 - 20));
			((Control)choyaSpinner).set_Visible(_showChoya.get_Value());
			_choyaSpinner = choyaSpinner;
			Label label2 = new Label();
			((Control)label2).set_Parent((Container)(object)this);
			((Label)label2).set_Text("Doing something very fancy right now ...");
			((Control)label2).set_Height(100);
			((Label)label2).set_HorizontalAlignment((HorizontalAlignment)1);
			((Label)label2).set_Font(GameService.Content.get_DefaultFont18());
			((Control)label2).set_Width(((Control)this).get_Width());
			((Control)label2).set_Location(new Point(0, ((Control)this).get_Height() - 125));
			_statusText = label2;
			Label label3 = new Label();
			((Control)label3).set_Parent((Container)(object)this);
			((Label)label3).set_Text("Any Key or Mouse press will cancel the current action!");
			((Control)label3).set_Height(50);
			((Label)label3).set_HorizontalAlignment((HorizontalAlignment)1);
			((Label)label3).set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)14, (FontStyle)0));
			((Control)label3).set_Width(((Control)this).get_Width());
			((Control)label3).set_Location(new Point(0, ((Control)this).get_Height() - 50));
			_disclaimerText = label3;
			_characterSwapping.StatusChanged += CharacterSwapping_StatusChanged;
			_characterSorting.StatusChanged += CharacterSorting_StatusChanged;
			_characterSwapping.Started += ShowIndicator;
			_characterSorting.Started += ShowIndicator;
			_characterSwapping.Finished += HideIndicator;
			_characterSorting.Finished += HideIndicator;
			_showChoya.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowChoya_SettingChanged);
		}

		private void ShowChoya_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			((Control)_loadingSpinner).set_Visible(!e.get_NewValue());
			((Control)_choyaSpinner).set_Visible(e.get_NewValue());
		}

		private void HideIndicator(object sender, EventArgs e)
		{
			((Control)this).Hide();
		}

		private void ShowIndicator(object sender, EventArgs e)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			if (_isEnabled.get_Value())
			{
				_screenPartionSize = new Point(Math.Min(640, ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X / 5), Math.Min(360, ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().Y / 5));
				int x = (((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X - _screenPartionSize.X) / 2;
				int y = (((Control)GameService.Graphics.get_SpriteScreen()).get_Size().Y - _screenPartionSize.Y) / 2;
				((Control)this).set_Location(new Point(x, y));
				((Control)this).set_Size(_screenPartionSize);
				((Control)_titleText).set_Width(((Control)this).get_Width());
				((Control)_statusText).set_Width(((Control)this).get_Width());
				((Control)_statusText).set_Location(new Point(0, ((Control)this).get_Height() - 125));
				int spinnerSize = Math.Min(_screenPartionSize.Y / 2, 96);
				((Control)_loadingSpinner).set_Size(new Point(spinnerSize, spinnerSize));
				((Control)_loadingSpinner).set_Location(new Point((_screenPartionSize.X - spinnerSize) / 2, (_screenPartionSize.Y - spinnerSize) / 2 - 20));
				((Control)_choyaSpinner).set_Size(new Point(_screenPartionSize.X - 20, spinnerSize));
				((Control)_choyaSpinner).set_Location(new Point(10, (_screenPartionSize.Y - spinnerSize) / 2 - 20));
				((Control)_disclaimerText).set_Width(((Control)this).get_Width());
				((Control)_disclaimerText).set_Location(new Point(0, ((Control)this).get_Height() - 50));
				((Control)this).Invalidate();
				((Control)this).Show();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_showChoya.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowChoya_SettingChanged);
			_characterSwapping.StatusChanged -= CharacterSwapping_StatusChanged;
			_characterSorting.StatusChanged -= CharacterSorting_StatusChanged;
			_characterSwapping.Started -= ShowIndicator;
			_characterSorting.Started -= ShowIndicator;
			_characterSwapping.Finished -= HideIndicator;
			_characterSorting.Finished -= HideIndicator;
		}

		private void CharacterSorting_StatusChanged(object sender, EventArgs e)
		{
			((Label)_statusText).set_Text(_characterSorting.Status);
		}

		private void CharacterSwapping_StatusChanged(object sender, EventArgs e)
		{
			((Label)_statusText).set_Text(_characterSwapping.Status);
		}
	}
}
