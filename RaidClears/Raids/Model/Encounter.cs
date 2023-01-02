using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace RaidClears.Raids.Model
{
	public class Encounter
	{
		public string id;

		public string name;

		public string short_name;

		public bool is_cleared;

		private Label _label;

		private Color ColorUnknown = new Color(64, 64, 64);

		private Color ColorNotCleared = new Color(120, 20, 20);

		private Color ColorCleared = new Color(20, 120, 20);

		public Encounter(string id, string name, string short_name)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			this.id = id;
			this.name = name;
			this.short_name = short_name;
		}

		public void SetClearColors(Color cleared, Color notCleared)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			ColorCleared = cleared;
			ColorNotCleared = notCleared;
		}

		public void UpdateColors(Color cleared, Color notCleared)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			ColorNotCleared = notCleared;
			ColorCleared = cleared;
			((Control)_label).set_BackgroundColor(is_cleared ? ColorCleared : ColorNotCleared);
		}

		public void SetLabelReference(Label label)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			_label = label;
			((Control)_label).set_BackgroundColor(ColorUnknown);
		}

		public Label GetLabelReference()
		{
			return _label;
		}

		public void SetCleared(bool cleared)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			((Control)_label).set_BackgroundColor(cleared ? ColorCleared : ColorNotCleared);
			is_cleared = cleared;
		}
	}
}
