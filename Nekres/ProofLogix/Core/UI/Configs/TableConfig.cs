using System;
using System.Collections.ObjectModel;
using MonoGame.Extended.Collections;
using Nekres.ProofLogix.Core.Services;
using Newtonsoft.Json;

namespace Nekres.ProofLogix.Core.UI.Configs
{
	public class TableConfig : ConfigBase
	{
		public enum Column
		{
			Timestamp,
			Class,
			Character,
			Account,
			Status
		}

		private int _selectedColumn;

		private bool _orderDescending;

		private bool _keepLeavers;

		private bool _alwaysSortStatus;

		private PartySyncService.ColorGradingMode _colorGradingMode;

		private int _maxPlayerCount = 100;

		private bool _requireProfile = true;

		private ObservableCollection<int> _tokenIds = new ObservableCollection<int>();

		private ObservableCollection<string> _profileIds = new ObservableCollection<string>();

		private ObservableCollection<Column> _columns = new ObservableCollection<Column>();

		public static TableConfig Default
		{
			get
			{
				TableConfig obj = new TableConfig
				{
					_alwaysSortStatus = true,
					_colorGradingMode = PartySyncService.ColorGradingMode.MedianComparison,
					_maxPlayerCount = 100,
					_requireProfile = true,
					_profileIds = new ObservableCollection<string>()
				};
				ObservableCollection<int> obj2 = new ObservableCollection<int>();
				((Collection<int>)(object)obj2).Add(77302);
				((Collection<int>)(object)obj2).Add(94020);
				((Collection<int>)(object)obj2).Add(93781);
				obj._tokenIds = obj2;
				ObservableCollection<Column> obj3 = new ObservableCollection<Column>();
				((Collection<Column>)(object)obj3).Add(Column.Status);
				((Collection<Column>)(object)obj3).Add(Column.Timestamp);
				((Collection<Column>)(object)obj3).Add(Column.Class);
				((Collection<Column>)(object)obj3).Add(Column.Character);
				((Collection<Column>)(object)obj3).Add(Column.Account);
				obj._columns = obj3;
				return obj;
			}
		}

		[JsonProperty("selected_column")]
		public int SelectedColumn
		{
			get
			{
				return _selectedColumn;
			}
			set
			{
				_selectedColumn = value;
				SaveConfig<TableConfig>(ProofLogix.Instance.TableConfig);
			}
		}

		[JsonProperty("order_descending")]
		public bool OrderDescending
		{
			get
			{
				return _orderDescending;
			}
			set
			{
				_orderDescending = value;
				SaveConfig<TableConfig>(ProofLogix.Instance.TableConfig);
			}
		}

		[JsonProperty("keep_leavers")]
		public bool KeepLeavers
		{
			get
			{
				return _keepLeavers;
			}
			set
			{
				_keepLeavers = value;
				SaveConfig<TableConfig>(ProofLogix.Instance.TableConfig);
			}
		}

		[JsonProperty("always_sort_status")]
		public bool AlwaysSortStatus
		{
			get
			{
				return _alwaysSortStatus;
			}
			set
			{
				_alwaysSortStatus = value;
				SaveConfig<TableConfig>(ProofLogix.Instance.TableConfig);
			}
		}

		[JsonProperty("color_grading_mode")]
		public PartySyncService.ColorGradingMode ColorGradingMode
		{
			get
			{
				return _colorGradingMode;
			}
			set
			{
				_colorGradingMode = value;
				SaveConfig<TableConfig>(ProofLogix.Instance.TableConfig);
			}
		}

		[JsonProperty("max_player_count")]
		public int MaxPlayerCount
		{
			get
			{
				return _maxPlayerCount;
			}
			set
			{
				_maxPlayerCount = value;
				SaveConfig<TableConfig>(ProofLogix.Instance.TableConfig);
			}
		}

		[JsonProperty("require_profile")]
		public bool RequireProfile
		{
			get
			{
				return _requireProfile;
			}
			set
			{
				_requireProfile = value;
				SaveConfig<TableConfig>(ProofLogix.Instance.TableConfig);
			}
		}

		[JsonProperty("token_ids")]
		public ObservableCollection<int> TokenIds
		{
			get
			{
				return _tokenIds;
			}
			set
			{
				_tokenIds = ResetDelegates<int>(_tokenIds, value);
			}
		}

		[JsonProperty("profile_ids")]
		public ObservableCollection<string> ProfileIds
		{
			get
			{
				return _profileIds;
			}
			set
			{
				_profileIds = ResetDelegates<string>(_profileIds, value);
			}
		}

		[JsonProperty("columns")]
		public ObservableCollection<Column> Columns
		{
			get
			{
				return _columns;
			}
			set
			{
				_columns = ResetDelegates<Column>(_columns, value);
			}
		}

		private ObservableCollection<T> ResetDelegates<T>(ObservableCollection<T> oldCollection, ObservableCollection<T> newCollection)
		{
			if (oldCollection != null)
			{
				oldCollection.remove_ItemRemoved((EventHandler<ItemEventArgs<T>>)OnAddOrRemove<T>);
				oldCollection.remove_ItemAdded((EventHandler<ItemEventArgs<T>>)OnAddOrRemove<T>);
			}
			if (newCollection == null)
			{
				newCollection = new ObservableCollection<T>();
			}
			newCollection.add_ItemRemoved((EventHandler<ItemEventArgs<T>>)OnAddOrRemove<T>);
			newCollection.add_ItemAdded((EventHandler<ItemEventArgs<T>>)OnAddOrRemove<T>);
			return newCollection;
		}

		private void OnAddOrRemove<T>(object o, ItemEventArgs<T> e)
		{
			SaveConfig<TableConfig>(ProofLogix.Instance.TableConfig);
		}
	}
}
