using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class StatSelection : BaseSelection
	{
		private readonly List<AttributeToggle> _statIcons = new List<AttributeToggle>();

		private readonly List<StatSelectable> _stats = new List<StatSelectable>();

		private readonly bool _created;

		private IReadOnlyList<int> _statChoices;

		private double _attributeAdjustments;

		public TemplatePresenter TemplatePresenter { get; }

		public Data Data { get; }

		public IReadOnlyList<int> StatChoices
		{
			get
			{
				return _statChoices;
			}
			set
			{
				Common.SetProperty(ref _statChoices, value, new ValueChangedEventHandler<IReadOnlyList<int>>(OnStatChoicesChanged));
			}
		}

		public double AttributeAdjustments
		{
			get
			{
				return _attributeAdjustments;
			}
			set
			{
				Common.SetProperty(ref _attributeAdjustments, value, new ValueChangedEventHandler<double>(OnAttributeAdjustmentsChanged));
			}
		}

		public StatSelection(TemplatePresenter templatePresenter, Data data)
		{
			TemplatePresenter = templatePresenter;
			Data = data;
			Search.PerformFiltering = new Action<string>(FilterStats);
			Search.SetLocation(Search.Left, Search.Top + 30);
			SelectionContent.SetLocation(Search.Left, Search.Bottom + 5);
			FilterStats();
			_created = true;
			Data.Loaded += new EventHandler(Data_Loaded);
			CreateStatSelectables();
		}

		private void Data_Loaded(object sender, EventArgs e)
		{
			CreateStatSelectables();
		}

		private void CreateStatSelectables()
		{
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			if (_stats.Count > 0)
			{
				return;
			}
			int size = 25;
			Point start = default(Point);
			((Point)(ref start))._002Ector(0, 0);
			List<AttributeType> obj = new List<AttributeType>
			{
				AttributeType.Power,
				AttributeType.Toughness,
				AttributeType.Vitality,
				AttributeType.Healing,
				AttributeType.Precision,
				AttributeType.CritDamage,
				AttributeType.ConditionDamage,
				AttributeType.ConditionDuration,
				AttributeType.BoonDuration
			};
			int i = 0;
			foreach (AttributeType stat2 in obj)
			{
				if (((uint)stat2 > 1u && stat2 != AttributeType.AgonyResistance) || 1 == 0)
				{
					int j = 0;
					List<AttributeToggle> statIcons = _statIcons;
					AttributeToggle obj2 = new AttributeToggle
					{
						Parent = this,
						Location = new Point(start.X + i * (size + 16), start.Y + j * size),
						Size = new Point(size, size),
						Attribute = stat2,
						OnCheckChanged = delegate
						{
							FilterStats();
						},
						Checked = false,
						BasicTooltipText = (stat2.GetDisplayName() ?? "")
					};
					AttributeToggle t = obj2;
					statIcons.Add(obj2);
					i++;
				}
			}
			foreach (KeyValuePair<int, Stat> stat in Data.Stats.Items)
			{
				StatSelectable selectable;
				_stats.Add(selectable = new StatSelectable
				{
					Parent = SelectionContent,
					Width = SelectionContent.Width - 35,
					Stat = stat.Value,
					OnClickAction = delegate
					{
						if (TemplatePresenter?.Template != null)
						{
							base.OnClickAction(stat.Value);
						}
					}
				});
			}
		}

		private void OnAttributeAdjustmentsChanged(object sender, ValueChangedEventArgs<double> e)
		{
			foreach (StatSelectable stat in _stats)
			{
				stat.AttributeAdjustment = AttributeAdjustments;
			}
		}

		private void OnStatChoicesChanged(object sender, ValueChangedEventArgs<IReadOnlyList<int>> e)
		{
			FilterStats();
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			_ = _created;
		}

		protected override void OnSelectionContent_Resized(object sender, ResizedEventArgs e)
		{
			base.OnSelectionContent_Resized(sender, e);
			Search?.SetSize(SelectionContent.Width - 5, null);
			foreach (StatSelectable stat in _stats)
			{
				stat.Width = SelectionContent.Width - 35;
			}
		}

		private void FilterStats(string? txt = null)
		{
			if (txt == null)
			{
				txt = Search.Text;
			}
			foreach (StatSelectable stat2 in _stats)
			{
				stat2.Visible = false;
			}
			bool first = true;
			string[] array = txt!.Split(' ');
			for (int i = 0; i < array.Length; i++)
			{
				string searchTxt = array[i].Trim().ToLower();
				bool anyName = string.IsNullOrEmpty(searchTxt);
				IReadOnlyList<int> validStats = StatChoices ?? new List<int>();
				_ = validStats.Count;
				bool anyAttribute = !_statIcons.Any((AttributeToggle e) => e.Checked);
				IEnumerable<AttributeType> attributes = from e in _statIcons
					where e.Checked
					select e.Attribute;
				foreach (StatSelectable stat in _stats)
				{
					IEnumerable<AttributeType> statAttributes = stat.Stat.Attributes.Select((KeyValuePair<AttributeType, StatAttribute> e) => e.Value.Id);
					stat.Visible = (first || stat.Visible) && (anyAttribute || attributes.All((AttributeType e) => statAttributes.Contains(e))) && validStats.Contains(stat.Stat.Id) && (anyName || (stat.Stat?.Name.ToLower().Trim().Contains(searchTxt) ?? false) || (stat.Stat?.DisplayAttributes.ToLower().Contains(searchTxt) ?? false));
				}
				first = false;
			}
			SelectionContent.SortChildren((StatSelectable a, StatSelectable b) => a.Stat.Name?.CompareTo(b.Stat?.Name) ?? 0);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_stats?.DisposeAll();
			_stats?.Clear();
			_statIcons?.DisposeAll();
			_statIcons?.Clear();
		}
	}
}
