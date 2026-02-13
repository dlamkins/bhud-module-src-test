using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using CinemaHUD.UI.Windows.SettingsSmall;
using CinemaModule;
using CinemaModule.Models;
using CinemaModule.Services;
using CinemaModule.Settings;
using Microsoft.Xna.Framework;

namespace CinemaHUD.UI.Windows.MainSettings
{
	public class SourceTabView : View
	{
		private enum StreamListItemType
		{
			Preset,
			PresetTwitch,
			Saved
		}

		private class StreamListItem
		{
			public string Key { get; set; }

			public string Title { get; set; }

			public string Subtitle { get; set; }

			public Color SubtitleColor { get; set; } = Color.get_Gray();


			public AsyncTexture2D AvatarTexture { get; set; }

			public AsyncTexture2D IconTexture { get; set; }

			public StreamListItemType ItemType { get; set; }

			public StreamPresetData PresetData { get; set; }

			public string TwitchChannel { get; set; }

			public string AvatarUrl { get; set; }

			public SavedStream SavedStream { get; set; }

			public bool IsOnline { get; set; }
		}

		private static readonly Logger Logger = Logger.GetLogger<SourceTabView>();

		private const int SavedStreamTextPanelWidth = 300;

		private const int PanelLeftPadding = 55;

		private const int ControlVerticalSpacing = 10;

		private const int CardVerticalSpacing = 4;

		private const string KeyPrefixPreset = "preset:";

		private const string KeyPrefixPresetTwitch = "preset_twitch:";

		private const string KeyPrefixSaved = "saved:";

		private static readonly Color AvailableColor = new Color(100, 200, 100);

		private readonly CinemaUserSettings _settings;

		private readonly CinemaController _controller;

		private readonly TwitchService _twitchService;

		private readonly PresetService _presetService;

		private FlowPanel _streamContainer;

		private Dictionary<string, ListCard> _streamCards = new Dictionary<string, ListCard>();

		private string _selectedStreamKey;

		private StreamEditorWindow _editorWindow;

		private CancellationTokenSource _cts;

		private CancellationTokenSource _rebuildCts;

		private EventHandler _savedStreamsChangedHandler;

		private EventHandler _presetsLoadedHandler;

