using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using rp.spark.Models.Api;
using rp.spark.Services;
using rp.spark.UI.Controls;

namespace rp.spark.UI.Views
{
	internal sealed class RollGroupView : View
	{
		private const int ContentWidth = 760;

		private const int ContentHeight = 610;

		private const int LeftWidth = 430;

		private const int RightWidth = 310;

		private const int RowHeight = 30;

		private const int ButtonWidth = 85;

		private readonly RollGroupService _service;

		private readonly PageList _historyPage = new PageList(8);

		private Container _content;

		private Label _status;

		private Label _expiry;

		private CancellationTokenSource _clockCancel;

		private Task _clockTask;

		private string _shownGroupId;

		private long _shownRevision;

		private long _shownSequence;

		private bool _disbandArmed;

		private bool _unloaded;

		public RollGroupView(RollGroupService service)
			: this()
		{
			_service = service;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			_unloaded = false;
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(760, 610));
			_content = (Container)val;
			_service.StateChanged += OnStateChanged;
			Render();
			_clockCancel = new CancellationTokenSource();
			_clockTask = RunClockAsync(_clockCancel.Token);
			RefreshOnOpen();
		}

		private async void RefreshOnOpen()
		{
			try
			{
				await _service.RefreshAsync();
			}
			catch (OperationCanceledException)
			{
			}
			catch
			{
				SetStatus("Unable to refresh the roll group.");
			}
		}

		private void OnStateChanged()
		{
			SparkUiThread.Queue(delegate
			{
				if (!_unloaded)
				{
					RollGroup currentGroup = _service.CurrentGroup;
					if (NeedsRender(currentGroup))
					{
						Render();
					}
					else
					{
						SetStatus(_service.LastStatus);
					}
				}
			});
		}

		private bool NeedsRender(RollGroup group)
		{
			if (string.Equals(_shownGroupId, group?.GroupId ?? string.Empty, StringComparison.Ordinal) && _shownSequence == (group?.LastSequence ?? 0))
			{
				return _shownRevision != (group?.Revision ?? 0);
			}
			return true;
		}

		private void Render()
		{
			RollGroup group = _service.CurrentGroup;
			string groupId = group?.GroupId ?? string.Empty;
			if (!string.Equals(_shownGroupId ?? string.Empty, groupId, StringComparison.Ordinal))
			{
				_historyPage.Reset();
				_disbandArmed = false;
			}
			ClearContent();
			if (group == null)
			{
				BuildNoGroup();
			}
			else
			{
				BuildGroup(group);
			}
			_shownGroupId = groupId;
			_shownRevision = group?.Revision ?? 0;
			_shownSequence = group?.LastSequence ?? 0;
			SetStatus(_service.LastStatus);
		}

