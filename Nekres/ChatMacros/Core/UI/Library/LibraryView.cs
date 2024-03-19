using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using LiteDB;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using Nekres.ChatMacros.Core.Services.Data;
using Nekres.ChatMacros.Core.UI.Configs;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.UI.Library
{
	internal class LibraryView : View
	{
		private class MenuItem<T> : MenuItem
		{
			public T Item;

			private bool _isActive;

			private Func<T, string> _basicTooltipText;

			private Func<T, Color> _color;

			private bool _mouseOverDelete;

			private Rectangle _deleteBounds;

			private bool _mouseOverActive;

			private Rectangle _activeBounds;

			private AsyncTexture2D _currentDeleteTexture;

			private AsyncTexture2D _deleteHoverTexture;

			private AsyncTexture2D _deleteTexture;

			private AsyncTexture2D _deletePressedTexture;

			private readonly AsyncTexture2D _textureArrow = AsyncTexture2D.FromAssetId(156057);

			private bool _mouseOverIconBox;

			public bool IsActive
			{
				get
				{
					return _isActive;
				}
				set
				{
					((Control)this).SetProperty<bool>(ref _isActive, value, true, "IsActive");
				}
			}

			private int LeftSidePadding
			{
				get
				{
					int leftSidePadding = 10;
					if (!((Container)this)._children.get_IsEmpty())
					{
						leftSidePadding += 16;
					}
					return leftSidePadding;
				}
			}

			private Rectangle FirstItemBoxRegion => new Rectangle(0, ((MenuItem)this).get_MenuItemHeight() / 2 - 16, 32, 32);

			public event EventHandler<EventArgs> DeleteClick;

			public MenuItem(T item, Func<T, string> basicTooltipText, Func<T, Color> color)
				: this()
			{
				Item = item;
				_basicTooltipText = basicTooltipText;
				_color = color;
				_deleteTexture = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175782);
				_deleteHoverTexture = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175784);
				_deletePressedTexture = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175783);
				_currentDeleteTexture = _deleteTexture;
			}

			public void AssignItem(T item, Func<T, string> basicTooltipText)
			{
				Item = item;
				_basicTooltipText = basicTooltipText;
			}

			protected override void OnMouseMoved(MouseEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				if (((Rectangle)(ref _deleteBounds)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					_currentDeleteTexture = _deleteHoverTexture;
					_mouseOverDelete = true;
					((Control)this).set_BasicTooltipText(Resources.Delete);
				}
				else if (((Rectangle)(ref _activeBounds)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					((Control)this).set_BasicTooltipText(Resources.This_macro_is_currently_active_and_can_be_triggered_);
					_mouseOverActive = true;
				}
				else
				{
					_currentDeleteTexture = _deleteTexture;
					_mouseOverDelete = false;
					_mouseOverActive = false;
					((Control)this).set_BasicTooltipText(_basicTooltipText(Item));
				}
				int mouseOverIconBox;
				if (base._canCheck && base._overSection)
				{
					Rectangle val = RectangleExtension.OffsetBy(FirstItemBoxRegion, LeftSidePadding, 0);
					mouseOverIconBox = (((Rectangle)(ref val)).Contains(((Control)this).get_RelativeMousePosition()) ? 1 : 0);
				}
				else
				{
					mouseOverIconBox = 0;
				}
				_mouseOverIconBox = (byte)mouseOverIconBox != 0;
				((MenuItem)this).OnMouseMoved(e);
			}

			protected override void OnClick(MouseEventArgs e)
			{
				if (!_mouseOverDelete)
				{
					((MenuItem)this).OnClick(e);
				}
			}

			protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
			{
				if (_mouseOverDelete)
				{
					_currentDeleteTexture = _deletePressedTexture;
					GameService.Content.PlaySoundEffectByName("button-click");
				}
				((Control)this).OnLeftMouseButtonPressed(e);
			}

			protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
			{
				if (_mouseOverDelete)
				{
					_currentDeleteTexture = _deleteHoverTexture;
					this.DeleteClick?.Invoke(this, EventArgs.Empty);
				}
				((Control)this).OnLeftMouseButtonReleased(e);
			}

			private void DrawDropdownArrow(SpriteBatch spriteBatch)
			{
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				Vector2 origin = default(Vector2);
				((Vector2)(ref origin))._002Ector(8f, 8f);
				Rectangle destinationRectangle = default(Rectangle);
				((Rectangle)(ref destinationRectangle))._002Ector(13, ((MenuItem)this).get_MenuItemHeight() / 2, 16, 16);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureArrow), destinationRectangle, (Rectangle?)null, Color.get_White(), ((MenuItem)this).get_ArrowRotation(), origin, (SpriteEffects)0);
			}

			public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Expected O, but got Unknown
				//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_0143: Unknown result type (might be due to invalid IL or missing references)
				int leftSidePadding = LeftSidePadding;
				if (!((Container)this)._children.get_IsEmpty())
				{
					DrawDropdownArrow(spriteBatch);
				}
				TextureRegion2D texture = null;
				if (((MenuItem)this).get_CanCheck())
				{
					string state = (((MenuItem)this).get_Checked() ? "-checked" : "-unchecked");
					string extension = "";
					extension = (_mouseOverIconBox ? "-active" : extension);
					extension = ((!((Control)this).get_Enabled()) ? "-disabled" : extension);
					texture = Checkable.TextureRegionsCheckbox.First((TextureRegion2D cb) => cb.get_Name() == "checkbox/cb" + state + extension);
				}
				else if (((MenuItem)this).get_Icon() != null && ((Container)this)._children.get_IsEmpty())
				{
					texture = new TextureRegion2D(AsyncTexture2D.op_Implicit(((MenuItem)this).get_Icon()));
				}
				if (texture != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, RectangleExtension.OffsetBy(FirstItemBoxRegion, leftSidePadding, 0));
				}
				if (base._canCheck)
				{
					leftSidePadding += 42;
				}
				else if (!((Container)this)._children.get_IsEmpty())
				{
					leftSidePadding += 10;
				}
				else if (base._icon != null)
				{
					leftSidePadding += 42;
				}
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, base._text, Control.get_Content().get_DefaultFont16(), new Rectangle(leftSidePadding, 0, ((Control)this).get_Width() - (leftSidePadding - 10), ((MenuItem)this).get_MenuItemHeight()), _color(Item), true, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}

			public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				int height = base._menuItemHeight - 7 - 3;
				_deleteBounds = new Rectangle(((Control)this).get_Width() - height - 4 - 15, (base._menuItemHeight - height) / 2, height, height);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_currentDeleteTexture), _deleteBounds);
				if (_isActive)
				{
					height += 4;
					_activeBounds = new Rectangle(((Rectangle)(ref _deleteBounds)).get_Left() - height - 4, _deleteBounds.Y, height, height);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, ChatMacros.Instance.Resources.EditIcon, _activeBounds);
				}
			}
		}

		private class MacroView : View
		{
			private class RawLinesEditView : View
			{
				protected ChatMacro _macro;

				public RawLinesEditView(ChatMacro macro)
					: this()
				{
					_macro = macro;
				}

				protected override void Build(Container buildPanel)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					//IL_002b: Unknown result type (might be due to invalid IL or missing references)
					//IL_002d: Unknown result type (might be due to invalid IL or missing references)
					//IL_003c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0056: Expected O, but got Unknown
					MultilineTextBox val = new MultilineTextBox();
					((Control)val).set_Parent(buildPanel);
					((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
					((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
					((TextInputBase)val).set_Font((BitmapFont)(object)ChatMacros.Instance.Resources.SourceCodePro24);
					MultilineTextBox editLinesBox = val;
					((TextInputBase)editLinesBox).set_Text(string.Join("\n", _macro.Lines.Select((ChatLine l) => l.Serialize())));
					((TextInputBase)editLinesBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object _, ValueEventArgs<bool> e)
					{
						if (!e.get_Value())
						{
							List<ChatLine> list = _macro.Lines.ToList();
							if (!ChatMacros.Instance.Data.DeleteMany(_macro.Lines))
							{
								_macro.Lines = list;
								ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
								GameService.Content.PlaySoundEffectByName("error");
							}
							else
							{
								_macro.Lines.Clear();
								List<ChatLine> list2 = ((TextInputBase)editLinesBox).get_Text().ReadLines().Select(ChatLine.Parse)
									.ToList();
								_macro.Lines = (SaveLines(list2.ToArray()) ? list2 : list);
							}
						}
					});
					((View<IPresenter>)this).Build(buildPanel);
				}

				protected bool SaveLines(params ChatLine[] lines)
				{
					if (lines.Length != 0 && !ChatMacros.Instance.Data.Insert(lines))
					{
						ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
						GameService.Content.PlaySoundEffectByName("error");
						return false;
					}
					if (ChatMacros.Instance.Data.Upsert(_macro))
					{
						ChatMacros.Instance.Speech.UpdateGrammar();
					}
					return true;
				}
			}

			private class FancyLinesEditView : RawLinesEditView
			{
				private class LineView : View
				{
					private readonly ChatLine _line;

					private AsyncTexture2D _squadBroadcastActive;

					private AsyncTexture2D _squadBroadcastInactive;

					private AsyncTexture2D _squadBroadcastActiveHover;

					private AsyncTexture2D _squadBroadcastInactiveHover;

					public bool IsDragging { get; private set; }

					public event EventHandler<EventArgs> RemoveClick;

					public event EventHandler<EventArgs> DragEnd;

					public LineView(ChatLine line)
						: this()
					{
						_line = line;
						_squadBroadcastActive = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1304068);
						_squadBroadcastActiveHover = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1304069);
						_squadBroadcastInactive = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1234950);
						_squadBroadcastInactiveHover = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1304070);
					}

					protected override void Build(Container buildPanel)
					{
						//IL_008b: Unknown result type (might be due to invalid IL or missing references)
						//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
						//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
						//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
						//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
						//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
						//IL_0115: Unknown result type (might be due to invalid IL or missing references)
						//IL_0128: Unknown result type (might be due to invalid IL or missing references)
						//IL_0134: Unknown result type (might be due to invalid IL or missing references)
						//IL_013e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0168: Unknown result type (might be due to invalid IL or missing references)
						//IL_0182: Expected O, but got Unknown
						//IL_0197: Unknown result type (might be due to invalid IL or missing references)
						//IL_019c: Unknown result type (might be due to invalid IL or missing references)
						//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
						//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
						//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
						//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
						//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
						//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
						//IL_020b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0223: Expected O, but got Unknown
						//IL_024e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0253: Unknown result type (might be due to invalid IL or missing references)
						//IL_025f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0267: Unknown result type (might be due to invalid IL or missing references)
						//IL_026f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0282: Unknown result type (might be due to invalid IL or missing references)
						//IL_029c: Unknown result type (might be due to invalid IL or missing references)
						//IL_02ac: Expected O, but got Unknown
						//IL_031c: Unknown result type (might be due to invalid IL or missing references)
						//IL_0321: Unknown result type (might be due to invalid IL or missing references)
						//IL_032d: Unknown result type (might be due to invalid IL or missing references)
						//IL_0335: Unknown result type (might be due to invalid IL or missing references)
						//IL_033d: Unknown result type (might be due to invalid IL or missing references)
						//IL_0350: Unknown result type (might be due to invalid IL or missing references)
						//IL_035b: Unknown result type (might be due to invalid IL or missing references)
						KeyValueDropdown<ChatChannel> keyValueDropdown = new KeyValueDropdown<ChatChannel>();
						((Control)keyValueDropdown).set_Parent(buildPanel);
						keyValueDropdown.PlaceholderText = Resources.Select_a_target_channel___;
						keyValueDropdown.SelectedItem = _line.Channel;
						((Control)keyValueDropdown).set_BasicTooltipText(Resources.Select_a_target_channel___);
						keyValueDropdown.AutoSizeWidth = true;
						KeyValueDropdown<ChatChannel> targetChannelDd = keyValueDropdown;
						foreach (ChatChannel channel in Enum.GetValues(typeof(ChatChannel)).Cast<ChatChannel>())
						{
							targetChannelDd.AddItem(channel, channel.ToDisplayName(), channel.GetHeadingColor());
						}
						TextBox messageInput = null;
						TextBox whisperTo = null;
						Image squadBroadcast = null;
						TextBox val = new TextBox();
						((Control)val).set_Parent(buildPanel);
						((TextInputBase)val).set_PlaceholderText(Resources.Enter_a_message___);
						((TextInputBase)val).set_Text(_line.Message);
						((Control)val).set_Width(buildPanel.get_ContentRegion().Width - ((Control)targetChannelDd).get_Right() - 12 - 50);
						((Control)val).set_Left(((Control)targetChannelDd).get_Right() + 4);
						((TextInputBase)val).set_ForeColor(_line.Channel.GetMessageColor());
						((Control)val).set_BasicTooltipText(string.IsNullOrWhiteSpace(_line.Message) ? Resources.Enter_a_message___ : _line.ToChatMessage());
						((TextInputBase)val).set_Font((BitmapFont)(object)ChatMacros.Instance.Resources.SourceCodePro24);
						messageInput = val;
						Image val2 = new Image(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1444522));
						((Control)val2).set_Parent(buildPanel);
						((Control)val2).set_Left(((Control)messageInput).get_Right() - 32);
						((Control)val2).set_Width(32);
						((Control)val2).set_Height(32);
						((Control)val2).set_Top(-2);
						((Control)val2).set_BasicTooltipText(string.Format(Resources.Message_exceeds_limit_of__0__characters_, 199));
						((Control)val2).set_Visible(_line.ToChatMessage().Length > 199);
						((Control)val2).set_ZIndex(((Control)messageInput).get_ZIndex() + 1);
						Image overlengthWarn = val2;
						if (targetChannelDd.SelectedItem == ChatChannel.Whisper)
						{
							CreateWhisperToField();
						}
						else if (targetChannelDd.SelectedItem == ChatChannel.Squad)
						{
							CreateSquadBroadcastTick();
						}
						Image val3 = new Image();
						((Control)val3).set_Parent(buildPanel);
						((Control)val3).set_Width(25);
						((Control)val3).set_Height(25);
						((Control)val3).set_Left(((Control)messageInput).get_Right() + 4);
						val3.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175782));
						((Control)val3).set_BasicTooltipText(Resources.Remove);
						Image remove = val3;
						((Control)remove).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
						{
							remove.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175784));
						});
						((Control)remove).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
						{
							remove.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175782));
						});
						((Control)remove).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
						{
							remove.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(2175783));
						});
						((Control)remove).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
						{
							GameService.Content.PlaySoundEffectByName("button-click");
							this.RemoveClick?.Invoke(this, EventArgs.Empty);
						});
						Image val4 = new Image(AsyncTexture2D.op_Implicit(ChatMacros.Instance.Resources.DragReorderIcon));
						((Control)val4).set_Parent(buildPanel);
						((Control)val4).set_Width(25);
						((Control)val4).set_Height(25);
						((Control)val4).set_Left(((Control)remove).get_Right() + 4);
						((Control)val4).set_BasicTooltipText(Resources.Drag_to_Reorder);
						((Control)val4).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
						{
							IsDragging = true;
							GameService.Content.PlaySoundEffectByName("button-click");
						});
						GameService.Input.get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
						((Control)val4).add_Disposed((EventHandler<EventArgs>)delegate
						{
							GameService.Input.get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnLeftMouseButtonReleased);
						});
						((TextInputBase)messageInput).add_TextChanged((EventHandler<EventArgs>)delegate
						{
							string text = (_line.Channel.ToShortChatCommand() + " ").TrimStart() + ((TextInputBase)messageInput).get_Text();
							((Control)overlengthWarn).set_Visible(text.Length > 199);
							((Control)messageInput).set_BasicTooltipText(string.IsNullOrEmpty(((TextInputBase)messageInput).get_Text()) ? Resources.Enter_a_message___ : text);
						});
						((TextInputBase)messageInput).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object _, ValueEventArgs<bool> e)
						{
							if (!e.get_Value())
							{
								_line.Message = ((TextInputBase)messageInput).get_Text().TrimEnd();
								((Control)messageInput).set_BasicTooltipText(string.IsNullOrEmpty(_line.Message) ? Resources.Enter_a_message___ : _line.ToChatMessage());
								Save();
							}
						});
						((Control)targetChannelDd).add_Resized((EventHandler<ResizedEventArgs>)delegate
						{
							//IL_000c: Unknown result type (might be due to invalid IL or missing references)
							((Control)messageInput).set_Width(buildPanel.get_ContentRegion().Width - ((Control)targetChannelDd).get_Width() - 16 - 50);
							((Control)messageInput).set_Left(((Control)targetChannelDd).get_Right() + 4);
						});
						targetChannelDd.ValueChanged += delegate(object _, ValueChangedEventArgs<ChatChannel> e)
						{
							//IL_002c: Unknown result type (might be due to invalid IL or missing references)
							//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
							_line.Channel = e.get_NewValue();
							((TextInputBase)messageInput).set_ForeColor(_line.Channel.GetMessageColor());
							((Control)messageInput).set_BasicTooltipText(string.IsNullOrWhiteSpace(_line.Message) ? Resources.Enter_a_message___ : _line.ToChatMessage());
							((Control)overlengthWarn).set_Visible(_line.ToChatMessage().Length > 199);
							Save();
							TextBox obj = whisperTo;
							if (obj != null)
							{
								((Control)obj).Dispose();
							}
							Image obj2 = squadBroadcast;
							if (obj2 != null)
							{
								((Control)obj2).Dispose();
							}
							((Control)messageInput).set_Width(buildPanel.get_ContentRegion().Width - ((Control)targetChannelDd).get_Right() - 16 - 50);
							((Control)messageInput).set_Left(((Control)targetChannelDd).get_Right() + 4);
							if (e.get_NewValue() == ChatChannel.Whisper)
							{
								CreateWhisperToField();
							}
							else if (e.get_NewValue() == ChatChannel.Squad)
							{
								CreateSquadBroadcastTick();
							}
						};
						((View<IPresenter>)this).Build(buildPanel);
						void CreateSquadBroadcastTick()
						{
							//IL_0015: Unknown result type (might be due to invalid IL or missing references)
							//IL_001a: Unknown result type (might be due to invalid IL or missing references)
							//IL_0026: Unknown result type (might be due to invalid IL or missing references)
							//IL_0037: Unknown result type (might be due to invalid IL or missing references)
							//IL_003f: Unknown result type (might be due to invalid IL or missing references)
							//IL_0047: Unknown result type (might be due to invalid IL or missing references)
							//IL_004f: Unknown result type (might be due to invalid IL or missing references)
							//IL_005a: Unknown result type (might be due to invalid IL or missing references)
							//IL_008f: Expected O, but got Unknown
							//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
							bool hovering = false;
							Image val5 = new Image();
							((Control)val5).set_Parent(buildPanel);
							((Control)val5).set_Left(((Control)targetChannelDd).get_Right());
							((Control)val5).set_Height(32);
							((Control)val5).set_Width(32);
							((Control)val5).set_Top(-2);
							((Control)val5).set_BasicTooltipText(Resources.Broadcast_to_Squad);
							val5.set_Texture(_line.SquadBroadcast ? _squadBroadcastActive : _squadBroadcastInactive);
							squadBroadcast = val5;
							((Control)squadBroadcast).add_Click((EventHandler<MouseEventArgs>)delegate
							{
								_line.SquadBroadcast = !_line.SquadBroadcast;
								if (hovering)
								{
									squadBroadcast.set_Texture(_line.SquadBroadcast ? _squadBroadcastActiveHover : _squadBroadcastInactiveHover);
								}
								else
								{
									squadBroadcast.set_Texture(_line.SquadBroadcast ? _squadBroadcastActive : _squadBroadcastInactive);
								}
								GameService.Content.PlaySoundEffectByName("button-click");
								Save();
							});
							((Control)squadBroadcast).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
							{
								hovering = true;
								squadBroadcast.set_Texture(_line.SquadBroadcast ? _squadBroadcastActiveHover : _squadBroadcastInactiveHover);
							});
							((Control)squadBroadcast).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
							{
								hovering = false;
								squadBroadcast.set_Texture(_line.SquadBroadcast ? _squadBroadcastActive : _squadBroadcastInactive);
							});
							((Control)messageInput).set_Width(buildPanel.get_ContentRegion().Width - ((Control)squadBroadcast).get_Right() - 8 - 50 - 2);
							((Control)messageInput).set_Left(((Control)squadBroadcast).get_Right() + 2);
							((Control)messageInput).Invalidate();
						}
						void CreateWhisperToField()
						{
							//IL_0001: Unknown result type (might be due to invalid IL or missing references)
							//IL_0006: Unknown result type (might be due to invalid IL or missing references)
							//IL_0012: Unknown result type (might be due to invalid IL or missing references)
							//IL_001d: Unknown result type (might be due to invalid IL or missing references)
							//IL_0025: Unknown result type (might be due to invalid IL or missing references)
							//IL_0036: Unknown result type (might be due to invalid IL or missing references)
							//IL_0040: Unknown result type (might be due to invalid IL or missing references)
							//IL_0053: Unknown result type (might be due to invalid IL or missing references)
							//IL_0068: Unknown result type (might be due to invalid IL or missing references)
							//IL_0083: Expected O, but got Unknown
							//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
							TextBox val6 = new TextBox();
							((Control)val6).set_Parent(buildPanel);
							((TextInputBase)val6).set_PlaceholderText(Resources.Recipient___);
							((Control)val6).set_Width(100);
							((TextInputBase)val6).set_ForeColor(_line.Channel.GetMessageColor());
							((Control)val6).set_Left(((Control)targetChannelDd).get_Right() + 4);
							((TextInputBase)val6).set_Font((BitmapFont)(object)ChatMacros.Instance.Resources.SourceCodePro24);
							((TextInputBase)val6).set_Text(_line.WhisperTo);
							whisperTo = val6;
							((TextInputBase)whisperTo).add_TextChanged((EventHandler<EventArgs>)delegate
							{
								_line.WhisperTo = ((TextInputBase)whisperTo).get_Text().Trim();
								((Control)whisperTo).set_BasicTooltipText(_line.WhisperTo);
								Save();
							});
							((Control)messageInput).set_Width(buildPanel.get_ContentRegion().Width - ((Control)whisperTo).get_Right() - 12 - 50);
							((Control)messageInput).set_Left(((Control)whisperTo).get_Right() + 4);
							((Control)messageInput).Invalidate();
						}
						void OnLeftMouseButtonReleased(object o, MouseEventArgs mouseEventArgs)
						{
							if (IsDragging)
							{
								this.DragEnd?.Invoke(this, EventArgs.Empty);
							}
							IsDragging = false;
						}
					}

					private void Save()
					{
						if (!ChatMacros.Instance.Data.Upsert(_line))
						{
							ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
						}
						else
						{
							ChatMacros.Instance.Macro.UpdateMacros();
						}
					}
				}

				public FancyLinesEditView(ChatMacro macro)
					: base(macro)
				{
				}

				protected override void Build(Container buildPanel)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					//IL_002b: Unknown result type (might be due to invalid IL or missing references)
					//IL_002d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0041: Unknown result type (might be due to invalid IL or missing references)
					//IL_0048: Unknown result type (might be due to invalid IL or missing references)
					//IL_004f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0056: Unknown result type (might be due to invalid IL or missing references)
					//IL_0061: Unknown result type (might be due to invalid IL or missing references)
					//IL_006b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0076: Unknown result type (might be due to invalid IL or missing references)
					//IL_0080: Unknown result type (might be due to invalid IL or missing references)
					//IL_008c: Expected O, but got Unknown
					FlowPanel val = new FlowPanel();
					((Control)val).set_Parent(buildPanel);
					((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
					((Control)val).set_Height(buildPanel.get_ContentRegion().Height - 7 - 50);
					val.set_FlowDirection((ControlFlowDirection)3);
					((Control)val).set_Top(0);
					((Panel)val).set_ShowBorder(true);
					val.set_ControlPadding(new Vector2(0f, 2f));
					val.set_OuterControlPadding(new Vector2(4f, 4f));
					((Panel)val).set_CanScroll(true);
					FlowPanel linesPanel = val;
					foreach (ChatLine line in _macro.Lines)
					{
						AddLine((Container)(object)linesPanel, line);
					}
					StandardButtonCustomFont standardButtonCustomFont = new StandardButtonCustomFont(ChatMacros.Instance.Resources.RubikRegular26);
					((Control)standardButtonCustomFont).set_Parent(buildPanel);
					((Control)standardButtonCustomFont).set_Width(((Control)linesPanel).get_Width() - 20);
					((Control)standardButtonCustomFont).set_Top(((Control)linesPanel).get_Bottom() + 7);
					((Control)standardButtonCustomFont).set_Left(10);
					((Control)standardButtonCustomFont).set_Height(50);
					((StandardButton)standardButtonCustomFont).set_Text(Resources.Add_Line);
					((Control)standardButtonCustomFont).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						ChatLine chatLine = ChatLine.Parse(null);
						chatLine.Channel = ((!_macro.Lines.IsNullOrEmpty()) ? _macro.Lines.Last().Channel : ChatChannel.Current);
						chatLine.WhisperTo = (_macro.Lines.IsNullOrEmpty() ? string.Empty : _macro.Lines.Last().WhisperTo);
						List<ChatLine> lines = _macro.Lines.ToList();
						if (SaveLines(chatLine))
						{
							_macro.Lines.Add(chatLine);
							GameService.Content.PlaySoundEffectByName("button-click");
							AddLine((Container)(object)linesPanel, chatLine);
						}
						else
						{
							_macro.Lines = lines;
						}
					});
				}

				private void AddLine(Container parent, ChatLine line)
				{
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					//IL_002d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0034: Unknown result type (might be due to invalid IL or missing references)
					//IL_0046: Unknown result type (might be due to invalid IL or missing references)
					//IL_0053: Expected O, but got Unknown
					ViewContainer val = new ViewContainer();
					((Control)val).set_Parent(parent);
					((Control)val).set_Width(parent.get_ContentRegion().Width - 18);
					((Control)val).set_Height(32);
					ViewContainer lineDisplay = val;
					LineView lineView = new LineView(line);
					lineDisplay.Show((IView)(object)lineView);
					lineView.RemoveClick += delegate
					{
						List<ChatLine> lines2 = _macro.Lines.ToList();
						if (_macro.Lines.RemoveAll((ChatLine x) => x.Id.Equals(line.Id)) >= 1)
						{
							if (!ChatMacros.Instance.Data.Delete(line))
							{
								_macro.Lines = lines2;
								ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
								GameService.Content.PlaySoundEffectByName("error");
							}
							else
							{
								if (!ChatMacros.Instance.Data.Upsert(_macro))
								{
									ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
									GameService.Content.PlaySoundEffectByName("error");
								}
								ChatMacros.Instance.Speech.UpdateGrammar();
								((Control)lineDisplay).Dispose();
							}
						}
					};
					int lineIndex = _macro.Lines.IndexOf(line);
					GameService.Input.get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)OnMouseMoved);
					((Control)lineDisplay).add_Disposed((EventHandler<EventArgs>)delegate
					{
						GameService.Input.get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)OnMouseMoved);
					});
					lineView.DragEnd += delegate
					{
						//IL_0006: Unknown result type (might be due to invalid IL or missing references)
						int index = MathHelper.Clamp((((Control)parent).get_RelativeMousePosition().Y - 36) / ((Control)lineDisplay).get_Height(), 0, _macro.Lines.Count - 1);
						List<ChatLine> lines = _macro.Lines.ToList();
						if (_macro.Lines.RemoveAll((ChatLine l) => l.Id.Equals(line.Id)) < 1)
						{
							_macro.Lines = lines;
							ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
							GameService.Content.PlaySoundEffectByName("error");
						}
						else
						{
							_macro.Lines.Insert(index, line);
							if (!ChatMacros.Instance.Data.Upsert(_macro))
							{
								_macro.Lines = lines;
								ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
								GameService.Content.PlaySoundEffectByName("error");
							}
							else
							{
								ChatMacros.Instance.Macro.UpdateMacros();
								GameService.Content.PlaySoundEffectByName("color-change");
								parent.ClearChildren();
								foreach (ChatLine current in _macro.Lines)
								{
									AddLine(parent, current);
								}
							}
						}
					};
					void OnMouseMoved(object o, MouseEventArgs mouseEventArgs)
					{
						//IL_002b: Unknown result type (might be due to invalid IL or missing references)
						if (lineView.IsDragging)
						{
							List<ChatLine> list = _macro.Lines.ToList();
							int dropIndex = MathHelper.Clamp((((Control)parent).get_RelativeMousePosition().Y - 36) / ((Control)lineDisplay).get_Height(), 0, _macro.Lines.Count - 1);
							if (lineIndex != dropIndex)
							{
								ChatMacros.Instance.Resources.PlayMenuItemClick();
								lineIndex = dropIndex;
							}
							list.Remove(line);
							list.Insert(dropIndex, line);
							parent.ClearChildren();
							foreach (ChatLine reLine in list)
							{
								AddLine(parent, reLine);
							}
						}
					}
				}
			}

			private readonly ChatMacro _macro;

			private ViewContainer _editField;

			private Image _linkFileState;

			public event EventHandler<ValueEventArgs<string>> TitleChanged;

			public MacroView(ChatMacro macro)
				: this()
			{
				_macro = macro;
			}

			private void UpdateLinkFileState(bool showError = false)
			{
				if (!FileUtil.Exists(_macro.LinkFile, out var _, ChatMacros.Logger, ChatMacros.Instance.BasePaths.ToArray()))
				{
					_macro.LinkFile = string.Empty;
					_linkFileState.set_Texture(AsyncTexture2D.op_Implicit(ChatMacros.Instance.Resources.LinkBrokenIcon));
					((Control)_linkFileState).set_BasicTooltipText(Resources.Inactive_File_Sync + ": " + Resources.Enter_a_file_path___ + " (" + Resources.Optional + ")");
					if (showError)
					{
						ScreenNotification.ShowNotification(Resources.File_not_found_or_access_denied_, (NotificationType)1, (Texture2D)null, 4);
					}
				}
				else
				{
					_linkFileState.set_Texture(AsyncTexture2D.op_Implicit(ChatMacros.Instance.Resources.LinkIcon));
					((Control)_linkFileState).set_BasicTooltipText(Resources.Active_File_Sync + ": " + Path.GetFileName(_macro.LinkFile));
				}
			}

			private void OnLinkFileUpdate(object sender, ValueEventArgs<BaseMacro> e)
			{
				ChatMacro chatMacro = e.get_Value() as ChatMacro;
				if (chatMacro != null)
				{
					_macro.LinkFile = chatMacro.LinkFile;
					UpdateLinkFileState();
					if (_editField != null && _macro.Id.Equals(e.get_Value().Id))
					{
						_macro.Lines = chatMacro.Lines;
						_editField.Show((IView)(object)(ChatMacros.Instance.LibraryConfig.get_Value().AdvancedEdit ? new RawLinesEditView(_macro) : new FancyLinesEditView(_macro)));
					}
				}
			}

			protected override void Unload()
			{
				ChatMacros.Instance.Macro.Observer.MacroUpdate -= OnLinkFileUpdate;
				((View<IPresenter>)this).Unload();
			}

			protected override void Build(Container buildPanel)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Expected O, but got Unknown
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00af: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e6: Expected O, but got Unknown
				//IL_0104: Unknown result type (might be due to invalid IL or missing references)
				//IL_0109: Unknown result type (might be due to invalid IL or missing references)
				//IL_0114: Unknown result type (might be due to invalid IL or missing references)
				//IL_011b: Unknown result type (might be due to invalid IL or missing references)
				//IL_011d: Unknown result type (might be due to invalid IL or missing references)
				//IL_012c: Unknown result type (might be due to invalid IL or missing references)
				//IL_012e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0152: Unknown result type (might be due to invalid IL or missing references)
				//IL_015e: Unknown result type (might be due to invalid IL or missing references)
				//IL_018c: Expected O, but got Unknown
				//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_020a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0224: Unknown result type (might be due to invalid IL or missing references)
				//IL_0230: Expected O, but got Unknown
				//IL_0243: Unknown result type (might be due to invalid IL or missing references)
				//IL_0248: Unknown result type (might be due to invalid IL or missing references)
				//IL_024f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0257: Unknown result type (might be due to invalid IL or missing references)
				//IL_025f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0270: Unknown result type (might be due to invalid IL or missing references)
				//IL_0288: Expected O, but got Unknown
				//IL_02de: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
				//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_0310: Unknown result type (might be due to invalid IL or missing references)
				//IL_0323: Unknown result type (might be due to invalid IL or missing references)
				//IL_0364: Unknown result type (might be due to invalid IL or missing references)
				//IL_0374: Expected O, but got Unknown
				TextBox val = new TextBox();
				((Control)val).set_Parent(buildPanel);
				((TextInputBase)val).set_PlaceholderText(Resources.Enter_a_title___);
				((TextInputBase)val).set_Text(_macro.Title);
				((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
				((Control)val).set_Height(35);
				((Control)val).set_BasicTooltipText(Resources.Enter_a_title___);
				((TextInputBase)val).set_MaxLength(100);
				((TextInputBase)val).set_Font(GameService.Content.get_DefaultFont18());
				((TextInputBase)val).set_ForeColor(ChatMacros.Instance.Resources.BrightGold);
				val.set_HorizontalAlignment((HorizontalAlignment)1);
				TextBox titleField = val;
				((TextInputBase)titleField).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object _, ValueEventArgs<bool> e)
				{
					if (!e.get_Value())
					{
						string text3 = new string(_macro.Title.ToCharArray());
						_macro.Title = ((TextInputBase)titleField).get_Text().Trim();
						if (!ChatMacros.Instance.Data.Upsert(_macro))
						{
							ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
							((TextInputBase)titleField).set_Text(text3);
							_macro.Title = text3;
						}
						else
						{
							this.TitleChanged?.Invoke(this, new ValueEventArgs<string>(_macro.Title));
							ChatMacros.Instance.Macro.UpdateMacros();
						}
					}
				});
				ViewContainer val2 = new ViewContainer();
				((Control)val2).set_Parent(buildPanel);
				((Control)val2).set_Width(buildPanel.get_ContentRegion().Width);
				((Control)val2).set_Height(200);
				((Control)val2).set_Top(((Control)titleField).get_Bottom() + 7);
				ViewContainer macroConfig = val2;
				macroConfig.Show((IView)(object)new BaseMacroSettings(_macro, () => ChatMacros.Instance.Data.Upsert(_macro)));
				ViewContainer val3 = new ViewContainer();
				((Panel)val3).set_Title(Resources.Message_Sequence);
				((Control)val3).set_Parent(buildPanel);
				((Control)val3).set_Width(buildPanel.get_ContentRegion().Width);
				((Control)val3).set_Height(buildPanel.get_ContentRegion().Height - ((Control)titleField).get_Bottom() - ((Control)macroConfig).get_Height() - 7);
				((Control)val3).set_Top(((Control)macroConfig).get_Bottom());
				((Control)val3).set_BasicTooltipText("List of Placeholders\n\nPlaceholders are executable commands inside your messages that get replaced with their result when the message is sent.\nPlaceholders must be surrounded by paranthesis and their parameters are separated by whitespace.\n\n" + string.Join("\n", ChatMacros.Instance.Resources.Placeholders));
				_editField = val3;
				_editField.Show((IView)(object)(ChatMacros.Instance.LibraryConfig.get_Value().AdvancedEdit ? new RawLinesEditView(_macro) : new FancyLinesEditView(_macro)));
				Image val4 = new Image();
				((Control)val4).set_Parent(buildPanel);
				((Control)val4).set_Width(22);
				((Control)val4).set_Height(22);
				((Control)val4).set_Left(((Control)_editField).get_Right() - 22 - 4);
				((Control)val4).set_Top(((Control)_editField).get_Top() + 8);
				val4.set_Texture(AsyncTexture2D.op_Implicit(ChatMacros.Instance.Resources.OpenExternalIcon));
				((Control)val4).set_BasicTooltipText("Open and Edit in default text editor.");
				Image openExternalBttn = val4;
				((Control)openExternalBttn).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					if (FileUtil.Exists(_macro.LinkFile, out var _, ChatMacros.Logger, ChatMacros.Instance.BasePaths.ToArray()))
					{
						FileUtil.OpenExternally(_macro.LinkFile);
					}
					else
					{
						string text = Path.Combine(ChatMacros.Instance.ModuleDirectory, "synced");
						Directory.CreateDirectory(text);
						string text2 = Path.Combine(text, $"{_macro.Id}.txt");
						try
						{
							using StreamWriter streamWriter = File.CreateText(text2);
							streamWriter.WriteAsync(string.Join("\n", _macro.Lines.Select((ChatLine x) => x.Serialize())));
						}
						catch (Exception)
						{
							ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
							return;
						}
						_macro.LinkFile = text2;
						UpdateLinkFileState(showError: true);
						ChatMacros.Instance.Data.LinkFileChanged(_macro);
						FileUtil.OpenExternally(_macro.LinkFile);
					}
				});
				Image val5 = new Image();
				((Control)val5).set_Parent(buildPanel);
				((Control)val5).set_Width(22);
				((Control)val5).set_Height(22);
				((Control)val5).set_Left(((Control)openExternalBttn).get_Left() - 22 - 4);
				((Control)val5).set_Top(((Control)_editField).get_Top() + 8);
				_linkFileState = val5;
				((Control)_linkFileState).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)async delegate
				{
					using AsyncFileDialog<OpenFileDialog> fileSelect = new AsyncFileDialog<OpenFileDialog>(Resources.File_to_monitor_and_synchronize_lines_with_, "txt files (*.txt)|*.txt", _macro.LinkFile);
					if (await fileSelect.Show() == DialogResult.OK)
					{
						string oldLinkFile = new string(_macro.LinkFile?.ToCharArray());
						_macro.LinkFile = fileSelect.Dialog.FileName;
						if (!ChatMacros.Instance.Data.Upsert(_macro))
						{
							_macro.LinkFile = oldLinkFile;
							ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
							GameService.Content.PlaySoundEffectByName("error");
						}
						else
						{
							GameService.Content.PlaySoundEffectByName("button-click");
							UpdateLinkFileState(showError: true);
							ChatMacros.Instance.Data.LinkFileChanged(_macro);
						}
					}
				});
				((Control)_linkFileState).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					_macro.LinkFile = string.Empty;
					UpdateLinkFileState();
					ChatMacros.Instance.Data.LinkFileChanged(_macro);
				});
				UpdateLinkFileState();
				ChatMacros.Instance.Macro.Observer.MacroUpdate += OnLinkFileUpdate;
				Image val6 = new Image();
				((Control)val6).set_Parent(buildPanel);
				((Control)val6).set_Width(32);
				((Control)val6).set_Height(32);
				((Control)val6).set_Left(((Control)_linkFileState).get_Left() - 32 - 4);
				((Control)val6).set_Top(((Control)_editField).get_Top() + 4);
				val6.set_Texture(AsyncTexture2D.op_Implicit(ChatMacros.Instance.LibraryConfig.get_Value().AdvancedEdit ? ChatMacros.Instance.Resources.SwitchModeOnIcon : ChatMacros.Instance.Resources.SwitchModeOffIcon));
				((Control)val6).set_BasicTooltipText("Change Edit Mode");
				Image editMode = val6;
				((Control)editMode).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					ChatMacros.Instance.LibraryConfig.get_Value().AdvancedEdit = !ChatMacros.Instance.LibraryConfig.get_Value().AdvancedEdit;
					if (ChatMacros.Instance.LibraryConfig.get_Value().AdvancedEdit)
					{
						_editField.Show((IView)(object)new RawLinesEditView(_macro));
						editMode.set_Texture(AsyncTexture2D.op_Implicit(ChatMacros.Instance.Resources.SwitchModeOnIcon));
					}
					else
					{
						_editField.Show((IView)(object)new FancyLinesEditView(_macro));
						editMode.set_Texture(AsyncTexture2D.op_Implicit(ChatMacros.Instance.Resources.SwitchModeOffIcon));
					}
				});
				((View<IPresenter>)this).Build(buildPanel);
			}
		}

		private const int MAX_MENU_ENTRY_TITLE_WIDTH = 195;

		private AsyncTexture2D _cogWheelIcon;

		private AsyncTexture2D _cogWheelIconHover;

		private AsyncTexture2D _cogWheelIconClick;

		private LibraryConfig _config;

		public Menu MacroEntries { get; private set; }

		public ViewContainer MacroConfig { get; private set; }

		public LibraryView(LibraryConfig config)
			: this()
		{
			_config = config;
			_cogWheelIcon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155052);
			_cogWheelIconHover = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(157110);
			_cogWheelIconClick = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(157109);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Expected O, but got Unknown
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Expected O, but got Unknown
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Expected O, but got Unknown
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Expected O, but got Unknown
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Expected O, but got Unknown
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(300);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height - 50);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_Title("Chat Macros");
			val.set_OuterControlPadding(new Vector2(2f, 5f));
			val.set_ControlPadding(new Vector2(2f, 5f));
			((Panel)val).set_ShowBorder(true);
			FlowPanel panel = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)panel);
			((Control)val2).set_Width(((Container)panel).get_ContentRegion().Width);
			((Control)val2).set_Height(35);
			val2.set_FlowDirection((ControlFlowDirection)2);
			val2.set_OuterControlPadding(new Vector2(0f, 0f));
			val2.set_ControlPadding(new Vector2(4f, 0f));
			FlowPanel filterWrap = val2;
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)filterWrap);
			((Control)val3).set_Width(((Container)filterWrap).get_ContentRegion().Width - 40);
			((Control)val3).set_Height(((Container)filterWrap).get_ContentRegion().Height);
			((TextInputBase)val3).set_PlaceholderText(Resources.Search___);
			TextBox searchBar = val3;
			Image val4 = new Image();
			((Control)val4).set_Parent((Container)(object)filterWrap);
			((Control)val4).set_Width(((Control)searchBar).get_Height());
			((Control)val4).set_Height(((Control)searchBar).get_Height());
			val4.set_Texture(_cogWheelIcon);
			Image filterCog = val4;
			((Control)filterCog).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				filterCog.set_Texture(_cogWheelIconHover);
			});
			((Control)filterCog).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				filterCog.set_Texture(_cogWheelIcon);
			});
			((Control)filterCog).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				filterCog.set_Texture(_cogWheelIconClick);
			});
			((Control)filterCog).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				filterCog.set_Texture(_cogWheelIconHover);
			});
			ContextMenuStrip val5 = new ContextMenuStrip();
			((Control)val5).set_Parent(buildPanel);
			((Control)val5).set_ClipsBounds(false);
			ContextMenuStrip menu = val5;
			ContextMenuStripItem val6 = new ContextMenuStripItem(Resources.Show_Actives_Only);
			((Control)val6).set_Parent((Container)(object)menu);
			val6.set_CanCheck(true);
			val6.set_Checked(_config.ShowActivesOnly);
			ContextMenuStripItem showActivesOnly = val6;
			((Control)filterCog).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				GameService.Content.PlaySoundEffectByName("button-click");
				menu.Show(GameService.Input.get_Mouse().get_Position());
			});
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)panel);
			((Control)val7).set_Width(((Container)panel).get_ContentRegion().Width);
			((Control)val7).set_Height(((Container)panel).get_ContentRegion().Height - 35 - (int)panel.get_ControlPadding().Y * 2 - (int)panel.get_OuterControlPadding().Y);
			val7.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val7).set_CanScroll(true);
			FlowPanel macrosMenuWrap = val7;
			ViewContainer val8 = new ViewContainer();
			((Control)val8).set_Parent(buildPanel);
			((Control)val8).set_Width(buildPanel.get_ContentRegion().Width - 300);
			((Control)val8).set_Height(buildPanel.get_ContentRegion().Height);
			((Control)val8).set_Left(((Control)panel).get_Right());
			MacroConfig = val8;
			Menu val9 = new Menu();
			((Control)val9).set_Parent((Container)(object)macrosMenuWrap);
			((Control)val9).set_Width(((Container)macrosMenuWrap).get_ContentRegion().Width);
			((Control)val9).set_Height(((Container)macrosMenuWrap).get_ContentRegion().Height);
			val9.set_CanSelect(true);
			MacroEntries = val9;
			StandardButtonCustomFont standardButtonCustomFont = new StandardButtonCustomFont(ChatMacros.Instance.Resources.RubikRegular26);
			((Control)standardButtonCustomFont).set_Parent(buildPanel);
			((Control)standardButtonCustomFont).set_Width(((Control)panel).get_Width() - 20);
			((Control)standardButtonCustomFont).set_Height(50);
			((Control)standardButtonCustomFont).set_Top(((Control)panel).get_Bottom());
			((Control)standardButtonCustomFont).set_Left(10);
			((StandardButton)standardButtonCustomFont).set_Text(Resources.Create_Macro);
			StandardButtonCustomFont createNewBttn = standardButtonCustomFont;
			foreach (ChatMacro macro in ChatMacros.Instance.Data.GetAllMacros())
			{
				AddMacroEntry(MacroEntries, macro);
			}
			((Control)createNewBttn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ChatMacro chatMacro = new ChatMacro
				{
					Id = new ObjectId(),
					Title = Resources.New_Macro,
					Lines = new List<ChatLine>(),
					VoiceCommands = new List<string>()
				};
				if (!ChatMacros.Instance.Data.Upsert(chatMacro))
				{
					GameService.Content.PlaySoundEffectByName("error");
					ScreenNotification.ShowNotification(Resources.Something_went_wrong__Please_try_again_, (NotificationType)2, (Texture2D)null, 4);
				}
				else
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					ChatMacros.Instance.Speech.UpdateGrammar();
					AddMacroEntry(MacroEntries, chatMacro);
				}
			});
			((TextInputBase)searchBar).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				UpdateFilter(((TextInputBase)searchBar).get_Text(), showActivesOnly.get_Checked());
			});
			showActivesOnly.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
			{
				_config.ShowActivesOnly = e.get_Checked();
				UpdateFilter(((TextInputBase)searchBar).get_Text(), showActivesOnly.get_Checked());
				GameService.Content.PlaySoundEffectByName("color-change");
			});
			ChatMacros.Instance.Macro.ActiveMacrosChange += OnMacroOnActiveMacrosChange;
			((Control)MacroEntries).add_Disposed((EventHandler<EventArgs>)delegate
			{
				ChatMacros.Instance.Macro.ActiveMacrosChange -= OnMacroOnActiveMacrosChange;
			});
			((View<IPresenter>)this).Build(buildPanel);
			UpdateFilter(((TextInputBase)searchBar).get_Text(), showActivesOnly.get_Checked());
			void OnMacroOnActiveMacrosChange(object o, ValueEventArgs<IReadOnlyList<BaseMacro>> valueEventArgs)
			{
				UpdateFilter(((TextInputBase)searchBar).get_Text(), showActivesOnly.get_Checked());
			}
		}

		private void UpdateFilter(string searchKey, bool showActives)
		{
			List<MenuItem<ChatMacro>> entries = SortMacroMenuEntries(((IEnumerable)((Container)MacroEntries).get_Children()).Cast<MenuItem<ChatMacro>>()).ToList();
			List<MenuItem<ChatMacro>> filtered = FastenshteinUtil.FindMatchesBy(searchKey, entries, (MenuItem<ChatMacro> entry) => entry.Item.Title).ToList();
			foreach (MenuItem<ChatMacro> entry2 in entries)
			{
				entry2.IsActive = ChatMacros.Instance.Macro.ActiveMacros.Any((BaseMacro macro) => macro.Id.Equals(entry2.Item.Id));
				bool match = string.IsNullOrEmpty(searchKey) || filtered.IsNullOrEmpty() || filtered.Any((MenuItem<ChatMacro> x) => x.Item.Id.Equals(entry2.Item.Id));
				if (showActives)
				{
					((Control)entry2).set_Visible(entry2.IsActive && match);
				}
				else
				{
					((Control)entry2).set_Visible(match);
				}
				((Control)entry2).set_Enabled(((Control)entry2).get_Visible());
			}
			((Control)MacroEntries).Invalidate();
		}

		private IEnumerable<MenuItem<ChatMacro>> SortMacroMenuEntries(IEnumerable<MenuItem<ChatMacro>> toSort)
		{
			return from x in toSort
				orderby ChatMacros.Instance.LibraryConfig.get_Value().IndexChannelHistory(x.Item.Lines.FirstOrDefault()?.Channel ?? ChatChannel.Current), (x.Item.Lines?.FirstOrDefault()?.Channel).GetValueOrDefault(), x.Item.Title.ToLowerInvariant()
				select x;
		}

		private void AddMacroEntry(Menu parent, ChatMacro macro)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			MenuItem<ChatMacro> menuItem = new MenuItem<ChatMacro>(macro, (ChatMacro m) => m.Title, (ChatMacro m) => m.GetDisplayColor());
			((Control)menuItem).set_Parent((Container)(object)parent);
			((Control)menuItem).set_Width(((Container)parent).get_ContentRegion().Width);
			((Control)menuItem).set_Height(50);
			((MenuItem)menuItem).set_Text(string.IsNullOrEmpty(macro.Title) ? Resources.Enter_a_title___ : AssetUtil.Truncate(macro.Title, 195, GameService.Content.get_DefaultFont16()));
			((Control)menuItem).set_BasicTooltipText(macro.Title);
			MenuItem<ChatMacro> menuEntry = menuItem;
			menuEntry.DeleteClick += delegate
			{
				if (ChatMacros.Instance.Data.Delete(macro))
				{
					((Control)menuEntry).Dispose();
					MacroConfig.Clear();
					ChatMacros.Instance.Speech.UpdateGrammar();
				}
			};
			((Control)menuEntry).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ChatMacros.Instance.Resources.PlayMenuItemClick();
				CreateMacroView();
			});
			void CreateMacroView()
			{
				ChatMacro i = ChatMacros.Instance.Data.GetChatMacro(macro.Id);
				if (i != null)
				{
					menuEntry.AssignItem(i, (ChatMacro x) => x.Title);
					MacroView view = new MacroView(i);
					view.TitleChanged += delegate(object _, ValueEventArgs<string> e)
					{
						((MenuItem)menuEntry).set_Text(string.IsNullOrEmpty(i.Title) ? Resources.Enter_a_title___ : AssetUtil.Truncate(e.get_Value(), 195, GameService.Content.get_DefaultFont16()));
						((Control)menuEntry).set_BasicTooltipText(e.get_Value());
					};
					MacroConfig.Show((IView)(object)view);
				}
			}
		}
	}
}