		public SourceTabView(CinemaUserSettings settings, CinemaController controller, TwitchService twitchService, PresetService presetService)
			: this()
		{
			_settings = settings;
			_controller = controller;
			_twitchService = twitchService;
			_presetService = presetService;
			_cts = new CancellationTokenSource();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			InitializeSelectedStreamKey();
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_OuterControlPadding(new Vector2(55f, 0f));
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Control)val).set_Parent(buildPanel);
			FlowPanel panel = val;
			BuildToolbarButtons(buildPanel);
			BuildStreamSection((Container)(object)panel);
			_savedStreamsChangedHandler = delegate
			{
				RebuildStreamList();
			};
			_settings.SavedStreamsChanged += _savedStreamsChangedHandler;
			_presetsLoadedHandler = delegate
			{
				RebuildStreamList();
			};
			_presetService.PresetsLoaded += _presetsLoadedHandler;
		}

		private void BuildToolbarButtons(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			val.set_Text("+ Add New");
			((Control)val).set_Width(100);
			((Control)val).set_Left(405);
			((Control)val).set_Parent(parent);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenEditorForNew();
			});
		}

		private void BuildStreamSection(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Select stream");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Parent(parent);
			Panel val2 = new Panel();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Height(6);
			((Control)val2).set_Parent(parent);
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Control)val3).set_Height(500);
			val3.set_ControlPadding(new Vector2(0f, 4f));
			((Panel)val3).set_CanScroll(true);
			((Control)val3).set_Parent(parent);
			_streamContainer = val3;
			RebuildStreamList();
		}

		private void RebuildStreamList()
		{
			_rebuildCts?.Cancel();
			_rebuildCts?.Dispose();
			_rebuildCts = new CancellationTokenSource();
			FlowPanel streamContainer = _streamContainer;
			if (streamContainer != null)
			{
				((Container)streamContainer).ClearChildren();
			}
			_streamCards.Clear();
			InitializeSelectedStreamKey();
			RebuildStreamListAsync(_rebuildCts.Token);
		}

		private async void RebuildStreamListAsync(CancellationToken rebuildToken)
		{
			List<StreamListItem> items = BuildStreamItems();
			if (rebuildToken.IsCancellationRequested || _cts.IsCancellationRequested)
			{
				return;
			}
			await FetchStreamStatusesAsync(items, rebuildToken);
			if (rebuildToken.IsCancellationRequested || _cts.IsCancellationRequested)
			{
				return;
			}
			List<StreamListItem> sortedItems = items.OrderByDescending((StreamListItem i) => i.IsOnline).ToList();
			foreach (StreamListItem item in sortedItems)
			{
				if (rebuildToken.IsCancellationRequested)
				{
					return;
				}
				CreateCardForItem(item);
			}
			LoadAvatarsAsync(sortedItems, rebuildToken);
		}

		private List<StreamListItem> BuildStreamItems()
		{
			List<StreamListItem> items = new List<StreamListItem>();
			HashSet<string> addedTwitchChannels = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (StreamPresetData preset in _presetService.StreamPresets)
			{
				StreamListItem item3 = new StreamListItem
				{
					Key = GetPresetKey(preset.Id),
					Title = preset.Name,
					Subtitle = (string.IsNullOrEmpty(preset.Url) ? "No URL configured" : string.Empty),
					ItemType = StreamListItemType.Preset,
					PresetData = preset,
					AvatarTexture = global::CinemaModule.CinemaModule.Instance.TextureService.GetDefaultAvatar()
				};
				items.Add(item3);
			}
			foreach (string channel in _presetService.TwitchChannels)
			{
				StreamListItem item2 = new StreamListItem
				{
					Key = GetPresetTwitchKey(channel),
					Title = channel,
					Subtitle = string.Empty,
					ItemType = StreamListItemType.PresetTwitch,
					TwitchChannel = channel,
					IconTexture = global::CinemaModule.CinemaModule.Instance.TextureService.GetTwitchIcon(),
					AvatarTexture = global::CinemaModule.CinemaModule.Instance.TextureService.GetDefaultAvatar()
				};
				items.Add(item2);
				addedTwitchChannels.Add(channel);
			}
			foreach (SavedStream stream in _settings.SavedStreams.Streams)
			{
				bool isTwitch = stream.SourceType == StreamSourceType.TwitchChannel;
				if (isTwitch && addedTwitchChannels.Contains(stream.Value))
				{
					Logger.Debug("Skipping duplicate Twitch channel: " + stream.Value);
					continue;
				}
				StreamListItem item = new StreamListItem
				{
					Key = GetSavedStreamKey(stream.Id),
					Title = stream.Name,
					Subtitle = string.Empty,
					ItemType = StreamListItemType.Saved,
					SavedStream = stream,
					TwitchChannel = (isTwitch ? stream.Value : null),
					IconTexture = (isTwitch ? global::CinemaModule.CinemaModule.Instance.TextureService.GetTwitchIcon() : null),
					AvatarTexture = global::CinemaModule.CinemaModule.Instance.TextureService.GetDefaultAvatar()
				};
				items.Add(item);
				if (isTwitch)
				{
					addedTwitchChannels.Add(stream.Value);
				}
			}
			return items;
		}

		private async Task FetchStreamStatusesAsync(List<StreamListItem> items, CancellationToken rebuildToken)
		{
			List<Task> tasks = new List<Task>();
			List<string> twitchChannels = new List<string>();
			Dictionary<string, StreamListItem> twitchItems = new Dictionary<string, StreamListItem>(StringComparer.OrdinalIgnoreCase);
			foreach (StreamListItem item in items)
			{
				if (rebuildToken.IsCancellationRequested)
				{
					return;
				}
				switch (item.ItemType)
				{
				case StreamListItemType.Preset:
					if (!string.IsNullOrEmpty(item.PresetData.Url))
					{
						tasks.Add(FetchUrlStatusAsync(item.PresetData.Url, item, rebuildToken));
					}
					break;
				case StreamListItemType.PresetTwitch:
					twitchChannels.Add(item.TwitchChannel);
					twitchItems[item.TwitchChannel] = item;
					break;
				case StreamListItemType.Saved:
					if (item.SavedStream.SourceType == StreamSourceType.TwitchChannel)
					{
						twitchChannels.Add(item.SavedStream.Value);
						twitchItems[item.SavedStream.Value] = item;
					}
					else
					{
						tasks.Add(FetchUrlStatusAsync(item.SavedStream.Value, item, rebuildToken));
					}
					break;
				}
			}
			if (twitchChannels.Count > 0)
			{
				tasks.Add(FetchMultipleTwitchStatusAsync(twitchChannels, twitchItems, rebuildToken));
			}
			await Task.WhenAll(tasks);
		}

		private async Task FetchMultipleTwitchStatusAsync(List<string> channelNames, Dictionary<string, StreamListItem> itemsByChannel, CancellationToken rebuildToken)
		{
			try
			{
				Dictionary<string, TwitchStreamInfo> streamInfos = await _twitchService.GetMultipleStreamInfoAsync(channelNames);
				if (rebuildToken.IsCancellationRequested || _cts.IsCancellationRequested)
				{
					return;
				}
				foreach (KeyValuePair<string, TwitchStreamInfo> kvp in streamInfos)
				{
					string channelName2 = kvp.Key;
					TwitchStreamInfo streamInfo = kvp.Value;
					if (itemsByChannel.TryGetValue(channelName2, out var item2))
					{
						item2.IsOnline = streamInfo.IsLive;
						item2.AvatarUrl = streamInfo.AvatarUrl;
						if (streamInfo.IsLive)
						{
							item2.Subtitle = "@" + channelName2 + " - LIVE: " + (streamInfo.GameName ?? "Streaming");
							item2.SubtitleColor = AvailableColor;
						}
						else
						{
							item2.Subtitle = "@" + channelName2 + " - Offline";
							item2.SubtitleColor = Color.get_Gray();
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				if (rebuildToken.IsCancellationRequested || _cts.IsCancellationRequested)
				{
					return;
				}
				Logger.Warn(ex, "Failed to check status for multiple Twitch channels");
				foreach (string channelName in channelNames)
				{
					if (itemsByChannel.TryGetValue(channelName, out var item))
					{
						item.Subtitle = "@" + channelName + " - Status unknown";
						item.SubtitleColor = Color.get_Gray();
						item.IsOnline = false;
					}
				}
			}
		}

		private async Task UpdateAvatarAsync(string itemKey, string channelName, string avatarUrl, CancellationToken rebuildToken)
		{
			try
			{
				AsyncTexture2D avatarTexture = await _twitchService.GetAvatarTextureAsync(channelName, avatarUrl);
				if (!rebuildToken.IsCancellationRequested && !_cts.IsCancellationRequested && avatarTexture != null)
				{
					UpdateCardStatus(itemKey, null, null, avatarTexture);
				}
			}
			catch (Exception ex)
			{
				Logger.Debug("Failed to load avatar for " + channelName + ": " + ex.Message);
			}
		}

		private async Task FetchUrlStatusAsync(string url, StreamListItem item, CancellationToken rebuildToken)
		{
			try
			{
				UrlAvailabilityResult result = await _twitchService.CheckUrlAvailabilityAsync(url);
				if (!rebuildToken.IsCancellationRequested && !_cts.IsCancellationRequested)
				{
					item.IsOnline = result.IsAvailable.GetValueOrDefault();
					item.Subtitle = (item.IsOnline ? "LIVE" : "Offline");
					item.SubtitleColor = (item.IsOnline ? AvailableColor : Color.get_Gray());
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				if (!rebuildToken.IsCancellationRequested && !_cts.IsCancellationRequested)
				{
					Logger.Warn(ex, "Failed to check status for URL: " + url);
					item.Subtitle = "Offline";
					item.SubtitleColor = Color.get_Gray();
					item.IsOnline = false;
				}
			}
		}

		private async Task LoadAvatarsAsync(List<StreamListItem> items, CancellationToken rebuildToken)
		{
			List<Task> avatarTasks = new List<Task>();
			foreach (StreamListItem item in items)
			{
				if (rebuildToken.IsCancellationRequested)
				{
					return;
				}
				string avatarUrl = item.AvatarUrl ?? item.PresetData?.Avatar;
				string cacheKey = item.TwitchChannel ?? item.PresetData?.Id;
				if (!string.IsNullOrEmpty(avatarUrl) && !string.IsNullOrEmpty(cacheKey))
				{
					avatarTasks.Add(UpdateAvatarAsync(item.Key, cacheKey, avatarUrl, rebuildToken));
				}
			}
			await Task.WhenAll(avatarTasks);
		}

		private void UpdateCardStatus(string key, string subtitle, Color? subtitleColor, AsyncTexture2D avatarTexture)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (_streamCards.TryGetValue(key, out var card))
			{
				if (subtitle != null && subtitleColor.HasValue)
				{
					card.SetSubtitle(subtitle, subtitleColor.Value);
				}
				if (avatarTexture != null)
				{
					card.SetAvatar(avatarTexture);
				}
			}
		}

		private void CreateCardForItem(StreamListItem item)
		{
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<ListCardButton> buttons = null;
			int textPanelWidth = 400;
			if (item.ItemType == StreamListItemType.Saved)
			{
				textPanelWidth = 300;
				buttons = new List<ListCardButton>
				{
					new ListCardButton
					{
						Text = "X",
						Width = 30,
						OnClick = delegate
						{
							_settings.DeleteSavedStream(item.SavedStream.Id);
						}
					},
					new ListCardButton
					{
						Text = "Edit",
						Width = 50,
						OnClick = delegate
						{
							OpenEditorForEdit(item.SavedStream);
						}
					}
				};
			}
			else if (item.ItemType == StreamListItemType.Preset && !string.IsNullOrEmpty(item.PresetData?.InfoUrl))
			{
				textPanelWidth = 300;
				string infoUrl = item.PresetData.InfoUrl;
				buttons = new List<ListCardButton>
				{
					new ListCardButton
					{
						Text = "Info",
						Width = 50,
						OnClick = delegate
						{
							OpenUrlInBrowser(infoUrl);
						}
					}
				};
			}
			else if (item.ItemType == StreamListItemType.PresetTwitch && !string.IsNullOrEmpty(item.TwitchChannel))
			{
				textPanelWidth = 300;
				string twitchUrl = _twitchService.GetChannelUrl(item.TwitchChannel);
				buttons = new List<ListCardButton>
				{
					new ListCardButton
					{
						Text = "Info",
						Width = 50,
						OnClick = delegate
						{
							OpenUrlInBrowser(twitchUrl);
						}
					}
				};
			}
			ListCard card = CreateCard(item.Key, item.Title, item.Subtitle, textPanelWidth, buttons, item.IconTexture);
			card.SetSubtitle(item.Subtitle, item.SubtitleColor);
			if (item.AvatarTexture != null)
			{
				card.SetAvatar(item.AvatarTexture);
			}
			switch (item.ItemType)
			{
			case StreamListItemType.Preset:
				((Control)card).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectPresetStream(item.PresetData, item.Key);
				});
				break;
			case StreamListItemType.PresetTwitch:
				((Control)card).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectTwitchChannelAsync(item.TwitchChannel, item.Key);
				});
				break;
			case StreamListItemType.Saved:
				((Control)card).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectSavedStreamAsync(item.SavedStream);
				});
				break;
			}
		}

		private void SelectPresetStream(StreamPresetData preset, string key)
		{
			_selectedStreamKey = key;
			_settings.SelectedSavedStreamId = "";
			_settings.CurrentTwitchChannel = "";
			_settings.CurrentStreamSourceType = StreamSourceType.Url;
			_settings.StreamUrl = preset.Url;
			UpdateCardSelection();
		}

		private async void SelectTwitchChannelAsync(string channelName, string key)
		{
			_selectedStreamKey = key;
			_settings.SelectedSavedStreamId = "";
			_settings.CurrentTwitchChannel = channelName;
			_settings.CurrentStreamSourceType = StreamSourceType.TwitchChannel;
			UpdateCardSelection();
			await TrySetStreamUrlAsync(() => _twitchService.GetPlayableStreamUrlAsync(channelName), "Twitch channel: " + channelName);
		}

		private async void SelectSavedStreamAsync(SavedStream stream)
		{
			_selectedStreamKey = GetSavedStreamKey(stream.Id);
			_controller.SelectSavedStream(stream.Id);
			UpdateCardSelection();
			if (stream.SourceType == StreamSourceType.TwitchChannel)
			{
				await TrySetStreamUrlAsync(() => _twitchService.GetPlayableStreamUrlAsync(stream.Value), "stream: " + stream.Name);
			}
			else
			{
				_settings.StreamUrl = stream.Value;
				Logger.Info("Selected stream: " + stream.Name);
			}
		}

		private void OpenEditorForNew()
		{
			EnsureEditorWindow();
			_editorWindow.OpenForNew();
		}

		private void OpenEditorForEdit(SavedStream stream)
		{
			EnsureEditorWindow();
			_editorWindow.OpenForEdit(stream);
		}

		private void OpenUrlInBrowser(string url)
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
				Logger.Warn(ex, "Failed to open URL in browser: " + url);
			}
		}

		private void EnsureEditorWindow()
		{
			if (_editorWindow == null)
			{
				_editorWindow = new StreamEditorWindow(_settings);
				_editorWindow.StreamSaved += delegate
				{
					RebuildStreamList();
				};
				_editorWindow.StreamDeleted += delegate
				{
					RebuildStreamList();
				};
			}
		}

		private void UpdateCardSelection()
		{
			foreach (KeyValuePair<string, ListCard> kvp in _streamCards)
			{
				kvp.Value.IsSelected = kvp.Key == _selectedStreamKey;
			}
		}

		private void InitializeSelectedStreamKey()
		{
			if (!string.IsNullOrEmpty(_settings.SelectedSavedStreamId))
			{
				_selectedStreamKey = GetSavedStreamKey(_settings.SelectedSavedStreamId);
			}
			else
			{
				if (_settings.CurrentStreamSourceType != StreamSourceType.TwitchChannel || string.IsNullOrEmpty(_settings.CurrentTwitchChannel))
				{
					return;
				}
				foreach (string channel in _presetService.TwitchChannels)
				{
					if (string.Equals(channel, _settings.CurrentTwitchChannel, StringComparison.OrdinalIgnoreCase))
					{
						_selectedStreamKey = GetPresetTwitchKey(channel);
						break;
					}
				}
			}
		}

		private static string GetPresetKey(string presetId)
		{
			return "preset:" + presetId;
		}

		private static string GetPresetTwitchKey(string channel)
		{
			return "preset_twitch:" + channel;
		}

		private static string GetSavedStreamKey(string id)
		{
			return "saved:" + id;
		}

		private ListCard CreateCard(string key, string title, string subtitle, int textPanelWidth = 400, IEnumerable<ListCardButton> buttons = null, AsyncTexture2D iconTexture = null)
		{
			bool isSelected = _selectedStreamKey == key;
			ListCard card = new ListCard((Container)(object)_streamContainer, title, subtitle, isSelected, textPanelWidth, buttons, null, iconTexture);
			_streamCards[key] = card;
			return card;
		}

		private async Task TrySetStreamUrlAsync(Func<Task<string>> getUrlAsync, string streamDescription)
		{
			try
			{
				string url = await getUrlAsync();
				if (!_cts.IsCancellationRequested && !string.IsNullOrEmpty(url))
				{
					_settings.StreamUrl = url;
					Logger.Info("Selected " + streamDescription);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				if (!_cts.IsCancellationRequested)
				{
					Logger.Error(ex, "Failed to get stream URL for " + streamDescription);
				}
			}
		}

		protected override void Unload()
		{
			_rebuildCts?.Cancel();
			_rebuildCts?.Dispose();
			_cts?.Cancel();
			_cts?.Dispose();
			_settings.SavedStreamsChanged -= _savedStreamsChangedHandler;
			_presetService.PresetsLoaded -= _presetsLoadedHandler;
			StreamEditorWindow editorWindow = _editorWindow;
			if (editorWindow != null)
			{
				((Control)editorWindow).Dispose();
			}
			((View<IPresenter>)this).Unload();
		}
	}
}
