using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace RaidClears.Dungeons.Model
{
	public class Path
	{
		public string id;

		public string name;

		public string short_name;

		public bool is_cleared;

		private Label _label;

		private Color ColorUnknown = new Color(64, 64, 64, 196);

		private Color ColorNotCleared = new Color(120, 20, 20, 196);

		private Color ColorCleared = new Color(20, 120, 20, 196);

		public Path(string id, string name, string short_name)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			this.id = id;
			this.name = name;
			this.short_name = short_name;
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
		}

		public void SetFrequenter(bool done)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			_label.set_TextColor(done ? Color.get_Yellow() : Color.get_White());
		}
	}
}
