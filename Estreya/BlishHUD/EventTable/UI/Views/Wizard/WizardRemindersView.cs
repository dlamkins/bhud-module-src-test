using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Models.Reminders;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Services.Audio;
using Estreya.BlishHUD.Shared.UI.Views;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Wizard
{
	public class WizardRemindersView : WizardView
	{
		private bool _useReminders;

		private bool _useFillers;

		private readonly ModuleSettings _moduleSettings;

		private readonly AudioService _audioService;

		protected override bool TestConfigurationsAvailable => true;

		public WizardRemindersView(ModuleSettings moduleSettings, AudioService audioService, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService)
			: base(apiManager, iconService, translationService)
		{
			_moduleSettings = moduleSettings;
			_audioService = audioService;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			FormattedLabel welcomeLbl = new FormattedLabelBuilder().SetWidth(((Container)parent).get_ContentRegion().Width).Wrap().AutoSizeHeight()
				.SetHorizontalAlignment((HorizontalAlignment)1)
				.CreatePart("Event Reminders", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)24);
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("You are probably already seeing your first area created and ready to use on your screen.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)18);
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("Please change the settings below to fit your needs regarding event areas.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
				{
					b.SetFontSize((FontSize)18);
				})
				.Build();
			((Control)welcomeLbl).set_Top((int)((float)((Container)parent).get_ContentRegion().Height * 0.1f));
			((Control)welcomeLbl).set_Parent((Container)(object)parent);
			Label useRemindersLbl = RenderLabel(parent, "Use Reminders:").TitleLabel;
			((Control)useRemindersLbl).set_Parent((Container)(object)parent);
			useRemindersLbl.set_AutoSizeWidth(false);
			((Control)useRemindersLbl).set_Width(base.LABEL_WIDTH);
			((Control)useRemindersLbl).set_Top(((Control)welcomeLbl).get_Bottom() + 100);
			((Control)useRemindersLbl).set_Left(150);
			_useReminders = _moduleSettings.RemindersEnabled.get_Value();
			((Control)RenderCheckbox(parent, new Point(((Control)useRemindersLbl).get_Right() + 20, ((Control)useRemindersLbl).get_Top()), _useReminders, delegate(bool val)
			{
				_useReminders = val;
			})).set_BasicTooltipText("Check this option if you would like to be reminded before an event starts.");
			Button button = RenderButtonAsync(parent, "Show Test Reminder", ShowTestReminder);
			((Control)button).set_Top(((Control)useRemindersLbl).get_Bottom() + 5);
			((Control)button).set_Left(((Control)useRemindersLbl).get_Left());
			FlowPanel buttons = GetButtonPanel(parent);
			Rectangle contentRegion = ((Container)parent).get_ContentRegion();
			((Control)buttons).set_Top(((Rectangle)(ref contentRegion)).get_Bottom() - 20 - ((Control)buttons).get_Height());
			((Control)buttons).set_Left(((Container)parent).get_ContentRegion().Width / 2 - ((Control)buttons).get_Width() / 2);
		}

		private async Task ShowTestReminder()
		{
			string title = "Test Event";
			string message = "Test starts in " + TimeSpan.FromHours(5.0).Add(TimeSpan.FromMinutes(21.0).Add(TimeSpan.FromSeconds(23.0))).Humanize(6, null, TimeUnit.Week, _moduleSettings.ReminderMinTimeUnit.get_Value()) + "!";
			AsyncTexture2D icon = base.IconService.GetIcon("textures/maintenance.png");
			ReminderType value = _moduleSettings.ReminderType.get_Value();
			if ((value == ReminderType.Control || value == ReminderType.Both) ? true : false)
			{
				EventNotification.ShowAsControl(title, message, icon, base.IconService, _moduleSettings);
				await EventNotification.PlaySound(_audioService);
			}
			value = _moduleSettings.ReminderType.get_Value();
			if ((value == ReminderType.Windows || value == ReminderType.Both) ? true : false)
			{
				await EventNotification.ShowAsWindowsNotification(title, message, icon);
			}
		}

		protected override Task ApplyConfigurations()
		{
			return Task.CompletedTask;
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
