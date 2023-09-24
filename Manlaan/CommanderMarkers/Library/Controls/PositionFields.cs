using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.CommanderMarkers.Presets.Model;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Library.Controls
{
	public class PositionFields : Container
	{
		private StandardButton _locBtn;

		private Label _xPos;

		private Label _yPos;

		private Label _zPos;

		private WorldCoord _worldCoord;

		public event EventHandler<WorldCoord>? WorldCoordChanged;

		public PositionFields(WorldCoord? marker)
			: this()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected O, but got Unknown
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Expected O, but got Unknown
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Expected O, but got Unknown
			((Control)this).set_Size(new Point(400, 30));
			_worldCoord = marker ?? new WorldCoord();
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Set Location");
			((Control)val).set_BasicTooltipText("Set the X, Y, Z location to where you are currently standing");
			((Control)val).set_Size(new Point(100, 30));
			((Control)val).set_Location(new Point(0, 0));
			_locBtn = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(110, 0));
			((Control)val2).set_Size(new Point(15, 30));
			val2.set_Text("X:");
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(_worldCoord.x.ToString());
			((Control)val3).set_Size(new Point(85, 30));
			((Control)val3).set_Location(new Point(125, 0));
			_xPos = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(210, 0));
			((Control)val4).set_Size(new Point(15, 30));
			val4.set_Text("Y:");
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(_worldCoord.y.ToString());
			((Control)val5).set_Size(new Point(85, 30));
			((Control)val5).set_Location(new Point(225, 0));
			_yPos = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			((Control)val6).set_Location(new Point(310, 0));
			((Control)val6).set_Size(new Point(15, 30));
			val6.set_Text("Z:");
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text(_worldCoord.z.ToString());
			((Control)val7).set_Size(new Point(85, 30));
			((Control)val7).set_Location(new Point(325, 0));
			_zPos = val7;
			((Control)_locBtn).add_Click((EventHandler<MouseEventArgs>)_locBtn_Click);
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
			_xPos.set_Text(_worldCoord.x.ToString());
			_yPos.set_Text(_worldCoord.y.ToString());
			_zPos.set_Text(_worldCoord.z.ToString());
			this.WorldCoordChanged?.Invoke(this, _worldCoord);
		}

		protected override void DisposeControl()
		{
			((Control)_locBtn).remove_Click((EventHandler<MouseEventArgs>)_locBtn_Click);
		}
	}
}
