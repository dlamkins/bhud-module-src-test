using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Presets.Model;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class MarkerSetEditor : FlowPanel
	{
		protected int _updateListingIndex = -1;

		private Action<bool> _returnToList;

		protected MarkerSet _markerSet = new MarkerSet();

		protected StandardButton? _AddMarkerButton;

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
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Expected O, but got Unknown
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Expected O, but got Unknown
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Expected O, but got Unknown
			_markerSet = markerSet ?? new MarkerSet();
			_updateListingIndex = idx;
			((Container)this).ClearChildren();
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
			PositionFields positionFields = new PositionFields(markerSet!.Trigger);
			((Control)positionFields).set_Parent((Container)(object)metaFlow);
			positionFields.WorldCoordChanged += delegate(object s, WorldCoord e)
			{
				_markerSet.trigger = e;
				_markerSet.mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				label.set_Text("Map: " + Service.MapDataCache.Describe(_markerSet.MapId));
			};
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text("Add Marker");
			((Control)val8).set_Width(410);
			((Control)val8).set_Enabled(_markerSet.marks.Count < 8);
			_AddMarkerButton = val8;
			markerSet!.marks.ForEach(delegate(MarkerCoord mark)
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

		protected void RemoveMarker(MarkerEditor editor)
		{
			((Container)this).get_Children().Remove((Control)(object)editor);
			_markerSet.marks.Remove(editor.Marker);
			((Control)_AddMarkerButton).set_Enabled(_markerSet.marks.Count < 8);
			((Control)this).Invalidate();
		}
	}
}
