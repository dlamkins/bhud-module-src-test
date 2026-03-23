using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SongbookOfTyria.Models;
using SongbookOfTyria.Services;
using SongbookOfTyria.UI.Utilities;

namespace SongbookOfTyria.UI.Controls
{
	public sealed class TabDetailsPanel : FlowPanel
	{
		private const int ThumbnailMaxWidth = 240;

		private const int ThumbnailMaxHeight = 220;

		private const int DetailLabelWidth = 120;

		private const int DetailValueWidth = 120;

		private const int PracticeModeIconAssetId = 528696;

		private static readonly Logger Logger = Logger.GetLogger<TabDetailsPanel>();

		private readonly MusicTab _musicTab;

		private readonly TextureService _textureService;

		private readonly AsyncTexture2D _practiceModeIconTexture;

		private bool _lastCollapsedState;

		private bool _isHandlingResize;

		public event EventHandler<bool> CollapsedChanged;

		public TabDetailsPanel(MusicTab musicTab, TextureService textureService, int panelWidth, bool collapsed)
			: this()
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			_musicTab = musicTab;
			_textureService = textureService;
			_lastCollapsedState = collapsed;
			if (_musicTab.PracticeMode)
			{
				_practiceModeIconTexture = AsyncTexture2D.FromAssetId(528696);
			}
			((Panel)this).set_ShowBorder(true);
			((Panel)this).set_Title("Details");
			((Panel)this).set_CanCollapse(true);
			((Control)this).set_Width(panelWidth);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 5f));
			((FlowPanel)this).set_OuterControlPadding(new Vector2(0f, 0f));
			BuildContent();
			((Panel)this).set_Collapsed(collapsed);
			((Panel)this).set_Title(collapsed ? " " : "Details");
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnResized);
		}

		private void BuildContent()
		{
			BuildThumbnail();
			AddDetailRow("Genre:", _musicTab.Genre);
			if (_musicTab.IsBeginner)
			{
				AddDetailRow("Beginner Tab:", "Yes");
			}
			if (_musicTab.TabType != null && _musicTab.TabType.Count > 0)
			{
				AddDetailRow("Song type:", string.Join(", ", _musicTab.TabType));
			}
			AddDetailRow("Released Date:", _musicTab.ReleaseDate);
			AddDetailRow("Tabbed by:", GetTabberName());
			if (_musicTab.ArrangerInfo != null && _musicTab.ArrangerInfo.Count > 0)
			{
				AddArrangerPicture(_musicTab.ArrangerInfo[0]);
			}
			AddViewOnWebsiteButton();
		}

		private void BuildThumbnail()
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(_musicTab.Thumbnail))
			{
				AsyncTexture2D thumbnailTexture = _textureService.GetRemoteTexture(_musicTab.Thumbnail);
				if (thumbnailTexture != null)
				{
					int detailsInnerWidth = ((Control)this).get_Width();
					Point imageSize = ImageSizeCalculator.CalculateAspectRatioSize(thumbnailTexture.get_Width(), thumbnailTexture.get_Height(), detailsInnerWidth - 20, 220);
					Panel val = new Panel();
					((Control)val).set_Width(detailsInnerWidth);
					((Control)val).set_Height(imageSize.Y + 15);
					((Control)val).set_Parent((Container)(object)this);
					Panel imageContainer = val;
					int centeredX = (((Control)imageContainer).get_Width() - imageSize.X) / 2;
					Image val2 = new Image(thumbnailTexture);
					((Control)val2).set_Size(imageSize);
					((Control)val2).set_Location(new Point(centeredX, 10));
					((Control)val2).set_Parent((Container)(object)imageContainer);
				}
			}
		}

		private string GetTabberName()
		{
			if (!string.IsNullOrEmpty(_musicTab.TabbedBy))
			{
				return _musicTab.TabbedBy;
			}
			ArrangerInfo arranger = _musicTab.ArrangerInfo?.FirstOrDefault();
			if (arranger != null)
			{
				string name = ((!string.IsNullOrEmpty(arranger.DisplayName)) ? arranger.DisplayName : arranger.Username);
				if (!string.IsNullOrEmpty(name))
				{
					return name;
				}
			}
			List<string> tabbedByMember = _musicTab.TabbedByMember;
			if (tabbedByMember != null && tabbedByMember.Count > 0)
			{
				return string.Join(", ", _musicTab.TabbedByMember);
			}
			return null;
		}

		private void AddDetailRow(string label, string value)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(value))
			{
				FlowPanel val = new FlowPanel();
				val.set_FlowDirection((ControlFlowDirection)2);
				((Control)val).set_Width(240);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				val.set_ControlPadding(new Vector2(5f, 0f));
				val.set_OuterControlPadding(new Vector2(15f, 0f));
				((Control)val).set_Parent((Container)(object)this);
				FlowPanel rowPanel = val;
				Label val2 = new Label();
				val2.set_Text(label);
				val2.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val2).set_Width(120);
				val2.set_AutoSizeHeight(true);
				((Control)val2).set_Parent((Container)(object)rowPanel);
				Label val3 = new Label();
				val3.set_Text(WebUtility.HtmlDecode(value));
				val3.set_Font(GameService.Content.get_DefaultFont14());
				((Control)val3).set_Width(120);
				val3.set_AutoSizeHeight(true);
				val3.set_WrapText(true);
				((Control)val3).set_Parent((Container)(object)rowPanel);
			}
		}

		private void AddArrangerPicture(ArrangerInfo arrangerInfo)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			if (string.IsNullOrEmpty(arrangerInfo.PictureUrl))
			{
				return;
			}
			AsyncTexture2D arrangerTexture = _textureService.GetRemoteTexture(arrangerInfo.PictureUrl);
			if (arrangerTexture == null)
			{
				return;
			}
			Panel val = new Panel();
			((Control)val).set_Width(240);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)this);
			Panel picturePanel = val;
			Image val2 = new Image(arrangerTexture);
			((Control)val2).set_Size(new Point(220, 220));
			((Control)val2).set_Location(new Point(10, 10));
			((Control)val2).set_Parent((Container)(object)picturePanel);
			Image arrangerImage = val2;
			if (arrangerTexture.get_HasTexture())
			{
				UpdateArrangerImageSize(arrangerImage, picturePanel, arrangerTexture.get_Texture());
			}
			arrangerTexture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate(object sender, ValueChangedEventArgs<Texture2D> e)
			{
				if (e.get_NewValue() != null)
				{
					UpdateArrangerImageSize(arrangerImage, picturePanel, e.get_NewValue());
				}
			});
		}

		private void UpdateArrangerImageSize(Image image, Panel panel, Texture2D texture)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			Point imageSize = ImageSizeCalculator.CalculateAspectRatioSize(texture.get_Width(), texture.get_Height(), 220, 220);
			((Control)image).set_Size(imageSize);
			((Control)image).set_Location(new Point((((Control)panel).get_Width() - imageSize.X) / 2, 10));
			((Control)panel).set_Height(imageSize.Y + 35);
		}

		private void AddViewOnWebsiteButton()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(_musicTab.Url))
			{
				return;
			}
			Panel val = new Panel();
			((Control)val).set_Width(((Control)this).get_Width());
			((Control)val).set_Height(45);
			((Control)val).set_Parent((Container)(object)this);
			Panel buttonPanel = val;
			int buttonWidth = 140;
			int iconSize = 20;
			int iconSpacing = 4;
			int totalWidth = (_musicTab.PracticeMode ? (buttonWidth + iconSize + iconSpacing) : buttonWidth);
			int startX = (((Control)buttonPanel).get_Width() - totalWidth) / 2;
			StandardButton val2 = new StandardButton();
			val2.set_Text("View on Website");
			((Control)val2).set_Width(buttonWidth);
			((Control)val2).set_Location(new Point(startX, 10));
			((Control)val2).set_Parent((Container)(object)buttonPanel);
			if (_musicTab.PracticeMode && _practiceModeIconTexture != null)
			{
				Image val3 = new Image(_practiceModeIconTexture);
				((Control)val3).set_Size(new Point(iconSize, iconSize));
				((Control)val3).set_Location(new Point(startX + buttonWidth + iconSpacing, 12));
				((Control)val3).set_BasicTooltipText("Practice Mode Available");
				((Control)val3).set_Parent((Container)(object)buttonPanel);
			}
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					Process.Start(new ProcessStartInfo
					{
						FileName = _musicTab.Url,
						UseShellExecute = true
					});
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to open URL: {Url}", new object[1] { _musicTab.Url });
				}
			});
		}

		private void OnResized(object sender, ResizedEventArgs e)
		{
			if (_isHandlingResize)
			{
				return;
			}
			_isHandlingResize = true;
			try
			{
				((Panel)this).set_Title(((Panel)this).get_Collapsed() ? " " : "Details");
				if (((Panel)this).get_Collapsed() != _lastCollapsedState)
				{
					_lastCollapsedState = ((Panel)this).get_Collapsed();
					this.CollapsedChanged?.Invoke(this, ((Panel)this).get_Collapsed());
				}
			}
			finally
			{
				_isHandlingResize = false;
			}
		}

		public void RebuildContent()
		{
			Control[] array = ((Container)this).get_Children().ToArray();
			foreach (Control obj in array)
			{
				obj.set_Parent((Container)null);
				obj.Dispose();
			}
			BuildContent();
			((Control)this).Invalidate();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			if (((Panel)this).get_Collapsed() != _lastCollapsedState)
			{
				_lastCollapsedState = ((Panel)this).get_Collapsed();
				this.CollapsedChanged?.Invoke(this, ((Panel)this).get_Collapsed());
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Resized((EventHandler<ResizedEventArgs>)OnResized);
			((FlowPanel)this).DisposeControl();
		}
	}
}
