using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using felix.BlishEmotes.Strings;

namespace felix.BlishEmotes.UI.Views
{
	internal class GlobalSettingsView : View
	{
		private ModuleSettings _settings;

		private FlowPanel _globalPanel;

		private FlowPanel _radialPanel;

		private FlowPanel _emotesRadialPanel;

		private LoadingSpinner _emotesRadialSpinner;

		private const int _labelWidth = 200;

		private const int _controlWidth = 150;

		private const int _height = 20;

		private const int _padding = 5;

		public GlobalSettingsView(ModuleSettings settings)
		{
			_settings = settings;
			_settings.OnEmotesLoaded += delegate
			{
				BuildEmotesRadialEnabledPanel();
			};
		}

		private FlowPanel CreatePanel(Container parent, Point location, int width)
		{
			return new FlowPanel
			{
				CanCollapse = false,
				CanScroll = false,
				Parent = parent,
				Location = location,
				Width = width,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleTopToBottom
			};
		}

		private FlowPanel CreateRowPanel(Container parent)
		{
			return new FlowPanel
			{
				CanCollapse = false,
				CanScroll = false,
				Parent = parent,
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				OuterControlPadding = new Vector2(5f, 5f)
			};
		}

		protected override void Build(Container buildPanel)
		{
			_globalPanel = CreatePanel(buildPanel, new Point(0, 0), 450);
			BuildGlobalPanel();
			_radialPanel = CreatePanel(buildPanel, new Point(400, 0), 450);
			BuildRadialPanel();
			new Label
			{
				Parent = buildPanel,
				Text = Common.settings_radial_emotesEnabled,
				Location = new Point(0, 160),
				Size = new Point(170, 40),
				Font = GameService.Content.DefaultFont18
			};
			_emotesRadialSpinner = new LoadingSpinner
			{
				Parent = buildPanel,
				Location = new Point(170, 160),
				Size = new Point(40, 40),
				Visible = true
			};
			_emotesRadialPanel = CreatePanel(buildPanel, new Point(0, 200), 0);
			_emotesRadialPanel.HeightSizingMode = SizingMode.Fill;
			_emotesRadialPanel.WidthSizingMode = SizingMode.Fill;
			_emotesRadialPanel.FlowDirection = ControlFlowDirection.TopToBottom;
			BuildEmotesRadialEnabledPanel();
		}

		private void BuildGlobalPanel()
		{
			FlowPanel _hideCornerIconRow = CreateRowPanel(_globalPanel);
			new Label
			{
				Parent = _hideCornerIconRow,
				Text = Common.settings_global_hideCornerIcon,
				Size = new Point(200, 20),
				Location = new Point(0, 0)
			};
			Checkbox _hideCornerIconCheckbox = new Checkbox
			{
				Parent = _hideCornerIconRow,
				Checked = _settings.GlobalHideCornerIcon.Value,
				Size = new Point(150, 20),
				Location = new Point(205, 0)
			};
			_hideCornerIconCheckbox.CheckedChanged += delegate
			{
				_settings.GlobalHideCornerIcon.Value = _hideCornerIconCheckbox.Checked;
			};
			FlowPanel _toggleEmoteListKeybindRow = CreateRowPanel(_globalPanel);
			new Label
			{
				Parent = _toggleEmoteListKeybindRow,
				Text = Common.settings_global_keybindToggleEmoteList,
				Size = new Point(200, 20),
				Location = new Point(0, 0)
			};
			new KeybindingAssigner(_settings.GlobalKeyBindToggleEmoteList.Value)
			{
				Parent = _toggleEmoteListKeybindRow,
				NameWidth = 0,
				Padding = Thickness.Zero,
				Size = new Point(150, 20),
				Location = new Point(198, 0)
			};
			FlowPanel _useRadialRow = CreateRowPanel(_globalPanel);
			new Label
			{
				Parent = _useRadialRow,
				Text = Common.settings_global_useRadialMenu,
				Size = new Point(200, 20),
				Location = new Point(0, 0)
			};
			Dropdown _useRadialDropdown = new Dropdown
			{
				Parent = _useRadialRow,
				Size = new Point(150, 20),
				Location = new Point(205, 0)
			};
			_useRadialDropdown.Items.Add("List menu");
			_useRadialDropdown.Items.Add("Radial menu");
			_useRadialDropdown.SelectedItem = (_settings.GlobalUseRadialMenu.Value ? "Radial menu" : "List menu");
			_useRadialDropdown.ValueChanged += delegate
			{
				_settings.GlobalUseRadialMenu.Value = _useRadialDropdown.SelectedItem.Equals("Radial menu");
			};
			FlowPanel _useCategoriesRow = CreateRowPanel(_globalPanel);
			new Label
			{
				Parent = _useCategoriesRow,
				Text = Common.settings_global_useCategories,
				Size = new Point(200, 20),
				Location = new Point(0, 0)
			};
			Checkbox _useCategoriesCheckbox = new Checkbox
			{
				Parent = _useCategoriesRow,
				Checked = _settings.GlobalUseCategories.Value,
				Size = new Point(150, 20),
				Location = new Point(205, 0)
			};
			_useCategoriesCheckbox.CheckedChanged += delegate
			{
				_settings.GlobalUseCategories.Value = _useCategoriesCheckbox.Checked;
			};
		}

