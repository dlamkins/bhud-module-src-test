using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Gw2Sharp.ChatLinks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;
using Nekres.ProofLogix.Core.UI.Configs;

namespace Nekres.ProofLogix.Core.UI.SmartPing
{
	public class SmartPingView : View
	{
		private SmartPingConfig _config;

		private AsyncTexture2D _cogWheelIcon;

		private AsyncTexture2D _cogWheelIconHover;

		private AsyncTexture2D _cogWheelIconClick;

		public SmartPingView(SmartPingConfig config)
			: this()
		{
			_config = config;
			_cogWheelIcon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155052);
			_cogWheelIconHover = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(157110);
			_cogWheelIconClick = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(157109);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Expected O, but got Unknown
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Expected O, but got Unknown
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Expected O, but got Unknown
			//IL_0385: Expected O, but got Unknown
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Expected O, but got Unknown
			//IL_03e9: Expected O, but got Unknown
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Expected O, but got Unknown
			//IL_044f: Expected O, but got Unknown
			//IL_047c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Expected O, but got Unknown
			//IL_049b: Expected O, but got Unknown
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0515: Expected O, but got Unknown
			//IL_0517: Expected O, but got Unknown
			Image val = new Image(_cogWheelIcon);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(32);
			((Control)val).set_Height(32);
			((Control)val).set_Top((buildPanel.get_ContentRegion().Height - 32) / 2);
			Image cogWheel = val;
			((Control)cogWheel).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				cogWheel.set_Texture(_cogWheelIconHover);
			});
			((Control)cogWheel).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				cogWheel.set_Texture(_cogWheelIcon);
			});
			((Control)cogWheel).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				cogWheel.set_Texture(_cogWheelIconClick);
			});
			((Control)cogWheel).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				cogWheel.set_Texture(_cogWheelIconHover);
			});
			Panel val2 = new Panel();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Width(buildPanel.get_ContentRegion().Width - 64 - 4 - 4);
			((Control)val2).set_Left(((Control)cogWheel).get_Right() + 4);
			((Control)val2).set_Height(buildPanel.get_ContentRegion().Height);
			val2.set_ShowBorder(true);
			Panel labelPanel = val2;
			Token token = ProofLogix.Instance.PartySync.LocalPlayer.KpProfile.GetToken(_config.SelectedToken);
			Color rarity = ProofLogix.Instance.Resources.GetItem(_config.SelectedToken).Rarity.AsColor();
			Label lbl = BuildItemLabel(AssetUtil.GetItemDisplayName(token.Name, token.Amount), rarity);
			((Control)lbl).set_Parent((Container)(object)labelPanel);
			((Control)lbl).set_Top((((Container)labelPanel).get_ContentRegion().Height - ((Control)lbl).get_Height()) / 2);
			((Control)lbl).set_Left((((Container)labelPanel).get_ContentRegion().Width - ((Control)lbl).get_Width()) / 2);
			Image val3 = new Image(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(784268));
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Width(32);
			((Control)val3).set_Height(32);
			((Control)val3).set_Left(((Control)labelPanel).get_Right() + 4);
			((Control)val3).set_Top(((Control)labelPanel).get_Top() + (((Control)labelPanel).get_Height() - 32) / 2);
			val3.set_SpriteEffects((SpriteEffects)1);
			val3.set_Tint(Color.get_White() * 0.8f);
			((Control)val3).set_BasicTooltipText("Send to Chat\nMouse 1: Proportionately each time until total is reached.\nMouse 2: Total with singular item.");
			Image sendBttn = val3;
			((Control)sendBttn).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				sendBttn.set_Tint(Color.get_White());
			});
			((Control)sendBttn).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				sendBttn.set_Tint(Color.get_White() * 0.8f);
			});
			((Control)sendBttn).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				sendBttn.set_Tint(Color.get_White() * 0.85f);
				((Control)sendBttn).set_Size(new Point(31, 31));
				((Control)sendBttn).set_Left(((Control)labelPanel).get_Right() + 4);
				((Control)sendBttn).set_Top(((Control)labelPanel).get_Top() + (((Control)labelPanel).get_Height() - ((Control)sendBttn).get_Height()) / 2);
			});
			((Control)sendBttn).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				sendBttn.set_Tint(Color.get_White());
				((Control)sendBttn).set_Size(new Point(32, 32));
				((Control)sendBttn).set_Left(((Control)labelPanel).get_Right() + 4);
				((Control)sendBttn).set_Top(((Control)labelPanel).get_Top() + (((Control)labelPanel).get_Height() - ((Control)sendBttn).get_Height()) / 2);
			});
			DateTime lastTotalReachedTime = DateTime.UtcNow;
			DateTime lastSendTime = DateTime.UtcNow;
			int currentReduction = 0;
			int currentValue = 0;
			int currentRepetitions = 0;
			bool busy = false;
			((Control)sendBttn).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				//IL_0119: Expected O, but got Unknown
				if (!busy)
				{
					busy = true;
					ProofLogix.Instance.Resources.PlayMenuItemClick();
					int amount2 = ProofLogix.Instance.PartySync.LocalPlayer.KpProfile.GetToken(_config.SelectedToken).Amount;
					if (!CanSend(amount2, lastTotalReachedTime))
					{
						GameService.Content.PlaySoundEffectByName("error");
						busy = false;
					}
					else
					{
						if (DateTime.UtcNow.Subtract(lastSendTime).TotalMilliseconds > 500.0)
						{
							currentReduction = 0;
							currentValue = 0;
							currentRepetitions = 0;
						}
						lastSendTime = DateTime.UtcNow;
						ItemChatLink val12 = new ItemChatLink();
						val12.set_ItemId(_config.SelectedToken);
						val12.set_Quantity(Convert.ToByte((amount2 <= 250) ? amount2 : GetNext(amount2, ref currentReduction, ref currentValue, ref currentRepetitions, ref lastTotalReachedTime)));
						ChatUtil.Send(((object)val12).ToString(), ProofLogix.Instance.ChatMessageKey.get_Value());
						busy = false;
					}
				}
			});
			((Control)sendBttn).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Expected O, but got Unknown
				if (!busy)
				{
					busy = true;
					ProofLogix.Instance.Resources.PlayMenuItemClick();
					int amount = ProofLogix.Instance.PartySync.LocalPlayer.KpProfile.GetToken(_config.SelectedToken).Amount;
					if (!CanSend(amount, lastTotalReachedTime))
					{
						GameService.Content.PlaySoundEffectByName("error");
						busy = false;
					}
					else
					{
						ItemChatLink val10 = new ItemChatLink();
						val10.set_ItemId(_config.SelectedToken);
						val10.set_Quantity((byte)1);
						ItemChatLink val11 = val10;
						ChatUtil.Send((amount > 255) ? AssetUtil.GetItemDisplayName(((object)val11).ToString(), amount) : ((object)val11).ToString(), ProofLogix.Instance.ChatMessageKey.get_Value());
						lastTotalReachedTime = DateTime.UtcNow;
						busy = false;
					}
				}
			});
			ContextMenuStrip val4 = new ContextMenuStrip();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_ClipsBounds(false);
			ContextMenuStrip menu = val4;
			IEnumerable<Token> playerTokens = ProofLogix.Instance.PartySync.LocalPlayer.KpProfile.GetTokens();
			List<Resource> generalItems = (from resource in ProofLogix.Instance.Resources.GetGeneralItems()
				where playerTokens.Any((Token item) => item.Id == resource.Id && item.Amount > 0)
				select resource).ToList();
			if (generalItems.Any())
			{
				ContextMenuStripItem val5 = new ContextMenuStripItem("General");
				((Control)val5).set_Parent((Container)(object)menu);
				val5.set_Submenu(new ContextMenuStrip());
				ContextMenuStripItem generalCategory = val5;
				AddProofEntries(generalCategory, generalItems, (Container)(object)labelPanel);
			}
			List<Resource> cofferItems = (from resource in ProofLogix.Instance.Resources.GetCofferItems()
				where playerTokens.Any((Token item) => item.Id == resource.Id && item.Amount > 0)
				select resource).ToList();
			if (cofferItems.Any())
			{
				ContextMenuStripItem val6 = new ContextMenuStripItem("Coffers");
				((Control)val6).set_Parent((Container)(object)menu);
				val6.set_Submenu(new ContextMenuStrip());
				ContextMenuStripItem coffersCategory = val6;
				AddProofEntries(coffersCategory, cofferItems, (Container)(object)labelPanel);
			}
			List<IEnumerable<Resource>> wingTokens = (from wing in ProofLogix.Instance.Resources.GetWings()
				select from ev in wing.Events
					where ev.Token != null
					select ev.Token into resource
					where playerTokens.Any((Token item) => item.Id == resource.Id && item.Amount > 0)
					select resource).ToList();
			if (wingTokens.Any())
			{
				ContextMenuStripItem val7 = new ContextMenuStripItem("Raids");
				((Control)val7).set_Parent((Container)(object)menu);
				val7.set_Submenu(new ContextMenuStrip());
				ContextMenuStripItem raidsCategory = val7;
				int i = 1;
				foreach (IEnumerable<Resource> wing2 in wingTokens)
				{
					ContextMenuStripItem val8 = new ContextMenuStripItem($"Wing {i++}");
					((Control)val8).set_Parent((Container)(object)raidsCategory.get_Submenu());
					val8.set_Submenu(new ContextMenuStrip());
					ContextMenuStripItem wingEntry = val8;
					AddProofEntries(wingEntry, wing2, (Container)(object)labelPanel);
				}
			}
			List<Resource> fractalItems = (from resource in ProofLogix.Instance.Resources.GetItemsForFractals()
				where playerTokens.Any((Token item) => item.Id == resource.Id && item.Amount > 0)
				select resource).ToList();
			if (fractalItems.Any())
			{
				ContextMenuStripItem val9 = new ContextMenuStripItem("Fractals");
				((Control)val9).set_Parent((Container)(object)menu);
				val9.set_Submenu(new ContextMenuStrip());
				ContextMenuStripItem fractalsCategory = val9;
				AddProofEntries(fractalsCategory, fractalItems, (Container)(object)labelPanel);
			}
			((Control)cogWheel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				GameService.Content.PlaySoundEffectByName("button-click");
				menu.Show(GameService.Input.get_Mouse().get_Position());
			});
			((View<IPresenter>)this).Build(buildPanel);
		}

		private bool CanSend(int totalAmount, DateTime lastTotalReachedTime)
		{
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				ScreenNotification.ShowNotification("Chat unavailable.", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			if (DateTime.UtcNow.Subtract(lastTotalReachedTime).TotalSeconds < 1.0)
			{
				ScreenNotification.ShowNotification("Action recharging.", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			if (totalAmount == 0)
			{
				ScreenNotification.ShowNotification("You can't ping empty records.", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			return true;
		}

		private void AddProofEntries(ContextMenuStripItem parent, IEnumerable<Resource> resources, Container labelPanel)
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			foreach (Resource resource in resources)
			{
				Token token = ProofLogix.Instance.PartySync.LocalPlayer.KpProfile.GetToken(resource.Id);
				string displayName = AssetUtil.GetItemDisplayName(resource.Name, token.Amount);
				Color rarity = ProofLogix.Instance.Resources.GetItem(resource.Id).Rarity.AsColor();
				ContextMenuStripItemWithColor contextMenuStripItemWithColor = new ContextMenuStripItemWithColor(displayName);
				((Control)contextMenuStripItemWithColor).set_Parent((Container)(object)parent.get_Submenu());
				contextMenuStripItemWithColor.TextColor = rarity;
				((Control)contextMenuStripItemWithColor).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0052: Unknown result type (might be due to invalid IL or missing references)
					//IL_007a: Unknown result type (might be due to invalid IL or missing references)
					//IL_009e: Unknown result type (might be due to invalid IL or missing references)
					Control obj = ((IEnumerable<Control>)labelPanel.get_Children()).FirstOrDefault();
					if (obj != null)
					{
						obj.Dispose();
					}
					_config.SelectedToken = resource.Id;
					Label val = BuildItemLabel(displayName, rarity);
					((Control)val).set_Parent(labelPanel);
					((Control)val).set_Top((labelPanel.get_ContentRegion().Height - ((Control)val).get_Height()) / 2);
					((Control)val).set_Left((labelPanel.get_ContentRegion().Width - ((Control)val).get_Width()) / 2);
					GameService.Content.PlaySoundEffectByName("color-change");
				});
			}
		}

		private Label BuildItemLabel(string displayName, Color color)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			Point size = LabelUtil.GetLabelSize((FontSize)20, displayName);
			Label val = new Label();
			val.set_Text(displayName);
			val.set_TextColor(color);
			val.set_StrokeText(true);
			val.set_ShowShadow(true);
			((Control)val).set_Size(new Point(size.X, size.Y));
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)20, (FontStyle)0));
			return val;
		}

		private int GetNext(int totalAmount, ref int currentReduction, ref int currentValue, ref int currentRepetitions, ref DateTime lastTotalReachedTime)
		{
			int rest = totalAmount - currentValue % totalAmount;
			int tempAmount = Math.Min(250 - currentReduction, rest);
			if (currentRepetitions >= RandomUtil.GetRandom(1, 3))
			{
				if (rest > 250)
				{
					currentValue += tempAmount;
					currentReduction++;
				}
				else
				{
					currentReduction = 0;
					currentValue = 0;
					lastTotalReachedTime = DateTime.UtcNow;
				}
				currentRepetitions = 0;
			}
			else
			{
				currentRepetitions++;
			}
			return tempAmount;
		}
	}
}
