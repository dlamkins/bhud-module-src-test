using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Blish_HUD.Input;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Kenedia.Modules.Core.Controls
{
	public class AutoSuggestComboBox<T> : TextBox
	{
		public static readonly TextureRegion2D TextureArrow = Blish_HUD.Controls.Resources.Control.TextureAtlasControl.GetRegion("inputboxes/dd-arrow");

		public static readonly TextureRegion2D TextureArrowActive = Blish_HUD.Controls.Resources.Control.TextureAtlasControl.GetRegion("inputboxes/dd-arrow-active");

		private FlowPanel _suggestionPanel;

		private IEnumerable<SelectableItem<T>> _selectables = Array.Empty<SelectableItem<T>>();

		private SelectableItem<T> _selectedItem;

		private BlankSelectableItem<T> _blankSelectable;

		private bool _queueQuery;

		private double _lastQueryTime;

		private bool _suggestionsOpen;

		public double QueryDelay { get; set; }

		public bool SetSelectedText { get; set; } = true;


		public int MaxSuggestionHeight { get; set; } = 200;


		public bool AllowBlankSelection
		{
			[CompilerGenerated]
			get
			{
				return _003CAllowBlankSelection_003Ek__BackingField;
			}
			set
			{
				_003CAllowBlankSelection_003Ek__BackingField = value;
				RebuildBlankSelectable();
			}
		}

		public string BlankSelectionText
		{
			[CompilerGenerated]
			get
			{
				return _003CBlankSelectionText_003Ek__BackingField;
			}
			set
			{
				_003CBlankSelectionText_003Ek__BackingField = value ?? string.Empty;
				RebuildBlankSelectable();
			}
		}

		public StringComparison Comparison { get; set; }

		public T? Selected
		{
			[CompilerGenerated]
			get
			{
				return _003CSelected_003Ek__BackingField;
			}
			set
			{
				if (!object.Equals(_003CSelected_003Ek__BackingField, value))
				{
					T oldValue = _003CSelected_003Ek__BackingField;
					_003CSelected_003Ek__BackingField = value;
					this.SelectedItemChanged?.Invoke(this, new Kenedia.Modules.Core.Models.ValueChangedEventArgs<T>(oldValue, _003CSelected_003Ek__BackingField));
					_selectedItem = GetAllSelectables().FirstOrDefault((SelectableItem<T> s) => object.Equals(s.Item, _003CSelected_003Ek__BackingField));
					if (SetSelectedText)
					{
						base.Text = _selectedItem?.GetDisplayText() ?? string.Empty;
					}
				}
			}
		}

		public Func<T, SelectableItem<T>> SelectableFactory { get; set; }

		public ObservableCollection<T> Items
		{
			[CompilerGenerated]
			get
			{
				return _003CItems_003Ek__BackingField;
			}
			set
			{
				if (_003CItems_003Ek__BackingField != value)
				{
					ObservableCollection<T> observableCollection = _003CItems_003Ek__BackingField;
					if (observableCollection != null)
					{
						observableCollection.CollectionChanged -= Items_CollectionChanged;
					}
					_003CItems_003Ek__BackingField = value;
					RebuildSelectables();
					ObservableCollection<T> observableCollection2 = _003CItems_003Ek__BackingField;
					if (observableCollection2 != null)
					{
						observableCollection2.CollectionChanged += Items_CollectionChanged;
					}
					RebuildBlankSelectable();
				}
			}
		}

		public event ValueChangedEventHandler<T> SelectedItemChanged;

		public AutoSuggestComboBox()
		{
			_003CAllowBlankSelection_003Ek__BackingField = true;
			_003CBlankSelectionText_003Ek__BackingField = string.Empty;
			Comparison = StringComparison.OrdinalIgnoreCase;
			base._002Ector();
			_suggestionPanel = new FlowPanel
			{
				Parent = GameService.Graphics.SpriteScreen,
				Visible = false,
				Width = base.Width,
				Height = 200,
				ShowBorder = true,
				BorderColor = Color.Black * 0.6f,
				BackgroundColor = new Color(20, 20, 20, 235),
				ZIndex = 1073741823,
				CanScroll = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ContentPadding = new RectangleDimensions(5, 0, 0, 0)
			};
			Blish_HUD.Controls.Control.Input.Mouse.LeftMouseButtonPressed += InputOnMousedOffDropdownPanel;
			Blish_HUD.Controls.Control.Input.Mouse.RightMouseButtonPressed += InputOnMousedOffDropdownPanel;
			RebuildBlankSelectable();
		}

		private void InputOnMousedOffDropdownPanel(object sender, MouseEventArgs e)
		{
			if (!base.MouseOver && _suggestionsOpen && !_suggestionPanel.MouseOver && !_selectables.Any((SelectableItem<T> s) => s.MouseOver))
			{
				_suggestionsOpen = false;
				_suggestionPanel.Hide();
			}
		}

		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			foreach (T item in e.NewItems?.Cast<T>() ?? Array.Empty<T>())
			{
				SelectableItem<T> selectable = SelectableFactory?.Invoke(item);
				if (selectable != null)
				{
					selectable.Parent = _suggestionPanel;
					selectable.Click += Selectable_Click;
					List<SelectableItem<T>> list = new List<SelectableItem<T>>();
					list.AddRange(_selectables);
					list.Add(selectable);
					_selectables = new _003C_003Ez__ReadOnlyList<SelectableItem<T>>(list);
				}
			}
			IEnumerable<T> oldItems = e.OldItems?.Cast<T>() ?? Array.Empty<T>();
			SelectableItem<T>[] array = _selectables.Where((SelectableItem<T> s) => oldItems.Contains(s.Item)).ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i]?.Dispose();
			}
			_selectables = _selectables.Where((SelectableItem<T> s) => !oldItems.Contains(s.Item)).ToArray();
			_queueQuery = true;
		}

		private void RebuildSelectables()
		{
			_selectables.DisposeAll();
			_selectables = Array.Empty<SelectableItem<T>>();
			if (Items == null)
			{
				return;
			}
			foreach (T item in Items)
			{
				SelectableItem<T> selectable = SelectableFactory?.Invoke(item);
				if (selectable != null)
				{
					selectable.Parent = _suggestionPanel;
					selectable.Click += Selectable_Click;
					List<SelectableItem<T>> list = new List<SelectableItem<T>>();
					list.AddRange(_selectables);
					list.Add(selectable);
					_selectables = new _003C_003Ez__ReadOnlyList<SelectableItem<T>>(list);
				}
			}
			_queueQuery = true;
		}

		private void Selectable_Click(object sender, MouseEventArgs e)
		{
			SelectableItem<T> selectable = sender as SelectableItem<T>;
			if (selectable != null)
			{
				Selected = selectable.Item;
				_suggestionsOpen = false;
				_suggestionPanel.Hide();
			}
		}

		private void Clear()
		{
			_selectables.DisposeAll();
			_selectables = Array.Empty<SelectableItem<T>>();
			_blankSelectable?.Dispose();
			_blankSelectable = null;
		}

		protected override void OnTextChanged(object sender, EventArgs e)
		{
			base.OnTextChanged(sender, e);
			_queueQuery = true;
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			_suggestionsOpen = false;
			_suggestionPanel.Hide();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			base.OnResized(e);
			FlowPanel suggestionPanel = _suggestionPanel;
			if (suggestionPanel != null)
			{
				suggestionPanel.Width = base.Width;
			}
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			base.OnMoved(e);
			_suggestionPanel?.SetLocation(base.AbsoluteBounds.Location.X, base.AbsoluteBounds.Location.Y + base.AbsoluteBounds.Height);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			base.DoUpdate(gameTime);
			if (!base.Visible)
			{
				_suggestionsOpen = false;
				_suggestionPanel?.Hide();
				return;
			}
			if (_queueQuery)
			{
				if (gameTime.TotalGameTime.TotalMilliseconds - _lastQueryTime < QueryDelay)
				{
					return;
				}
				UpdateSuggestions();
				_queueQuery = false;
				_lastQueryTime = gameTime.TotalGameTime.TotalMilliseconds;
			}
			_suggestionPanel?.SetLocation(base.AbsoluteBounds.Location.X, base.AbsoluteBounds.Location.Y + base.AbsoluteBounds.Height);
			UpdateSuggestionPanelVisibility();
		}

		private void UpdateSuggestions()
		{
			string queryString = base.Text?.ToLowerInvariant() ?? string.Empty;
			_suggestionPanel.SuspendLayout();
			BlankSelectableItem<T> blankSelectable = _blankSelectable;
			if (blankSelectable != null)
			{
				blankSelectable.Visible = true;
			}
			if (!string.IsNullOrEmpty(queryString))
			{
				foreach (SelectableItem<T> selectable in _selectables)
				{
					selectable.Visible = selectable.MatchesQuery(queryString);
				}
			}
			else
			{
				foreach (SelectableItem<T> selectable2 in _selectables)
				{
					selectable2.Visible = true;
				}
			}
			_suggestionPanel.ResumeLayout(forceRecalculate: true);
			_suggestionPanel.SetLocation(base.AbsoluteBounds.Location.X, base.AbsoluteBounds.Location.Y + base.AbsoluteBounds.Height);
			UpdateSuggestionPanelHeight();
			UpdateSuggestionPanelVisibility();
			_suggestionPanel.Invalidate();
			_suggestionPanel.SortChildren((SelectableItem<T> a, SelectableItem<T> b) => string.Compare(a.GetDisplayText(), b.GetDisplayText(), Comparison));
		}

		private void UpdateSuggestionPanelHeight()
		{
			int visibleCount = GetAllSelectables().Count((SelectableItem<T> s) => s.Visible);
			int visibleHeight = (from s in GetAllSelectables()
				where s.Visible
				select s).Sum((SelectableItem<T> s) => s.Height);
			int spacing = Math.Max(0, visibleCount - 1) * (int)_suggestionPanel.ControlPadding.Y;
			int padding = _suggestionPanel.Height - _suggestionPanel.ContentRegion.Height;
			_suggestionPanel.Height = Math.Min(MaxSuggestionHeight, visibleHeight + spacing + padding);
		}

		private void UpdateSuggestionPanelVisibility()
		{
			bool hasVisibleSuggestions = GetAllSelectables().Any((SelectableItem<T> s) => s.Visible);
			if (base.Focused || _suggestionPanel.MouseOver)
			{
				_ = 1;
			}
			else
				GetAllSelectables().Any((SelectableItem<T> s) => s.MouseOver);
			if (base.Focused && hasVisibleSuggestions)
			{
				_suggestionsOpen = true;
			}
			_suggestionPanel.Visible = hasVisibleSuggestions && _suggestionsOpen;
			if (!_suggestionPanel.Visible && SetSelectedText)
			{
				base.Text = _selectedItem?.GetDisplayText() ?? string.Empty;
			}
		}

		[IteratorStateMachine(typeof(AutoSuggestComboBox<>._003CGetAllSelectables_003Ed__62))]
		private IEnumerable<SelectableItem<T>> GetAllSelectables()
		{
			return new _003CGetAllSelectables_003Ed__62(-2)
			{
				_003C_003E4__this = this
			};
		}

		private void RebuildBlankSelectable()
		{
			_blankSelectable?.Dispose();
			_blankSelectable = null;
			if (AllowBlankSelection && _suggestionPanel != null)
			{
				_blankSelectable = new BlankSelectableItem<T>(this, BlankSelectionText)
				{
					Parent = _suggestionPanel
				};
				_blankSelectable.Click += Selectable_Click;
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.Paint(spriteBatch, bounds);
			spriteBatch.DrawOnCtrl(this, (base.Enabled && base.MouseOver) ? TextureArrowActive : TextureArrow, new Rectangle(_size.X - TextureArrow.Width - 5, _size.Y / 2 - TextureArrow.Height / 2, TextureArrow.Width, TextureArrow.Height));
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Clear();
			_suggestionPanel?.Dispose();
			_suggestionPanel = null;
			Blish_HUD.Controls.Control.Input.Mouse.LeftMouseButtonPressed -= InputOnMousedOffDropdownPanel;
			Blish_HUD.Controls.Control.Input.Mouse.RightMouseButtonPressed -= InputOnMousedOffDropdownPanel;
		}
	}
}
