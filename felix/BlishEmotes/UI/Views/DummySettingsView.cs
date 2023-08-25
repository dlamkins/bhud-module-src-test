using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using felix.BlishEmotes.Strings;
using felix.BlishEmotes.UI.Presenters;

namespace felix.BlishEmotes.UI.Views
{
	internal class DummySettingsView : View
	{
		private StandardButton _bttnOpenSettings;

		public event EventHandler<EventArgs> OpenSettingsClicked;

		public DummySettingsView(Action OpenSettings)
		{
			WithPresenter(new DummySettingsPresenter(this, OpenSettings));
		}

		protected override void Build(Container buildPanel)
		{
			_bttnOpenSettings = new StandardButton
			{
				Text = Common.settings_button,
				Width = 192,
				Parent = buildPanel
			};
			_bttnOpenSettings.Location = new Point(Math.Max(buildPanel.Width / 2 - _bttnOpenSettings.Width / 2, 20), Math.Max(buildPanel.Height / 2 - _bttnOpenSettings.Height, 20) - _bttnOpenSettings.Height - 10);
			_bttnOpenSettings.Click += _bttnOpenSettings_Click;
		}

		private void _bttnOpenSettings_Click(object sender, MouseEventArgs e)
		{
			this.OpenSettingsClicked?.Invoke(this, e);
		}

		protected override void Unload()
		{
			if (_bttnOpenSettings != null)
			{
				_bttnOpenSettings.Click -= _bttnOpenSettings_Click;
			}
		}
	}
}
