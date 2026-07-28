using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Presets.Model;
using Manlaan.CommanderMarkers.RtApi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class PositionFields : Container
	{
		private StandardButton _locBtn;

		private StandardButton? _importBtn;

		private Label _xPos;

		private Label _yPos;

		private Label _zPos;

		private WorldCoord _worldCoord;

		private readonly Func<int?>? _getRtApiSlotIndex;

		public event EventHandler<WorldCoord>? WorldCoordChanged;

		public PositionFields(WorldCoord? marker, Func<int?>? getRtApiSlotIndex = null)
			: this()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Expected O, but got Unknown
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Expected O, but got Unknown
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Expected O, but got Unknown
			_getRtApiSlotIndex = getRtApiSlotIndex;
			((Control)this).set_Size((_getRtApiSlotIndex != null) ? new Point(510, 30) : new Point(400, 30));
			_worldCoord = marker ?? new WorldCoord();
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Set Location");
			((Control)val).set_BasicTooltipText("Set the X, Y, Z location to where you are currently standing");
			((Control)val).set_Size(new Point(100, 30));
			((Control)val).set_Location(new Point(0, 0));
			_locBtn = val;
			int coordX = ((_getRtApiSlotIndex != null) ? 210 : 110);
			if (_getRtApiSlotIndex != null)
			{
				StandardButton val2 = new StandardButton();
				((Control)val2).set_Parent((Container)(object)this);
				val2.set_Text("Import");
				((Control)val2).set_Size(new Point(80, 30));
				((Control)val2).set_Location(new Point(105, 0));
				val2.set_Icon(AsyncTexture2D.op_Implicit(Service.Textures!.IconImport));
				((Control)val2).set_BasicTooltipText("Import this marker's position from squad markers placed in-game.\nRequires the Real-Time API addon.");
				((Control)val2).set_Enabled(Service.RtApiConnection?.IsActive ?? false);
				_importBtn = val2;
				((Control)_importBtn).add_Click((EventHandler<MouseEventArgs>)ImportBtn_Click);
				if (Service.RtApiConnection != null)
				{
					Service.RtApiConnection!.ConnectionStateChanged += new EventHandler<RtApiConnectionState>(OnRtApiConnectionStateChanged);
				}
			}
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(coordX, 0));
			((Control)val3).set_Size(new Point(15, 30));
			val3.set_Text("X:");
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(_worldCoord.x.ToString());
			((Control)val4).set_Size(new Point(85, 30));
			((Control)val4).set_Location(new Point(coordX + 15, 0));
			_xPos = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Location(new Point(coordX + 100, 0));
			((Control)val5).set_Size(new Point(15, 30));
			val5.set_Text("Y:");
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(_worldCoord.y.ToString());
			((Control)val6).set_Size(new Point(85, 30));
			((Control)val6).set_Location(new Point(coordX + 115, 0));
			_yPos = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)this);
			((Control)val7).set_Location(new Point(coordX + 200, 0));
			((Control)val7).set_Size(new Point(15, 30));
			val7.set_Text("Z:");
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text(_worldCoord.z.ToString());
			((Control)val8).set_Size(new Point(85, 30));
			((Control)val8).set_Location(new Point(coordX + 215, 0));
			_zPos = val8;
			((Control)_locBtn).add_Click((EventHandler<MouseEventArgs>)_locBtn_Click);
		}

		private void OnRtApiConnectionStateChanged(object? sender, RtApiConnectionState state)
		{
			if (_importBtn != null)
			{
				((Control)_importBtn).set_Enabled(state == RtApiConnectionState.Active);
			}
		}

		private void ImportBtn_Click(object sender, MouseEventArgs e)
		{
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			int? slotIndex = _getRtApiSlotIndex?.Invoke();
			if (slotIndex.HasValue && Service.RtApiConnection != null)
			{
				if (!Service.RtApiConnection!.EnsureActive())
				{
					ScreenNotification.ShowNotification("Real-Time API is not available.", (NotificationType)2, (Texture2D)null, 4);
					return;
				}
				if (!Service.RtApiConnection!.TryGetSquadMarkerPosition(slotIndex.Value, out var position))
				{
					ScreenNotification.ShowNotification("No squad marker is placed for this slot.", (NotificationType)2, (Texture2D)null, 4);
					return;
				}
				_worldCoord.x = position.X;
				_worldCoord.y = position.Y;
				_worldCoord.z = position.Z;
				ApplyWorldCoord(_worldCoord);
			}
		}

		private void _locBtn_Click(object sender, MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			Vector3 pos = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			_worldCoord.x = pos.X;
			_worldCoord.y = pos.Y;
			_worldCoord.z = pos.Z;
			ApplyWorldCoord(_worldCoord);
		}

		private void ApplyWorldCoord(WorldCoord coord)
		{
			_xPos.set_Text(coord.x.ToString());
			_yPos.set_Text(coord.y.ToString());
			_zPos.set_Text(coord.z.ToString());
			this.WorldCoordChanged?.Invoke(this, coord);
		}

		protected override void DisposeControl()
		{
			((Control)_locBtn).remove_Click((EventHandler<MouseEventArgs>)_locBtn_Click);
			if (_importBtn != null)
			{
				((Control)_importBtn).remove_Click((EventHandler<MouseEventArgs>)ImportBtn_Click);
			}
			if (Service.RtApiConnection != null)
			{
				Service.RtApiConnection!.ConnectionStateChanged -= new EventHandler<RtApiConnectionState>(OnRtApiConnectionStateChanged);
			}
		}
	}
}
