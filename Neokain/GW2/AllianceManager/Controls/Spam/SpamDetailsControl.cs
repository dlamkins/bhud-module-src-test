using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.AllianceManager.Services;
using Neokain.GW2.AllianceManager.Windows;
using Neokain.GW2.WebClient.Models.Enums;
using Neokain.GW2.WebClient.Models.Spams;
using Neokain.GW2.WebClient.Models.Spams.SpamLines;

namespace Neokain.GW2.AllianceManager.Controls.Spam
{
	public class SpamDetailsControl : Panel
	{
		private readonly SpamClient _spamClient;

		private readonly SpamSource _source;

		private readonly Module _module;

		private readonly SpamContext _spamContext;

		private SpamDetailDto _spam;

		private readonly FlowPanel _panelButtons;

		private readonly StandardButton _buttonEdit;

		private readonly StandardButton _buttonNew;

		private readonly StandardButton _buttonCancel;

		private readonly StandardButton _buttonDelete;

		private readonly Label _labelForName;

		private readonly Label _labelName;

		private readonly TextBox _textBoxName;

		private readonly Label _labelForDescription;

		private readonly Label _labelDescription;

		private readonly TextBox _textBoxDescription;

		private readonly Label _labelForLastSpam;

		private readonly Label _labelLastSpam;

		private readonly Label _labelForCooldown;

		private readonly Label _labelCooldown;

		private readonly TextBox _textBoxCooldown;

		private readonly Label _labelForCooldownStatus;

		private readonly Label _labelCooldownStatus;

		private readonly Label _labelForCategory;

		private readonly Label _labelCategory;

		private readonly Dropdown _dropdownCategory;

		private List<SpamCategoryDto> _categories = new List<SpamCategoryDto>();

		private readonly Label _labelForCreatedAt;

		private readonly Label _labelCreatedAt;

		private readonly Label _labelForLastModified;

		private readonly Label _labelLastModified;

		private readonly SpamLinesControl _spamLinesControl;

		private readonly StandardButton _buttonSpam;

		private readonly StandardButton _buttonMapHistory;

		private readonly StandardButton _buttonUsageLog;

		private readonly StandardButton _buttonFavorite;

		private bool _isFavorited;

		private bool _editMode;

		private bool _initialized;

		private bool _isNewSpam;

		private TimeSpan? _cooldownDuration;

		private Timer _updateTimer;

		private bool _hasValidationError;

		private const int CTRL_PADDING = 5;

		public event EventHandler EditingStopped;

		public event EventHandler EditingStarted;

		public event EventHandler<Guid> SpamUseRequested;

		public event EventHandler<(Guid SpamId, bool AddToFavorites)> FavoriteToggleRequested;

