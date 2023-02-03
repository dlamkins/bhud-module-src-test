using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace Ideka.RacingMeter
{
	public class MapPicker : Container
	{
		private const int Spacing = 10;

		private int _value;

		private readonly TextBox _textBox;

		private readonly StandardButton _button;

		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (_value != value)
				{
					_value = value;
					TextBox textBox = _textBox;
					string text;
					((Control)_textBox).set_BasicTooltipText(text = RacingModule.MapData.Describe(Value));
					((TextInputBase)textBox).set_Text(text);
					UpdateButton();
					this.ValueChanged?.Invoke(Value);
				}
			}
		}

		public event Action<int>? ValueChanged;

		public MapPicker()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Enabled(false);
			_textBox = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(Strings.ChangeToCurrent);
			_button = val2;
			UpdateLayout();
			UpdateButton();
			((Control)_button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Value = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			});
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
		}

		private void UpdateButton()
		{
			int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			bool alreadySet = mapId == Value;
			((Control)_button).set_Enabled(!alreadySet);
			((Control)_button).set_BasicTooltipText(alreadySet ? Strings.AlreadySetTooltip : RacingModule.MapData.Describe(mapId));
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (_textBox != null)
			{
				((Control)_button).set_Right(((Container)this).get_ContentRegion().Width);
				((Control)(object)_button).ArrangeRightLeft(10, (Control)_textBox);
				((Control)(object)_textBox).WidthFillLeft();
				((Container)(object)this).SetContentRegionHeight(Math.Max(((Control)_textBox).get_Height(), ((Control)_button).get_Height()));
				((Control)(object)_textBox).AlignMiddle();
				((Control)(object)_button).AlignMiddle();
			}
		}

		private void MapChanged(object sender, ValueEventArgs<int> e)
		{
			UpdateButton();
		}

		protected override void DisposeControl()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)MapChanged);
			((Container)this).DisposeControl();
		}
	}
}
