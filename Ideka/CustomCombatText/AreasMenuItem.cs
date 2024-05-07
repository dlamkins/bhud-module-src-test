using System.Linq;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;

namespace Ideka.CustomCombatText
{
	public class AreasMenuItem : OnelineMenuItem
	{
		public AreaView Area { get; private set; }

		public AreasMenuItem(AreaView area, AreaView? opened)
			: base("")
		{
			Area = area;
			UpdateVisuals(Area, opened);
		}

		public void UpdateVisuals(AreaView area, AreaView? opened)
		{
			Area = area;
			((MenuItem)this).set_Text(((opened != null && opened != Area) ? "  " : "") + Area.Model.Describe);
		}

		public void UpdateTooltip()
		{
			string describe = Area.Model.Describe;
			int x2 = Area.Model.Receivers.Count;
			string text = x2 switch
			{
				1 => "1 receiver.", 
				0 => "No receivers.", 
				_ => $"{x2} receivers.", 
			};
			int x = Area.GetChildren().Count();
			string text2 = x switch
			{
				1 => "1 area inside.", 
				0 => "No areas inside.", 
				_ => $"{x} areas inside.", 
			};
			((Control)this).set_BasicTooltipText("Area: " + describe + "\n" + text + "\n" + text2 + "\nDouble click to open.");
		}
	}
}
