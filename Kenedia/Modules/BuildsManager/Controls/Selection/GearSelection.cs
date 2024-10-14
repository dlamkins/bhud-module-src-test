using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class GearSelection : BaseSelection
	{
		private List<SelectionPanelSelectable> _selectables = new List<SelectionPanelSelectable>();

		private TemplateSlotType _activeSlot;

		private GearSubSlotType _subSlotType;

		private string _filterText = string.Empty;

		private TemplatePresenter _templatePresenter;

		private List<SelectionPanelSelectable> _armors = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _trinkets = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _backs = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _weapons = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _pvpAmulets = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _pveSigils = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _pvpSigils = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _pveRunes = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _pvpRunes = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _nourishment = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _utilites = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _enrichments = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _infusions = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _powerCores = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _pveRelics = new List<SelectionPanelSelectable>();

		private List<SelectionPanelSelectable> _pvpRelics = new List<SelectionPanelSelectable>();

		public TemplatePresenter TemplatePresenter
		{
			get
			{
				return _templatePresenter;
			}
			private set
			{
				Common.SetProperty(ref _templatePresenter, value, new ValueChangedEventHandler<TemplatePresenter>(OnTemplatePresenterChanged));
			}
		}

		public Data Data { get; }

		public TemplateSlotType ActiveSlot
		{
			get
			{
				return _activeSlot;
			}
			set
			{
				Common.SetProperty(ref _activeSlot, value, new Action(ApplySlot));
			}
		}

		public GearSubSlotType SubSlotType
		{
			get
			{
				return _subSlotType;
			}
			set
			{
				Common.SetProperty(ref _subSlotType, value, new PropertyChangedEventHandler(ApplySubSlot), triggerOnUpdate: true, "SubSlotType");
			}
		}

		public GearSelection(TemplatePresenter templatePresenter, Data data)
		{
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			TemplatePresenter = templatePresenter;
			Data = data;
			Search.TextChangedAction = delegate(string txt)
			{
				_filterText = txt.Trim().ToLower();
				PerformFiltering();
			};
			SelectionContent.ControlPadding = new Vector2(2f);
			SelectionContent.FlowDirection = ControlFlowDirection.SingleTopToBottom;
			SelectionContent.SetLocation(Search.Left, Search.Bottom + 5);
			Data.Loaded += new EventHandler(Data_Loaded);
		}

		private void Data_Loaded(object sender, EventArgs e)
		{
			_armors = AddItems<SelectionPanelSelectable, Armor>(from e in Data.Armors.Values
				orderby e.Rarity descending, e.Id
				select e);
			_trinkets = AddItems<SelectionPanelSelectable, Trinket>(from e in Data.Trinkets.Values
				orderby e.Rarity descending, e.Id
				select e);
			_backs = AddItems<SelectionPanelSelectable, Trinket>(from e in Data.Backs.Values
				orderby e.Rarity descending, e.Id
				select e);
			_weapons = AddItems<SelectionPanelSelectable, Kenedia.Modules.BuildsManager.DataModels.Items.Weapon>(from e in Data.Weapons.Values
				orderby e.WeaponType, e.Rarity descending, e.Id
				select e);
			_pvpAmulets = AddItems<SelectionPanelSelectable, PvpAmulet>(from e in Data.PvpAmulets.Values
				orderby e.Name, e.Id
				select e);
			_pveSigils = AddItems<SelectionPanelSelectable, Sigil>(from e in Data.PveSigils.Values
				orderby e.Rarity descending, e.Name, e.Id
				select e);
			_pvpSigils = AddItems<SelectionPanelSelectable, Sigil>(from e in Data.PvpSigils.Values
				orderby e.Rarity descending, e.Name, e.Id
				select e);
			_pveRunes = AddItems<SelectionPanelSelectable, Rune>(from e in Data.PveRunes.Values
				orderby e.Rarity descending, e.Name, e.Id
				select e);
			_pvpRunes = AddItems<SelectionPanelSelectable, Rune>(from e in Data.PvpRunes.Values
				orderby e.Rarity descending, e.Name, e.Id
				select e);
			_nourishment = AddItems<SelectionPanelSelectable, Nourishment>(from e in Data.Nourishments.Values
				orderby e.Rarity descending, e.Name, e.Id
				select e);
			_utilites = AddItems<SelectionPanelSelectable, Enhancement>(from e in Data.Enhancements.Values
				orderby e.Name, e.Id
				select e);
			_enrichments = AddItems<SelectionPanelSelectable, Enrichment>(from e in Data.Enrichments.Values
				orderby e.Rarity descending, e.Name, e.Id
				select e);
			_infusions = AddItems<SelectionPanelSelectable, Infusion>(from e in Data.Infusions.Values
				orderby e.Rarity descending, e.Name.Length, e.Name, e.Id
				select e);
			_powerCores = AddItems<SelectionPanelSelectable, PowerCore>(from e in Data.PowerCores.Values
				orderby e.Rarity descending, e.Name.Length descending, e.Name descending, e.Id
				select e);
			_pveRelics = AddItems<SelectionPanelSelectable, Relic>(from e in Data.PveRelics.Values
				orderby e.Rarity descending, e.Name, e.Id
				select e);
			_pvpRelics = AddItems<SelectionPanelSelectable, Relic>(from e in Data.PvpRelics.Values
				orderby e.Rarity descending, e.Name, e.Id
				select e);
		}

		private List<S> AddItems<S, T>(IOrderedEnumerable<T> items) where S : SelectionPanelSelectable, new()where T : BaseItem
		{
			List<S> list = new List<S>();
			foreach (T item in items)
			{
				S entry;
				_selectables.Add(entry = new S
				{
					Parent = SelectionContent,
					Item = item,
					Visible = false,
					Width = 330,
					OnClickAction = delegate
					{
						if (TemplatePresenter?.Template != null)
						{
							base.OnClickAction(item);
						}
					},
					Type = ((item is Rune) ? SelectionPanelSelectable.SelectableType.Rune : ((item is Sigil) ? SelectionPanelSelectable.SelectableType.Sigil : ((item is Infusion) ? SelectionPanelSelectable.SelectableType.Infusion : SelectionPanelSelectable.SelectableType.None)))
				});
				list.Add(entry);
			}
			return list;
		}

		private void OnTemplatePresenterChanged(object sender, ValueChangedEventArgs<TemplatePresenter> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.TemplateChanged -= new ValueChangedEventHandler<Template>(Template_TemplateChanged);
				e.OldValue!.GameModeChanged -= new ValueChangedEventHandler<GameModeType>(Template_GameModeChanged);
			}
			if (e.NewValue != null)
			{
				e.NewValue!.TemplateChanged += new ValueChangedEventHandler<Template>(Template_TemplateChanged);
				e.NewValue!.GameModeChanged += new ValueChangedEventHandler<GameModeType>(Template_GameModeChanged);
			}
		}

		private void Template_GameModeChanged(object sender, ValueChangedEventArgs<GameModeType> e)
		{
			PerformFiltering();
		}

		private void Template_TemplateChanged(object sender, ValueChangedEventArgs<Template> e)
		{
			ApplySlot();
		}

		private void Template_ProfessionChanged(object sender, PropertyChangedEventArgs e)
		{
			string tempTxt = Search.Text;
			ApplySubSlot(sender, e);
			Search.Text = tempTxt;
		}

		private bool MatchingMethod(BaseItem item)
		{
			string filterText = _filterText ?? string.Empty;
			switch (item?.Type)
			{
			case ItemType.Consumable:
			{
				Enhancement enhancement = item as Enhancement;
				if (enhancement != null)
				{
					if (item.Name != null && !string.IsNullOrEmpty(filterText) && !item.Name.ToLower().Contains(filterText))
					{
						ConsumableDetails details = enhancement.Details;
						if (details == null)
						{
							return false;
						}
						return details.Description?.ToLower()?.Contains(filterText) == true;
					}
					return true;
				}
				Nourishment nourishment = item as Nourishment;
				if (nourishment == null)
				{
					break;
				}
				if (item.Name != null && !string.IsNullOrEmpty(filterText) && !item.Name.ToLower().Contains(filterText))
				{
					ConsumableDetails details2 = nourishment.Details;
					if (details2 == null)
					{
						return false;
					}
					return details2.Description?.ToLower()?.Contains(filterText) == true;
				}
				return true;
			}
			case ItemType.UpgradeComponent:
			{
				Rune rune = item as Rune;
				if (rune != null)
				{
					if (item.Name != null && !string.IsNullOrEmpty(filterText) && !item.Name.ToLower().Contains(filterText))
					{
						return rune.Bonus.ToLower()?.Contains(filterText) ?? false;
					}
					return true;
				}
				Sigil sigil = item as Sigil;
				if (sigil != null)
				{
					if (item.Name != null && !string.IsNullOrEmpty(filterText) && !item.Name.ToLower().Contains(filterText))
					{
						return sigil.Buff.ToLower()?.Contains(filterText) ?? false;
					}
					return true;
				}
				Infusion infusion = item as Infusion;
				if (infusion != null)
				{
					if (item.Name != null && !string.IsNullOrEmpty(filterText) && !item.Name.ToLower().Contains(filterText))
					{
						return infusion.Bonus.ToLower()?.Contains(filterText) ?? false;
					}
					return true;
				}
				break;
			}
			case ItemType.PvpAmulet:
			{
				PvpAmulet amulet = item as PvpAmulet;
				if (amulet == null)
				{
					break;
				}
				bool matched = true;
				string[] array = filterText.Split(' ');
				for (int i = 0; i < array.Length; i++)
				{
					string searchTxt = array[i].Trim().ToLower();
					if (string.IsNullOrEmpty(searchTxt))
					{
						continue;
					}
					int num;
					if (matched)
					{
						if (!item.Name.ToLower().Contains(searchTxt))
						{
							string attributesString = amulet.AttributesString;
							num = ((attributesString != null && attributesString.ToLower()?.Contains(searchTxt) == true) ? 1 : 0);
						}
						else
						{
							num = 1;
						}
					}
					else
					{
						num = 0;
					}
					matched = (byte)num != 0;
				}
				return item.Name == null || string.IsNullOrEmpty(filterText) || matched;
			}
			}
			if (item != null && item.Name != null && !string.IsNullOrEmpty(filterText))
			{
				return item.Name.ToLower().Contains(filterText);
			}
			return true;
		}

		private void ApplySubSlot(object sender, PropertyChangedEventArgs e)
		{
			Search.Text = null;
			foreach (SelectionPanelSelectable selectable in _selectables)
			{
				selectable.SubSlotType = SubSlotType;
			}
			ApplySlot();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			SelectionContent?.Children?.DisposeAll();
			_selectables?.DisposeAll();
		}

		public void PerformFiltering()
		{
			if (TemplatePresenter?.Template == null)
			{
				return;
			}
			switch (ActiveSlot)
			{
			case TemplateSlotType.Head:
			case TemplateSlotType.Shoulder:
			case TemplateSlotType.Chest:
			case TemplateSlotType.Hand:
			case TemplateSlotType.Leg:
			case TemplateSlotType.Foot:
			case TemplateSlotType.AquaBreather:
				if (SubSlotType == GearSubSlotType.Item)
				{
					foreach (SelectionPanelSelectable item7 in _armors)
					{
						item7.Visible = MatchingMethod(item7.Item) && item7.Item?.TemplateSlot == ActiveSlot && TemplatePresenter?.Template?.Profession.GetArmorType() == (item7.Item as Armor).Weight;
					}
				}
				else if (SubSlotType == GearSubSlotType.Rune)
				{
					foreach (SelectionPanelSelectable item2 in _pveRunes)
					{
						item2.Visible = MatchingMethod(item2.Item);
					}
				}
				else
				{
					if (SubSlotType != GearSubSlotType.Infusion)
					{
						break;
					}
					foreach (SelectionPanelSelectable item in _infusions)
					{
						item.Visible = MatchingMethod(item.Item);
					}
				}
				break;
			case TemplateSlotType.MainHand:
			case TemplateSlotType.OffHand:
			case TemplateSlotType.Aquatic:
			case TemplateSlotType.AltMainHand:
			case TemplateSlotType.AltOffHand:
			case TemplateSlotType.AltAquatic:
			{
				TemplateSlotType activeSlot = ActiveSlot;
				bool flag = ((activeSlot == TemplateSlotType.OffHand || activeSlot == TemplateSlotType.AltOffHand) ? true : false);
				bool slotIsOffhand = flag;
				if (SubSlotType == GearSubSlotType.Item)
				{
					foreach (SelectionPanelSelectable item3 in _weapons)
					{
						bool weaponMatch = true;
						KeyValuePair<Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon.WeaponType, Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon> weapon = Data.Professions[TemplatePresenter?.Template.Profession ?? ProfessionType.Guardian].Weapons.Where<KeyValuePair<Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon.WeaponType, Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon>>((KeyValuePair<Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon.WeaponType, Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon> e) => (item3.Item as Kenedia.Modules.BuildsManager.DataModels.Items.Weapon).WeaponType.IsWeaponType(e.Value.Type)).FirstOrDefault();
						if (weapon.Value != null)
						{
							activeSlot = ActiveSlot;
							flag = ((activeSlot == TemplateSlotType.Aquatic || activeSlot == TemplateSlotType.AltAquatic) ? true : false);
							bool terrainMatch = (flag ? weapon.Value.Type.IsAquatic(ActiveSlot) : (!weapon.Value.Type.IsAquatic(ActiveSlot)));
							bool wieldMatch = (slotIsOffhand ? weapon.Value.Wielded.HasFlag(Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon.WieldingFlag.Offhand) : (weapon.Value.Wielded.HasFlag(Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon.WieldingFlag.Mainhand) || weapon.Value.Wielded.HasFlag(Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon.WieldingFlag.TwoHand)));
							if (weapon.Value.Specialization == 0 && wieldMatch)
							{
								weaponMatch = true;
							}
							if (weapon.Value.Specialization == TemplatePresenter?.Template.EliteSpecialization?.Id && wieldMatch)
							{
								weaponMatch = true;
							}
							if (ActiveSlot != TemplateSlotType.AltAquatic && ActiveSlot != TemplateSlotType.AltMainHand && ActiveSlot != TemplateSlotType.AltOffHand)
							{
								_ = ActiveSlot;
							}
							item3.Visible = wieldMatch && weaponMatch && terrainMatch && MatchingMethod(item3.Item);
						}
					}
				}
				else if (SubSlotType == GearSubSlotType.Sigil)
				{
					TemplatePresenter templatePresenter2 = TemplatePresenter;
					foreach (SelectionPanelSelectable item20 in (templatePresenter2 != null && !templatePresenter2.IsPve) ? _pveSigils : _pvpSigils)
					{
						item20.Visible = false;
					}
					TemplatePresenter templatePresenter3 = TemplatePresenter;
					foreach (SelectionPanelSelectable item9 in (templatePresenter3 != null && !templatePresenter3.IsPve) ? _pvpSigils : _pveSigils)
					{
						item9.Visible = MatchingMethod(item9.Item);
					}
				}
				else
				{
					if (SubSlotType != GearSubSlotType.Infusion)
					{
						break;
					}
					foreach (SelectionPanelSelectable item8 in _infusions)
					{
						item8.Visible = MatchingMethod(item8.Item);
					}
				}
				break;
			}
			case TemplateSlotType.Amulet:
				if (SubSlotType == GearSubSlotType.Item)
				{
					foreach (SelectionPanelSelectable item16 in _trinkets)
					{
						item16.Visible = MatchingMethod(item16.Item) && item16.Item?.TemplateSlot == ActiveSlot;
					}
				}
				else
				{
					if (SubSlotType != GearSubSlotType.Enrichment)
					{
						break;
					}
					foreach (SelectionPanelSelectable item13 in _enrichments)
					{
						item13.Visible = MatchingMethod(item13.Item);
					}
				}
				break;
			case TemplateSlotType.Back:
				if (SubSlotType == GearSubSlotType.Item)
				{
					foreach (SelectionPanelSelectable item12 in _backs)
					{
						item12.Visible = MatchingMethod(item12.Item) && item12.Item?.TemplateSlot == ActiveSlot;
					}
				}
				else
				{
					if (SubSlotType != GearSubSlotType.Infusion)
					{
						break;
					}
					foreach (SelectionPanelSelectable item10 in _infusions)
					{
						item10.Visible = MatchingMethod(item10.Item);
					}
				}
				break;
			case TemplateSlotType.Accessory_1:
			case TemplateSlotType.Accessory_2:
			case TemplateSlotType.Ring_1:
			case TemplateSlotType.Ring_2:
				if (SubSlotType == GearSubSlotType.Item)
				{
					foreach (SelectionPanelSelectable item18 in _trinkets)
					{
						TemplateSlotType effectiveSlot = ((ActiveSlot == TemplateSlotType.Ring_2) ? TemplateSlotType.Ring_1 : ((ActiveSlot == TemplateSlotType.Accessory_2) ? TemplateSlotType.Accessory_1 : ActiveSlot));
						int visible;
						if (MatchingMethod(item18.Item))
						{
							BaseItem item19 = item18.Item;
							visible = ((item19 != null && item19.TemplateSlot == effectiveSlot) ? 1 : 0);
						}
						else
						{
							visible = 0;
						}
						item18.Visible = (byte)visible != 0;
					}
				}
				else
				{
					if (SubSlotType != GearSubSlotType.Infusion)
					{
						break;
					}
					foreach (SelectionPanelSelectable item17 in _infusions)
					{
						item17.Visible = MatchingMethod(item17.Item);
					}
				}
				break;
			case TemplateSlotType.Nourishment:
				foreach (SelectionPanelSelectable item11 in _nourishment)
				{
					item11.Visible = MatchingMethod(item11.Item);
				}
				break;
			case TemplateSlotType.Enhancement:
				foreach (SelectionPanelSelectable item6 in _utilites)
				{
					item6.Visible = MatchingMethod(item6.Item);
				}
				break;
			case TemplateSlotType.PveRelic:
			case TemplateSlotType.PvpRelic:
			{
				TemplatePresenter templatePresenter = TemplatePresenter;
				foreach (SelectionPanelSelectable item4 in (templatePresenter != null && !templatePresenter.IsPve) ? _pvpRelics : _pveRelics)
				{
					item4.Visible = MatchingMethod(item4.Item);
				}
				break;
			}
			case TemplateSlotType.PowerCore:
				foreach (SelectionPanelSelectable item5 in _powerCores)
				{
					item5.Visible = MatchingMethod(item5.Item);
				}
				break;
			case TemplateSlotType.PvpAmulet:
				if (SubSlotType == GearSubSlotType.Item)
				{
					foreach (SelectionPanelSelectable item14 in _pvpAmulets)
					{
						item14.Visible = MatchingMethod(item14.Item) && item14.Item?.TemplateSlot == ActiveSlot;
					}
				}
				else
				{
					if (SubSlotType != GearSubSlotType.Rune)
					{
						break;
					}
					foreach (SelectionPanelSelectable item15 in _pvpRunes)
					{
						item15.Visible = MatchingMethod(item15.Item);
					}
				}
				break;
			}
			SelectionContent.SortChildren((SelectionPanelSelectable a, SelectionPanelSelectable b) => a.Item.Name.CompareTo(b.Item.Name));
			SelectionContent.Invalidate();
		}

		public void ApplySlot()
		{
			foreach (SelectionPanelSelectable selectable in _selectables)
			{
				selectable.Visible = false;
				selectable.ActiveSlot = ActiveSlot;
				selectable.Width = 330;
			}
			PerformFiltering();
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			Search?.SetSize(base.Width - Search.Left, null);
		}
	}
}
