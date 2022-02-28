using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Glide;
using Gw2Sharp.ChatLinks;
using KillProofModule.Models;
using KillProofModule.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KillProofModule.Controls
{
	public class SmartPingMenu : IDisposable
	{
		private DateTimeOffset _smartPingCooldownSend = DateTimeOffset.Now;

		private DateTimeOffset _smartPingHotButtonTimeSend = DateTimeOffset.Now;

		private int _smartPingRepetitions;

		private int _smartPingCurrentReduction;

		private int _smartPingCurrentValue;

		private int _smartPingCurrentRepetitions;

		private Panel _smartPingMenu;

		private const uint KEY_PRESSED = 32768u;

		private const uint VK_LCONTROL = 162u;

		private const uint VK_LSHIFT = 160u;

		private bool IsUiAvailable()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_IsInGame())
			{
				return !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			return false;
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			ToggleSmartPingMenu(!e.get_Value(), 0.45f);
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			ToggleSmartPingMenu(e.get_Value(), 0.1f);
		}

		private void OnSmartPingMenuEnabledSettingChanged(object o, ValueChangedEventArgs<bool> e)
		{
			ToggleSmartPingMenu(e.get_NewValue(), 0.1f);
		}

		private void OnSPM_RepetitionsChanged(object o, ValueChangedEventArgs<int> e)
		{
			_smartPingRepetitions = MathHelper.Clamp(e.get_NewValue(), 10, 100) / 10;
		}

		private void OnSelfKillProofChanged(object o, ValueEventArgs<KillProof> e)
		{
			BuildSmartPingMenu();
		}

		[DllImport("USER32.dll")]
		private static extern short GetKeyState(uint vk);

		private bool IsPressed(uint key)
		{
			return Convert.ToBoolean((long)GetKeyState(key) & 0x8000L);
		}

		public SmartPingMenu()
		{
			KillProofModule.ModuleInstance.SmartPingMenuEnabled.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSmartPingMenuEnabledSettingChanged);
			OnSPM_RepetitionsChanged(KillProofModule.ModuleInstance.SPM_Repetitions, new ValueChangedEventArgs<int>(0, KillProofModule.ModuleInstance.SPM_Repetitions.get_Value()));
			KillProofModule.ModuleInstance.SPM_Repetitions.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSPM_RepetitionsChanged);
			GameService.GameIntegration.add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			KillProofModule.ModuleInstance.PartyManager.Self.KillProofChanged += OnSelfKillProofChanged;
		}

		private void ToggleSmartPingMenu(bool enabled, float tDuration)
		{
			if (enabled)
			{
				BuildSmartPingMenu();
			}
			else
			{
				if (_smartPingMenu == null)
				{
					return;
				}
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(_smartPingMenu, (object)new
				{
					Opacity = 0f
				}, tDuration, 0f, true).OnComplete((Action)delegate
				{
					Panel smartPingMenu = _smartPingMenu;
					if (smartPingMenu != null)
					{
						((Control)smartPingMenu).Dispose();
					}
				});
			}
		}

		private void DoSmartPing(Token token)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			ItemChatLink val = new ItemChatLink();
			val.set_ItemId(token.Id);
			ItemChatLink chatLink = val;
			int totalAmount = KillProofModule.ModuleInstance.PartyManager.Self.KillProof.GetToken(token.Id)?.Amount ?? 0;
			if (totalAmount <= 250)
			{
				chatLink.set_Quantity(Convert.ToByte(totalAmount));
				GameService.GameIntegration.get_Chat().Send(((object)chatLink).ToString());
				return;
			}
			if (DateTimeOffset.Now.Subtract(_smartPingHotButtonTimeSend).TotalMilliseconds > 500.0)
			{
				_smartPingCurrentReduction = 0;
				_smartPingCurrentValue = 0;
				_smartPingCurrentRepetitions = 0;
			}
			int rest = totalAmount - _smartPingCurrentValue % totalAmount;
			if (rest > 250)
			{
				int tempAmount = 250 - _smartPingCurrentReduction;
				if (_smartPingCurrentRepetitions < _smartPingRepetitions)
				{
					_smartPingCurrentRepetitions++;
				}
				else
				{
					_smartPingCurrentValue += tempAmount;
					_smartPingCurrentReduction++;
					_smartPingCurrentRepetitions = 0;
				}
				chatLink.set_Quantity(Convert.ToByte(tempAmount));
			}
			else
			{
				chatLink.set_Quantity(Convert.ToByte(rest));
				if (_smartPingCurrentRepetitions < _smartPingRepetitions)
				{
					_smartPingCurrentRepetitions++;
				}
				else
				{
					_smartPingCurrentReduction = 0;
					_smartPingCurrentValue = 0;
					_smartPingCurrentRepetitions = 0;
					_smartPingCooldownSend = DateTimeOffset.Now;
				}
			}
			GameService.GameIntegration.get_Chat().Send(((object)chatLink).ToString());
			_smartPingHotButtonTimeSend = DateTimeOffset.Now;
		}

		private void BuildSmartPingMenu()
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Expected O, but got Unknown
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Expected O, but got Unknown
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Expected O, but got Unknown
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Expected O, but got Unknown
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Expected O, but got Unknown
			Panel smartPingMenu = _smartPingMenu;
			if (smartPingMenu != null)
			{
				((Control)smartPingMenu).Dispose();
			}
			if (!KillProofModule.ModuleInstance.SmartPingMenuEnabled.get_Value() || !IsUiAvailable() || !KillProofModule.ModuleInstance.PartyManager.Self.HasKillProof())
			{
				return;
			}
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Location(new Point(10, 38));
			((Control)val).set_Size(new Point(400, 40));
			((Control)val).set_Opacity(0f);
			val.set_ShowBorder(true);
			_smartPingMenu = val;
			((Control)_smartPingMenu).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				((Control)_smartPingMenu).set_Location(new Point(10, 38));
			});
			((Control)_smartPingMenu).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(_smartPingMenu, (object)new
				{
					Opacity = 1f
				}, 0.45f, 0f, true);
			});
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_smartPingMenu);
			((Control)val2).set_Size(((Control)_smartPingMenu).get_Size());
			val2.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)20, (FontStyle)0));
			val2.set_Text("[");
			((Control)val2).set_Location(new Point(0, 0));
			val2.set_HorizontalAlignment((HorizontalAlignment)0);
			val2.set_VerticalAlignment((VerticalAlignment)0);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_smartPingMenu);
			((Control)val3).set_Size(new Point(30, 30));
			((Control)val3).set_Location(new Point(10, -2));
			Label quantity = val3;
			Dropdown val4 = new Dropdown();
			((Control)val4).set_Parent((Container)(object)_smartPingMenu);
			((Control)val4).set_Size(new Point(260, 20));
			((Control)val4).set_Location(new Point(((Control)quantity).get_Right() + 2, 3));
			val4.set_SelectedItem(global::KillProofModule.Properties.Resources.Loading___);
			Dropdown dropdown = val4;
			((Control)_smartPingMenu).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(_smartPingMenu, (object)new
				{
					Opacity = 0.4f
				}, 0.45f, 0f, true);
			});
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)_smartPingMenu);
			((Control)val5).set_Size(new Point(10, ((Control)_smartPingMenu).get_Height()));
			val5.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)20, (FontStyle)0));
			val5.set_Text("]");
			((Control)val5).set_Location(new Point(((Control)dropdown).get_Right(), 0));
			val5.set_HorizontalAlignment((HorizontalAlignment)0);
			val5.set_VerticalAlignment((VerticalAlignment)0);
			Label rightBracket = val5;
			List<string> tokenStringSorted = new List<string>();
			foreach (Token token in KillProofModule.ModuleInstance.PartyManager.Self.KillProof.GetAllTokens())
			{
				Wing wing = KillProofModule.ModuleInstance.Resources.GetWing(token);
				if (wing != null)
				{
					tokenStringSorted.Add($"W{KillProofModule.ModuleInstance.Resources.GetAllWings().ToList().IndexOf(wing) + 1} | {token.Name}");
				}
				else
				{
					tokenStringSorted.Add(token.Name);
				}
			}
			tokenStringSorted.Sort((string e1, string e2) => string.Compare(e1, e2, StringComparison.InvariantCultureIgnoreCase));
			foreach (string tokenString in tokenStringSorted)
			{
				dropdown.get_Items().Add(tokenString);
			}
			dropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object o, ValueChangedEventArgs e)
			{
				quantity.set_Text(KillProofModule.ModuleInstance.PartyManager.Self.KillProof?.GetToken(e.get_CurrentValue())?.Amount.ToString() ?? "");
				KillProofModule.ModuleInstance.SPM_DropdownSelection.set_Value(e.get_CurrentValue());
			});
			string oldSelection = dropdown.get_Items().FirstOrDefault((string x) => x.Equals(KillProofModule.ModuleInstance.SPM_DropdownSelection.get_Value(), StringComparison.InvariantCultureIgnoreCase));
			dropdown.set_SelectedItem(oldSelection ?? ((dropdown.get_Items().Count > 0) ? dropdown.get_Items()[0] : ""));
			Image val6 = new Image();
			((Control)val6).set_Parent((Container)(object)_smartPingMenu);
			((Control)val6).set_Size(new Point(24, 24));
			((Control)val6).set_Location(new Point(((Control)rightBracket).get_Right() + 1, 0));
			val6.set_Texture(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("784268")));
			val6.set_SpriteEffects((SpriteEffects)1);
			((Control)val6).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Send_To_Chat_nLeft_Click__Only_send_code_up_to_a_stack_s_worth__250x___nLeft_Ctrl___Left_Click__Send_killproof_me_total_amount_);
			Image sendButton = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)_smartPingMenu);
			((Control)val7).set_Size(new Point(29, 24));
			((Control)val7).set_Location(new Point(((Control)sendButton).get_Right() + 7, 0));
			val7.set_Text(KillProofModule.ModuleInstance.SPM_WingSelection.get_Value());
			((Control)val7).set_BackgroundColor(Color.get_Gray());
			((Control)val7).set_BasicTooltipText(global::KillProofModule.Properties.Resources.Random_token_from_selected_wing_when_pressing_Send_To_Chat__nLeft_Click__Toggle_nLeft_Ctrl___Left_Click__Iterate_wings);
			StandardButton randomizeButton = val7;
			((Control)randomizeButton).add_PropertyChanged((PropertyChangedEventHandler)delegate(object o, PropertyChangedEventArgs e)
			{
				if (e.PropertyName.Equals("Text"))
				{
					KillProofModule.ModuleInstance.SPM_WingSelection.set_Value(randomizeButton.get_Text());
				}
			});
			((Control)randomizeButton).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				((Control)randomizeButton).set_Size(new Point(27, 22));
				((Control)randomizeButton).set_Location(new Point(((Control)sendButton).get_Right() + 5, 2));
			});
			((Control)randomizeButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				((Control)randomizeButton).set_Size(new Point(29, 24));
				((Control)randomizeButton).set_Location(new Point(((Control)sendButton).get_Right() + 7, 0));
			});
			((Control)randomizeButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				((Control)randomizeButton).set_Size(new Point(29, 24));
				((Control)randomizeButton).set_Location(new Point(((Control)sendButton).get_Right() + 7, 0));
			});
			((Control)randomizeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				if (IsPressed(162u))
				{
					List<Wing> list3 = KillProofModule.ModuleInstance.Resources.GetAllWings().ToList();
					Wing wing4 = KillProofModule.ModuleInstance.Resources.GetWing(randomizeButton.get_Text());
					int num = list3.IndexOf(wing4) + 1;
					int num2 = ((num + 1 > list3.Count()) ? 1 : (num + 1));
					randomizeButton.set_Text($"W{num2}");
				}
				else
				{
					((Control)randomizeButton).set_BackgroundColor((((Control)randomizeButton).get_BackgroundColor() == Color.get_Gray()) ? Color.get_LightGreen() : Color.get_Gray());
				}
			});
			((Control)sendButton).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				((Control)sendButton).set_Size(new Point(22, 22));
				((Control)sendButton).set_Location(new Point(((Control)rightBracket).get_Right() + 3, 2));
			});
			((Control)sendButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				((Control)sendButton).set_Size(new Point(24, 24));
				((Control)sendButton).set_Location(new Point(((Control)rightBracket).get_Right() + 1, 0));
			});
			((Control)sendButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				((Control)sendButton).set_Size(new Point(24, 24));
				((Control)sendButton).set_Location(new Point(((Control)rightBracket).get_Right() + 1, 0));
			});
			Dictionary<int, DateTimeOffset> timeOutRightSend = new Dictionary<int, DateTimeOffset>();
			((Control)sendButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Expected O, but got Unknown
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0568: Unknown result type (might be due to invalid IL or missing references)
				//IL_056d: Unknown result type (might be due to invalid IL or missing references)
				if (IsPressed(162u))
				{
					ItemChatLink chatLink = new ItemChatLink();
					if (((Control)randomizeButton).get_BackgroundColor() == Color.get_LightGreen())
					{
						Wing wing2 = KillProofModule.ModuleInstance.Resources.GetWing(randomizeButton.get_Text());
						IEnumerable<Token> wingTokens2 = wing2.GetTokens();
						List<Token> list = (from x in KillProofModule.ModuleInstance.PartyManager.Self.KillProof.GetAllTokens()
							where wingTokens2.Any((Token y) => y.Id.Equals(x.Id))
							select x).ToList();
						if (list.Count != 0)
						{
							Token token2 = list.ElementAt(RandomUtil.GetRandom(0, list.Count - 1));
							chatLink.set_ItemId(token2.Id);
							if (timeOutRightSend.Any((KeyValuePair<int, DateTimeOffset> x) => x.Key == chatLink.get_ItemId()))
							{
								TimeSpan timeSpan = DateTimeOffset.Now.Subtract(timeOutRightSend[chatLink.get_ItemId()]);
								if (timeSpan.TotalMinutes < 2.0)
								{
									TimeSpan timeSpan2 = TimeSpan.FromMinutes(2.0 - timeSpan.TotalMinutes);
									string text = ((timeSpan2.TotalSeconds > 119.0) ? $" {timeSpan2:%m} minutes and" : ((timeSpan2.TotalSeconds > 59.0) ? $" {timeSpan2:%m} minute and" : ""));
									string text2 = ((timeSpan2.Seconds > 9) ? $"{timeSpan2:ss} seconds" : ((timeSpan2.Seconds > 1) ? $"{timeSpan2:%s} seconds" : $"{timeSpan2:%s} second"));
									ScreenNotification.ShowNotification("You can't send your " + token2.Name + " total\nwithin the next" + text + " " + text2 + " again.", (NotificationType)2, (Texture2D)null, 4);
									return;
								}
								timeOutRightSend[chatLink.get_ItemId()] = DateTimeOffset.Now;
							}
							else
							{
								timeOutRightSend.Add(chatLink.get_ItemId(), DateTimeOffset.Now);
							}
							chatLink.set_Quantity(Convert.ToByte(1));
							GameService.GameIntegration.get_Chat().Send($"Total: {KillProofModule.ModuleInstance.PartyManager.Self.KillProof.GetToken(token2.Id)?.Amount ?? 0} of {chatLink} (killproof.me/{KillProofModule.ModuleInstance.PartyManager.Self.KillProof.KpId})");
						}
					}
					else
					{
						Token token3 = KillProofModule.ModuleInstance.PartyManager.Self.KillProof.GetToken(dropdown.get_SelectedItem());
						if (token3 != null)
						{
							chatLink.set_ItemId(token3.Id);
							if (timeOutRightSend.Any((KeyValuePair<int, DateTimeOffset> x) => x.Key == chatLink.get_ItemId()))
							{
								TimeSpan timeSpan3 = DateTimeOffset.Now.Subtract(timeOutRightSend[chatLink.get_ItemId()]);
								if (timeSpan3.TotalMinutes < 2.0)
								{
									TimeSpan timeSpan4 = TimeSpan.FromMinutes(2.0 - timeSpan3.TotalMinutes);
									string text3 = ((timeSpan4.TotalSeconds > 119.0) ? $" {timeSpan4:%m} minutes and" : ((timeSpan4.TotalSeconds > 59.0) ? $" {timeSpan4:%m} minute and" : ""));
									string text4 = ((timeSpan4.Seconds > 9) ? $"{timeSpan4:ss} seconds" : ((timeSpan4.Seconds > 1) ? $"{timeSpan4:%s} seconds" : $"{timeSpan4:%s} second"));
									ScreenNotification.ShowNotification("You can't send your " + token3.Name + " total\nwithin the next" + text3 + " " + text4 + " again.", (NotificationType)2, (Texture2D)null, 4);
									return;
								}
								timeOutRightSend[chatLink.get_ItemId()] = DateTimeOffset.Now;
							}
							else
							{
								timeOutRightSend.Add(chatLink.get_ItemId(), DateTimeOffset.Now);
							}
							chatLink.set_Quantity(Convert.ToByte(1));
							GameService.GameIntegration.get_Chat().Send(global::KillProofModule.Properties.Resources.Total___0__of__1___killproof_me__2__.Replace("{0}", token3.Amount.ToString()).Replace("{1}", ((object)chatLink).ToString()).Replace("{2}", KillProofModule.ModuleInstance.PartyManager.Self.KpId));
						}
					}
				}
				else if (!GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
				{
					if (DateTimeOffset.Now.Subtract(_smartPingCooldownSend).TotalSeconds < 1.0)
					{
						ScreenNotification.ShowNotification("Your total has been reached. Cooling down.", (NotificationType)2, (Texture2D)null, 4);
					}
					else if (((Control)randomizeButton).get_BackgroundColor() == Color.get_LightGreen())
					{
						Wing wing3 = KillProofModule.ModuleInstance.Resources.GetWing(randomizeButton.get_Text());
						IEnumerable<Token> wingTokens = wing3.GetTokens();
						List<Token> list2 = (from x in KillProofModule.ModuleInstance.PartyManager.Self.KillProof.GetAllTokens()
							where wingTokens.Any((Token y) => y.Id.Equals(x.Id))
							select x).ToList();
						if (list2.Count != 0)
						{
							DoSmartPing(list2.ElementAt(RandomUtil.GetRandom(0, list2.Count - 1)));
						}
					}
					else
					{
						Token token4 = KillProofModule.ModuleInstance.PartyManager.Self.KillProof.GetToken(dropdown.get_SelectedItem());
						if (token4 != null)
						{
							DoSmartPing(token4);
						}
					}
				}
			});
			((Control)_smartPingMenu).add_Disposed((EventHandler<EventArgs>)delegate
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(_smartPingMenu, (object)new
				{
					Opacity = 0f
				}, 0.2f, 0f, true);
			});
			quantity.set_Text(KillProofModule.ModuleInstance.PartyManager.Self.KillProof.GetToken(dropdown.get_SelectedItem())?.Amount.ToString() ?? "0");
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(_smartPingMenu, (object)new
			{
				Opacity = 0.4f
			}, 0.35f, 0f, true);
		}

		public void Dispose()
		{
			Panel smartPingMenu = _smartPingMenu;
			if (smartPingMenu != null)
			{
				((Control)smartPingMenu).Dispose();
			}
			KillProofModule.ModuleInstance.SmartPingMenuEnabled.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnSmartPingMenuEnabledSettingChanged);
			KillProofModule.ModuleInstance.SPM_Repetitions.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnSPM_RepetitionsChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			KillProofModule.ModuleInstance.PartyManager.Self.KillProofChanged -= OnSelfKillProofChanged;
		}
	}
}
