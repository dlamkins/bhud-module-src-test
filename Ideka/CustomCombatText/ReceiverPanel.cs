using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;

namespace Ideka.CustomCombatText
{
	public class ReceiverPanel : Panel
	{
		private const int Spacing = 10;

		private List<MessageReceiver>? _receivers;

		private MessageReceiver? _target;

		private readonly StandardButton _selectButton;

		private readonly Label _infoLabel;

		private readonly StandardButton _addNewButton;

		private readonly StandardButton _deleteButton;

		private readonly StandardButton _moveUpButton;

		private readonly StandardButton _moveDownButton;

		private readonly StringBox _nameBox;

		private readonly EnumSetBox<MessageCategory> _categoriesBox;

		private readonly EnumSetBox<EventResult> _resultsBox;

		private readonly EnumDropdown<EntityFilter> _entityFilterDropdown;

		private readonly BoolBox _enabledBox;

		private readonly StringBox _templateBox;

		private ContextMenuStrip? _receiverMenu;

		public List<MessageReceiver>? Receivers
		{
			get
			{
				return _receivers;
			}
			set
			{
				_receivers = null;
				((Control)_addNewButton).set_Enabled(value != null);
				((Control)_selectButton).set_Enabled(value != null && value!.Count > 0);
				_receivers = value;
				if (Target != null)
				{
					List<MessageReceiver>? receivers = _receivers;
					if (receivers != null && receivers!.Contains(Target))
					{
						goto IL_0074;
					}
				}
				Target = _receivers?.FirstOrDefault();
				goto IL_0074;
				IL_0074:
				UpdateInfo();
			}
		}

		private MessageReceiver? Target
		{
			get
			{
				return _target;
			}
			set
			{
				_target = null;
				StandardButton deleteButton = _deleteButton;
				StandardButton moveUpButton = _moveUpButton;
				StandardButton moveDownButton = _moveDownButton;
				StringBox nameBox = _nameBox;
				StringBox nameBox2 = _nameBox;
				EnumSetBox<MessageCategory> categoriesBox = _categoriesBox;
				EnumSetBox<MessageCategory> categoriesBox2 = _categoriesBox;
				EnumSetBox<EventResult> resultsBox = _resultsBox;
				EnumSetBox<EventResult> resultsBox2 = _resultsBox;
				EnumDropdown<EntityFilter> entityFilterDropdown = _entityFilterDropdown;
				EnumDropdown<EntityFilter> entityFilterDropdown2 = _entityFilterDropdown;
				BoolBox enabledBox = _enabledBox;
				bool flag;
				((Control)_enabledBox).set_Enabled(flag = value != null);
				bool flag3 = (enabledBox.ControlEnabled = flag);
				bool flag4;
				((Control)entityFilterDropdown2).set_Enabled(flag4 = flag3);
				bool flag6 = (entityFilterDropdown.ControlEnabled = flag4);
				bool flag7;
				((Control)resultsBox2).set_Enabled(flag7 = flag6);
				bool flag9 = (resultsBox.ControlEnabled = flag7);
				bool flag10;
				((Control)categoriesBox2).set_Enabled(flag10 = flag9);
				bool flag12 = (categoriesBox.ControlEnabled = flag10);
				bool flag13;
				((Control)nameBox2).set_Enabled(flag13 = flag12);
				bool flag15 = (nameBox.ControlEnabled = flag13);
				bool flag16;
				((Control)moveDownButton).set_Enabled(flag16 = flag15);
				bool enabled;
				((Control)moveUpButton).set_Enabled(enabled = flag16);
				((Control)deleteButton).set_Enabled(enabled);
				_nameBox.Value = value?.Name ?? "";
				_categoriesBox.Value = value?.Categories ?? new HashSet<MessageCategory>();
				_resultsBox.Value = value?.Results ?? new HashSet<EventResult>();
				_entityFilterDropdown.Value = value?.EntityFilter ?? EntityFilter.Any;
				_enabledBox.Value = value?.Enabled ?? false;
				_templateBox.Value = value?.Template ?? "";
				_target = value;
				UpdateTitle();
				UpdateInfo();
			}
		}

