using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Presets.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class MarkerEditor : FlowPanel
	{
		private Action<MarkerEditor> _onDeleteCallback;

		private PositionFields? _position;

		private MarkerCoord? _markerCoord;

		private IconPicker? _iconPicker;

		public MarkerCoord Marker => _markerCoord ?? new MarkerCoord();

		public MarkerEditor(MarkerCoord marker, Action<MarkerEditor> onDeleteCallback)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Expected O, but got Unknown
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			MarkerCoord marker2 = marker;
			((FlowPanel)this)._002Ector();
			MarkerEditor markerEditor = this;
			_markerCoord = marker2;
			_onDeleteCallback = onDeleteCallback;
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)0);
			((FlowPanel)this).set_ControlPadding(new Vector2(10f, 5f));
			((FlowPanel)this).set_OuterControlPadding(new Vector2(0f, 10f));
			((Control)this).set_Size(new Point(450, 70));
			IconPicker iconPicker = new IconPicker();
			((Control)iconPicker).set_Parent((Container)(object)this);
			((Control)iconPicker).set_Size(new Point(300, 30));
			_iconPicker = iconPicker;
			List<(int, Texture2D)> list = new List<(int, Texture2D)>
			{
				(1, Service.Textures!._imgArrow),
				(2, Service.Textures!._imgCircle),
				(3, Service.Textures!._imgHeart),
				(4, Service.Textures!._imgSquare),
				(5, Service.Textures!._imgStar),
				(6, Service.Textures!._imgSpiral),
				(7, Service.Textures!._imgTriangle),
				(8, Service.Textures!._imgX)
			};
			_iconPicker!.LoadList(list);
			_iconPicker!.SelectItem(marker2.icon);
			_iconPicker!.IconSelectionChanged += delegate(object s, int e)
			{
				markerEditor._markerCoord!.icon = e;
			};
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((TextInputBase)val).set_Text(marker2.name);
			((Control)val).set_Size(new Point(100, 30));
			((Control)val).set_BasicTooltipText("Name the marker.\nHelpful for remembering which marker is where.");
			TextBox description = val;
			((TextInputBase)description).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				markerEditor._markerCoord!.name = ((TextInputBase)description).get_Text();
			});
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_BasicTooltipText("Delete marker");
			val2.set_Texture(AsyncTexture2D.op_Implicit(Service.Textures!.IconDelete));
			((Control)val2).set_Size(new Point(28, 28));
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				markerEditor._onDeleteCallback(markerEditor);
			});
			PositionFields positionFields = new PositionFields(marker2);
			((Control)positionFields).set_Parent((Container)(object)this);
			_position = positionFields;
			_position!.WorldCoordChanged += delegate(object s, WorldCoord e)
			{
				marker2.FromWorldCoord(e);
			};
		}

		protected override void DisposeControl()
		{
			IconPicker? iconPicker = _iconPicker;
			if (iconPicker != null)
			{
				((Control)iconPicker).Dispose();
			}
			PositionFields? position = _position;
			if (position != null)
			{
				((Control)position).Dispose();
			}
		}
	}
}
