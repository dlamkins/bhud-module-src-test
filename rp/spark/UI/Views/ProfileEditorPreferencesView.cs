using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using rp.spark.Models;
using rp.spark.Services;

namespace rp.spark.UI.Views
{
	public class ProfileEditorPreferencesView : View
	{
		private const int FormWidth = 760;

		private const int FormHeight = 500;

		private const int CheckboxWidth = 230;

		private const int CheckboxHeight = 30;

		private const int CheckboxColumns = 3;

		private const int CheckboxGap = 5;

		private readonly ProfileEditorSession _session;

		private readonly List<KeyValuePair<Checkbox, ProfilePreferenceFlags>> _preferenceCheckboxes = new List<KeyValuePair<Checkbox, ProfilePreferenceFlags>>();

		private readonly List<KeyValuePair<Checkbox, ProfileThemeFlags>> _themeCheckboxes = new List<KeyValuePair<Checkbox, ProfileThemeFlags>>();

		private readonly List<KeyValuePair<Checkbox, ProfileStyleFlags>> _styleCheckboxes = new List<KeyValuePair<Checkbox, ProfileStyleFlags>>();

		private Label _status;

		private Dropdown _experienceDropdown;

		private bool _isRefreshing;

		public ProfileEditorPreferencesView(ProfileEditorSession session)
			: this()
		{
			_session = session;
		}

		protected override void Build(Container buildPanel)
		{
			_preferenceCheckboxes.Clear();
			_themeCheckboxes.Clear();
			_styleCheckboxes.Clear();
			if (!_session.State.CanEditProfile)
			{
				ProfileEditorUI.ShowUnavailableMessage(buildPanel);
				return;
			}
			FlowPanel form = SparkFormLayout.AddVerticalStack(buildPanel, 0, 0, 760, 500, 15);
			FlowPanel experienceGroup = SparkFormLayout.AddAutoStack((Container)(object)form, 760, 0);
			SparkFormLayout.AddLabel((Container)(object)experienceGroup, "Experience", 760);
			_experienceDropdown = SparkFormLayout.AddDropdown((Container)(object)experienceGroup, ProfileLabels.ExperienceOptions, null, 220);
			_experienceDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (!_isRefreshing)
				{
					_session.Profile.Experience = ProfileLabels.ParseExperience(_experienceDropdown.get_SelectedItem()?.ToString());
				}
			});
			SparkFormLayout.AddLabel((Container)(object)form, "Preferences", 760);
			BuildFlagCheckboxes((Container)(object)form, ProfileLabels.PreferenceOptions, (ProfilePreferenceFlags option) => (_session.Profile.Preferences & option) == option, delegate(ProfilePreferenceFlags option)
			{
				_session.Profile.Preferences = ToggleFlag(_session.Profile.Preferences, option);
			}, delegate(Checkbox checkbox, ProfilePreferenceFlags option)
			{
				_preferenceCheckboxes.Add(new KeyValuePair<Checkbox, ProfilePreferenceFlags>(checkbox, option));
			});
			SparkFormLayout.AddLabel((Container)(object)form, "Themes", 760);
			BuildFlagCheckboxes((Container)(object)form, ProfileLabels.ThemeOptions, (ProfileThemeFlags option) => (_session.Profile.Themes & option) == option, delegate(ProfileThemeFlags option)
			{
				_session.Profile.Themes = ToggleFlag(_session.Profile.Themes, option);
			}, delegate(Checkbox checkbox, ProfileThemeFlags option)
			{
				_themeCheckboxes.Add(new KeyValuePair<Checkbox, ProfileThemeFlags>(checkbox, option));
			});
			SparkFormLayout.AddLabel((Container)(object)form, "Styles", 760);
			BuildFlagCheckboxes((Container)(object)form, ProfileLabels.StyleOptions, (ProfileStyleFlags option) => (_session.Profile.Styles & option) == option, delegate(ProfileStyleFlags option)
			{
				_session.Profile.Styles = ToggleFlag(_session.Profile.Styles, option);
			}, delegate(Checkbox checkbox, ProfileStyleFlags option)
			{
				_styleCheckboxes.Add(new KeyValuePair<Checkbox, ProfileStyleFlags>(checkbox, option));
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

		private void BuildFlagCheckboxes<TFlag>(Container parent, IEnumerable<KeyValuePair<TFlag, string>> options, Func<TFlag, bool> isChecked, Action<TFlag> onToggle, Action<Checkbox, TFlag> trackCheckbox)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			List<KeyValuePair<TFlag, string>> optionList = options.ToList();
			int rowCount = Math.Max(1, (int)Math.Ceiling((double)optionList.Count / 3.0));
			FlowPanel val = new FlowPanel();
			((Control)val).set_Width(760);
			((Control)val).set_Height(rowCount * 35);
			val.set_FlowDirection((ControlFlowDirection)0);
			val.set_ControlPadding(new Vector2(15f, 5f));
			((Control)val).set_Parent(parent);
			FlowPanel grid = val;
			foreach (KeyValuePair<TFlag, string> option in optionList)
			{
				Checkbox checkbox = SparkFormLayout.AddCheckbox((Container)(object)grid, option.Value, isChecked(option.Key), 230);
				checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
				{
					if (!_isRefreshing)
					{
						onToggle(option.Key);
					}
				});
				trackCheckbox(checkbox, option.Key);
			}
		}

		protected override void Unload()
		{
			_session.StatusChanged -= HandleStatusChanged;
			_session.ProfileChanged -= HandleProfileChanged;
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
				Dropdown experienceDropdown = _experienceDropdown;
				if (((experienceDropdown != null) ? ((Control)experienceDropdown).get_Parent() : null) != null)
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
				_experienceDropdown.set_SelectedItem(ProfileLabels.GetExperienceLabel(_session.Profile.Experience));
				foreach (KeyValuePair<Checkbox, ProfilePreferenceFlags> pair3 in _preferenceCheckboxes)
				{
					pair3.Key.set_Checked((_session.Profile.Preferences & pair3.Value) == pair3.Value);
				}
				foreach (KeyValuePair<Checkbox, ProfileThemeFlags> pair2 in _themeCheckboxes)
				{
					pair2.Key.set_Checked((_session.Profile.Themes & pair2.Value) == pair2.Value);
				}
				foreach (KeyValuePair<Checkbox, ProfileStyleFlags> pair in _styleCheckboxes)
				{
					pair.Key.set_Checked((_session.Profile.Styles & pair.Value) == pair.Value);
				}
			}
			finally
			{
				_isRefreshing = false;
			}
		}

		private static ProfilePreferenceFlags ToggleFlag(ProfilePreferenceFlags current, ProfilePreferenceFlags value)
		{
			if ((current & value) != value)
			{
				return current | value;
			}
			return current & ~value;
		}

		private static ProfileThemeFlags ToggleFlag(ProfileThemeFlags current, ProfileThemeFlags value)
		{
			if ((current & value) != value)
			{
				return current | value;
			}
			return current & ~value;
		}

		private static ProfileStyleFlags ToggleFlag(ProfileStyleFlags current, ProfileStyleFlags value)
		{
			if ((current & value) != value)
			{
				return current | value;
			}
			return current & ~value;
		}
	}
}
