using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CinemaModule.Models;
using CinemaModule.Services;
using CinemaModule.Settings;
using Microsoft.Xna.Framework;

namespace CinemaModule.UI.Windows.Dialogs
{
	public class StreamEditorWindow : SmallWindow
	{
		private const int TextBoxWidth = 350;

		private const int ButtonWidth = 100;

		private const int SourceButtonSize = 32;

		private const string UrlHelpText = "Supported formats:\n• Video files: MP4, MKV, AVI, WEBM\n• Live streams: M3U8, HLS, RTSP, RTMP\n• Radio/Audio: MP3, AAC, OGG streams\n• Local files: file:///C:/path/to/video.mp4";

		private const string YouTubeVideoHelpText = "Supported formats:\n• Full URL: https://www.youtube.com/watch?v=VIDEO_ID\n• Short URL: https://youtu.be/VIDEO_ID\n• Video ID: VIDEO_ID";

		private const string YouTubePlaylistHelpText = "Supported formats:\n• Playlist URL: https://www.youtube.com/playlist?list=PLxxxxxxxx\n• Playlist ID: PLxxxxxxxx\n• Channel ID: UCxxxxxxxx (shows latest uploads)";

		private readonly CinemaUserSettings _settings;

		private readonly TextureService _textureService;

		private SavedStream _stream;

		private bool _isNewStream;

		private string _tabId;

		private StreamSourceType _selectedSourceType;

		private TextBox _nameTextBox;

		private GlowButton _twitchButton;

		private GlowButton _urlButton;

		private GlowButton _youtubeVideoButton;

		private GlowButton _youtubePlaylistButton;

		private TextBox _valueTextBox;

		private Label _valueLabel;

		private Label _helpLabel;

		private StandardButton _saveButton;

		private StandardButton _deleteButton;

		public event EventHandler StreamSaved;

		public event EventHandler StreamDeleted;

		public StreamEditorWindow(CinemaUserSettings settings, TextureService textureService)
			: base("Add Source")
		{
			_settings = settings;
			_textureService = textureService;
			Initialize();
		}

