using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Models.BlishHudAPI;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public class BlishHUDAPIView : BaseView
	{
		private const string REGISTER_URL = "https://blish-hud.estreya.de/register";

		protected static readonly Point PADDING = new Point(25, 25);

		private readonly IFlurlClient _flurlClient;

		private KofiStatus _kofiStatus;

		private FlowPanel _mainParent;

		private FlowPanel _kofiStatusGroup;

		private Texture2D _kofiLogo;

		protected BlishHudApiService BlishHUDAPIService { get; }

		public BlishHUDAPIView(Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, BlishHudApiService blishHudApiService, IFlurlClient flurlClient)
			: base(apiManager, iconService, translationService)
		{
			BlishHUDAPIService = blishHudApiService;
			_flurlClient = flurlClient;
		}

		protected sealed override void InternalBuild(Panel parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - PADDING.X * 2);
			((Control)val).set_Height(((Container)parent).get_ContentRegion().Height - PADDING.Y * 2);
			((Control)val).set_Location(new Point(PADDING.X, PADDING.Y));
			((Panel)val).set_CanScroll(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			_mainParent = val;
			BuildLoginSection(_mainParent);
		}

		private void RenderKofiStatus(FlowPanel flowPanel)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			if (_kofiStatusGroup == null)
			{
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent((Container)(object)flowPanel);
				((Control)val).set_Width(((Container)flowPanel).get_ContentRegion().Width - (int)flowPanel.get_OuterControlPadding().X * 2);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				val.set_FlowDirection((ControlFlowDirection)3);
				_kofiStatusGroup = val;
			}
			bool subscriptionActive = _kofiStatus?.Active ?? false;
			string lastPayment = _kofiStatus?.LastPayment?.ToLocalTime().ToString();
			FlowPanel kofiStatusGroup = _kofiStatusGroup;
			string value = subscriptionActive.ToString();
			Color? val2 = (subscriptionActive ? Color.get_Green() : Color.get_Red());
			RenderLabel(valueXLocation: 200, parent: (Panel)(object)kofiStatusGroup, title: "Subscription active?", value: value, textColorTitle: null, textColorValue: val2);
			FlowPanel kofiStatusGroup2 = _kofiStatusGroup;
			string value2 = lastPayment ?? "Never";
			int? valueXLocation2 = 200;
			val2 = null;
			Color? textColorTitle = val2;
			val2 = null;
			RenderLabel((Panel)(object)kofiStatusGroup2, "Last Payment", value2, textColorTitle, val2, valueXLocation2);
			RenderEmptyLine((Panel)(object)_kofiStatusGroup);
			Button button = RenderButton((Panel)(object)_kofiStatusGroup, "Ko-fi", delegate
			{
				Process.Start("https://ko-fi.com/estreya");
			});
			button.Icon = AsyncTexture2D.op_Implicit(_kofiLogo);
			((Control)button).set_Height(48);
			((Control)button).set_Width(150);
			button.Font = GameService.Content.get_DefaultFont18();
			RenderEmptyLine((Panel)(object)_kofiStatusGroup);
			FormattedLabelBuilder labelBuilder = GetLabelBuilder((Panel)(object)_kofiStatusGroup);
			labelBuilder.CreatePart("Your payment hasn't been detected?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
			{
				b.MakeBold().MakeUnderlined().SetFontSize((FontSize)20);
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("Make sure you have used the same email on ko-fi as you have used on Estreya BlishHUD.", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("If it is still not picked up, dm ", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("estreya", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetHyperLink("discord://-/users/248146979773874177/").MakeBold();
				})
				.CreatePart(" on Discord.", (Action<FormattedLabelPartBuilder>)delegate
				{
				});
			((Control)labelBuilder.Build()).set_Parent((Container)(object)_kofiStatusGroup);
			RenderEmptyLine((Panel)(object)_kofiStatusGroup);
			FormattedLabelBuilder labelBuilder2 = GetLabelBuilder((Panel)(object)_kofiStatusGroup);
			labelBuilder2.CreatePart("What counts as a subscription?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
			{
				b.MakeBold().MakeUnderlined().SetFontSize((FontSize)20);
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("Currently only the membership subscription counts.", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("The price you choose does not matter. It can be as low as 1€.", (Action<FormattedLabelPartBuilder>)delegate
				{
				});
			((Control)labelBuilder2.Build()).set_Parent((Container)(object)_kofiStatusGroup);
		}

		private async void RedrawKofiStatusGroup(object sender, EventArgs e)
		{
			if (_kofiStatusGroup != null)
			{
				await LoadKofiStatus();
				((Container)_kofiStatusGroup).ClearChildren();
				RenderKofiStatus(_mainParent);
			}
		}

		private void BuildLoginSection(FlowPanel parent)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(0f, 5f));
			val.set_FlowDirection((ControlFlowDirection)3);
			FlowPanel loginPanel = val;
			string password = AsyncHelper.RunSync(() => BlishHUDAPIService.GetAPIPassword());
			TextBox usernameTextBox = RenderTextbox((Panel)(object)loginPanel, new Point(0, 0), 500, BlishHUDAPIService.GetAPIUsername(), base.TranslationService.GetTranslation("blishHUDAPIView-username", "Username"));
			TextBox passwordTextBox = RenderTextbox((Panel)(object)loginPanel, new Point(0, 0), 500, (!string.IsNullOrWhiteSpace(password)) ? "<<Unchanged>>" : null, base.TranslationService.GetTranslation("blishHUDAPIView-password", "Password"));
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)loginPanel);
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_FlowDirection((ControlFlowDirection)2);
			FlowPanel buttonPanel = val2;
			RenderButtonAsync((Panel)(object)buttonPanel, base.TranslationService.GetTranslation("blishHUDAPIView-btn-save", "Save"), async delegate
			{
				BlishHUDAPIService.SetAPIUsername(((TextInputBase)usernameTextBox).get_Text());
				await BlishHUDAPIService.SetAPIPassword((((TextInputBase)passwordTextBox).get_Text() == "<<Unchanged>>") ? password : ((TextInputBase)passwordTextBox).get_Text());
				await BlishHUDAPIService.Login();
				ShowInfo("Login successful!");
			});
			RenderButtonAsync((Panel)(object)buttonPanel, base.TranslationService.GetTranslation("blishHUDAPIView-btn-testLogin", "Test Login"), async delegate
			{
				await BlishHUDAPIService.TestLogin(((TextInputBase)usernameTextBox).get_Text(), (((TextInputBase)passwordTextBox).get_Text() == "<<Unchanged>>") ? password : ((TextInputBase)passwordTextBox).get_Text());
				ShowInfo("Login successful!");
			});
			RenderButtonAsync((Panel)(object)buttonPanel, base.TranslationService.GetTranslation("blishHUDAPIView-btn-clearCredentials", "Clear Credentials"), async delegate
			{
				BlishHUDAPIService.SetAPIUsername(null);
				await BlishHUDAPIService.SetAPIPassword(null);
				BlishHUDAPIService.Logout();
				ShowInfo("Logout successful!");
			});
			RenderButton((Panel)(object)buttonPanel, base.TranslationService.GetTranslation("blishHUDAPIView-btn-register", "Register"), delegate
			{
				try
				{
					Process.Start("https://blish-hud.estreya.de/register");
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			});
		}

		private FormattedLabelBuilder GetLabelBuilder(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return new FormattedLabelBuilder().SetWidth(((Container)parent).get_ContentRegion().Width - PADDING.X * 2).AutoSizeHeight().SetVerticalAlignment((VerticalAlignment)0);
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			await LoadKofiStatus();
			await LoadKofiLogo();
			return true;
		}

		protected override void Unload()
		{
			if (BlishHUDAPIService != null)
			{
				BlishHUDAPIService.NewLogin -= RedrawKofiStatusGroup;
				BlishHUDAPIService.RefreshedLogin -= RedrawKofiStatusGroup;
				BlishHUDAPIService.LoggedOut -= RedrawKofiStatusGroup;
			}
			_kofiStatus = null;
			_kofiLogo = null;
			_kofiStatusGroup = null;
			_mainParent = null;
			base.Unload();
		}

		private async Task LoadKofiStatus()
		{
			try
			{
				_kofiStatus = await BlishHUDAPIService.GetKofiStatus();
			}
			catch (Exception ex)
			{
				_kofiStatus = null;
				_logger.Warn(ex, "Could not load ko-fi status.");
			}
		}

		private async Task LoadKofiLogo()
		{
			try
			{
				Bitmap bitmap = ImageUtil.ResizeImage(Image.FromStream(await _flurlClient.Request("https://storage.ko-fi.com/cdn/nav-logo-stroke.png").GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0)), 48, 32);
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					bitmap.Save(memoryStream, ImageFormat.Png);
					await Task.Run(delegate
					{
						//IL_0005: Unknown result type (might be due to invalid IL or missing references)
						//IL_000a: Unknown result type (might be due to invalid IL or missing references)
						GraphicsDeviceContext val = GameService.Graphics.LendGraphicsDeviceContext();
						try
						{
							_kofiLogo = Texture2D.FromStream(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), (Stream)memoryStream);
						}
						finally
						{
							((GraphicsDeviceContext)(ref val)).Dispose();
						}
					});
				}
				finally
				{
					if (memoryStream != null)
					{
						((IDisposable)memoryStream).Dispose();
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
