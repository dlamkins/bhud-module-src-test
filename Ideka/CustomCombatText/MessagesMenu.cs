using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.ArcDps.Models;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace Ideka.CustomCombatText
{
	public class MessagesMenu : SelectablesMenu<MessagesMenu.MessageKey, MessagesMenu.MessageKey, MessagesMenuItem>
	{
		public class MessageKey
		{
			public readonly LogEntry LogEntry;

			public readonly CombatEvent Cbt;

			public readonly Message Message;

			public MessageKey(LogEntry entry, CombatEvent cbt, Message message)
			{
				LogEntry = entry;
				Cbt = cbt;
				Message = message;
				base._002Ector();
			}
		}

		private string _template = "";

		private List<TemplateParser.MarkupFragment> _markupFrags = new List<TemplateParser.MarkupFragment>();

		private int _fontSize = 25;

		public bool DebugTooltips { get; set; }

		public string Template
		{
			get
			{
				return _template;
			}
			set
			{
				_template = value;
				_markupFrags = TemplateParser.ParseMarkup(_template);
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

		protected override MessageKey ExtractId(MessageKey item)
		{
			return item;
		}

		protected override MessagesMenuItem Construct(MessagesMenuItem? item, MessageKey key)
		{
			MessagesMenuItem item2 = item;
			if (item2 == null)
			{
				item2 = new MessagesMenuItem(key, _markupFrags, FontSize);
				((Control)item2).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					Select(item2.Key);
				});
				((Control)item2).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					if (DebugTooltips)
					{
						item2.CreateDebugTooltip();
					}
					else
					{
						item2.CreateSkillTooltip();
					}
				});
			}
			else
			{
				item2.UpdateVisuals(key, _markupFrags, FontSize);
			}
			base.MenuItemHeight = Math.Max((int)item2.InnerHeight, base.MenuItemHeight);
			return item2;
		}

		public void Repopulate(IEnumerable<LogEntry> entries)
		{
			IEnumerable<LogEntry> entries2 = entries;
			Repopulate(delegate
			{
				base.MenuItemHeight = 32;
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
				base.MenuItemHeight = 1;
			}
			var (cbt, messages) = entry.ProcessAndInterpret();
			if (messages.Any())
			{
				Message message = messages.First();
				MessageKey key = new MessageKey(entry, cbt, message);
				SetSelectable(key, key);
				while (base.Selectables.Count > CTextModule.Settings.MessageLogLength.Value && base.Selectables.Count > 0)
				{
					RemoveSelectable(base.Selectables.First().Key);
				}
			}
		}

		private void UpdateItemVisuals()
		{
			foreach (MessagesMenuItem item in base.Items.Values)
			{
				if (item != null)
				{
					MessagesMenuItem messageItem = item;
					messageItem.UpdateVisuals(_markupFrags, FontSize);
				}
			}
		}
	}
}