		private void BuildRadialPanel()
		{
			FlowPanel _spawnAtCursorRow = CreateRowPanel(_radialPanel);
			new Label
			{
				Parent = _spawnAtCursorRow,
				Text = Common.settings_radial_spawnAtCursor,
				Size = new Point(200, 20),
				Location = new Point(0, 0)
			};
			Checkbox _spawnAtCursorCheckbox = new Checkbox
			{
				Parent = _spawnAtCursorRow,
				Checked = _settings.RadialSpawnAtCursor.Value,
				Size = new Point(150, 20),
				Location = new Point(205, 0)
			};
			_spawnAtCursorCheckbox.CheckedChanged += delegate
			{
				_settings.RadialSpawnAtCursor.Value = _spawnAtCursorCheckbox.Checked;
			};
			FlowPanel _actionCamKeybindRow = CreateRowPanel(_radialPanel);
			new Label
			{
				Parent = _actionCamKeybindRow,
				Text = Common.settings_radial_actionCamKeybind,
				Size = new Point(200, 20),
				Location = new Point(0, 0)
			};
			new KeybindingAssigner(_settings.RadialToggleActionCameraKeyBind.Value)
			{
				Parent = _actionCamKeybindRow,
				NameWidth = 0,
				Padding = Thickness.Zero,
				Size = new Point(150, 20),
				Location = new Point(198, 0)
			};
			FlowPanel _radiusModifierRow = CreateRowPanel(_radialPanel);
			new Label
			{
				Parent = _radiusModifierRow,
				Text = Common.settings_radial_radiusModifier,
				Size = new Point(200, 20),
				Location = new Point(0, 0)
			};
			TrackBar _radiusModifierTrackBar = new TrackBar
			{
				Parent = _radiusModifierRow,
				Value = _settings.RadialRadiusModifier.Value * 100f,
				MinValue = 25f,
				MaxValue = 50f,
				Size = new Point(150, 20),
				Location = new Point(205, 0)
			};
			_radiusModifierTrackBar.ValueChanged += delegate
			{
				_settings.RadialRadiusModifier.Value = _radiusModifierTrackBar.Value / 100f;
			};
			FlowPanel _innerRadiusPercentageRow = CreateRowPanel(_radialPanel);
			new Label
			{
				Parent = _innerRadiusPercentageRow,
				Text = Common.settings_radial_innerRadiusPercentage,
				Size = new Point(200, 20),
				Location = new Point(0, 0),
				BasicTooltipText = Common.settings_radial_innerRadiusPercentage_description
			};
			TrackBar _innerRadiusPercentageTrackBar = new TrackBar
			{
				Parent = _innerRadiusPercentageRow,
				Value = _settings.RadialInnerRadiusPercentage.Value * 100f,
				MinValue = 0f,
				MaxValue = 50f,
				Size = new Point(150, 20),
				Location = new Point(205, 0),
				BasicTooltipText = Common.settings_radial_innerRadiusPercentage_description
			};
			_innerRadiusPercentageTrackBar.ValueChanged += delegate
			{
				_settings.RadialInnerRadiusPercentage.Value = _innerRadiusPercentageTrackBar.Value / 100f;
			};
			FlowPanel _iconSizeModifierRow = CreateRowPanel(_radialPanel);
			new Label
			{
				Parent = _iconSizeModifierRow,
				Text = Common.settings_radial_iconSizeModifier,
				Size = new Point(200, 20),
				Location = new Point(0, 0)
			};
			TrackBar _iconSizeModifierTrackBar = new TrackBar
			{
				Parent = _iconSizeModifierRow,
				Value = _settings.RadialIconSizeModifier.Value * 100f,
				MinValue = 25f,
				MaxValue = 75f,
				Size = new Point(150, 20),
				Location = new Point(205, 0)
			};
			_iconSizeModifierTrackBar.ValueChanged += delegate
			{
				_settings.RadialIconSizeModifier.Value = _iconSizeModifierTrackBar.Value / 100f;
			};
			FlowPanel _iconOpacityRow = CreateRowPanel(_radialPanel);
			new Label
			{
				Parent = _iconOpacityRow,
				Text = Common.settings_radial_iconOpacity,
				Size = new Point(200, 20),
				Location = new Point(0, 0)
			};
			TrackBar _iconOpacityTrackBar = new TrackBar
			{
				Parent = _iconOpacityRow,
				Value = _settings.RadialIconOpacity.Value * 100f,
				MinValue = 50f,
				MaxValue = 75f,
				Size = new Point(150, 20),
				Location = new Point(205, 0)
			};
			_iconOpacityTrackBar.ValueChanged += delegate
			{
				_settings.RadialIconOpacity.Value = _iconOpacityTrackBar.Value / 100f;
			};
		}

