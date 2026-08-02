using System.Linq;

namespace TurtleMyWaypoint
{
	internal static class WaypointData
	{
		public static readonly RegionSection[] CoreTyria = new RegionSection[5]
		{
			new RegionSection("Ascalon", "Ascalon", new MapEntry[7]
			{
				new MapEntry("Plains of Ashford", "Plaines d'Ashford", new string[18]
				{
					"[&BH8BAAA=]", "[&BIABAAA=]", "[&BIEBAAA=]", "[&BIIBAAA=]", "[&BIMBAAA=]", "[&BIQBAAA=]", "[&BIUBAAA=]", "[&BIYBAAA=]", "[&BIcBAAA=]", "[&BIgBAAA=]",
					"[&BIkBAAA=]", "[&BIoBAAA=]", "[&BJcDAAA=]", "[&BJgDAAA=]", "[&BJkDAAA=]", "[&BMcDAAA=]", "[&BMgDAAA=]", "[&BPgGAAA=]"
				}, new string[18]
				{
					"Vir's Gate Waypoint", "Smokestead Waypoint", "Greysteel Armory Waypoint", "Martyr's Waypoint", "Temperus Point Waypoint", "Ashford Waypoint", "Adorea Waypoint", "Ascalonian Catacombs Waypoint", "Ascalon City Waypoint", "Watchcrag Tower Waypoint",
					"Duskrend Overlook Waypoint", "Irondock Shipyard Waypoint", "Feritas Waypoint", "Guardpoint Decimus Waypoint", "Spirit Hunter Camp Waypoint", "Loreclaw Waypoint", "Phasmatis Waypoint", "Langmar Estate Waypoint"
				}, new string[18]
				{
					"Point de passage du Portail de Vir", "Point de passage de Fumerolles", "Point de passage de l'Armurerie de Grisacier", "Point de passage du martyre", "Point de passage du Point de Temperus", "Point de passage d'Ashford", "Point de passage d'Adoréa", "Point de passage des Catacombes d'Ascalon", "Point de passage de la Cité d'Ascalon", "Point de passage de la Tour de guet de l'Escarpement",
					"Point de passage du Belvédère de Scinde-nuit", "Point de passage du Chantier naval de Dockfer", "Point de passage de Feritas", "Point de passage du Point de garde de Décimus", "Point de passage du Camp du Chasseur d'esprits", "Point de passage de Fablegriffe", "Point de passage de Phasmatis", "Point de passage du Domaine de Langmar"
				}),
				new MapEntry("Diessa Plateau", "Plateau de Diessa", new string[19]
				{
					"[&BNkAAAA=]", "[&BNoAAAA=]", "[&BNsAAAA=]", "[&BNwAAAA=]", "[&BN0AAAA=]", "[&BN4AAAA=]", "[&BF4BAAA=]", "[&BF8BAAA=]", "[&BGABAAA=]", "[&BGEBAAA=]",
					"[&BGIBAAA=]", "[&BGMBAAA=]", "[&BGQBAAA=]", "[&BJoDAAA=]", "[&BMQDAAA=]", "[&BMUDAAA=]", "[&BMYDAAA=]", "[&BMkDAAA=]", "[&BEIEAAA=]"
				}, new string[19]
				{
					"Charrgate Haven Waypoint", "Blasted Moors Waypoint", "Bloodsaw Mill Waypoint", "Font of Rhand Waypoint", "Nageling Waypoint", "Nolan Waypoint", "Oldgate Waypoint", "Butcher's Block Waypoint", "Charradis Estate Waypoint", "Breached Wall Waypoint",
					"Incendio Waypoint", "Nemus Grove Waypoint", "Breachwater Waypoint", "Dawnright Estate Waypoint", "Bloodcliff Waypoint", "Sanctum Waypoint", "Redreave Mill Waypoint", "Manbane's Waypoint", "Blackblade Waypoint"
				}, new string[19]
				{
					"Point de passage du Refuge de Porticharr", "Point de passage des landes ravagées", "Point de passage de la Scierie de Mortesciée", "Point de passage de la Fontaine de Rhand", "Point de passage de Nageling", "Point de passage de Nolan", "Point de passage de Portevieille", "Point de passage du Bloc du boucher", "Point de passage du domaine de Charradis", "Point de passage du Mur fendu",
					"Point de passage d'Incendio", "Point de passage du Bosquet de Nemus", "Point de passage de Brèchezeaux", "Point de passage du Domaine de Dawnright.", "Point de passage de Rougefalaise", "Point de passage du Sanctuaire", "Point de passage de la Scierie de Couperouge", "Point de passage du Mort-Homme", "Point de passage de Lamenoire"
				}),
				new MapEntry("Fields of Ruin", "Champs de ruine", new string[17]
				{
					"[&BNMAAAA=]", "[&BNQAAAA=]", "[&BNUAAAA=]", "[&BNYAAAA=]", "[&BNcAAAA=]", "[&BNgAAAA=]", "[&BEoBAAA=]", "[&BEsBAAA=]", "[&BEwBAAA=]", "[&BE0BAAA=]",
					"[&BE4BAAA=]", "[&BE8BAAA=]", "[&BFABAAA=]", "[&BFEBAAA=]", "[&BDwEAAA=]", "[&BD0EAAA=]", "[&BD4EAAA=]"
				}, new string[17]
				{
					"Hawkgates Waypoint", "Deathblade's Watch Waypoint", "Summit Waypoint", "Tyler's Bivouac Waypoint", "Tenaebron Waypoint", "Rosko's Campsite Waypoint", "Skoll's Bivouac Waypoint", "Helliot Mine Waypoint", "Fangfury Watch Waypoint", "Vulture's Waypoint",
					"Wreckage of Bloodgorge Watch Waypoint", "Ogre Road Waypoint", "Forlorn Gate Waypoint", "Fallen Angels Garrison Waypoint", "Spotter's Waypoint", "Kestrel Waypoint", "Thunderbreak Waypoint"
				}, new string[17]
				{
					"Point de passage des Portails du faucon", "Point de passage du Guet de Mortelame", "Point de passage du Sommet", "Point de passage du Bivouac de Tyler", "Point de passage de Tenaebron", "Point de passage du Campement de Rosko", "Point de passage du Bivouac de Skoll", "Point de passage de la Mine d'Hellion", "Point de passage du Guet de Mordrage", "Point de passage du Vautour",
					"Point de passage des Décombres du guet de Gorgesang", "Point de passage de la Route de l'Ogre", "Point de passage du Portail de Folespoir", "Point de passage de la Garnison des Anges déchus", "Point de passage du Guetteur", "Point de passage de Kestrel", "Point de passage de Brise-tonnerre"
				}),
				new MapEntry("Blazeridge Steppes", "Les Steppes de la strie flamboyante", new string[18]
				{
					"[&BPkBAAA=]", "[&BPoBAAA=]", "[&BPsBAAA=]", "[&BPwBAAA=]", "[&BP0BAAA=]", "[&BP4BAAA=]", "[&BP8BAAA=]", "[&BAACAAA=]", "[&BAECAAA=]", "[&BAICAAA=]",
					"[&BAMCAAA=]", "[&BAQCAAA=]", "[&BAUCAAA=]", "[&BE4DAAA=]", "[&BE8DAAA=]", "[&BFADAAA=]", "[&BFEDAAA=]", "[&BFIDAAA=]"
				}, new string[18]
				{
					"The Last Whiskey Bar Waypoint", "Steeleye Waypoint", "Tumok's Waypoint", "Twin Sisters Waypoint", "Behem Waypoint", "Expanse Waypoint", "Guardian Stone Waypoint", "Lunk Kraal Waypoint", "Terra Carorunda Waypoint", "Brokentooth Maw Waypoint",
					"Kindling Waypoint", "Brandview Waypoint", "Refuge Sanctum Waypoint", "Lowland Burns Waypoint", "Gastor Gullet Waypoint", "Kinar Fort Waypoint", "Splintercrest Fort Waypoint", "Soot Road Waypoint"
				}, new string[18]
				{
					"Point de passage du Dernier pour la route", "Point de passage de l'Œil-d'acier", "Point de passage de Tumok", "Point de passage des Sœurs Jumelles", "Point de passage de Behem", "Point de passage de la plaine", "Point de passage de la Pierre gardienne", "Point de passage du Kraal des Ogres", "Point de passage de Terra Carorunda", "Point de passage du gouffre de Cassedent",
					"Point de passage de Petit-bois", "Point de passage du Glaive", "Point de passage du Sanctuaire", "Point de passage des Basses Terres Calcinées", "Point de passage du Gosier de Gastor", "Point de passage de Fort Kinar", "Point de passage du Fort de Crête-Fendue", "Point de passage de la Route de Suie"
				}),
				new MapEntry("Iron Marches", "Marais de fer", new string[14]
				{
					"[&BOIBAAA=]", "[&BOMBAAA=]", "[&BOQBAAA=]", "[&BOUBAAA=]", "[&BOYBAAA=]", "[&BOcBAAA=]", "[&BOgBAAA=]", "[&BOkBAAA=]", "[&BOoBAAA=]", "[&BOsBAAA=]",
					"[&BOwBAAA=]", "[&BO0BAAA=]", "[&BO4BAAA=]", "[&BO8BAAA=]"
				}, new string[14]
				{
					"Dewclaw Waypoint", "Bloodfin Lake Waypoint", "Old Piken Ruins Waypoint", "Warhound Village Waypoint", "Hellion Waypoint", "Village of Scalecatch Waypoint", "Sleekfur Encampment Waypoint", "Brandwatch Encampment Waypoint", "Viper's Run Waypoint", "Town of Cowlfang's Star Waypoint",
					"Bulwark Waypoint", "Firewatch Encampment Waypoint", "Gladefall Waypoint", "Grostogg's Kraal Waypoint"
				}, new string[14]
				{
					"Point de passage d'Ergoville", "Point de passage du Lac Pourprenage", "Point de passage des Ruines de Piken la vieille", "Point de passage du Village de Molosseguerre", "Point de passage d'Hellion", "Point de passage du Village d'Attrapécaille", "Point de passage du camp de Doucepelisse", "Point de passage du Campement du Guet du stigmate", "Point de passage de la Piste de la Vipère", "Point de passage de la Ville de l'Etoile de Caninecol",
					"Point de passage du Rempart", "Point de passage du Campement de Guet-du-feu", "Point de passage de Gladefall", "Point de passage du Kraal de Grostogg"
				}),
				new MapEntry("Fireheart Rise", "Montée de Flambecœur", new string[18]
				{
					"[&BBYCAAA=]", "[&BBcCAAA=]", "[&BBgCAAA=]", "[&BBkCAAA=]", "[&BBoCAAA=]", "[&BBsCAAA=]", "[&BBwCAAA=]", "[&BB0CAAA=]", "[&BB4CAAA=]", "[&BB8CAAA=]",
					"[&BCACAAA=]", "[&BCECAAA=]", "[&BCICAAA=]", "[&BCMCAAA=]", "[&BCQCAAA=]", "[&BCUCAAA=]", "[&BCYCAAA=]", "[&BEAFAAA=]"
				}, new string[18]
				{
					"Sati Waypoint", "Pig Iron Waypoint", "Tuyere Command Post Waypoint", "Severed Breach Waypoint", "Breaktooth's Waypoint", "Havoc Waypoint", "Vidius Castrum Waypoint", "Apostate Waypoint", "Rustbowl Waypoint", "Switchback Waypoint",
					"Icespear's Waypoint", "Snow Ridge Camp Waypoint", "Vorgas Garrison Waypoint", "Forlorn Waypoint", "Senecus Castrum Waypoint", "Simurgh Waypoint", "Keeper's Waypoint", "The Citadel of Flame Waypoint"
				}, new string[18]
				{
					"Point de passage de Sati", "Point de passage de fonte brute", "Point de passage du Poste de commandement de Tuyère", "Point de passage de Brèche brisée", "Point de passage de Cassedent", "Point de passage d'Havoc", "Point de passage du Camp fortifié de Vidius", "Point de passage de l'Apostat", "Point de passage de Bolrouille", "Point de passage des Lacets",
					"Point de passage de la Pointe de glace", "Point de passage du Camp de la Crête de neige", "Point de passage de la Garnison de Vorgas", "Point de passage de Folespoir", "Point de passage du Camp fortifié de Sénécus", "Point de passage de Simurgh", "Point de passage du Gardien", "Point de passage de la Citadelle de la Flamme"
				}),
				new MapEntry("Black Citadel", "La Citadelle noire", new string[12]
				{
					"[&BKQDAAA=]", "[&BKUDAAA=]", "[&BKYDAAA=]", "[&BKcDAAA=]", "[&BKgDAAA=]", "[&BKkDAAA=]", "[&BKoDAAA=]", "[&BKsDAAA=]", "[&BKwDAAA=]", "[&BK0DAAA=]",
					"[&BDcEAAA=]", "[&BCkHAAA=]"
				}, new string[12]
				{
					"Gladium Waypoint", "Mustering Ground Waypoint", "Memorial Waypoint", "Factorium Waypoint", "Diessa Gate Waypoint", "Ligacus Aquilo Waypoint", "Bane Waypoint", "Hero's Waypoint", "Ruins of Rin Waypoint", "Imperator's Waypoint",
					"Junker's Waypoint", "Haunted Nolani Waypoint"
				}, new string[12]
				{
					"Point de passage du gladium", "Point de passage des Terres du rassemblement", "Point de passage du Mémorial", "Point de passage du Factorium", "Point de passage de la Porte de Diessa", "Point de passage de Ligacus Aquilo", "Point de passage du Fléau", "Point de passage du Héros", "Point de passage des Ruines de Rin", "Point de passage de l'Imperator",
					"Point de passage des épaves", "Point de passage de Nolani hanté"
				})
			}),
			new RegionSection("Kryta", "Kryte", new MapEntry[7]
			{
				new MapEntry("Queensdale", "La Vallée de la reine", new string[16]
				{
					"[&BO8AAAA=]", "[&BPAAAAA=]", "[&BPEAAAA=]", "[&BPIAAAA=]", "[&BPMAAAA=]", "[&BPQAAAA=]", "[&BPUAAAA=]", "[&BPYAAAA=]", "[&BPcAAAA=]", "[&BPgAAAA=]",
					"[&BPkAAAA=]", "[&BPoAAAA=]", "[&BPsAAAA=]", "[&BPwAAAA=]", "[&BEQDAAA=]", "[&BEUDAAA=]"
				}, new string[16]
				{
					"Shaemoor Waypoint", "Fields Waypoint", "Garrison Waypoint", "Crossing Waypoint", "Phinney Waypoint", "Vale Waypoint", "Heartwood Pass Camp Waypoint", "Claypool Waypoint", "Swamplost Haven Waypoint", "Krytan Waypoint",
					"Ojon's Lumbermill Waypoint", "Beetletun Waypoint", "Tunwatch Redoubt Waypoint", "Godslost Waypoint", "Orchard Waypoint", "Scaver Waypoint"
				}, new string[16]
				{
					"Point de passage de Shaemoor", "Point de passage des champs", "Point de passage de la Garnison", "Point de passage du Carrefour", "Point de passage de Phinney", "Point de passage de la vallée", "Point de passage du Camp de la passe de Boisecœur", "Point de passage de Claypool", "Point de passage du Refuge des Marais de la Perdition", "Point de passage krytien",
					"Point de passage de la Scierie d'Ojon", "Point de passage de Beetletun", "Point de passage de Redoute Miretun", "Point de passage du Marais d'Anathema", "Point de passage du verger", "Point de passage de Scaver"
				}),
				new MapEntry("Kessex Hills", "Collines de Kessex", new string[16]
				{
					"[&BAMAAAA=]", "[&BAQAAAA=]", "[&BAYAAAA=]", "[&BAcAAAA=]", "[&BAgAAAA=]", "[&BAoAAAA=]", "[&BAwAAAA=]", "[&BBAAAAA=]", "[&BBEAAAA=]", "[&BBIAAAA=]",
					"[&BBMAAAA=]", "[&BBQAAAA=]", "[&BBUAAAA=]", "[&BBYAAAA=]", "[&BLkDAAA=]", "[&BLoDAAA=]"
				}, new string[16]
				{
					"Fort Salma Waypoint", "Overlord's Waypoint", "Halacon Waypoint", "Sojourner's Waypoint", "Delanian Waypoint", "Ireko Tradecamp Waypoint", "Shadowheart Site Waypoint", "Viathan Waypoint", "Darkwound Waypoint", "Cereboth Waypoint",
					"Overlake Haven Waypoint", "Kessex Haven Waypoint", "Cavernhold Camp Waypoint", "Greyhoof Camp Waypoint", "Earthworks Camp Waypoint", "Gap Waypoint"
				}, new string[16]
				{
					"Point de passage de Fort Salma", "Point de passage du seigneur", "Point de passage d'Halacorn", "Point de passage de l'Itinérant", "Point de passage de Delania", "Point de passage du Comptoir d'Ireko", "Point de passage du Site d'Ombrecœur", "Point de passage de Viathan", "Point de passage de Sombreplaie", "Point de passage de Cereboth",
					"Point de passage du Refuge d'Overlac", "Point de passage du Refuge de Kessex", "Point de passage du Camp de Cavernhold", "Point de passage du Camp de Sabogris", "Point de passage du Camp Façonneterre", "Point de passage du ravin"
				}),
				new MapEntry("Gendarran Fields", "Champs de Gendarran", new string[22]
				{
					"[&BN8AAAA=]", "[&BOAAAAA=]", "[&BOEAAAA=]", "[&BOIAAAA=]", "[&BOMAAAA=]", "[&BOQAAAA=]", "[&BO0AAAA=]", "[&BO4AAAA=]", "[&BIsBAAA=]", "[&BIwBAAA=]",
					"[&BI0BAAA=]", "[&BI4BAAA=]", "[&BI8BAAA=]", "[&BJABAAA=]", "[&BJEBAAA=]", "[&BJIBAAA=]", "[&BJMBAAA=]", "[&BJQBAAA=]", "[&BJsDAAA=]", "[&BMwDAAA=]",
					"[&BM0DAAA=]", "[&BAIEAAA=]"
				}, new string[22]
				{
					"Traveler's Dale Waypoint", "Stoneguard Gate Waypoint", "Broadhollow Waypoint", "Oogooth Waypoint", "Cornucopian Fields Waypoint", "Provern Shore Waypoint", "Junction Haven Waypoint", "Winter Haven Waypoint", "First Haven Waypoint", "Talajian Waypoint",
					"Blood Hill Waypoint", "Nebo Terrace Waypoint", "Ascalon Settlement Waypoint", "Northfields Waypoint", "Applenook Hamlet Waypoint", "Vigil Keep Waypoint", "Icegate Waypoint", "Almuten Waypoint", "Lionbridge Waypoint", "Snowblind Waypoint",
					"Brigantine Waypoint", "Bloodfields Waypoint"
				}, new string[22]
				{
					"Point de passage de la Vallée du voyageur", "Point de passage du Portail de Gardepierre", "Point de passage du gouffre", "Point de passage d'Oogooth", "Point de passage des Champs de Cornabonde", "Point de passage de la Côte de Provern", "Point de passage du Refuge de la jonction", "Point de passage du Refuge hivernal", "Point de passage du Premier Refuge", "Point de passage de Talajian",
					"Point de passage de la Colline pourpre", "Point de passage de la Terrasse de Nebo", "Point de passage de la Colonie d'Ascalon", "Point de passage des Champs du nord", "Point de passage du Hameau de Pommeville", "Point de passage du Fort des Veilleurs", "Point de passage de la porte des glaces", "Point de passage d'Almuten", "Point de passage du Pont-aux-Lions", "Point de passage d'Aveugleneige",
					"Point de passage de Brigantine", "Point de passage des Champs du sang"
				}),
				new MapEntry("Harathi Hinterlands", "Hinterlands harathis", new string[15]
				{
					"[&BKUAAAA=]", "[&BKYAAAA=]", "[&BKcAAAA=]", "[&BKgAAAA=]", "[&BKkAAAA=]", "[&BKoAAAA=]", "[&BKsAAAA=]", "[&BKwAAAA=]", "[&BK0AAAA=]", "[&BK4AAAA=]",
					"[&BK8AAAA=]", "[&BLAAAAA=]", "[&BLEAAAA=]", "[&BLIAAAA=]", "[&BMMAAAA=]"
				}, new string[15]
				{
					"Faun's Waypoint", "Shieldbluff Waypoint", "Seraph's Landing Waypoint", "Wynchona Rally Point Waypoint", "Grey Gritta's Waypoint", "Nightguard Waypoint", "Demetra Waypoint", "Recovery Camp Waypoint", "Barricade Camp Waypoint", "Trebusha's Overlook Waypoint",
					"Bridgewatch Camp Waypoint", "Junction Camp Waypoint", "Cloven Hoof Waypoint", "Arca Waypoint", "Arcallion Waypoint"
				}, new string[15]
				{
					"Point de passage du faune", "Point de passage de Chimèrepavois", "Point de passage du Plateau du Séraphin", "Point de passage du Point de ralliement de Wynchona", "Point de passage de Grey Gritta", "Point de passage de Gardenuit", "Point de passage de Demetra", "Point de passage du Camp de Récupération", "Point de passage du Camp de la barricade", "Point de passage du Belvédère de Trebusha",
					"Point de passage du Camp du Guet du pont", "Point de passage du Camp de la jonction", "Point de passage du Sabot fendu", "Point de passage d'Arca", "Point de passage d'Arcallion"
				}),
				new MapEntry("Bloodtide Coast", "Côte de la marée sanglante", new string[15]
				{
					"[&BKMBAAA=]", "[&BKQBAAA=]", "[&BKUBAAA=]", "[&BKYBAAA=]", "[&BKcBAAA=]", "[&BKgBAAA=]", "[&BKkBAAA=]", "[&BKoBAAA=]", "[&BKsBAAA=]", "[&BKwBAAA=]",
					"[&BK0BAAA=]", "[&BK4BAAA=]", "[&BK8BAAA=]", "[&BLABAAA=]", "[&BAsEAAA=]"
				}, new string[15]
				{
					"Archen Foreland Waypoint", "Sorrowful Waypoint", "Stormbluff Waypoint", "Marshwatch Haven Waypoint", "Remanda Waypoint", "Laughing Gull Waypoint", "Barrier Camp Waypoint", "Firthside Vigil Waypoint", "Lostwreck Waypoint", "Bogside Camp Waypoint",
					"Mournful Waypoint", "Castavall Waypoint", "Jelako Waypoint", "Whisperwill Waypoint", "Deadend Waypoint"
				}, new string[15]
				{
					"Point de passage du Promontoire d'Archen", "Point de passage des Lamentations", "Point de passage de la Falaise tumultueuse", "Point de passage du Refuge de Miremarais", "Point de passage de Remanda", "Point de passage de la Mouette rieuse", "Point de passage du Camp de la barrière", "Point de passage du Poste de surveillance de l'Estuaire", "Point de passage d'Épaveperdue", "Point de passage du Camp des marais",
					"Point de passage lugubre", "Point de passage de Castavall", "Point de passage de Jelako", "Point de passage de Murmureguise", "Point de passage de l'Impasse"
				}),
				new MapEntry("Divinity's Reach", "Le Promontoire divin", new string[13]
				{
					"[&BCMDAAA=]", "[&BCQDAAA=]", "[&BCUDAAA=]", "[&BCYDAAA=]", "[&BCcDAAA=]", "[&BCgDAAA=]", "[&BCkDAAA=]", "[&BCoDAAA=]", "[&BCsDAAA=]", "[&BCwDAAA=]",
					"[&BC0DAAA=]", "[&BC4DAAA=]", "[&BP4EAAA=]"
				}, new string[13]
				{
					"Dwayna Waypoint", "Grenth Waypoint", "Kormir Waypoint", "Lyssa Waypoint", "Melandru Waypoint", "Balthazar Waypoint", "Palace Waypoint", "Commons Waypoint", "Rurikton Waypoint", "Crown Pavilion Waypoint",
					"Ossan Waypoint", "Salma Waypoint", "Ministers Waypoint"
				}, new string[13]
				{
					"Point de passage de Dwayna", "Point de passage de Grenth", "Point de passage de Kormir", "Point de passage de Lyssa", "Point de passage de Melandru", "Point de passage de Balthazar", "Point de passage du Palais", "Point de passage du quartier populaire", "Point de passage de Rurikton", "Point de passage du Pavillon de la Couronne",
					"Point de passage d'Ossa", "Point de passage de Salma", "Point de passage des ministres"
				}),
				new MapEntry("Lion's Arch", "L'Arche du Lion", new string[13]
				{
					"[&BAwEAAA=]", "[&BA0EAAA=]", "[&BA4EAAA=]", "[&BA8EAAA=]", "[&BBAEAAA=]", "[&BBEEAAA=]", "[&BC0EAAA=]", "[&BC4EAAA=]", "[&BC8EAAA=]", "[&BDAEAAA=]",
					"[&BDEEAAA=]", "[&BDIEAAA=]", "[&BDMEAAA=]"
				}, new string[13]
				{
					"Commodore's Quarter Waypoint", "Guild Bluff Waypoint", "Bloodcoast Ward Waypoint", "Claw Island Portage Waypoint", "Trader's Forum Waypoint", "Gate Hub Plaza Waypoint", "Diverse Ledges Waypoint", "Western Ward Waypoint", "Sanctum Harbor Waypoint", "Fort Marriner Waypoint",
					"Eastern Ward Waypoint", "Cavern Waypoint", "Farshore Waypoint"
				}, new string[13]
				{
					"Point de passage du Quartier du Commodore", "Point de passage des Falaises de la guilde", "Point de passage du quartier de la Côte sanglante", "Point de passage du Portage de l'Île de la griffe", "Point de passage du Forum des commerçants", "Point de passage de la Grand-place de la porte", "Point de passage des Corniches diverses", "Point de passage du Quartier ouest", "Point de passage du Port du sanctuaire", "Point de passage de Fort Marrin",
					"Point de passage du Quartier est", "Point de passage de la caverne", "Point de passage d'Autrerive"
				})
			}),
			new RegionSection("Maguuma Jungle", "Jungle de Maguuma", new MapEntry[7]
			{
				new MapEntry("Caledon Forest", "Forêt de Caledon", new string[18]
				{
					"[&BDQBAAA=]", "[&BDUBAAA=]", "[&BDYBAAA=]", "[&BDcBAAA=]", "[&BDgBAAA=]", "[&BDkBAAA=]", "[&BDoBAAA=]", "[&BDsBAAA=]", "[&BDwBAAA=]", "[&BD0BAAA=]",
					"[&BD4BAAA=]", "[&BD8BAAA=]", "[&BEABAAA=]", "[&BEEBAAA=]", "[&BEIBAAA=]", "[&BEwDAAA=]", "[&BEEFAAA=]", "[&BP4FAAA=]"
				}, new string[18]
				{
					"Astorea Waypoint", "Spiral Waypoint", "Caer Astorea Waypoint", "Gleaner's Cove Waypoint", "Brigid's Overlook Waypoint", "Sperrins Waypoint", "Mabon Waypoint", "Town of Cathal Waypoint", "Caledon Haven Waypoint", "Titan's Staircase Waypoint",
					"Falias Thorp Waypoint", "Hamlet of Annwen Waypoint", "Kraitbane Haven Waypoint", "Wychmire Waypoint", "Wardenhurst Waypoint", "Lionguard Waystation Waypoint", "Twilight Arbor Waypoint", "Sleive's Waypoint"
				}, new string[18]
				{
					"Point de passage d'Astorea", "Point de passage de la Spirale", "Point de passage de Caer Astorea", "Point de passage de la Crique des glaneurs", "Point de passage du Belvédère de Brigid", "Point de passage de Sperrins", "Point de passage de Mabon", "Point de passage de la Ville de Cathal", "Point de passage du Refuge de Caledon", "Point de passage de l'Escalier du titan",
					"Point de passage de Falias Thorp", "Point de passage du Hameau d'Annwen", "Point de passage du Refuge de Kraitban", "Point de passage du Marais de Wychmire", "Point de passage de Wardenhurst", "Point de passage du Dépôt de la Garde du Lion", "Point de passage de la Tonnelle du crépuscule", "Point de passage de Sleive"
				}),
				new MapEntry("Metrica Province", "Province de Metrica", new string[16]
				{
					"[&BEAAAAA=]", "[&BEEAAAA=]", "[&BEIAAAA=]", "[&BEMAAAA=]", "[&BEQAAAA=]", "[&BEUAAAA=]", "[&BEYAAAA=]", "[&BEcAAAA=]", "[&BEgAAAA=]", "[&BK4EAAA=]",
					"[&BK8EAAA=]", "[&BLAEAAA=]", "[&BLEEAAA=]", "[&BLIEAAA=]", "[&BLMEAAA=]", "[&BPcEAAA=]"
				}, new string[16]
				{
					"Soren Draa Waypoint", "Jeztar Falls Waypoint", "Akk Wilds Waypoint", "Rana Landing Complex Waypoint", "Arterium Haven Waypoint", "Hexane Regrade Waypoint", "Survivor's Encampment Waypoint", "Muridian Waypoint", "Desider Atum Waypoint", "Old Golem Factory Waypoint",
					"Loch Waypoint", "Anthill Waypoint", "Michotl Grounds Waypoint", "Cuatl Waypoint", "Artergon Waypoint", "Hydrone Unit Waypoint"
				}, new string[16]
				{
					"Point de passage de Soren Draa", "Point de passage des Chutes de Jeztar", "Point de passage des Contrées sauvages d'Akk", "Point de passage de la Zone d'amarrage Rana", "Point de passage du Refuge d'Arterium", "Point de passage du Dénivelé d'Hexane", "Point de passage du Campement des rescapés", "Point de passage muridienne", "Point de passage de Desider Atum", "Point de passage de la Vieille fonderie de golems",
					"Point de passage du Loch", "Point de passage de la Fourmilière", "Point de passage des Terres michotls", "Point de passage Cuatl", "Point de passage d'Artegon", "Point de passage de l'Unité Hydrone"
				}),
				new MapEntry("Brisban Wildlands", "Terres sauvages de Brisban", new string[13]
				{
					"[&BFwAAAA=]", "[&BF0AAAA=]", "[&BF4AAAA=]", "[&BF8AAAA=]", "[&BGAAAAA=]", "[&BGEAAAA=]", "[&BGIAAAA=]", "[&BGMAAAA=]", "[&BGQAAAA=]", "[&BGUAAAA=]",
					"[&BHUAAAA=]", "[&BHYAAAA=]", "[&BPkGAAA=]"
				}, new string[13]
				{
					"Watchful Source Waypoint", "Wendon Waypoint", "Tunnels Waypoint", "Hillstead Waypoint", "East End Waypoint", "Brilitine Waypoint", "Seraph Observers Waypoint", "Gallowfields Waypoint", "Triforge Point Waypoint", "Ulta Metamagicals Waypoint",
					"Mrot Boru Waypoint", "Mirkrise Waypoint", "Proxemics Lab Waypoint"
				}, new string[13]
				{
					"Point de passage de la Source de la Vigilance", "Point de passage de Wendon", "Point de passage des Tunnels", "Point de passage de la Colline", "Point de passage de l'Extrêmité Est", "Point de passage de Brilitine", "Point de passage des Observateurs séraphins", "Point de passage des Champs de la potence", "Point de passage de la Pointe de Triforge", "Point de passage de la Métamagique d'Ulta",
					"Point de passage de Mrot Boru", "Point de passage de Mirkrise", "Point de passage du Laboratoire de proxémie"
				}),
				new MapEntry("Sparkfly Fen", "Marais de Lumillule", new string[17]
				{
					"[&BMQBAAA=]", "[&BMUBAAA=]", "[&BMYBAAA=]", "[&BMcBAAA=]", "[&BMgBAAA=]", "[&BMkBAAA=]", "[&BMoBAAA=]", "[&BMsBAAA=]", "[&BMwBAAA=]", "[&BM0BAAA=]",
					"[&BM4BAAA=]", "[&BM8BAAA=]", "[&BNABAAA=]", "[&BNEBAAA=]", "[&BE0DAAA=]", "[&BLMDAAA=]", "[&BLQDAAA=]"
				}, new string[17]
				{
					"Orvar's Glen Waypoint", "Fort Cadence Waypoint", "Braggi's Stead Waypoint", "Saltflood Waypoint", "Zintl Holy Grounds Waypoint", "Ocean's Gullet Waypoint", "Forvar's Waypoint", "Toade's Head Waypoint", "Flamefrog Waypoint", "Darkweather Waypoint",
					"Brackwater Waypoint", "Aleem's Waypoint", "Splintered Coast Waypoint", "Brooloonu Waypoint", "Dryground Village Waypoint", "Caer Brier Waypoint", "Mire Waypoint"
				}, new string[17]
				{
					"Point de passage du Vallon d'Orvar", "Point de passage de Fort Cadence", "Point de passage de la Maison de Braggi", "Point de passage d'Inondesel", "Point de passage des Terres sacrées de Zintl", "Point de passage du Goulet de l'océan", "Point de passage de Forvar", "Point de passage de la Tête du crapaud", "Point de passage de Brûlereinette", "Point de passage du ciel lugubre",
					"Point de passage d'Ondesaline", "Point de passage d'Aleem", "Point de passage de la Côte effilochée", "Point de passage de Brooloonu", "Point de passage du Village de Tersech", "Point de passage de Caer Bruyère", "Point de passage du Bourbier"
				}),
				new MapEntry("Mount Maelstrom", "Mont Maelström", new string[17]
				{
					"[&BMcCAAA=]", "[&BMgCAAA=]", "[&BMkCAAA=]", "[&BMoCAAA=]", "[&BMsCAAA=]", "[&BMwCAAA=]", "[&BM0CAAA=]", "[&BM4CAAA=]", "[&BM8CAAA=]", "[&BNACAAA=]",
					"[&BNECAAA=]", "[&BNICAAA=]", "[&BNMCAAA=]", "[&BNQCAAA=]", "[&BNUCAAA=]", "[&BNYCAAA=]", "[&BEIFAAA=]"
				}, new string[17]
				{
					"Murkvale Waypoint", "Govoran Waypoint", "Criterion Waypoint", "Firebreak Fort Waypoint", "Spaecia Illogica Waypoint", "Bard's Waypoint", "Maelstrom's Waypoint", "Ashen Waypoint", "Avernan Waypoint", "Broken Arrow Waypoint",
					"Oxbow Isle Waypoint", "Irwin Isle Waypoint", "Gauntlet Waypoint", "Old Sledge Site Waypoint", "Judgment Waypoint", "Magmatic Waypoint", "Crucible of Eternity Waypoint"
				}, new string[17]
				{
					"Point de passage de Valbrouillard", "Point de passage de Govoran", "Point de passage de Criterion", "Point de passage de Fort Barfeu", "Point de passage de Spaecia Illogica", "Point de passage du Barde", "Point de passage de Maelström", "Point de passage des Cendres", "Point de passage d'Avernan", "Point de passage de la Flèche brisée",
					"Point de passage de l'Ile du Méandre", "Point de passage de l'Ile d'Irwin", "Point de passage du Gantelet", "Point de passage du Site du Vieux traîneau", "Point de passage du Jugement", "Point de passage magmatique", "Point de passage du Creuset de l'éternité"
				}),
				new MapEntry("Rata Sum", "Rata Sum", new string[9] { "[&BBMEAAA=]", "[&BBQEAAA=]", "[&BLQEAAA=]", "[&BLUEAAA=]", "[&BLYEAAA=]", "[&BLcEAAA=]", "[&BLgEAAA=]", "[&BLkEAAA=]", "[&BAcFAAA=]" }, new string[9] { "Metrical Court Waypoint", "Magustan Court Waypoint", "Magicat Court Waypoint", "Incubation Waypoint", "Accountancy Waypoint", "Apprentice Waypoint", "Research Waypoint", "Auxiliary Waypoint", "Port Waypoint" }, new string[9] { "Point de passage de la Cour métrique", "Point de passage de la Cour de Magustan", "Point de passage de la Cour magique", "Point de passage de l'incubateur", "Point de passage de la Comptabilité", "Point de passage des apprentis", "Point de passage de la recherche", "Point de passage auxiliaire", "Point de passage du port" }),
				new MapEntry("The Grove", "Le Bosquet", new string[4] { "[&BBIEAAA=]", "[&BLoEAAA=]", "[&BLsEAAA=]", "[&BLwEAAA=]" }, new string[4] { "Caledon Waypoint", "Upper Commons Waypoint", "Reckoner's Waypoint", "Ronan's Waypoint" }, new string[4] { "Point de passage de Caledon", "Point de passage de la Terrasse commune supérieure", "Point de passage du Spéculateur", "Point de passage de Ronan" })
			}),
			new RegionSection("Ruins of Orr", "Ruines d'Orr", new MapEntry[3]
			{
				new MapEntry("Straits of Devastation", "Détroit de la dévastation", new string[20]
				{
					"[&BOwCAAA=]", "[&BO0CAAA=]", "[&BO4CAAA=]", "[&BO8CAAA=]", "[&BPACAAA=]", "[&BPECAAA=]", "[&BPICAAA=]", "[&BPMCAAA=]", "[&BPQCAAA=]", "[&BPUCAAA=]",
					"[&BPYCAAA=]", "[&BPcCAAA=]", "[&BPgCAAA=]", "[&BPkCAAA=]", "[&BPoCAAA=]", "[&BPsCAAA=]", "[&BNIEAAA=]", "[&BFgGAAA=]", "[&BFkGAAA=]", "[&BOUGAAA=]"
				}, new string[20]
				{
					"Bramble Pass Waypoint", "Signal Peak Waypoint", "Fort Trinity Waypoint", "Thorn Pass Waypoint", "Thunderhead Waypoint", "Broken Spit Waypoint", "Vesper Bell Waypoint", "Royal Forum Waypoint", "Brassclaw Waypoint", "Antheneum Waypoint",
					"Conquest Marina Waypoint", "Lone Post Waypoint", "Waywarde Waypoint", "Xenarius Waypoint", "Glorious Victory Waypoint", "Sentry Waypoint", "Rally Waypoint", "Plinth Timberland Waypoint", "Underbelly Waypoint", "Dire Shoal Waypoint"
				}, new string[20]
				{
					"Point de passage du Passage ronceux", "Point de passage du Pic de signal", "Point de passage de Fort Trinité", "Point de passage de la Sente aux épines", "Point de passage de Tonnecaboche", "Point de passage de la Pointe brisée", "Point de passage de la Cloche vespérale", "Point de passage du Forum Royal", "Point de passage de Serrecuivre", "Point de passage de l'Atheneum",
					"Point de passage de la Marina de la Conquête", "Point de passage du Poste isolé", "Point de passage du rétif", "Point de passage de Xenarius", "Point de passage de la Glorieuse victoire", "Point de passage de la sentinelle", "Point de passage du Ralliement", "Point de passage des Terres forestières de Plinthe", "Point de passage de la Menace", "Point de passage des Bas-fonds funestes"
				}),
				new MapEntry("Malchor's Leap", "Saut de Malchor", new string[13]
				{
					"[&BKYCAAA=]", "[&BKcCAAA=]", "[&BKgCAAA=]", "[&BKkCAAA=]", "[&BKoCAAA=]", "[&BKsCAAA=]", "[&BKwCAAA=]", "[&BK0CAAA=]", "[&BK4CAAA=]", "[&BK8CAAA=]",
					"[&BLACAAA=]", "[&BLECAAA=]", "[&BLICAAA=]"
				}, new string[13]
				{
					"Pagga's Waypoint", "Doric's Waypoint", "Waste Hollows Waypoint", "Colonnade Waypoint", "Valley of Lyss Waypoint", "Union Waypoint", "Wren Waypoint", "Lyssa Waypoint", "Blighted Arch Waypoint", "Murmur Waypoint",
					"Tempest Waypoint", "Versoconjouring Waypoint", "Lights Waypoint"
				}, new string[13]
				{
					"Point de passage de Pagga", "Point de passage de Doric", "Point de passage des Crevasses oubliées", "Point de passage de la Colonnade", "Point de passage de la Vallée de Lyss", "Point de passage d'Union", "Point de passage de Wren", "Point de passage de Lyssa", "Point de passage de l'Arche corrompue", "Point de passage des Murmures",
					"Point de passage des Tempêtes", "Point de passage de Versoconjuration", "Point de passage des lumières"
				}),
				new MapEntry("Cursed Shore", "Rivage maudit", new string[13]
				{
					"[&BBcDAAA=]", "[&BBgDAAA=]", "[&BBkDAAA=]", "[&BBoDAAA=]", "[&BBsDAAA=]", "[&BBwDAAA=]", "[&BB0DAAA=]", "[&BB4DAAA=]", "[&BB8DAAA=]", "[&BCADAAA=]",
					"[&BCEDAAA=]", "[&BCIDAAA=]", "[&BOQGAAA=]"
				}, new string[13]
				{
					"Pursuit Pass Waypoint", "R&D Waypoint", "Penitent Waypoint", "Gavbeorn's Waypoint", "Verdance Waypoint", "Shelter's Gate Waypoint", "Jofast's Waypoint", "Meddler's Waypoint", "Anchorage Waypoint", "Arah Waypoint",
					"Caer Shadowfain Waypoint", "Murdered Dreams Waypoint", "Shipwreck Rock Waypoint"
				}, new string[13]
				{
					"Point de passage du Passage de la poursuite", "Point de passage R&D", "Point de passage du pénitent", "Point de passage de Gavbeorn", "Point de passage de Verdance", "Point de passage du Portail de l'Abri", "Point de passage de Jofast", "Point de passage de la Crête indiscrète", "Point de passage du Mouillage", "Point de passage d'Arah",
					"Point de passage de Caer Mande-Ombre", "Point de passage des rêves anéantis", "Point de passage du Rocher de l'épave"
				})
			}),
			new RegionSection("Shiverpeak Mountains", "Chaîne des Cimefroides", new MapEntry[7]
			{
				new MapEntry("Wayfarer Foothills", "Contreforts du voyageur", new string[17]
				{
					"[&BHIBAAA=]", "[&BHMBAAA=]", "[&BHQBAAA=]", "[&BHUBAAA=]", "[&BHYBAAA=]", "[&BHcBAAA=]", "[&BHgBAAA=]", "[&BHkBAAA=]", "[&BHoBAAA=]", "[&BHsBAAA=]",
					"[&BHwBAAA=]", "[&BH0BAAA=]", "[&BH4BAAA=]", "[&BMEDAAA=]", "[&BMIDAAA=]", "[&BMMDAAA=]", "[&BAEEAAA=]"
				}, new string[17]
				{
					"Outcast's Waypoint", "Hero's Moot Waypoint", "Horncall Waypoint", "Darkriven Waypoint", "Taigan Waypoint", "Zelechor Hot Springs Waypoint", "Halvaunt Waypoint", "Crossroads Haven Waypoint", "Dawnrise Waypoint", "Dolyak Pass Waypoint",
					"Solitude Waypoint", "Twinspur Haven Waypoint", "Vendrake's Homestead Waypoint", "Lostvyrm Cave Waypoint", "Krennak's Homestead Waypoint", "Grawlenfjord Waypoint", "Osenfold Waypoint"
				}, new string[17]
				{
					"Point de passage du Paria", "Point de passage du Festibière du Héros", "Point de passage d'Appelcor", "Point de passage d'Ecorchenuit", "Point de passage de Taigan", "Point de passage des Sources chaudes de Zelechor", "Point de passage d'Halvaunt", "Point de passage du Refuge du croisement", "Point de passage de l'aube", "Point de passage du Col du dolyak",
					"Point de passage de la solitude", "Point de passage du Refuge de Doubléperon", "Point de passage de la Colonie de Vendrake", "Point de passage de la Caverne de Pertevyrm", "Point de passage de la Colonie de Krennak", "Point de passage du Fjord des Grawls", "Point de passage d'Osenfold"
				}),
				new MapEntry("Snowden Drifts", "Congères d'Antreneige", new string[19]
				{
					"[&BLMAAAA=]", "[&BLQAAAA=]", "[&BLUAAAA=]", "[&BLYAAAA=]", "[&BLcAAAA=]", "[&BLgAAAA=]", "[&BLkAAAA=]", "[&BLoAAAA=]", "[&BLsAAAA=]", "[&BLwAAAA=]",
					"[&BL0AAAA=]", "[&BL4AAAA=]", "[&BL8AAAA=]", "[&BMAAAAA=]", "[&BMEAAAA=]", "[&BMIAAAA=]", "[&BL8DAAA=]", "[&BMADAAA=]", "[&BP8GAAA=]"
				}, new string[19]
				{
					"Skradden Waypoint", "Lornar's Waypoint", "Highpass Haven Waypoint", "Lost Child's Sorrow Waypoint", "Scholar's Cleft Waypoint", "Isenfall Waypoint", "Snowdrift Haven Waypoint", "Seraph Outriders' Waypoint", "Torstvedt Homestead Waypoint", "Exile Waypoint",
					"Njordstead Waypoint", "Soderhem Steading Waypoint", "Snowhawk Landing Waypoint", "Reaver's Waypoint", "Owl Waypoint", "Podaga Steading Waypoint", "Frozen Sweeps Waypoint", "Valslake Waypoint", "Angvar's Trove Waypoint"
				}, new string[19]
				{
					"Point de passage de Skradden", "Point de passage de Lornar", "Point de passage du Refuge de la Haute-passe", "Point de passage du Chagrin de l'enfant perdu", "Point de passage de la Faille de l'érudit", "Point de passage d'Isenfall", "Point de passage du Refuge de la Congère", "Point de passage des Précurseurs séraphins", "Point de passage de la Colonie de Torstvedt", "Point de passage de l'Exil",
					"Point de passage de Njorstead", "Point de passage de la Maison de Soderhem", "Point de passage de la Terrasse du Faucon des neiges", "Point de passage du Faucheur", "Point de passage de la Chouette", "Point de passage de la Maison Podaga", "Point de passage des Étendues gelées", "Point de passage de Valselac", "Point de passage de la Cache d'Angvar"
				}),
				new MapEntry("Lornar's Pass", "Passage de Lornar", new string[17]
				{
					"[&BOUAAAA=]", "[&BOYAAAA=]", "[&BOcAAAA=]", "[&BOgAAAA=]", "[&BOkAAAA=]", "[&BOoAAAA=]", "[&BOsAAAA=]", "[&BOwAAAA=]", "[&BJYBAAA=]", "[&BJcBAAA=]",
					"[&BJgBAAA=]", "[&BJkBAAA=]", "[&BFEGAAA=]", "[&BFIGAAA=]", "[&BFMGAAA=]", "[&BFQGAAA=]", "[&BDEHAAA=]"
				}, new string[17]
				{
					"Vanjir's Stead Waypoint", "Demon's Maw Waypoint", "Winterthaw Waypoint", "False Lake Waypoint", "Durmand Priory Waypoint", "Lamentation Waypoint", "Thunderhorns Waypoint", "Nentor Waypoint", "Guutra's Homestead Waypoint", "Stonescatter Waypoint",
					"Pinnacle Enclave Waypoint", "Mistriven Waypoint", "Icedevil's Waypoint", "Refuge Peak Waypoint", "Afgar's Waypoint", "Cascade Bridge Waypoint", "False River Waypoint"
				}, new string[17]
				{
					"Point de passage de la Maison de Vanjir", "Point de passage de la Gueule du démon", "Point de passage du dégel", "Point de passage du Lac trompeur", "Point de passage du Prieuré de Durmand", "Point de passage de la Complainte", "Point de passage des Cornes foudroyantes", "Point de passage de Nentor", "Point de passage de la Colonie de Guutra", "Point de passage de Sèmeroche",
					"Point de passage de l'Enclave du Pinacle", "Point de passage de Mistriven", "Point de passage du Démon des glaces", "Point de passage du Pic du refuge", "Point de passage d'Afgar", "Point de passage du Pont de la Cascade", "Point de passage du fleuve trompeur"
				}),
				new MapEntry("Dredgehaunt Cliffs", "Falaises de Hantedraguerre", new string[18]
				{
					"[&BFYCAAA=]", "[&BFcCAAA=]", "[&BFgCAAA=]", "[&BFkCAAA=]", "[&BFoCAAA=]", "[&BFsCAAA=]", "[&BFwCAAA=]", "[&BF0CAAA=]", "[&BF4CAAA=]", "[&BF8CAAA=]",
					"[&BGACAAA=]", "[&BGECAAA=]", "[&BGICAAA=]", "[&BGMCAAA=]", "[&BGQCAAA=]", "[&BGUCAAA=]", "[&BO4EAAA=]", "[&BD8FAAA=]"
				}, new string[18]
				{
					"Tribulation Waypoint", "Frostland Waypoint", "Dociu Waypoint", "Seven Pines Waypoint", "Grey Road Waypoint", "Steelbrachen Waypoint", "Toran Hollow Waypoint", "Hessdallen Kenning Waypoint", "Kenning Testing Ground Waypoint", "Wide Expanse Waypoint",
					"Nottowr Fault Waypoint", "Mountain's Tail Waypoint", "Graupel Waypoint", "Havfrue Basin Waypoint", "Travelen's Waypoint", "Wyrmblood Waypoint", "Granite Citadel Waypoint", "Sorrow's Embrace Waypoint"
				}, new string[18]
				{
					"Point de passage de la tribulation", "Point de passage des Terres gelées", "Point de passage de Dociu", "Point de passage des Sept Pins", "Point de passage de la Route grise", "Point de passage de Steelbrachen", "Point de passage de la Crevasse de Toran", "Point de passage du Rassemblement d'Hessdallen", "Point de passage du Terrain d'entraînement du Belvédère", "Point de passage de la Grande étendue",
					"Point de passage de la Faille de Nouzottr", "Point de passage de la Queue de la montagne", "Point de passage de Graupel", "Point de passage du Bassin d'Havfrue", "Point de passage de Travelen", "Point de passage de Wyrmsang", "Point de passage de la Citadelle de granite", "Point de passage de l'Étreinte des lamentations"
				}),
				new MapEntry("Timberline Falls", "Chutes de la canopée", new string[19]
				{
					"[&BEQCAAA=]", "[&BEUCAAA=]", "[&BEYCAAA=]", "[&BEcCAAA=]", "[&BEgCAAA=]", "[&BEkCAAA=]", "[&BEoCAAA=]", "[&BEsCAAA=]", "[&BEwCAAA=]", "[&BE0CAAA=]",
					"[&BE4CAAA=]", "[&BE8CAAA=]", "[&BFACAAA=]", "[&BFECAAA=]", "[&BFICAAA=]", "[&BFMCAAA=]", "[&BFQCAAA=]", "[&BFUCAAA=]", "[&BEYEAAA=]"
				}, new string[19]
				{
					"Talus Waypoint", "Serpent Waypoint", "Okarinoo Waypoint", "Coil Waypoint", "Scale Strand Waypoint", "Nonmoa Waypoint", "Eztlitl Grounds Waypoint", "Gyre Rapids Waypoint", "Valance Tutory Waypoint", "Ogduk Waypoint",
					"Foundation 86 Waypoint", "White Paper Waypoint", "Gentle River Waypoint", "Thistlereed Waypoint", "Krongar Waypoint", "Iron Veil Waypoint", "Rankor Ruins Waypoint", "Concordia Waypoint", "Stromkarl Waypoint"
				}, new string[19]
				{
					"Point de passage de Talus", "Point de passage du Serpent", "Point de passage d'Okarinoo", "Point de passage de l'anneau", "Point de passage de Fil d'Écaille", "Point de passage de Nonmoa", "Point de passage des Terres Eztlitl", "Point de passage des Rapides de Gyre", "Point de passage du Tutorat de Valance", "Point de passage d'Ogduk",
					"Point de passage de la Fondation 86", "Point de passage de Feuilleblanche", "Point de passage de la Rivière tranquille", "Point de passage de Piqueroseau", "Point de passage de Krongar", "Point de passage du Voile de fer", "Point de passage des Ruines de Rankor", "Point de passage de Concordia", "Point de passage de Stromkarl"
				}),
				new MapEntry("Frostgorge Sound", "Détroit des gorges glacées", new string[16]
				{
					"[&BHgCAAA=]", "[&BHkCAAA=]", "[&BHoCAAA=]", "[&BHsCAAA=]", "[&BHwCAAA=]", "[&BH0CAAA=]", "[&BH4CAAA=]", "[&BH8CAAA=]", "[&BIACAAA=]", "[&BIECAAA=]",
					"[&BIICAAA=]", "[&BIMCAAA=]", "[&BIQCAAA=]", "[&BIUCAAA=]", "[&BIYCAAA=]", "[&BEMFAAA=]"
				}, new string[16]
				{
					"Arundon Waypoint", "Groznev Waypoint", "Earthshake Waypoint", "Path of Starry Skies Waypoint", "Slough of Despond Waypoint", "Watchful Waypoint", "Ice Floe Waypoint", "Dimotiki Waypoint", "Twoloop Waypoint", "Skyheight Steading Waypoint",
					"Highpeaks Waypoint", "Ridgerock Camp Waypoint", "Yak's Bend Waypoint", "Blue Ice Shining Waypoint", "Drakkar Waypoint", "Honor of the Waves Waypoint"
				}, new string[16]
				{
					"Point de passage d'Arundon", "Point de passage de Groznev", "Point de passage de Trembleterre", "Point de passage du Sentier des Cieux étoilés", "Point de passage du Bourbier de la Mélancolie", "Point de passage de la Vigilance", "Point de passage de la Banquise", "Point de passage de Dimotiki", "Point de passage de Doubleboucle", "Point de passage de la Maison de Hautecieux",
					"Point de passage des Pics glacés", "Point de passage du Camp de Rochecrête", "Point de passage de la Courbe du Yak", "Point de passage de la Lumière froide", "Point de passage de Drakkar", "Point de passage d'Honneur des vagues"
				}),
				new MapEntry("Hoelbrak", "Hoelbrak", new string[14]
				{
					"[&BIUDAAA=]", "[&BIYDAAA=]", "[&BIcDAAA=]", "[&BIgDAAA=]", "[&BIkDAAA=]", "[&BIoDAAA=]", "[&BIsDAAA=]", "[&BIwDAAA=]", "[&BI0DAAA=]", "[&BI4DAAA=]",
					"[&BI8DAAA=]", "[&BJADAAA=]", "[&BNUDAAA=]", "[&BAgFAAA=]"
				}, new string[14]
				{
					"Might and Main Waypoint", "Trade Commons Waypoint", "Snow Leopard Waypoint", "Raven Waypoint", "Bear Waypoint", "Wolf Waypoint", "Shelter Rock Waypoint", "Legends Waypoint", "Eastern Watchpost Waypoint", "Southern Watchpost Waypoint",
					"Peeta's Waypoint", "Hero's Compass Waypoint", "Great Lodge Waypoint", "Upper Balcony Waypoint"
				}, new string[14]
				{
					"Point de passage d'Agora", "Point de passage du Forum du commerce", "Point de passage de la Panthère des neiges", "Point de passage du Corbeau", "Point de passage de l'Ourse", "Point de passage du Loup", "Point de passage du Rocher de l'abri", "Point de passage des Légendes", "Point de passage du Poste d'observation est", "Point de passage du Poste d'observation sud",
					"Point de passage de Peeta", "Point de passage de la Boussole du Héros", "Point de passage de la Grand-loge", "Point de passage du Balcon supérieur"
				})
			})
		};

