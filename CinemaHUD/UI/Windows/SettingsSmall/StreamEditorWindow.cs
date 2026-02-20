using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CinemaModule.Models;
using CinemaModule.Settings;
using Microsoft.Xna.Framework;

namespace CinemaHUD.UI.Windows.SettingsSmall
{
	public class StreamEditorWindow : SmallWindow
	{
		private const int TextBoxWidth = 350;

		private const int DropdownWidth = 200;

		private const int ButtonWidth = 100;

		private const string SourceTypeTwitch = "Twitch Channel";

		private const string SourceTypeUrl = "URL";

		private const string UrlHelpText = "Supported formats:\n• Video files: MP4, MKV, AVI, WEBM\n• Live streams: M3U8, HLS, RTSP, RTMP\n• Radio/Audio: MP3, AAC, OGG streams\n• Local files: file:///C:/path/to/video.mp4";

		private readonly CinemaUserSettings _settings;

		private SavedStream _stream;

		private bool _isNewStream;

		private TextBox _nameTextBox;

		private Dropdown _sourceTypeDropdown;

		private TextBox _valueTextBox;

		private Label _valueLabel;

		private Label _helpLabel;

		private StandardButton _saveButton;

		private StandardButton _deleteButton;

		public event EventHandler StreamSaved;

		public event EventHandler StreamDeleted;

		public StreamEditorWindow(CinemaUserSettings settings)
			: base("Add Stream")
		{
			_settings = settings;
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
			val.set_Text("Stream Name");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Parent(parent);
			TextBox val2 = new TextBox();
			((Control)val2).set_Width(350);
			((TextInputBase)val2).set_PlaceholderText("Enter a name for this stream");
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
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Source Type");
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			val.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val).set_Parent(parent);
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Width(200);
			((Control)val2).set_Parent(parent);
			_sourceTypeDropdown = val2;
			_sourceTypeDropdown.get_Items().Add("URL");
			_sourceTypeDropdown.get_Items().Add("Twitch Channel");
			_sourceTypeDropdown.set_SelectedItem("Twitch Channel");
			_sourceTypeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				OnSourceTypeChanged();
			});
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
			val.set_FlowDirection((ControlFlowDirection)0);
			((Container)val).set_WidthSizingMode((SizingMode)2);
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
			bool isTwitch = _sourceTypeDropdown.get_SelectedItem() == "Twitch Channel";
			_valueLabel.set_Text(isTwitch ? "Channel Name" : "Stream URL");
			((TextInputBase)_valueTextBox).set_PlaceholderText(isTwitch ? "Channel name (e.g., phandrel)" : "Enter stream or video URL");
			_helpLabel.set_Text(isTwitch ? "" : "Supported formats:\n• Video files: MP4, MKV, AVI, WEBM\n• Live streams: M3U8, HLS, RTSP, RTMP\n• Radio/Audio: MP3, AAC, OGG streams\n• Local files: file:///C:/path/to/video.mp4");
		}

		public void OpenForNew(StreamSourceType sourceType = StreamSourceType.TwitchChannel)
		{
			_isNewStream = true;
			_stream = new SavedStream();
			((WindowBase2)this).set_Title("Add Stream");
			((TextInputBase)_nameTextBox).set_Text("");
			_sourceTypeDropdown.set_SelectedItem(GetDropdownValue(sourceType));
			((TextInputBase)_valueTextBox).set_Text("");
			((Control)_deleteButton).set_Visible(false);
			OnSourceTypeChanged();
			((Control)this).Show();
		}

		public void OpenForEdit(SavedStream stream)
		{
			_isNewStream = false;
			_stream = stream;
			((WindowBase2)this).set_Title("Edit Stream");
			((TextInputBase)_nameTextBox).set_Text(stream.Name ?? "");
			_sourceTypeDropdown.set_SelectedItem(GetDropdownValue(stream.SourceType));
			((TextInputBase)_valueTextBox).set_Text(stream.Value ?? "");
			((Control)_deleteButton).set_Visible(true);
			OnSourceTypeChanged();
			((Control)this).Show();
		}

		private string GetDropdownValue(StreamSourceType sourceType)
		{
			if (sourceType != StreamSourceType.TwitchChannel)
			{
				return "URL";
			}
			return "Twitch Channel";
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
				StreamSourceType sourceType = ((_sourceTypeDropdown.get_SelectedItem() == "Twitch Channel") ? StreamSourceType.TwitchChannel : StreamSourceType.Url);
				if (_isNewStream)
				{
					_settings.AddSavedStream(name, sourceType, value);
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
