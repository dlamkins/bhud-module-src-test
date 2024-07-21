using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.Discovery.Version
{
	public class VersionIndicatorView : View
	{
		private Label _nameLabel;

		public VersionIndicatorView()
			: this()
		{
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			val.set_Text($"v{MysticCraftingModule.Settings.LastAcknowledgedUpdate.get_Value()}");
			((Control)val).set_BasicTooltipText(Common.VersionClick);
			((Control)val).set_Location(new Point(30, 10));
			((Control)val).set_Size(new Point(200, 20));
			val.set_StrokeText(true);
			_nameLabel = val;
			((Control)_nameLabel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MysticCraftingModule.OpenUpdateWindow();
			});
		}
	}
}