		private static void AddHeading(Container parent, string text, int width, int height, bool large = false)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			SparkFormLayout.AddLabel(parent, text, width, height, large ? GameService.Content.get_DefaultFont18() : GameService.Content.get_DefaultFont16(), Color.get_White(), strokeText: true);
		}

		private void BuildNoGroup()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel stack = SparkFormLayout.AddVerticalStack(_content, 90, 45, 580, 520);
			SparkFormLayout.AddLabel((Container)(object)stack, "Group up with others and use dice rolls! \nCreate a shared group for dice rolls, or join an existing group using its code.\nUp to 50 people can join a group at a time.", 580, 58, GameService.Content.get_DefaultFont16(), SparkViewUI.SecondaryTextColor).set_WrapText(true);
			AddHeading((Container)(object)stack, "Create a group", 580, 28);
			FlowPanel createRow = SparkFormLayout.AddRow((Container)(object)stack, 580, 30, 8);
			TextBox createPassword = SparkFormLayout.AddTextBox((Container)(object)createRow, string.Empty, "Password (optional)", 442, 30, 64);
			SparkUiActions.BindClick(SparkFormLayout.AddButton((Container)(object)createRow, "Create Group", 130, 30), async delegate
			{
				await _service.CreateAsync(((TextInputBase)createPassword).get_Text());
			}, SetStatus, "Unable to create the group.");
			SparkFormLayout.AddSpacer((Container)(object)stack, 580, 12);
			AddHeading((Container)(object)stack, "Join an existing group", 580, 28);
			FlowPanel joinRow = SparkFormLayout.AddRow((Container)(object)stack, 580, 30, 8);
			TextBox codeInput = SparkFormLayout.AddTextBox((Container)(object)joinRow, string.Empty, "Group code", 155, 30, 5);
			TextBox joinPassword = SparkFormLayout.AddTextBox((Container)(object)joinRow, string.Empty, "Password (if needed)", 230, 30, 64);
			SparkUiActions.BindClick(SparkFormLayout.AddButton((Container)(object)joinRow, "Join", 90, 30), join, SetStatus, "Unable to join the group.");
			codeInput.add_EnterPressed((EventHandler<EventArgs>)async delegate
			{
				await join();
			});
			_status = AddStatus((Container)(object)stack, 580, 48);
			Task join()
			{
				return _service.JoinAsync(((TextInputBase)codeInput).get_Text(), ((TextInputBase)joinPassword).get_Text());
			}
		}

		private void BuildGroup(RollGroup group)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(_content);
			((Control)val).set_Size(new Point(760, 610));
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(20f, 0f));
			FlowPanel columns = val;
			BuildRolls((Container)(object)columns, group);
			BuildMembers((Container)(object)columns, group);
		}

		private void BuildRolls(Container parent, RollGroup group)
		{
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel stack = SparkFormLayout.AddVerticalStack(parent, 0, 0, 430, 610, 8);
			AddHeading((Container)(object)stack, "Dice Rolling", 430, 30, large: true);
			FlowPanel rollRow = SparkFormLayout.AddRow((Container)(object)stack, 430, 30, 8);
			TextBox rollInput = SparkFormLayout.AddTextBox((Container)(object)rollRow, string.Empty, "Examples: d20, 1d8 + 4 #attack", 332, 30, 96);
			SparkUiActions.BindClick(SparkFormLayout.AddButton((Container)(object)rollRow, "Roll", 90, 30), () => SubmitRollAsync(rollInput), SetStatus, "Unable to submit the roll.");
			rollInput.add_EnterPressed((EventHandler<EventArgs>)async delegate
			{
				await SubmitRollAsync(rollInput);
			});
			AddHeading((Container)(object)stack, "History", 430, 26);
			ProfileScrollList profileScrollList = new ProfileScrollList(414, 350, 36);
			((Control)profileScrollList).set_Parent((Container)(object)stack);
			ProfileScrollList history = profileScrollList;
			List<RollEvent> events = (group.History ?? new List<RollEvent>()).OrderByDescending((RollEvent entry) => entry.Sequence).ToList();
			_historyPage.Clamp(events.Count);
			if (events.Count == 0)
			{
				history.ShowEmptyMessage("No activity yet.");
			}
			IReadOnlyList<RollEvent> page = _historyPage.GetPage(events);
			for (int index = 0; index < page.Count; index++)
			{
				RollEvent entry2 = page[index];
				bool isHeader = IsHeader(entry2);
				string text = (isHeader ? ("— " + HeaderText(entry2) + " —") : FormatRoll(entry2));
				Tooltip tooltip = (isHeader ? MakeHeaderTooltip(entry2) : MakeRollTooltip(entry2));
				Panel row = history.AddRow(index, tooltip);
				Label cell = history.AddCell((Container)(object)row, text, 8, 6, 390, (Color)(isHeader ? new Color(255, 215, 100) : Color.get_White()));
				if (isHeader)
				{
					cell.set_HorizontalAlignment((HorizontalAlignment)1);
				}
				((Control)cell).set_BasicTooltipText((string)null);
				((Control)cell).set_Tooltip(tooltip);
			}
			new PageListControls((Container)(object)stack, _historyPage, 430, delegate
			{
				SparkUiThread.Queue(Render);
			}).Update(events.Count);
			_status = AddStatus((Container)(object)stack, 430, 42);
		}

		private Task SubmitRollAsync(TextBox input)
		{
			return SubmitActivityAsync(input, (string text) => _service.RollAsync(text));
		}

		private Task SubmitHeaderAsync(TextBox input)
		{
			return SubmitActivityAsync(input, (string text) => _service.AddHeaderAsync(text));
		}

		private static async Task SubmitActivityAsync(TextBox input, Func<string, Task<bool>> submit)
		{
			TextBox obj = input;
			if (!(await submit((obj != null) ? ((TextInputBase)obj).get_Text() : null)))
			{
				return;
			}
			SparkUiThread.Queue(delegate
			{
				TextBox obj2 = input;
				if (((obj2 != null) ? ((Control)obj2).get_Parent() : null) != null)
				{
					((TextInputBase)input).set_Text(string.Empty);
				}
			});
		}

		private void BuildMembers(Container parent, RollGroup group)
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel stack = SparkFormLayout.AddVerticalStack(parent, 0, 0, 310, 610, 8);
			FlowPanel parent2 = SparkFormLayout.AddRow((Container)(object)stack, 310, 30, 8);
			AddHeading((Container)(object)parent2, "Group " + group.Code, 202, 30, large: true);
			((Control)SparkFormLayout.AddButton((Container)(object)parent2, "Copy Code", 100, 30)).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await CopyCodeAsync();
			});
			_expiry = SparkFormLayout.AddLabel((Container)(object)stack, string.Empty, 310, 24, GameService.Content.get_DefaultFont12(), SparkViewUI.SecondaryTextColor);
			RefreshExpiry();
			bool isOwner = group.IsOwner(_service.CurrentAccountName);
			if (isOwner)
			{
				BuildOwnerSettings(stack, group);
			}
			AddHeading((Container)(object)stack, $"Members ({group.Members?.Count ?? 0}/{50})", 310, 26);
			ProfileScrollList profileScrollList = new ProfileScrollList(294, isOwner ? 299 : 375, 30);
			((Control)profileScrollList).set_Parent((Container)(object)stack);
			ProfileScrollList memberList = profileScrollList;
			List<RollMember> members = OrderedMembers(group);
			if (members.Count == 0)
			{
				memberList.ShowEmptyMessage("No members.");
			}
			for (int index = 0; index < members.Count; index++)
			{
				AddMemberRow(memberList, group, members[index], index, isOwner);
			}
			string exitText = ((!isOwner) ? "Leave Group" : (_disbandArmed ? "Confirm Disband" : "Disband Group"));
			StandardButton exitButton = SparkFormLayout.AddButton((Container)(object)stack, exitText, 145, 30);
			if (isOwner)
			{
				SparkUiActions.BindClick(exitButton, async delegate
				{
					if (!_disbandArmed)
					{
						_disbandArmed = true;
						exitButton.set_Text("Confirm Disband");
						SetStatus("Click Confirm Disband to permanently close this group.");
					}
					else
					{
						await _service.DisbandAsync();
					}
				}, SetStatus, "Unable to disband the group.");
			}
			else
			{
				SparkUiActions.BindClick(exitButton, async delegate
				{
					await _service.LeaveAsync();
				}, SetStatus, "Unable to leave the group.");
			}
		}

		private void BuildOwnerSettings(FlowPanel stack, RollGroup group)
		{
			Checkbox allowNewMembers = SparkFormLayout.AddCheckbox((Container)(object)stack, "Allow new members", !group.JoinLocked, 310);
			allowNewMembers.add_CheckedChanged((EventHandler<CheckChangedEvent>)async delegate
			{
				await Update(string.Empty, clear: false);
			});
			FlowPanel passwordRow = SparkFormLayout.AddRow((Container)(object)stack, 310, 30, 6);
			TextBox passwordInput = SparkFormLayout.AddTextBox((Container)(object)passwordRow, string.Empty, group.HasPassword ? "Change password" : "Set password", 155, 30, 64);
			SparkUiActions.BindClick(SparkFormLayout.AddButton((Container)(object)passwordRow, "Set", 62, 30), () => Update(((TextInputBase)passwordInput).get_Text(), clear: false), SetStatus, "Unable to set the password.");
			SparkUiActions.BindClick(SparkFormLayout.AddButton((Container)(object)passwordRow, "Remove", 75, 30, group.HasPassword), () => Update(string.Empty, clear: true), SetStatus, "Unable to remove the password.");
			FlowPanel headerRow = SparkFormLayout.AddRow((Container)(object)stack, 310, 30, 6);
			TextBox headerInput = SparkFormLayout.AddTextBox((Container)(object)headerRow, string.Empty, "Add header", 212, 30, 40);
			SparkUiActions.BindClick(SparkFormLayout.AddButton((Container)(object)headerRow, "Post", 90, 30), () => SubmitHeaderAsync(headerInput), SetStatus, "Unable to add the group header.");
			headerInput.add_EnterPressed((EventHandler<EventArgs>)async delegate
			{
				await SubmitHeaderAsync(headerInput);
			});
			Task Update(string password, bool clear)
			{
				return _service.UpdateSettingsAsync(allowNewMembers.get_Checked(), password, clear);
			}
		}

		private void AddMemberRow(ProfileScrollList list, RollGroup group, RollMember member, int index, bool isOwner)
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			bool memberIsOwner = IsOwner(group, member);
			string name = FormatMember(member);
			Tooltip tooltip = MakeMemberTooltip(group, member);
			Panel row = list.AddRow(index, tooltip);
			bool num = isOwner && !memberIsOwner;
			int nameWidth = (num ? 183 : 276);
			Label obj = list.AddCell((Container)(object)row, name, 8, 3, nameWidth, (Color)(memberIsOwner ? new Color(255, 215, 100) : Color.get_White()));
			((Control)obj).set_BasicTooltipText((string)null);
			((Control)obj).set_Tooltip(tooltip);
			if (num)
			{
				StandardButton val = new StandardButton();
				val.set_Text("Kick");
				((Control)val).set_Location(new Point(201, 3));
				((Control)val).set_Size(new Point(85, 25));
				((Control)val).set_Parent((Container)(object)row);
				SparkUiActions.BindClick(val, () => _service.KickAsync(member.AccountName), SetStatus, "Unable to remove " + member.AccountName + ".");
			}
		}

		private static List<RollMember> OrderedMembers(RollGroup group)
		{
			return (from member in @group?.Members ?? new List<RollMember>()
				orderby IsOwner(@group, member) descending, member?.AccountName
				select member).ToList();
		}

		private static string FormatMember(RollMember member)
		{
			string account = member?.AccountName?.Trim() ?? "Unknown";
			string character = member?.CharacterName?.Trim() ?? string.Empty;
			if (!string.IsNullOrWhiteSpace(character))
			{
				return account + " [" + character + "]";
			}
			return account;
		}

		private static Tooltip MakeMemberTooltip(RollGroup group, RollMember member)
		{
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Expected O, but got Unknown
			string account = member?.AccountName?.Trim() ?? "Unknown";
			List<string> rolls = (from entry in @group?.History ?? new List<RollEvent>()
				where IsRoll(entry) && string.Equals(entry.AccountName?.Trim(), account, StringComparison.OrdinalIgnoreCase)
				orderby entry.Sequence descending
				select entry).Take(3).Select(FormatMemberRoll).ToList();
			return new Tooltip((ITooltipView)(object)new ProfilePresenceTooltipView(member?.CharacterName?.Trim(), account, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, showKnownFor: false, showCurrently: false, showOutOfCharacter: false, trimLongSections: false, 3, null, (rolls.Count > 0) ? "Recent Rolls" : string.Empty, string.Join("\n", rolls)));
		}

		private static bool IsRoll(RollEvent entry)
		{
			return string.Equals(entry?.Type, "roll", StringComparison.OrdinalIgnoreCase);
		}

		private static string FormatMemberRoll(RollEvent entry)
		{
			string time = entry.Timestamp.ToLocalTime().ToString("h:mmtt").ToLowerInvariant();
			string tag = (string.IsNullOrWhiteSpace(entry.Tag) ? string.Empty : ("[" + entry.Tag.Trim() + "] "));
			return $"{time} - {tag}{entry.Total}";
		}

		private static bool IsHeader(RollEvent entry)
		{
			return string.Equals(entry?.Type, "header", StringComparison.OrdinalIgnoreCase);
		}

		private static string HeaderText(RollEvent entry)
		{
			if (!string.IsNullOrWhiteSpace(entry?.Text))
			{
				return entry.Text.Trim();
			}
			return "Group Header";
		}

		private static Tooltip MakeHeaderTooltip(RollEvent entry)
		{
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			string account = entry?.AccountName?.Trim() ?? "Unknown";
			string character = entry?.CharacterName?.Trim();
			string author = (string.IsNullOrWhiteSpace(character) ? account : character);
			string time = entry?.Timestamp.ToLocalTime().ToString("h:mmtt").ToLowerInvariant() ?? string.Empty;
			string details = "[" + time + "] Posted by " + author;
			if (!string.IsNullOrWhiteSpace(character) && !string.IsNullOrWhiteSpace(account))
			{
				details = details + "\nAccount: " + account;
			}
			return new Tooltip((ITooltipView)(object)new ProfileTooltipView(HeaderText(entry), details, "Group Header"));
		}

		private static string FormatRoll(RollEvent entry)
		{
			if (entry == null)
			{
				return string.Empty;
			}
			string name = (string.IsNullOrWhiteSpace(entry.CharacterName) ? entry.AccountName : entry.CharacterName);
			string result = ((entry.Rolls == null || entry.Rolls.Count <= 1) ? entry.Total.ToString() : string.Format("[{0}] = {1}", string.Join(", ", entry.Rolls), entry.Total));
			string time = entry.Timestamp.ToLocalTime().ToString("h:mmtt").ToLowerInvariant();
			string tag = (string.IsNullOrWhiteSpace(entry.Tag) ? string.Empty : ("[" + entry.Tag.Trim() + "] "));
			return "[" + time + "] " + tag + name + " rolled " + entry.Expression + ": " + result;
		}

		private static Tooltip MakeRollTooltip(RollEvent entry)
		{
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			string details = FormatRoll(entry);
			string account = entry?.AccountName?.Trim();
			if (!string.IsNullOrWhiteSpace(entry?.CharacterName) && !string.IsNullOrWhiteSpace(account))
			{
				details = details + "\nAccount: " + account;
			}
			return new Tooltip((ITooltipView)(object)new ProfileTooltipView(string.IsNullOrWhiteSpace(entry?.Tag) ? $"Roll total: {entry?.Total ?? 0}" : (entry.Tag.Trim() + " — " + $"Roll total: {entry.Total}"), details, "Dice Roll"));
		}

		private async Task CopyCodeAsync()
		{
			string code = _service.CurrentGroup?.Code?.Trim() ?? string.Empty;
			if (string.IsNullOrWhiteSpace(code))
			{
				SetStatus("No group code is available.");
				return;
			}
			string status;
			try
			{
				status = ((await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(code)) ? ("Copied group code " + code + ".") : "Couldn't copy the group code right now.");
			}
			catch
			{
				status = "Couldn't copy the group code right now.";
			}
			SparkUiThread.Queue(delegate
			{
				SetStatus(status);
			});
		}

		private async Task RunClockAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(30.0), cancellationToken);
				}
				catch (OperationCanceledException)
				{
					return;
				}
				SparkUiThread.Queue(delegate
				{
					if (!_unloaded)
					{
						RefreshExpiry();
					}
				});
			}
		}

		private void RefreshExpiry()
		{
			if (_expiry == null)
			{
				return;
			}
			RollGroup group = _service.CurrentGroup;
			if (group == null)
			{
				_expiry.set_Text(string.Empty);
				return;
			}
			TimeSpan remaining = group.ExpiresAt.ToUniversalTime() - DateTime.UtcNow;
			if (remaining <= TimeSpan.Zero)
			{
				_expiry.set_Text("Group expired.");
			}
			else
			{
				_expiry.set_Text($"Expires in {(int)remaining.TotalHours}h {remaining.Minutes}m");
			}
		}

		private static bool IsOwner(RollGroup group, RollMember member)
		{
			return string.Equals(member?.AccountName?.Trim(), group?.OwnerAccountName?.Trim(), StringComparison.OrdinalIgnoreCase);
		}

		private void SetStatus(string message)
		{
			if (_status != null)
			{
				_status.set_Text(message ?? string.Empty);
			}
		}

		private static Label AddStatus(Container parent, int width, int height)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			Label obj = SparkFormLayout.AddLabel(parent, string.Empty, width, height, GameService.Content.get_DefaultFont12(), SparkViewUI.SecondaryTextColor);
			obj.set_WrapText(true);
			return obj;
		}

		private void ClearContent()
		{
			if (_content != null)
			{
				Control[] array = _content.get_Children().ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Dispose();
				}
				_status = null;
				_expiry = null;
			}
		}

		protected override void Unload()
		{
			_unloaded = true;
			_service.StateChanged -= OnStateChanged;
			CancellationTokenSource cancellation = _clockCancel;
			Task task = _clockTask;
			_clockCancel = null;
			_clockTask = null;
			if (cancellation != null)
			{
				cancellation.Cancel();
				TaskCleanup.DisposeWhenComplete(task, cancellation);
			}
		}
	}
}
