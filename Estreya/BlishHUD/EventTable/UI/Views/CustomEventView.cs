using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Helpers;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class CustomEventView : BaseView
	{
		private static Point PADDING = new Point(25, 25);

		private BlishHudApiService _blishHudApiService;

		public CustomEventView(Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, BlishHudApiService blishHudApiService, BitmapFont font = null)
			: base(apiManager, iconService, translationService, font)
		{
			_blishHudApiService = blishHudApiService;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - PADDING.X * 2);
			((Control)val).set_Height(((Container)parent).get_ContentRegion().Height - PADDING.Y * 2);
			((Control)val).set_Location(new Point(PADDING.X, PADDING.Y));
			((Panel)val).set_CanScroll(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			FlowPanel flowPanel = val;
			BuildInstructionSection(flowPanel);
			BuildLoginSection(flowPanel);
		}

		private void BuildInstructionSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_ShowBorder(true);
			FlowPanel instructionPanel = val;
			((Control)GetLabelBuilder((Panel)(object)parent).CreatePart("1. Make an account at ", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20);
			}).CreatePart("Estreya BlishHUD API.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).SetHyperLink("https://blish-hud.estreya.de/register");
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("2. Follow steps send by mail.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetFontSize((FontSize)20);
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("3. Add your own custom events.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetFontSize((FontSize)20);
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("4. Enter login details below.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetFontSize((FontSize)20);
				})
				.Build()).set_Parent((Container)(object)instructionPanel);
			RenderEmptyLine((Panel)(object)instructionPanel, 20);
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
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(0f, 5f));
			val.set_FlowDirection((ControlFlowDirection)3);
			FlowPanel loginPanel = val;
			string password = AsyncHelper.RunSync(() => _blishHudApiService.GetAPIPassword());
			TextBox usernameTextBox = RenderTextbox((Panel)(object)loginPanel, new Point(0, 0), 250, _blishHudApiService.GetAPIUsername(), "API Username");
			TextBox passwordTextBox = RenderTextbox((Panel)(object)loginPanel, new Point(0, 0), 250, (!string.IsNullOrWhiteSpace(password)) ? "<<Unchanged>>" : null, "API Password");
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)loginPanel);
			((Container)val2).set_WidthSizingMode((SizingMode)1);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_FlowDirection((ControlFlowDirection)2);
			FlowPanel buttonPanel = val2;
			RenderButtonAsync((Panel)(object)buttonPanel, "Save", async delegate
			{
				_blishHudApiService.SetAPIUsername(((TextInputBase)usernameTextBox).get_Text());
				await _blishHudApiService.SetAPIPassword((((TextInputBase)passwordTextBox).get_Text() == "<<Unchanged>>") ? password : ((TextInputBase)passwordTextBox).get_Text());
				await _blishHudApiService.Login();
			});
			RenderButtonAsync((Panel)(object)buttonPanel, "Test Login", async delegate
			{
				await _blishHudApiService.TestLogin(((TextInputBase)usernameTextBox).get_Text(), (((TextInputBase)passwordTextBox).get_Text() == "<<Unchanged>>") ? password : ((TextInputBase)passwordTextBox).get_Text());
				ShowInfo("Login successful!");
			});
			RenderButtonAsync((Panel)(object)buttonPanel, "Clear Credentials", async delegate
			{
				_blishHudApiService.SetAPIUsername(null);
				await _blishHudApiService.SetAPIPassword(null);
				_blishHudApiService.Logout();
				ShowInfo("Logout successful!");
			});
		}

		private FormattedLabelBuilder GetLabelBuilder(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return new FormattedLabelBuilder().SetWidth(((Container)parent).get_ContentRegion().Width - PADDING.X * 2).AutoSizeHeight().SetVerticalAlignment((VerticalAlignment)0);
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
