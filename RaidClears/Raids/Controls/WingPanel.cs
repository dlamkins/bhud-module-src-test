using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using RaidClears.Raids.Model;
using RaidClears.Settings;

namespace RaidClears.Raids.Controls
{
	public class WingPanel : FlowPanel
	{
		private Wing _wing;

		private Orientation _orientation;

		private WingLabel _labelDisplay;

		private Label _wingLabelObj;

		public WingPanel(Container parent, Wing wing, Orientation orientation, WingLabel label, FontSize fontSize)
			: this()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected O, but got Unknown
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			_wing = wing;
			((FlowPanel)this).set_ControlPadding(new Vector2(2f, 2f));
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Control)this).set_Parent(parent);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			Label val = new Label();
			val.set_AutoSizeHeight(true);
			((Control)val).set_BasicTooltipText(wing.name);
			val.set_HorizontalAlignment(WingLabelAlignment());
			((Control)val).set_Opacity(1f);
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(GetWingLabelText());
			_wingLabelObj = val;
			Encounter[] encounters = _wing.encounters;
			foreach (Encounter encounter in encounters)
			{
				Label val2 = new Label();
				val2.set_AutoSizeHeight(true);
				((Control)val2).set_BasicTooltipText(encounter.name);
				val2.set_HorizontalAlignment((HorizontalAlignment)1);
				((Control)val2).set_Opacity(1f);
				((Control)val2).set_Parent((Container)(object)this);
				val2.set_Text(encounter.short_name);
				Label encounterLabel = val2;
				encounter.SetLabelReference(encounterLabel);
			}
			SetOrientation(orientation);
			SetLabelDisplay(label);
			SetFontSize(fontSize);
		}

		public string GetWingLabelText()
		{
			return _labelDisplay switch
			{
				WingLabel.NoLabel => "", 
				WingLabel.WingNumber => _wing.index.ToString(), 
				WingLabel.Abbreviation => _wing.shortName, 
				_ => "-", 
			};
		}

		private HorizontalAlignment WingLabelAlignment()
		{
			if (_orientation == Orientation.Vertical)
			{
				if (_labelDisplay != 0)
				{
					return (HorizontalAlignment)2;
				}
				return (HorizontalAlignment)1;
			}
			return (HorizontalAlignment)1;
		}

		public void SetOrientation(Orientation orientation)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			_orientation = orientation;
			((FlowPanel)this).set_FlowDirection(GetFlowDirection(orientation));
			_wingLabelObj.set_HorizontalAlignment(WingLabelAlignment());
		}

		private ControlFlowDirection GetFlowDirection(Orientation orientation)
		{
			return (ControlFlowDirection)(orientation switch
			{
				Orientation.Horizontal => 3, 
				Orientation.Vertical => 2, 
				Orientation.SingleRow => 2, 
				_ => 2, 
			});
		}

		public void SetLabelDisplay(WingLabel label)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_labelDisplay = label;
			_wingLabelObj.set_Text(GetWingLabelText());
			_wingLabelObj.set_HorizontalAlignment(WingLabelAlignment());
			if (label == WingLabel.NoLabel)
			{
				((Control)_wingLabelObj).Hide();
			}
			else
			{
				((Control)_wingLabelObj).Show();
			}
			((Control)this).Invalidate();
		}

		public void SetFontSize(FontSize fontSize)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0);
			_wingLabelObj.set_Font(font);
			Encounter[] encounters = _wing.encounters;
			for (int i = 0; i < encounters.Length; i++)
			{
				encounters[i].GetLabelReference().set_Font(font);
			}
		}

		public void SetWingLabelOpacity(float opacity)
		{
			((Control)_wingLabelObj).set_Opacity(opacity);
		}

		public void SetEncounterOpacity(float opacity)
		{
			Encounter[] encounters = _wing.encounters;
			for (int i = 0; i < encounters.Length; i++)
			{
				((Control)encounters[i].GetLabelReference()).set_Opacity(opacity);
			}
		}

		public void ShowHide(bool shouldShow)
		{
			if (((Control)this).get_Visible() && !shouldShow)
			{
				((Control)this).Hide();
			}
			if (!((Control)this).get_Visible() && shouldShow)
			{
				((Control)this).Show();
			}
		}
	}
}
