using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Services;
using rp.spark.UI.Controls;

namespace rp.spark.UI.Views
{
	internal sealed class SparkBlocklist : IDisposable
	{
		private const int InputHeight = 30;

		private const int DefaultListHeight = 360;

		private const int RowHeight = 30;

		private const int ActionButtonWidth = 90;

		private readonly SparkSettings _settings;

		private readonly Func<string, string> _blockAccount;

		private readonly Func<string, string> _unblockAccount;

		private readonly Action<Action> _watchBlocks;

		private readonly Action<Action> _unwatchBlocks;

		private readonly PageList _page = new PageList();

		private PageListControls _pageControls;

		private bool _isDisposed;

		private TextBox _input;

		private ProfileScrollList _list;

		private Label _status;

		public SparkBlocklist(SparkSettings settings, Func<string, string> blockAccount, Func<string, string> unblockAccount, Action<Action> watchBlocklistChanged, Action<Action> unwatchBlocklistChanged)
		{
			_settings = settings;
			_blockAccount = blockAccount;
			_unblockAccount = unblockAccount;
			_watchBlocks = watchBlocklistChanged;
			_unwatchBlocks = unwatchBlocklistChanged;
		}

		public void Build(FlowPanel settingsStack, int contentWidth, int listHeight = 360)
		{
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			_isDisposed = false;
			FlowPanel blockedStack = SparkFormLayout.AddAutoStack((Container)(object)settingsStack, contentWidth);
			FlowPanel inputRow = SparkFormLayout.AddRow((Container)(object)blockedStack, contentWidth, 30, 8);
			_input = SparkFormLayout.AddTextBox((Container)(object)inputRow, string.Empty, "Account name, such as Name.1234", contentWidth - 90 - 8, 30, 30);
			((Control)SparkFormLayout.AddButton((Container)(object)inputRow, "Block", 90, 30)).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AddBlock();
			});
			_input.add_EnterPressed((EventHandler<EventArgs>)delegate
			{
				AddBlock();
			});
			ProfileScrollList profileScrollList = new ProfileScrollList(contentWidth - 16, listHeight, 30);
			((Control)profileScrollList).set_Parent((Container)(object)blockedStack);
			_list = profileScrollList;
			_pageControls = new PageListControls((Container)(object)blockedStack, _page, contentWidth, delegate
			{
				Refresh(resetPage: false);
			});
			_status = SparkFormLayout.AddLabel((Container)(object)blockedStack, string.Empty, contentWidth, 24, GameService.Content.get_DefaultFont12(), SparkViewUI.SecondaryTextColor);
			_watchBlocks?.Invoke(OnBlocksChanged);
			Refresh(resetPage: true);
		}

		private void OnBlocksChanged()
		{
			SparkUiThread.Queue(delegate
			{
				if (!_isDisposed)
				{
					Refresh(resetPage: false);
				}
			});
		}

		private void AddBlock()
		{
			TextBox input = _input;
			string accountName = ((input == null) ? null : ((TextInputBase)input).get_Text()?.Trim()) ?? string.Empty;
			if (!SparkSettings.IsValidAccountName(accountName))
			{
				SetStatus("Enter an account name like Name.1234.");
				return;
			}
			SetStatus(_blockAccount?.Invoke(accountName) ?? "Couldn't update the block list.");
			if (_settings.IsBlockedAccount(accountName) && _input != null)
			{
				((TextInputBase)_input).set_Text(string.Empty);
			}
			Refresh(resetPage: true);
		}

		private void Refresh(bool resetPage)
		{
			if (_list == null)
			{
				return;
			}
			List<string> blockedAccounts = _settings.GetBlockedAccountNames().ToList();
			if (resetPage)
			{
				_page.Reset();
			}
			_page.Clamp(blockedAccounts.Count);
			_pageControls?.Update(blockedAccounts.Count);
			if (blockedAccounts.Count == 0)
			{
				_list.ShowEmptyMessage("No blocked accounts.");
				return;
			}
			_list.ClearRows();
			IReadOnlyList<string> pageRows = _page.GetPage(blockedAccounts);
			for (int index = 0; index < pageRows.Count; index++)
			{
				AddRow(index, pageRows[index]);
			}
		}

		private void AddRow(int index, string accountName)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			Panel row = _list.AddRow(index, accountName);
			int buttonX = ((Control)_list).get_Width() - 16 - 90 - 8;
			_list.AddCell((Container)(object)row, accountName, 8, 3, buttonX - 16, Color.get_White());
			StandardButton val = new StandardButton();
			val.set_Text("Unblock");
			((Control)val).set_Location(new Point(buttonX, 3));
			((Control)val).set_Size(new Point(90, 25));
			((Control)val).set_Parent((Container)(object)row);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SetStatus(_unblockAccount?.Invoke(accountName) ?? "Couldn't update the block list.");
				Refresh(resetPage: false);
			});
		}

		private void SetStatus(string message)
		{
			if (_status != null)
			{
				_status.set_Text(message ?? string.Empty);
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_unwatchBlocks?.Invoke(OnBlocksChanged);
			}
		}
	}
}