		public static readonly MapEntry[] HeartOfThorns = new MapEntry[4]
		{
			new MapEntry("Verdant Brink", "Orée d'émeraude", new string[7] { "[&BNUHAAA=]", "[&BN4HAAA=]", "[&BOAHAAA=]", "[&BO8HAAA=]", "[&BAEIAAA=]", "[&BAgIAAA=]", "[&BBwIAAA=]" }, new string[7] { "Mellaggan's Valor Waypoint", "Shipwreck Peak Waypoint", "Jaka Itzel Waypoint", "Faren's Flyer Waypoint", "Shrouded Ruins Waypoint", "Pact Encampment Waypoint", "Treacherous Path Waypoint" }, new string[7] { "Point de passage de la Bravoure de Mellaggan", "Point de passage du Pic de l'épave", "Point de passage de Jaka Itzel", "Point de passage de l'Envol de Faren", "Point de passage des Ruines sibyllines", "Point de passage du Campement du Pacte", "Point de passage du Sentier tortueux" }),
			new MapEntry("Auric Basin", "Bassin aurique", new string[7] { "[&BMYHAAA=]", "[&BNYHAAA=]", "[&BN0HAAA=]", "[&BAIIAAA=]", "[&BAYIAAA=]", "[&BEkIAAA=]", "[&BGwIAAA=]" }, new string[7] { "Forgotten City Waypoint", "Wanderer's Waypoint", "Northwatch Waypoint", "Southwatch Waypoint", "Westwatch Waypoint", "Chak Hollow Waypoint", "Eastwatch Waypoint" }, new string[7] { "Point de passage de la Cité des Oubliés", "Point de passage du voyageur", "Point de passage de la Garde du Nord", "Point de passage de la Garde du Sud", "Point de passage de la Garde de l'Ouest", "Point de passage du Vallon du chak", "Point de passage de la Garde de l'Est" }),
			new MapEntry("Tangled Depths", "Profondeurs verdoyantes", new string[7] { "[&BMwHAAA=]", "[&BPUHAAA=]", "[&BAAIAAA=]", "[&BAMIAAA=]", "[&BAwIAAA=]", "[&BA4IAAA=]", "[&BIgIAAA=]" }, new string[7] { "Ogre Camp Waypoint", "Ley-Line Confluence Waypoint", "SCAR Camp Waypoint", "Rata Novus Waypoint", "Teku Nuhoch Waypoint", "Order of Whispers Camp Waypoint", "Dragon's Passage Waypoint" }, new string[7] { "Point de passage du Camp ogre", "Point de passage de la Confluence des lignes de force", "Point de passage du Camp du B.R.U.T.A.L.", "Point de passage de Rata Novus", "Point de passage de Teku Nuhoch", "Point de passage du Camp de l'Ordre des Soupirs", "Point de passage de la Traverse du dragon" }),
			new MapEntry("Dragon's Stand", "Repli du dragon", new string[11]
			{
				"[&BBAIAAA=]", "[&BC4IAAA=]", "[&BE8IAAA=]", "[&BFIIAAA=]", "[&BFMIAAA=]", "[&BFcIAAA=]", "[&BGUIAAA=]", "[&BHoIAAA=]", "[&BJcIAAA=]", "[&BK4IAAA=]",
				"[&BAAJAAA=]"
			}, new string[11]
			{
				"Pact Base Camp Waypoint", "Central Forward Camp Waypoint", "Southern Forward Camp Waypoint", "Northern Forward Camp Waypoint", "Central Advance Camp Waypoint", "Dragon's Domain Southern Waypoint", "Southern Advance Camp Waypoint", "Pact Base Camp Southern Waypoint", "Dragon's Domain Central Waypoint", "Dragon's Domain Northern Waypoint",
				"Northern Advance Camp Waypoint"
			}, new string[11]
			{
				"Point de passage du Campement de base du Pacte", "Point de passage du Camp avant central", "Point de passage du Camp avant sud", "Point de passage du Camp avant nord", "Point de passage du Camp avancé central", "Point de passage sud du Domaine du dragon", "Point de passage du Camp avancé sud", "Point de passage sud du Campement de base du Pacte", "Point de passage central du Domaine du dragon", "Point de passage nord du Domaine du dragon",
				"Point de passage du Camp avancé nord"
			})
		};

