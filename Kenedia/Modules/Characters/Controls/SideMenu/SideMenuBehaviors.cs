using System;
using System.Collections.Generic;
using System.Resources;
using Blish_HUD;
using Blish_HUD.Controls;
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
			new KeyValuePair<string, DisplayCheckToggle>("NextBirthday", null),
			new KeyValuePair<string, DisplayCheckToggle>("Age", null),
			new KeyValuePair<string, DisplayCheckToggle>("Map", null),
			new KeyValuePair<string, DisplayCheckToggle>("CraftingProfession", null),
			new KeyValuePair<string, DisplayCheckToggle>("OnlyMaxCrafting", null),
			new KeyValuePair<string, DisplayCheckToggle>("CustomIndex", null),
			new KeyValuePair<string, DisplayCheckToggle>("Tags", null)
		};

		private readonly Kenedia.Modules.Core.Controls.Panel _separator;

		private readonly Kenedia.Modules.Core.Controls.Dropdown _orderDropdown;

		private readonly Kenedia.Modules.Core.Controls.Dropdown _flowDropdown;

		private readonly Kenedia.Modules.Core.Controls.Dropdown _filterBehaviorDropdown;

		private readonly Kenedia.Modules.Core.Controls.Dropdown _matchingDropdown;

		private readonly DisplayCheckToggle _toggleAll;

		private readonly ResourceManager _resourceManager;

		private readonly Settings _settings;

		private readonly Action _onSortChanged;

		private Rectangle _contentRectangle;

		public SideMenuBehaviors(ResourceManager resourceManager, TextureManager textureManager, Settings settings, Action onSortChanged)
		{
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			_resourceManager = resourceManager;
			_settings = settings;
			_onSortChanged = onSortChanged;
			base.FlowDirection = ControlFlowDirection.SingleTopToBottom;
			WidthSizingMode = SizingMode.Fill;
			base.AutoSizePadding = new Point(5, 5);
			HeightSizingMode = SizingMode.AutoSize;
			base.OuterControlPadding = new Vector2(5f, 5f);
			base.ControlPadding = new Vector2(5f, 5f);
			base.Location = new Point(0, 25);
			_orderDropdown = new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = this,
				SetLocalizedTooltip = () => strings.CustomOrderDisclaimer
			};
			_orderDropdown.ValueChanged += OrderDropdown_ValueChanged;
			_flowDropdown = new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = this
			};
			_flowDropdown.ValueChanged += FlowDropdown_ValueChanged;
			_filterBehaviorDropdown = new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = this
			};
			_filterBehaviorDropdown.ValueChanged += FilterBehaviorDropdown_ValueChanged;
			_matchingDropdown = new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = this
			};
			_matchingDropdown.ValueChanged += MatchingDropdown_ValueChanged;
			_toggleAll = new DisplayCheckToggle(textureManager)
			{
				Parent = this
			};
			_toggleAll.ShowChanged += new EventHandler<bool>(All_ShowChanged);
			_toggleAll.CheckChanged += new EventHandler<bool>(All_CheckChanged);
			_toggleAll.ShowTooltipChanged += new EventHandler<bool>(All_ShowTooltipCheckChanged);
			_separator = new Kenedia.Modules.Core.Controls.Panel
			{
				BackgroundColor = Color.get_White() * 0.6f,
				Height = 2,
				Parent = this
			};
			for (int i = 0; i < _toggles.Count; i++)
			{
				KeyValuePair<string, DisplayCheckToggle> t = _toggles[i];
				Settings settings2 = _settings;
				string key = t.Key;
				bool show;
				switch (t.Key)
				{
				case "Name":
				case "Profession":
				case "LastLogin":
					show = true;
					break;
				default:
					show = false;
					break;
				}
				DisplayCheckToggle ctrl = new DisplayCheckToggle(textureManager, settings2, key, show)
				{
					Parent = this
				};
				ctrl.Changed += new EventHandler<Tuple<bool, bool>>(Toggle_Changed);
				_toggles[i] = new KeyValuePair<string, DisplayCheckToggle>(t.Key, ctrl);
			}
			GameService.Overlay.UserLocale.SettingChanged += OnLanguageChanged;
			OnLanguageChanged();
		}

		private void All_ShowTooltipCheckChanged(object sender, bool e)
		{
			foreach (KeyValuePair<string, DisplayCheckToggle> toggle in _toggles)
			{
				toggle.Value.ShowTooltipChecked = e;
			}
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
			_settings.ResultMatchingBehavior.Value = e.CurrentValue.GetMatchingBehavior();
		}

		private void FilterBehaviorDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			_settings.ResultFilterBehavior.Value = e.CurrentValue.GetFilterBehavior();
		}

		private void FlowDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			_settings.SortOrder.Value = e.CurrentValue.GetSortOrder();
			_onSortChanged?.Invoke();
		}

		private void OrderDropdown_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			_settings.SortType.Value = e.CurrentValue.GetSortType();
			_onSortChanged?.Invoke();
		}

		public void OnLanguageChanged(object s = null, EventArgs e = null)
		{
			_orderDropdown.SelectedItem = _settings.SortType.Value.GetSortType();
			_orderDropdown.Items.Clear();
			_orderDropdown.Items.Add(string.Format(strings.SortBy, strings.Name));
			_orderDropdown.Items.Add(string.Format(strings.SortBy, strings.Level));
			_orderDropdown.Items.Add(string.Format(strings.SortBy, strings.Race));
			_orderDropdown.Items.Add(string.Format(strings.SortBy, strings.Gender));
			_orderDropdown.Items.Add(string.Format(strings.SortBy, strings.Profession));
			_orderDropdown.Items.Add(string.Format(strings.SortBy, strings.Specialization));
			_orderDropdown.Items.Add(string.Format(strings.SortBy, strings.TimeSinceLogin));
			_orderDropdown.Items.Add(string.Format(strings.SortBy, strings.NextBirthday));
			_orderDropdown.Items.Add(string.Format(strings.SortBy, strings.Age));
			_orderDropdown.Items.Add(string.Format(strings.SortBy, strings.Map));
			_orderDropdown.Items.Add(strings.Custom);
			_flowDropdown.SelectedItem = _settings.SortOrder.Value.GetSortOrder();
			_flowDropdown.Items.Clear();
			_flowDropdown.Items.Add(strings.Ascending);
			_flowDropdown.Items.Add(strings.Descending);
			_matchingDropdown.SelectedItem = _settings.ResultMatchingBehavior.Value.GetMatchingBehavior();
			_matchingDropdown.Items.Clear();
			_matchingDropdown.Items.Add(strings.MatchAnyFilter);
			_matchingDropdown.Items.Add(strings.MatchAllFilter);
			_filterBehaviorDropdown.SelectedItem = _settings.ResultFilterBehavior.Value.GetFilterBehavior();
			_filterBehaviorDropdown.Items.Clear();
			_filterBehaviorDropdown.Items.Add(strings.IncludeMatches);
			_filterBehaviorDropdown.Items.Add(strings.ExcludeMatches);
			_toggleAll.Text = strings.ToggleAll;
			foreach (KeyValuePair<string, DisplayCheckToggle> t in _toggles)
			{
				t.Value.Text = _resourceManager.GetString(t.Key);
				t.Value.DisplayTooltip = string.Format(_resourceManager.GetString("ShowItem"), _resourceManager.GetString(t.Key));
				t.Value.CheckTooltip = string.Format(_resourceManager.GetString("CheckItem"), _resourceManager.GetString(t.Key));
				t.Value.ShowTooltipTooltip = string.Format(_resourceManager.GetString("ShowItemOnTooltip"), _resourceManager.GetString(t.Key));
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			GameService.Overlay.UserLocale.SettingChanged -= OnLanguageChanged;
			_orderDropdown.ValueChanged -= OrderDropdown_ValueChanged;
			_flowDropdown.ValueChanged -= FlowDropdown_ValueChanged;
			_filterBehaviorDropdown.ValueChanged -= FilterBehaviorDropdown_ValueChanged;
			_matchingDropdown.ValueChanged -= MatchingDropdown_ValueChanged;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			base.OnResized(e);
			_contentRectangle = new Rectangle((int)base.OuterControlPadding.X, (int)base.OuterControlPadding.Y, base.Width - (int)base.OuterControlPadding.X * 2, base.Height - (int)base.OuterControlPadding.Y * 2);
			_orderDropdown.Width = _contentRectangle.Width;
			_flowDropdown.Width = _contentRectangle.Width;
			_filterBehaviorDropdown.Width = _contentRectangle.Width;
			_matchingDropdown.Width = _contentRectangle.Width;
			_separator.Width = _contentRectangle.Width;
		}
	}
}
