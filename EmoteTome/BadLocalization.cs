using System.Collections.Generic;

namespace EmoteTome
{
	internal class BadLocalization
	{
		public static readonly int ENGLISH = 0;

		public static readonly int FRENCH = 1;

		public static readonly int GERMAN = 2;

		public static readonly int SPANISH = 3;

		public static readonly List<string> WINDOWTITLE = new List<string> { "Emote Tome", "Emote Tome", "Emote Tome", "Emote Tome" };

		public static readonly List<string> UNLOCKABLEPANELTITLE = new List<string> { "Unlockable Emotes", "Emotes pour déverrouiller", "Freischaltbare Emotes", "Emotes desbloqueables" };

		public static readonly List<string> RANKPANELTITLE = new List<string> { "Rank Emotes", "Rang Emotes", "Rang Emotes", "Rango Emotes" };

		public static readonly List<string> COREPANELTITLE = new List<string> { "Core Emotes", "Émote de base", "Kern Emotes", "Emotes de base" };

		public static readonly List<string> NOEMOTEONKEYPRESSED = new List<string> { "Can't peform emotes while a key is pressed", "Impossible d'effectuer des emotes pendant qu'une touche est pressée", "Nicht möglich, während eine Taste gedrückt ist", "No se pueden realizar emotes mientras se pulsa una tecla" };

		public static readonly List<string> NOEMOTEWHENMOVING = new List<string> { "Can't peform emotes while moving", "Impossible d'effectuer des emotes en se déplaçant", "Während der Bewegung nicht möglich", "No se pueden realizar emotes en movimiento" };

		public static readonly List<string> NOEMOTEONMOUNT = new List<string> { "Can't peform emotes while on mount", "Impossible d'effectuer des emotes lorsque l'on est sur une monture", "Auf Mounts nicht möglich", "No se pueden realizar emotes mientras se está montado" };

		public static readonly List<string> NOEMOTEINCOMBAT = new List<string> { "Can't peform emotes while in combat", "Impossible d'effectuer des emotes pendant le combat", "Im Kampf nicht möglich", "No se pueden realizar emotes en combate" };

		public static readonly List<string> SYNCHRONCHECKBOXTEXT = new List<string> { "Synchronize emotes", "Synchroniser les emotes", "Emotes synchronisieren", "Sincronizar emotes" };

		public static readonly List<string> SYNCHRONCHECKBOXTOOLTIP = new List<string> { "Synchronizes your emotes with other players.", "Synchronise vos emotes avec celles des autres joueurs.", "Synchronisiert Dein Emote mit anderen Spielern.", "Sincroniza tus emotes con los de otros jugadores." };

		public static readonly List<string> TARGETCHECKBOXTEXT = new List<string> { "Enable Targeting", "Activer le ciblage", "Anvisieren aktivieren", "Activar objetivo" };

		public static readonly List<string> TARGETCHECKBOXTOOLTIP = new List<string> { "Peforme emote on target.\nSome emotes are not affected.", "Peforme emote on target.\nCertaines emotes ne sont pas affectées.", "Führt Emote an Ziel aus.\nBetrifft nicht alle Emotes.", "Peforme emote en target.\nAlgunos emotes no se ven afectados." };
	}
}