		public static readonly MapEntry[] PathOfFire = new MapEntry[5]
		{
			new MapEntry("Crystal Oasis", "Oasis de cristal", new string[3] { "[&BEAKAAA=]", "[&BJMKAAA=]", "[&BLsKAAA=]" }, new string[3] { "Temple of Kormir Waypoint", "Destiny's Gorge Waypoint", "Amnoon Waypoint" }, new string[3] { "Point de passage du Temple de Kormir", "Point de passage de la gorge de la destinée", "Point de passage d'Amnoon" }),
			new MapEntry("Desert Highlands", "Hautes-terres du désert", new string[5] { "[&BGoKAAA=]", "[&BGsKAAA=]", "[&BJEKAAA=]", "[&BJ0KAAA=]", "[&BKQKAAA=]" }, new string[5] { "Lommuld Kraal Waypoint", "Makali Outpost Waypoint", "Brightwater Waypoint", "Highjump Ranch Waypoint", "Derelict Delve Waypoint" }, new string[5] { "Point de passage du Kraal de Lommuld", "Point de passage de l'avant-poste de Makali", "Point de passage d'Eauvive", "Point de passage du ranch de Longsaut", "Point de passage de l'antre abandonné" }),
			new MapEntry("Elon Riverlands", "Rives de l'Elon", new string[3] { "[&BCgKAAA=]", "[&BFMKAAA=]", "[&BGcKAAA=]" }, new string[3] { "Olishar's Oasis Camp Waypoint", "Augury's Shadow Waypoint", "Skimmer Ranch Waypoint" }, new string[3] { "Point de passage du camp de l'oasis d'Olishar", "Point de passage de l'Ombre de l'augure", "Point de passage du ranch des voldécumes" }),
			new MapEntry("The Desolation", "La Désolation", new string[6] { "[&BAsKAAA=]", "[&BHMKAAA=]", "[&BHcKAAA=]", "[&BKMKAAA=]", "[&BMEKAAA=]", "[&BNwKAAA=]" }, new string[6] { "Joko's Domain Waypoint", "The Scourgeway Waypoint", "Sand Jackal Run Waypoint", "The Darklands Waypoint", "Lair of the Forgotten Waypoint", "Bonestrand Waypoint" }, new string[6] { "Point de passage du Domaine de Joko", "Point de passage de la voie du fléau", "Point de passage de la piste des chacals des sables", "Point de passage des terres noires", "Point de passage du repaire des Oubliés", "Point de passage de l'ossuaire" }),
			new MapEntry("Domain of Vabbi", "Domaine de Vabbi", new string[6] { "[&BA8KAAA=]", "[&BEoKAAA=]", "[&BHQKAAA=]", "[&BKUKAAA=]", "[&BNAKAAA=]", "[&BO0KAAA=]" }, new string[6] { "Vehtendi Academy Waypoint", "Necropolis Waypoint", "Market Ruins Waypoint", "Cragged Vale Waypoint", "Seborhin Waypoint", "Vehjin Palace Waypoint" }, new string[6] { "Point de passage de l'académie des Vehtendi", "Point de passage de la nécropole", "Point de passage des ruines du marché", "Point de passage du vallon escarpé", "Point de passage de Seborhin", "Point de passage du palais de Vehjin" })
		};

