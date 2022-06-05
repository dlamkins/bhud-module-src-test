using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.QoL.Strings;
using Kenedia.Modules.QoL.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL.Classes
{
	public abstract class SubModule
	{
		protected CornerIcon CornerIcon;

		public string Name;

		public bool Loaded;

		public bool Include;

		public Texture2D ModuleIcon;

		public Texture2D ModuleIconHovered;

		public Texture2D ModuleIcon_Active;

		public Texture2D ModuleIconHovered_Active;

		public SettingEntry<KeyBinding> ToggleModule_Key;

		public SettingEntry<bool> Enabled;

		public SettingEntry<bool> ShowOnBar;

		public SettingEntry<bool> _Active;

		public Ticks Ticks = new Ticks();

		public Hotbar_Button Hotbar_Button;

		public bool Active
		{
			get
			{
				if (_Active != null)
				{
					return _Active.get_Value();
				}
				return false;
			}
			set
			{
				if (_Active != null)
				{
					_Active.set_Value(value);
				}
			}
		}

		public event EventHandler Toggled;

		public SubModule()
		{
			QoL.ModuleInstance.LanguageChanged += UpdateLanguage;
			ModuleIcon = QoL.ModuleInstance.TextureManager.getIcon(_Icons.ModuleIcon);
			ModuleIconHovered = QoL.ModuleInstance.TextureManager.getIcon(_Icons.ModuleIcon_HoveredWhite);
			ModuleIcon_Active = QoL.ModuleInstance.TextureManager.getIcon(_Icons.ModuleIcon_Active);
			ModuleIconHovered_Active = QoL.ModuleInstance.TextureManager.getIcon(_Icons.ModuleIcon_Active_HoveredWhite);
		}

		public virtual void Initialize()
		{
		}

		public virtual void DefineSettings(SettingCollection settings)
		{
			SettingCollection internal_settings = settings.AddSubCollection(Name + " Internal Settings", false, false);
			_Active = internal_settings.DefineSetting<bool>("_Active", false, (Func<string>)null, (Func<string>)null);
		}

		private void CornerIcon_Click(object sender, MouseEventArgs e)
		{
			ToggleModule();
		}

		public abstract void LoadData();

		public virtual void ToggleModule()
		{
			if (Enabled.get_Value())
			{
				Active = !Active;
				ScreenNotification.ShowNotification(string.Format(common.RunStateChange, Name, (!Active) ? common.Deactivated : common.Activated), (NotificationType)1, (Texture2D)null, 4);
				this.Toggled?.Invoke(this, EventArgs.Empty);
			}
		}

		public abstract void Update(GameTime gameTime);

		public virtual void UpdateLanguage(object sender, EventArgs e)
		{
			if (Hotbar_Button != null)
			{
				((Control)Hotbar_Button).set_BasicTooltipText(string.Format(common.Toggle, Name ?? ""));
			}
		}

		public virtual void Dispose()
		{
			QoL.ModuleInstance.LanguageChanged -= UpdateLanguage;
			ModuleIcon = null;
			ModuleIconHovered = null;
			ModuleIcon_Active = null;
			ModuleIconHovered_Active = null;
		}
	}
}
