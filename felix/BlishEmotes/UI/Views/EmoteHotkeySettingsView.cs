using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace felix.BlishEmotes.UI.Views
{
	internal class EmoteHotkeySettingsView : View
	{
		private FlowPanel FlowPanel;

		private LoadingSpinner Spinner;

		public EmoteHotkeySettingsView(ModuleSettings settings)
		{
			WithPresenter(new EmoteHotkeySettingsPresenter(this, settings));
		}

		protected override void Build(Container buildPanel)
		{
			FlowPanel = new FlowPanel
			{
				Parent = buildPanel,
				FlowDirection = ControlFlowDirection.TopToBottom,
				HeightSizingMode = SizingMode.Fill,
				WidthSizingMode = SizingMode.Fill,
				CanScroll = false
			};
			Spinner = new LoadingSpinner
			{
				Parent = buildPanel,
				Location = new Point(buildPanel.Width / 2, buildPanel.Height / 2),
				Visible = true
			};
		}

		public void BuildEmoteHotkeyPanel(SettingCollection EmotesShortcutsSettings)
		{
			FlowPanel?.ClearChildren();
			if (EmotesShortcutsSettings.Count() > 0)
			{
				Spinner.Hide();
			}
			List<SettingEntry> list = new List<SettingEntry>(EmotesShortcutsSettings.Entries);
			list.Sort((SettingEntry a, SettingEntry b) => a.DisplayName.CompareTo(b.DisplayName));
			foreach (SettingEntry<KeyBinding> entry in list)
			{
				FlowPanel containerPanel = new FlowPanel
				{
					Parent = FlowPanel,
					FlowDirection = ControlFlowDirection.SingleLeftToRight,
					WidthSizingMode = SizingMode.AutoSize,
					HeightSizingMode = SizingMode.AutoSize,
					OuterControlPadding = new Vector2(5f, 5f)
				};
				new Label
				{
					Parent = containerPanel,
					Text = entry.DisplayName,
					Size = new Point(100, 20)
				};
				new KeybindingAssigner(entry.Value)
				{
					Parent = containerPanel,
					NameWidth = 0,
					Size = new Point(100, 20),
					Location = new Point(105, 0)
				};
			}
		}

		protected override void Unload()
		{
			FlowPanel?.Dispose();
			Spinner?.Dispose();
		}
	}
}
