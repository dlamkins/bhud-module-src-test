using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ProofLogix.Core.Services.KpWebApi.V1.Models;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;
using Nekres.ProofLogix.Core.UI.KpProfile;

namespace Nekres.ProofLogix.Core.UI
{
	public class RegisterView : View
	{
		private AsyncTexture2D _kpLogo;

		public RegisterView()
			: this()
		{
			_kpLogo = AsyncTexture2D.op_Implicit(ProofLogix.Instance.ContentsManager.GetTexture("killproof_logo.png"));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Expected O, but got Unknown
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Expected O, but got Unknown
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Expected O, but got Unknown
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Expected O, but got Unknown
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Expected O, but got Unknown
			Image val = new Image(_kpLogo);
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(128);
			((Control)val).set_Height(128);
			((Control)val).set_Left((buildPanel.get_ContentRegion().Width - 128) / 2);
			Image image = val;
			((Control)image).add_Disposed((EventHandler<EventArgs>)delegate
			{
				_kpLogo.Dispose();
			});
			string text = "You do not appear to have added a key to www.killproof.me yet.\nYou can add yourself now to track your progress.";
			Point size = LabelUtil.GetLabelSize(GameService.Content.get_DefaultFont16(), text);
			FormattedLabel label = new FormattedLabelBuilder().SetWidth(buildPanel.get_ContentRegion().Width).SetHeight(size.Y).SetHorizontalAlignment((HorizontalAlignment)1)
				.CreatePart(text, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					o.SetFontSize((FontSize)16);
				})
				.Build();
			((Control)label).set_Top(((Control)image).get_Bottom());
			((Control)label).set_Parent(buildPanel);
			string perms = string.Join(", ", ProofLogix.Instance.KpWebApi.RequiredPermissions);
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Width(buildPanel.get_ContentRegion().Width / 2);
			((Control)val2).set_Height(32);
			((Control)val2).set_Left(buildPanel.get_ContentRegion().Width / 4);
			((Control)val2).set_Top(((Control)label).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			((TextInputBase)val2).set_PlaceholderText("Guild Wars 2 API key");
			((Control)val2).set_BasicTooltipText("A Guild Wars 2 API key.\nRequired permissions: " + perms);
			TextBox apiInput = val2;
			Image val3 = new Image();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Top(((Control)apiInput).get_Top());
			((Control)val3).set_Left(((Control)apiInput).get_Right() + 4);
			((Control)val3).set_Height(32);
			((Control)val3).set_Width(32);
			val3.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(1234939));
			Image checkmark = val3;
			((TextInputBase)apiInput).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				bool flag = Gw2ApiUtil.HasCorrectFormat(((TextInputBase)apiInput).get_Text());
				checkmark.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(flag ? 1234938 : 1234939));
			});
			Checkbox val4 = new Checkbox();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Height(40);
			((Control)val4).set_Top(((Control)apiInput).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			val4.set_Text("I would be willing to open instances for people. (Optional)");
			((Control)val4).set_BasicTooltipText("This feature will give your Guild Wars 2 account name to people if you are able to open an instance to a specific boss.\nFor example someone might want to skip Escort and if that's all you've done in Wing 3, you could be one of the accounts shown. It will show your account name so they can contact you in-game.");
			Checkbox openerCb = val4;
			((Control)openerCb).set_Left((buildPanel.get_ContentRegion().Width - ((Control)openerCb).get_Width()) / 2);
			Checkbox val5 = new Checkbox();
			((Control)val5).set_Parent(buildPanel);
			((Control)val5).set_Height(60);
			val5.set_Text("I hereby grant www.killproof.me permission to track and store my raid related progress.\nI understand that I can claim ownership of my records at anytime by creating an account\nat www.killproof.me.");
			((Control)val5).set_Top(((Control)openerCb).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			Checkbox agreeCb = val5;
			((Control)agreeCb).set_Left((buildPanel.get_ContentRegion().Width - ((Control)agreeCb).get_Width()) / 2);
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent(buildPanel);
			((Control)val6).set_Top(((Control)agreeCb).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
			((Control)val6).set_Width(120);
			((Control)val6).set_Height(36);
			((Control)val6).set_Left((buildPanel.get_ContentRegion().Width - 120) / 2);
			val6.set_Text("Add yourself now!");
			StandardButton acceptBttn = val6;
			((Control)acceptBttn).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (!Gw2ApiUtil.HasCorrectFormat(((TextInputBase)apiInput).get_Text()))
				{
					GameService.Content.PlaySoundEffectByName("error");
					ScreenNotification.ShowNotification("Please enter a valid Guild Wars 2 API key.", (NotificationType)2, (Texture2D)null, 4);
				}
				else if (!agreeCb.get_Checked())
				{
					GameService.Content.PlaySoundEffectByName("error");
					ScreenNotification.ShowNotification("Your consent is required to proceed.", (NotificationType)2, (Texture2D)null, 4);
				}
				else
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					StandardWindow window = (StandardWindow)buildPanel;
					window.Show((IView)(object)new LoadingView("Adding key...", "Please, wait."));
					AddKey response = await ProofLogix.Instance.KpWebApi.AddKey(((TextInputBase)apiInput).get_Text(), openerCb.get_Checked());
					if (response.IsError)
					{
						ProofLogix.Logger.Warn(response.Error);
						GameService.Content.PlaySoundEffectByName("error");
						ScreenNotification.ShowNotification("Something went wrong. Please, try again.", (NotificationType)2, (Texture2D)null, 4);
						window.Show((IView)(object)this);
						ProofLogix.Instance.ToggleRegisterWindow();
					}
					else
					{
						GameService.Content.PlaySoundEffectByName("color-change");
						ScreenNotification.ShowNotification("Profile added successfully! ID: " + response.KpId, (NotificationType)5, (Texture2D)null, 4);
						Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models.Profile profile = await ProofLogix.Instance.KpWebApi.GetProfile(response.KpId);
						ProofLogix.Instance.PartySync.LocalPlayer.AttachProfile(profile);
						ProfileView.Open(profile);
						((Control)window).Dispose();
					}
				}
			});
			buildPanel.add_ContentResized((EventHandler<RegionChangedEventArgs>)delegate(object _, RegionChangedEventArgs e)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0096: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0134: Unknown result type (might be due to invalid IL or missing references)
				//IL_014b: Unknown result type (might be due to invalid IL or missing references)
				//IL_017e: Unknown result type (might be due to invalid IL or missing references)
				//IL_019a: Unknown result type (might be due to invalid IL or missing references)
				((Control)image).set_Left((e.get_CurrentRegion().Width - ((Control)image).get_Width()) / 2);
				((Control)label).set_Width(e.get_CurrentRegion().Width);
				((Control)label).set_Top(((Control)image).get_Bottom());
				((Control)apiInput).set_Width(e.get_CurrentRegion().Width / 2);
				((Control)apiInput).set_Left(e.get_CurrentRegion().Width / 4);
				((Control)apiInput).set_Top(((Control)label).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
				((Control)checkmark).set_Top(((Control)apiInput).get_Top());
				((Control)checkmark).set_Left(((Control)apiInput).get_Right() + 4);
				((Control)openerCb).set_Top(((Control)apiInput).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
				((Control)openerCb).set_Left((e.get_CurrentRegion().Width - ((Control)openerCb).get_Width()) / 2);
				((Control)agreeCb).set_Top(((Control)openerCb).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
				((Control)agreeCb).set_Left((e.get_CurrentRegion().Width - ((Control)agreeCb).get_Width()) / 2);
				((Control)acceptBttn).set_Top(((Control)agreeCb).get_Bottom() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().Y);
				((Control)acceptBttn).set_Left((buildPanel.get_ContentRegion().Width - 120) / 2);
			});
			((View<IPresenter>)this).Build(buildPanel);
		}
	}
}
