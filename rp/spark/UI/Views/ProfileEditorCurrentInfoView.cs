using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	public class ProfileEditorCurrentInfoView : View
	{
		private const int TextBoxWidth = 760;

		private const int FormHeight = 500;

		private const int SaveY = 515;

		private const int StatusY = 520;

		private const int ContextY = 560;

		private readonly ProfileEditorSession _session;

		private Label _status;

		private MultilineTextBox _currently;

		private MultilineTextBox _outOfCharacterInfo;

		private bool _isRefreshing;

		public ProfileEditorCurrentInfoView(ProfileEditorSession session)
			: this()
		{
			_session = session;
		}

		protected override void Build(Container buildPanel)
		{
			if (!_session.State.CanEditProfile)
			{
				ProfileEditorUI.ShowUnavailableMessage(buildPanel);
				return;
			}
			FlowPanel form = SparkFormLayout.AddVerticalStack(buildPanel, 0, 0, 760, 500, 20);
			_currently = (MultilineTextBox)(object)SparkFormLayout.AddLabeledMultilineTextBox(form, "Currently", string.Empty, "What is your character doing right now?", 760, 180, 500);
			((TextInputBase)_currently).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					_session.Profile.Currently = ((TextInputBase)_currently).get_Text()?.Trim() ?? string.Empty;
				}
			});
			_outOfCharacterInfo = (MultilineTextBox)(object)SparkFormLayout.AddLabeledMultilineTextBox(form, "Other information (out of character)", string.Empty, "OOC notes, contact preferences, boundaries, or scheduling info.", 760, 225, 1000);
			((TextInputBase)_outOfCharacterInfo).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					_session.Profile.OutOfCharacterInfo = ((TextInputBase)_outOfCharacterInfo).get_Text()?.Trim() ?? string.Empty;
				}
			});
			BuildFooter(buildPanel);
			_session.ProfileChanged += HandleProfileChanged;
			RefreshFromSession();
		}

		private void BuildFooter(Container buildPanel)
		{
			_status = ProfileEditorUI.AddSaveFooter(buildPanel, _session);
			_session.StatusChanged += HandleStatusChanged;
		}

		private void HandleStatusChanged(string statusText)
		{
			SparkUiThread.Queue(delegate
			{
				Label status = _status;
				if (((status != null) ? ((Control)status).get_Parent() : null) != null)
				{
					_status.set_Text(statusText ?? string.Empty);
				}
			});
		}

		private void HandleProfileChanged()
		{
			SparkUiThread.Queue(delegate
			{
				MultilineTextBox currently = _currently;
				if (((currently != null) ? ((Control)currently).get_Parent() : null) != null)
				{
					RefreshFromSession();
				}
			});
		}

		private void RefreshFromSession()
		{
			_isRefreshing = true;
			try
			{
				((TextInputBase)_currently).set_Text(_session.Profile.Currently ?? string.Empty);
				((TextInputBase)_outOfCharacterInfo).set_Text(_session.Profile.OutOfCharacterInfo ?? string.Empty);
			}
			finally
			{
				_isRefreshing = false;
			}
		}

		protected override void Unload()
		{
			_session.StatusChanged -= HandleStatusChanged;
			_session.ProfileChanged -= HandleProfileChanged;
		}
	}
}