		public static readonly MapEntry[] EndOfDragons = new MapEntry[5]
		{
			new MapEntry("Seitung Province", "Province de Seitung", new string[4] { "[&BJ4MAAA=]", "[&BL8MAAA=]", "[&BGQNAAA=]", "[&BGUNAAA=]" }, new string[4] { "Village Waypoint", "Monastery Waypoint", "Haiju Docks Waypoint", "Daigo Ward Waypoint" }, new string[4] { "Point de passage du village", "Point de passage du monastère", "Point de passage des Quais de Haiju", "Point de passage du Quartier Daigo" }),
			new MapEntry("New Kaineng City", "Néo-Kaineng", new string[4] { "[&BMYMAAA=]", "[&BBYNAAA=]", "[&BBkNAAA=]", "[&BCANAAA=]" }, new string[4] { "Promenade Waypoint", "Garden Heights Waypoint", "Ministry Ward Waypoint", "Lutgardis Plaza Waypoint" }, new string[4] { "Point de passage de la promenade", "Point de passage des Hauts jardins", "Point de passage du quartier des ministères", "Point de passage de la Place de Lutgardis" }),
			new MapEntry("The Echovald Wilds", "Terres sauvages d'Echovald", new string[6] { "[&BIsMAAA=]", "[&BMcMAAA=]", "[&BMoMAAA=]", "[&BMwMAAA=]", "[&BNQMAAA=]", "[&BPkMAAA=]" }, new string[6] { "Junkyard Waypoint", "Mori Village Waypoint", "Waypoint zu Heltzer", "Kropa Waypoint", "Qinkai Waypoint", "Jade Gate Waypoint" }, new string[6] { "Point de passage de la décharge", "Point de passage du village de Mori", "Point de passage zu Heltzer", "Point de passage de Kropa", "Point de passage de Qinkai", "Point de passage de la Porte de jade" }),
			new MapEntry("Dragon's End", "Trépas du dragon", new string[5] { "[&BKAMAAA=]", "[&BKIMAAA=]", "[&BPEMAAA=]", "[&BBENAAA=]", "[&BDsNAAA=]" }, new string[5] { "Harvest Complex Waypoint", "Jade Quarry Waypoint", "Frozen Sea Waypoint", "Brotherhood Waypoint", "Speakers Waypoint" }, new string[5] { "Point de passage du Complexe des moissons", "Point de passage de la Carrière de Jade", "Point de passage de la Mer gelée", "Point de passage de la Confrérie", "Point de passage des Orateurs" }),
			new MapEntry("Gyala Delve", "Fosse de Gyala", new string[4] { "[&BJINAAA=]", "[&BJkNAAA=]", "[&BKQNAAA=]", "[&BKoNAAA=]" }, new string[4] { "Behemoth's Gap Waypoint", "Luxon Terminus Waypoint", "Jadepillar Point Waypoint", "Park Mining Ops Waypoint" }, new string[4] { "Point de passage du Ravin du béhémoth", "Point de passage de Terminus luxon", "Point de passage de la Position du pilier de jade", "Point de passage de l'Exploitation minière Park" })
		};

