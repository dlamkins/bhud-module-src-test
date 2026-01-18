using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Components
{
	public class StatusBar : Panel
	{
		public static class Layout
		{
			public static int Height => 26;
		}

		private readonly Label _statusLabel;

		private readonly StandardButton _communityButton;

		private readonly StandardButton _importButton;

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

		public event EventHandler ImportClicked;

		public event EventHandler CommunityClicked;

		public StatusBar(int width)
			: this()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Expected O, but got Unknown
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Expected O, but got Unknown
			((Control)this).set_Size(new Point(width, Layout.Height));
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			int buttonsWidth = 185;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(width - buttonsWidth - 10);
			((Control)val).set_Height(((Control)this).get_Height());
			val.set_Font(GameService.Content.get_DefaultFont12());
			val.set_TextColor(MaestroTheme.LightGray);
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			_statusLabel = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Community");
			((Control)val2).set_Location(new Point(width - buttonsWidth, 0));
			((Control)val2).set_Size(new Point(90, 26));
			((Control)val2).set_Enabled(false);
			((Control)val2).set_BasicTooltipText("Coming soon!");
			_communityButton = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Import");
			((Control)val3).set_Location(new Point(width - 90, 0));
			((Control)val3).set_Size(new Point(90, 26));
			_importButton = val3;
			((Control)_importButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.ImportClicked?.Invoke(this, EventArgs.Empty);
			});
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
			StandardButton communityButton = _communityButton;
			if (communityButton != null)
			{
				((Control)communityButton).Dispose();
			}
			StandardButton importButton = _importButton;
			if (importButton != null)
			{
				((Control)importButton).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