		protected override void BuildContent()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_OuterControlPadding(new Vector2(10f, 10f));
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Panel)val).set_CanScroll(true);
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel panel = val;
			BuildNameSection((Container)(object)panel);
			BuildSourceTypeSection((Container)(object)panel);
			BuildValueSection((Container)(object)panel);
			BuildButtons((Container)(object)panel);
		}

		private void BuildNameSection(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Name");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Parent(parent);
			TextBox val2 = new TextBox();
			((Control)val2).set_Width(350);
			((TextInputBase)val2).set_PlaceholderText("Enter a name");
			((Control)val2).set_Parent(parent);
			_nameTextBox = val2;
		}

		private void BuildSourceTypeSection(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Source Type");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Parent(parent);
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)2);
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(8f, 0f));
			((Control)val2).set_Parent(parent);
			FlowPanel buttonPanel = val2;
			_twitchButton = CreateSourceButton((Container)(object)buttonPanel, _textureService.GetTwitchIcon(), "Twitch Channel", StreamSourceType.TwitchChannel);
			_urlButton = CreateSourceButton((Container)(object)buttonPanel, _textureService.GetVlcIcon(), "URL", StreamSourceType.Url);
			_youtubeVideoButton = CreateSourceButton((Container)(object)buttonPanel, _textureService.GetYoutubeIcon(), "YouTube Video", StreamSourceType.YouTubeVideo);
			_youtubePlaylistButton = CreateSourceButton((Container)(object)buttonPanel, _textureService.GetYoutubeIcon(), "YouTube Playlist", StreamSourceType.YouTubePlaylist);
		}

		private GlowButton CreateSourceButton(Container parent, AsyncTexture2D icon, string tooltip, StreamSourceType sourceType)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			GlowButton val = new GlowButton();
			val.set_Icon(icon);
			val.set_ToggleGlow(true);
			((Control)val).set_Size(new Point(32, 32));
			((Control)val).set_BasicTooltipText(tooltip);
			((Control)val).set_Parent(parent);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SelectSourceType(sourceType);
			});
			return val;
		}

		private void SelectSourceType(StreamSourceType sourceType)
		{
			_selectedSourceType = sourceType;
			_twitchButton.set_Checked(sourceType == StreamSourceType.TwitchChannel);
			_urlButton.set_Checked(sourceType == StreamSourceType.Url);
			_youtubeVideoButton.set_Checked(sourceType == StreamSourceType.YouTubeVideo);
			_youtubePlaylistButton.set_Checked(sourceType == StreamSourceType.YouTubePlaylist);
			OnSourceTypeChanged();
		}

		private void BuildValueSection(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Stream URL / Channel");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Parent(parent);
			_valueLabel = val;
			TextBox val2 = new TextBox();
			((Control)val2).set_Width(350);
			((TextInputBase)val2).set_PlaceholderText("Enter URL or Twitch channel name");
			((Control)val2).set_Parent(parent);
			_valueTextBox = val2;
			Label val3 = new Label();
			val3.set_Text("");
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			val3.set_TextColor(Color.get_White());
			((Control)val3).set_Parent(parent);
			_helpLabel = val3;
		}

		private void BuildButtons(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)2);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(10f, 0f));
			((Control)val).set_Parent(parent);
			FlowPanel buttonPanel = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("Save");
			((Control)val2).set_Width(100);
			((Control)val2).set_Parent((Container)(object)buttonPanel);
			_saveButton = val2;
			((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Save();
			});
			StandardButton val3 = new StandardButton();
			val3.set_Text("Delete");
			((Control)val3).set_Width(100);
			((Control)val3).set_Visible(false);
			((Control)val3).set_Parent((Container)(object)buttonPanel);
			_deleteButton = val3;
			((Control)_deleteButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Delete();
			});
			StandardButton val4 = new StandardButton();
			val4.set_Text("Cancel");
			((Control)val4).set_Width(100);
			((Control)val4).set_Parent((Container)(object)buttonPanel);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).Hide();
			});
		}

		private void OnSourceTypeChanged()
		{
			switch (_selectedSourceType)
			{
			case StreamSourceType.TwitchChannel:
				_valueLabel.set_Text("Channel Name");
				((TextInputBase)_valueTextBox).set_PlaceholderText("Channel name (e.g., phandrel)");
				_helpLabel.set_Text("");
				break;
			case StreamSourceType.YouTubeVideo:
				_valueLabel.set_Text("YouTube URL or Video ID");
				((TextInputBase)_valueTextBox).set_PlaceholderText("YouTube URL or video ID");
				_helpLabel.set_Text("Supported formats:\n• Full URL: https://www.youtube.com/watch?v=VIDEO_ID\n• Short URL: https://youtu.be/VIDEO_ID\n• Video ID: VIDEO_ID");
				break;
			case StreamSourceType.YouTubePlaylist:
				_valueLabel.set_Text("YouTube Playlist");
				((TextInputBase)_valueTextBox).set_PlaceholderText("Playlist URL or ID");
				_helpLabel.set_Text("Supported formats:\n• Playlist URL: https://www.youtube.com/playlist?list=PLxxxxxxxx\n• Playlist ID: PLxxxxxxxx\n• Channel ID: UCxxxxxxxx (shows latest uploads)");
				break;
			default:
				_valueLabel.set_Text("URL");
				((TextInputBase)_valueTextBox).set_PlaceholderText("Enter URL");
				_helpLabel.set_Text("Supported formats:\n• Video files: MP4, MKV, AVI, WEBM\n• Live streams: M3U8, HLS, RTSP, RTMP\n• Radio/Audio: MP3, AAC, OGG streams\n• Local files: file:///C:/path/to/video.mp4");
				break;
			}
		}

		public void OpenForNew(string tabId = null, StreamSourceType sourceType = StreamSourceType.TwitchChannel)
		{
			_isNewStream = true;
			_stream = new SavedStream();
			_tabId = tabId;
			((WindowBase2)this).set_Title("Add Source");
			((TextInputBase)_nameTextBox).set_Text("");
			SelectSourceType(sourceType);
			((TextInputBase)_valueTextBox).set_Text("");
			((Control)_saveButton).set_Visible(true);
			((Control)_deleteButton).set_Visible(false);
			OnSourceTypeChanged();
			((Control)this).Show();
		}

		public void OpenForEdit(SavedStream stream)
		{
			_isNewStream = false;
			_stream = stream;
			_tabId = stream.TabId;
			((WindowBase2)this).set_Title("Edit Source");
			((TextInputBase)_nameTextBox).set_Text(stream.Name ?? "");
			SelectSourceType(stream.SourceType);
			((TextInputBase)_valueTextBox).set_Text(stream.Value ?? "");
			((Control)_saveButton).set_Visible(true);
			((Control)_deleteButton).set_Visible(true);
			OnSourceTypeChanged();
			((Control)this).Show();
		}

		private void Save()
		{
			string name = ((TextInputBase)_nameTextBox).get_Text().Trim();
			string value = ((TextInputBase)_valueTextBox).get_Text().Trim();
			if (!string.IsNullOrWhiteSpace(value))
			{
				if (string.IsNullOrWhiteSpace(name))
				{
					name = value;
				}
				StreamSourceType sourceType = _selectedSourceType;
				if (_isNewStream)
				{
					_settings.AddSavedStream(name, sourceType, value, _tabId);
				}
				else
				{
					_stream.Name = name;
					_stream.SourceType = sourceType;
					_stream.Value = value;
					_settings.UpdateSavedStream(_stream);
				}
				this.StreamSaved?.Invoke(this, EventArgs.Empty);
				((Control)this).Hide();
			}
		}

		private void Delete()
		{
			if (_stream != null && !_isNewStream)
			{
				_settings.DeleteSavedStream(_stream.Id);
				this.StreamDeleted?.Invoke(this, EventArgs.Empty);
				((Control)this).Hide();
			}
		}
	}
}