		public static readonly MapEntry[] SecretsOfTheObscure = new MapEntry[3]
		{
			new MapEntry("Skywatch Archipelago", "Archipel de l'observatoire céleste", new string[7] { "[&BL4NAAA=]", "[&BOANAAA=]", "[&BA4OAAA=]", "[&BBUOAAA=]", "[&BCsOAAA=]", "[&BD8OAAA=]", "[&BEYOAAA=]" }, new string[7] { "Droknar's Light Waypoint", "Rata Novus Promenade Waypoint", "Astral Ward Encampment Waypoint", "Garenhoff Refugee Camp Waypoint", "Beacon of Ages Waypoint", "Kestrel's Vow Waypoint", "Observation Camp Waypoint" }, new string[7] { "Point de passage de la Lumière de Droknar", "Point de passage de la promenade de Rata Novus", "Point de passage du campement de la Garde astrale", "Point de passage du camp des réfugiés de Garenhoff", "Point de passage du Phare des Âges", "Point de passage du Serment des Kestrels", "Point de passage du camp d'observation" }),
			new MapEntry("Amnytas", "Amnytas", new string[6] { "[&BNMNAAA=]", "[&BAoOAAA=]", "[&BDQOAAA=]", "[&BD0OAAA=]", "[&BEIOAAA=]", "[&BEcOAAA=]" }, new string[6] { "Bastion of Strength Waypoint", "Bastion of the Celestial Waypoint", "Bastion of the Natural Waypoint", "Bastion of Balance Waypoint", "Bastion of Knowledge Waypoint", "Bastion of the Obscure Waypoint" }, new string[6] { "Point de passage du Bastion de la Force", "Point de passage du Bastion de l'Avatar céleste", "Point de passage du Bastion du Naturel", "Point de passage du Bastion de l'Équilibre", "Point de passage du Bastion du Savoir", "Point de passage du Bastion de l'Obscur" }),
			new MapEntry("Inner Nayos", "Nayos intérieur", new string[2] { "[&BGYOAAA=]", "[&BHYOAAA=]" }, new string[2] { "Astral Ward Camp Waypoint", "Forward Bivouac Waypoint" }, new string[2] { "Point de passage du Camp de la Garde astrale", "Point de passage du bivouac avancé" })
		};

