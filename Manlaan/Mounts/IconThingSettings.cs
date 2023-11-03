using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Settings;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework;
using Mounts.Settings;

namespace Manlaan.Mounts
{
	public class IconThingSettings : ThingsSettings
	{
		public SettingEntry<string> Name;

		public SettingEntry<bool> IsEnabled;

		public SettingEntry<bool> DisplayCornerIcons;

		public SettingEntry<IconOrientation> Orientation;

		public SettingEntry<Point> Location;

		public SettingEntry<bool> IsDraggingEnabled;

		public SettingEntry<int> Size;

		public SettingEntry<float> Opacity;

		public int Id { get; }

		public bool IsDefault => Id == 0;

		public bool ShouldDisplayCornerIcons
		{
			get
			{
				if (IsDefault)
				{
					return DisplayCornerIcons.get_Value();
				}
				return false;
			}
		}

		public event EventHandler<SettingsUpdatedEvent> IconSettingsUpdated;

		public IconThingSettings(SettingCollection settingCollection, int id, string defaultName = "", List<Thing> defaultThings = null)
			: base(settingCollection, defaultThings, $"IconThingSettings{id}Things")
		{
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			Id = id;
			Name = settingCollection.DefineSetting<string>($"IconThingSettings{Id}name", defaultName, (Func<string>)null, (Func<string>)null);
			IsEnabled = settingCollection.DefineSetting<bool>($"IconThingSettings{Id}IsEnabled", true, (Func<string>)null, (Func<string>)null);
			DisplayCornerIcons = settingCollection.DefineSetting<bool>($"IconThingSettings{Id}DisplayCornerIcons", true, (Func<string>)null, (Func<string>)null);
			Orientation = settingCollection.DefineSetting<IconOrientation>($"IconThingSettings{Id}Orientation", IconOrientation.Horizontal, (Func<string>)null, (Func<string>)null);
			Location = settingCollection.DefineSetting<Point>($"IconThingSettings{Id}Location", new Point(100, 100), (Func<string>)null, (Func<string>)null);
			IsDraggingEnabled = settingCollection.DefineSetting<bool>($"IconThingSettings{Id}IsDragging", false, (Func<string>)null, (Func<string>)null);
			Size = settingCollection.DefineSetting<int>($"IconThingSettings{Id}Size", 50, (Func<string>)null, (Func<string>)null);
			SettingComplianceExtensions.SetRange(Size, 0, 200);
			Opacity = settingCollection.DefineSetting<float>($"IconThingSettings{Id}Opacity", 1f, (Func<string>)null, (Func<string>)null);
			SettingComplianceExtensions.SetRange(Opacity, 0f, 1f);
			IsEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged);
			DisplayCornerIcons.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged);
			Orientation.add_SettingChanged((EventHandler<ValueChangedEventArgs<IconOrientation>>)SettingChanged);
			Location.add_SettingChanged((EventHandler<ValueChangedEventArgs<Point>>)SettingChanged);
			IsDraggingEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)SettingChanged);
			Size.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)SettingChanged);
			Opacity.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)SettingChanged);
			ThingsSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<IList<string>>>)ThingsSetting_SettingChanged);
		}

		public override void DeleteFromSettings(SettingCollection settingCollection)
		{
			settingCollection.UndefineSetting($"IconThingSettings{Id}name");
			settingCollection.UndefineSetting($"IconThingSettings{Id}IsEnabled");
			settingCollection.UndefineSetting($"IconThingSettings{Id}DisplayCornerIcons");
			settingCollection.UndefineSetting($"IconThingSettings{Id}Orientation");
			settingCollection.UndefineSetting($"IconThingSettings{Id}Location");
			settingCollection.UndefineSetting($"IconThingSettings{Id}IsDragging");
			settingCollection.UndefineSetting($"IconThingSettings{Id}Size");
			settingCollection.UndefineSetting($"IconThingSettings{Id}Opacity");
			base.DeleteFromSettings(settingCollection);
		}

		private void UpdateIconThingSettingsInternal()
		{
			SettingsUpdatedEvent myevent = new SettingsUpdatedEvent();
			if (this.IconSettingsUpdated != null)
			{
				this.IconSettingsUpdated(this, myevent);
			}
		}

		private void ThingsSetting_SettingChanged(object sender, ValueChangedEventArgs<IList<string>> e)
		{
			UpdateIconThingSettingsInternal();
		}

		private void SettingChanged(object sender, ValueChangedEventArgs<IconOrientation> e)
		{
			UpdateIconThingSettingsInternal();
		}

		private void SettingChanged(object sender, ValueChangedEventArgs<Point> e)
		{
			UpdateIconThingSettingsInternal();
		}

		private void SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			UpdateIconThingSettingsInternal();
		}

		private void SettingChanged(object sender, ValueChangedEventArgs<float> e)
		{
			UpdateIconThingSettingsInternal();
		}

		private void SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			UpdateIconThingSettingsInternal();
		}
	}
}
