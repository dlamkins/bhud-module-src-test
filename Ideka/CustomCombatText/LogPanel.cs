using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class LogPanel : Panel, IUIPanel
	{
		private const int Spacing = 10;

		private const string DefaultTemplate = "%i %s [%v ][%b ][%r ]%m %f %o %t";

		private readonly PanelStack _panelStack;

		private readonly StandardButton _backButton;

		private readonly StringBox _templateBox;

		private readonly StandardButton _resetButton;

		private readonly BoolBox _stickToBottomBox;

		private readonly BoolBox _debugTooltipsBox;

		private readonly GlowButton _filterButton;

		private readonly MessagesMenu _messagesMenu;

		public Panel Panel => (Panel)(object)this;

		public AsyncTexture2D Icon { get; } = AsyncTexture2D.FromAssetId(1414035);


		public string Caption => "Message Log";

		public LogPanel(PanelStack panelStack)
			: this()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Expected O, but got Unknown
			_panelStack = panelStack;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Back");
			_backButton = val;
			StringBox stringBox = new StringBox();
			((Control)stringBox).set_Parent((Container)(object)this);
			stringBox.Label = "Template";
			stringBox.Value = "%i %s [%v ][%b ][%r ]%m %f %o %t";
			stringBox.AllBasicTooltipText = "Templates:\n%i - skill icon\n%v - damage taken/damage healed/barrier applied\n%b - barrier damage taken\n%r - result's associated text, if any (e.g. block, invuln)\n%f - message source\n%t - message target\n%s - skill name\n%n - number of messages (for combined messages)\n%m - mesage source elite spec icon\n%o - mesage target elite spec icon\n\n[col=XXXXXX][/col] tags can be used to override default colors.\n%c can be used in [col] tags to use the source profession's color.\nSquare brackets can be used around templates (e.g. [%b]) to mark them as \"optional\". The content of the brackets will only be shown if the template within is relevant to the current message.\nOptional brackets must contain exactly one template.\n[col] tags only work outside optional brackets (but can have optional brackets inside).";
			_templateBox = stringBox;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Reset Template");
			_resetButton = val2;
			BoolBox boolBox = new BoolBox();
			((Control)boolBox).set_Parent((Container)(object)this);
			boolBox.Label = "Stick to Bottom";
			boolBox.Value = true;
			_stickToBottomBox = boolBox;
			BoolBox boolBox2 = new BoolBox();
			((Control)boolBox2).set_Parent((Container)(object)this);
			boolBox2.Label = "Debug Tooltips";
			boolBox2.Value = false;
			boolBox2.AllBasicTooltipText = "Enable to show debug information when hovering over events, instead of skill tooltips.";
			_debugTooltipsBox = boolBox2;
			GlowButton val3 = new GlowButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Icon(AsyncTexture2D.FromAssetId(157109));
			val3.set_ActiveIcon(AsyncTexture2D.FromAssetId(157110));
			_filterButton = val3;
			MessagesMenu messagesMenu = new MessagesMenu();
			((Control)messagesMenu).set_Parent((Container)(object)this);
			((Panel)messagesMenu).set_CanScroll(true);
			((Panel)messagesMenu).set_Title("Messages");
			messagesMenu.DebugTooltips = _debugTooltipsBox.Value;
			messagesMenu.Scrollbar = ((IEnumerable)((Container)this).get_Children()).OfType<Scrollbar>().First();
			_messagesMenu = messagesMenu;
			UpdateLayout();
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_panelStack.GoBack();
			});
			_templateBox.TempValue += delegate(string value)
			{
				_messagesMenu.Template = value;
			};
			_templateBox.TempClear += delegate
			{
				_messagesMenu.Template = _templateBox.Value;
			};
			_templateBox.ValueCommitted += delegate(string value)
			{
				_messagesMenu.Template = value;
			};
			((Control)_resetButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_templateBox.CommitValue("%i %s [%v ][%b ][%r ]%m %f %o %t");
			});
			_debugTooltipsBox.ValueCommitted += delegate(bool value)
			{
				_messagesMenu.DebugTooltips = value;
			};
			_messagesMenu.Template = _templateBox.Value;
			_messagesMenu.Repopulate(CTextModule.Log);
			_messagesMenu.SetScroll(1f);
			CTextModule.EntryLogged += new Action<LogEntry>(EntryLogged);
			ContextMenuStrip menu = null;
			((Control)_filterButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Expected O, but got Unknown
				ContextMenuStrip obj = menu;
				if (obj != null)
				{
					((Control)obj).Dispose();
				}
				menu = new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)build);
				((Control)menu).add_Hidden((EventHandler<EventArgs>)delegate
				{
					ContextMenuStrip obj2 = menu;
					if (obj2 != null)
					{
						((Control)obj2).Dispose();
					}
				});
				menu.Show((Control)(object)_filterButton);
				IEnumerable<ContextMenuStripItem> build()
				{
					yield return forBool("Unique skills only", () => CTextModule.Settings.LogPanelUniqueSkillsOnly.Value, delegate(bool value)
					{
						CTextModule.Settings.LogPanelUniqueSkillsOnly.Value = value;
						_messagesMenu.Refresh();
					});
					yield return forBool("Show incoming", () => CTextModule.Settings.LogPanelShowIncoming.Value, delegate(bool value)
					{
						CTextModule.Settings.LogPanelShowIncoming.Value = value;
						_messagesMenu.Refresh();
					});
					yield return forBool("Show outgoing", () => CTextModule.Settings.LogPanelShowOutgoing.Value, delegate(bool value)
					{
						CTextModule.Settings.LogPanelShowOutgoing.Value = value;
						_messagesMenu.Refresh();
					});
					static ContextMenuStripItem forBool(string text, Func<bool> get, Action<bool> set)
					{
						//IL_000f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0014: Unknown result type (might be due to invalid IL or missing references)
						//IL_001b: Unknown result type (might be due to invalid IL or missing references)
						//IL_002c: Expected O, but got Unknown
						Action<bool> set2 = set;
						ContextMenuStripItem val4 = new ContextMenuStripItem(text);
						val4.set_CanCheck(true);
						val4.set_Checked(get());
						ContextMenuStripItem item = val4;
						item.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
						{
							set2(item.get_Checked());
						});
						return item;
					}
				}
			});
			_messagesMenu.Filter = delegate(IEnumerable<MessagesMenu.MessageKey> list)
			{
				HashSet<uint> skills = new HashSet<uint>();
				return list.Where((MessagesMenu.MessageKey x) => (!CTextModule.Settings.LogPanelUniqueSkillsOnly.Value || skills.Add(x.Message.Ev.get_SkillId())) && (CTextModule.Settings.LogPanelShowIncoming.Value || !x.Message.IsIn) && (CTextModule.Settings.LogPanelShowOutgoing.Value || !x.Message.IsOut));
			};
		}

		private void EntryLogged(LogEntry entry)
		{
			if (_stickToBottomBox.Value)
			{
				Scrollbar? scrollbar = _messagesMenu.Scrollbar;
				if ((scrollbar != null && scrollbar!.get_ScrollDistance() == 1f) || !_messagesMenu.HasScrollbar())
				{
					_messagesMenu.SetScroll(1f);
					goto IL_0056;
				}
			}
			_messagesMenu.SaveScroll();
			goto IL_0056;
			IL_0056:
			_messagesMenu.PushEntry(entry);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (_backButton != null)
			{
				((Control)_backButton).set_Location(Point.get_Zero());
				((Control)_templateBox).set_Width(300);
				((Control)_stickToBottomBox).set_Width(_stickToBottomBox.ControlRight);
				((Control)_debugTooltipsBox).set_Width(_debugTooltipsBox.ControlRight);
				((Control)(object)_backButton).ArrangeLeftRight(10, (Control)_templateBox, (Control)_resetButton, (Control)_stickToBottomBox, (Control)_debugTooltipsBox);
				((Control)(object)_templateBox).MiddleWith((Control)(object)_backButton);
				((Control)(object)_stickToBottomBox).MiddleWith((Control)(object)_backButton);
				((Control)(object)_debugTooltipsBox).MiddleWith((Control)(object)_backButton);
				((Control)(object)_filterButton).MiddleWith((Control)(object)_backButton);
				((Control)(object)_filterButton).AlignRight();
				GlowButton filterButton = _filterButton;
				((Control)filterButton).set_Left(((Control)filterButton).get_Left() - 10);
				((Control)(object)_backButton).ArrangeTopDown(10, (Control)_messagesMenu);
				((Control)(object)_messagesMenu).WidthFillRight(10);
				((Control)(object)_messagesMenu).HeightFillDown(10);
			}
		}

		protected override void DisposeControl()
		{
			CTextModule.EntryLogged -= new Action<LogEntry>(EntryLogged);
			((Panel)this).DisposeControl();
		}
	}
}
