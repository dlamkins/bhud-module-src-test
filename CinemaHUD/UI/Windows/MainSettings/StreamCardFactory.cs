using System;
using System.Collections.Generic;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CinemaModule.Models;
using CinemaModule.Services;

namespace CinemaHUD.UI.Windows.MainSettings
{
	public class StreamCardFactory
	{
		private const int CardTextPanelWidth = 220;

		private readonly TextureService _textureService;

		private readonly TwitchService _twitchService;

		private readonly Func<string> _getSelectedKey;

		private readonly Dictionary<string, ListCard> _streamCards;

		public Action<string> OnOpenChat { get; set; }

		public Action<string> OnCopyWaypoint { get; set; }

		public Action<ChannelData> OnApplyWorldPosition { get; set; }

		public StreamCardFactory(TextureService textureService, TwitchService twitchService, Func<string> getSelectedKey, Dictionary<string, ListCard> streamCards)
		{
			_textureService = textureService;
			_twitchService = twitchService;
			_getSelectedKey = getSelectedKey;
			_streamCards = streamCards;
		}

		public ListCard CreateFollowedCard(FlowPanel parent, string key, TwitchStreamInfo stream, Action<string, string> onSelect)
		{
			List<ListCardButton> buttons = CreateTwitchButtons(stream.ChannelName);
			StreamStatus status = StreamStatus.Live(stream.GameName, stream.ViewerCount);
			ListCard listCard = CreateCard(parent, key, stream.ChannelName, status, buttons, _textureService.GetDefaultAvatar());
			((Control)listCard).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				onSelect(stream.ChannelName, key);
			});
			return listCard;
		}

		public ListCard CreateTwitchCard(FlowPanel parent, StreamListItem item, Action<string, string> onSelect)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			List<ListCardButton> buttons = CreateTwitchButtons(item.TwitchChannel);
			StreamStatus status = new StreamStatus
			{
				Subtitle = item.Subtitle,
				SubtitleColor = item.SubtitleColor
			};
			ListCard listCard = CreateCard(parent, item.Key, item.Title, status, buttons, item.AvatarTexture);
			((Control)listCard).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				onSelect(item.TwitchChannel, item.Key);
			});
			return listCard;
		}

		public ListCard CreateChannelCard(FlowPanel parent, StreamListItem item, Action<ChannelData, string> onSelect)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			List<ListCardButton> buttons = CreateChannelButtons(item.ChannelData);
			StreamStatus status = new StreamStatus
			{
				Subtitle = item.Subtitle,
				SubtitleColor = item.SubtitleColor
			};
			ListCard listCard = CreateCard(parent, item.Key, item.Title, status, buttons, item.AvatarTexture);
			((Control)listCard).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				onSelect(item.ChannelData, item.Key);
			});
			return listCard;
		}

		public ListCard CreateCustomCard(FlowPanel parent, string key, SavedStream stream, StreamStatus status, Action onDelete, Action onEdit, Action<SavedStream> onSelect)
		{
			List<ListCardButton> buttons = CreateCustomButtons(stream, onDelete, onEdit);
			StreamStatus effectiveStatus = status ?? new StreamStatus
			{
				Subtitle = stream.Value
			};
			ListCard listCard = CreateCard(parent, key, stream.Name, effectiveStatus, buttons, _textureService.GetDefaultAvatar());
			((Control)listCard).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				onSelect(stream);
			});
			return listCard;
		}

		private ListCard CreateCard(FlowPanel parent, string key, string title, StreamStatus status, List<ListCardButton> buttons, AsyncTexture2D avatar)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			string subtitle = status.Subtitle ?? string.Empty;
			ListCard card = new ListCard((Container)(object)parent, title, subtitle, _getSelectedKey() == key, 220, buttons, avatar);
			card.SetSubtitle(subtitle, status.SubtitleColor);
			_streamCards[key] = card;
			return card;
		}

		private List<ListCardButton> CreateTwitchButtons(string channelName)
		{
			string twitchUrl = _twitchService.GetChannelUrl(channelName);
			return new List<ListCardButton>
			{
				CreateIconButton(_textureService.GetInfoIcon(), "Open in Browser", delegate
				{
					OpenUrl(twitchUrl);
				}),
				CreateIconButton(_textureService.GetTwitchChatIcon(), "Open Chat", delegate
				{
					OnOpenChat?.Invoke(channelName);
				})
			};
		}

		private List<ListCardButton> CreateChannelButtons(ChannelData channel)
		{
			List<ListCardButton> buttons = new List<ListCardButton>();
			if (!string.IsNullOrEmpty(channel?.InfoUrl))
			{
				buttons.Add(CreateIconButton(_textureService.GetInfoIcon(), "Open in Browser", delegate
				{
					OpenUrl(channel.InfoUrl);
				}));
			}
			if (!string.IsNullOrEmpty(channel?.YoutubeUrl))
			{
				buttons.Add(CreateIconButton(_textureService.GetYoutubeIcon(), "Watch on YouTube", delegate
				{
					OpenUrl(channel.YoutubeUrl);
				}));
			}
			if (!string.IsNullOrEmpty(channel?.Waypoint))
			{
				buttons.Add(CreateIconButton(_textureService.GetWaypointIcon(), "Copy Waypoint", delegate
				{
					OnCopyWaypoint?.Invoke(channel.Waypoint);
				}));
			}
			if (channel?.HasWorldPosition ?? false)
			{
				buttons.Add(CreateIconButton(_textureService.GetSetScreenIcon(), "Set Ingame Screen Position", delegate
				{
					OnApplyWorldPosition?.Invoke(channel);
				}));
			}
			return buttons;
		}

		private List<ListCardButton> CreateCustomButtons(SavedStream stream, Action onDelete, Action onEdit)
		{
			List<ListCardButton> buttons = new List<ListCardButton> { CreateIconButton(_textureService.GetDeleteIcon(), "Delete", onDelete) };
			if (stream.SourceType == StreamSourceType.TwitchChannel)
			{
				buttons.Add(CreateIconButton(_textureService.GetTwitchChatIcon(), "Open Chat", delegate
				{
					OnOpenChat?.Invoke(stream.Value);
				}));
			}
			buttons.Add(new ListCardButton
			{
				Text = "Edit",
				Width = 50,
				OnClick = onEdit
			});
			return buttons;
		}

		private ListCardButton CreateIconButton(AsyncTexture2D icon, string tooltip, Action onClick)
		{
			return new ListCardButton
			{
				Text = "",
				Width = 30,
				Icon = icon,
				Tooltip = tooltip,
				OnClick = onClick
			};
		}

		private void OpenUrl(string url)
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = url,
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				Logger.GetLogger<StreamCardFactory>().Debug("Failed to open URL: " + ex.Message);
			}
		}
	}
}
