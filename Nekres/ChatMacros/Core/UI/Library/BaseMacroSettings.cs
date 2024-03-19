using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ChatMacros.Core.Services.Data;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.UI.Library
{
	internal class BaseMacroSettings : View
	{
		private class ItemEntry<T> : View
		{
			private readonly T _item;

			private Func<T, string> _displayString;

			private Func<T, string> _basicTooltipText;

			public event EventHandler<EventArgs> Remove;

			public ItemEntry(T item, Func<T, string> displayString, Func<T, string> basicTooltipText)
				: this()
			{
				_item = item;
				_displayString = displayString;
				_basicTooltipText = basicTooltipText;
			}

			protected override void Build(Container buildPanel)
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Expected O, but got Unknown
				//IL_00af: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0108: Expected O, but got Unknown
				Label val = new Label();
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_BasicTooltipText(_basicTooltipText(_item));
				val.set_Text(AssetUtil.Truncate(_displayString(_item), buildPanel.get_ContentRegion().Width - 60, GameService.Content.get_DefaultFont14()));
				val.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val).set_Width(buildPanel.get_ContentRegion().Width - 47);
				((Control)val).set_Height(24);
				val.set_VerticalAlignment((VerticalAlignment)1);
				Label title = val;
				Image val2 = new Image();
				((Control)val2).set_Parent(buildPanel);
				((Control)val2).set_Width(24);
				((Control)val2).set_Height(24);
				((Control)val2).set_Left(((Control)title).get_Right() + 3);
				val2.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175782));
				((Control)val2).set_BasicTooltipText(Resources.Remove);
				Image delete = val2;
				((Control)delete).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					delete.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175784));
				});
				((Control)delete).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					delete.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175782));
				});
				((Control)delete).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					delete.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175783));
				});
				((Control)delete).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					this.Remove?.Invoke(this, EventArgs.Empty);
					((Control)buildPanel).Dispose();
				});
				((View<IPresenter>)this).Build(buildPanel);
			}
		}

		private BaseMacro _macro;

		private Func<bool> _upsert;

		public BaseMacroSettings(BaseMacro macro, Func<bool> upsert)
			: this()
		{
			_macro = macro;
			_upsert = upsert;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Expected O, but got Unknown
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Expected O, but got Unknown
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Expected O, but got Unknown
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Expected O, but got Unknown
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Expected O, but got Unknown
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Expected O, but got Unknown
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			Rectangle contentRegion = default(Rectangle);
			((Rectangle)(ref contentRegion))._002Ector(4, 7, buildPanel.get_ContentRegion().Width - 8, buildPanel.get_ContentRegion().Height);
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(contentRegion.Width / 2 - 2);
			((Control)val).set_Height(contentRegion.Height - 7 - 16);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_Title(Resources.Active_Areas);
			FlowPanel activeMapsWrap = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)activeMapsWrap);
			((Control)val2).set_Width(((Container)activeMapsWrap).get_ContentRegion().Width);
			((Control)val2).set_Height(((Container)activeMapsWrap).get_ContentRegion().Height - 30 - 7);
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_ControlPadding(new Vector2(0f, 4f));
			val2.set_OuterControlPadding(new Vector2(4f, 7f));
			((Panel)val2).set_CanScroll(true);
			((Panel)val2).set_ShowBorder(true);
			FlowPanel activeMapsPanel = val2;
			foreach (int id in _macro.MapIds)
			{
				Map map2 = ChatMacros.Instance.Macro.AllMaps.FirstOrDefault((Map map) => map.get_Id() == id);
				if (map2 != null)
				{
					AddActiveMap((Container)(object)activeMapsPanel, map2);
				}
			}
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)activeMapsWrap);
			((Control)val3).set_Width(((Container)activeMapsWrap).get_ContentRegion().Width - 2);
			((Control)val3).set_Height(30);
			((Control)val3).set_BasicTooltipText(Resources.Add_Map___ + "\n" + string.Format(Resources.Current_Map_ID___0_, GameService.Gw2Mumble.get_CurrentMap().get_Id()));
			((TextInputBase)val3).set_PlaceholderText(Resources.Add_Map___);
			TextBox addActiveMap = val3;
			((TextInputBase)addActiveMap).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object _, ValueEventArgs<bool> e)
			{
				if (!e.get_Value())
				{
					if (string.IsNullOrWhiteSpace(((TextInputBase)addActiveMap).get_Text()))
					{
						((TextInputBase)addActiveMap).set_Text(string.Empty);
					}
					else
					{
						Map val10;
						if (int.TryParse(((TextInputBase)addActiveMap).get_Text().Trim(), out var mapId))
						{
							val10 = ChatMacros.Instance.Macro.AllMaps?.FirstOrDefault((Map x) => x.get_Id() == mapId);
							if (val10 == null)
							{
								ScreenNotification.ShowNotification(string.Format(Resources._0__does_not_exist_, string.Format(Resources.Map_ID__0_, mapId)), (NotificationType)1, (Texture2D)null, 4);
								((TextInputBase)addActiveMap).set_Text(string.Empty);
								return;
							}
						}
						else
						{
							val10 = FastenshteinUtil.FindClosestMatchBy(((TextInputBase)addActiveMap).get_Text().Trim(), ChatMacros.Instance.Macro.AllMaps, (Map map) => map.get_Name());
							if (val10 == null)
							{
								ScreenNotification.ShowNotification(string.Format(Resources._0__not_found__Check_your_spelling_, "\"" + ((TextInputBase)addActiveMap).get_Text() + "\""), (NotificationType)1, (Texture2D)null, 4);
								((TextInputBase)addActiveMap).set_Text(string.Empty);
								return;
							}
						}
						((TextInputBase)addActiveMap).set_Text(string.Empty);
						List<int> list = _macro.MapIds.ToList();
						if (list.Contains(val10.get_Id()))
						{
							ScreenNotification.ShowNotification(string.Format(Resources._0__already_added_, "\"" + val10.get_Name() + "\""), (NotificationType)0, (Texture2D)null, 4);
						}
						else
						{
							_macro.MapIds.Add(val10.get_Id());
							if (!_upsert())
							{
								_macro.MapIds = list;
								ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
								GameService.Content.PlaySoundEffectByName("error");
							}
							else
							{
								AddActiveMap((Container)(object)activeMapsPanel, val10);
								ChatMacros.Instance.Macro.UpdateMacros();
							}
						}
					}
				}
			});
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Width(((Control)activeMapsWrap).get_Width());
			((Control)val4).set_Height(16);
			((Control)val4).set_Top(((Control)activeMapsWrap).get_Bottom());
			val4.set_FlowDirection((ControlFlowDirection)2);
			val4.set_ControlPadding(new Vector2(20f, 0f));
			val4.set_OuterControlPadding(new Vector2(4f, 0f));
			FlowPanel activeGameModes = val4;
			foreach (GameMode mode in Enum.GetValues(typeof(GameMode)).Cast<GameMode>().Skip(1))
			{
				Checkbox val5 = new Checkbox();
				((Control)val5).set_Parent((Container)(object)activeGameModes);
				((Control)val5).set_Width(50);
				((Control)val5).set_Height(((Container)activeGameModes).get_ContentRegion().Height);
				val5.set_Checked(_macro.HasGameMode(mode));
				val5.set_Text(mode.ToShortDisplayString());
				((Control)val5).set_BasicTooltipText(mode.ToDisplayString());
				Checkbox cb = val5;
				cb.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
				{
					if ((_macro.GameModes & ~mode) == 0)
					{
						cb.GetPrivateField("_checked").SetValue(cb, !e.get_Checked());
						GameService.Content.PlaySoundEffectByName("error");
					}
					else
					{
						GameMode gameModes = _macro.GameModes;
						if (e.get_Checked())
						{
							_macro.GameModes |= mode;
						}
						else
						{
							_macro.GameModes &= ~mode;
						}
						if (!_upsert())
						{
							_macro.GameModes = gameModes;
							cb.GetPrivateField("_checked").SetValue(cb, !e.get_Checked());
							ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
							GameService.Content.PlaySoundEffectByName("error");
						}
						else
						{
							ChatMacros.Instance.Macro.UpdateMacros();
							GameService.Content.PlaySoundEffectByName("color-change");
						}
					}
				});
			}
			FlowPanel val6 = new FlowPanel();
			((Control)val6).set_Parent(buildPanel);
			((Control)val6).set_Left(((Control)activeMapsWrap).get_Right() + 8);
			((Control)val6).set_Width(contentRegion.Width / 2 - 2);
			((Control)val6).set_Height(contentRegion.Height - 7 - 16);
			val6.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val6).set_Title(Resources.Trigger_Options);
			FlowPanel voiceCommandsWrap = val6;
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)voiceCommandsWrap);
			((Control)val7).set_Width(((Container)voiceCommandsWrap).get_ContentRegion().Width);
			((Control)val7).set_Height(((Container)voiceCommandsWrap).get_ContentRegion().Height - 30 - 7);
			val7.set_FlowDirection((ControlFlowDirection)3);
			val7.set_ControlPadding(new Vector2(0f, 4f));
			val7.set_OuterControlPadding(new Vector2(4f, 7f));
			((Panel)val7).set_CanScroll(true);
			((Panel)val7).set_ShowBorder(true);
			FlowPanel commandsPanel = val7;
			TextBox val8 = new TextBox();
			((Control)val8).set_Parent((Container)(object)voiceCommandsWrap);
			((Control)val8).set_Width(((Container)voiceCommandsWrap).get_ContentRegion().Width);
			((Control)val8).set_Height(30);
			((Control)val8).set_BasicTooltipText(Resources.Add_Voice_Command___);
			((TextInputBase)val8).set_PlaceholderText(Resources.Add_Voice_Command___);
			TextBox addVoiceCmd = val8;
			foreach (string cmd in _macro.VoiceCommands)
			{
				AddVoiceCommand((Container)(object)commandsPanel, cmd);
			}
			((TextInputBase)addVoiceCmd).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object _, ValueEventArgs<bool> e)
			{
				if (!e.get_Value())
				{
					if (string.IsNullOrWhiteSpace(((TextInputBase)addVoiceCmd).get_Text()))
					{
						((TextInputBase)addVoiceCmd).set_Text(string.Empty);
					}
					else
					{
						string[] array = ((TextInputBase)addVoiceCmd).get_Text().Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Except(_macro.VoiceCommands, StringComparer.InvariantCultureIgnoreCase)
							.ToArray();
						List<string> voiceCommands = _macro.VoiceCommands.ToList();
						_macro.VoiceCommands.AddRange(array);
						((TextInputBase)addVoiceCmd).set_Text(string.Empty);
						if (!_upsert())
						{
							_macro.VoiceCommands = voiceCommands;
							ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
							GameService.Content.PlaySoundEffectByName("error");
						}
						else
						{
							string[] array2 = array;
							foreach (string command in array2)
							{
								AddVoiceCommand((Container)(object)commandsPanel, command);
							}
							ChatMacros.Instance.Speech.UpdateGrammar();
						}
					}
				}
			});
			KeybindingAssigner val9 = new KeybindingAssigner(_macro.KeyBinding);
			((Control)val9).set_Parent(buildPanel);
			val9.set_KeyBindingName(Resources.Non_Voice_Trigger);
			((Control)val9).set_Width(((Control)voiceCommandsWrap).get_Width() - 8);
			((Control)val9).set_Top(((Control)voiceCommandsWrap).get_Bottom());
			((Control)val9).set_Left(((Control)voiceCommandsWrap).get_Left() + 4);
			val9.set_NameWidth(((Control)val9).get_Width() / 2);
			val9.add_BindingChanged((EventHandler<EventArgs>)delegate
			{
				_upsert();
				ChatMacros.Instance.Macro.UpdateMacros();
			});
			((View<IPresenter>)this).Build(buildPanel);
		}

		private void AddVoiceCommand(Container parent, string command)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(parent);
			((Control)val).set_Width(parent.get_ContentRegion().Width);
			((Control)val).set_Height(32);
			ItemEntry<string> view = new ItemEntry<string>(command, (string x) => x, (string x) => x);
			view.Remove += delegate
			{
				_macro.VoiceCommands.Remove(command);
				if (!_upsert())
				{
					_macro.VoiceCommands.Add(command);
					ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
					GameService.Content.PlaySoundEffectByName("error");
				}
				else
				{
					ChatMacros.Instance.Speech.UpdateGrammar();
				}
			};
			val.Show((IView)(object)view);
		}

		private void AddActiveMap(Container parent, Map map)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent(parent);
			((Control)val).set_Width(parent.get_ContentRegion().Width);
			((Control)val).set_Height(32);
			((Control)val).set_BasicTooltipText(string.Format(Resources.Map_ID__0_, map.get_Id()));
			ItemEntry<Map> view = new ItemEntry<Map>(map, (Map x) => x.get_Name(), (Map x) => $"{x.get_Name()} ({x.get_Id()})");
			view.Remove += delegate
			{
				_macro.MapIds.Remove(map.get_Id());
				if (!_upsert())
				{
					_macro.MapIds.Add(map.get_Id());
					ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
					GameService.Content.PlaySoundEffectByName("error");
				}
				else
				{
					ChatMacros.Instance.Macro.UpdateMacros();
				}
			};
			val.Show((IView)(object)view);
		}
	}
}
