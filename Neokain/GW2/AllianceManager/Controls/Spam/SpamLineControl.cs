using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Neokain.GW2.AllianceManager.Controls.Shared;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Services;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Enums;
using Neokain.GW2.WebClient.Models.Guilds;
using Neokain.GW2.WebClient.Models.Spams.SpamLines;

namespace Neokain.GW2.AllianceManager.Controls.Spam
{
	public class SpamLineControl : Panel
	{
		private readonly IFontService _fontService;

		private readonly SpamContext _spamContext;

		private StandardButton _upButton;

		private StandardButton _downButton;

		private StandardButton _deleteButton;

		private Dropdown _targetDropdown;

		private Label _targetLabel;

		private Dropdown _extraInfoDropdown;

		private TextBox _extraInfoTextBox;

		private Label _extraInfoLabel;

		private AutoHeightTextBox _textBox;

		private AutoWrappingLabel _label;

		private Label _validationLabel;

		private int _row1Right;

		private const int PADDING = 5;

		private const int BUTTON_WIDTH = 28;

		private const int BUTTON_HEIGHT = 24;

		private const int ROW_HEIGHT = 28;

		private const int DROPDOWN_WIDTH = 80;

		private const int EXTRA_INFO_WIDTH = 150;

		private const int RIGHT_MARGIN = 15;

		public SpamLineDto SpamLine { get; private set; }

		public bool EditMode { get; private set; }

		public ValidationResult CurrentValidation { get; private set; }

		public event EventHandler DeleteClicked;

		public event EventHandler UpClicked;

		public event EventHandler DownClicked;

		public event EventHandler<ValidationResult> ValidationChanged;

		public SpamLineControl(SpamLineDto line, bool editMode, IFontService fontService, SpamContext spamContext)
			: this()
		{
			SpamLine = line ?? throw new ArgumentNullException("line");
			EditMode = editMode;
			_fontService = fontService ?? throw new ArgumentNullException("fontService");
			_spamContext = spamContext ?? throw new ArgumentNullException("spamContext");
			BuildUI();
		}

		public void SetEditMode(bool editMode)
		{
			EditMode = editMode;
			BuildUI();
		}

		public void UpdateLine(SpamLineDto line)
		{
			SpamLine = line ?? throw new ArgumentNullException("line");
			BuildUI();
		}

		public SpamLineDto GetLine()
		{
			string text = ((!EditMode) ? _label?.Text : _textBox?.Text);
			object obj;
			if (!EditMode)
			{
				Label targetLabel = _targetLabel;
				obj = ((targetLabel != null) ? targetLabel.get_Text() : null);
			}
			else
			{
				Dropdown targetDropdown = _targetDropdown;
				obj = ((targetDropdown != null) ? targetDropdown.get_SelectedItem() : null);
			}
			string targetStr = (string)obj;
			ChatType target = ChatType.Guild;
			if (!string.IsNullOrEmpty(targetStr))
			{
				Enum.TryParse<ChatType>(targetStr, out target);
			}
			return new SpamLineDto
			{
				Id = SpamLine.Id,
				LineText = (text ?? string.Empty),
				Target = target,
				TargetInfo1 = GetTargetInfo1Value()
			};
		}

		private string GetTargetInfo1Value()
		{
			if (!EditMode)
			{
				return SpamLine.TargetInfo1;
			}
			Dropdown targetDropdown = _targetDropdown;
			string targetStr = ((targetDropdown != null) ? targetDropdown.get_SelectedItem() : null);
			if (string.IsNullOrEmpty(targetStr))
			{
				return null;
			}
			if (!Enum.TryParse<ChatType>(targetStr, out var chatType))
			{
				return null;
			}
			switch (chatType)
			{
			case ChatType.Guild:
				return GetSelectedGuildId();
			case ChatType.Alliance:
				if (_spamContext.ContextType == SpamContextType.Account)
				{
					return GetSelectedAllianceId();
				}
				return null;
			case ChatType.Whisper:
			case ChatType.Channel:
			{
				TextBox extraInfoTextBox = _extraInfoTextBox;
				if (extraInfoTextBox == null)
				{
					return null;
				}
				return ((TextInputBase)extraInfoTextBox).get_Text();
			}
			default:
				return null;
			}
		}

