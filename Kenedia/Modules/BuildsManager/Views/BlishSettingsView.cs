using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Res;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class BlishSettingsView : View
	{
		private StandardButton _openSettingsButton;

		private readonly Action _toggleWindow;

		public BlishSettingsView(Action? toggleWindow)
		{
			_toggleWindow = toggleWindow;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			_openSettingsButton = new StandardButton
			{
				Text = strings_common.OpenSettings,
				Width = 192,
				Parent = buildPanel
			};
			_openSettingsButton.Location = new Point(Math.Max(buildPanel.Width / 2 - _openSettingsButton.Width / 2, 20), Math.Max(buildPanel.Height / 2 - _openSettingsButton.Height, 20) - _openSettingsButton.Height - 10);
			_openSettingsButton.Click += OpenSettingsButton_Click;
		}

		private void OpenSettingsButton_Click(object sender, MouseEventArgs e)
		{
			_toggleWindow?.Invoke();
		}

		protected override void Unload()
		{
			if (_openSettingsButton != null)
			{
				_openSettingsButton.Click -= OpenSettingsButton_Click;
			}
		}
	}
}