		public static readonly MapEntry[] JanthirWilds = new MapEntry[4]
		{
			new MapEntry("Lowland Shore", "Côte des basses terres", new string[6] { "[&BK4OAAA=]", "[&BMkOAAA=]", "[&BN8OAAA=]", "[&BPsOAAA=]", "[&BCcPAAA=]", "[&BC4PAAA=]" }, new string[6] { "Harvest Den Waypoint", "Hot Springs Waypoint", "Harvest Shore Waypoint", "Journeykin Outpost Waypoint", "Astral Ward Moon Camp Waypoint", "Autumn's Vale Waypoint" }, new string[6] { "Point de passage du Repaire des moissons", "Point de passage des sources chaudes", "Point de passage de la Côte des moissons", "Point de passage de l'avant-poste des trottetins", "Point de passage du Camp de la lune de la Garde astrale", "Point de passage du Val d'automne" }),
			new MapEntry("Janthir Syntri", "Syntri de Janthir", new string[4] { "[&BLcOAAA=]", "[&BLgOAAA=]", "[&BCoPAAA=]", "[&BCwPAAA=]" }, new string[4] { "Forager's Hunt Waypoint", "Festering Basin Waypoint", "Stricken Plains Waypoint", "Sanguine Crater Waypoint" }, new string[4] { "Point de passage de la Halte de chasse du cueilleur", "Point de passage du Bassin putride", "Point de passage des Plaines affligées", "Point de passage du Cratère sanguin" }),
			new MapEntry("Mistburned Barrens", "Landes de Feu-de-Brume", new string[1] { "[&BFAPAAA=]" }, new string[1] { "Alliance Staging Ground" }, new string[1] { "Base opérationnelle de l'alliance" }),
			new MapEntry("Bava Nisos", "Bava Nisos", new string[2] { "[&BGEPAAA=]", "[&BG0PAAA=]" }, new string[2] { "Mantle's Arrival Waypoint", "Hidden Nodule Waypoint" }, new string[2] { "Point de passage de l'arrivage du Manteau", "Point de passage du nodule caché" })
		};

