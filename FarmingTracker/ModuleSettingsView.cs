using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class ModuleSettingsView : View
	{
		private OpenSettingsButton _openSettingsButton;

		private readonly FarmingTrackerWindow _farmingTrackerWindow;

		public ModuleSettingsView(FarmingTrackerWindow farmingTrackerWindow)
			: this()
		{
			_farmingTrackerWindow = farmingTrackerWindow;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			_openSettingsButton = new OpenSettingsButton("Open Settings", _farmingTrackerWindow, buildPanel);
			int x = Math.Max(((Control)buildPanel).get_Width() / 2 - ((Control)_openSettingsButton).get_Width() / 2, 20);
			int y = Math.Max(((Control)buildPanel).get_Height() / 2 - ((Control)_openSettingsButton).get_Height() / 2, 20);
			((Control)_openSettingsButton).set_Location(new Point(x, y));
		}

		protected override void Unload()
		{
			OpenSettingsButton openSettingsButton = _openSettingsButton;
			if (openSettingsButton != null)
			{
				((Control)openSettingsButton).Dispose();
			}
		}
	}
}
