using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Models;

namespace rp.spark.UI.Controls
{
	internal sealed class ProfileFilterMenu : IDisposable
	{
		private readonly Dictionary<ProfileFilterCategory, HashSet<string>> _selected = new Dictionary<ProfileFilterCategory, HashSet<string>>
		{
			[ProfileFilterCategory.Experience] = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
			[ProfileFilterCategory.Preference] = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
			[ProfileFilterCategory.Theme] = new HashSet<string>(StringComparer.OrdinalIgnoreCase),
			[ProfileFilterCategory.Style] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		};

		private readonly StandardButton _button;

		private readonly ContextMenuStrip _menu;

		private readonly Action _changed;

		private ContextMenuStripItem _resetItem;

		private bool _isDisposed;

		public int ActiveCount => _selected.Values.Sum((HashSet<string> values) => values.Count);

		public ProfileFilterMenu(Container parent, Action changed)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			_changed = changed;
			StandardButton val = new StandardButton();
			val.set_Text("Filters");
			((Control)val).set_Location(new Point(650, 40));
			((Control)val).set_Size(new Point(100, 35));
			((Control)val).set_Parent(parent);
			_button = val;
			ContextMenuStrip val2 = new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)GetMenuItems);
			((Control)val2).set_Width(200);
			_menu = val2;
			((Control)_button).add_Click((EventHandler<MouseEventArgs>)OnButtonClick);
		}

		public bool Matches(ProfileExperience experience, ProfileDiscoveryTags tags)
		{
			tags = tags ?? new ProfileDiscoveryTags();
			if (MatchesExperience(experience) && MatchesCategory(ProfileFilterCategory.Preference, tags.Preferences) && MatchesCategory(ProfileFilterCategory.Theme, tags.Themes))
			{
				return MatchesCategory(ProfileFilterCategory.Style, tags.Styles);
			}
			return false;
		}

		private bool MatchesExperience(ProfileExperience experience)
		{
			HashSet<string> selected = _selected[ProfileFilterCategory.Experience];
			if (selected.Count != 0)
			{
				return selected.Contains(experience.ToString());
			}
			return true;
		}

		private bool MatchesCategory(ProfileFilterCategory category, IEnumerable<string> available)
		{
			HashSet<string> selected = _selected[category];
			HashSet<string> availableValues = new HashSet<string>(available ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);
			if (selected.Count != 0)
			{
				return selected.IsSubsetOf(availableValues);
			}
			return true;
		}

		[IteratorStateMachine(typeof(_003CGetMenuItems_003Ed__12))]
		private IEnumerable<ContextMenuStripItem> GetMenuItems()
		{
			return new _003CGetMenuItems_003Ed__12(-2)
			{
				_003C_003E4__this = this
			};
		}

		private ContextMenuStripItem CreateSubmenu(string text, ProfileFilterCategory category, IEnumerable<KeyValuePair<string, string>> options)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_004a: Expected O, but got Unknown
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text(text);
			ContextMenuStrip val2 = new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => GetOptionItems(category, options)));
			((Control)val2).set_Width(220);
			val.set_Submenu(val2);
			return val;
		}

		[IteratorStateMachine(typeof(_003CGetOptionItems_003Ed__14))]
		private IEnumerable<ContextMenuStripItem> GetOptionItems(ProfileFilterCategory category, IEnumerable<KeyValuePair<string, string>> options)
		{
			return new _003CGetOptionItems_003Ed__14(-2)
			{
				_003C_003E4__this = this,
				_003C_003E3__category = category,
				_003C_003E3__options = options
			};
		}

		private void SetSelected(ProfileFilterCategory category, string optionId, bool selected)
		{
			if (selected)
			{
				_selected[category].Add(optionId);
			}
			else
			{
				_selected[category].Remove(optionId);
			}
			UpdateButton();
			_changed?.Invoke();
		}

		private void Reset()
		{
			foreach (HashSet<string> value in _selected.Values)
			{
				value.Clear();
			}
			UpdateButton();
			_changed?.Invoke();
		}

		private void UpdateButton()
		{
			_button.set_Text((ActiveCount == 0) ? "Filters" : $"Filters ({ActiveCount})");
			if (_resetItem != null)
			{
				((Control)_resetItem).set_Enabled(ActiveCount > 0);
			}
		}

		private void OnButtonClick(object sender, MouseEventArgs e)
		{
			_menu.Show((Control)(object)_button);
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				((Control)_button).remove_Click((EventHandler<MouseEventArgs>)OnButtonClick);
				((Control)_menu).Dispose();
			}
		}
	}
}
