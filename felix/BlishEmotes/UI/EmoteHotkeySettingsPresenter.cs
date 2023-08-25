using System;
using System.Threading.Tasks;
using Blish_HUD.Graphics.UI;
using felix.BlishEmotes.UI.Views;

namespace felix.BlishEmotes.UI
{
	internal class EmoteHotkeySettingsPresenter : Presenter<EmoteHotkeySettingsView, ModuleSettings>
	{
		public EmoteHotkeySettingsPresenter(EmoteHotkeySettingsView view, ModuleSettings model)
			: base(view, model)
		{
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			base.Model.OnEmotesLoaded += Model_EmotesSettingsLoaded;
			return base.Load(progress);
		}

		protected override void UpdateView()
		{
			base.View.BuildEmoteHotkeyPanel(base.Model.EmotesShortcutsSettings);
		}

		private void Model_EmotesSettingsLoaded(object sender, bool e)
		{
			UpdateView();
		}

		protected override void Unload()
		{
			base.Model.OnEmotesLoaded -= Model_EmotesSettingsLoaded;
		}
	}
}
