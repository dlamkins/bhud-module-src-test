using System;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Flurl.Http;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class EventTableBlishHUDAPIView : BlishHUDAPIView
	{
		private const string DASHBOARD_URL = "https://blish-hud.estreya.de";

		protected override bool DrawKofiStatus => false;

		public EventTableBlishHUDAPIView(Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, BlishHudApiService blishHudApiService, IFlurlClient flurlClient)
			: base(apiManager, iconService, translationService, blishHudApiService, flurlClient)
		{
		}

		protected override FormattedLabelBuilder GetDescriptionBuilder()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new FormattedLabelBuilder().CreatePart("Login to use the custom events you created ", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("here.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
			{
				b.SetHyperLink("https://blish-hud.estreya.de");
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("The created events will show up in your areas/tables, if not disabled", (Action<FormattedLabelPartBuilder>)delegate
				{
				});
		}
	}
}
