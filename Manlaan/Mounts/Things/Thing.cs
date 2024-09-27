using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.Mounts.Things
{
	public abstract class Thing : IEquatable<Thing>
	{
		protected static readonly Logger Logger = Logger.GetLogger<Thing>();

		protected readonly Helper _helper;

		private DateTime? _queuedTimestamp;

		public string Name { get; private set; }

		public string DisplayName { get; private set; }

		public string ImageFileName { get; private set; }

		public SettingEntry<int> OrderSetting { get; private set; }

		public SettingEntry<KeyBinding> KeybindingSetting { get; private set; }

		public SettingEntry<string> ImageFileNameSetting { get; private set; }

		public CornerIcon CornerIcon { get; private set; }

		public DateTime? QueuedTimestamp
		{
			get
			{
				return _queuedTimestamp;
			}
			internal set
			{
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Expected O, but got Unknown
				DateTime? oldvalue = _queuedTimestamp;
				Logger.Debug(string.Format("Setting {0} on {1} to: {2}", "QueuedTimestamp", Name, value));
				_queuedTimestamp = value;
				if (this.QueuedTimestampUpdated != null && oldvalue != value)
				{
					this.QueuedTimestampUpdated(this, new ValueChangedEventArgs("", ""));
				}
			}
		}

		public DateTime? LastUsedTimestamp { get; internal set; }

		public bool IsKeybindSet
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Invalid comparison between Unknown and I4
				if ((int)KeybindingSetting.get_Value().get_ModifierKeys() == 0)
				{
					return (int)KeybindingSetting.get_Value().get_PrimaryKey() > 0;
				}
				return true;
			}
		}

		public bool IsAvailable => IsKeybindSet;

		public event EventHandler<ValueChangedEventArgs> QueuedTimestampUpdated;

		public Thing(SettingCollection settingCollection, Helper helper, string name, string displayName, string imageFileName)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			_helper = helper;
			Name = name;
			DisplayName = displayName;
			ImageFileName = imageFileName;
			KeybindingSetting = settingCollection.DefineSetting<KeyBinding>("Mount" + name + "Binding", new KeyBinding((Keys)0), (Func<string>)(() => displayName + " Binding"), (Func<string>)(() => ""));
			ImageFileNameSetting = settingCollection.DefineSetting<string>("Mount" + name + "ImageFileName", imageFileName + ".png", (Func<string>)(() => displayName + " Image File Name"), (Func<string>)(() => ""));
		}

		public void CreateCornerIcon(Texture2D img)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			CornerIcon cornerIcon = CornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			CornerIcon val = new CornerIcon();
			val.set_IconName(DisplayName);
			val.set_Icon(AsyncTexture2D.op_Implicit(img));
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(img));
			val.set_Priority(10);
			CornerIcon = val;
			((Control)CornerIcon).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await DoAction(unconditionallyDoAction: false, isActionComingFromMouseActionOnModuleUI: true);
			});
		}

		public void DisposeCornerIcon()
		{
			CornerIcon cornerIcon = CornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
		}

		public async Task DoAction(bool unconditionallyDoAction, bool isActionComingFromMouseActionOnModuleUI)
		{
			if (unconditionallyDoAction)
			{
				await _helper.TriggerKeybind(KeybindingSetting, WhichKeybindToRun.Both);
				return;
			}
			if (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat() && Module._settingEnableMountQueueing.get_Value() && !IsUsableInCombat() && !_helper.IsPlayerInWvwMap())
			{
				Logger.Debug("DoAction Set queued for out of combat: " + Name);
				QueuedTimestamp = DateTime.UtcNow;
				return;
			}
			if (!Module.CanThingBeActivated())
			{
				_helper.StoreThingForLaterActivation(this, "NotAbleToActivate");
				return;
			}
			if (isActionComingFromMouseActionOnModuleUI && IsGroundTargeted())
			{
				switch (Module._settingGroundTargeting.get_Value())
				{
				case GroundTargeting.Instant:
					if (ShouldGroundTargetingBeDelayed())
					{
						_helper.StoredRangedThing = this;
						return;
					}
					break;
				case GroundTargeting.FastWithRangeIndicator:
					_helper.StoredRangedThing = this;
					break;
				}
			}
			WhichKeybindToRun whichKeybindToRun = WhichKeybindToRun.Both;
			LastUsedTimestamp = DateTime.UtcNow;
			switch (Module._settingGroundTargeting.get_Value())
			{
			case GroundTargeting.Instant:
				_helper.StoredRangedThing = null;
				break;
			case GroundTargeting.FastWithRangeIndicator:
				if (IsGroundTargeted())
				{
					whichKeybindToRun = (isActionComingFromMouseActionOnModuleUI ? WhichKeybindToRun.Press : WhichKeybindToRun.Release);
				}
				break;
			}
			await _helper.TriggerKeybind(KeybindingSetting, whichKeybindToRun);
		}

		public virtual bool IsInUse()
		{
			return false;
		}

		public virtual bool IsUsableInCombat()
		{
			return false;
		}

		public virtual bool IsGroundTargeted()
		{
			return false;
		}

		public virtual bool ShouldGroundTargetingBeDelayed()
		{
			return false;
		}

		public bool Equals(Thing other)
		{
			if (other != null)
			{
				return Name == other.Name;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return 657878212 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
		}
	}
}
