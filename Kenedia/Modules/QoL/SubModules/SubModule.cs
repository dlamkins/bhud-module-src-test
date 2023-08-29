using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
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
				Common.SetProperty(ref _enabled, value, OnEnabledChanged);
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
				Texture = AsyncTexture2D.op_Implicit(BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.ContentsManager.GetTexture($"textures\\{SubModuleType}.png")),
				HoveredTexture = AsyncTexture2D.op_Implicit(BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.ContentsManager.GetTexture($"textures\\{SubModuleType}_Hovered.png"))
			};
			ModuleButton obj = new ModuleButton
			{
				Icon = Icon
			};
			((Control)obj).set_BasicTooltipText(SubModuleType.ToString());
			obj.Checked = EnabledSetting.get_Value();
			((Control)obj).set_Size(new Point(32));
			((Control)obj).set_Visible(EnabledSetting.get_Value());
			obj.OnCheckChanged = delegate(bool b)
			{
				Enabled = b;
			};
			obj.Module = this;
			ToggleControl = obj;
		}

		public abstract void Update(GameTime gameTime);

		public abstract void CreateSettingsPanel(FlowPanel flowPanel, int width);

		private void OnEnabledChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			(e.NewValue ? new Action(Enable) : new Action(Disable))();
			EnabledSetting.set_Value((e.NewValue ? ((byte)1) : ((byte)0)) != 0);
			HotbarButton toggle = ToggleControl;
			if (toggle == null)
			{
				return;
			}
			toggle.Checked = Enabled;
			ModuleButton toggleControl = ToggleControl;
			if (toggleControl != null)
			{
				Container parent = ((Control)toggleControl).get_Parent();
				if (parent != null)
				{
					((Control)parent).RecalculateLayout();
				}
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
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			Settings = settings.AddSubCollection($"{SubModuleType}", true);
			Settings.set_RenderInUi(false);
			EnabledSetting = Settings.DefineSetting<bool>("EnabledSetting", false, (Func<string>)null, (Func<string>)null);
			HotKey = Settings.DefineSetting<KeyBinding>("HotKey", new KeyBinding((Keys)0), (Func<string>)(() => string.Format(strings.HotkeyEntry_Name, $"{SubModuleType}")), (Func<string>)(() => string.Format(strings.HotkeyEntry_Description, $"{SubModuleType}")));
			ShowInHotbar = Settings.DefineSetting<bool>("ShowInHotbar", true, (Func<string>)(() => string.Format(strings.ShowInHotbar_Name, $"{SubModuleType}")), (Func<string>)(() => string.Format(strings.ShowInHotbar_Description, $"{SubModuleType}")));
			HotKey.get_Value().set_Enabled(true);
			HotKey.get_Value().add_Activated((EventHandler<EventArgs>)HotKey_Activated);
			ShowInHotbar.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowInHotbar_SettingChanged);
		}

		private void ShowInHotbar_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (ToggleControl != null)
			{
				ModuleButton toggleControl = ToggleControl;
				object obj;
				if (toggleControl == null)
				{
					obj = null;
				}
				else
				{
					Container parent = ((Control)toggleControl).get_Parent();
					obj = ((parent != null) ? ((Control)parent).get_Parent() : null);
				}
				ModuleHotbar moduleHotbar = obj as ModuleHotbar;
				if (moduleHotbar != null)
				{
					moduleHotbar.SetButtonsExpanded();
					((Control)moduleHotbar).RecalculateLayout();
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
				LocalizingService.LocaleChanged += LocalizingService_LocaleChanged;
				LocalizingService_LocaleChanged();
				Enabled = EnabledSetting.get_Value();
			}
		}

		public virtual void Unload()
		{
			if (!_unloaded)
			{
				_unloaded = true;
				ModuleButton toggleControl = ToggleControl;
				if (toggleControl != null)
				{
					((Control)toggleControl).Dispose();
				}
				((IEnumerable<IDisposable>)UI_Elements).DisposeAll();
				HotKey.get_Value().remove_Activated((EventHandler<EventArgs>)HotKey_Activated);
				LocalizingService.LocaleChanged -= LocalizingService_LocaleChanged;
			}
		}
	}
}
