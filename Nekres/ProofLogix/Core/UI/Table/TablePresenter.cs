using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Nekres.ProofLogix.Core.Services.PartySync.Models;
using Nekres.ProofLogix.Core.UI.Configs;
using Nekres.ProofLogix.Core.UI.KpProfile;

namespace Nekres.ProofLogix.Core.UI.Table
{
	public class TablePresenter : Presenter<TableView, TableConfig>
	{
		private readonly Timer _bulkLoadTimer;

		private const int BULKLOAD_INTERVAL = 1000;

		private readonly SynchronizedCollection<TablePlayerEntry> _bulk;

		public TablePresenter(TableView view, TableConfig model)
			: base(view, model)
		{
			_bulk = new SynchronizedCollection<TablePlayerEntry>();
			_bulkLoadTimer = new Timer(1000.0)
			{
				AutoReset = false
			};
			_bulkLoadTimer.Elapsed += OnBulkLoadTimerElapsed;
			ProofLogix.Instance.PartySync.PlayerAdded += PlayerAddedOrChanged;
			ProofLogix.Instance.PartySync.PlayerChanged += PlayerAddedOrChanged;
			ProofLogix.Instance.PartySync.PlayerRemoved += PlayerRemoved;
		}

		private void OnBulkLoadTimerElapsed(object sender, ElapsedEventArgs e)
		{
			FlowPanel table = base.get_View().Table;
			if (table == null)
			{
				return;
			}
			List<TablePlayerEntry> list2 = _bulk.ToList();
			list2.Sort(Comparer);
			ControlCollection<Control> list = new ControlCollection<Control>((IEnumerable<Control>)list2);
			foreach (Control item in list)
			{
				item.GetPrivateField("_parent").SetValue(item, table);
			}
			table.GetPrivateField("_children").SetValue(table, list);
			((Control)table).Invalidate();
		}

		private void ResetBulkLoadTimer()
		{
			_bulkLoadTimer.Stop();
			_bulkLoadTimer.Interval = 1000.0;
			_bulkLoadTimer.Start();
		}

		public void CreatePlayerEntry(Player player)
		{
			if (!player.HasKpProfile)
			{
				return;
			}
			if (TryGetPlayerEntry(player, out var playerEntry))
			{
				playerEntry.Player = player;
				return;
			}
			ResetBulkLoadTimer();
			TablePlayerEntry tablePlayerEntry = new TablePlayerEntry(player);
			((Control)tablePlayerEntry).set_Height(32);
			tablePlayerEntry.Remember = ((IEnumerable<string>)base.get_Model().ProfileIds).Any((string id) => id.Equals(player.KpProfile.Id)) || player.Equals(ProofLogix.Instance.PartySync.LocalPlayer);
			TablePlayerEntry entry = tablePlayerEntry;
			_bulk.Add(entry);
			((Control)entry).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				ProfileView.Open(entry.Player.KpProfile);
			});
			((Control)entry).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				if (!player.Equals(ProofLogix.Instance.PartySync.LocalPlayer))
				{
					if (entry.Remember)
					{
						GameService.Content.PlaySoundEffectByName("button-click");
						((IList<string>)base.get_Model().ProfileIds).RemoveAll(entry.Player.KpProfile.Id);
					}
					else
					{
						GameService.Content.PlaySoundEffectByName("color-change");
						((Collection<string>)(object)base.get_Model().ProfileIds).Add(entry.Player.KpProfile.Id);
					}
					entry.Remember = !entry.Remember;
				}
			});
		}

		private void PlayerRemoved(object sender, ValueEventArgs<Player> e)
		{
			TablePlayerEntry playerEntry;
			if (base.get_Model().KeepLeavers)
			{
				ResetBulkLoadTimer();
			}
			else if (TryGetPlayerEntry(e.get_Value(), out playerEntry) && !playerEntry.Remember)
			{
				ResetBulkLoadTimer();
				if (_bulk.Remove(playerEntry))
				{
					((Control)playerEntry).Dispose();
				}
			}
		}

		private void PlayerAddedOrChanged(object sender, ValueEventArgs<Player> e)
		{
			CreatePlayerEntry(e.get_Value());
		}

		private bool TryGetPlayerEntry(Player player, out TablePlayerEntry playerEntry)
		{
			playerEntry = _bulk.ToList().FirstOrDefault((TablePlayerEntry ctrl) => ctrl.Player.Equals(player));
			return playerEntry != null;
		}

		private int Comparer(TablePlayerEntry x, TablePlayerEntry y)
		{
			if (base.get_Model().AlwaysSortStatus && x.Player.Status.CompareTo(y.Player.Status) != 0)
			{
				if (x.Player.Status == Player.OnlineStatus.Unknown)
				{
					return 1;
				}
				if (y.Player.Status == Player.OnlineStatus.Unknown)
				{
					return -1;
				}
				if (x.Player.Status == Player.OnlineStatus.Online && y.Player.Status == Player.OnlineStatus.Away)
				{
					return -1;
				}
				if (x.Player.Status == Player.OnlineStatus.Away && y.Player.Status == Player.OnlineStatus.Online)
				{
					return 1;
				}
			}
			int column = base.get_Model().SelectedColumn;
			int comparison = 0;
			if (column == 4)
			{
				comparison = x.Player.Status.CompareTo(y.Player.Status);
			}
			if (column == 0)
			{
				comparison = x.Player.Created.CompareTo(y.Player.Created);
			}
			if (column == 1)
			{
				comparison = string.Compare(x.Player.Class, y.Player.Class, StringComparison.InvariantCultureIgnoreCase);
			}
			if (column == 2)
			{
				comparison = string.Compare(x.Player.CharacterName, y.Player.CharacterName, StringComparison.InvariantCultureIgnoreCase);
			}
			if (column == 3)
			{
				comparison = string.Compare(x.Player.AccountName, y.Player.AccountName, StringComparison.InvariantCultureIgnoreCase);
			}
			int len = Enum.GetValues(typeof(TableConfig.Column)).Length;
			if (column >= len)
			{
				int id = ((IEnumerable<int>)ProofLogix.Instance.TableConfig.get_Value().TokenIds).ElementAtOrDefault(column - len);
				comparison = x.Player.KpProfile.GetToken(id).Amount.CompareTo(y.Player.KpProfile.GetToken(id).Amount);
			}
			if (!base.get_Model().OrderDescending)
			{
				return -comparison;
			}
			return comparison;
		}

		protected override void Unload()
		{
			ProofLogix.Instance.PartySync.PlayerAdded -= PlayerAddedOrChanged;
			ProofLogix.Instance.PartySync.PlayerChanged -= PlayerAddedOrChanged;
			ProofLogix.Instance.PartySync.PlayerRemoved -= PlayerRemoved;
			_bulkLoadTimer.Dispose();
			foreach (TablePlayerEntry item in _bulk)
			{
				if (item != null)
				{
					((Control)item).Dispose();
				}
			}
			base.Unload();
		}

		public void SortEntries()
		{
			base.get_View().Table.SortChildren<TablePlayerEntry>((Comparison<TablePlayerEntry>)Comparer);
		}
	}
}
