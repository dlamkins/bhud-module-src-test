using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Kenedia.Modules.Characters.Enums;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Interfaces;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls.SideMenu
{
	public class SideMenuToggles : FlowTab, ILocalizable
	{
		private readonly List<Tag> _tags = new List<Tag>();

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _toggleFlowPanel;

		private readonly TagFlowPanel _tagFlowPanel;

		private readonly List<KeyValuePair<ImageColorToggle, Action>> _toggles = new List<KeyValuePair<ImageColorToggle, Action>>();

		private readonly TextureManager _textureManager;

		private readonly SearchFilterCollection _tagFilters;

		private readonly SearchFilterCollection _searchFilters;

		private readonly Action _onFilterChanged;

		private readonly TagList _allTags;

		private readonly Data _data;

		private Rectangle _contentRectangle;

		public event EventHandler TogglesChanged;

		public SideMenuToggles(TextureManager textureManager, SearchFilterCollection tagFilters, SearchFilterCollection searchFilters, Action onFilterChanged, TagList allTags, Data data)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			_textureManager = textureManager;
			_tagFilters = tagFilters;
			_searchFilters = searchFilters;
			_onFilterChanged = onFilterChanged;
			_allTags = allTags;
			_data = data;
			base.FlowDirection = ControlFlowDirection.SingleTopToBottom;
			base.AutoSizePadding = new Point(5, 5);
			HeightSizingMode = SizingMode.AutoSize;
			base.OuterControlPadding = new Vector2(5f, 5f);
			base.ControlPadding = new Vector2(5f, 3f);
			base.Location = new Point(0, 25);
			_toggleFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				FlowDirection = ControlFlowDirection.TopToBottom,
				ControlPadding = new Vector2(5f, 3f),
				Height = 286,
				Width = base.Width
			};
			_tagFlowPanel = new TagFlowPanel
			{
				Parent = this,
				FlowDirection = ControlFlowDirection.LeftToRight,
				ControlPadding = new Vector2(5f, 3f),
				Width = base.Width
			};
			CreateToggles();
			CreateTags();
			GameService.Overlay.UserLocale.SettingChanged += OnLanguageChanged;
			_allTags.CollectionChanged += Tags_CollectionChanged;
			OnLanguageChanged();
		}

		public void ResetToggles()
		{
			_tags.ForEach(delegate(Tag t)
			{
				t.SetActive(active: false);
			});
			_toggles.ForEach(delegate(KeyValuePair<ImageColorToggle, Action> t)
			{
				t.Key.Active = false;
			});
			foreach (KeyValuePair<string, SearchFilter<Character_Model>> searchFilter in _searchFilters)
			{
				searchFilter.Value.IsEnabled = false;
			}
			foreach (KeyValuePair<string, SearchFilter<Character_Model>> tagFilter in _tagFilters)
			{
				tagFilter.Value.IsEnabled = false;
			}
		}

		private void Tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			CreateTags();
			Invalidate();
		}

		private void CreateTags()
		{
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<string> tagLlist = _tags.Select((Tag e) => e.Text);
			IEnumerable<string> deleteTags = tagLlist.Except(_allTags);
			IEnumerable<string> addTags = _allTags.Except(tagLlist);
			if (!deleteTags.Any() && !addTags.Any())
			{
				return;
			}
			List<Tag> deleteList = new List<Tag>();
			foreach (string tag2 in deleteTags)
			{
				Tag t3 = _tags.FirstOrDefault((Tag e) => e.Text == tag2);
				if (t3 != null)
				{
					deleteList.Add(t3);
				}
			}
			foreach (Tag t2 in deleteList)
			{
				t2.Dispose();
				_tags.Remove(t2);
			}
			foreach (string tag in addTags)
			{
				Tag t;
				_tags.Add(t = new Tag
				{
					Parent = _tagFlowPanel,
					Text = tag,
					ShowDelete = true,
					CanInteract = true
				});
				t.OnDeleteAction = delegate
				{
					_tags.Remove(t);
					_allTags.Remove(t.Text);
					_tagFlowPanel.Invalidate();
				};
				t.OnClickAction = delegate
				{
					_tagFilters[t.Text].IsEnabled = t.Active;
					_onFilterChanged?.Invoke();
				};
				if (!_tagFilters.ContainsKey(tag))
				{
					_tagFilters.Add(tag, new SearchFilter<Character_Model>((Character_Model c) => c.Tags.Contains(tag)));
				}
				t.SetActive(active: false);
			}
			_tagFlowPanel.FitWidestTag(base.ContentRegion.Width);
		}

		private void CreateToggles()
		{
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_0705: Unknown result type (might be due to invalid IL or missing references)
			//IL_085a: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0945: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<ProfessionType, Data.Profession> profs = _data.Professions.ToDictionary<KeyValuePair<ProfessionType, Data.Profession>, ProfessionType, Data.Profession>((KeyValuePair<ProfessionType, Data.Profession> entry) => entry.Key, (KeyValuePair<ProfessionType, Data.Profession> entry) => entry.Value);
			profs = (from e in profs
				orderby e.Value.WeightClass, e.Value.APIId
				select e).ToDictionary((KeyValuePair<ProfessionType, Data.Profession> e) => e.Key, (KeyValuePair<ProfessionType, Data.Profession> e) => e.Value);
			foreach (KeyValuePair<ProfessionType, Data.Profession> profession2 in profs)
			{
				ImageColorToggle t6 = new ImageColorToggle(delegate(bool b)
				{
					action(b, "Core " + profession2.Value.Name);
				})
				{
					Texture = profession2.Value.IconBig,
					UseGrayScale = false,
					ColorActive = profession2.Value.Color,
					ColorHovered = profession2.Value.Color,
					ColorInActive = profession2.Value.Color * 0.5f,
					Active = _searchFilters["Core " + profession2.Value.Name].IsEnabled,
					BasicTooltipText = "Core " + profession2.Value.Name,
					Alpha = 0.7f
				};
				KeyValuePair<ImageColorToggle, Action> tt = new KeyValuePair<ImageColorToggle, Action>(t6, delegate
				{
					t6.BasicTooltipText = "Core " + profession2.Value.Name;
				});
				_toggles.Add(tt);
			}
			foreach (KeyValuePair<ProfessionType, Data.Profession> profession in profs)
			{
				ImageColorToggle t5 = new ImageColorToggle(delegate(bool b)
				{
					action(b, profession.Value.Name);
				})
				{
					Texture = profession.Value.IconBig,
					Active = _searchFilters[profession.Value.Name].IsEnabled,
					BasicTooltipText = profession.Value.Name
				};
				_toggles.Add(new KeyValuePair<ImageColorToggle, Action>(t5, delegate
				{
					t5.BasicTooltipText = profession.Value.Name;
				}));
			}
			List<KeyValuePair<ImageColorToggle, Action>> specToggles = new List<KeyValuePair<ImageColorToggle, Action>>();
			foreach (KeyValuePair<SpecializationType, Data.Specialization> specialization in _data.Specializations)
			{
				ImageColorToggle t4 = new ImageColorToggle(delegate(bool b)
				{
					action(b, specialization.Value.Name);
				})
				{
					Texture = specialization.Value.IconBig,
					Profession = specialization.Value.Profession,
					Active = _searchFilters[specialization.Value.Name].IsEnabled,
					BasicTooltipText = specialization.Value.Name
				};
				specToggles.Add(new KeyValuePair<ImageColorToggle, Action>(t4, delegate
				{
					t4.BasicTooltipText = specialization.Value.Name;
				}));
			}
			for (int i = 0; i < 3; i++)
			{
				foreach (KeyValuePair<ProfessionType, Data.Profession> p in profs)
				{
					KeyValuePair<ImageColorToggle, Action> t = specToggles.Find((KeyValuePair<ImageColorToggle, Action> e) => p.Key == e.Key.Profession && !_toggles.Contains(e));
					if (t.Key != null)
					{
						_toggles.Add(t);
					}
				}
			}
			foreach (KeyValuePair<int, Data.CraftingProfession> crafting in _data.CrafingProfessions)
			{
				if (crafting.Key > 0)
				{
					ImageColorToggle img = new ImageColorToggle(delegate(bool b)
					{
						action(b, crafting.Value.Name);
					})
					{
						Texture = crafting.Value.Icon,
						UseGrayScale = false,
						TextureRectangle = ((crafting.Key > 0) ? new Rectangle(8, 7, 17, 19) : new Rectangle(4, 4, 24, 24)),
						SizeRectangle = new Rectangle(4, 4, 20, 20),
						Active = _searchFilters[crafting.Value.Name].IsEnabled,
						BasicTooltipText = crafting.Value.Name
					};
					_toggles.Add(new KeyValuePair<ImageColorToggle, Action>(img, delegate
					{
						img.BasicTooltipText = crafting.Value.Name;
					}));
				}
			}
			ImageColorToggle hidden = new ImageColorToggle(delegate(bool b)
			{
				action(b, "Hidden");
			})
			{
				Texture = AsyncTexture2D.FromAssetId(605021),
				UseGrayScale = true,
				TextureRectangle = new Rectangle(4, 4, 24, 24),
				BasicTooltipText = strings.ShowHidden_Tooltip
			};
			_toggles.Add(new KeyValuePair<ImageColorToggle, Action>(hidden, delegate
			{
				hidden.BasicTooltipText = strings.ShowHidden_Tooltip;
			}));
			ImageColorToggle birthday = new ImageColorToggle(delegate(bool b)
			{
				action(b, "Birthday");
			})
			{
				Texture = AsyncTexture2D.FromAssetId(593864),
				UseGrayScale = true,
				TextureRectangle = new Rectangle(1, 0, 30, 32),
				BasicTooltipText = strings.Show_Birthday_Tooltip
			};
			_toggles.Add(new KeyValuePair<ImageColorToggle, Action>(birthday, delegate
			{
				birthday.BasicTooltipText = strings.Show_Birthday_Tooltip;
			}));
			foreach (KeyValuePair<RaceType, Data.Race> race in _data.Races)
			{
				ImageColorToggle t3 = new ImageColorToggle(delegate(bool b)
				{
					action(b, race.Value.Name);
				})
				{
					Texture = race.Value.Icon,
					UseGrayScale = true,
					BasicTooltipText = race.Value.Name
				};
				_toggles.Add(new KeyValuePair<ImageColorToggle, Action>(t3, delegate
				{
					t3.BasicTooltipText = race.Value.Name;
				}));
			}
			ImageColorToggle male = new ImageColorToggle(delegate(bool b)
			{
				action(b, "Male");
			})
			{
				Texture = (AsyncTexture2D)_textureManager.GetIcon(TextureManager.Icons.Male),
				UseGrayScale = true,
				TextureRectangle = new Rectangle(1, 0, 30, 32),
				BasicTooltipText = strings.Show_Birthday_Tooltip
			};
			_toggles.Add(new KeyValuePair<ImageColorToggle, Action>(male, delegate
			{
				male.BasicTooltipText = strings.Male;
			}));
			ImageColorToggle female = new ImageColorToggle(delegate(bool b)
			{
				action(b, "Female");
			})
			{
				Texture = (AsyncTexture2D)_textureManager.GetIcon(TextureManager.Icons.Female),
				UseGrayScale = true,
				TextureRectangle = new Rectangle(1, 0, 30, 32),
				BasicTooltipText = strings.Show_Birthday_Tooltip
			};
			_toggles.Add(new KeyValuePair<ImageColorToggle, Action>(female, delegate
			{
				female.BasicTooltipText = strings.Female;
			}));
			int j = 0;
			foreach (KeyValuePair<ImageColorToggle, Action> t2 in _toggles)
			{
				j++;
				t2.Key.Parent = _toggleFlowPanel;
				t2.Key.Size = new Point(29, 29);
			}
			void action(bool active, string entry)
			{
				_searchFilters[entry].IsEnabled = active;
				_onFilterChanged?.Invoke();
			}
		}

		public void OnLanguageChanged(object s = null, EventArgs e = null)
		{
			_toggles.ForEach(delegate(KeyValuePair<ImageColorToggle, Action> t)
			{
				t.Value();
			});
		}

		public void OnTogglesChanged(object s = null, EventArgs e = null)
		{
			this.TogglesChanged?.Invoke(this, e);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			GameService.Overlay.UserLocale.SettingChanged -= OnLanguageChanged;
			_allTags.CollectionChanged -= Tags_CollectionChanged;
			_tagFilters.Clear();
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
			_toggleFlowPanel.Width = _contentRectangle.Width;
			_tagFlowPanel.FitWidestTag(_contentRectangle.Width);
		}

		protected override void OnShown(EventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			base.OnShown(e);
			_tagFlowPanel.FitWidestTag(base.ContentRegion.Width);
			Invalidate();
		}
	}
}
