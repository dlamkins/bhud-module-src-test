using Blish_HUD;
using Blish_HUD.Controls;
using MonoGame.Extended.BitmapFonts;

namespace FarmingTracker
{
	public class SetupInstructions
	{
		public static void CreateSetupInstructions(BitmapFont font, FlowPanel addDrfTokenFlowPanel, Services services)
		{
			string buttonTooltip = "Open DRF website in your default web browser.";
			BitmapFont headerFont = services.FontService.Fonts[(FontSize)20];
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "DRF SETUP INSTRUCTIONS", headerFont);
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Prerequisite:", font);
			new HintLabel((Container?)(object)addDrfTokenFlowPanel, "- Windows 8 or newer because DRF requires websocket technolgy.");
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Setup DRF DLL and DRF account:", font);
			new HintLabel((Container?)(object)addDrfTokenFlowPanel, "1. Click the button below and follow the instructions to setup the drf.dll.\n2. Create a drf account on the website and link it with\nyour GW2 Account(s).");
			new OpenUrlInBrowserButton("https://drf.rs/getting-started", "Open drf.dll setup instructions", buttonTooltip, services.TextureService.OpenLinkTexture, (Container)(object)addDrfTokenFlowPanel);
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			string testDrfHeader = "Test DRF DLL and DRF account";
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, testDrfHeader + ":", font);
			new HintLabel((Container?)(object)addDrfTokenFlowPanel, "1. Click the button below to open the DRF web live tracker.\n2. Use this web live tracker to check if the tracking is working.\ne.g. by opening an unidentified gear.\nThe items should appear almost instantly in the web live tracker.");
			new OpenUrlInBrowserButton("https://drf.rs/dashboard/livetracker", "Open DRF web live tracker", buttonTooltip, services.TextureService.OpenLinkTexture, (Container)(object)addDrfTokenFlowPanel);
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Does NOT work? :-( Try this:", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- After a GW2 patch, you will have to wait until a fixed arcdps version is released if you use arcdps to load the drf.dll.\n- If you installed drf.dll a while ago, check the drf website whether an updated version of drf.dll is available.\n- If none of this applies, the DRF Discord can help:");
			new OpenUrlInBrowserButton("https://discord.gg/VSgehyHkrD", "Open DRF Discord", "Open DRF discord in your default web browser.", services.TextureService.OpenLinkTexture, (Container)(object)addDrfTokenFlowPanel);
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Is working? :-) Get the DRF Token:", font);
			new HintLabel((Container?)(object)addDrfTokenFlowPanel, "1. Click the button below to open the drf.rs settings page.\n2. Click on 'Regenerate Token'.\n3. Copy the 'DRF Token' by clicking on the copy icon.\n4. Paste the DRF Token with CTRL + V into the DRF token input above.\n5. Done! Open the first tab again to see the tracked items/currencies :-)");
			new OpenUrlInBrowserButton("https://drf.rs/dashboard/user/settings", "Open DRF web settings", buttonTooltip, services.TextureService.OpenLinkTexture, (Container)(object)addDrfTokenFlowPanel);
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "TROUBLESHOOTING", headerFont);
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Module shows 'Add GW2 API key!' but BlishHUD already has API key", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "Sometimes BlishHUD fails to give a module access to the GW2 API key. That can be caused by a GW2 API timeout when BlishHUD is starting or for other unknown reasons. Possible workarounds:\n- Restart BlishHUD.\n- disable the module, wait a few seconds, then enable the module again.");
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "'DRF Server Connection' shows 'Authentication failed'", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- Make sure you copied the DRF token into the module with the copy button and CTRL+V as explained above. Otherwise you may accidentally copy only part of the token. In this case the DRF token input above will show you that the format is incomplete/invalid.\n- After you have clicked on 'Regenerate Token' on the DRF website, any old DRF token you may have used previously will become invalid.You must add the new token to the module.");
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "'DRF Server Connection' shows 'Connected' but does not track changes", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- Currencies and items changes will be shown after the 'Updating...' or 'Resetting...' hint disappears. While those hints are shown the module normally waits for the GW2 API.If the GW2 API is slow or has a timeout, this can unfortunately take a while.\n- The DRF DLL sends data to the DRF Server. Then the DRF Server sends data to this module. If 'DRF Server Connection' shows 'Connected', this only means that the module is connected to the DRF Server and the DRF account is probably set up correctly. But it does not mean that the DRF DLL is set up correctly. Follow the steps from '" + testDrfHeader + "' to test that.");
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Why is the GW2 API needed?", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- DRF offers only raw data. To get further details like item/currency name, description, icon and profits the GW2 API is still needed.\n- The GW2 API is the reason why the module cannot display changes to your account immediately but somtimes takes several second because it has to wait for the GW2 API responses.");
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Red bug images appear", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- When the bug image is used for an item/currency:\nhover with the mouse over the bug icon to read the tooltip. In most cases the tooltip should mention that those are items missing in the GW2 API. E.g. lvl-80-boost item or some reknown heart items.\n\n- If the bug images appears somewhere else in the module's UI or the item tooltip is not mentioning an missing item:\nReason 1: The item is new and BlishHUD's texture cache does not know the icon yet.\nOR\nReason 2: You ran BlishHUD as admin at one point and later stopped running BlishHUD as admin. This causes file permission issues for software like BlishHUD that has to create cache or config data.\nYou can try to fix 'Reason 2' by closing BlishHUD and then deleting the 'Blish HUD' folder at 'C:\\ProgramData\\Blish HUD'.");
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Known DRF issues", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "These issues cannot be fixed or might be fixed in a future release.\n\n- Bank Slot Expansion Crash:\nThe DRF.dll will crash your game when you use a Bank Slot Expansion.\n\n- Equipment changes are tracked:\nOnly none-legendary equipment is affected. Equipping an item counts as losing the item. Unequipping an item counts as gaining the item. This applies to runes and regular gathing tools too. It only somtimes applies to infinite gathering tools. Swapping equipment templates is not tracked. This issue only affects you when you swap equipment by using your bank/inventory. As a workaround you can add equipment items that you swap often to the ignored items.\n\n- Bouncy Chests:\nIf you have more than 4 bouncy chests and swap map, the game will automatically consume all but 4 of them. DRF is currently not noticing this change.\n\n- whole wallet is tracked\nSometimes the whole wallet is accidentely interpreted as a drop. You should not notice this bug, because the module will ignore drops that include more than 10 currencies at once. But you might be affected by this on accounts that have less than 10 currencies");
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "Known MODULE issues", font);
			new FixedWidthHintLabel((Container)(object)addDrfTokenFlowPanel, 480, "- The 'GW2 API error' hint constantly appears\nReason 1: GW2 API is down or instable. The GW2 API can be very instable in the evening. This results in frequent GW2 API timeouts.\nReason 2: A bug in the GW2 API libary used by this module. This can only be fixed by restarting Blish HUD.");
			AddVerticalSpacing(services, addDrfTokenFlowPanel);
		}

		private static void AddVerticalSpacing(Services services, FlowPanel addDrfTokenFlowPanel)
		{
			new HeaderLabel((Container)(object)addDrfTokenFlowPanel, "", services.FontService.Fonts[(FontSize)8]);
		}
	}
}
