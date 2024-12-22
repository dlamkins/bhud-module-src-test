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

		public static readonly List<string> SHOWNAMES = new List<string> { "Show Emote Names", "Afficher le nom de Emote", "Zeige Emote Namen", "Mostrar el nombre de Emote" };

		public static readonly List<string> SHOWNAMESTEXT = new List<string> { "Shows the name of the emote under it's icon.", "Affiche le nom de l'emote sous son icône.", "Zeigt den Namen eines Emotes unter dessen Icon an.", "Muestra el nombre del emote debajo de su icono." };

		public static readonly List<string> LARGERNAMELABELS = new List<string> { "Larger Name Labels", "Étiquettes de noms plus grandes", "Groeßere Namensfelder", "Etiquetas de nombres más grandes" };

		public static readonly List<string> LARGERNAMELABELSTEXT = new List<string> { "Some emote names are long, this setting will give them more space.", "Certains noms d'émoticônes sont longs, ce paramètre leur donnera plus d'espace.", "Manche Emotenamen sind lang, diese Option gibt ihnen mehr Platz.", "Algunos nombres de emoticonos son largos, esta configuración les dará más espacio." };

		public static readonly List<string> HALLOWEENMODE = new List<string> { "Halloween Mode", "Mode Halloween", "Halloween Modus", "Modo de Halloween" };

		public static readonly List<string> HALLOWEENMODETEXT = new List<string> { "Only shows those emotes in core panel that are used in 'Mad King Says'.\nSets all core emotes back to 'Show' when disabled.", "Affiche uniquement les émoticônes du noyau utilisés dans 'Mad King Says'.\nRétablit tous les émoticônes du noyau à 'Afficher' lorsqu'il est désactivé.", "Zeigt nur jene Emotes in der Kern-Kategorie, die für 'Der Verrueckte Koenig Sagt' gebraucht werden.\nSetzt alle Emotes auf 'Zeigen', wenn deaktiviert.", "Solo muestra los emoticonos en el panel principal que se utilizan en 'Mad King Says'.\nRestablece todos los emoticonos principales a 'Mostrar' cuando está desactivado." };

		public static readonly List<string> EMOTETEXT = new List<string> { "Shows/Hides this emote", "Affiche/Masque cet emote", "Zeigt/versteckt dieses Emote", "Muestra/Oculta este emote" };

		public static readonly List<string> CHECKKEY = new List<string> { "Check keystrokes for emotes", "vérifier les touches enfoncées pour les émotes", "Prüfe Tastendruck bei Emotes", "Verifique las pulsaciones de teclas para ver emoticones" };

		public static readonly List<string> CHECKKEYTEXT = new List<string> { "Does not allow emotes when a key is pressed.\nIt's not recommended to disable this setting.", "N'autorise pas les émoticônes lorsqu'une touche est enfoncée.\nIl n'est pas recommandé de désactiver ce paramètre.", "Erlaubt keine Emotes, während eine Taste gedrückt wird.\nEs wird nicht empfohlen, diese Einstellung zu deaktivieren.", "No permite gestos cuando se presiona una tecla.\nNo se recomienda deshabilitar esta configuración." };

		public static readonly List<string> CHECKMOVE = new List<string> { "Check movement for emotes", "Vérifiez le mouvement pour les émoticônes", "Prüfe Bewegung bei Emotes", "Comprueba el movimiento de los emoticones" };

		public static readonly List<string> CHECKMOVETEXT = new List<string> { "Does not allow emotes when player is moving.\nIt's not recommended to disable this setting.", "N'autorise pas les émoticônes lorsque le joueur se déplace.\nIl n'est pas recommandé de désactiver ce paramètre.", "Erlaubt keine Emotes, während sich der Spieler bewegt.\nEs wird nicht empfohlen, diese Einstellung zu deaktivieren.", "No permite gestos cuando el jugador está en movimiento.\nNo se recomienda deshabilitar esta configuración." };

		public static readonly List<string> BECKON = new List<string> { "Beckon", "Approcher", "Herbeiwinken", "Señas" };

		public static readonly List<string> BOW = new List<string> { "Bow", "Reverence", "Verbeugen", "Inclinarse" };

		public static readonly List<string> CHEER = new List<string> { "Cheer", "Encourager", "Jubeln", "Animar" };

		public static readonly List<string> COWER = new List<string> { "Cower", "Lache", "Ducken", "Cobarde" };

		public static readonly List<string> CROSSARMS = new List<string> { "Crossarms", "Brascroises", "Armekreuzen", "Cruzarse" };

		public static readonly List<string> CRY = new List<string> { "Cry", "Pleurer", "Weinen", "Llorar" };

		public static readonly List<string> DANCE = new List<string> { "Dance", "Danse", "Tanzen", "Bailar" };

		public static readonly List<string> FACEPALM = new List<string> { "Facepalm", "Fache", "Veraergert", "Disgustado" };

		public static readonly List<string> KNEEL = new List<string> { "Kneel", "Genoux", "Hinknien", "Arrodillarse" };

		public static readonly List<string> LAUGH = new List<string> { "Laugh", "Rire", "Lachen", "Reír" };

		public static readonly List<string> NO = new List<string> { "No", "Non", "Nein", "No" };

		public static readonly List<string> POINT = new List<string> { "Point", "Montrer", "Zeigen", "Señalar" };

		public static readonly List<string> PONDER = new List<string> { "Ponder", "Cogite", "Gruebeln", "Reflexionar" };

		public static readonly List<string> SAD = new List<string> { "Sad", "Triste", "Traurig", "Triste" };

		public static readonly List<string> SALUTE = new List<string> { "Salute", "Salut", "Salutieren", "Firmesr" };

		public static readonly List<string> SHRUG = new List<string> { "Shrug", "Epaules", "Schulterzucken", "Encogerse" };

		public static readonly List<string> SIT = new List<string> { "Sit", "Asseoir", "Hinsetzen", "Sentarse" };

		public static readonly List<string> SLEEP = new List<string> { "Sleep", "Dormir", "Schlafen", "Dormir" };

		public static readonly List<string> SURPRISED = new List<string> { "Surprised", "Surpris", "Ueberrascht", "Sorpresa" };

		public static readonly List<string> TALK = new List<string> { "Talk", "Parler", "Reden", "Hablar" };

		public static readonly List<string> THANKS = new List<string> { "Thanks", "Merci", "Danke", "Gracias" };

		public static readonly List<string> THREATEN = new List<string> { "Threaten", "Menace", "Drohen", "Amenaza" };

		public static readonly List<string> WAVE = new List<string> { "Wave", "Coucou", "Winken", "Saludar" };

		public static readonly List<string> YES = new List<string> { "Yes", "Oui", "Ja", "Si" };

		public static readonly List<string> BLESS = new List<string> { "Bless", "Bénir", "Segnen", "Bless" };

		public static readonly List<string> GEARGRIND = new List<string> { "Geargrind", "Coureur", "Endlos", "Corredor" };

		public static readonly List<string> HEROIC = new List<string> { "Heroic", "Heroic", "Heldenhaft", "Heroic" };

		public static readonly List<string> HISS = new List<string> { "Hiss", "Feule", "Zischen", "Bufar" };

		public static readonly List<string> MAGICJUGGLE = new List<string> { "Magicjuggle", "Jonglagemagique", "Magischesjonglieren", "Malabarmagico" };

		public static readonly List<string> PAPER = new List<string> { "Paper", "Feuille", "Papier", "Papel" };

		public static readonly List<string> PLAYDEAD = new List<string> { "Playdead", "Faitlemort", "Totstellen", "Yacer" };

		public static readonly List<string> POSSESSED = new List<string> { "Possessed", "Possede", "Besessen", "Poseido" };

		public static readonly List<string> READBOOK = new List<string> { "Readbook", "Lecture", "Buchlesen", "Leerlibro" };

		public static readonly List<string> ROCK = new List<string> { "Rock", "Pierre", "Stein", "Piedra" };

		public static readonly List<string> ROCKOUT = new List<string> { "Rockout", "Hardrock", "Abrocken", "Rockear" };

		public static readonly List<string> SCISSORS = new List<string> { "Scissors", "Ciseaux", "Schere", "Tijeras" };

		public static readonly List<string> SERVE = new List<string> { "Serve", "Servir", "Servieren", "Servir" };

		public static readonly List<string> SHIVER = new List<string> { "Shiver", "Tremble", "Zittern", "Tiritar" };

		public static readonly List<string> SHIVERPLUS = new List<string> { "Shiverplus", "Tremblefort", "Zitternplus", "Tiritarmucho" };

		public static readonly List<string> SHUFFLE = new List<string> { "Shuffle", "Shuffle", "Shuffle", "Mezclador" };

		public static readonly List<string> SIPCOFFEE = new List<string> { "Sipcoffee", "Boirecafe", "Kaffeeschluerf", "Sipcoffee" };

		public static readonly List<string> STEP = new List<string> { "Step", "Esquive", "Schritt", "Paso" };

		public static readonly List<string> STRETCH = new List<string> { "Stretch", "Étirement", "Dehnen", "Estirarse" };

		public static readonly List<string> UNLEASH = new List<string> { "Unleash", "Libere", "Entfesseln", "Desatar" };

		public static readonly List<string> PETALTHROW = new List<string> { "Petalthrow", "Lancerpetale", "Bluetenblaetterwerfen", "Lanzarpetalos" };

		public static readonly List<string> BREAKDANCE = new List<string> { "Breakdance", "Breakdance", "Breakdance", "Breakdance" };

		public static readonly List<string> BOOGIE = new List<string> { "Boogie", "Boogie", "Boogie", "Boogie" };

		public static readonly List<string> BEAR = new List<string> { "Bear", "Ours", "Baer", "Osa" };

		public static readonly List<string> DEER = new List<string> { "Deer", "Daim", "Hirsch", "Ciervo" };

		public static readonly List<string> DOLYAK = new List<string> { "Dolyak", "Dolyak", "Dolyak", "Dolyak" };

		public static readonly List<string> DRAGON = new List<string> { "Dragon", "Dragon", "Drache", "Dragón" };

		public static readonly List<string> PHOENIX = new List<string> { "Phoenix", "Phénix", "Phoenix", "Fénix" };

		public static readonly List<string> RABBIT = new List<string> { "Rabbit", "Lapin", "Kaninchen", "Conejo" };

		public static readonly List<string> SHARK = new List<string> { "Shark", "Requin", "Hai", "Tiburón" };

		public static readonly List<string> TIGER = new List<string> { "Tiger", "Tigre", "Tiger", "Tigre" };

		public static readonly List<string> WOLF = new List<string> { "Wolf", "Loup", "Wolf", "Lobo" };

		public static readonly List<string> YOURRANK = new List<string> { "Your Rank", "Ton Rang", "Dein Rang", "Su Rango" };
	}
}
