using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using RaidClears.Features.Shared.Controls;

namespace RaidClears.Features.Shared.Models
{
	public class BoxModel
	{
		public string id;

		public string name;

		public string shortName;

		private bool _isCleared;

		private readonly Color _colorUnknown = new Color(64, 64, 64);

		private Color _colorNotCleared = new Color(120, 20, 20);

		private Color _colorCleared = new Color(20, 120, 20);

		public GridBox Box { get; private set; }

		public BoxModel(string id, string name, string shortName)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			this.id = id;
			this.name = name;
			this.shortName = shortName;
		}

		public void SetClearColors(Color cleared, Color notCleared)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			_colorCleared = cleared;
			_colorNotCleared = notCleared;
			((Control)Box).set_BackgroundColor(_isCleared ? _colorCleared : _colorNotCleared);
			((Control)Box).Invalidate();
		}

		public void SetGridBoxReference(GridBox box)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			Box = box;
			((Control)Box).set_BackgroundColor(_colorUnknown);
		}

		public void SetCleared(bool cleared)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			((Control)Box).set_BackgroundColor(cleared ? _colorCleared : _colorNotCleared);
			_isCleared = cleared;
		}
	}
}
