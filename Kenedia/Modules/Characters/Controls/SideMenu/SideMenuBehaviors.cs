using System;
using System.Collections.Generic;
using System.Resources;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Characters.Extensions;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Interfaces;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls.SideMenu
{
	public class SideMenuBehaviors : FlowTab, ILocalizable
	{
		private readonly List<KeyValuePair<string, DisplayCheckToggle>> _toggles = new List<KeyValuePair<string, DisplayCheckToggle>>
		{
			new KeyValuePair<string, DisplayCheckToggle>("Name", null),
			new KeyValuePair<string, DisplayCheckToggle>("Level", null),
			new KeyValuePair<string, DisplayCheckToggle>("Race", null),
			new KeyValuePair<string, DisplayCheckToggle>("Gender", null),
			new KeyValuePair<string, DisplayCheckToggle>("Profession", null),
			new KeyValuePair<string, DisplayCheckToggle>("LastLogin", null),
			new KeyValuePair<string, DisplayCheckToggle>("Map", null),
			new KeyValuePair<string, DisplayCheckToggle>("CraftingProfession", null),
			new KeyValuePair<string, DisplayCheckToggle>("OnlyMaxCrafting", null),
			new KeyValuePair<string, DisplayCheckToggle>("Tags", null)
		};

		private readonly Panel _separator;

		private readonly Dropdown _orderDropdown;

		private readonly Dropdown _flowDropdown;

		private readonly Dropdown _filterBehaviorDropdown;

		private readonly Dropdown _matchingDropdown;

		private readonly DisplayCheckToggle _toggleAll;

		private readonly ResourceManager _resourceManager;

		private readonly SettingsModel _settings;

		private readonly Action _onSortChanged;

		private Rectangle _contentRectangle;

		public SideMenuBehaviors(ResourceManager resourceManager, TextureManager textureManager, SettingsModel settings, Action onSortChanged)
		{
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			_resourceManager = resourceManager;
			_settings = settings;
			_onSortChanged = onSortChanged;
			base.FlowDirection = (ControlFlowDirection)3;
			((Container)this).set_WidthSizingMode((SizingMode)2);
			((Container)this).set_AutoSizePadding(new Point(5, 5));
			((Container)this).set_HeightSizingMode((SizingMode)1);
			base.OuterControlPadding = new Vector2(5f, 5f);
			base.ControlPadding = new Vector2(5f, 5f);
			((Control)this).set_Location(new Point(0, 25));
			Dropdown dropdown = new Dropdown();
			((Control)dropdown).set_Parent((Container)(object)this);
			_orderDropdown = dropdown;
			((Dropdown)_orderDropdown).add_ValueChanged((EventHandler<ValueChangedEventArgs>)OrderDropdown_ValueChanged);
			Dropdown dropdown2 = new Dropdown();
			((Control)dropdown2).set_Parent((Container)(object)this);
			_flowDropdown = dropdown2;
			((Dropdown)_flowDropdown).add_ValueChanged((EventHandler<ValueChangedEventArgs>)FlowDropdown_ValueChanged);
			Dropdown dropdown3 = new Dropdown();
			((Control)dropdown3).set_Parent((Container)(object)this);
			_filterBehaviorDropdown = dropdown3;
			((Dropdown)_filterBehaviorDropdown).add_ValueChanged((EventHandler<ValueChangedEventArgs>)FilterBehaviorDropdown_ValueChanged);
			Dropdown dropdown4 = new Dropdown();
			((Control)dropdown4).set_Parent((Container)(object)this);
			_matchingDropdown = dropdown4;
			((Dropdown)_matchingDropdown).add_ValueChanged((EventHandler<ValueChangedEventArgs>)MatchingDropdown_ValueChanged);
			DisplayCheckToggle displayCheckToggle = new DisplayCheckToggle(textureManager);
			((Control)displayCheckToggle).set_Parent((Container)(object)this);
			_toggleAll = displayCheckToggle;
			_toggleAll.ShowChanged += All_ShowChanged;
			_toggleAll.CheckChanged += All_CheckChanged;
			Panel obj = new Panel
			{
				BackgroundColor = Color.get_White() * 0.6f
			};
			((Control)obj).set_Height(2);
			((Control)obj).set_Parent((Container)(object)this);
			_separator = obj;
			for (int i = 0; i < _toggles.Count; i++)
			{
				KeyValuePair<string, DisplayCheckToggle> t = _toggles[i];
				SettingsModel settings2 = _settings;
				string key = t.Key;
				string key2 = t.Key;
				DisplayCheckToggle displayCheckToggle2 = new DisplayCheckToggle(textureManager, settings2, key, key2 == "Name" || key2 == "Profession" || key2 == "LastLogin");
				((Control)displayCheckToggle2).set_Parent((Container)(object)this);
				DisplayCheckToggle ctrl = displayCheckToggle2;
				ctrl.Changed += Toggle_Changed;
				_toggles[i] = new KeyValuePair<string, DisplayCheckToggle>(t.Key, ctrl);
			}
			GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)OnLanguageChanged);
			OnLanguageChanged();
		}

		private void All_CheckChanged(object sender, bool e)
		{
			foreach (KeyValuePair<string, DisplayCheckToggle> toggle in _toggles)
			{
				toggle.Value.CheckChecked = e;
			}
		}

		private void All_ShowChanged(object sender, bool e)
		{
			foreach (KeyValuePair<string, DisplayCheckToggle> toggle in _toggles)
			{
				toggle.Value.ShowChecked = e;
			}
		}

		private void Toggle_Changed(object sender, Tuple<bool, bool> e)
		{
		}

		private void MatchingDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			_settings.ResultMatchingBehavior.set_Value(e.get_CurrentValue().GetMatchingBehavior());
		}

		private void FilterBehaviorDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			_settings.ResultFilterBehavior.set_Value(e.get_CurrentValue().GetFilterBehavior());
		}

		private void FlowDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			_settings.SortOrder.set_Value(e.get_CurrentValue().GetSortOrder());
			_onSortChanged?.Invoke();
		}

		private void OrderDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			_settings.SortType.set_Value(e.get_CurrentValue().GetSortType());
			_onSortChanged?.Invoke();
		}

		public void OnLanguageChanged(object s = null, EventArgs e = null)
		{
			((Dropdown)_orderDropdown).set_SelectedItem(_settings.SortType.get_Value().GetSortType());
			((Dropdown)_orderDropdown).get_Items().Clear();
			((Dropdown)_orderDropdown).get_Items().Add(string.Format(strings.SortBy, strings.Name));
			((Dropdown)_orderDropdown).get_Items().Add(string.Format(strings.SortBy, strings.Tags));
			((Dropdown)_orderDropdown).get_Items().Add(string.Format(strings.SortBy, strings.Profession));
			((Dropdown)_orderDropdown).get_Items().Add(string.Format(strings.SortBy, strings.LastLogin));
			((Dropdown)_orderDropdown).get_Items().Add(string.Format(strings.SortBy, strings.Map));
			((Dropdown)_orderDropdown).get_Items().Add(strings.Custom);
			((Dropdown)_flowDropdown).set_SelectedItem(_settings.SortOrder.get_Value().GetSortOrder());
			((Dropdown)_flowDropdown).get_Items().Clear();
			((Dropdown)_flowDropdown).get_Items().Add(strings.Ascending);
			((Dropdown)_flowDropdown).get_Items().Add(strings.Descending);
			((Dropdown)_matchingDropdown).set_SelectedItem(_settings.ResultMatchingBehavior.get_Value().GetMatchingBehavior());
			((Dropdown)_matchingDropdown).get_Items().Clear();
			((Dropdown)_matchingDropdown).get_Items().Add(strings.MatchAnyFilter);
			((Dropdown)_matchingDropdown).get_Items().Add(strings.MatchAllFilter);
			((Dropdown)_filterBehaviorDropdown).set_SelectedItem(_settings.ResultFilterBehavior.get_Value().GetFilterBehavior());
			((Dropdown)_filterBehaviorDropdown).get_Items().Clear();
			((Dropdown)_filterBehaviorDropdown).get_Items().Add(strings.IncludeMatches);
			((Dropdown)_filterBehaviorDropdown).get_Items().Add(strings.ExcludeMatches);
			_toggleAll.Text = strings.ToggleAll;
			foreach (KeyValuePair<string, DisplayCheckToggle> t in _toggles)
			{
				t.Value.Text = _resourceManager.GetString(t.Key);
				t.Value.DisplayTooltip = string.Format(_resourceManager.GetString("ShowItem"), _resourceManager.GetString(t.Key));
				t.Value.CheckTooltip = string.Format(_resourceManager.GetString("CheckItem"), _resourceManager.GetString(t.Key));
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)OnLanguageChanged);
			((Dropdown)_orderDropdown).remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OrderDropdown_ValueChanged);
			((Dropdown)_flowDropdown).remove_ValueChanged((EventHandler<ValueChangedEventArgs>)FlowDropdown_ValueChanged);
			((Dropdown)_filterBehaviorDropdown).remove_ValueChanged((EventHandler<ValueChangedEventArgs>)FilterBehaviorDropdown_ValueChanged);
			((Dropdown)_matchingDropdown).remove_ValueChanged((EventHandler<ValueChangedEventArgs>)MatchingDropdown_ValueChanged);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).OnResized(e);
			_contentRectangle = new Rectangle((int)base.OuterControlPadding.X, (int)base.OuterControlPadding.Y, ((Control)this).get_Width() - (int)base.OuterControlPadding.X * 2, ((Control)this).get_Height() - (int)base.OuterControlPadding.Y * 2);
			((Control)_orderDropdown).set_Width(_contentRectangle.Width);
			((Control)_flowDropdown).set_Width(_contentRectangle.Width);
			((Control)_filterBehaviorDropdown).set_Width(_contentRectangle.Width);
			((Control)_matchingDropdown).set_Width(_contentRectangle.Width);
			((Control)_separator).set_Width(_contentRectangle.Width);
		}
	}
}
