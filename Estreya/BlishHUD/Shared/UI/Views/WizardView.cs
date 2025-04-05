using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Threading.Events;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public abstract class WizardView : BaseView
	{
		public bool NextAvailable { get; internal set; }

		public bool NextIsFinish { get; internal set; }

		public bool PreviousAvailable { get; internal set; }

		protected virtual bool TestConfigurationsAvailable => false;

		public event AsyncEventHandler CancelClicked;

		public event AsyncEventHandler NextClicked;

		public event AsyncEventHandler FinishClicked;

		public event AsyncEventHandler PreviousClicked;

		protected WizardView(Gw2ApiManager apiManager, IconService iconService, TranslationService translationService)
			: base(apiManager, iconService, translationService)
		{
		}

		protected virtual Task OnNextClicked()
		{
			return Task.CompletedTask;
		}

		protected virtual Task OnPreviousClicked()
		{
			return Task.CompletedTask;
		}

		private async Task Next()
		{
			await ApplyConfigurations();
			await OnNextClicked();
			await (this.NextClicked?.Invoke(this) ?? Task.CompletedTask);
		}

		private async Task Finish()
		{
			await ApplyConfigurations();
			await OnNextClicked();
			await (this.FinishClicked?.Invoke(this) ?? Task.CompletedTask);
		}

		private async Task Previous()
		{
			await OnPreviousClicked();
			await (this.PreviousClicked?.Invoke(this) ?? Task.CompletedTask);
		}

		private async Task Cancel()
		{
			await (this.CancelClicked?.Invoke(this) ?? Task.CompletedTask);
		}

		public FlowPanel GetButtonPanel(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_FlowDirection((ControlFlowDirection)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			FlowPanel panel = val;
			((Control)RenderButtonAsync((Panel)(object)panel, "Previous", async delegate
			{
				await Previous();
			})).set_Enabled(PreviousAvailable);
			((Control)RenderButtonAsync((Panel)(object)panel, "Skip Wizard", async delegate
			{
				await Cancel();
			})).set_BasicTooltipText("Skips the wizard.\nOnly following pages will be skipped.\nAll already completed pages will keep their applied settings.");
			if (TestConfigurationsAvailable)
			{
				((Control)RenderButtonAsync((Panel)(object)panel, "Test Configurations", async delegate
				{
					await ApplyConfigurations();
				})).set_BasicTooltipText("Applies all options on the current page to the module.");
			}
			((Control)RenderButtonAsync((Panel)(object)panel, (!NextIsFinish) ? "Next" : "Finish", async delegate
			{
				if (!NextIsFinish)
				{
					await Next();
				}
				else
				{
					await Finish();
				}
			})).set_Enabled(NextAvailable);
			((Control)panel).RecalculateLayout();
			((Control)panel).Update(GameService.Overlay.get_CurrentGameTime());
			return panel;
		}

		protected virtual Task ApplyConfigurations()
		{
			return Task.CompletedTask;
		}
	}
}
