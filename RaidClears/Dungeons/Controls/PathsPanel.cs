using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using RaidClears.Dungeons.Model;
using Settings.Enums;

namespace RaidClears.Dungeons.Controls
{
	public class PathsPanel : FlowPanel
	{
		private Dungeon _dungeon;

		private DungeonOrientation _orientation;

		private DungeonLabel _labelDisplay;

		private Label _dungeonLabel;

		public PathsPanel(Container parent, Dungeon dungeon, DungeonOrientation orientation, DungeonLabel label, FontSize fontSize)
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
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected O, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			_dungeon = dungeon;
			((FlowPanel)this).set_ControlPadding(new Vector2(2f, 2f));
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Control)this).set_Parent(parent);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			Label val = new Label();
			val.set_AutoSizeHeight(true);
			((Control)val).set_BasicTooltipText(dungeon.GetTooltip());
			val.set_HorizontalAlignment(DungeonLabelAlignment());
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(GetWingLabelText());
			_dungeonLabel = val;
			Path[] paths = _dungeon.paths;
			foreach (Path path in paths)
			{
				Label val2 = new Label();
				val2.set_AutoSizeHeight(true);
				((Control)val2).set_BasicTooltipText(path.name);
				val2.set_HorizontalAlignment((HorizontalAlignment)1);
				((Control)val2).set_Parent((Container)(object)this);
				val2.set_Text(path.short_name);
				Label pathLabel = val2;
				path.SetLabelReference(pathLabel);
			}
			SetOrientation(orientation);
			SetLabelDisplay(label);
			SetFontSize(fontSize);
		}

		public string GetWingLabelText()
		{
			return _labelDisplay switch
			{
				DungeonLabel.NoLabel => "", 
				DungeonLabel.Abbreviation => _dungeon.shortName, 
				_ => "-", 
			};
		}

		private HorizontalAlignment DungeonLabelAlignment()
		{
			if (_orientation != 0)
			{
				return (HorizontalAlignment)1;
			}
			return (HorizontalAlignment)2;
		}

		public void SetOrientation(DungeonOrientation orientation)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			_orientation = orientation;
			((FlowPanel)this).set_FlowDirection(GetFlowDirection(orientation));
			_dungeonLabel.set_HorizontalAlignment(DungeonLabelAlignment());
		}

		private ControlFlowDirection GetFlowDirection(DungeonOrientation orientation)
		{
			return (ControlFlowDirection)(orientation switch
			{
				DungeonOrientation.Horizontal => 3, 
				DungeonOrientation.Vertical => 2, 
				DungeonOrientation.SingleRow => 2, 
				_ => 2, 
			});
		}

		public void SetLabelDisplay(DungeonLabel label)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_labelDisplay = label;
			_dungeonLabel.set_Text(GetWingLabelText());
			_dungeonLabel.set_HorizontalAlignment(DungeonLabelAlignment());
			if (label == DungeonLabel.NoLabel)
			{
				((Control)_dungeonLabel).Hide();
			}
			else
			{
				((Control)_dungeonLabel).Show();
			}
			((Control)this).Invalidate();
		}

		public void SetFontSize(FontSize fontSize)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			BitmapFont font = GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0);
			int width = GetLabelWidthForFontSize(fontSize);
			_dungeonLabel.set_Font(font);
			((Control)_dungeonLabel).set_Width(width);
			Path[] paths = _dungeon.paths;
			foreach (Path obj in paths)
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
			((Control)_dungeonLabel).set_Opacity(opacity);
		}

		public void SetEncounterOpacity(float opacity)
		{
			Path[] paths = _dungeon.paths;
			for (int i = 0; i < paths.Length; i++)
			{
				((Control)paths[i].GetLabelReference()).set_Opacity(opacity);
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
