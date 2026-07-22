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

		private readonly System.Drawing.Color _spacingColor = System.Drawing.Color.FromArgb(255, 200, 200, 200);

		private readonly System.Drawing.Color _ignoredColor = System.Drawing.Color.FromArgb(255, 100, 100, 100);

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
			_settings = settings;
			_ocr = ocr;
			base.BorderColor = Microsoft.Xna.Framework.Color.Black;
			base.BackgroundImage = AsyncTexture2D.FromAssetId(156003);
			base.TextureRectangle = new Microsoft.Xna.Framework.Rectangle(50, 50, 500, 500);
			base.Height = 350;
			base.Width = 620;
			Kenedia.Modules.Core.Controls.FlowPanel contentFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Width = base.Width,
				HeightSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Microsoft.Xna.Framework.Point(3, 3),
				OuterControlPadding = new Vector2(3f, 3f),
				ControlPadding = new Vector2(3f, 3f),
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				BorderColor = Microsoft.Xna.Framework.Color.Black,
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
				Size = new Microsoft.Xna.Framework.Point(25, 25),
				TextureRectangle = new Microsoft.Xna.Framework.Rectangle(7, 7, 20, 20)
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
				Height = GameService.Content.DefaultFont32.LineHeight + 8,
				BorderColor = Microsoft.Xna.Framework.Color.Black * 0.7f,
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.4f,
				BorderWidth = new RectangleDimensions(2)
			};
			_bestMatchLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Location = new Microsoft.Xna.Framework.Point(5, 0),
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
				TextColor = Microsoft.Xna.Framework.Color.White,
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
				Height = GameService.Content.DefaultFont32.LineHeight + 8,
				BorderColor = Microsoft.Xna.Framework.Color.Black * 0.7f,
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.4f,
				BorderWidth = new RectangleDimensions(2)
			};
			_resultLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Location = new Microsoft.Xna.Framework.Point(5, 0),
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
				TextColor = Microsoft.Xna.Framework.Color.White,
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
				BorderColor = Microsoft.Xna.Framework.Color.Black * 0.7f,
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.4f,
				BorderWidth = new RectangleDimensions(2)
			};
			_scaledImage = new Kenedia.Modules.Core.Controls.Image
			{
				Location = new Microsoft.Xna.Framework.Point(5, 5),
				Parent = p
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = fp,
				Height = p.Height,
				Width = 100,
				TextColor = Microsoft.Xna.Framework.Color.White,
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
				BorderColor = Microsoft.Xna.Framework.Color.Black * 0.7f,
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.4f,
				BorderWidth = new RectangleDimensions(2)
			};
			_cleanedImage = new Kenedia.Modules.Core.Controls.Image
			{
				Location = new Microsoft.Xna.Framework.Point(5, 5),
				Parent = p
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = fp,
				Height = p.Height,
				Width = 100,
				TextColor = Microsoft.Xna.Framework.Color.White,
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
				BorderColor = Microsoft.Xna.Framework.Color.Black * 0.7f,
				BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.4f,
				BorderWidth = new RectangleDimensions(2)
			};
			_sourceImage = new Kenedia.Modules.Core.Controls.Image
			{
				Location = new Microsoft.Xna.Framework.Point(5, 5),
				Parent = p
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = fp,
				Height = p.Height,
				Width = 100,
				TextColor = Microsoft.Xna.Framework.Color.White,
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
				Size = new Microsoft.Xna.Framework.Point(100, 25),
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
				BackgroundColor = new Microsoft.Xna.Framework.Color(_spacingColor.R, _spacingColor.G, _spacingColor.B, _spacingColor.A),
				Size = new Microsoft.Xna.Framework.Point(25, 25)
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
				BackgroundColor = new Microsoft.Xna.Framework.Color(_ignoredColor.R, _ignoredColor.G, _ignoredColor.B, _ignoredColor.A),
				Size = new Microsoft.Xna.Framework.Point(25, 25)
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
			_ocrRegionContainer = new ResizeableContainer
			{
				Parent = GameService.Graphics.SpriteScreen,
				Visible = false,
				Location = _settings.ActiveOCRRegion.Location,
				Size = _settings.ActiveOCRRegion.Size,
				BorderColor = ContentService.Colors.ColonialWhite,
				ShowResizeOnlyOnMouseOver = true,
				Width = base.Width,
				Height = 50,
				MaxSize = new Microsoft.Xna.Framework.Point(base.Width, 100),
				BorderWidth = new RectangleDimensions(2),
				ZIndex = 2147483646
			};
			_ocrRegionContainer.Resized += Container_Changed;
			_ocrRegionContainer.Moved += Container_Changed;
			_ = _settings.ActiveOCRRegion.Size;
			base.Location = new Microsoft.Xna.Framework.Point(_ocrRegionContainer.Left, _ocrRegionContainer.Top - base.Height - 5);
			ForceOnScreen();
		}

		public void EnableMaskedRegion()
		{
			Microsoft.Xna.Framework.Rectangle b = _settings.ActiveOCRRegion;
			_maskedRegion.Size = b.Size;
			_maskedRegion.Location = b.Location;
			_maskedRegion?.Show();
		}

		public void DisableMaskedRegion()
		{
			_maskedRegion?.Hide();
		}

		private void Container_Changed(object sender, EventArgs e)
		{
			if (!_sizeSet)
			{
				string key = _settings.OCRKey;
				Dictionary<string, Microsoft.Xna.Framework.Rectangle> regions = _settings.OCRRegions.Value;
				Microsoft.Xna.Framework.Rectangle bounds = new Microsoft.Xna.Framework.Rectangle(_ocrRegionContainer.Left + _ocrRegionContainer.BorderWidth.Left, _ocrRegionContainer.Top + _ocrRegionContainer.BorderWidth.Top, _ocrRegionContainer.Width - _ocrRegionContainer.BorderWidth.Horizontal, _ocrRegionContainer.Height - _ocrRegionContainer.BorderWidth.Vertical);
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
			base.Location = new Microsoft.Xna.Framework.Point(_ocrRegionContainer.Left, _ocrRegionContainer.Top - base.Height - 5);
			Microsoft.Xna.Framework.Rectangle b = _settings.ActiveOCRRegion;
			_maskedRegion.Size = b.Size;
			_maskedRegion.Location = b.Location;
		}

		private void CloseButton_Click(object sender, MouseEventArgs e)
		{
			ToggleContainer();
		}

		public void ToggleContainer()
		{
			bool visible = this.ToggleVisibility();
			ForceOnScreen();
			_ocrRegionContainer?.ToggleVisibility(visible);
			_maskedRegion?.ToggleVisibility(visible);
			if (_ocrRegionContainer.Visible)
			{
				_sizeSet = true;
				_ocrRegionContainer.Location = _settings.ActiveOCRRegion.Location.Add(new Microsoft.Xna.Framework.Point(-_ocrRegionContainer.BorderWidth.Left, -_ocrRegionContainer.BorderWidth.Top));
				_ocrRegionContainer.Size = _settings.ActiveOCRRegion.Size.Add(new Microsoft.Xna.Framework.Point(_ocrRegionContainer.BorderWidth.Horizontal, _ocrRegionContainer.BorderWidth.Vertical));
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
			_maskedRegion.Visible = !_ocrRegionContainer.AbsoluteBounds.Contains(Control.Input.Mouse.Position);
			if (gameTime.TotalGameTime.TotalMilliseconds - _readTick > 250.0 && _maskedRegion.Visible)
			{
				_readTick = gameTime.TotalGameTime.TotalMilliseconds;
				string result = await _ocr.Read(show: true);
				if (result != null && _ocr.SourceTexture != null)
				{
					_sourceImage.Texture = _ocr.SourceTexture;
					_sourceImage.Size = _ocr.SourceTexture.Bounds.Size;
					_cleanedImage.Texture = _ocr.CleanedTexture;
					_cleanedImage.Size = _ocr.CleanedTexture.Bounds.Size;
					_scaledImage.Texture = _ocr.ScaledTexture;
					_scaledImage.Size = _ocr.ScaledTexture.Bounds.Size;
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
