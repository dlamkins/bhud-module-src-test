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
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Expected O, but got Unknown
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Expected O, but got Unknown
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Expected O, but got Unknown
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Expected O, but got Unknown
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Expected O, but got Unknown
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Expected O, but got Unknown
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_OuterControlPadding(new Vector2(10f, 10f));
			val.set_ControlPadding(new Vector2(0f, 10f));
			((Panel)val).set_CanScroll(true);
			((Control)val).set_Parent((Container)(object)this);
			FlowPanel panel = val;
			Label val2 = new Label();
			val2.set_Text("Stream Name");
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val2).set_Parent((Container)(object)panel);
			TextBox val3 = new TextBox();
			((Control)val3).set_Width(350);
			((TextInputBase)val3).set_PlaceholderText("Enter a name for this stream");
			((Control)val3).set_Parent((Container)(object)panel);
			_nameTextBox = val3;
			Label val4 = new Label();
			val4.set_Text("Source Type");
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			val4.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val4).set_Parent((Container)(object)panel);
			Dropdown val5 = new Dropdown();
			((Control)val5).set_Width(200);
			((Control)val5).set_Parent((Container)(object)panel);
			_sourceTypeDropdown = val5;
			_sourceTypeDropdown.get_Items().Add("URL");
			_sourceTypeDropdown.get_Items().Add("Twitch Channel");
			_sourceTypeDropdown.set_SelectedItem("Twitch Channel");
			_sourceTypeDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				OnSourceTypeChanged();
			});
			Label val6 = new Label();
			val6.set_Text("Stream URL / Channel");
			val6.set_AutoSizeHeight(true);
			val6.set_AutoSizeWidth(true);
			val6.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val6).set_Parent((Container)(object)panel);
			_valueLabel = val6;
			TextBox val7 = new TextBox();
			((Control)val7).set_Width(350);
			((TextInputBase)val7).set_PlaceholderText("Enter URL or Twitch channel name");
			((Control)val7).set_Parent((Container)(object)panel);
			_valueTextBox = val7;
			Label val8 = new Label();
			val8.set_Text("");
			val8.set_AutoSizeHeight(true);
			val8.set_AutoSizeWidth(true);
			val8.set_TextColor(Color.get_White());
			((Control)val8).set_Parent((Container)(object)panel);
			_helpLabel = val8;
			FlowPanel val9 = new FlowPanel();
			val9.set_FlowDirection((ControlFlowDirection)0);
			((Container)val9).set_WidthSizingMode((SizingMode)2);
			((Container)val9).set_HeightSizingMode((SizingMode)1);
			val9.set_ControlPadding(new Vector2(10f, 0f));
			((Control)val9).set_Parent((Container)(object)panel);
			FlowPanel buttonPanel = val9;
			StandardButton val10 = new StandardButton();
			val10.set_Text("Save");
			((Control)val10).set_Width(100);
			((Control)val10).set_Parent((Container)(object)buttonPanel);
			_saveButton = val10;
			((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Save();
			});
			StandardButton val11 = new StandardButton();
			val11.set_Text("Delete");
			((Control)val11).set_Width(100);
			((Control)val11).set_Visible(false);
			((Control)val11).set_Parent((Container)(object)buttonPanel);
			_deleteButton = val11;
			((Control)_deleteButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Delete();
			});
			StandardButton val12 = new StandardButton();
			val12.set_Text("Cancel");
			((Control)val12).set_Width(100);
			((Control)val12).set_Parent((Container)(object)buttonPanel);
			((Control)val12).add_Click((EventHandler<MouseEventArgs>)delegate
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
			_sourceTypeDropdown.set_SelectedItem((sourceType == StreamSourceType.TwitchChannel) ? "Twitch Channel" : "URL");
			((TextInputBase)_valueTextBox).set_Text("");
			((Control)_deleteButton).set_Visible(false);
			OnSourceTypeChanged();
			((Control)this).Show();
		}

		public void OpenForEdit(SavedStream stream)
		{
			if (stream != null)
			{
				_isNewStream = false;
				_stream = stream;
				((WindowBase2)this).set_Title("Edit Stream");
				((TextInputBase)_nameTextBox).set_Text(stream.Name ?? "");
				_sourceTypeDropdown.set_SelectedItem((stream.SourceType == StreamSourceType.TwitchChannel) ? "Twitch Channel" : "URL");
				((TextInputBase)_valueTextBox).set_Text(stream.Value ?? "");
				((Control)_deleteButton).set_Visible(true);
				OnSourceTypeChanged();
				((Control)this).Show();
			}
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
