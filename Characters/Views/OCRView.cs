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

		private readonly Settings _settings;

		private readonly Color _spacingColor = Color.FromArgb(255, 200, 200, 200);

		private readonly Color _ignoredColor = Color.FromArgb(255, 100, 100, 100);

		private readonly NumberBox _columnBox;

		private readonly NumberBox _thresholdBox;

		private readonly Kenedia.Modules.Core.Controls.Label _instructions;

		private readonly Kenedia.Modules.Core.Controls.Label _bestMatchLabel;

		private readonly Kenedia.Modules.Core.Controls.Label _resultLabel;

		private readonly Kenedia.Modules.Core.Controls.Image _sourceImage;

		private readonly Kenedia.Modules.Core.Controls.Image _cleanedImage;

		private readonly Kenedia.Modules.Core.Controls.Image _scaledImage;

		private readonly ImageButton _closeButton;

		private readonly ResizeableContainer _ocrRegionContainer;

		private readonly MaskedRegion _maskedRegion;

		private bool _sizeSet;

		private double _readTick;

		public OCRView(Settings settings, OCR ocr)
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
			base.Height = 350;
			base.Width = 620;
			Kenedia.Modules.Core.Controls.FlowPanel contentFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Width = base.Width,
				HeightSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Point(3, 3),
				OuterControlPadding = new Vector2(3f, 3f),
				ControlPadding = new Vector2(3f, 3f),
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				BorderColor = Color.get_Black(),
				BorderWidth = new RectangleDimensions(2)
			};
			Kenedia.Modules.Core.Controls.FlowPanel headerPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				Parent = contentFlowPanel,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				ControlPadding = new Vector2(5f, 5f)
			};
			_instructions = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = headerPanel,
				AutoSizeHeight = true,
				Width = contentFlowPanel.ContentRegion.Width - 35,
				WrapText = true,
				TextColor = ContentService.Colors.ColonialWhite,
				SetLocalizedText = () => strings.OCR_Instructions
			};
			_closeButton = new ImageButton
			{
				Parent = headerPanel,
				Texture = AsyncTexture2D.FromAssetId(156012),
				HoveredTexture = AsyncTexture2D.FromAssetId(156011),
				Size = new Point(25, 25),
				TextureRectangle = new Rectangle(7, 7, 20, 20)
			};
			_closeButton.Click += CloseButton_Click;
			Kenedia.Modules.Core.Controls.FlowPanel fp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = contentFlowPanel,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				ControlPadding = new Vector2(10f, 0f),
				FlowDirection = ControlFlowDirection.SingleLeftToRight
			};
			FramedContainer p = new FramedContainer
			{
				Parent = fp,
				Width = 500,
				Height = GameService.Content.DefaultFont32.get_LineHeight() + 8,
				BorderColor = Color.get_Black() * 0.7f,
				BackgroundColor = Color.get_Black() * 0.4f,
				BorderWidth = new RectangleDimensions(2)
			};
			_bestMatchLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Location = new Point(5, 0),
				Parent = p,
				Height = p.Height,
				AutoSizeWidth = true,
				TextColor = ContentService.Colors.ColonialWhite,
				Font = GameService.Content.DefaultFont32,
				VerticalAlignment = VerticalAlignment.Middle
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = fp,
				VerticalAlignment = VerticalAlignment.Middle,
				Height = p.Height,
				Width = 100,
				TextColor = Color.get_White(),
				Font = GameService.Content.DefaultFont16,
				WrapText = true,
				SetLocalizedText = () => "Best Match"
			};
			fp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = contentFlowPanel,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				ControlPadding = new Vector2(10f, 0f),
				FlowDirection = ControlFlowDirection.SingleLeftToRight
			};
			p = new FramedContainer
			{
				Parent = fp,
				Width = 500,
				Height = GameService.Content.DefaultFont32.get_LineHeight() + 8,
				BorderColor = Color.get_Black() * 0.7f,
				BackgroundColor = Color.get_Black() * 0.4f,
				BorderWidth = new RectangleDimensions(2)
			};
			_resultLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Location = new Point(5, 0),
				Parent = p,
				Height = p.Height,
				AutoSizeWidth = true,
				TextColor = ContentService.Colors.ColonialWhite,
				Font = GameService.Content.DefaultFont32,
				VerticalAlignment = VerticalAlignment.Middle
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = fp,
				VerticalAlignment = VerticalAlignment.Middle,
				Height = p.Height,
				Width = 100,
				TextColor = Color.get_White(),
				Font = GameService.Content.DefaultFont16,
				WrapText = true,
				SetLocalizedText = () => "OCR Result"
			};
			fp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = contentFlowPanel,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				ControlPadding = new Vector2(10f, 0f),
				FlowDirection = ControlFlowDirection.SingleLeftToRight
			};
			p = new FramedContainer
			{
				Parent = fp,
				Width = 500,
				Height = 55,
				BorderColor = Color.get_Black() * 0.7f,
				BackgroundColor = Color.get_Black() * 0.4f,
				BorderWidth = new RectangleDimensions(2)
			};
			_scaledImage = new Kenedia.Modules.Core.Controls.Image
			{
				Location = new Point(5, 5),
				Parent = p
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = fp,
				Height = p.Height,
				Width = 100,
				TextColor = Color.get_White(),
				Font = GameService.Content.DefaultFont16,
				WrapText = true,
				SetLocalizedText = () => "Scaled",
				VerticalAlignment = VerticalAlignment.Middle
			};
			fp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = contentFlowPanel,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				ControlPadding = new Vector2(10f, 0f),
				FlowDirection = ControlFlowDirection.SingleLeftToRight
			};
			p = new FramedContainer
			{
				Parent = fp,
				Width = 500,
				Height = 55,
				BorderColor = Color.get_Black() * 0.7f,
				BackgroundColor = Color.get_Black() * 0.4f,
				BorderWidth = new RectangleDimensions(2)
			};
			_cleanedImage = new Kenedia.Modules.Core.Controls.Image
			{
				Location = new Point(5, 5),
				Parent = p
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = fp,
				Height = p.Height,
				Width = 100,
				TextColor = Color.get_White(),
				Font = GameService.Content.DefaultFont16,
				WrapText = true,
				SetLocalizedText = () => "Cleaned",
				VerticalAlignment = VerticalAlignment.Middle
			};
			fp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = contentFlowPanel,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				ControlPadding = new Vector2(10f, 0f),
				FlowDirection = ControlFlowDirection.SingleLeftToRight
			};
			p = new FramedContainer
			{
				Parent = fp,
				Width = 500,
				Height = 55,
				BorderColor = Color.get_Black() * 0.7f,
				BackgroundColor = Color.get_Black() * 0.4f,
				BorderWidth = new RectangleDimensions(2)
			};
			_sourceImage = new Kenedia.Modules.Core.Controls.Image
			{
				Location = new Point(5, 5),
				Parent = p
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = fp,
				Height = p.Height,
				Width = 100,
				TextColor = Color.get_White(),
				Font = GameService.Content.DefaultFont16,
				WrapText = true,
				SetLocalizedText = () => "Source",
				VerticalAlignment = VerticalAlignment.Middle
			};
			Kenedia.Modules.Core.Controls.FlowPanel thresholdPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = contentFlowPanel,
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize,
				OuterControlPadding = new Vector2(0f, 5f),
				ControlPadding = new Vector2(5f, 5f),
				FlowDirection = ControlFlowDirection.SingleLeftToRight
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = thresholdPanel,
				Height = 25,
				AutoSizeWidth = true,
				TextColor = ContentService.Colors.ColonialWhite,
				SetLocalizedText = () => strings.EmptyColumns,
				SetLocalizedTooltip = () => strings.EmptyColumns_Tooltip
			};
			_columnBox = new NumberBox
			{
				Parent = thresholdPanel,
				Size = new Point(100, 25),
				MinValue = 0,
				MaxValue = 100,
				Value = _settings.OCRNoPixelColumns.Value,
				SetLocalizedTooltip = () => strings.EmptyColumnsThreshold_Tooltip,
				ValueChangedAction = delegate(int num)
				{
					_settings.OCRNoPixelColumns.Value = num;
				}
			};
			new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = thresholdPanel,
				BackgroundColor = new Color(_spacingColor.R, _spacingColor.G, _spacingColor.B, _spacingColor.A),
				Size = new Point(25, 25)
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = thresholdPanel,
				Height = 25,
				AutoSizeWidth = true,
				TextColor = ContentService.Colors.ColonialWhite,
				SetLocalizedText = () => strings.EmptyColumn,
				SetLocalizedTooltip = () => strings.EmptyColumn_Tooltip
			};
			new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = thresholdPanel,
				BackgroundColor = new Color(_ignoredColor.R, _ignoredColor.G, _ignoredColor.B, _ignoredColor.A),
				Size = new Point(25, 25)
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = thresholdPanel,
				Height = 25,
				AutoSizeWidth = true,
				TextColor = ContentService.Colors.ColonialWhite,
				SetLocalizedText = () => strings.IgnoredPart,
				SetLocalizedTooltip = () => strings.IgnoredPart_Tooltip
			};
			_thresholdBox = new NumberBox
			{
				Parent = thresholdPanel,
				Height = 25,
				Width = 100,
				MinValue = 0,
				MaxValue = 255,
				Value = _settings.OCR_ColorThreshold.Value,
				SetLocalizedTooltip = () => "Threshold of 'white' a pixel has to be to be converted to black to be read (RGB Value: 0 - 255)",
				ValueChangedAction = delegate(int num)
				{
					_settings.OCR_ColorThreshold.Value = num;
				}
			};
			_maskedRegion = new MaskedRegion
			{
				Parent = GameService.Graphics.SpriteScreen,
				ZIndex = int.MaxValue,
				Visible = false
			};
			ResizeableContainer obj = new ResizeableContainer
			{
				Parent = GameService.Graphics.SpriteScreen,
				Visible = false
			};
			Rectangle activeOCRRegion = _settings.ActiveOCRRegion;
			obj.Location = ((Rectangle)(ref activeOCRRegion)).get_Location();
			activeOCRRegion = _settings.ActiveOCRRegion;
			obj.Size = ((Rectangle)(ref activeOCRRegion)).get_Size();
			obj.BorderColor = ContentService.Colors.ColonialWhite;
			obj.ShowResizeOnlyOnMouseOver = true;
			obj.MaxSize = new Point(base.Width, 100);
			obj.BorderWidth = new RectangleDimensions(2);
			obj.ZIndex = 2147483646;
			_ocrRegionContainer = obj;
			_ocrRegionContainer.Resized += Container_Changed;
			_ocrRegionContainer.Moved += Container_Changed;
			activeOCRRegion = _settings.ActiveOCRRegion;
			((Rectangle)(ref activeOCRRegion)).get_Size();
			base.Location = new Point(_ocrRegionContainer.Left, _ocrRegionContainer.Top - base.Height - 5);
			ForceOnScreen();
		}

		public void EnableMaskedRegion()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			Rectangle b = _settings.ActiveOCRRegion;
			_maskedRegion.Size = ((Rectangle)(ref b)).get_Size();
			_maskedRegion.Location = ((Rectangle)(ref b)).get_Location();
			_maskedRegion?.Show();
		}

		public void DisableMaskedRegion()
		{
			_maskedRegion?.Hide();
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
				Dictionary<string, Rectangle> regions = _settings.OCRRegions.Value;
				Rectangle bounds = default(Rectangle);
				((Rectangle)(ref bounds))._002Ector(_ocrRegionContainer.Left + _ocrRegionContainer.BorderWidth.Left, _ocrRegionContainer.Top + _ocrRegionContainer.BorderWidth.Top, _ocrRegionContainer.Width - _ocrRegionContainer.BorderWidth.Horizontal, _ocrRegionContainer.Height - _ocrRegionContainer.BorderWidth.Vertical);
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
			base.Location = new Point(_ocrRegionContainer.Left, _ocrRegionContainer.Top - base.Height - 5);
			Rectangle b = _settings.ActiveOCRRegion;
			_maskedRegion.Size = ((Rectangle)(ref b)).get_Size();
			_maskedRegion.Location = ((Rectangle)(ref b)).get_Location();
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
			bool visible = this.ToggleVisibility();
			ForceOnScreen();
			_ocrRegionContainer?.ToggleVisibility(visible);
			_maskedRegion?.ToggleVisibility(visible);
			if (_ocrRegionContainer.Visible)
			{
				_sizeSet = true;
				ResizeableContainer ocrRegionContainer = _ocrRegionContainer;
				Rectangle activeOCRRegion = _settings.ActiveOCRRegion;
				ocrRegionContainer.Location = ((Rectangle)(ref activeOCRRegion)).get_Location().Add(new Point(-_ocrRegionContainer.BorderWidth.Left, -_ocrRegionContainer.BorderWidth.Top));
				ResizeableContainer ocrRegionContainer2 = _ocrRegionContainer;
				activeOCRRegion = _settings.ActiveOCRRegion;
				ocrRegionContainer2.Size = ((Rectangle)(ref activeOCRRegion)).get_Size().Add(new Point(_ocrRegionContainer.BorderWidth.Horizontal, _ocrRegionContainer.BorderWidth.Vertical));
			}
		}

		public override async void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (!base.Visible)
			{
				return;
			}
			ForceOnScreen();
			MaskedRegion maskedRegion = _maskedRegion;
			Rectangle val = _ocrRegionContainer.AbsoluteBounds;
			maskedRegion.Visible = !((Rectangle)(ref val)).Contains(Control.Input.Mouse.Position);
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _readTick > 250.0 && _maskedRegion.Visible)
			{
				_readTick = gameTime.get_TotalGameTime().TotalMilliseconds;
				string result = await _ocr.Read(show: true);
				if (result != null)
				{
					_sourceImage.Texture = _ocr.SourceTexture;
					Kenedia.Modules.Core.Controls.Image sourceImage = _sourceImage;
					val = _ocr.SourceTexture.get_Bounds();
					sourceImage.Size = ((Rectangle)(ref val)).get_Size();
					_cleanedImage.Texture = _ocr.CleanedTexture;
					Kenedia.Modules.Core.Controls.Image cleanedImage = _cleanedImage;
					val = _ocr.CleanedTexture.get_Bounds();
					cleanedImage.Size = ((Rectangle)(ref val)).get_Size();
					_scaledImage.Texture = _ocr.ScaledTexture;
					Kenedia.Modules.Core.Controls.Image scaledImage = _scaledImage;
					val = _ocr.ScaledTexture.get_Bounds();
					scaledImage.Size = ((Rectangle)(ref val)).get_Size();
					_resultLabel.Font = Control.Content.DefaultFont32;
					_resultLabel.WrapText = false;
					_bestMatchLabel.Text = _ocr.BestMatchResult;
					_resultLabel.Text = _ocr.ReadResult;
				}
				else if (!_ocr.IsLoaded)
				{
					_bestMatchLabel.Text = ((!string.IsNullOrEmpty(result)) ? _ocr.BestMatchResult : $"Tesseract Engine Loaded: {_ocr.IsLoaded}");
					_resultLabel.Text = ((!string.IsNullOrEmpty(result)) ? _ocr.ReadResult : (_ocr.PathToEngine ?? ""));
					_resultLabel.Font = Control.Content.DefaultFont14;
					_resultLabel.WrapText = true;
				}
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_columnBox?.Dispose();
			_thresholdBox?.Dispose();
			_ocrRegionContainer?.Dispose();
			_instructions?.Dispose();
			_sourceImage?.Dispose();
			_cleanedImage?.Dispose();
			_scaledImage?.Dispose();
			_resultLabel?.Dispose();
			_bestMatchLabel?.Dispose();
			_maskedRegion?.Dispose();
		}

		private void ForceOnScreen()
		{
			Screen screen = Control.Graphics.SpriteScreen;
			if (_ocrRegionContainer.Bottom > screen.Bottom)
			{
				_ocrRegionContainer.Bottom = screen.Bottom;
			}
			if (_ocrRegionContainer.Top < screen.Top + base.Height)
			{
				_ocrRegionContainer.Top = screen.Top + base.Height;
			}
			if (_ocrRegionContainer.Left < screen.Left)
			{
				_ocrRegionContainer.Left = screen.Left;
			}
			if (base.Right > screen.Right)
			{
				_ocrRegionContainer.Left = screen.Right - base.Width;
			}
		}
	}
}
