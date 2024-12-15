using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Localization;
using RaidClears.Settings.Models;
using RaidClears.Utils;

namespace RaidClears.Settings.Views.SubViews
{
	public class DungeonPathSelectionView : View
	{
		private readonly DungeonSettings _settings;

		public DungeonPathSelectionView(DungeonSettings settings)
			: this()
		{
			_settings = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			FlowPanelExtensions.BeginFlow(new FlowPanel(), buildPanel).AddString(Strings.Settings_Dungeon_Heading).AddSetting((IEnumerable<SettingEntry>?)_settings.DungeonPaths)
				.AddSpace()
				.AddSetting((SettingEntry)(object)_settings.DungeonFrequenterVisible);
			Image val = new Image();
			val.set_Texture(AsyncTexture2D.op_Implicit(Service.Textures!.BaseLogo));
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Location(new Point(300, 65));
			((Control)val).set_Size(PointExtensions.Scale(new Point(400, 278), 0.5f));
		}
	}
}
