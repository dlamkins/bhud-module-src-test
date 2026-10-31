using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SeaStove.Gw2Tldr.UI
{
	internal sealed class MechanicTypeButton : Panel
	{
		private readonly Color _color;

		public MechanicIcon TypeIcon { get; }

		public MechanicTypeLabel Caption { get; }

		public MechanicTypeButton(string type, Color color)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			_color = color;
			MechanicIcon mechanicIcon = new MechanicIcon(type, color);
			((Control)mechanicIcon).set_Parent((Container)(object)this);
			((Control)mechanicIcon).set_Location(new Point(8, 4));
			((Control)mechanicIcon).set_Size(new Point(18, 18));
			mechanicIcon.CaptureMouse = false;
			((Control)mechanicIcon).set_BasicTooltipText("Choose mechanic type");
			TypeIcon = mechanicIcon;
			MechanicTypeLabel mechanicTypeLabel = new MechanicTypeLabel();
			((Control)mechanicTypeLabel).set_Parent((Container)(object)this);
			((Control)mechanicTypeLabel).set_Location(new Point(33, 1));
			((Control)mechanicTypeLabel).set_Size(new Point(100, 24));
			((Label)mechanicTypeLabel).set_Text(string.IsNullOrEmpty(type) ? "" : (char.ToUpperInvariant(type[0]) + type.Substring(1)));
			((Label)mechanicTypeLabel).set_TextColor(color);
			((Label)mechanicTypeLabel).set_VerticalAlignment((VerticalAlignment)1);
			((Control)mechanicTypeLabel).set_BasicTooltipText("Choose mechanic type");
			Caption = mechanicTypeLabel;
		}

		public override void PaintBeforeChildren(SpriteBatch batch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(batch, (Control)(object)this, Textures.get_Pixel(), bounds, OverlayTheme.Light ? new Color(205, 210, 215, 255) : new Color(40, 44, 50, 255));
			SpriteBatchExtensions.DrawOnCtrl(batch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, 3, bounds.Height), _color);
		}
	}
}
