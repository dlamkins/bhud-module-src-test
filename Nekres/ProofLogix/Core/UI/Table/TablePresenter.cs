using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Timers;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ProofLogix.Core.Services.PartySync.Models;
using Nekres.ProofLogix.Core.UI.Configs;
using Nekres.ProofLogix.Core.UI.KpProfile;

namespace Nekres.ProofLogix.Core.UI.Table
{
	public class TablePresenter : Presenter<TableView, TableConfig>
	{
		private System.Timers.Timer _bulkLoadTimer;

		private const int BULKLOAD_INTERVAL = 1000;

		private ConcurrentQueue<IDisposable> _disposables;

		private readonly System.Timers.Timer _cleanUpTimer;

		private static int _lockFlag;

		public TablePresenter(TableView view, TableConfig model)
			: base(view, model)
		{
			_disposables = new ConcurrentQueue<IDisposable>();
			_cleanUpTimer = new System.Timers.Timer(1000.0)
			{
				AutoReset = false
			};
			_cleanUpTimer.Elapsed += OnCleanUpTimerElapsed;
			_bulkLoadTimer = new System.Timers.Timer(1000.0)
			{
				AutoReset = false
			};
			_bulkLoadTimer.Elapsed += OnBulkLoadTimerElapsed;
			ProofLogix.Instance.PartySync.PlayerAdded += OnPartyChanged;
			ProofLogix.Instance.PartySync.PlayerRemoved += OnPartyChanged;
			ResetBulkLoadTimer();
		}

		private void OnPartyChanged(object sender, ValueEventArgs<Player> e)
		{
			ResetBulkLoadTimer();
		}

		private void OnCleanUpTimerElapsed(object sender, ElapsedEventArgs e)
		{
			_cleanUpTimer.Stop();
			IDisposable disposable;
			while (_disposables.TryDequeue(out disposable))
			{
				disposable?.Dispose();
			}
			_cleanUpTimer.Interval = 1000.0;
			_cleanUpTimer.Start();
		}

		private void OnBulkLoadTimerElapsed(object sender, ElapsedEventArgs e)
		{
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			if (Interlocked.CompareExchange(ref _lockFlag, 1, 0) != 0)
			{
				return;
			}
			FlowPanel table = base.get_View().Table;
			if (table == null)
			{
				return;
			}
			using (((Control)table).SuspendLayoutContext())
			{
				int scrollOffsetY = ((Container)table).get_VerticalScrollOffset();
				foreach (Control oldChild in ((Container)table).get_Children().ToList())
				{
					_disposables.Enqueue((IDisposable)oldChild);
				}
				List<TablePlayerEntry> bulk = (from x in ProofLogix.Instance.PartySync.PlayerList.Prepend(ProofLogix.Instance.PartySync.LocalPlayer).Select(CreatePlayerEntry)
					where x != null
					select x).ToList();
				List<TablePlayerEntry> toDisplay = bulk.Where((TablePlayerEntry x) => x.Remember || x.Player.Equals(ProofLogix.Instance.PartySync.LocalPlayer)).ToList();
				toDisplay.AddRange((from x in bulk.Except(toDisplay)
					orderby x.Player.Created descending
					select x).Take(Math.Abs(base.get_Model().MaxPlayerCount - 1)));
				toDisplay.Sort(Comparer);
				ControlCollection<Control> list = new ControlCollection<Control>((IEnumerable<Control>)toDisplay);
				foreach (Control item in list)
				{
					item.GetPrivateField("_parent").SetValue(item, table);
				}
				table.GetPrivateField("_children").SetValue(table, list);
				((Control)table).Invalidate();
				base.get_View().PlayerCountLbl.set_Text($"{toDisplay.Count}/{base.get_Model().MaxPlayerCount}");
				if (toDisplay.Count > base.get_Model().MaxPlayerCount)
				{
					base.get_View().PlayerCountLbl.set_TextColor(new Color(255, 57, 57));
				}
				else if (toDisplay.Count == base.get_Model().MaxPlayerCount)
				{
					base.get_View().PlayerCountLbl.set_TextColor(new Color(128, 255, 128));
				}
				else
				{
					base.get_View().PlayerCountLbl.set_TextColor(Color.get_White());
				}
				((Container)table).set_VerticalScrollOffset(scrollOffsetY);
				Interlocked.Exchange(ref _lockFlag, 0);
			}
		}

		private void ResetBulkLoadTimer()
		{
			if (_bulkLoadTimer != null)
			{
				try
				{
					_bulkLoadTimer.Stop();
					_bulkLoadTimer.Interval = 1000.0;
					_bulkLoadTimer.Start();
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		public TablePlayerEntry CreatePlayerEntry(Player player)
		{
			if (base.get_Model().RequireProfile && !player.HasKpProfile)
			{
				return null;
			}
			TablePlayerEntry tablePlayerEntry = new TablePlayerEntry(player);
			((Control)tablePlayerEntry).set_Height(32);
			tablePlayerEntry.Remember = ((IEnumerable<string>)base.get_Model().ProfileIds).Any((string id) => id.Equals(player.KpProfile.Id)) || player.Equals(ProofLogix.Instance.PartySync.LocalPlayer);
			TablePlayerEntry entry = tablePlayerEntry;
			((Control)entry).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				if (entry.Player.KpProfile.NotFound)
				{
					GameService.Content.PlaySoundEffectByName("error");
					ScreenNotification.ShowNotification("This player has no profile.", (NotificationType)2, (Texture2D)null, 4);
				}
				else
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					ProfileView.Open(entry.Player.KpProfile);
				}
			});
			((Control)entry).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				if (!player.Equals(ProofLogix.Instance.PartySync.LocalPlayer) && !player.KpProfile.NotFound)
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
			return entry;
		}

		private int Comparer(TablePlayerEntry x, TablePlayerEntry y)
		{
			if (base.get_Model().AlwaysSortStatus && x.Player.Status.CompareTo(y.Player.Status) != 0)
			{
				if (x.Player.Status < y.Player.Status)
				{
					return 1;
				}
				if (x.Player.Status > y.Player.Status)
				{
					return -1;
				}
			}
			if (!x.Player.HasKpProfile)
			{
				return 1;
			}
			if (!y.Player.HasKpProfile)
			{
				return -1;
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
			_bulkLoadTimer.Dispose();
			_bulkLoadTimer = null;
			OnCleanUpTimerElapsed(null, null);
			_cleanUpTimer.Dispose();
			ProofLogix.Instance.PartySync.PlayerAdded -= OnPartyChanged;
			ProofLogix.Instance.PartySync.PlayerRemoved -= OnPartyChanged;
			Interlocked.Exchange(ref _lockFlag, 0);
			base.Unload();
		}

		public void SortEntries()
		{
			base.get_View().Table.SortChildren<TablePlayerEntry>((Comparison<TablePlayerEntry>)Comparer);
		}
	}
}
