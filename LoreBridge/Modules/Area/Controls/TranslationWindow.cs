using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using FontStashSharp;
using LoreBridge.Controls;
using Microsoft.Xna.Framework;

namespace LoreBridge.Modules.Area.Controls
{
	public sealed class TranslationWindow : SimpleWindow
	{
		private readonly LabelCustom _label;

		private readonly Scrollbar _scroll;

		private readonly LoadingSpinner _loading;

		private bool _isLoading;

		public string Text
		{
			set
			{
				_label.Text = value;
			}
		}

		public bool Loading
		{
			set
			{
				_isLoading = value;
				if (_isLoading)
				{
					((Control)_loading).Show();
					((Control)_label).Hide();
				}
				else
				{
					((Control)_loading).Hide();
					((Control)_label).Show();
				}
			}
		}

		public TranslationWindow(SpriteFontBase font)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Expected O, but got Unknown
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			base.TopMost = true;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_BackgroundColor(Color.get_Black() * 0.9f);
			val.set_CanScroll(false);
			val.set_CanCollapse(false);
			val.set_Collapsed(false);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			Panel panel = val;
			Scrollbar val2 = new Scrollbar((Container)(object)panel);
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Right(0);
			((Control)val2).set_Top(0);
			_scroll = val2;
			LabelCustom labelCustom = new LabelCustom();
			((Control)labelCustom).set_Parent((Container)(object)panel);
			labelCustom.Font = font;
			labelCustom.WrapText = true;
			labelCustom.TextColor = Color.get_White();
			labelCustom.AutoSizeHeight = true;
			((Control)labelCustom).set_Location(new Point(4, 0));
			_label = labelCustom;
			((Control)_label).Hide();
			LoadingSpinner val3 = new LoadingSpinner();
			((Control)val3).set_Parent((Container)(object)panel);
			_loading = val3;
			((Control)_loading).Hide();
			((Control)this).add_Click((EventHandler<MouseEventArgs>)OnClick);
		}

		public void UpdateFont(SpriteFontBase font)
		{
			_label.Font = font;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			base.OnResized(e);
			if (_label != null)
			{
				((Control)_label).set_Width(e.get_CurrentSize().X - 16 - 4);
			}
			if (_scroll != null)
			{
				((Control)_scroll).set_Height(e.get_CurrentSize().Y);
				((Control)_scroll).set_Right(e.get_CurrentSize().X);
			}
			if (_loading != null)
			{
				int width = e.get_CurrentSize().X - 16;
				int height = e.get_CurrentSize().Y;
				int size = Math.Min(Math.Min(width, 64), height);
				((Control)_loading).set_Size(new Point(size, size));
				((Control)_loading).set_Location(new Point(width / 2 - size / 2, height / 2 - size / 2));
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)OnClick);
		}

		private void OnClick(object sender, MouseEventArgs e)
		{
			((Control)this).Hide();
		}
	}
}
