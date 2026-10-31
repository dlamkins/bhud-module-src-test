using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeaStove.Gw2Tldr.Data;

namespace SeaStove.Gw2Tldr.UI
{
	public sealed class GuideImportWindow : IDisposable
	{
		private readonly StandardWindow _window;

		private readonly TextBox _input;

		private readonly Label _status;

		private readonly GuideCatalog _catalog;

		private readonly Action<JObject> _apply;

		public GuideImportWindow(GuideCatalog catalog, Action<JObject> apply)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Expected O, but got Unknown
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Expected O, but got Unknown
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			_catalog = catalog;
			_apply = apply;
			StandardWindow val = new StandardWindow(AsyncTexture2D.FromAssetId(155997), new Rectangle(25, 26, 560, 640), new Rectangle(40, 50, 540, 590));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Import guide notes");
			((WindowBase2)val).set_Subtitle("Single or multiple encounters");
			((Control)val).set_Location(new Point(300, 200));
			((WindowBase2)val).set_CanClose(true);
			((WindowBase2)val).set_CanCloseWithEscape(true);
			_window = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_window);
			((Control)val2).set_Location(new Point(10, 15));
			((Control)val2).set_Width(480);
			val2.set_AutoSizeHeight(true);
			val2.set_WrapText(true);
			val2.set_Text("Paste notes exported from the website or add-on. Import replaces local notes for those encounters.");
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)_window);
			((Control)val3).set_Location(new Point(10, 80));
			((Control)val3).set_Width(490);
			((TextInputBase)val3).set_MaxLength(2000000);
			((TextInputBase)val3).set_PlaceholderText("Paste exported guide JSON here");
			_input = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)_window);
			((Control)val4).set_Location(new Point(10, 125));
			((Control)val4).set_Width(490);
			val4.set_AutoSizeHeight(true);
			val4.set_WrapText(true);
			_status = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)_window);
			((Control)val5).set_Location(new Point(10, 180));
			((Control)val5).set_Width(180);
			val5.set_Text("Import and replace");
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					JObject obj = GuideEdits.ParseAll(((TextInputBase)_input).get_Text(), _catalog.Guides);
					_apply(obj);
					((Control)_window).Hide();
				}
				catch (Exception ex) when (ex is JsonException || ex is ArgumentException || ex is InvalidOperationException)
				{
					_status.set_Text("Import failed: " + ex.Message);
				}
			});
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)_window);
			((Control)val6).set_Location(new Point(200, 180));
			((Control)val6).set_Width(130);
			val6.set_Text("Cancel");
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)_window).Hide();
			});
			((Control)_window).Show();
		}

		public void Dispose()
		{
			((Control)_window).Dispose();
		}
	}
}
