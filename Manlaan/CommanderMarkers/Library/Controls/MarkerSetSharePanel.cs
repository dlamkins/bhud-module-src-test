using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Manlaan.CommanderMarkers.Presets.Model;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class MarkerSetSharePanel : FlowPanel
	{
		private MarkerSet _markerSet = new MarkerSet();

		private Dropdown? _categoryDropdown;

		private TextBox? _customCategoryBox;

		private Label? _errorLabel;

		private Label? _statusLabel;

		private List<string> _categoryNames = new List<string>();

		public bool IsSubmitting { get; private set; }

		public MarkerSetSharePanel()
			: this()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_ControlPadding(new Vector2(5f, 5f));
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
		}

		public void LoadMarkerSet(MarkerSet markerSet)
		{
			_markerSet = markerSet;
			IsSubmitting = false;
			((Container)this).ClearChildren();
			BuildContent();
		}

		public void SetSubmitting(bool submitting)
		{
			IsSubmitting = submitting;
			if (_categoryDropdown != null)
			{
				((Control)_categoryDropdown).set_Enabled(!submitting);
			}
			if (_customCategoryBox != null)
			{
				((Control)_customCategoryBox).set_Enabled(!submitting);
			}
		}

		public void SetFeedback(string? error, string? status)
		{
			if (_errorLabel != null)
			{
				_errorLabel!.set_Text(error ?? "");
				((Control)_errorLabel).set_Visible(!string.IsNullOrWhiteSpace(error));
			}
			if (_statusLabel != null)
			{
				_statusLabel!.set_Text(status ?? "");
				((Control)_statusLabel).set_Visible(!string.IsNullOrWhiteSpace(status));
			}
		}

		public bool TryGetCategory(out string category, out string? error)
		{
			Dropdown? categoryDropdown = _categoryDropdown;
			int categoryIndex = ((categoryDropdown != null) ? categoryDropdown!.get_Items().IndexOf(_categoryDropdown!.get_SelectedItem()) : (-1));
			TextBox? customCategoryBox = _customCategoryBox;
			string customCategory = ((customCategoryBox != null) ? ((TextInputBase)customCategoryBox).get_Text() : null) ?? "";
			category = CommunityShareHelper.ResolveCategory(categoryIndex, customCategory, _categoryNames);
			if (string.IsNullOrWhiteSpace(category))
			{
				error = "Enter a category name.";
				return false;
			}
			error = null;
			return true;
		}

		private void BuildContent()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Expected O, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Expected O, but got Unknown
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Expected O, but got Unknown
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Expected O, but got Unknown
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Share this marker set with the community!");
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Check your marker set text and choose a category. Submissions are reviewed prior to publishing.");
			((Control)val2).set_Width(((Control)this).get_Width() - 40);
			val2.set_WrapText(true);
			val2.set_AutoSizeHeight(true);
			if (Service.Gw2ApiManager == null || !Service.Gw2ApiManager.HasPermission((TokenPermission)1))
			{
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)this);
				val3.set_Text("Enable the Account API permission for Commander Markers in BlishHUD to share marker sets.");
				((Control)val3).set_Width(((Control)this).get_Width() - 40);
				val3.set_WrapText(true);
				val3.set_AutoSizeHeight(true);
				return;
			}
			AddReadOnlyField("Name", _markerSet.name ?? "");
			AddReadOnlyField("Description", _markerSet.description ?? "");
			AddReadOnlyField("Map", _markerSet.MapName);
			AddReadOnlyField("Markers", (_markerSet.marks?.Count ?? 0).ToString());
			_categoryNames = CommunityShareHelper.CategoryNames();
			int customIndex = _categoryNames.Count;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_FlowDirection((ControlFlowDirection)0);
			val4.set_ControlPadding(new Vector2(10f, 5f));
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			FlowPanel categoryRow = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)categoryRow);
			val5.set_Text("Category");
			((Control)val5).set_Width(100);
			((Control)val5).set_Height(30);
			Dropdown val6 = new Dropdown();
			((Control)val6).set_Parent((Container)(object)categoryRow);
			((Control)val6).set_Width(280);
			((Control)val6).set_Height(30);
			_categoryDropdown = val6;
			foreach (string name in _categoryNames)
			{
				_categoryDropdown!.get_Items().Add(name);
			}
			_categoryDropdown!.get_Items().Add("Suggest New Category...");
			_categoryDropdown!.set_SelectedItem((_categoryNames.Count == 0) ? _categoryDropdown!.get_Items()[customIndex] : _categoryDropdown!.get_Items()[0]);
			TextBox val7 = new TextBox();
			((Control)val7).set_Parent((Container)(object)categoryRow);
			((Control)val7).set_Width(280);
			((Control)val7).set_BasicTooltipText("Type category name");
			((Control)val7).set_Visible(_categoryNames.Count == 0);
			_customCategoryBox = val7;
			_categoryDropdown!.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate
			{
				if (_categoryDropdown != null && _customCategoryBox != null)
				{
					int num = _categoryDropdown!.get_Items().IndexOf(_categoryDropdown!.get_SelectedItem());
					((Control)_customCategoryBox).set_Visible(num == customIndex || _categoryNames.Count == 0);
				}
			});
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)this);
			((Control)val8).set_Width(((Control)this).get_Width() - 40);
			val8.set_WrapText(true);
			val8.set_AutoSizeHeight(true);
			((Control)val8).set_Visible(false);
			_errorLabel = val8;
			Label val9 = new Label();
			((Control)val9).set_Parent((Container)(object)this);
			((Control)val9).set_Width(((Control)this).get_Width() - 40);
			val9.set_WrapText(true);
			val9.set_AutoSizeHeight(true);
			((Control)val9).set_Visible(false);
			_statusLabel = val9;
		}

		private void AddReadOnlyField(string label, string value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2(10f, 5f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			FlowPanel row = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)row);
			val2.set_Text(label);
			((Control)val2).set_Width(100);
			((Control)val2).set_Height(30);
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)row);
			val3.set_Text(value);
			((Control)val3).set_Width(400);
			val3.set_WrapText(true);
			val3.set_AutoSizeHeight(true);
		}
	}
}
