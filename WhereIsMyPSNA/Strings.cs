using System.Collections.Generic;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace WhereIsMyPSNA
{
	internal static class Strings
	{
		private class Entry
		{
			public string En;

			public string De;

			public string Es;

			public string Fr;
		}

		private static readonly Dictionary<string, Entry> Table = new Dictionary<string, Entry>
		{
			["HideKnownNpcs_Name"] = new Entry
			{
				En = "Hide NPC if recipe is already known",
				De = "NPC ausblenden, wenn Rezept bereits bekannt ist",
				Es = "Ocultar PNJ si la receta ya se conoce",
				Fr = "Masquer le PNJ si la recette est déjà connue"
			},
			["HideKnownNpcs_Description"] = new Entry
			{
				En = "Hides the panel and copy button for NPCs whose today's recipe you already know.",
				De = "Blendet das Panel und die Kopieren-Schaltfläche für NPCs aus, deren heutiges Rezept du bereits kennst.",
				Es = "Oculta el panel y el botón de copiar para los PNJ cuya receta de hoy ya conoces.",
				Fr = "Masque le panneau et le bouton de copie pour les PNJ dont vous connaissez déjà la recette du jour."
			},
			["Tab_TodayLocations"] = new Entry
			{
				En = "Locations and Recipes",
				De = "Standorte und Rezepte",
				Es = "Ubicaciones y recetas",
				Fr = "Emplacements et recettes"
			},
			["Tab_Community"] = new Entry
			{
				En = "Community Submissions",
				De = "Community-Einsendungen",
				Es = "Envíos de la comunidad",
				Fr = "Contributions de la communauté"
			},
			["Duration_HoursMinutes"] = new Entry
			{
				En = "Duration: {0}h {1}min",
				De = "Dauer: {0} Std. {1} Min.",
				Es = "Duración: {0} h {1} min",
				Fr = "Durée : {0} h {1} min"
			},
			["Duration_HoursOnly"] = new Entry
			{
				En = "Duration: {0}h",
				De = "Dauer: {0} Std.",
				Es = "Duración: {0} h",
				Fr = "Durée : {0} h"
			},
			["Duration_MinutesOnly"] = new Entry
			{
				En = "Duration: {0} min",
				De = "Dauer: {0} Min.",
				Es = "Duración: {0} min",
				Fr = "Durée : {0} min"
			},
			["Binding_AccountBound"] = new Entry
			{
				En = "Account Bound",
				De = "Kontogebunden",
				Es = "Vinculado a la cuenta",
				Fr = "Lié au compte"
			},
			["Binding_SoulboundOnAcquire"] = new Entry
			{
				En = "Soulbound on Acquire",
				De = "Seelengebunden beim Erhalt",
				Es = "Vinculado al alma al obtenerlo",
				Fr = "Lié à l'âme à l'acquisition"
			},
			["Binding_SoulboundOnUse"] = new Entry
			{
				En = "Soulbound on Use",
				De = "Seelengebunden bei Benutzung",
				Es = "Vinculado al alma al usarlo",
				Fr = "Lié à l'âme à l'utilisation"
			},
			["FetchFailed"] = new Entry
			{
				En = "Fetch failed",
				De = "Abruf fehlgeschlagen",
				Es = "Error al obtener datos",
				Fr = "Échec de la récupération"
			},
			["Copy"] = new Entry
			{
				En = "Copy",
				De = "Kopieren",
				Es = "Copiar",
				Fr = "Copier"
			},
			["AlreadyKnowRecipe"] = new Entry
			{
				En = "You already know this recipe",
				De = "Du kennst dieses Rezept bereits",
				Es = "Ya conoces esta receta",
				Fr = "Vous connaissez déjà cette recette"
			},
			["NoChatCodeYet"] = new Entry
			{
				En = "No chat code for this location yet",
				De = "Noch kein Chat-Code für diesen Ort vorhanden",
				Es = "Aún no hay código de chat para esta ubicación",
				Fr = "Pas encore de code de discussion pour cet endroit"
			},
			["NotDeterminedYet"] = new Entry
			{
				En = "Not determined yet",
				De = "Noch nicht bestimmt",
				Es = "Aún no determinado",
				Fr = "Pas encore déterminé"
			},
			["Status_FetchingSchedule"] = new Entry
			{
				En = "Fetching schedule...",
				De = "Zeitplan wird abgerufen...",
				Es = "Obteniendo el calendario...",
				Fr = "Récupération du planning..."
			},
			["Status_LoadingRecipeData"] = new Entry
			{
				En = "Loading recipe data...",
				De = "Rezeptdaten werden geladen...",
				Es = "Cargando datos de la receta...",
				Fr = "Chargement des données de recette..."
			},
			["Status_LoadingIcons"] = new Entry
			{
				En = "Loading icons...",
				De = "Symbole werden geladen...",
				Es = "Cargando iconos...",
				Fr = "Chargement des icônes..."
			},
			["Status_CheckingKnownRecipes"] = new Entry
			{
				En = "Checking known recipes...",
				De = "Bekannte Rezepte werden geprüft...",
				Es = "Comprobando recetas conocidas...",
				Fr = "Vérification des recettes connues..."
			},
			["Community_Disclaimer"] = new Entry
			{
				En = "Community submissions are shared and unmoderated — please only submit a recipe you're confident is correct today.",
				De = "Community-Einsendungen werden geteilt und nicht moderiert – bitte reiche nur ein Rezept ein, von dem du überzeugt bist, dass es heute korrekt ist.",
				Es = "Los envíos de la comunidad se comparten y no están moderados; envía solo una receta que estés seguro de que es correcta hoy.",
				Fr = "Les contributions de la communauté sont partagées et non modérées — merci de ne soumettre une recette que si vous êtes certain qu'elle est correcte aujourd'hui."
			},
			["IUnderstand"] = new Entry
			{
				En = "I Understand",
				De = "Verstanden",
				Es = "Entendido",
				Fr = "J'ai compris"
			},
			["Community_Header"] = new Entry
			{
				En = "Submit today's recipe for an NPC if the list above hasn't updated yet.",
				De = "Reiche das heutige Rezept für einen NPC ein, falls die obige Liste noch nicht aktualisiert wurde.",
				Es = "Envía la receta de hoy para un PNJ si la lista de arriba aún no se ha actualizado.",
				Fr = "Soumettez la recette du jour pour un PNJ si la liste ci-dessus n'a pas encore été mise à jour."
			},
			["SearchItemPlaceholder"] = new Entry
			{
				En = "Search item...",
				De = "Gegenstand suchen...",
				Es = "Buscar objeto...",
				Fr = "Rechercher un objet..."
			},
			["Submit"] = new Entry
			{
				En = "Submit",
				De = "Absenden",
				Es = "Enviar",
				Fr = "Envoyer"
			},
			["Edit"] = new Entry
			{
				En = "Edit",
				De = "Bearbeiten",
				Es = "Editar",
				Fr = "Modifier"
			},
			["Community_LoadingExisting"] = new Entry
			{
				En = "Loading existing submissions...",
				De = "Vorhandene Einsendungen werden geladen...",
				Es = "Cargando envíos existentes...",
				Fr = "Chargement des contributions existantes..."
			},
			["Community_NotConfigured"] = new Entry
			{
				En = "Community submissions aren't set up yet",
				De = "Community-Einsendungen sind noch nicht eingerichtet",
				Es = "Los envíos de la comunidad aún no están configurados",
				Fr = "Les contributions de la communauté ne sont pas encore configurées"
			},
			["Community_SubmitFailed"] = new Entry
			{
				En = "Submission failed — please try again",
				De = "Einsendung fehlgeschlagen – bitte versuche es erneut",
				Es = "Error al enviar; inténtalo de nuevo",
				Fr = "Échec de l'envoi — veuillez réessayer"
			}
		};

		public static string Get(string key)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected I4, but got Unknown
			if (!Table.TryGetValue(key, out var entry))
			{
				return key;
			}
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			return (value - 1) switch
			{
				1 => entry.De ?? entry.En, 
				0 => entry.Es ?? entry.En, 
				2 => entry.Fr ?? entry.En, 
				_ => entry.En, 
			};
		}
	}
}
