using System;
using Blish_HUD.Controls;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class RaceMetaEditPanel : FlowPanel
	{
		private const int Spacing = 10;

		private readonly RaceEditor _editor;

		private readonly StringBox _nameBox;

		private readonly StringBox _idBox;

		private readonly EnumDropdown<RaceType> _typeSelect;

		private readonly MapPickerBox _mapPickerBox;

		public RaceMetaEditPanel(RaceEditor editor)
			: this()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			_editor = editor;
			((Panel)this).set_ShowTint(true);
			((Panel)this).set_Title(" ");
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(10f, 10f);
			((FlowPanel)this).set_OuterControlPadding(val);
			((FlowPanel)this).set_ControlPadding(val);
			StringBox stringBox = new StringBox();
			((Control)stringBox).set_Parent((Container)(object)this);
			stringBox.Label = Strings.RaceName;
			_nameBox = stringBox;
			StringBox stringBox2 = new StringBox();
			((Control)stringBox2).set_Parent((Container)(object)this);
			stringBox2.Label = Strings.RaceId;
			stringBox2.AllBasicTooltipText = Strings.RaceIdTooltip;
			stringBox2.ControlEnabled = false;
			_idBox = stringBox2;
			EnumDropdown<RaceType> enumDropdown = new EnumDropdown<RaceType>(new Func<RaceType, string>(DataExtensions.Describe), RaceType.Custom);
			((Control)enumDropdown).set_Parent((Container)(object)this);
			enumDropdown.Label = Strings.RaceType;
			_typeSelect = enumDropdown;
			MapPickerBox mapPickerBox = new MapPickerBox();
			((Control)mapPickerBox).set_Parent((Container)(object)this);
			mapPickerBox.Label = Strings.RaceMap;
			_mapPickerBox = mapPickerBox;
			UpdateLayout();
			_nameBox.ValueCommitted += delegate(string name)
			{
				_editor.RenameRace(name);
				UpdateValues();
			};
			_typeSelect.ValueCommitted += delegate(RaceType raceType)
			{
				_editor.SetRaceType(raceType);
				UpdateValues();
			};
			_mapPickerBox.ValueCommitted += delegate(int mapId)
			{
				_editor.SetRaceMap(mapId);
				UpdateValues();
			};
			RaceLoaded(_editor.FullRace);
			_editor.RaceLoaded += new Action<FullRace>(RaceLoaded);
			_editor.RaceModified += new Action<FullRace>(RaceModified);
		}

		private void UpdateValues()
		{
			((Panel)this).set_Title(StringExtensions.Format(Strings.EditingLabel, _editor.FullRace.Race.Name));
			_idBox.Value = _editor.FullRace.Meta.Id;
			_nameBox.Value = _editor.FullRace.Race.Name;
			_typeSelect.Value = _editor.FullRace.Race.Type;
			_mapPickerBox.Value = _editor.FullRace.Race.MapId;
		}

		private void RaceLoaded(FullRace _)
		{
			UpdateValues();
		}

		private void RaceModified(FullRace _)
		{
			UpdateValues();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			if (_editor != null)
			{
				StringBox nameBox = _nameBox;
				StringBox idBox = _idBox;
				EnumDropdown<RaceType> typeSelect = _typeSelect;
				int num;
				((Control)_mapPickerBox).set_Width(num = 400);
				int num2;
				((Control)typeSelect).set_Width(num2 = num);
				int width;
				((Control)idBox).set_Width(width = num2);
				((Control)nameBox).set_Width(width);
				ValueControl.AlignLabels(_nameBox, _idBox, _typeSelect, _mapPickerBox);
				((Container)(object)this).SetContentRegionWidth(((Control)_nameBox).get_Width() + 20);
				((Container)(object)this).SetContentRegionHeight(((Control)_mapPickerBox).get_Bottom() + 10);
			}
		}

		protected override void DisposeControl()
		{
			_editor.RaceLoaded -= new Action<FullRace>(RaceLoaded);
			_editor.RaceModified -= new Action<FullRace>(RaceModified);
			((FlowPanel)this).DisposeControl();
		}
	}
}
