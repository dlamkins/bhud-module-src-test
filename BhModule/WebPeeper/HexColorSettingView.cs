using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using MonoGame.Extended;

namespace BhModule.WebPeeper
{
	internal class HexColorSettingView : StringSettingView
	{
		private ColorPreview _colorPreview;

		public HexColorSettingView(SettingEntry<string> setting, int definedWidth = -1)
			: this(setting, definedWidth)
		{
		}

		protected override void BuildSetting(Container buildPanel)
		{
			((StringSettingView)this).BuildSetting(buildPanel);
			Control obj = buildPanel.get_Children().get_Item(1);
			obj.set_Parent((Container)null);
			ColorPreview colorPreview = new ColorPreview();
			((Control)colorPreview).set_Parent(buildPanel);
			_colorPreview = colorPreview;
			ApplyColor();
			((SettingView<string>)this).add_ValueChanged((EventHandler<ValueEventArgs<string>>)delegate
			{
				ApplyColor();
			});
			obj.set_Parent(buildPanel);
			obj.add_Moved((EventHandler<MovedEventArgs>)delegate(object s, MovedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)_colorPreview).set_Left(e.get_CurrentLocation().X - ((Control)_colorPreview).get_Width());
			});
		}

		private void ApplyColor()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_colorPreview.Color = ColorHelper.FromHex(((SettingView<string>)this).get_Value());
			}
			catch
			{
			}
		}
	}
}