		public static readonly MapEntry[] VisionsOfEternity = new MapEntry[3]
		{
			new MapEntry("Shipwreck Strand", "Rive aux épaves", new string[3] { "[&BJEPAAA=]", "[&BJwPAAA=]", "[&BMwPAAA=]" }, new string[3] { "Pub Canach Waypoint", "Dark Leviathan Waypoint", "Hullgarden Pier Waypoint" }, new string[3] { "Point de passage du Pub Canach", "Point de passage de l'obscur Léviathan", "Point de passage du quai de Goussardin" }),
			new MapEntry("Starlit Weald", "Bois étoilé", new string[3] { "[&BIoPAAA=]", "[&BJ4PAAA=]", "[&BMAPAAA=]" }, new string[3] { "Hunter's Camp Waypoint", "Priory Expedition Waypoint", "Foothold Bivouac Waypoint" }, new string[3] { "Point de passage du camp de chasse", "Point de passage de l'expédition du Prieuré", "Point de passage du bivouac de base" }),
			new MapEntry("Eternity's Garden", "Jardin de l'éternité", new string[4] { "[&BOQPAAA=]", "[&BPAPAAA=]", "[&BPoPAAA=]", "[&BPwPAAA=]" }, new string[4] { "Deepwood Outpost Waypoint", "Sanctum Exterior Waypoint", "Temple of Abnegation Waypoint", "Pilgrim's Rest Waypoint" }, new string[4] { "Point de passage de l'avant-poste du Bois profond", "Point de passage du Sanctuaire extérieur", "Point de passage du Temple de l'abnégation", "Point de passage du Repos du pèlerin" })
		};

