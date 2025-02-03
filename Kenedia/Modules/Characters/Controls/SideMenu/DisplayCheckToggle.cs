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
	public class DisplayCheckToggle : Kenedia.Modules.Core.Controls.FlowPanel
	{
		private readonly AsyncTexture2D _eye;

		private readonly AsyncTexture2D _eyeHovered;

		private readonly AsyncTexture2D _telescope;

		private readonly AsyncTexture2D _telescopeHovered;

		private readonly AsyncTexture2D _info;

		private readonly AsyncTexture2D _infoHovered;

		private readonly ImageToggle _showButton;

		private readonly ImageToggle _checkButton;

		private readonly ImageToggle _showTooltipButton;

		private readonly Kenedia.Modules.Core.Controls.Label _textLabel;

		private readonly string _key;

		private readonly Settings _settings;

		private bool _checkChecked;

		private bool _showChecked;

		private bool _showTooltip;

		public string Text
		{
			get
			{
				return _textLabel.Text;
			}
			set
			{
				_textLabel.Text = value;
			}
		}

		public string CheckTooltip
		{
			get
			{
				return _checkButton.BasicTooltipText;
			}
			set
			{
				_checkButton.BasicTooltipText = value;
			}
		}

		public string ShowTooltipTooltip
		{
			get
			{
				return _showTooltipButton.BasicTooltipText;
			}
			set
			{
				_showTooltipButton.BasicTooltipText = value;
			}
		}

		public string DisplayTooltip
		{
			get
			{
				return _showButton.BasicTooltipText;
			}
			set
			{
				_showButton.BasicTooltipText = value;
			}
		}

		public bool ShowTooltipChecked
		{
			get
			{
				return _showTooltip;
			}
			set
			{
				_showTooltip = value;
				_showTooltipButton.Checked = value;
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

		public event EventHandler<bool> ShowTooltipChanged;

		public event EventHandler<bool> CheckChanged;

		public DisplayCheckToggle(TextureManager textureManager, bool displayButton_Checked = true, bool checkbox_Checked = true, bool showTooltip_Checked = true)
		{
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			_eye = (AsyncTexture2D)textureManager.GetControlTexture(TextureManager.ControlTextures.Eye_Button);
			_eyeHovered = (AsyncTexture2D)textureManager.GetControlTexture(TextureManager.ControlTextures.Eye_Button_Hovered);
			_telescope = (AsyncTexture2D)textureManager.GetControlTexture(TextureManager.ControlTextures.Telescope_Button);
			_telescopeHovered = (AsyncTexture2D)textureManager.GetControlTexture(TextureManager.ControlTextures.Telescope_Button_Hovered);
			_info = (AsyncTexture2D)textureManager.GetControlTexture(TextureManager.ControlTextures.Info_Button);
			_infoHovered = (AsyncTexture2D)textureManager.GetControlTexture(TextureManager.ControlTextures.Info_Button_Hovered);
			WidthSizingMode = SizingMode.Fill;
			HeightSizingMode = SizingMode.AutoSize;
			base.FlowDirection = ControlFlowDirection.SingleLeftToRight;
			base.ControlPadding = new Vector2(5f, 0f);
			_showButton = new ImageToggle
			{
				Parent = this,
				ShowX = true,
				Texture = _eye,
				HoveredTexture = _eyeHovered,
				Size = new Point(20, 20),
				Checked = displayButton_Checked
			};
			ShowChecked = _showButton.Checked;
			_showButton.CheckedChanged += new EventHandler<CheckChangedEvent>(SettingChanged);
			_checkButton = new ImageToggle
			{
				Parent = this,
				ShowX = true,
				Texture = _telescope,
				HoveredTexture = _telescopeHovered,
				Size = new Point(20, 20),
				Checked = checkbox_Checked
			};
			CheckChecked = _checkButton.Checked;
			_checkButton.CheckedChanged += new EventHandler<CheckChangedEvent>(SettingChanged);
			_showTooltipButton = new ImageToggle
			{
				Parent = this,
				ShowX = true,
				Texture = _info,
				HoveredTexture = _infoHovered,
				Size = new Point(20, 20),
				Checked = showTooltip_Checked
			};
			ShowTooltipChecked = _showTooltipButton.Checked;
			_showTooltipButton.CheckedChanged += new EventHandler<CheckChangedEvent>(SettingChanged);
			_textLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = 20,
				VerticalAlignment = VerticalAlignment.Middle,
				AutoSizeWidth = true
			};
		}

		public DisplayCheckToggle(TextureManager textureManager, Settings settings, string key, bool show = true, bool check = true, bool tooltip = true)
			: this(textureManager)
		{
			_settings = settings;
			_key = key;
			if (!settings.DisplayToggles.Value.ContainsKey(_key))
			{
				settings.DisplayToggles.Value.Add(_key, new ShowCheckPair(show, check, tooltip));
			}
			_showButton.Checked = settings.DisplayToggles.Value[_key].Show;
			ShowChecked = _showButton.Checked;
			_checkButton.Checked = settings.DisplayToggles.Value[_key].Check;
			CheckChecked = _checkButton.Checked;
			_showTooltipButton.Checked = settings.DisplayToggles.Value[_key].ShowTooltip;
			ShowTooltipChecked = _showTooltipButton.Checked;
		}

		private void SettingChanged(object sender, CheckChangedEvent e)
		{
			if (_settings != null)
			{
				_settings.DisplayToggles.Value = new Dictionary<string, ShowCheckPair>(_settings.DisplayToggles.Value) { [_key] = new ShowCheckPair(_showButton.Checked, _checkButton.Checked, _showTooltipButton.Checked) };
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
			if (_showTooltip != _showTooltipButton.Checked)
			{
				_showTooltip = _showTooltipButton.Checked;
				this.ShowTooltipChanged?.Invoke(this, _showTooltipButton.Checked);
			}
			this.Changed?.Invoke(this, new Tuple<bool, bool>(_showButton.Checked, _checkButton.Checked));
		}
	}
}
