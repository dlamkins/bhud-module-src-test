using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Settings.Controls;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class SocialContract : FlowPanel, IDisposable
	{
		public SocialContract(Container parent)
			: this()
		{
			Build(parent);
		}

		protected void Build(Container container)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Expected O, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Expected O, but got Unknown
			((Panel)this).set_CanScroll(true);
			FlowPanel container2 = ((FlowPanel)(object)this).BeginFlowFill(container);
			Label val = new Label();
			val.set_Text(Service.Config.Name + " is a family-friendly and light-hearted game\nhosted by your friends at BlishHUD");
			val.set_AutoSizeWidth(true);
			((Control)val).set_Height(50);
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0));
			val.set_TextColor(Color.get_LightGoldenrodYellow());
			FlowPanel container3 = container2.AddControl<Label>(val).AddSpace();
			Label val2 = new Label();
			val2.set_Text("We ask that you agree with these statements when participating");
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Height(30);
			val2.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)20, (FontStyle)0));
			FlowPanel container4 = container3.AddControl<Label>(val2);
			Label val3 = new Label();
			val3.set_Text("1) I will respect all of my fellow players.\n2) I will use clean and non-offensive language\n3) I will not harass nor be toxic to my fellow players\n4) I will strive be the best GeoGuesser in Tyria!");
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Height(120);
			val3.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)20, (FontStyle)0));
			val3.set_TextColor(Color.get_LawnGreen());
			FlowPanel container5 = container4.AddControl<Label>(val3).AddSpace();
			NuclearOptionButton nuclearOptionButton = new NuclearOptionButton();
			((Control)nuclearOptionButton).set_Location(new Point(0, 210));
			((Control)nuclearOptionButton).set_Width(400);
			((StandardButton)nuclearOptionButton).set_Text("I agree to and will abide by this social contract");
			NuclearOptionButton agree;
			FlowPanel container6 = container5.AddControl<NuclearOptionButton>(nuclearOptionButton, out agree);
			Label val4 = new Label();
			((Control)val4).set_Location(new Point(0, 240));
			val4.set_Text("Press and hold the CTRL and SHIFT keys on your keyboard to enable the button");
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Height(20);
			val4.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)16, (FontStyle)0));
			val4.set_TextColor(Color.get_LightGoldenrodYellow());
			container6.AddControl<Label>(val4).AddSpace().AddString("Please report any user games that are created against the spirit of this")
				.AddString("contract on the BlishHUD discord. Thank you and happy hunting.")
				.AddString("");
			((Control)agree).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Service.Settings.SignedSocialContract.set_Value(true);
				Service.GeoGuessWindow.OpenWindowState();
			});
		}

		protected override void DisposeControl()
		{
		}
	}
}