		public static readonly LwSeason[] LivingWorld = new LwSeason[5]
		{
			new LwSeason(1, new MapEntry[1]
			{
				new MapEntry("Southsun Cove", "Crique de Sud-Soleil", new string[6] { "[&BNAGAAA=]", "[&BNIGAAA=]", "[&BNUGAAA=]", "[&BNcGAAA=]", "[&BNgGAAA=]", "[&BNwGAAA=]" }, new string[6] { "Lion Point Waypoint", "Pride Point Waypoint", "Pearl Islet Waypoint", "Camp Karka Waypoint", "Owain's Refuge Waypoint", "Kiel's Outpost Waypoint" }, new string[6] { "Point de passage du Point du Lion", "Point de passage du Point de la Fierté", "Point de passage de l'Îlot de perles", "Point de passage de Camp karka", "Point de passage du Refuge d'Owain", "Point de passage de l'avant-poste de Kiel" })
			}),
			new LwSeason(2, new MapEntry[2]
			{
				new MapEntry("Dry Top", "Cimesèche", new string[5] { "[&BHoHAAA=]", "[&BIAHAAA=]", "[&BIYHAAA=]", "[&BIgHAAA=]", "[&BJcHAAA=]" }, new string[5] { "Prosperity Waypoint", "Dry Top Entry Waypoint", "Vine Bridge Waypoint", "Restoration Refuge Waypoint", "Repair Station Waypoint" }, new string[5] { "Point de passage de Prospérité", "Point de passage d'entrée à la Cimesèche", "Point de passage du pont de lianes", "Point de passage du refuge de restauration", "Point de passage de l'atelier de réparation" }),
				new MapEntry("The Silverwastes", "Les Contrées sauvages d'argent", new string[3] { "[&BH8HAAA=]", "[&BKwHAAA=]", "[&BLoHAAA=]" }, new string[3] { "Camp Resolve Waypoint", "Hidden Depths Waypoint", "Drydock Grotto Waypoint" }, new string[3] { "Point de passage du Camp de la détermination", "Point de passage des profondeurs silencieuses", "Point de passage de la grotte de Quai-sec" })
			}),
			new LwSeason(3, new MapEntry[6]
			{
				new MapEntry("Bloodstone Fen", "Marais de la pierre de sang", new string[4] { "[&BD8JAAA=]", "[&BEEJAAA=]", "[&BEsJAAA=]", "[&BE4JAAA=]" }, new string[4] { "Depths of the Maw Waypoint", "Ground Zero Waypoint", "Soulkeeper's Airship Waypoint", "Zealot's Overlook Waypoint" }, new string[4] { "Point de passage des Profondeurs du Gouffre", "Point de passage du niveau zéro", "Point de passage de l'aéronef du Garde-âme", "Point de passage du Belvédère du zélote" }),
				new MapEntry("Ember Bay", "Baie des braises", new string[4] { "[&BFUJAAA=]", "[&BF8JAAA=]", "[&BGAJAAA=]", "[&BHgJAAA=]" }, new string[4] { "Scratch Gate Waypoint", "Promontory Waypoint", "Crumbling Trail Waypoint", "Castaway Circus Waypoint" }, new string[4] { "Point de passage du Portail de Griffure", "Point de passage du Promontoire", "Point de passage de la Piste escarpée", "Point de passage du Cirque des naufragés" }),
				new MapEntry("Bitterfrost Frontier", "Confins de Givramer", new string[2] { "[&BH0JAAA=]", "[&BIEJAAA=]" }, new string[2] { "Sorrow's Eclipse Waypoint", "Koda's Welcome Waypoint" }, new string[2] { "Point de passage de l'Éclipse des lamentations", "Point de passage de l'Accueil de Koda" }),
				new MapEntry("Lake Doric", "Lac Doric", new string[3] { "[&BK0JAAA=]", "[&BK8JAAA=]", "[&BLQJAAA=]" }, new string[3] { "Doric's Landing Waypoint", "Red Leaf Retreat Waypoint", "Lakeside Bazaar Waypoint" }, new string[3] { "Point de passage du Débarcadère de Doric", "Point de passage de la retraite de la feuille rouge", "Point de passage du Bazar lacustre" }),
				new MapEntry("Draconis Mons", "Mont Draconis", new string[3] { "[&BL0JAAA=]", "[&BM0JAAA=]", "[&BNUJAAA=]" }, new string[3] { "Mariner Landing Waypoint", "Heathen's Hold Waypoint", "Ancient Hollow Waypoint" }, new string[3] { "Point de passage du Débarcadère du marin", "Point de passage du Fort du païen", "Point de passage de la Grotte ancienne" }),
				new MapEntry("Siren's Landing", "Plage des sirènes", new string[1] { "[&BO8JAAA=]" }, new string[1] { "Camp Reclamation Waypoint" }, new string[1] { "Point de passage de Camp Reconquête" })
			}),
			new LwSeason(4, new MapEntry[6]
			{
				new MapEntry("Domain of Istan", "Domaine d'Istan", new string[3] { "[&BPoKAAA=]", "[&BAkLAAA=]", "[&BBoLAAA=]" }, new string[3] { "Astralarium Waypoint", "Chalon Docks Waypoint", "Champion's Dawn Waypoint" }, new string[3] { "Point de passage de l'Astralarium", "Point de passage des Quais de Chalon", "Point de passage de l'Aube du Champion" }),
				new MapEntry("Sandswept Isles", "Îles de Ventesable", new string[2] { "[&BCULAAA=]", "[&BEMLAAA=]" }, new string[2] { "Anniogel Encampment Waypoint", "Atholma Waypoint" }, new string[2] { "Point de passage du campement d'Anniogel", "Point de passage d'Atholma" }),
				new MapEntry("Domain of Kourna", "Domaine de Kourna", new string[2] { "[&BFALAAA=]", "[&BFcLAAA=]" }, new string[2] { "Apizmic Grounds Waypoint", "Allied Encampment Waypoint" }, new string[2] { "Point de passage des Terres apizmiques", "Point de passage du campement allié" }),
				new MapEntry("Jahai Bluffs", "Promontoire de Jahai", new string[4] { "[&BH4LAAA=]", "[&BIMLAAA=]", "[&BJMLAAA=]", "[&BJkLAAA=]" }, new string[4] { "Yatendi Village Waypoint", "Soul's Vendetta Waypoint", "Venta Pass Waypoint", "Reclaimed Chantry Waypoint" }, new string[4] { "Point de passage du village de Yatendi", "Point de passage de la Vendetta de l'âme", "Point de passage du Passage de Venta", "Point de passage de la chapelle récupérée" }),
				new MapEntry("Thunderhead Peaks", "Pics de Chef-Tonnerre", new string[4] { "[&BKYLAAA=]", "[&BLkLAAA=]", "[&BLoLAAA=]", "[&BLsLAAA=]" }, new string[4] { "Revolution's Heart Waypoint", "Observation Deck Waypoint", "Moorage Waypoint", "History's End Waypoint" }, new string[4] { "Point de passage du Cœur de la révolution", "Point de passage du Pont d'observation", "Point de passage de l'Amarrage", "Point de passage de la Fin de l'histoire" }),
				new MapEntry("Dragonfall", "Chute draconique", new string[4] { "[&BNELAAA=]", "[&BNILAAA=]", "[&BN4LAAA=]", "[&BOYLAAA=]" }, new string[4] { "The Underworld Waypoint", "Melandru's Lost Domain Waypoint", "Pact Command Waypoint", "Burning Forest Waypoint" }, new string[4] { "Point de passage de l'Outre-monde", "Point de passage du Domaine perdu de Melandru", "Point de passage du haut commandement du Pacte", "Point de passage de la forêt en feu" })
			}),
			new LwSeason(5, new MapEntry[3]
			{
				new MapEntry("Grothmar Valley", "Vallée de Grothmar", new string[3] { "[&BPgLAAA=]", "[&BA4MAAA=]", "[&BBsMAAA=]" }, new string[3] { "Wardowns Waypoint", "Dalada Forest Waypoint", "Blood Keep Waypoint" }, new string[3] { "Point de passage du Sentier de la guerre", "Point de passage de la Forêt de Dalada", "Point de passage du Fort sanglant" }),
				new MapEntry("Bjora Marches", "Frontière de Bjora", new string[2] { "[&BCcMAAA=]", "[&BDkMAAA=]" }, new string[2] { "Jora's Keep Waypoint", "Still Waters Speaking Waypoint" }, new string[2] { "Point de passage du Fort de Jora", "Point de passage des Eaux calmes" }),
				new MapEntry("Drizzlewood Coast", "Côte de Bruinebois", new string[2] { "[&BGQMAAA=]", "[&BHIMAAA=]" }, new string[2] { "Base Camp Waypoint", "Forward Camp Waypoint" }, new string[2] { "Point de passage du Camp de base", "Point de passage du Camp avant" })
			})
		};

		public static readonly MapEntry[] AllMaps = CoreTyria.SelectMany((RegionSection section) => section.Maps).Concat(HeartOfThorns).Concat(PathOfFire)
			.Concat(EndOfDragons)
			.Concat(SecretsOfTheObscure)
			.Concat(JanthirWilds)
			.Concat(VisionsOfEternity)
			.Concat(LivingWorld.SelectMany((LwSeason season) => season.Maps))
			.ToArray();
	}
}
