using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Views
{
	public class AccountRestrictedView : View
	{
		[CompilerGenerated]
		private bool _003CshowBackButton_003EP;

		protected FlowPanel _flowPanel;

		protected Panel _controlsHeader;

		protected Image _backButton;

		protected UserCheckServerResponse _userCheck;

		public AccountRestrictedView(bool showBackButton = false)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			_003CshowBackButton_003EP = showBackButton;
			_flowPanel = new FlowPanel();
			Panel val = new Panel();
			((Control)val).set_Height(50);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			_controlsHeader = val;
			Image val2 = new Image();
			val2.set_Texture(Service.Textures.DatAsset(784268));
			_backButton = val2;
			_userCheck = new UserCheckServerResponse
			{
				Banned = false,
				VersionCheck = true,
				Ban = null
			};
			((View)this)._002Ector();
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			if (Service.UserManager.Account != null)
			{
				progress.Report("Checking user ban ");
				UserCheckServerResponse check = await Service.GeoServerWrapper.PerformUserCheck(Service.UserManager.Account!.get_Name());
				if (check != null)
				{
					_userCheck = check;
					if (check.Tutorial != null)
					{
						Service.UserManager.SetTutorialState(check.Tutorial);
					}
				}
			}
			return true;
		}

		protected override void Unload()
		{
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			((View<IPresenter>)this).Build(buildPanel);
			if (_userCheck.Banned)
			{
				Label val = new Label();
				val.set_Text("Your account (" + (_userCheck.Ban?.AccountName ?? "Unknown") + ") has been banned\n\nDate: " + (_userCheck.Ban?.BannedOn.ToString() ?? "Unknown") + "\nReason: " + (_userCheck.Ban?.Reason ?? "Unknown") + "\n\nPlease visit the BlishHUD discord to ask for assistance");
				((Control)val).set_Parent(buildPanel);
				((Control)val).set_Width(((Control)buildPanel).get_Width());
				((Control)val).set_Height(((Control)buildPanel).get_Height());
				val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0));
				val.set_TextColor(Color.get_Red());
				val.set_HorizontalAlignment((HorizontalAlignment)0);
				val.set_VerticalAlignment((VerticalAlignment)0);
			}
			else if (!_userCheck.VersionCheck)
			{
				Label val2 = new Label();
				val2.set_Text("!! Update Required !!\n\nVersion " + Module.MODULE_VERSION + " is not supported\n\nPlease upgrade to the latest version or\nvisit the BlishHUD discord to ask for assistance");
				((Control)val2).set_Parent(buildPanel);
				((Control)val2).set_Width(((Control)buildPanel).get_Width());
				((Control)val2).set_Height(((Control)buildPanel).get_Height());
				val2.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0));
				val2.set_TextColor(Color.get_Orange());
				val2.set_HorizontalAlignment((HorizontalAlignment)1);
				val2.set_VerticalAlignment((VerticalAlignment)0);
			}
			else if (!Service.Settings.SignedSocialContract.get_Value())
			{
				new SocialContract(buildPanel);
			}
			else if (Service.UserManager.Account == null)
			{
				Label val3 = new Label();
				val3.set_Text("Attempting to load GW2 API Account Information...\n\nIf this screen persists, please check your API key.");
				((Control)val3).set_Parent(buildPanel);
				((Control)val3).set_Width(((Control)buildPanel).get_Width());
				((Control)val3).set_Height(((Control)buildPanel).get_Height() / 2);
				val3.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0));
				val3.set_TextColor(Color.get_LawnGreen());
				val3.set_HorizontalAlignment((HorizontalAlignment)1);
				val3.set_VerticalAlignment((VerticalAlignment)1);
			}
			else
			{
				_flowPanel.BeginFlowFill(buildPanel).AddChildPanel(_controlsHeader);
				((Container)_flowPanel).set_WidthSizingMode((SizingMode)2);
				_flowPanel.set_OuterControlPadding(new Vector2(5f, 5f));
				_flowPanel.set_ControlPadding(new Vector2(5f, 5f));
				if (_003CshowBackButton_003EP)
				{
					((Control)_backButton).set_Parent((Container)(object)_controlsHeader);
					((Control)_backButton).set_Location(new Point(5, 5));
					((Control)_backButton).set_Size(new Point(40, 40));
				}
				DoBuild(buildPanel);
			}
		}

		protected virtual void DoBuild(Container buildPanel)
		{
		}
	}
}
