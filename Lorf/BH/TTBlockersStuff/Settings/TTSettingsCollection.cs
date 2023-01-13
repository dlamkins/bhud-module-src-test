using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Lorf.BH.TTBlockersStuff.Settings
{
	internal class TTSettingsCollection : SettingView<SettingCollection>
	{
		private FlowPanel _settingFlowPanel;

		private readonly SettingCollection _settings;

		private bool _lockBounds = true;

		private ViewContainer _lastSettingContainer;

		public bool LockBounds
		{
			get
			{
				return _lockBounds;
			}
			set
			{
				if (_lockBounds != value)
				{
					_lockBounds = value;
					UpdateBoundsLocking(_lockBounds);
				}
			}
		}

		public TTSettingsCollection(SettingEntry<SettingCollection> setting, int definedWidth = -1)
			: base(setting, definedWidth)
		{
			_settings = setting.Value;
		}

		public TTSettingsCollection(SettingCollection settings, int definedWidth = -1)
			: this(new SettingEntry<SettingCollection>
			{
				Value = settings
			}, definedWidth)
		{
		}

		private void UpdateBoundsLocking(bool locked)
		{
			if (_settingFlowPanel != null)
			{
				_settingFlowPanel.ShowBorder = !locked;
				_settingFlowPanel.CanCollapse = !locked;
			}
		}

		protected override void BuildSetting(Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			_settingFlowPanel = new FlowPanel
			{
				Size = buildPanel.Size,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(5f, 2f),
				OuterControlPadding = new Vector2(10f, 15f),
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Point(0, 15),
				Parent = buildPanel
			};
			foreach (SettingEntry setting in _settings.Where((SettingEntry s) => s.SessionDefined))
			{
				IView settingView;
				if ((settingView = SettingView.FromType(setting, _settingFlowPanel.Width)) != null)
				{
					_lastSettingContainer = new ViewContainer
					{
						WidthSizingMode = SizingMode.Fill,
						HeightSizingMode = SizingMode.AutoSize,
						Parent = _settingFlowPanel
					};
					_lastSettingContainer.Show(settingView);
					SettingsView subSettingsView2 = settingView as SettingsView;
					if (subSettingsView2 != null)
					{
						subSettingsView2.LockBounds = false;
					}
				}
				else if (setting.SettingType == typeof(Color))
				{
					settingView = new ColorPickerSettingView(setting as SettingEntry<Color>, _settingFlowPanel.Width);
					_lastSettingContainer = new ViewContainer
					{
						WidthSizingMode = SizingMode.Fill,
						HeightSizingMode = SizingMode.AutoSize,
						Parent = _settingFlowPanel
					};
					_lastSettingContainer.Show(settingView);
					SettingsView subSettingsView = settingView as SettingsView;
					if (subSettingsView != null)
					{
						subSettingsView.LockBounds = false;
					}
				}
			}
			UpdateBoundsLocking(_lockBounds);
		}

		protected override void RefreshDisplayName(string displayName)
		{
			_settingFlowPanel.Title = displayName;
		}

		protected override void RefreshDescription(string description)
		{
			_settingFlowPanel.BasicTooltipText = description;
		}

		protected override void RefreshValue(SettingCollection value)
		{
		}
	}
}
