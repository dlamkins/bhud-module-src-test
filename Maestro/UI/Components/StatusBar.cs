using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Components
{
	public class StatusBar : Panel
	{
		public static class Layout
		{
			public const int Height = 24;
		}

		private readonly Label _statusLabel;

		private int _visibleCount;

		private int _totalCount;

		public int VisibleCount
		{
			get
			{
				return _visibleCount;
			}
			set
			{
				_visibleCount = value;
				UpdateText();
			}
		}

		public int TotalCount
		{
			get
			{
				return _totalCount;
			}
			set
			{
				_totalCount = value;
				UpdateText();
			}
		}

		public StatusBar(int width)
			: this()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			((Control)this).set_Size(new Point(width, 24));
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(((Control)this).get_Width());
			((Control)val).set_Height(((Control)this).get_Height());
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroColors.LightGray);
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			_statusLabel = val;
		}

		private void UpdateText()
		{
			_statusLabel.set_Text((_visibleCount == _totalCount) ? $"  {_totalCount} songs" : $"  {_visibleCount} of {_totalCount} songs");
		}

		protected override void DisposeControl()
		{
			Label statusLabel = _statusLabel;
			if (statusLabel != null)
			{
				((Control)statusLabel).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