		private string GetSelectedGuildId()
		{
			Dropdown extraInfoDropdown = _extraInfoDropdown;
			if (((extraInfoDropdown != null) ? extraInfoDropdown.get_SelectedItem() : null) == null)
			{
				return null;
			}
			string selectedText = _extraInfoDropdown.get_SelectedItem();
			return (_spamContext.AvailableGuilds?.FirstOrDefault((GuildMembershipDto g) => "[" + g.GuildTag + "] " + g.GuildName == selectedText))?.GuildId.ToString();
		}

		private string GetSelectedAllianceId()
		{
			Dropdown extraInfoDropdown = _extraInfoDropdown;
			if (((extraInfoDropdown != null) ? extraInfoDropdown.get_SelectedItem() : null) == null)
			{
				return null;
			}
			string selectedText = _extraInfoDropdown.get_SelectedItem();
			return (_spamContext.AvailableAlliances?.FirstOrDefault((AllianceMembershipDto a) => "[" + a.AllianceTag + "] " + a.AllianceName == selectedText))?.AllianceId.ToString();
		}

		public override void RecalculateLayout()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Parent() != null)
			{
				int textWidth = ((Container)this).get_ContentRegion().Width - 10 - 15;
				if (textWidth < 120)
				{
					textWidth = 120;
				}
				if (_label != null)
				{
					_label.MaxWidth = textWidth;
					((Control)_label).set_Width(textWidth);
					((Control)_label).RecalculateLayout();
					int row2Top2 = 33;
					((Control)this).set_Height(row2Top2 + Math.Max(28, ((Control)_label).get_Height()) + 5);
				}
				if (_textBox != null)
				{
					((Control)_textBox).set_Width(textWidth);
					((Control)_textBox).RecalculateLayout();
					int row2Top = 33;
					((Control)this).set_Height(row2Top + Math.Max(28, ((Control)_textBox).get_Height()) + 5);
				}
				if (_validationLabel != null)
				{
					((Control)_validationLabel).set_Left(_row1Right + 5);
				}
			}
		}

		private void BuildUI()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Expected O, but got Unknown
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Expected O, but got Unknown
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Expected O, but got Unknown
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Expected O, but got Unknown
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0481: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Unknown result type (might be due to invalid IL or missing references)
			//IL_048d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Expected O, but got Unknown
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_052a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Expected O, but got Unknown
			//IL_059f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_061c: Unknown result type (might be due to invalid IL or missing references)
			foreach (Control item in ((Container)this).get_Children().ToList())
			{
				item.Dispose();
			}
			int left = 0;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Top(0);
			((Control)val).set_Left(left);
			((Control)val).set_Width(28);
			((Control)val).set_Height(24);
			val.set_Text("^");
			((Control)val).set_Enabled(EditMode);
			_upButton = val;
			((Control)_upButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (EditMode)
				{
					this.UpClicked?.Invoke(this, EventArgs.Empty);
				}
			});
			left += 33;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Top(0);
			((Control)val2).set_Left(left);
			((Control)val2).set_Width(28);
			((Control)val2).set_Height(24);
			val2.set_Text("v");
			((Control)val2).set_Enabled(EditMode);
			_downButton = val2;
			((Control)_downButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (EditMode)
				{
					this.DownClicked?.Invoke(this, EventArgs.Empty);
				}
			});
			left += 33;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Top(0);
			((Control)val3).set_Left(left);
			((Control)val3).set_Width(28);
			((Control)val3).set_Height(24);
			val3.set_Text("-");
			((Control)val3).set_Enabled(EditMode);
			_deleteButton = val3;
			((Control)_deleteButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (EditMode)
				{
					this.DeleteClicked?.Invoke(this, EventArgs.Empty);
				}
			});
			left += 33;
			if (EditMode)
			{
				Dropdown val4 = new Dropdown();
				((Control)val4).set_Parent((Container)(object)this);
				((Control)val4).set_Top(0);
				((Control)val4).set_Left(left);
				((Control)val4).set_Width(80);
				((Control)val4).set_Enabled(true);
				_targetDropdown = val4;
				foreach (ChatType chatType in Enum.GetValues(typeof(ChatType)).Cast<ChatType>())
				{
					if (ChatTypeValidationService.IsAllowedInContext(chatType, _spamContext.ContextType))
					{
						_targetDropdown.get_Items().Add(chatType.ToString());
					}
				}
				_targetDropdown.set_SelectedItem(SpamLine.Target.ToString());
				_targetDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnChatTypeChanged);
				left += 85;
			}
			else
			{
				Label val5 = new Label();
				((Control)val5).set_Parent((Container)(object)this);
				((Control)val5).set_Top(0);
				((Control)val5).set_Left(left);
				val5.set_AutoSizeWidth(true);
				((Control)val5).set_Height(24);
				val5.set_Text(SpamLine.Target.ToString());
				val5.set_Font((BitmapFont)(object)_fontService.DejaVuSansDefault);
				_targetLabel = val5;
				left += ((Control)_targetLabel).get_Width() + 5;
			}
			_row1Right = left;
			BuildExtraInfoField(left);
			if (EditMode)
			{
				Label val6 = new Label();
				((Control)val6).set_Parent((Container)(object)this);
				((Control)val6).set_Top(0);
				((Control)val6).set_Left(_row1Right + 5);
				val6.set_AutoSizeWidth(true);
				((Control)val6).set_Height(24);
				val6.set_Font((BitmapFont)(object)_fontService.DejaVuSansDefault);
				_validationLabel = val6;
			}
			int row2Top = 33;
			int textWidth = 500;
			if (textWidth < 120)
			{
				textWidth = 120;
			}
			if (EditMode)
			{
				AutoHeightTextBox autoHeightTextBox = new AutoHeightTextBox((BitmapFont)(object)_fontService.DejaVuSansDefault);
				((Control)autoHeightTextBox).set_Parent((Container)(object)this);
				((Control)autoHeightTextBox).set_Top(row2Top);
				((Control)autoHeightTextBox).set_Left(0);
				((Control)autoHeightTextBox).set_Width(textWidth);
				autoHeightTextBox.Text = SpamLine.LineText ?? string.Empty;
				_textBox = autoHeightTextBox;
				_textBox.TextChanged += OnTextChanged;
				_textBox.HeightChanged += OnTextBoxHeightChanged;
				UpdateValidation();
				((Control)this).set_Height(row2Top + Math.Max(28, ((Control)_textBox).get_Height()) + 5);
				return;
			}
			int viewTextWidth = ((((Container)this).get_ContentRegion().Width > 0) ? (((Container)this).get_ContentRegion().Width - 10 - 15) : textWidth);
			if (viewTextWidth < 120)
			{
				viewTextWidth = 120;
			}
			AutoWrappingLabel autoWrappingLabel = new AutoWrappingLabel();
			((Control)autoWrappingLabel).set_Parent((Container)(object)this);
			((Control)autoWrappingLabel).set_Top(row2Top);
			((Control)autoWrappingLabel).set_Left(0);
			autoWrappingLabel.MaxWidth = viewTextWidth;
			((Control)autoWrappingLabel).set_Width(viewTextWidth);
			autoWrappingLabel.Text = SpamLine.LineText ?? string.Empty;
			autoWrappingLabel.Font = (BitmapFont)(object)_fontService.DejaVuSansDefault;
			_label = autoWrappingLabel;
			Label targetLabel = _targetLabel;
			int displayRow1Right = ((targetLabel != null) ? ((Control)targetLabel).get_Right() : left);
			if (ChatTypeValidationService.RequiresExtraInfo(SpamLine.Target) && !string.IsNullOrEmpty(SpamLine.TargetInfo1))
			{
				Label val7 = new Label();
				((Control)val7).set_Parent((Container)(object)this);
				((Control)val7).set_Top(0);
				((Control)val7).set_Left(displayRow1Right + 5);
				val7.set_AutoSizeWidth(true);
				((Control)val7).set_Height(24);
				val7.set_Text(FormatTargetInfo1ForDisplay(SpamLine.Target, SpamLine.TargetInfo1));
				val7.set_Font((BitmapFont)(object)_fontService.DejaVuSansDefault);
				val7.set_TextColor(Color.get_LightGray());
				_extraInfoLabel = val7;
				displayRow1Right = ((Control)_extraInfoLabel).get_Right();
			}
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Top(0);
			((Control)val8).set_Left(displayRow1Right + 5);
			val8.set_AutoSizeWidth(true);
			((Control)val8).set_Height(24);
			val8.set_Font((BitmapFont)(object)_fontService.DejaVuSansDefault);
			_validationLabel = val8;
			var (min, max) = TextInterpolationService.CalculateMinMaxLength(SpamLine.LineText ?? string.Empty);
			if (min > 199)
			{
				_validationLabel.set_Text($"Too long: {FormatLengthRange(min, max)} (max {199})");
				_validationLabel.set_TextColor(Color.get_Red());
			}
			else if (max > 199)
			{
				_validationLabel.set_Text($"May exceed: {FormatLengthRange(min, max)} (max {199})");
				_validationLabel.set_TextColor(Color.get_Yellow());
			}
			else
			{
				_validationLabel.set_Text($"Length: {FormatLengthRange(min, max)} / {199}");
				_validationLabel.set_TextColor(Color.get_LightGreen());
			}
			((Control)this).set_Height(row2Top + Math.Max(28, ((Control)_label).get_Height()) + 5);
		}

		private void BuildExtraInfoField(int left)
		{
			if (!EditMode)
			{
				return;
			}
			Dropdown targetDropdown = _targetDropdown;
			string targetStr = ((targetDropdown != null) ? targetDropdown.get_SelectedItem() : null);
			if (string.IsNullOrEmpty(targetStr) || !Enum.TryParse<ChatType>(targetStr, out var chatType))
			{
				return;
			}
			Dropdown extraInfoDropdown = _extraInfoDropdown;
			if (extraInfoDropdown != null)
			{
				((Control)extraInfoDropdown).Dispose();
			}
			_extraInfoDropdown = null;
			TextBox extraInfoTextBox = _extraInfoTextBox;
			if (extraInfoTextBox != null)
			{
				((Control)extraInfoTextBox).Dispose();
			}
			_extraInfoTextBox = null;
			switch (chatType)
			{
			case ChatType.Guild:
				BuildGuildDropdown(left);
				break;
			case ChatType.Alliance:
				if (_spamContext.ContextType == SpamContextType.Account)
				{
					BuildAllianceDropdown(left);
				}
				break;
			case ChatType.Whisper:
				BuildWhisperTextBox(left);
				break;
			case ChatType.Channel:
				BuildChannelTextBox(left);
				break;
			}
		}

		private void BuildGuildDropdown(int left)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Top(0);
			((Control)val).set_Left(left);
			((Control)val).set_Width(150);
			((Control)val).set_Enabled(true);
			_extraInfoDropdown = val;
			IEnumerable<GuildMembershipDto> availableGuilds = _spamContext.AvailableGuilds;
			foreach (GuildMembershipDto guild2 in availableGuilds ?? Enumerable.Empty<GuildMembershipDto>())
			{
				_extraInfoDropdown.get_Items().Add("[" + guild2.GuildTag + "] " + guild2.GuildName);
			}
			if (!string.IsNullOrEmpty(SpamLine.TargetInfo1) && Guid.TryParse(SpamLine.TargetInfo1, out var guildId))
			{
				GuildMembershipDto guild = _spamContext.AvailableGuilds?.FirstOrDefault((GuildMembershipDto g) => g.GuildId == guildId);
				if (guild != null)
				{
					_extraInfoDropdown.set_SelectedItem("[" + guild.GuildTag + "] " + guild.GuildName);
				}
			}
			else if (_extraInfoDropdown.get_Items().Count > 0)
			{
				_extraInfoDropdown.set_SelectedItem(_extraInfoDropdown.get_Items()[0]);
			}
			if (_spamContext.ContextType == SpamContextType.Guild && _extraInfoDropdown.get_Items().Count == 1)
			{
				((Control)_extraInfoDropdown).set_Enabled(false);
			}
			_extraInfoDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnExtraInfoChanged);
			_row1Right = ((Control)_extraInfoDropdown).get_Right();
		}

		private void BuildAllianceDropdown(int left)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Top(0);
			((Control)val).set_Left(left);
			((Control)val).set_Width(150);
			((Control)val).set_Enabled(true);
			_extraInfoDropdown = val;
			IEnumerable<AllianceMembershipDto> availableAlliances = _spamContext.AvailableAlliances;
			foreach (AllianceMembershipDto alliance2 in availableAlliances ?? Enumerable.Empty<AllianceMembershipDto>())
			{
				_extraInfoDropdown.get_Items().Add("[" + alliance2.AllianceTag + "] " + alliance2.AllianceName);
			}
			if (!string.IsNullOrEmpty(SpamLine.TargetInfo1) && Guid.TryParse(SpamLine.TargetInfo1, out var allianceId))
			{
				AllianceMembershipDto alliance = _spamContext.AvailableAlliances?.FirstOrDefault((AllianceMembershipDto a) => a.AllianceId == allianceId);
				if (alliance != null)
				{
					_extraInfoDropdown.set_SelectedItem("[" + alliance.AllianceTag + "] " + alliance.AllianceName);
				}
			}
			else if (_extraInfoDropdown.get_Items().Count > 0)
			{
				_extraInfoDropdown.set_SelectedItem(_extraInfoDropdown.get_Items()[0]);
			}
			_extraInfoDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnExtraInfoChanged);
			_row1Right = ((Control)_extraInfoDropdown).get_Right();
		}

		private void BuildWhisperTextBox(int left)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Top(0);
			((Control)val).set_Left(left);
			((Control)val).set_Width(150);
			((TextInputBase)val).set_Text(SpamLine.TargetInfo1 ?? string.Empty);
			((TextInputBase)val).set_PlaceholderText("Player name");
			((TextInputBase)val).set_Font((BitmapFont)(object)_fontService.DejaVuSansDefault);
			_extraInfoTextBox = val;
			((TextInputBase)_extraInfoTextBox).add_TextChanged((EventHandler<EventArgs>)OnExtraInfoChanged);
			_row1Right = ((Control)_extraInfoTextBox).get_Right();
		}

		private void BuildChannelTextBox(int left)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Top(0);
			((Control)val).set_Left(left);
			((Control)val).set_Width(150);
			((TextInputBase)val).set_Text(SpamLine.TargetInfo1 ?? "/");
			((TextInputBase)val).set_PlaceholderText("/channel");
			((TextInputBase)val).set_Font((BitmapFont)(object)_fontService.DejaVuSansDefault);
			_extraInfoTextBox = val;
			((TextInputBase)_extraInfoTextBox).add_TextChanged((EventHandler<EventArgs>)OnExtraInfoChanged);
			_row1Right = ((Control)_extraInfoTextBox).get_Right();
		}

		private string FormatTargetInfo1ForDisplay(ChatType type, string targetInfo1)
		{
			if (string.IsNullOrEmpty(targetInfo1))
			{
				return string.Empty;
			}
			switch (type)
			{
			case ChatType.Guild:
			{
				if (Guid.TryParse(targetInfo1, out var guildId))
				{
					GuildMembershipDto guild = _spamContext.AvailableGuilds?.FirstOrDefault((GuildMembershipDto g) => g.GuildId == guildId);
					if (guild != null)
					{
						return "[" + guild.GuildTag + "]";
					}
				}
				return targetInfo1;
			}
			case ChatType.Alliance:
			{
				if (Guid.TryParse(targetInfo1, out var allianceId))
				{
					AllianceMembershipDto alliance = _spamContext.AvailableAlliances?.FirstOrDefault((AllianceMembershipDto a) => a.AllianceId == allianceId);
					if (alliance != null)
					{
						return "[" + alliance.AllianceTag + "]";
					}
				}
				return targetInfo1;
			}
			case ChatType.Whisper:
				return "-> " + targetInfo1;
			case ChatType.Channel:
				return targetInfo1;
			default:
				return targetInfo1;
			}
		}

		private void OnChatTypeChanged(object sender, ValueChangedEventArgs e)
		{
			BuildExtraInfoField(_row1Right = ((Control)_targetDropdown).get_Right() + 5);
			if (_validationLabel != null)
			{
				((Control)_validationLabel).set_Left(_row1Right + 5);
			}
			UpdateValidation();
		}

		private void OnExtraInfoChanged(object sender, EventArgs e)
		{
			UpdateValidation();
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			UpdateValidation();
		}

		private void OnTextBoxHeightChanged(object sender, EventArgs e)
		{
			((Control)this).Invalidate();
			Container parent = ((Control)this).get_Parent();
			if (parent != null)
			{
				((Control)parent).Invalidate();
			}
			Container parent2 = ((Control)this).get_Parent();
			if (parent2 != null)
			{
				Container parent3 = ((Control)parent2).get_Parent();
				if (parent3 != null)
				{
					((Control)parent3).Invalidate();
				}
			}
		}

		private void UpdateValidation()
		{
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			SpamLineDto line = GetLine();
			ValidationResult extraInfoValidation = ChatTypeValidationService.ValidateTargetInfo1(line.Target, line.TargetInfo1, _spamContext);
			if (extraInfoValidation.Status == ValidationStatus.Error)
			{
				CurrentValidation = extraInfoValidation;
			}
			else
			{
				var (min, max) = TextInterpolationService.CalculateMinMaxLength(line.LineText ?? string.Empty);
				if (min > 199)
				{
					CurrentValidation = new ValidationResult
					{
						Status = ValidationStatus.Error,
						Message = $"Too long: {FormatLengthRange(min, max)} (max {199})",
						MinLength = min,
						MaxLength = max
					};
				}
				else if (max > 199)
				{
					CurrentValidation = new ValidationResult
					{
						Status = ValidationStatus.Warning,
						Message = $"May exceed: {FormatLengthRange(min, max)} (max {199})",
						MinLength = min,
						MaxLength = max
					};
				}
				else
				{
					CurrentValidation = new ValidationResult
					{
						Status = ValidationStatus.Ok,
						Message = $"Length: {FormatLengthRange(min, max)} / {199}",
						MinLength = min,
						MaxLength = max
					};
				}
			}
			if (_validationLabel != null)
			{
				_validationLabel.set_Text(CurrentValidation.Message);
				_validationLabel.set_TextColor(GetValidationColor(CurrentValidation.Status));
			}
			this.ValidationChanged?.Invoke(this, CurrentValidation);
		}

		private static Color GetValidationColor(ValidationStatus status)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(status switch
			{
				ValidationStatus.Error => Color.get_Red(), 
				ValidationStatus.Warning => Color.get_Yellow(), 
				_ => Color.get_LightGreen(), 
			});
		}

		private static string FormatLengthRange(int min, int max)
		{
			if (min != max)
			{
				return $"{min}-{max}";
			}
			return $"{min}";
		}
	}
}
