using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class MessagesMenu : SelectablesMenu<MessagesMenu.MessageKey, MessagesMenu.MessageKey>
	{
		public class MessageKey
		{
			public readonly LogEntry LogEntry;

			public readonly Message Message;

			public MessageKey(LogEntry entry, Message message)
			{
				LogEntry = entry;
				Message = message;
				base._002Ector();
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<MessagesMenu>();

		private string _template = "";

		private List<TemplateParser.TemplatePreFrag> _preFrags = new List<TemplateParser.TemplatePreFrag>();

		private int _fontSize = 25;

		public bool DebugTooltips { get; set; }

		public int Limit { get; set; } = 100;


		public string Template
		{
			get
			{
				return _template;
			}
			set
			{
				_template = value;
				_preFrags = TemplateParser.PreParse(_template);
				UpdateItemVisuals();
			}
		}

		public int FontSize
		{
			get
			{
				return _fontSize;
			}
			set
			{
				_fontSize = value;
				UpdateItemVisuals();
			}
		}

		protected override MessageKey? ExtractId(MessageKey? item)
		{
			return item;
		}

		public void Repopulate(IEnumerable<LogEntry> entries)
		{
			IEnumerable<LogEntry> entries2 = entries;
			Repopulate(delegate
			{
				_menu.set_CanSelect(false);
				_menu.set_MenuItemHeight(32);
				if (!entries2.Any())
				{
					Placeholder("Nothing");
				}
				else
				{
					foreach (LogEntry current in entries2)
					{
						PushEntry(current);
					}
				}
			});
		}

		public void PushEntry(LogEntry entry)
		{
			if (base.Empty)
			{
				Repopulate(delegate
				{
				});
				_menu.set_MenuItemHeight(1);
			}
			_menu.set_CanSelect(true);
			var (cbt, messages) = entry.ProcessAndInterpret();
			if (!messages.Any())
			{
				return;
			}
			Message message = messages.First();
			MessageKey key = new MessageKey(entry, message);
			MessageMenuItem item = new MessageMenuItem(message, _preFrags, FontSize);
			_menu.set_MenuItemHeight(Math.Max((int)item.InnerHeight, _menu.get_MenuItemHeight()));
			((Control)item).set_Parent((Container)(object)_menu);
			SetItem(key, item);
			((Control)item).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_027d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0282: Unknown result type (might be due to invalid IL or missing references)
				//IL_0289: Unknown result type (might be due to invalid IL or missing references)
				//IL_0295: Expected O, but got Unknown
				//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
				if (DebugTooltips)
				{
					if (((Control)item).get_BasicTooltipText() == null)
					{
						Tooltip tooltip = ((Control)item).get_Tooltip();
						if (tooltip != null)
						{
							((Control)tooltip).Dispose();
						}
						((Control)item).set_Tooltip((Tooltip)null);
						((Control)item).set_BasicTooltipText($"ID: {cbt.get_Id()}\n" + $"Skill ID: raw: {cbt.get_Ev().get_SkillId()} used: {message.SkillId}\n" + "Raw Skill Name: " + cbt.get_SkillName() + "\n" + $"Icon ID: {message.SkillIconId}\n" + "\n" + $"Src ID: {cbt.get_Src().get_Id()} / {cbt.get_Ev().get_SrcInstId()} (self: {message.Src.get_Self() == 1})\n" + "Src Raw Name: " + cbt.get_Src().get_Name() + "\n" + $"Src Prof: {(object)(ProfessionType)(byte)cbt.get_Src().get_Profession()} / {message.Src.get_Elite()}\n" + "\n" + $"Dst ID: {cbt.get_Dst().get_Id()} / {cbt.get_Ev().get_DstInstId()} (self: {message.Dst.get_Self() == 1})\n" + "Dst Raw Name: " + cbt.get_Dst().get_Name() + "\n" + $"Dst Prof: {(object)(ProfessionType)(byte)cbt.get_Dst().get_Profession()} / {message.Dst.get_Elite()}");
					}
				}
				else
				{
					((Control)item).set_BasicTooltipText((string)null);
					if (((Control)item).get_Tooltip() == null && message.HsSkill != null)
					{
						try
						{
							MessageMenuItem messageMenuItem = item;
							Tooltip val = new Tooltip();
							((Container)val).set_HeightSizingMode((SizingMode)1);
							((Container)val).set_WidthSizingMode((SizingMode)1);
							((Control)messageMenuItem).set_Tooltip(val);
							SkillTooltip skillTooltip = new SkillTooltip(new SkillTooltipData(message.HsSkill, message.HsSkill!.Icon ?? message.SkillIconId));
							((Control)skillTooltip).set_Parent((Container)(object)((Control)item).get_Tooltip());
							((Control)skillTooltip).set_Location(Point.get_Zero());
						}
						catch (Exception ex)
						{
							Logger.Warn(ex, "Tooltip generation failed.");
						}
					}
				}
			});
			((Control)item).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Select(key);
			});
			while (((Container)_menu).get_Children().get_Count() > Limit)
			{
				((Container)_menu).RemoveChild(((IEnumerable<Control>)((Container)_menu).get_Children()).First());
			}
		}

		private void UpdateItemVisuals()
		{
			foreach (Control child in ((Container)_menu).get_Children())
			{
				(child as MessageMenuItem)?.UpdateVisuals(_preFrags, FontSize);
			}
		}
	}
}
