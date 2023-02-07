using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls.SideMenu
{
	public class DisplayCheckToggle : FlowPanel
	{
		private readonly AsyncTexture2D _eye;

		private readonly AsyncTexture2D _eyeHovered;

		private readonly AsyncTexture2D _telescope;

		private readonly AsyncTexture2D _telescopeHovered;

		private readonly ImageToggle _showButton;

		private readonly ImageToggle _checkButton;

		private readonly Label _textLabel;

		private readonly string _key;

		private readonly SettingsModel _settings;

		private bool _checkChecked;

		private bool _showChecked;

		public string Text
		{
			get
			{
				return ((Label)_textLabel).get_Text();
			}
			set
			{
				((Label)_textLabel).set_Text(value);
			}
		}

		public string CheckTooltip
		{
			get
			{
				return ((Control)_checkButton).get_BasicTooltipText();
			}
			set
			{
				((Control)_checkButton).set_BasicTooltipText(value);
			}
		}

		public string DisplayTooltip
		{
			get
			{
				return ((Control)_showButton).get_BasicTooltipText();
			}
			set
			{
				((Control)_showButton).set_BasicTooltipText(value);
			}
		}

		public bool CheckChecked
		{
			get
			{
				return _checkChecked;
			}
			set
			{
				_checkChecked = value;
				_checkButton.Checked = value;
			}
		}

		public bool ShowChecked
		{
			get
			{
				return _showChecked;
			}
			set
			{
				_showChecked = value;
				_showButton.Checked = value;
			}
		}

		public event EventHandler<Tuple<bool, bool>> Changed;

		public event EventHandler<bool> ShowChanged;

		public event EventHandler<bool> CheckChanged;

		public DisplayCheckToggle(TextureManager textureManager, bool displayButton_Checked = true, bool checkbox_Checked = true)
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			_eye = AsyncTexture2D.op_Implicit(textureManager.GetControlTexture(TextureManager.ControlTextures.Eye_Button));
			_eyeHovered = AsyncTexture2D.op_Implicit(textureManager.GetControlTexture(TextureManager.ControlTextures.Eye_Button_Hovered));
			_telescope = AsyncTexture2D.op_Implicit(textureManager.GetControlTexture(TextureManager.ControlTextures.Telescope_Button));
			_telescopeHovered = AsyncTexture2D.op_Implicit(textureManager.GetControlTexture(TextureManager.ControlTextures.Telescope_Button_Hovered));
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)2);
			((FlowPanel)this).set_ControlPadding(new Vector2(5f, 0f));
			ImageToggle imageToggle = new ImageToggle();
			((Control)imageToggle).set_Parent((Container)(object)this);
			imageToggle.ShowX = true;
			imageToggle.Texture = _eye;
			imageToggle.HoveredTexture = _eyeHovered;
			((Control)imageToggle).set_Size(new Point(20, 20));
			imageToggle.Checked = displayButton_Checked;
			_showButton = imageToggle;
			ShowChecked = _showButton.Checked;
			_showButton.CheckedChanged += SettingChanged;
			ImageToggle imageToggle2 = new ImageToggle();
			((Control)imageToggle2).set_Parent((Container)(object)this);
			imageToggle2.ShowX = true;
			imageToggle2.Texture = _telescope;
			imageToggle2.HoveredTexture = _telescopeHovered;
			((Control)imageToggle2).set_Size(new Point(20, 20));
			imageToggle2.Checked = checkbox_Checked;
			_checkButton = imageToggle2;
			CheckChecked = _checkButton.Checked;
			_checkButton.CheckedChanged += SettingChanged;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)this);
			((Control)label).set_Height(20);
			((Label)label).set_VerticalAlignment((VerticalAlignment)1);
			((Label)label).set_AutoSizeWidth(true);
			_textLabel = label;
		}

		public DisplayCheckToggle(TextureManager textureManager, SettingsModel settings, string key, bool show = true, bool check = true)
			: this(textureManager)
		{
			_settings = settings;
			_key = key;
			if (!settings.DisplayToggles.get_Value().ContainsKey(_key))
			{
				settings.DisplayToggles.get_Value().Add(_key, new ShowCheckPair(show, check));
			}
			_showButton.Checked = settings.DisplayToggles.get_Value()[_key].Show;
			ShowChecked = _showButton.Checked;
			_checkButton.Checked = settings.DisplayToggles.get_Value()[_key].Check;
			CheckChecked = _checkButton.Checked;
		}

		private void SettingChanged(object sender, CheckChangedEvent e)
		{
			if (_settings != null)
			{
				_settings.DisplayToggles.set_Value(new Dictionary<string, ShowCheckPair>(_settings.DisplayToggles.get_Value()) { [_key] = new ShowCheckPair(_showButton.Checked, _checkButton.Checked) });
			}
			if (_checkChecked != _checkButton.Checked)
			{
				_checkChecked = _checkButton.Checked;
				this.CheckChanged?.Invoke(this, _checkButton.Checked);
			}
			if (_showChecked != _showButton.Checked)
			{
				_showChecked = _showButton.Checked;
				this.ShowChanged?.Invoke(this, _showButton.Checked);
			}
			this.Changed?.Invoke(this, new Tuple<bool, bool>(_showButton.Checked, _checkButton.Checked));
		}
	}
}