		public void BuildEmotesRadialEnabledPanel()
		{
			_emotesRadialPanel?.ClearChildren();
			if (_settings.EmotesRadialEnabledMap.Count() > 0)
			{
				_emotesRadialSpinner?.Dispose();
			}
			List<KeyValuePair<Emote, SettingEntry<bool>>> list = _settings.EmotesRadialEnabledMap.ToList();
			list.Sort((KeyValuePair<Emote, SettingEntry<bool>> a, KeyValuePair<Emote, SettingEntry<bool>> b) => a.Value.DisplayName.CompareTo(b.Value.DisplayName));
			foreach (KeyValuePair<Emote, SettingEntry<bool>> entry in list)
			{
				FlowPanel _emoteRadialEnabledRow = CreateRowPanel(_emotesRadialPanel);
				_emoteRadialEnabledRow.OuterControlPadding = new Vector2(10f, 5f);
				new Label
				{
					Parent = _emoteRadialEnabledRow,
					Text = entry.Value.DisplayName,
					Size = new Point(100, 20),
					Location = new Point(0, 0)
				};
				Checkbox _emoteRadialEnableCheckbox = new Checkbox
				{
					Parent = _emoteRadialEnabledRow,
					Checked = entry.Value.Value,
					Size = new Point(150, 20),
					Location = new Point(205, 0)
				};
				_emoteRadialEnableCheckbox.CheckedChanged += delegate
				{
					entry.Value.Value = _emoteRadialEnableCheckbox.Checked;
				};
			}
		}

		protected override void Unload()
		{
			_globalPanel?.Dispose();
			_radialPanel?.Dispose();
			_emotesRadialPanel?.Dispose();
			_emotesRadialSpinner?.Dispose();
		}
	}
}
