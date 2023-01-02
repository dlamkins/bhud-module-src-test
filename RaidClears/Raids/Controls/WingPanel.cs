using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using RaidClears.Raids.Model;
using Settings.Enums;

namespace RaidClears.Raids.Controls
{
	public class WingPanel : FlowPanel
	{
		private Wing _wing;

		private Orientation _orientation;

		private WingLabel _labelDisplay;

		private Label _wingLabelObj;

		public WingPanel(Container parent, Wing wing, Orientation orientation, WingLabel label, FontSize fontSize, Color clearedColor, Color notClearedColor)
			: this()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			_wing = wing;
			((FlowPanel)this).set_ControlPadding(new Vector2(2f, 2f));
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Control)this).set_Parent(parent);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			Label val = new Label();
			val.set_AutoSizeHeight(true);
			((Control)val).set_BasicTooltipText(GetWingTooltip());
			val.set_HorizontalAlignment(WingLabelAlignment());
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(GetWingLabelText());
			_wingLabelObj = val;
			Encounter[] encounters = _wing.encounters;
			foreach (Encounter encounter in encounters)
			{
				encounter.SetClearColors(clearedColor, notClearedColor);
				Label val2 = new Label();
				val2.set_AutoSizeHeight(true);
				((Control)val2).set_BasicTooltipText(encounter.name);
				val2.set_HorizontalAlignment((HorizontalAlignment)1);
				((Control)val2).set_Parent((Container)(object)this);
				val2.set_Text(encounter.short_name);
				Label encounterLabel = val2;
				encounter.SetLabelReference(encounterLabel);
			}
			SetOrientation(orientation);
			SetLabelDisplay(label);
			SetFontSize(fontSize);
		}

		public string GetWingTooltip()
		{
			if (_wing.isCallOfTheMist)
			{
				return "(Call of the Mists) " + _wing.name;
			}
			if (_wing.isEmboldened)
			{
				return "(Emboldened) " + _wing.name;
			}
			return _wing.name;
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

		public void SetHighlightColor(Color color)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).get_Children().ToList().ForEach(delegate(Control elem)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				Label val = (Label)(object)((elem is Label) ? elem : null);
				if (val != null)
				{
					val.set_TextColor(color);
				}
			});
		}

		public void UpdateEncounterColors(Color cleared, Color notCleared)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < _wing.encounters.Length; i++)
			{
				_wing.encounters[i].UpdateColors(cleared, notCleared);
			}
		}

		public void SetFontSize(FontSize fontSize)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0);
			int width = GetLabelWidthForFontSize(fontSize);
			_wingLabelObj.set_Font(font);
			((Control)_wingLabelObj).set_Width(width);
			Encounter[] encounters = _wing.encounters;
			foreach (Encounter obj in encounters)
			{
				obj.GetLabelReference().set_Font(font);
				((Control)obj.GetLabelReference()).set_Width(width);
			}
		}

		public int GetLabelWidthForFontSize(FontSize size)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Invalid comparison between Unknown and I4
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Invalid comparison between Unknown and I4
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Invalid comparison between Unknown and I4
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Invalid comparison between Unknown and I4
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Invalid comparison between Unknown and I4
			if ((int)size <= 16)
			{
				if ((int)size <= 11)
				{
					if ((int)size == 8)
					{
						return 39;
					}
					if ((int)size == 11)
					{
						return 35;
					}
				}
				else if ((int)size == 14 || (int)size == 16)
				{
					return 40;
				}
			}
			else if ((int)size <= 24)
			{
				if ((int)size == 20 || (int)size == 24)
				{
					return 50;
				}
			}
			else if ((int)size == 32 || (int)size == 34)
			{
				return 80;
			}
			return 40;
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
