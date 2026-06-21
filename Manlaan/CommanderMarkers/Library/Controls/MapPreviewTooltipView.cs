using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Manlaan.CommanderMarkers.Library.Enums;
using Manlaan.CommanderMarkers.Library.Models;
using Manlaan.CommanderMarkers.Presets.Model;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public sealed class MapPreviewTooltipView : View, ITooltipView, IView
	{
		private const int PreviewSize = 768;

		private const int LegendPad = 8;

		private static readonly Color LegendBackground = new Color(12, 12, 12, 205);

		private readonly MapPreviewTarget _target;

		private Container? _buildPanel;

		private Panel? _previewContainer;

		private Image? _previewImage;

		private FlowPanel? _legendPanel;

		private int _detailFetchGeneration;

		public MapPreviewTooltipView(MapPreviewTarget target)
			: this()
		{
			_target = target;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Expected O, but got Unknown
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Expected O, but got Unknown
			if (_buildPanel != null)
			{
				return;
			}
			_buildPanel = buildPanel;
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 4f));
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			FlowPanel root = val;
			if (!string.IsNullOrWhiteSpace(_target.Label))
			{
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)root);
				val2.set_Text(_target.Label);
				val2.set_AutoSizeWidth(true);
				val2.set_AutoSizeHeight(true);
				val2.set_ShowShadow(true);
			}
			string description = _target.Description?.Replace("\r\n", "\n").Replace("\r", "\n") ?? "";
			if (!string.IsNullOrWhiteSpace(description))
			{
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)root);
				val3.set_Text(description);
				((Control)val3).set_Width(768);
				val3.set_AutoSizeHeight(true);
				val3.set_WrapText(true);
				val3.set_ShowShadow(true);
			}
			bool hasPreviewImage = !string.IsNullOrEmpty(_target.CommunitySetId);
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)root);
			((Control)val4).set_Size(new Point(768, 768));
			((Control)val4).set_Visible(hasPreviewImage);
			_previewContainer = val4;
			Image val5 = new Image();
			((Control)val5).set_Parent((Container)(object)_previewContainer);
			((Control)val5).set_Location(Point.get_Zero());
			((Control)val5).set_Size(new Point(768, 768));
			((Control)val5).set_Visible(false);
			_previewImage = val5;
			FlowPanel val6 = new FlowPanel();
			((Control)val6).set_Parent((Container)(hasPreviewImage ? ((object)_previewContainer) : ((object)root)));
			val6.set_FlowDirection((ControlFlowDirection)3);
			val6.set_ControlPadding(new Vector2(8f, 8f));
			((Container)val6).set_WidthSizingMode((SizingMode)1);
			((Container)val6).set_HeightSizingMode((SizingMode)1);
			((Control)val6).set_BackgroundColor(LegendBackground);
			((Control)val6).set_Visible(false);
			_legendPanel = val6;
			if (hasPreviewImage)
			{
				((Control)_legendPanel).add_Resized((EventHandler<ResizedEventArgs>)delegate
				{
					UpdateLegendPosition();
				});
			}
			ApplyPreviewTexture();
			RenderLegend(_target.Markers);
			RequestPreviewIfNeeded();
			RequestDetailIfNeeded();
		}

		private void RequestPreviewIfNeeded()
		{
			if (!string.IsNullOrEmpty(_target.CommunitySetId))
			{
				Service.PreviewImageCache.RequestPreview(_target.CommunitySetId, _target.PreviewLargeUrl, delegate
				{
					GameThreadUtil.Enqueue(new Action(ApplyPreviewTexture));
				});
			}
		}

		private void RequestDetailIfNeeded()
		{
			if (_target.Markers.Count > 0 || string.IsNullOrEmpty(_target.CommunitySetId))
			{
				return;
			}
			string setId = _target.CommunitySetId;
			int generation = ++_detailFetchGeneration;
			Task.Run(delegate
			{
				MarkerSet fetched = Service.CommunityCatalog.FetchSetDetail(setId);
				if (fetched != null)
				{
					GameThreadUtil.Enqueue(delegate
					{
						if (generation == _detailFetchGeneration)
						{
							FlowPanel? legendPanel = _legendPanel;
							if (((legendPanel != null) ? ((Control)legendPanel).get_Parent() : null) != null)
							{
								RenderLegend(fetched.marks);
							}
						}
					});
				}
			});
		}

		private void ApplyPreviewTexture()
		{
			if (_previewImage != null && !string.IsNullOrEmpty(_target.CommunitySetId))
			{
				Texture2D texture = Service.PreviewImageCache.GetPreviewTexture(_target.CommunitySetId);
				if (texture != null)
				{
					_previewImage!.set_Texture(AsyncTexture2D.op_Implicit(texture));
					((Control)_previewImage).set_Visible(true);
					InvalidateLayout();
				}
			}
		}

		private void InvalidateLayout()
		{
			Container? buildPanel = _buildPanel;
			if (buildPanel != null)
			{
				((Control)buildPanel).Invalidate();
			}
			UpdateLegendPosition();
		}

		private void UpdateLegendPosition()
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			if (_legendPanel != null && _previewContainer != null && ((Control)_legendPanel).get_Visible() && ((Control)_legendPanel).get_Parent() == _previewContainer)
			{
				int legendHeight = ((Control)_legendPanel).get_Height();
				int containerHeight = ((Control)_previewContainer).get_Height();
				((Control)_legendPanel).set_Location(new Point(8, Math.Max(8, containerHeight - legendHeight - 8)));
			}
		}

		private void RenderLegend(IEnumerable<MarkerCoord> markers)
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Expected O, but got Unknown
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			if (_legendPanel == null)
			{
				return;
			}
			((Container)_legendPanel).get_Children().Clear();
			List<MarkerCoord> entries = (from m in markers
				where !string.IsNullOrWhiteSpace(m.name) && m.icon != 9
				orderby m.icon
				select m).ToList();
			((Control)_legendPanel).set_Visible(entries.Count > 0);
			foreach (MarkerCoord mark in entries)
			{
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent((Container)(object)_legendPanel);
				val.set_FlowDirection((ControlFlowDirection)2);
				val.set_ControlPadding(new Vector2(6f, 0f));
				((Container)val).set_WidthSizingMode((SizingMode)1);
				((Control)val).set_Height(22);
				FlowPanel row = val;
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)row);
				((Control)val2).set_Size(new Point(18, 18));
				val2.set_Texture(AsyncTexture2D.op_Implicit(((SquadMarker)mark.icon).GetIcon()));
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)row);
				val3.set_Text(mark.name);
				val3.set_AutoSizeWidth(true);
				val3.set_AutoSizeHeight(true);
				val3.set_TextColor(Color.get_White());
				val3.set_ShowShadow(true);
			}
			GameThreadUtil.Enqueue(delegate
			{
				FlowPanel? legendPanel = _legendPanel;
				if (((legendPanel != null) ? ((Control)legendPanel).get_Parent() : null) == _previewContainer)
				{
					UpdateLegendPosition();
				}
			});
			InvalidateLayout();
		}
	}
}