		public SpamDetailsControl(SpamClient spamClient, SpamSource source, Module module, SpamContext spamContext)
			: this()
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected O, but got Unknown
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Expected O, but got Unknown
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Expected O, but got Unknown
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Expected O, but got Unknown
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Expected O, but got Unknown
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Expected O, but got Unknown
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Expected O, but got Unknown
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Expected O, but got Unknown
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Expected O, but got Unknown
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Expected O, but got Unknown
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Expected O, but got Unknown
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Expected O, but got Unknown
			//IL_0368: Unknown result type (might be due to invalid IL or missing references)
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0396: Expected O, but got Unknown
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Expected O, but got Unknown
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Expected O, but got Unknown
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Expected O, but got Unknown
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Expected O, but got Unknown
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Expected O, but got Unknown
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Expected O, but got Unknown
			//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_051e: Expected O, but got Unknown
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0536: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Expected O, but got Unknown
			//IL_0552: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Expected O, but got Unknown
			//IL_0585: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Expected O, but got Unknown
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d0: Expected O, but got Unknown
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0603: Expected O, but got Unknown
			//IL_0604: Unknown result type (might be due to invalid IL or missing references)
			//IL_0609: Unknown result type (might be due to invalid IL or missing references)
			//IL_0610: Unknown result type (might be due to invalid IL or missing references)
			//IL_061b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Expected O, but got Unknown
			//IL_0637: Unknown result type (might be due to invalid IL or missing references)
			//IL_063c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_064e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0669: Expected O, but got Unknown
			_spamClient = spamClient ?? throw new ArgumentNullException("spamClient");
			_source = source;
			_module = module ?? throw new ArgumentNullException("module");
			_spamContext = spamContext ?? throw new ArgumentNullException("spamContext");
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("New");
			_buttonNew = val;
			((Control)_buttonNew).add_Click((EventHandler<MouseEventArgs>)ButtonNewClick);
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_FlowDirection((ControlFlowDirection)2);
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			_panelButtons = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_panelButtons);
			val3.set_Text("Usage Log");
			((Control)val3).set_Visible(false);
			_buttonUsageLog = val3;
			((Control)_buttonUsageLog).add_Click((EventHandler<MouseEventArgs>)ButtonUsageLogClick);
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)_panelButtons);
			val4.set_Text("Map History");
			((Control)val4).set_Visible(false);
			_buttonMapHistory = val4;
			((Control)_buttonMapHistory).add_Click((EventHandler<MouseEventArgs>)ButtonMapHistoryClick);
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)_panelButtons);
			val5.set_Text("Spam");
			_buttonSpam = val5;
			((Control)_buttonSpam).add_Click((EventHandler<MouseEventArgs>)ButtonSpamClick);
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)_panelButtons);
			val6.set_Text("+ Fav");
			((Control)val6).set_BasicTooltipText("Add to favorites");
			_buttonFavorite = val6;
			((Control)_buttonFavorite).add_Click((EventHandler<MouseEventArgs>)ButtonFavoriteClick);
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)_panelButtons);
			val7.set_Text("Edit");
			_buttonEdit = val7;
			((Control)_buttonEdit).add_Click((EventHandler<MouseEventArgs>)ButtonEditClick);
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)_panelButtons);
			val8.set_Text("Cancel");
			((Control)val8).set_Visible(false);
			_buttonCancel = val8;
			((Control)_buttonCancel).add_Click((EventHandler<MouseEventArgs>)ButtonCancelClick);
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)_panelButtons);
			val9.set_Text("Delete");
			((Control)val9).set_Enabled(false);
			_buttonDelete = val9;
			((Control)_buttonDelete).add_Click((EventHandler<MouseEventArgs>)ButtonDeleteClick);
			Label val10 = new Label();
			((Control)val10).set_Parent((Container)(object)this);
			val10.set_Text("Name:");
			val10.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelForName = val10;
			Label val11 = new Label();
			((Control)val11).set_Parent((Container)(object)this);
			val11.set_Text(string.Empty);
			val11.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelName = val11;
			TextBox val12 = new TextBox();
			((Control)val12).set_Parent((Container)(object)this);
			((Control)val12).set_Visible(false);
			((TextInputBase)val12).set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_textBoxName = val12;
			Label val13 = new Label();
			((Control)val13).set_Parent((Container)(object)this);
			val13.set_Text("Description:");
			val13.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelForDescription = val13;
			Label val14 = new Label();
			((Control)val14).set_Parent((Container)(object)this);
			val14.set_Text(string.Empty);
			val14.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelDescription = val14;
			TextBox val15 = new TextBox();
			((Control)val15).set_Parent((Container)(object)this);
			((Control)val15).set_Visible(false);
			((TextInputBase)val15).set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_textBoxDescription = val15;
			Label val16 = new Label();
			((Control)val16).set_Parent((Container)(object)this);
			val16.set_Text("Last Spam:");
			val16.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelForLastSpam = val16;
			Label val17 = new Label();
			((Control)val17).set_Parent((Container)(object)this);
			val17.set_Text(string.Empty);
			val17.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelLastSpam = val17;
			Label val18 = new Label();
			((Control)val18).set_Parent((Container)(object)this);
			val18.set_Text("Cooldown:");
			val18.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			((Control)val18).set_Visible(true);
			_labelForCooldown = val18;
			Label val19 = new Label();
			((Control)val19).set_Parent((Container)(object)this);
			val19.set_Text(string.Empty);
			val19.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			((Control)val19).set_Visible(true);
			_labelCooldown = val19;
			TextBox val20 = new TextBox();
			((Control)val20).set_Parent((Container)(object)this);
			((Control)val20).set_Visible(false);
			((TextInputBase)val20).set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			((TextInputBase)val20).set_PlaceholderText("e.g. 5m, 1h30m");
			_textBoxCooldown = val20;
			Label val21 = new Label();
			((Control)val21).set_Parent((Container)(object)this);
			val21.set_Text("Status:");
			val21.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			((Control)val21).set_Visible(true);
			_labelForCooldownStatus = val21;
			Label val22 = new Label();
			((Control)val22).set_Parent((Container)(object)this);
			val22.set_Text(string.Empty);
			val22.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			((Control)val22).set_Visible(true);
			_labelCooldownStatus = val22;
			Label val23 = new Label();
			((Control)val23).set_Parent((Container)(object)this);
			val23.set_Text("Category:");
			val23.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelForCategory = val23;
			Label val24 = new Label();
			((Control)val24).set_Parent((Container)(object)this);
			val24.set_Text(string.Empty);
			val24.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelCategory = val24;
			Dropdown val25 = new Dropdown();
			((Control)val25).set_Parent((Container)(object)this);
			((Control)val25).set_Visible(false);
			_dropdownCategory = val25;
			Label val26 = new Label();
			((Control)val26).set_Parent((Container)(object)this);
			val26.set_Text("Created At:");
			val26.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelForCreatedAt = val26;
			Label val27 = new Label();
			((Control)val27).set_Parent((Container)(object)this);
			val27.set_Text(string.Empty);
			val27.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelCreatedAt = val27;
			Label val28 = new Label();
			((Control)val28).set_Parent((Container)(object)this);
			val28.set_Text("Last Modified:");
			val28.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelForLastModified = val28;
			Label val29 = new Label();
			((Control)val29).set_Parent((Container)(object)this);
			val29.set_Text(string.Empty);
			val29.set_Font((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			_labelLastModified = val29;
			SpamLinesControl spamLinesControl = new SpamLinesControl(_module.FontService, _spamContext);
			((Control)spamLinesControl).set_Parent((Container)(object)this);
			((Control)spamLinesControl).set_Left(0);
			((Control)spamLinesControl).set_Top(0);
			((Control)spamLinesControl).set_Width(400);
			((Container)spamLinesControl).set_HeightSizingMode((SizingMode)1);
			_spamLinesControl = spamLinesControl;
			_spamLinesControl.ValidationChanged += OnSpamLinesValidationChanged;
			if (Module.Instance?.SpamOrchestrator != null)
			{
				Module.Instance.SpamOrchestrator.SpamStateChanged += OnSpamStateChanged;
			}
			_updateTimer = new Timer(1000.0);
			_updateTimer.Elapsed += UpdateTimer_Elapsed;
			_updateTimer.Start();
			_initialized = true;
		}

		public void SetSpam(SpamDto spam)
		{
			LoadSpamDetailAsync(spam?.Id ?? Guid.Empty);
		}

		public void SetCategories(List<SpamCategoryDto> categories)
		{
			_categories = categories ?? new List<SpamCategoryDto>();
			PopulateCategoryDropdown();
		}

		private void PopulateCategoryDropdown()
		{
			_dropdownCategory.get_Items().Clear();
			_dropdownCategory.get_Items().Add("(None)");
			foreach (SpamCategoryDto category in _categories.OrderBy((SpamCategoryDto c) => c.DisplayOrder))
			{
				_dropdownCategory.get_Items().Add(category.Name);
			}
			if (_spam != null)
			{
				SelectCategoryInDropdown(_spam.CategoryId);
			}
			else
			{
				_dropdownCategory.set_SelectedItem("(None)");
			}
		}

		private void SelectCategoryInDropdown(Guid? categoryId)
		{
			if (!categoryId.HasValue)
			{
				_dropdownCategory.set_SelectedItem("(None)");
				return;
			}
			SpamCategoryDto category = _categories.FirstOrDefault((SpamCategoryDto c) => c.Id == categoryId.Value);
			if (category != null)
			{
				_dropdownCategory.set_SelectedItem(category.Name);
			}
			else
			{
				_dropdownCategory.set_SelectedItem("(None)");
			}
		}

		private Guid? GetSelectedCategoryId()
		{
			string selectedItem = _dropdownCategory.get_SelectedItem()?.ToString();
			if (string.IsNullOrEmpty(selectedItem) || selectedItem == "(None)")
			{
				return null;
			}
			return _categories.FirstOrDefault((SpamCategoryDto c) => c.Name == selectedItem)?.Id;
		}

		public void RefreshCurrentSpam()
		{
			if (_spam != null && _spam.Id != Guid.Empty)
			{
				LoadSpamDetailAsync(_spam.Id);
			}
		}

		public void UpdateSpamData(SpamDetailDto updatedSpam)
		{
			if (_spam != null && updatedSpam != null && !(_spam.Id != updatedSpam.Id) && !_editMode)
			{
				_spam = updatedSpam;
				UpdateControls();
			}
		}

		public void RemoveSpam()
		{
			_spam = null;
			_editMode = false;
			UpdateControls();
			UpdateEditMode();
		}

		private async Task LoadSpamDetailAsync(Guid spamId)
		{
			try
			{
				if (spamId == Guid.Empty)
				{
					_spam = null;
					UpdateControls();
					_editMode = false;
					UpdateEditMode();
					((Control)this).Invalidate();
					return;
				}
				SpamDetailDto spamDto = await _spamClient.GetSpam(_source, spamId);
				if (spamDto != null)
				{
					_spam = spamDto;
				}
				else
				{
					_spam = null;
				}
				UpdateControls();
				_editMode = false;
				UpdateEditMode();
				((Control)this).Invalidate();
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to load spam details: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
				_spam = null;
				UpdateControls();
				((Control)this).Invalidate();
			}
		}

		private void UpdateControls()
		{
			using (((Control)this).SuspendLayoutContext())
			{
				((Control)_buttonEdit).set_Enabled(_spam != null || _editMode);
				((Control)_buttonNew).set_Enabled(!_editMode);
				((Control)_buttonCancel).set_Enabled(_editMode);
				((Control)_buttonDelete).set_Enabled(_spam != null && !_editMode);
				if (_spam == null)
				{
					_labelName.set_Text(string.Empty);
					((TextInputBase)_textBoxName).set_Text(string.Empty);
					_labelDescription.set_Text(string.Empty);
					((TextInputBase)_textBoxDescription).set_Text(string.Empty);
					_labelLastSpam.set_Text(string.Empty);
					_labelCooldown.set_Text(string.Empty);
					_labelCooldownStatus.set_Text(string.Empty);
					_labelCategory.set_Text(string.Empty);
					_dropdownCategory.set_SelectedItem("(None)");
					_labelCreatedAt.set_Text(string.Empty);
					_labelLastModified.set_Text(string.Empty);
					_cooldownDuration = null;
					((Control)_buttonSpam).set_Enabled(false);
					((Control)_buttonMapHistory).set_Visible(false);
					((Control)_buttonUsageLog).set_Visible(false);
					((Control)_buttonFavorite).set_Enabled(false);
					_isFavorited = false;
					UpdateFavoriteButton();
					_spamLinesControl.SetLines(new List<SpamLineDto>());
					return;
				}
				_labelName.set_Text(_spam.Name);
				((TextInputBase)_textBoxName).set_Text(_spam.Name);
				_labelDescription.set_Text(_spam.Description);
				((TextInputBase)_textBoxDescription).set_Text(_spam.Description);
				_cooldownDuration = ParseCooldownFormatted(_spam.CooldownFormatted);
				_labelCooldown.set_Text(_spam.CooldownFormatted ?? "None");
				((TextInputBase)_textBoxCooldown).set_Text(_spam.CooldownFormatted ?? "5m");
				_labelCategory.set_Text(_spam.CategoryName ?? "(None)");
				SelectCategoryInDropdown(_spam.CategoryId);
				UpdateTimeDependentLabels();
				_labelCreatedAt.set_Text(_spam.CreatedAt.ToLocalTime().ToString("g"));
				_labelLastModified.set_Text(_spam.ModifiedAt.HasValue ? _spam.ModifiedAt.Value.ToLocalTime().ToString("g") : "Never");
				_spamLinesControl.SetLines(_spam.SpamLines ?? new List<SpamLineDto>());
				_spamLinesControl.SetEditMode(_editMode);
				bool hasMapLines = _spam.SpamLines?.Any((SpamLineDto l) => l.Target == ChatType.Map) ?? false;
				((Control)_buttonMapHistory).set_Visible(hasMapLines && !_editMode);
				((Control)_buttonUsageLog).set_Visible(!_editMode);
				((Control)_buttonFavorite).set_Enabled(!_isNewSpam && !_editMode);
				((Control)_panelButtons).Invalidate();
			}
		}

		private void UpdateEditMode()
		{
			using (((Control)this).SuspendLayoutContext())
			{
				_buttonEdit.set_Text(_editMode ? "Save" : "Edit");
				((Control)_buttonNew).set_Enabled(!_editMode);
				((Control)_buttonCancel).set_Visible(_editMode);
				((Control)_labelName).set_Visible(!_editMode);
				((Control)_textBoxName).set_Visible(_editMode);
				((Control)_labelDescription).set_Visible(!_editMode);
				((Control)_textBoxDescription).set_Visible(_editMode);
				((Control)_labelCooldown).set_Visible(!_editMode);
				((Control)_textBoxCooldown).set_Visible(_editMode);
				((Control)_labelCategory).set_Visible(!_editMode);
				((Control)_dropdownCategory).set_Visible(_editMode);
				_spamLinesControl.SetEditMode(_editMode);
				if (_editMode)
				{
					((Control)_buttonMapHistory).set_Visible(false);
					((Control)_buttonUsageLog).set_Visible(false);
					((Control)_buttonFavorite).set_Enabled(false);
				}
				else if (_spam != null)
				{
					bool hasMapLines = _spam.SpamLines?.Any((SpamLineDto l) => l.Target == ChatType.Map) ?? false;
					((Control)_buttonMapHistory).set_Visible(hasMapLines);
					((Control)_buttonUsageLog).set_Visible(true);
					((Control)_buttonFavorite).set_Enabled(!_isNewSpam);
				}
				((Control)_panelButtons).Invalidate();
			}
			((Control)this).Invalidate();
		}

		private void ButtonEditClick(object sender, MouseEventArgs e)
		{
			HandleButtonEditClickAsync();
		}

		private async Task HandleButtonEditClickAsync()
		{
			if (_editMode)
			{
				if (IsChanged())
				{
					try
					{
						List<SpamLineDto> currentLines = _spamLinesControl.GetLines();
						if (_isNewSpam)
						{
							SpamCreateDto createDto = new SpamCreateDto
							{
								Name = ((TextInputBase)_textBoxName).get_Text(),
								Description = ((TextInputBase)_textBoxDescription).get_Text(),
								CooldownFormatted = ((TextInputBase)_textBoxCooldown).get_Text(),
								CategoryId = GetSelectedCategoryId(),
								SpamLines = currentLines
							};
							await _spamClient.AddSpam(_source, createDto);
							_isNewSpam = false;
						}
						else
						{
							SpamUpdateDto updateDto = new SpamUpdateDto
							{
								Id = _spam.Id,
								Name = ((TextInputBase)_textBoxName).get_Text(),
								Description = ((TextInputBase)_textBoxDescription).get_Text(),
								CooldownFormatted = ((TextInputBase)_textBoxCooldown).get_Text(),
								CategoryId = GetSelectedCategoryId(),
								SpamLines = currentLines
							};
							_spam = await _spamClient.UpdateSpam(_source, _spam.Id, updateDto);
						}
						ScreenNotification.ShowNotification("Spam saved successfully", (NotificationType)0, (Texture2D)null, 4);
					}
					catch (Exception ex)
					{
						ScreenNotification.ShowNotification("Failed to save spam: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
						return;
					}
				}
				_editMode = false;
				UpdateControls();
				UpdateEditMode();
				OnEditingStopped();
			}
			else if (_spam != null)
			{
				_editMode = true;
				UpdateEditMode();
				OnEditingStarted();
			}
		}

		private void ButtonNewClick(object sender, MouseEventArgs e)
		{
			_spam = new SpamDetailDto
			{
				Id = Guid.NewGuid(),
				Name = string.Empty,
				Description = string.Empty,
				CooldownFormatted = "5m",
				CreatedAt = DateTimeOffset.UtcNow,
				LastSpammed = null,
				SpamLines = new List<SpamLineDto>()
			};
			_isNewSpam = true;
			_editMode = true;
			UpdateControls();
			UpdateEditMode();
			OnEditingStarted();
		}

		private void ButtonCancelClick(object sender, MouseEventArgs e)
		{
			_editMode = false;
			_isNewSpam = false;
			UpdateControls();
			UpdateEditMode();
			OnEditingStopped();
		}

		private void ButtonDeleteClick(object sender, MouseEventArgs e)
		{
			HandleButtonDeleteClickAsync();
		}

		private async Task HandleButtonDeleteClickAsync()
		{
			if (_spam != null && !_editMode)
			{
				try
				{
					await _spamClient.DeleteSpam(_source, _spam.Id);
					_spam = null;
					UpdateControls();
					UpdateEditMode();
					OnEditingStopped();
					ScreenNotification.ShowNotification("Spam deleted successfully", (NotificationType)0, (Texture2D)null, 4);
				}
				catch (Exception ex)
				{
					ScreenNotification.ShowNotification("Failed to delete spam: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
				}
			}
		}

		private void ButtonSpamClick(object sender, MouseEventArgs e)
		{
			if (_spam != null)
			{
				List<string> validationErrors = ValidateAllLinesForUse();
				if (validationErrors.Count > 0)
				{
					ScreenNotification.ShowNotification("Cannot send spam: " + validationErrors[0], (NotificationType)2, (Texture2D)null, 4);
				}
				else
				{
					this.SpamUseRequested?.Invoke(this, _spam.Id);
				}
			}
		}

		private void ButtonMapHistoryClick(object sender, MouseEventArgs e)
		{
			if (_spam != null)
			{
				((Control)new MapSpamHistoryWindow(_spamClient, _source, _spam.Id, _spam.Name)).Show();
			}
		}

		private void ButtonUsageLogClick(object sender, MouseEventArgs e)
		{
			if (_spam != null)
			{
				((Control)new SpamUsageLogWindow(_spamClient, _source, _spam.Id, _spam.Name)).Show();
			}
		}

		private void ButtonFavoriteClick(object sender, MouseEventArgs e)
		{
			if (_spam != null && !_isNewSpam)
			{
				this.FavoriteToggleRequested?.Invoke(this, (_spam.Id, !_isFavorited));
			}
		}

		public void SetIsFavorited(bool isFavorited)
		{
			_isFavorited = isFavorited;
			UpdateFavoriteButton();
		}

		private void UpdateFavoriteButton()
		{
			if (_isFavorited)
			{
				_buttonFavorite.set_Text("- Fav");
				((Control)_buttonFavorite).set_BasicTooltipText("Remove from favorites");
			}
			else
			{
				_buttonFavorite.set_Text("+ Fav");
				((Control)_buttonFavorite).set_BasicTooltipText("Add to favorites");
			}
		}

		private List<string> ValidateAllLinesForUse()
		{
			List<string> errors = new List<string>();
			List<SpamLineDto> lines = _spamLinesControl.GetLines();
			for (int i = 0; i < lines.Count; i++)
			{
				SpamLineDto line = lines[i];
				if (!ChatTypeValidationService.IsAllowedInContext(line.Target, _spamContext.ContextType))
				{
					errors.Add($"Line {i + 1}: {ChatTypeValidationService.GetDisallowedReason(line.Target, _spamContext.ContextType)}");
					continue;
				}
				ValidationResult targetValidation = ChatTypeValidationService.ValidateTargetInfo1(line.Target, line.TargetInfo1, _spamContext);
				if (targetValidation.Status == ValidationStatus.Error)
				{
					errors.Add($"Line {i + 1}: {targetValidation.Message}");
					continue;
				}
				int min = TextInterpolationService.CalculateMinMaxLength(line.LineText ?? string.Empty).min;
				if (min > 199)
				{
					errors.Add($"Line {i + 1}: Message too long ({min} > {199})");
				}
			}
			return errors;
		}

		private bool IsChanged()
		{
			if (_spam == null)
			{
				return false;
			}
			if (((TextInputBase)_textBoxName).get_Text() != _spam.Name)
			{
				return true;
			}
			if (((TextInputBase)_textBoxDescription).get_Text() != _spam.Description)
			{
				return true;
			}
			if (((TextInputBase)_textBoxCooldown).get_Text() != (_spam.CooldownFormatted ?? "5m"))
			{
				return true;
			}
			if (GetSelectedCategoryId() != _spam.CategoryId)
			{
				return true;
			}
			List<SpamLineDto> currentLines = _spamLinesControl.GetLines();
			if (currentLines == null || _spam.SpamLines == null)
			{
				if ((currentLines?.Count ?? 0) != (_spam.SpamLines?.Count ?? 0))
				{
					return true;
				}
			}
			else
			{
				if (currentLines.Count != _spam.SpamLines.Count)
				{
					return true;
				}
				for (int i = 0; i < currentLines.Count; i++)
				{
					SpamLineDto currentLine = currentLines[i];
					SpamLineDto originalLine = _spam.SpamLines[i];
					if (currentLine.LineText != originalLine.LineText || currentLine.Target != originalLine.Target || currentLine.TargetInfo1 != originalLine.TargetInfo1)
					{
						return true;
					}
				}
			}
			return false;
		}

		protected virtual void OnEditingStarted()
		{
			this.EditingStarted?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnEditingStopped()
		{
			this.EditingStopped?.Invoke(this, EventArgs.Empty);
		}

		private void OnSpamLinesValidationChanged(object sender, ValidationStatus status)
		{
			_hasValidationError = status == ValidationStatus.Error;
			UpdateSpamButtonState();
		}

		private void OnSpamStateChanged(object sender, EventArgs e)
		{
			UpdateSpamButtonState();
		}

		private void UpdateSpamButtonState()
		{
			bool canSpam = _spam != null && !_editMode && !_hasValidationError && !(Module.Instance?.SpamOrchestrator?.IsSpamming).GetValueOrDefault();
			((Control)_buttonSpam).set_Enabled(canSpam);
		}

		private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (_spam != null && !_editMode)
			{
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					UpdateTimeDependentLabels();
				});
			}
		}

		private TimeSpan? ParseCooldownFormatted(string cooldownFormatted)
		{
			if (string.IsNullOrEmpty(cooldownFormatted))
			{
				return null;
			}
			Match match = Regex.Match(cooldownFormatted, "^(?:(\\d+)h)?(?:(\\d+)m)?(?:(\\d+)s)?$");
			if (!match.Success)
			{
				return null;
			}
			int hours = (match.Groups[1].Success ? int.Parse(match.Groups[1].Value) : 0);
			int minutes = (match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0);
			int seconds = (match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0);
			return new TimeSpan(hours, minutes, seconds);
		}

		private void UpdateTimeDependentLabels()
		{
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			if (_spam == null)
			{
				return;
			}
			if (_spam.LastSpammed.HasValue)
			{
				TimeSpan elapsed = DateTime.UtcNow - _spam.LastSpammed.Value;
				_labelLastSpam.set_Text(FormatElapsedTime(elapsed));
			}
			else
			{
				_labelLastSpam.set_Text("Never");
			}
			if (_cooldownDuration.HasValue && _spam.LastSpammed.HasValue)
			{
				TimeSpan elapsed2 = DateTime.UtcNow - _spam.LastSpammed.Value;
				TimeSpan remaining = _cooldownDuration.Value - elapsed2;
				if (remaining.TotalSeconds > 0.0)
				{
					_labelCooldownStatus.set_Text("On cooldown (" + FormatTimeSpan(remaining) + " remaining)");
					_labelCooldownStatus.set_TextColor(Color.get_Red());
				}
				else
				{
					_labelCooldownStatus.set_Text("Ready");
					_labelCooldownStatus.set_TextColor(Color.get_Green());
				}
			}
			else if (!_spam.LastSpammed.HasValue)
			{
				_labelCooldownStatus.set_Text("Ready (never used)");
				_labelCooldownStatus.set_TextColor(Color.get_Green());
			}
			else
			{
				_labelCooldownStatus.set_Text("Ready");
				_labelCooldownStatus.set_TextColor(Color.get_Green());
			}
			UpdateSpamButtonState();
		}

		private string FormatElapsedTime(TimeSpan elapsed)
		{
			if (elapsed.TotalSeconds < 60.0)
			{
				return $"{(int)elapsed.TotalSeconds}s ago";
			}
			if (elapsed.TotalMinutes < 60.0)
			{
				return $"{(int)elapsed.TotalMinutes}m {elapsed.Seconds}s ago";
			}
			if (elapsed.TotalHours < 24.0)
			{
				return $"{(int)elapsed.TotalHours}h {elapsed.Minutes}m ago";
			}
			return $"{(int)elapsed.TotalDays}d {elapsed.Hours}h ago";
		}

		private string FormatTimeSpan(TimeSpan time)
		{
			if (time.TotalSeconds < 60.0)
			{
				return $"{(int)time.TotalSeconds}s";
			}
			if (time.TotalMinutes < 60.0)
			{
				return $"{(int)time.TotalMinutes}m {time.Seconds}s";
			}
			return $"{(int)time.TotalHours}h {time.Minutes}m";
		}

		public override void RecalculateLayout()
		{
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			if (!_initialized)
			{
				return;
			}
			using (((Control)this).SuspendLayoutContext())
			{
				int scrollHeight = ((Container)this).get_HorizontalScrollOffset();
				((Control)_buttonNew).set_Left(10);
				((Control)_buttonNew).set_Top(10);
				((Control)_buttonNew).set_Width(75);
				((Control)_buttonNew).set_Height(24);
				((Control)_panelButtons).set_Left(10);
				((Control)_panelButtons).set_Top(((Control)_buttonNew).get_Bottom() + 5);
				((Control)_buttonSpam).set_Width(75);
				((Control)_buttonSpam).set_Height(24);
				((Control)_buttonMapHistory).set_Width(85);
				((Control)_buttonMapHistory).set_Height(24);
				((Control)_buttonUsageLog).set_Width(85);
				((Control)_buttonUsageLog).set_Height(24);
				((Control)_buttonFavorite).set_Width(50);
				((Control)_buttonFavorite).set_Height(24);
				((Control)_buttonEdit).set_Width(75);
				((Control)_buttonEdit).set_Height(24);
				((Control)_buttonCancel).set_Width(75);
				((Control)_buttonCancel).set_Height(24);
				((Control)_buttonDelete).set_Width(75);
				((Control)_buttonDelete).set_Height(24);
				((Control)_labelForName).set_Left(((Control)_panelButtons).get_Left());
				((Control)_labelForName).set_Top(((Control)_panelButtons).get_Bottom() + 5);
				_labelForName.set_AutoSizeWidth(true);
				((Control)_labelForName).set_Height(24);
				((Control)_labelForDescription).set_Left(((Control)_panelButtons).get_Left());
				((Control)_labelForDescription).set_Top(((Control)_labelForName).get_Bottom() + 5);
				_labelForDescription.set_AutoSizeWidth(true);
				((Control)_labelForDescription).set_Height(24);
				int spamTextBottom = ((Control)_labelForDescription).get_Bottom();
				((Control)_spamLinesControl).set_Left(((Control)_panelButtons).get_Left());
				((Control)_spamLinesControl).set_Top(spamTextBottom + 5);
				((Control)_spamLinesControl).set_Width(((Container)this).get_ContentRegion().Width - ((Control)_spamLinesControl).get_Left());
				((Control)_spamLinesControl).RecalculateLayout();
				int linesBottom = ((Control)_spamLinesControl).get_Bottom();
				((Control)_labelForLastSpam).set_Left(((Control)_panelButtons).get_Left());
				((Control)_labelForLastSpam).set_Top(linesBottom + 5);
				_labelForLastSpam.set_AutoSizeWidth(true);
				((Control)_labelForLastSpam).set_Height(24);
				((Control)_labelForCooldown).set_Left(((Control)_panelButtons).get_Left());
				((Control)_labelForCooldown).set_Top(((Control)_labelForLastSpam).get_Bottom() + 5);
				_labelForCooldown.set_AutoSizeWidth(true);
				((Control)_labelForCooldown).set_Height(24);
				((Control)_labelForCooldownStatus).set_Left(((Control)_panelButtons).get_Left());
				((Control)_labelForCooldownStatus).set_Top(((Control)_labelForCooldown).get_Bottom() + 5);
				_labelForCooldownStatus.set_AutoSizeWidth(true);
				((Control)_labelForCooldownStatus).set_Height(24);
				((Control)_labelForCategory).set_Left(((Control)_panelButtons).get_Left());
				((Control)_labelForCategory).set_Top(((Control)_labelForCooldownStatus).get_Bottom() + 5);
				_labelForCategory.set_AutoSizeWidth(true);
				((Control)_labelForCategory).set_Height(24);
				((Control)_labelForCreatedAt).set_Left(((Control)_panelButtons).get_Left());
				((Control)_labelForCreatedAt).set_Top(((Control)_labelForCategory).get_Bottom() + 5);
				_labelForCreatedAt.set_AutoSizeWidth(true);
				((Control)_labelForCreatedAt).set_Height(24);
				((Control)_labelForLastModified).set_Left(((Control)_panelButtons).get_Left());
				((Control)_labelForLastModified).set_Top(((Control)_labelForCreatedAt).get_Bottom() + 5);
				_labelForLastModified.set_AutoSizeWidth(true);
				((Control)_labelForLastModified).set_Height(24);
				int left = Math.Max(((Control)_labelForName).get_Width(), Math.Max(((Control)_labelForCreatedAt).get_Width(), Math.Max(((Control)_labelForDescription).get_Width(), Math.Max(((Control)_labelForLastModified).get_Width(), Math.Max(((Control)_labelForLastSpam).get_Width(), Math.Max(((Control)_labelForCooldown).get_Width(), Math.Max(((Control)_labelForCooldownStatus).get_Width(), ((Control)_labelForCategory).get_Width()))))))) + ((Control)_labelForName).get_Left() + 5;
				((Control)_labelName).set_Left(left);
				((Control)_labelName).set_Top(((Control)_labelForName).get_Top());
				_labelName.set_AutoSizeWidth(true);
				((Control)_labelName).set_Height(24);
				((Control)_textBoxName).set_Left(left);
				((Control)_textBoxName).set_Top(((Control)_labelForName).get_Top());
				((Control)_textBoxName).set_Width(200);
				((Control)_textBoxName).set_Height(24);
				((Control)_labelDescription).set_Left(left);
				((Control)_labelDescription).set_Top(((Control)_labelForDescription).get_Top());
				_labelDescription.set_AutoSizeWidth(true);
				((Control)_labelDescription).set_Height(24);
				((Control)_textBoxDescription).set_Left(left);
				((Control)_textBoxDescription).set_Top(((Control)_labelForDescription).get_Top());
				((Control)_textBoxDescription).set_Width(200);
				((Control)_textBoxDescription).set_Height(24);
				((Control)_labelLastSpam).set_Left(left);
				((Control)_labelLastSpam).set_Top(((Control)_labelForLastSpam).get_Top());
				_labelLastSpam.set_AutoSizeWidth(true);
				((Control)_labelLastSpam).set_Height(24);
				((Control)_labelCooldown).set_Left(left);
				((Control)_labelCooldown).set_Top(((Control)_labelForCooldown).get_Top());
				_labelCooldown.set_AutoSizeWidth(true);
				((Control)_labelCooldown).set_Height(24);
				((Control)_textBoxCooldown).set_Left(left);
				((Control)_textBoxCooldown).set_Top(((Control)_labelForCooldown).get_Top());
				((Control)_textBoxCooldown).set_Width(100);
				((Control)_textBoxCooldown).set_Height(24);
				((Control)_labelCooldownStatus).set_Left(left);
				((Control)_labelCooldownStatus).set_Top(((Control)_labelForCooldownStatus).get_Top());
				_labelCooldownStatus.set_AutoSizeWidth(true);
				((Control)_labelCooldownStatus).set_Height(24);
				((Control)_labelCategory).set_Left(left);
				((Control)_labelCategory).set_Top(((Control)_labelForCategory).get_Top());
				_labelCategory.set_AutoSizeWidth(true);
				((Control)_labelCategory).set_Height(24);
				((Control)_dropdownCategory).set_Left(left);
				((Control)_dropdownCategory).set_Top(((Control)_labelForCategory).get_Top());
				((Control)_dropdownCategory).set_Width(150);
				((Control)_dropdownCategory).set_Height(24);
				((Control)_labelCreatedAt).set_Left(left);
				((Control)_labelCreatedAt).set_Top(((Control)_labelForCreatedAt).get_Top());
				_labelCreatedAt.set_AutoSizeWidth(true);
				((Control)_labelCreatedAt).set_Height(24);
				((Control)_labelLastModified).set_Left(left);
				((Control)_labelLastModified).set_Top(((Control)_labelForLastModified).get_Top());
				_labelLastModified.set_AutoSizeWidth(true);
				((Control)_labelLastModified).set_Height(24);
				if (!((Panel)this).get_CanScroll() && ((Container)this).get_Children().get_Count() > 0)
				{
					int maxBottom = 0;
					int maxRight = 0;
					foreach (Control c in ((Container)this).get_Children())
					{
						if (c.get_Bottom() > maxBottom)
						{
							maxBottom = c.get_Bottom();
						}
						if (c.get_Right() > maxRight)
						{
							maxRight = c.get_Right();
						}
					}
					((Control)this).set_Height(maxBottom + 5);
				}
				((Container)this).set_HorizontalScrollOffset(scrollHeight);
			}
		}

		protected override void DisposeControl()
		{
			if (_updateTimer != null)
			{
				_updateTimer.Stop();
				_updateTimer.Elapsed -= UpdateTimer_Elapsed;
				_updateTimer.Dispose();
			}
			if (_spamLinesControl != null)
			{
				_spamLinesControl.ValidationChanged -= OnSpamLinesValidationChanged;
			}
			if (Module.Instance?.SpamOrchestrator != null)
			{
				Module.Instance.SpamOrchestrator.SpamStateChanged -= OnSpamStateChanged;
			}
			((Panel)this).DisposeControl();
		}
	}
}
