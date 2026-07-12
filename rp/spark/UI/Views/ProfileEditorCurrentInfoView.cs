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

		private Label _currentlyCounter;

		private Label _outOfCharacterInfoCounter;

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
			FlowPanel parent = SparkFormLayout.AddVerticalStack(buildPanel, 0, 0, 760, 500, 8);
			FlowPanel currentlyGroup = SparkFormLayout.AddAutoStack((Container)(object)parent, 760, 0);
			SparkFormLayout.AddLabel((Container)(object)currentlyGroup, "Currently", 760);
			_currently = (MultilineTextBox)(object)SparkFormLayout.AddMultilineTextBox((Container)(object)currentlyGroup, string.Empty, "What is your character doing right now?", 760, 180, 500);
			_currentlyCounter = ProfileEditorUI.AddCharacterCounter((Container)(object)currentlyGroup, ((TextInputBase)_currently).get_Text(), 500, 760);
			((TextInputBase)_currently).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				ProfileEditorUI.UpdateCharacterCounter(_currentlyCounter, ((TextInputBase)_currently).get_Text(), 500);
				if (!_isRefreshing)
				{
					_session.Profile.Currently = ((TextInputBase)_currently).get_Text()?.Trim() ?? string.Empty;
				}
			});
			FlowPanel outOfCharacterGroup = SparkFormLayout.AddAutoStack((Container)(object)parent, 760, 0);
			SparkFormLayout.AddLabel((Container)(object)outOfCharacterGroup, "Other information (out of character)", 760);
			_outOfCharacterInfo = (MultilineTextBox)(object)SparkFormLayout.AddMultilineTextBox((Container)(object)outOfCharacterGroup, string.Empty, "OOC notes, contact preferences, boundaries, or scheduling info.", 760, 225, 1000);
			_outOfCharacterInfoCounter = ProfileEditorUI.AddCharacterCounter((Container)(object)outOfCharacterGroup, ((TextInputBase)_outOfCharacterInfo).get_Text(), 1000, 760);
			((TextInputBase)_outOfCharacterInfo).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				ProfileEditorUI.UpdateCharacterCounter(_outOfCharacterInfoCounter, ((TextInputBase)_outOfCharacterInfo).get_Text(), 1000);
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
				ProfileEditorUI.UpdateCharacterCounter(_currentlyCounter, ((TextInputBase)_currently).get_Text(), 500);
				ProfileEditorUI.UpdateCharacterCounter(_outOfCharacterInfoCounter, ((TextInputBase)_outOfCharacterInfo).get_Text(), 1000);
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
