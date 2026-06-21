using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Presets.Model;
using Manlaan.CommanderMarkers.RtApi;
using Manlaan.CommanderMarkers.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class MarkerSetEditor : FlowPanel
	{
		protected int _updateListingIndex = -1;

		private Action<bool> _returnToList;

		protected MarkerSet _markerSet = new MarkerSet();

		protected StandardButton? _AddMarkerButton;

		protected StandardButton? _importAllButton;

		public MarkerSet MarkerSet => _markerSet;

		public MarkerSetEditor(Action<bool> callback)
			: this()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			((FlowPanel)this).set_ControlPadding(new Vector2(5f, 5f));
			_returnToList = callback;
		}

		public void LoadMarkerSet(MarkerSet? markerSet, int idx)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Expected O, but got Unknown
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Expected O, but got Unknown
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Expected O, but got Unknown
			_markerSet = markerSet ?? new MarkerSet();
			_updateListingIndex = idx;
			((Container)this).ClearChildren();
			if (RtApiIntegrationHelper.IsEnabled)
			{
				Service.RtApiConnection?.EnsureActive();
			}
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)0);
			val.set_ControlPadding(new Vector2(10f, 5f));
			((Control)val).set_Size(new Point(450, 135));
			FlowPanel metaFlow = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)metaFlow);
			val2.set_Text("Name");
			((Control)val2).set_Size(new Point(99, 30));
			((Control)val2).set_BasicTooltipText("The name shown on the map when you are within range of using the marker set");
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)metaFlow);
			((Control)val3).set_Location(new Point(0, 0));
			((Control)val3).set_Size(new Point(299, 30));
			((TextInputBase)val3).set_Text(_markerSet.name);
			((Control)val3).set_BasicTooltipText("The name shown on the map when you are within range of using the marker set");
			TextBox title = val3;
			((TextInputBase)title).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				_markerSet.name = ((TextInputBase)title).get_Text();
			});
			IconButton iconButton = new IconButton();
			((Control)iconButton).set_Parent((Container)(object)metaFlow);
			iconButton.Icon = Service.Textures!.IconEye;
			((Control)iconButton).set_BasicTooltipText("Preview");
			((Control)iconButton).set_Size(new Point(30, 30));
			((Control)iconButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				Service.MapWatch.PreviewMarkerSet(_markerSet);
			});
			((Control)iconButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				Service.MapWatch.RemovePreviewMarkerSet();
			});
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)metaFlow);
			val4.set_Text("Description");
			((Control)val4).set_Size(new Point(100, 30));
			((Control)val4).set_BasicTooltipText("This text is shown on the map when you are within range of using the marker set");
			TextBox val5 = new TextBox();
			((Control)val5).set_Parent((Container)(object)metaFlow);
			((Control)val5).set_Location(new Point(0, 0));
			((Control)val5).set_Size(new Point(300, 30));
			((TextInputBase)val5).set_Text(_markerSet.description);
			((Control)val5).set_BasicTooltipText("This text is shown on the map when you are within range of using the marker set");
			TextBox description = val5;
			((TextInputBase)description).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				_markerSet.description = ((TextInputBase)description).get_Text();
			});
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)metaFlow);
			((Control)val6).set_Size(new Point(100, 30));
			val6.set_Text("Trigger Location");
			((Control)val6).set_BasicTooltipText("Location to be near to activate this marker set");
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)metaFlow);
			((Control)val7).set_Size(new Point(300, 30));
			val7.set_Text("Map: " + Service.MapDataCache.Describe(_markerSet.MapId));
			((Control)val7).set_BasicTooltipText("Set trigger location to update map");
			Label label = val7;
			PositionFields positionFields = new PositionFields(_markerSet.trigger);
			((Control)positionFields).set_Parent((Container)(object)metaFlow);
			positionFields.WorldCoordChanged += delegate(object s, WorldCoord e)
			{
				_markerSet.trigger = e;
				_markerSet.mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				label.set_Text("Map: " + Service.MapDataCache.Describe(_markerSet.MapId));
			};
			if (RtApiIntegrationHelper.IsEnabled)
			{
				StandardButton val8 = new StandardButton();
				((Control)val8).set_Parent((Container)(object)this);
				val8.set_Text("Import active squad markers");
				((Control)val8).set_Width(410);
				val8.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconImport));
				((Control)val8).set_BasicTooltipText("Copy currently placed squad marker locations from the Real-Time API.\nRequires the Real-Time API addon.");
				((Control)val8).set_Enabled(Service.RtApiConnection?.IsActive ?? false);
				_importAllButton = val8;
				((Control)_importAllButton).add_Click((EventHandler<MouseEventArgs>)ImportAllButton_Click);
				if (Service.RtApiConnection != null)
				{
					Service.RtApiConnection!.ConnectionStateChanged += new EventHandler<RtApiConnectionState>(OnRtApiConnectionStateChanged);
				}
			}
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)this);
			val9.set_Text("Add Marker");
			((Control)val9).set_Width(410);
			((Control)val9).set_Enabled(_markerSet.marks.Count < 8);
			_AddMarkerButton = val9;
			_markerSet.marks.ForEach(delegate(MarkerCoord mark)
			{
				((Control)new MarkerEditor(mark, new Action<MarkerEditor>(RemoveMarker))).set_Parent((Container)(object)this);
			});
			((Control)_AddMarkerButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_markerSet.marks.Count < 8)
				{
					MarkerCoord markerCoord = new MarkerCoord();
					markerCoord.SetFromMumbleLocation();
					_markerSet.marks.Add(markerCoord);
					((Control)new MarkerEditor(markerCoord, new Action<MarkerEditor>(RemoveMarker))).set_Parent((Container)(object)this);
				}
				((Control)_AddMarkerButton).set_Enabled(_markerSet.marks.Count < 8);
			});
		}

		private void OnRtApiConnectionStateChanged(object? sender, RtApiConnectionState state)
		{
			if (_importAllButton != null)
			{
				((Control)_importAllButton).set_Enabled(state == RtApiConnectionState.Active);
			}
		}

		private void ImportAllButton_Click(object sender, MouseEventArgs e)
		{
			if (Service.RtApiConnection == null || !Service.RtApiConnection!.EnsureActive())
			{
				ScreenNotification.ShowNotification("Real-Time API is not available.", (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			List<MarkerCoord> imported = new List<MarkerCoord>();
			for (int slotIndex = 0; slotIndex < 8; slotIndex++)
			{
				MarkerCoord marker = new MarkerCoord();
				if (Service.RtApiConnection!.TryImportSquadMarker(slotIndex, marker))
				{
					imported.Add(marker);
				}
			}
			if (imported.Count == 0)
			{
				ScreenNotification.ShowNotification("No active squad markers were found to import.", (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			_markerSet.marks = imported;
			_markerSet.mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			RebuildMarkerEditors();
		}

		private void RebuildMarkerEditors()
		{
			foreach (MarkerEditor editor in ((IEnumerable)((Container)this).get_Children()).OfType<MarkerEditor>().ToList())
			{
				((Container)this).RemoveChild((Control)(object)editor);
				((Control)editor).Dispose();
			}
			foreach (MarkerCoord mark in _markerSet.marks)
			{
				((Control)new MarkerEditor(mark, new Action<MarkerEditor>(RemoveMarker))).set_Parent((Container)(object)this);
			}
			if (_AddMarkerButton != null)
			{
				((Control)_AddMarkerButton).set_Enabled(_markerSet.marks.Count < 8);
			}
		}

		protected void RemoveMarker(MarkerEditor editor)
		{
			((Container)this).get_Children().Remove((Control)(object)editor);
			_markerSet.marks.Remove(editor.Marker);
			((Control)_AddMarkerButton).set_Enabled(_markerSet.marks.Count < 8);
			((Control)this).Invalidate();
		}

		protected override void DisposeControl()
		{
			if (Service.RtApiConnection != null)
			{
				Service.RtApiConnection!.ConnectionStateChanged -= new EventHandler<RtApiConnectionState>(OnRtApiConnectionStateChanged);
			}
			((FlowPanel)this).DisposeControl();
		}
	}
}
