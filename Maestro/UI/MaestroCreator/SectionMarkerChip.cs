using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Maestro.UI.MaestroCreator
{
	public class SectionMarkerChip : BaseChip
	{
		public static class Layout
		{
			public const int Height = 26;

			public const int CloseButtonSize = 16;

			public const int Padding = 6;

			public const int CloseButtonMargin = 2;
		}

		private static readonly Color SectionColor = new Color(80, 68, 58);

		private static readonly Color SectionHoverColor = MaestroTheme.Brighten(SectionColor);

		private readonly Label _sectionLabel;

		private readonly Label _closeButton;

		public string SectionName { get; }

		public SectionMarkerChip(string sectionName, int index, int containerWidth)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Expected O, but got Unknown
			SectionName = sectionName;
			base.Index = index;
			int chipWidth = containerWidth - 26;
			((Control)this).set_Size(new Point(chipWidth, 26));
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			_currentColor = SectionColor;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(sectionName);
			((Control)val).set_Location(new Point(6, 1));
			((Control)val).set_Size(new Point(chipWidth - 16 - 12, 24));
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(MaestroTheme.CreamWhite);
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			val.set_VerticalAlignment((VerticalAlignment)1);
			_sectionLabel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("×");
			((Control)val2).set_Location(new Point(chipWidth - 16 - 2, 2));
			((Control)val2).set_Size(new Point(16, 20));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_TextColor(MaestroTheme.MutedCream);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			_closeButton = val2;
			((Control)_closeButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_closeButton.set_TextColor(MaestroTheme.Error);
			});
			((Control)_closeButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_closeButton.set_TextColor(MaestroTheme.MutedCream);
			});
			((Control)_closeButton).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)delegate
			{
				FireRemoveClicked();
			});
			((Control)this).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_currentColor = SectionHoverColor;
				((Control)this).Invalidate();
			});
			((Control)this).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				_currentColor = SectionColor;
				((Control)this).Invalidate();
			});
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				UpdateLayout();
			});
		}

		private void UpdateLayout()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			((Control)_sectionLabel).set_Size(new Point(((Control)this).get_Width() - 16 - 12, 24));
			((Control)_closeButton).set_Location(new Point(((Control)this).get_Width() - 16 - 2, 2));
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			int closeButtonX = ((Control)this).get_Width() - 16 - 2;
			if (((Control)this).get_RelativeMousePosition().X < closeButtonX)
			{
				FireChipClicked(e);
			}
			((Panel)this).OnClick(e);
		}

		protected override void DisposeControl()
		{
			Label sectionLabel = _sectionLabel;
			if (sectionLabel != null)
			{
				((Control)sectionLabel).Dispose();
			}
			Label closeButton = _closeButton;
			if (closeButton != null)
			{
				((Control)closeButton).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