		public ReceiverPanel()
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			((Panel)this).set_ShowTint(true);
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Select");
			_selectButton = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			_infoLabel = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Add New");
			((Control)val3).set_BasicTooltipText("Add a new message receiver to this area.");
			_addNewButton = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Delete");
			((Control)val4).set_BasicTooltipText("Delete this message receiver.");
			_deleteButton = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Move Up");
			((Control)val5).set_BasicTooltipText("Swap with the previous receiver on the list.");
			_moveUpButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Move Down");
			((Control)val6).set_BasicTooltipText("Swap with the next receiver on the list.");
			_moveDownButton = val6;
			StringBox stringBox = new StringBox();
			((Control)stringBox).set_Parent((Container)(object)this);
			stringBox.Label = "Name";
			stringBox.AllBasicTooltipText = "For easy identification only.";
			_nameBox = stringBox;
			EnumSetBox<MessageCategory> enumSetBox = new EnumSetBox<MessageCategory>(new Func<MessageCategory, string>(DataExtensions.Describe));
			((Control)enumSetBox).set_Parent((Container)(object)this);
			enumSetBox.Label = "Categories";
			enumSetBox.LabelBasicTooltipText = "Only receive messages from these categories.\nLeave empty to receive from all.";
			_categoriesBox = enumSetBox;
			EnumSetBox<EventResult> enumSetBox2 = new EnumSetBox<EventResult>(new Func<EventResult, string>(DataExtensions.Describe));
			((Control)enumSetBox2).set_Parent((Container)(object)this);
			enumSetBox2.Label = "Results";
			enumSetBox2.LabelBasicTooltipText = "Only receive messages with these results.\nLeave empty to receive all.";
			_resultsBox = enumSetBox2;
			EnumDropdown<EntityFilter> enumDropdown = new EnumDropdown<EntityFilter>(new Func<EntityFilter, string>(DataExtensions.Describe), EntityFilter.Any);
			((Control)enumDropdown).set_Parent((Container)(object)this);
			enumDropdown.Label = "Entity Filter";
			enumDropdown.AllBasicTooltipText = "Only receive messages if the source/target matches this filter.";
			_entityFilterDropdown = enumDropdown;
			BoolBox boolBox = new BoolBox();
			((Control)boolBox).set_Parent((Container)(object)this);
			boolBox.Label = "Enabled";
			boolBox.AllBasicTooltipText = "Disabled receivers do not receive any messages.";
			_enabledBox = boolBox;
			StringBox stringBox2 = new StringBox();
			((Control)stringBox2).set_Parent((Container)(object)this);
			stringBox2.Label = "Template";
			stringBox2.AllBasicTooltipText = "Templates:\n%i (icon) skill icon\n%v (value) damage done/healed/barrier applied\n%b (barrier) barrier damage taken\n%r (result) result's associated text, if any (e.g. block, invuln)\n%f (from) source of the message/skill cast\n%t (to) destination of the message/skill cast\n%s (skill) skill name\n%n (number) number of messages (for combined messages)\n\n[col=XXXXXX][/col] tags can be used to override default colors.\n%c can be used in [col] tags to use the source profession's color.\nSquare brackets can be used around templates (e.g. [%b]) to mark them as \"optional\". The content of the brackets will only be shown if the template within is relevant to the current message.\nOptional brackets must contain exactly one template.\n[col] tags only work outside optional brackets.";
			_templateBox = stringBox2;
			UpdateLayout();
			((Control)_selectButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Expected O, but got Unknown
				List<MessageReceiver> receivers5 = Receivers;
				if (receivers5 != null)
				{
					ContextMenuStrip? receiverMenu = _receiverMenu;
					if (receiverMenu != null)
					{
						((Control)receiverMenu).Dispose();
					}
					_receiverMenu = new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)menu);
					_receiverMenu!.Show((Control)(object)_selectButton);
				}
				IEnumerable<ContextMenuStripItem> menu()
				{
					foreach (MessageReceiver receiver in receivers5)
					{
						ContextMenuStripItem item = new ContextMenuStripItem(receiver.Describe + ((receiver == Target) ? " (selected)" : ""));
						((Control)item).add_Click((EventHandler<MouseEventArgs>)delegate
						{
							Target = receiver;
						});
						yield return item;
					}
				}
			});
			((Control)_addNewButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				List<MessageReceiver> receivers4 = Receivers;
				if (receivers4 != null)
				{
					MessageReceiver messageReceiver = new MessageReceiver();
					receivers4.Add(messageReceiver);
					Receivers = receivers4;
					Target = messageReceiver;
				}
			});
			((Control)_deleteButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				MessageReceiver target9 = Target;
				if (target9 != null)
				{
					List<MessageReceiver> receivers3 = Receivers;
					if (receivers3 != null && await CTextModule.ConfirmationModal.ShowAsync("Are you sure?", "This action cannot be undone.", "Delete", "Cancel"))
					{
						receivers3.Remove(target9);
						Receivers = receivers3;
					}
				}
			});
			((Control)_moveUpButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MessageReceiver target8 = Target;
				if (target8 != null)
				{
					List<MessageReceiver> receivers2 = Receivers;
					if (receivers2 != null)
					{
						int index2 = (receivers2.IndexOf(target8) - 1 + receivers2.Count) % receivers2.Count;
						receivers2.Remove(target8);
						receivers2.Insert(index2, target8);
						UpdateInfo();
					}
				}
			});
			((Control)_moveDownButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MessageReceiver target7 = Target;
				if (target7 != null)
				{
					List<MessageReceiver> receivers = Receivers;
					if (receivers != null)
					{
						int index = (receivers.IndexOf(target7) + 1) % receivers.Count;
						receivers.Remove(target7);
						receivers.Insert(index, target7);
						UpdateInfo();
					}
				}
			});
			_nameBox.ValueCommitted += delegate(string value)
			{
				MessageReceiver target6 = Target;
				if (target6 != null)
				{
					target6.Name = value;
					UpdateTitle();
				}
			};
			_categoriesBox.ValueCommitted += delegate(HashSet<MessageCategory> value)
			{
				MessageReceiver target5 = Target;
				if (target5 != null)
				{
					target5.Categories = value;
				}
			};
			_resultsBox.ValueCommitted += delegate(HashSet<EventResult> value)
			{
				MessageReceiver target4 = Target;
				if (target4 != null)
				{
					target4.Results = value;
				}
			};
			_entityFilterDropdown.ValueCommitted += delegate(EntityFilter value)
			{
				MessageReceiver target3 = Target;
				if (target3 != null)
				{
					target3.EntityFilter = value;
				}
			};
			_enabledBox.ValueCommitted += delegate(bool value)
			{
				MessageReceiver target2 = Target;
				if (target2 != null)
				{
					target2.Enabled = value;
				}
			};
			_templateBox.ValueCommitted += delegate(string value)
			{
				MessageReceiver target = Target;
				if (target != null)
				{
					target.Template = value;
				}
			};
			Receivers = null;
		}

		private void UpdateInfo()
		{
			int index = ((Target != null) ? ((Receivers?.IndexOf(Target) ?? (-1)) + 1) : 0);
			_infoLabel.set_Text($"Editing: {index} / {Receivers?.Count ?? 0}");
		}

		private void UpdateTitle()
		{
			((Panel)this).set_Title("Message Receiver: " + Target?.Describe);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			if (_selectButton != null)
			{
				StandardButton moveUpButton = _moveUpButton;
				int top;
				((Control)_moveUpButton).set_Left(top = 10);
				((Control)moveUpButton).set_Top(top);
				StandardButton moveUpButton2 = _moveUpButton;
				((Control)_moveDownButton).set_Width(top = 100);
				((Control)moveUpButton2).set_Width(top);
				((Control)(object)_moveUpButton).ArrangeTopDown(10, (Control)_moveDownButton, (Control)_nameBox, (Control)_categoriesBox, (Control)_resultsBox, (Control)_entityFilterDropdown, (Control)_templateBox);
				((Control)(object)_moveUpButton).ArrangeLeftRight(10, (Control)_selectButton, (Control)_infoLabel);
				((Control)(object)_infoLabel).MiddleWith((Control)(object)_selectButton);
				((Control)(object)_infoLabel).WidthFillRight(10);
				((Control)(object)_moveDownButton).ArrangeLeftRight(10, (Control)_addNewButton, (Control)_deleteButton);
				((Control)(object)_nameBox).WidthFillRight(10);
				((Control)(object)_categoriesBox).WidthFillRight(10);
				((Control)(object)_resultsBox).WidthFillRight(10);
				((Control)_enabledBox).set_Width(_enabledBox.ControlRight);
				((Control)_enabledBox).set_Right(((Container)this).get_ContentRegion().Width - 10);
				((Control)(object)_enabledBox).MiddleWith((Control)(object)_entityFilterDropdown);
				((Control)_entityFilterDropdown).set_Right(((Control)_enabledBox).get_Left() - 10);
				((Control)(object)_entityFilterDropdown).WidthFillLeft(10);
				((Control)(object)_templateBox).WidthFillRight(10);
				ValueControl.AlignLabels(_nameBox, _categoriesBox, _resultsBox, _entityFilterDropdown, _templateBox);
				((Container)(object)this).MatchHeightToBottom((Control)(object)_templateBox, 10);
			}
		}

		protected override void DisposeControl()
		{
			ContextMenuStrip? receiverMenu = _receiverMenu;
			if (receiverMenu != null)
			{
				((Control)receiverMenu).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
