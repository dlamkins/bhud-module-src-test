using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.QoL.Controls;
using Kenedia.Modules.QoL.Res;
using Kenedia.Modules.QoL.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.QoL.SubModules
{
	public abstract class SubModule
	{
		private readonly SettingCollection _settings;

		private bool _loaded;

		private bool _unloaded;

		private bool _enabled;

		private Func<string> _localizedName;

		private Func<string> _localizedDescription;

		protected SettingCollection Settings;

		protected SubModuleUI UI_Elements = new SubModuleUI();

		public abstract SubModuleType SubModuleType { get; }

		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				Common.SetProperty(ref _enabled, value, new ValueChangedEventHandler<bool>(OnEnabledChanged));
			}
		}

		public Func<string> LocalizedName
		{
			get
			{
				return _localizedName;
			}
			set
			{
				Common.SetProperty(ref _localizedName, value);
			}
		}

		public Func<string> LocalizedDescription
		{
			get
			{
				return _localizedDescription;
			}
			set
			{
				Common.SetProperty(ref _localizedDescription, value);
			}
		}

		public ModuleButton ToggleControl { get; }

		public DetailedTexture Icon { get; }

		public string Name { get; set; }

		public string Description { get; set; }

		public SettingEntry<bool> EnabledSetting { get; set; }

		public SettingEntry<bool> ShowInHotbar { get; set; }

		public SettingEntry<KeyBinding> HotKey { get; set; }

		public SubModule(SettingCollection settings)
		{
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			_settings = settings;
			DefineSettings(_settings);
			Name = SubModuleType.ToString();
			Icon = new DetailedTexture
			{
				Texture = (AsyncTexture2D)BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.ContentsManager.GetTexture($"textures\\{SubModuleType}.png"),
				HoveredTexture = (AsyncTexture2D)BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.ContentsManager.GetTexture($"textures\\{SubModuleType}_Hovered.png")
			};
			ToggleControl = new ModuleButton
			{
				Icon = Icon,
				BasicTooltipText = SubModuleType.ToString(),
				Checked = EnabledSetting.Value,
				Size = new Point(32),
				Visible = EnabledSetting.Value,
				OnCheckChanged = delegate(bool b)
				{
					Enabled = b;
				},
				Module = this
			};
		}

		public abstract void Update(GameTime gameTime);

		public abstract void CreateSettingsPanel(Kenedia.Modules.Core.Controls.FlowPanel flowPanel, int width);

		private void OnEnabledChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<bool> e)
		{
			(e.NewValue ? new Action(Enable) : new Action(Disable))();
			EnabledSetting.Value = (e.NewValue ? ((byte)1) : ((byte)0)) != 0;
			HotbarButton toggle = ToggleControl;
			if (toggle != null)
			{
				toggle.Checked = Enabled;
				ToggleControl?.Parent?.RecalculateLayout();
			}
		}

		private void LocalizingService_LocaleChanged(object sender = null, EventArgs e = null)
		{
			SwitchLanguage();
		}

		protected virtual void Enable()
		{
			Enabled = true;
		}

		protected virtual void Disable()
		{
			Enabled = false;
		}

		protected virtual void SwitchLanguage()
		{
			Name = LocalizedName?.Invoke() ?? Name;
			Description = LocalizedDescription?.Invoke() ?? Description;
		}

		protected virtual void DefineSettings(SettingCollection settings)
		{
			Settings = settings.AddSubCollection($"{SubModuleType}", lazyLoaded: true);
			Settings.RenderInUi = false;
			EnabledSetting = Settings.DefineSetting("EnabledSetting", defaultValue: false);
			HotKey = Settings.DefineSetting("HotKey", new KeyBinding((Keys)0), () => string.Format(strings.HotkeyEntry_Name, $"{SubModuleType}"), () => string.Format(strings.HotkeyEntry_Description, $"{SubModuleType}"));
			ShowInHotbar = Settings.DefineSetting("ShowInHotbar", defaultValue: true, () => string.Format(strings.ShowInHotbar_Name, $"{SubModuleType}"), () => string.Format(strings.ShowInHotbar_Description, $"{SubModuleType}"));
			HotKey.Value.Enabled = true;
			HotKey.Value.Activated += HotKey_Activated;
			ShowInHotbar.SettingChanged += ShowInHotbar_SettingChanged;
		}

		private void ShowInHotbar_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			if (ToggleControl != null)
			{
				ModuleHotbar moduleHotbar = ToggleControl?.Parent?.Parent as ModuleHotbar;
				if (moduleHotbar != null)
				{
					moduleHotbar.SetButtonsExpanded();
					moduleHotbar.RecalculateLayout();
				}
			}
		}

		private void HotKey_Activated(object sender, EventArgs e)
		{
			Enabled = !Enabled;
		}

		public virtual void Load()
		{
			if (!_loaded)
			{
				_loaded = true;
				LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(LocalizingService_LocaleChanged);
				LocalizingService_LocaleChanged();
				Enabled = EnabledSetting.Value;
			}
		}

		public virtual void Unload()
		{
			if (!_unloaded)
			{
				_unloaded = true;
				ToggleControl?.Dispose();
				UI_Elements.DisposeAll();
				HotKey.Value.Activated -= HotKey_Activated;
				LocalizingService.LocaleChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(LocalizingService_LocaleChanged);
			}
		}
	}
}
