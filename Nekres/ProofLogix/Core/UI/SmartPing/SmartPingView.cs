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
		private const char BRACKET_LEFT = '[';

		private const char BRACKET_RIGHT = ']';

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
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Expected O, but got Unknown
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Expected O, but got Unknown
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Expected O, but got Unknown
			//IL_0305: Expected O, but got Unknown
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Expected O, but got Unknown
			//IL_0345: Expected O, but got Unknown
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Expected O, but got Unknown
			//IL_0386: Expected O, but got Unknown
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Expected O, but got Unknown
			//IL_03e2: Expected O, but got Unknown
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0472: Unknown result type (might be due to invalid IL or missing references)
			//IL_047c: Expected O, but got Unknown
			//IL_047e: Expected O, but got Unknown
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
			int quantity = ProofLogix.Instance.PartySync.LocalPlayer.KpProfile.GetToken(_config.SelectedToken).Amount;
			FormattedLabel lbl = BuildItemLabel(quantity, _config.SelectedToken);
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
			((Control)val3).set_BasicTooltipText("Send to Chat\nMouse 1: As item with adjusted quantity portion.\nMouse 2: As message.");
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
			((Control)sendBttn).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Expected O, but got Unknown
				ProofLogix.Instance.Resources.PlayMenuItemClick();
				int amount2 = ProofLogix.Instance.PartySync.LocalPlayer.KpProfile.GetToken(_config.SelectedToken).Amount;
				if (!CanSend(amount2, lastTotalReachedTime))
				{
					GameService.Content.PlaySoundEffectByName("error");
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
				}
			});
			((Control)sendBttn).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Expected O, but got Unknown
				ProofLogix.Instance.Resources.PlayMenuItemClick();
				int amount = ProofLogix.Instance.PartySync.LocalPlayer.KpProfile.GetToken(_config.SelectedToken).Amount;
				if (!CanSend(amount, lastTotalReachedTime))
				{
					GameService.Content.PlaySoundEffectByName("error");
				}
				else
				{
					ItemChatLink val10 = new ItemChatLink();
					val10.set_ItemId(_config.SelectedToken);
					val10.set_Quantity((byte)1);
					ItemChatLink val11 = val10;
					ChatUtil.Send($"{'['}{amount} {val11}{']'}", ProofLogix.Instance.ChatMessageKey.get_Value());
					lastTotalReachedTime = DateTime.UtcNow;
				}
			});
			ContextMenuStrip val4 = new ContextMenuStrip();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_ClipsBounds(false);
			ContextMenuStrip menu = val4;
			ContextMenuStripItem val5 = new ContextMenuStripItem("General");
			((Control)val5).set_Parent((Container)(object)menu);
			val5.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem generalCategory = val5;
			AddProofEntries(generalCategory, ProofLogix.Instance.Resources.GetGeneralItems(), (Container)(object)labelPanel);
			ContextMenuStripItem val6 = new ContextMenuStripItem("Coffers");
			((Control)val6).set_Parent((Container)(object)menu);
			val6.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem coffersCategory = val6;
			AddProofEntries(coffersCategory, ProofLogix.Instance.Resources.GetCofferItems(), (Container)(object)labelPanel);
			ContextMenuStripItem val7 = new ContextMenuStripItem("Raids");
			((Control)val7).set_Parent((Container)(object)menu);
			val7.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem raidsCategory = val7;
			int i = 1;
			foreach (Raid.Wing wing in ProofLogix.Instance.Resources.GetWings())
			{
				ContextMenuStripItem val8 = new ContextMenuStripItem($"Wing {i++}");
				((Control)val8).set_Parent((Container)(object)raidsCategory.get_Submenu());
				val8.set_Submenu(new ContextMenuStrip());
				ContextMenuStripItem wingEntry = val8;
				AddProofEntries(wingEntry, from ev in wing.Events
					where ev.Token != null
					select ev.Token, (Container)(object)labelPanel);
			}
			ContextMenuStripItem val9 = new ContextMenuStripItem("Fractals");
			((Control)val9).set_Parent((Container)(object)menu);
			val9.set_Submenu(new ContextMenuStrip());
			ContextMenuStripItem fractalsCategory = val9;
			AddProofEntries(fractalsCategory, ProofLogix.Instance.Resources.GetItemsForFractals(), (Container)(object)labelPanel);
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
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			foreach (Resource resource in resources)
			{
				ContextMenuStripItem val = new ContextMenuStripItem(resource.Name);
				((Control)val).set_Parent((Container)(object)parent.get_Submenu());
				((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
					Control obj = ((IEnumerable<Control>)labelPanel.get_Children()).FirstOrDefault();
					if (obj != null)
					{
						obj.Dispose();
					}
					_config.SelectedToken = resource.Id;
					int amount = ProofLogix.Instance.PartySync.LocalPlayer.KpProfile.GetToken(resource.Id).Amount;
					FormattedLabel val2 = BuildItemLabel(amount, resource.Id);
					((Control)val2).set_Parent(labelPanel);
					((Control)val2).set_Top((labelPanel.get_ContentRegion().Height - ((Control)val2).get_Height()) / 2);
					((Control)val2).set_Left((labelPanel.get_ContentRegion().Width - ((Control)val2).get_Width()) / 2);
				});
			}
		}

		private FormattedLabel BuildItemLabel(int quantity, int tokenId)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			string str = $"{'['}{quantity} {ProofLogix.Instance.Resources.GetItem(tokenId).Name}{']'}";
			Point size = LabelUtil.GetLabelSize((FontSize)20, str);
			return new FormattedLabelBuilder().SetWidth(size.X).SetHeight(size.Y).CreatePart(str, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
			{
				o.SetFontSize((FontSize)20);
			})
				.Build();
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
