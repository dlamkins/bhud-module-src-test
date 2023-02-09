using System;
using System.Collections.Generic;
using System.Drawing;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;

namespace Characters.Views
{
	public class OCRView : FramedContainer
	{
		private readonly OCR _ocr;

		private readonly SettingsModel _settings;

		private readonly Color _spacingColor = Color.FromArgb(255, 200, 200, 200);

		private readonly Color _ignoredColor = Color.FromArgb(255, 100, 100, 100);

		private readonly NumberBox _columnBox;

		private readonly NumberBox _thresholdBox;

		private readonly Label _instructions;

		private readonly Label _bestMatchLabel;

		private readonly Label _resultLabel;

		private readonly Kenedia.Modules.Core.Controls.Image _sourceImage;

		private readonly Kenedia.Modules.Core.Controls.Image _cleanedImage;

		private readonly Kenedia.Modules.Core.Controls.Image _scaledImage;

		private readonly ImageButton _closeButton;

		private readonly ResizeableContainer _ocrRegionContainer;

		private readonly MaskedRegion _maskedRegion;

		private bool _sizeSet;

		private double _readTick;

		public OCRView(SettingsModel settings, OCR ocr)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_058e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0636: Unknown result type (might be due to invalid IL or missing references)
			//IL_0668: Unknown result type (might be due to invalid IL or missing references)
			//IL_0672: Unknown result type (might be due to invalid IL or missing references)
			//IL_0682: Unknown result type (might be due to invalid IL or missing references)
			//IL_068c: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0759: Unknown result type (might be due to invalid IL or missing references)
			//IL_078b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0795: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07af: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_080b: Unknown result type (might be due to invalid IL or missing references)
			//IL_087c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0891: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0927: Unknown result type (might be due to invalid IL or missing references)
			//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_09df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c16: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c21: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c40: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ca3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cc9: Unknown result type (might be due to invalid IL or missing references)
			_settings = settings;
			_ocr = ocr;
			base.BorderColor = Color.get_Black();
			base.BackgroundImage = AsyncTexture2D.FromAssetId(156003);
			base.TextureRectangle = new Rectangle(50, 50, 500, 500);
			((Control)this).set_Height(350);
			((Control)this).set_Width(620);
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((Control)flowPanel).set_Width(((Control)this).get_Width());
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_AutoSizePadding(new Point(3, 3));
			((FlowPanel)flowPanel).set_OuterControlPadding(new Vector2(3f, 3f));
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(3f, 3f));
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			flowPanel.BorderColor = Color.get_Black();
			flowPanel.BorderWidth = new RectangleDimensions(2);
			FlowPanel contentFlowPanel = flowPanel;
			FlowPanel flowPanel2 = new FlowPanel();
			((FlowPanel)flowPanel2).set_FlowDirection((ControlFlowDirection)2);
			((Control)flowPanel2).set_Parent((Container)(object)contentFlowPanel);
			((Container)flowPanel2).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(5f, 5f));
			FlowPanel headerPanel = flowPanel2;
			Label label = new Label();
			((Control)label).set_Parent((Container)(object)headerPanel);
			((Label)label).set_AutoSizeHeight(true);
			((Control)label).set_Width(((Container)contentFlowPanel).get_ContentRegion().Width - 35);
			((Label)label).set_WrapText(true);
			((Label)label).set_TextColor(Colors.ColonialWhite);
			label.SetLocalizedText = () => strings.OCR_Instructions;
			_instructions = label;
			ImageButton imageButton = new ImageButton();
			((Control)imageButton).set_Parent((Container)(object)headerPanel);
			imageButton.Texture = AsyncTexture2D.FromAssetId(156012);
			imageButton.HoveredTexture = AsyncTexture2D.FromAssetId(156011);
			((Control)imageButton).set_Size(new Point(25, 25));
			imageButton.TextureRectangle = new Rectangle(7, 7, 20, 20);
			_closeButton = imageButton;
			((Control)_closeButton).add_Click((EventHandler<MouseEventArgs>)CloseButton_Click);
			FlowPanel flowPanel3 = new FlowPanel();
			((Control)flowPanel3).set_Parent((Container)(object)contentFlowPanel);
			((Container)flowPanel3).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel3).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel3).set_ControlPadding(new Vector2(10f, 0f));
			((FlowPanel)flowPanel3).set_FlowDirection((ControlFlowDirection)2);
			FlowPanel fp = flowPanel3;
			FramedContainer framedContainer = new FramedContainer();
			((Control)framedContainer).set_Parent((Container)(object)fp);
			((Control)framedContainer).set_Width(500);
			((Control)framedContainer).set_Height(GameService.Content.get_DefaultFont32().get_LineHeight() + 8);
			framedContainer.BorderColor = Color.get_Black() * 0.7f;
			framedContainer.BackgroundColor = Color.get_Black() * 0.4f;
			framedContainer.BorderWidth = new RectangleDimensions(2);
			FramedContainer p = framedContainer;
			Label label2 = new Label();
			((Control)label2).set_Location(new Point(5, 0));
			((Control)label2).set_Parent((Container)(object)p);
			((Control)label2).set_Height(((Control)p).get_Height());
			((Label)label2).set_AutoSizeWidth(true);
			((Label)label2).set_TextColor(Colors.ColonialWhite);
			((Label)label2).set_Font(GameService.Content.get_DefaultFont32());
			((Label)label2).set_VerticalAlignment((VerticalAlignment)1);
			_bestMatchLabel = label2;
			Label label3 = new Label();
			((Control)label3).set_Parent((Container)(object)fp);
			((Label)label3).set_VerticalAlignment((VerticalAlignment)1);
			((Control)label3).set_Height(((Control)p).get_Height());
			((Control)label3).set_Width(100);
			((Label)label3).set_TextColor(Color.get_White());
			((Label)label3).set_Font(GameService.Content.get_DefaultFont16());
			((Label)label3).set_WrapText(true);
			label3.SetLocalizedText = () => "Best Match";
			FlowPanel flowPanel4 = new FlowPanel();
			((Control)flowPanel4).set_Parent((Container)(object)contentFlowPanel);
			((Container)flowPanel4).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel4).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel4).set_ControlPadding(new Vector2(10f, 0f));
			((FlowPanel)flowPanel4).set_FlowDirection((ControlFlowDirection)2);
			fp = flowPanel4;
			FramedContainer framedContainer2 = new FramedContainer();
			((Control)framedContainer2).set_Parent((Container)(object)fp);
			((Control)framedContainer2).set_Width(500);
			((Control)framedContainer2).set_Height(GameService.Content.get_DefaultFont32().get_LineHeight() + 8);
			framedContainer2.BorderColor = Color.get_Black() * 0.7f;
			framedContainer2.BackgroundColor = Color.get_Black() * 0.4f;
			framedContainer2.BorderWidth = new RectangleDimensions(2);
			p = framedContainer2;
			Label label4 = new Label();
			((Control)label4).set_Location(new Point(5, 0));
			((Control)label4).set_Parent((Container)(object)p);
			((Control)label4).set_Height(((Control)p).get_Height());
			((Label)label4).set_AutoSizeWidth(true);
			((Label)label4).set_TextColor(Colors.ColonialWhite);
			((Label)label4).set_Font(GameService.Content.get_DefaultFont32());
			((Label)label4).set_VerticalAlignment((VerticalAlignment)1);
			_resultLabel = label4;
			Label label5 = new Label();
			((Control)label5).set_Parent((Container)(object)fp);
			((Label)label5).set_VerticalAlignment((VerticalAlignment)1);
			((Control)label5).set_Height(((Control)p).get_Height());
			((Control)label5).set_Width(100);
			((Label)label5).set_TextColor(Color.get_White());
			((Label)label5).set_Font(GameService.Content.get_DefaultFont16());
			((Label)label5).set_WrapText(true);
			label5.SetLocalizedText = () => "OCR Result";
			FlowPanel flowPanel5 = new FlowPanel();
			((Control)flowPanel5).set_Parent((Container)(object)contentFlowPanel);
			((Container)flowPanel5).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel5).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel5).set_ControlPadding(new Vector2(10f, 0f));
			((FlowPanel)flowPanel5).set_FlowDirection((ControlFlowDirection)2);
			fp = flowPanel5;
			FramedContainer framedContainer3 = new FramedContainer();
			((Control)framedContainer3).set_Parent((Container)(object)fp);
			((Control)framedContainer3).set_Width(500);
			((Control)framedContainer3).set_Height(55);
			framedContainer3.BorderColor = Color.get_Black() * 0.7f;
			framedContainer3.BackgroundColor = Color.get_Black() * 0.4f;
			framedContainer3.BorderWidth = new RectangleDimensions(2);
			p = framedContainer3;
			Kenedia.Modules.Core.Controls.Image image = new Kenedia.Modules.Core.Controls.Image();
			((Control)image).set_Location(new Point(5, 5));
			((Control)image).set_Parent((Container)(object)p);
			_scaledImage = image;
			Label label6 = new Label();
			((Control)label6).set_Parent((Container)(object)fp);
			((Control)label6).set_Height(((Control)p).get_Height());
			((Control)label6).set_Width(100);
			((Label)label6).set_TextColor(Color.get_White());
			((Label)label6).set_Font(GameService.Content.get_DefaultFont16());
			((Label)label6).set_WrapText(true);
			label6.SetLocalizedText = () => "Scaled";
			((Label)label6).set_VerticalAlignment((VerticalAlignment)1);
			FlowPanel flowPanel6 = new FlowPanel();
			((Control)flowPanel6).set_Parent((Container)(object)contentFlowPanel);
			((Container)flowPanel6).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel6).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel6).set_ControlPadding(new Vector2(10f, 0f));
			((FlowPanel)flowPanel6).set_FlowDirection((ControlFlowDirection)2);
			fp = flowPanel6;
			FramedContainer framedContainer4 = new FramedContainer();
			((Control)framedContainer4).set_Parent((Container)(object)fp);
			((Control)framedContainer4).set_Width(500);
			((Control)framedContainer4).set_Height(55);
			framedContainer4.BorderColor = Color.get_Black() * 0.7f;
			framedContainer4.BackgroundColor = Color.get_Black() * 0.4f;
			framedContainer4.BorderWidth = new RectangleDimensions(2);
			p = framedContainer4;
			Kenedia.Modules.Core.Controls.Image image2 = new Kenedia.Modules.Core.Controls.Image();
			((Control)image2).set_Location(new Point(5, 5));
			((Control)image2).set_Parent((Container)(object)p);
			_cleanedImage = image2;
			Label label7 = new Label();
			((Control)label7).set_Parent((Container)(object)fp);
			((Control)label7).set_Height(((Control)p).get_Height());
			((Control)label7).set_Width(100);
			((Label)label7).set_TextColor(Color.get_White());
			((Label)label7).set_Font(GameService.Content.get_DefaultFont16());
			((Label)label7).set_WrapText(true);
			label7.SetLocalizedText = () => "Cleaned";
			((Label)label7).set_VerticalAlignment((VerticalAlignment)1);
			FlowPanel flowPanel7 = new FlowPanel();
			((Control)flowPanel7).set_Parent((Container)(object)contentFlowPanel);
			((Container)flowPanel7).set_WidthSizingMode((SizingMode)2);
			((Container)flowPanel7).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel7).set_ControlPadding(new Vector2(10f, 0f));
			((FlowPanel)flowPanel7).set_FlowDirection((ControlFlowDirection)2);
			fp = flowPanel7;
			FramedContainer framedContainer5 = new FramedContainer();
			((Control)framedContainer5).set_Parent((Container)(object)fp);
			((Control)framedContainer5).set_Width(500);
			((Control)framedContainer5).set_Height(55);
			framedContainer5.BorderColor = Color.get_Black() * 0.7f;
			framedContainer5.BackgroundColor = Color.get_Black() * 0.4f;
			framedContainer5.BorderWidth = new RectangleDimensions(2);
			p = framedContainer5;
			Kenedia.Modules.Core.Controls.Image image3 = new Kenedia.Modules.Core.Controls.Image();
			((Control)image3).set_Location(new Point(5, 5));
			((Control)image3).set_Parent((Container)(object)p);
			_sourceImage = image3;
			Label label8 = new Label();
			((Control)label8).set_Parent((Container)(object)fp);
			((Control)label8).set_Height(((Control)p).get_Height());
			((Control)label8).set_Width(100);
			((Label)label8).set_TextColor(Color.get_White());
			((Label)label8).set_Font(GameService.Content.get_DefaultFont16());
			((Label)label8).set_WrapText(true);
			label8.SetLocalizedText = () => "Source";
			((Label)label8).set_VerticalAlignment((VerticalAlignment)1);
			FlowPanel flowPanel8 = new FlowPanel();
			((Control)flowPanel8).set_Parent((Container)(object)contentFlowPanel);
			((Container)flowPanel8).set_WidthSizingMode((SizingMode)1);
			((Container)flowPanel8).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)flowPanel8).set_OuterControlPadding(new Vector2(0f, 5f));
			((FlowPanel)flowPanel8).set_ControlPadding(new Vector2(5f, 5f));
			((FlowPanel)flowPanel8).set_FlowDirection((ControlFlowDirection)2);
			FlowPanel thresholdPanel = flowPanel8;
			Label label9 = new Label();
			((Control)label9).set_Parent((Container)(object)thresholdPanel);
			((Control)label9).set_Height(25);
			((Label)label9).set_AutoSizeWidth(true);
			((Label)label9).set_TextColor(Colors.ColonialWhite);
			label9.SetLocalizedText = () => strings.EmptyColumns;
			label9.SetLocalizedTooltip = () => strings.EmptyColumns_Tooltip;
			NumberBox numberBox = new NumberBox();
			((Control)numberBox).set_Parent((Container)(object)thresholdPanel);
			((Control)numberBox).set_Size(new Point(100, 25));
			numberBox.MinValue = 0;
			numberBox.MaxValue = 100;
			numberBox.Value = _settings.OCRNoPixelColumns.get_Value();
			numberBox.SetLocalizedTooltip = () => strings.EmptyColumnsThreshold_Tooltip;
			numberBox.ValueChangedAction = delegate(int num)
			{
				_settings.OCRNoPixelColumns.set_Value(num);
			};
			_columnBox = numberBox;
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)thresholdPanel);
			panel.BackgroundColor = new Color(_spacingColor.R, _spacingColor.G, _spacingColor.B, _spacingColor.A);
			((Control)panel).set_Size(new Point(25, 25));
			Label label10 = new Label();
			((Control)label10).set_Parent((Container)(object)thresholdPanel);
			((Control)label10).set_Height(25);
			((Label)label10).set_AutoSizeWidth(true);
			((Label)label10).set_TextColor(Colors.ColonialWhite);
			label10.SetLocalizedText = () => strings.EmptyColumn;
			label10.SetLocalizedTooltip = () => strings.EmptyColumn_Tooltip;
			Panel panel2 = new Panel();
			((Control)panel2).set_Parent((Container)(object)thresholdPanel);
			panel2.BackgroundColor = new Color(_ignoredColor.R, _ignoredColor.G, _ignoredColor.B, _ignoredColor.A);
			((Control)panel2).set_Size(new Point(25, 25));
			Label label11 = new Label();
			((Control)label11).set_Parent((Container)(object)thresholdPanel);
			((Control)label11).set_Height(25);
			((Label)label11).set_AutoSizeWidth(true);
			((Label)label11).set_TextColor(Colors.ColonialWhite);
			label11.SetLocalizedText = () => strings.IgnoredPart;
			label11.SetLocalizedTooltip = () => strings.IgnoredPart_Tooltip;
			NumberBox numberBox2 = new NumberBox();
			((Control)numberBox2).set_Parent((Container)(object)thresholdPanel);
			((Control)numberBox2).set_Height(25);
			((Control)numberBox2).set_Width(100);
			numberBox2.MinValue = 0;
			numberBox2.MaxValue = 255;
			numberBox2.Value = _settings.OCR_ColorThreshold.get_Value();
			numberBox2.SetLocalizedTooltip = () => "Threshold of 'white' a pixel has to be to be converted to black to be read (RGB Value: 0 - 255)";
			numberBox2.ValueChangedAction = delegate(int num)
			{
				_settings.OCR_ColorThreshold.set_Value(num);
			};
			_thresholdBox = numberBox2;
			MaskedRegion maskedRegion = new MaskedRegion();
			((Control)maskedRegion).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)maskedRegion).set_ZIndex(int.MaxValue);
			((Control)maskedRegion).set_Visible(false);
			_maskedRegion = maskedRegion;
			ResizeableContainer resizeableContainer = new ResizeableContainer();
			((Control)resizeableContainer).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)resizeableContainer).set_Visible(false);
			Rectangle activeOCRRegion = _settings.ActiveOCRRegion;
			((Control)resizeableContainer).set_Location(((Rectangle)(ref activeOCRRegion)).get_Location());
			activeOCRRegion = _settings.ActiveOCRRegion;
			((Control)resizeableContainer).set_Size(((Rectangle)(ref activeOCRRegion)).get_Size());
			resizeableContainer.BorderColor = Colors.ColonialWhite;
			resizeableContainer.ShowResizeOnlyOnMouseOver = true;
			resizeableContainer.MaxSize = new Point(((Control)this).get_Width(), 100);
			resizeableContainer.BorderWidth = new RectangleDimensions(2);
			((Control)resizeableContainer).set_ZIndex(2147483646);
			_ocrRegionContainer = resizeableContainer;
			((Control)_ocrRegionContainer).add_Resized((EventHandler<ResizedEventArgs>)Container_Changed);
			((Control)_ocrRegionContainer).add_Moved((EventHandler<MovedEventArgs>)Container_Changed);
			activeOCRRegion = _settings.ActiveOCRRegion;
			((Rectangle)(ref activeOCRRegion)).get_Size();
			((Control)this).set_Location(new Point(((Control)_ocrRegionContainer).get_Left(), ((Control)_ocrRegionContainer).get_Top() - ((Control)this).get_Height() - 5));
			ForceOnScreen();
		}

		public void EnableMaskedRegion()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			Rectangle b = _settings.ActiveOCRRegion;
			((Control)_maskedRegion).set_Size(((Rectangle)(ref b)).get_Size());
			((Control)_maskedRegion).set_Location(((Rectangle)(ref b)).get_Location());
			MaskedRegion maskedRegion = _maskedRegion;
			if (maskedRegion != null)
			{
				((Control)maskedRegion).Show();
			}
		}

		public void DisableMaskedRegion()
		{
			MaskedRegion maskedRegion = _maskedRegion;
			if (maskedRegion != null)
			{
				((Control)maskedRegion).Hide();
			}
		}

		private void Container_Changed(object sender, EventArgs e)
		{
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			if (!_sizeSet)
			{
				string key = _settings.OCRKey;
				Dictionary<string, Rectangle> regions = _settings.OCRRegions.get_Value();
				Rectangle bounds = default(Rectangle);
				((Rectangle)(ref bounds))._002Ector(((Control)_ocrRegionContainer).get_Left() + _ocrRegionContainer.BorderWidth.Left, ((Control)_ocrRegionContainer).get_Top() + _ocrRegionContainer.BorderWidth.Top, ((Control)_ocrRegionContainer).get_Width() - _ocrRegionContainer.BorderWidth.Horizontal, ((Control)_ocrRegionContainer).get_Height() - _ocrRegionContainer.BorderWidth.Vertical);
				if (!regions.ContainsKey(key))
				{
					regions.Add(key, bounds);
				}
				else
				{
					regions[key] = bounds;
				}
			}
			_sizeSet = false;
			((Control)this).set_Location(new Point(((Control)_ocrRegionContainer).get_Left(), ((Control)_ocrRegionContainer).get_Top() - ((Control)this).get_Height() - 5));
			Rectangle b = _settings.ActiveOCRRegion;
			((Control)_maskedRegion).set_Size(((Rectangle)(ref b)).get_Size());
			((Control)_maskedRegion).set_Location(((Rectangle)(ref b)).get_Location());
		}

		private void CloseButton_Click(object sender, MouseEventArgs e)
		{
			ToggleContainer();
		}

		public void ToggleContainer()
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			bool visible = ((Control)(object)this).ToggleVisibility();
			ForceOnScreen();
			((Control)(object)_ocrRegionContainer)?.ToggleVisibility(visible);
			((Control)(object)_maskedRegion)?.ToggleVisibility(visible);
			if (((Control)_ocrRegionContainer).get_Visible())
			{
				_sizeSet = true;
				ResizeableContainer ocrRegionContainer = _ocrRegionContainer;
				Rectangle activeOCRRegion = _settings.ActiveOCRRegion;
				((Control)ocrRegionContainer).set_Location(((Rectangle)(ref activeOCRRegion)).get_Location().Add(new Point(-_ocrRegionContainer.BorderWidth.Left, -_ocrRegionContainer.BorderWidth.Top)));
				ResizeableContainer ocrRegionContainer2 = _ocrRegionContainer;
				activeOCRRegion = _settings.ActiveOCRRegion;
				((Control)ocrRegionContainer2).set_Size(((Rectangle)(ref activeOCRRegion)).get_Size().Add(new Point(_ocrRegionContainer.BorderWidth.Horizontal, _ocrRegionContainer.BorderWidth.Vertical)));
			}
		}

		public override async void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (!((Control)this).get_Visible())
			{
				return;
			}
			ForceOnScreen();
			MaskedRegion maskedRegion = _maskedRegion;
			Rectangle val = ((Control)_ocrRegionContainer).get_AbsoluteBounds();
			((Control)maskedRegion).set_Visible(!((Rectangle)(ref val)).Contains(Control.get_Input().get_Mouse().get_Position()));
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _readTick > 250.0 && ((Control)_maskedRegion).get_Visible())
			{
				_readTick = gameTime.get_TotalGameTime().TotalMilliseconds;
				if (await _ocr.Read(show: true) != null)
				{
					((Label)_bestMatchLabel).set_Text(_ocr.BestMatchResult);
					((Label)_resultLabel).set_Text(_ocr.ReadResult);
					((Image)_sourceImage).set_Texture(AsyncTexture2D.op_Implicit(_ocr.SourceTexture));
					Kenedia.Modules.Core.Controls.Image sourceImage = _sourceImage;
					val = _ocr.SourceTexture.get_Bounds();
					((Control)sourceImage).set_Size(((Rectangle)(ref val)).get_Size());
					((Image)_cleanedImage).set_Texture(AsyncTexture2D.op_Implicit(_ocr.CleanedTexture));
					Kenedia.Modules.Core.Controls.Image cleanedImage = _cleanedImage;
					val = _ocr.CleanedTexture.get_Bounds();
					((Control)cleanedImage).set_Size(((Rectangle)(ref val)).get_Size());
					((Image)_scaledImage).set_Texture(AsyncTexture2D.op_Implicit(_ocr.ScaledTexture));
					Kenedia.Modules.Core.Controls.Image scaledImage = _scaledImage;
					val = _ocr.ScaledTexture.get_Bounds();
					((Control)scaledImage).set_Size(((Rectangle)(ref val)).get_Size());
				}
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			NumberBox columnBox = _columnBox;
			if (columnBox != null)
			{
				((Control)columnBox).Dispose();
			}
			NumberBox thresholdBox = _thresholdBox;
			if (thresholdBox != null)
			{
				((Control)thresholdBox).Dispose();
			}
			ResizeableContainer ocrRegionContainer = _ocrRegionContainer;
			if (ocrRegionContainer != null)
			{
				((Control)ocrRegionContainer).Dispose();
			}
			Label instructions = _instructions;
			if (instructions != null)
			{
				((Control)instructions).Dispose();
			}
			Kenedia.Modules.Core.Controls.Image sourceImage = _sourceImage;
			if (sourceImage != null)
			{
				((Control)sourceImage).Dispose();
			}
			Kenedia.Modules.Core.Controls.Image cleanedImage = _cleanedImage;
			if (cleanedImage != null)
			{
				((Control)cleanedImage).Dispose();
			}
			Kenedia.Modules.Core.Controls.Image scaledImage = _scaledImage;
			if (scaledImage != null)
			{
				((Control)scaledImage).Dispose();
			}
			Label resultLabel = _resultLabel;
			if (resultLabel != null)
			{
				((Control)resultLabel).Dispose();
			}
			Label bestMatchLabel = _bestMatchLabel;
			if (bestMatchLabel != null)
			{
				((Control)bestMatchLabel).Dispose();
			}
			MaskedRegion maskedRegion = _maskedRegion;
			if (maskedRegion != null)
			{
				((Control)maskedRegion).Dispose();
			}
		}

		private void ForceOnScreen()
		{
			Screen screen = Control.get_Graphics().get_SpriteScreen();
			if (((Control)_ocrRegionContainer).get_Bottom() > ((Control)screen).get_Bottom())
			{
				((Control)_ocrRegionContainer).set_Bottom(((Control)screen).get_Bottom());
			}
			if (((Control)_ocrRegionContainer).get_Top() < ((Control)screen).get_Top() + ((Control)this).get_Height())
			{
				((Control)_ocrRegionContainer).set_Top(((Control)screen).get_Top() + ((Control)this).get_Height());
			}
			if (((Control)_ocrRegionContainer).get_Left() < ((Control)screen).get_Left())
			{
				((Control)_ocrRegionContainer).set_Left(((Control)screen).get_Left());
			}
			if (((Control)this).get_Right() > ((Control)screen).get_Right())
			{
				((Control)_ocrRegionContainer).set_Left(((Control)screen).get_Right() - ((Control)this).get_Width());
			}
		}
	}
}
