using Blish_HUD;
using Blish_HUD.Settings;
using Gw2Sharp.Models;

namespace Manlaan.Mounts.Things.Mounts
{
	public abstract class Mount : Thing
	{
		protected abstract MountType MountType { get; }

		public Mount(SettingCollection settingCollection, Helper helper, string name, string displayName, string imageFileName)
			: base(settingCollection, helper, name, displayName, imageFileName)
		{
		}

		public override bool IsInUse()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount() == MountType;
		}
	}
}
