using System.Collections.Generic;
using Blish_HUD;
using Gw2Sharp.WebApi;

namespace WhereIsMyPSNA
{
	internal static class PsnaScheduleLocalization
	{
		private class Entry
		{
			public string De;

			public string Es;

			public string Fr;
		}

		private static readonly Dictionary<string, Entry> MapNames = new Dictionary<string, Entry>
		{
			["The Silverwastes"] = new Entry
			{
				De = "Die Silberwüste",
				Es = "Los Páramos Argentos",
				Fr = "Les Contrées sauvages d'argent"
			},
			["Brisban Wildlands"] = new Entry
			{
				De = "Brisban-Wildnis",
				Es = "Selvas Brisbanas",
				Fr = "Terres sauvages de Brisban"
			},
			["Straits of Devastation"] = new Entry
			{
				De = "Meerenge der Verwüstung",
				Es = "Estrechos de la Devastación",
				Fr = "Détroit de la dévastation"
			},
			["Queensdale"] = new Entry
			{
				De = "Königintal",
				Es = "Valle de la Reina",
				Fr = "La Vallée de la reine"
			},
			["Lornar's Pass"] = new Entry
			{
				De = "Lornars Pass",
				Es = "Paso de Lornar",
				Fr = "Passage de Lornar"
			},
			["Iron Marches"] = new Entry
			{
				De = "Eisenmark",
				Es = "Fronteras de Hierro",
				Fr = "Marais de fer"
			},
			["Dry Top"] = new Entry
			{
				De = "Trockenkuppe",
				Es = "Cima Seca",
				Fr = "Cimesèche"
			},
			["Mount Maelstrom"] = new Entry
			{
				De = "Mahlstromgipfel",
				Es = "Monte Vorágine",
				Fr = "Mont Maelström"
			},
			["Malchor's Leap"] = new Entry
			{
				De = "Malchors Sprung",
				Es = "Salto de Malchor",
				Fr = "Saut de Malchor"
			},
			["Southsun Cove"] = new Entry
			{
				De = "Südlicht-Bucht",
				Es = "Cala del Sol Austral",
				Fr = "Crique de Sud-Soleil"
			},
			["Wayfarer Foothills"] = new Entry
			{
				De = "Wanderer-Hügel",
				Es = "Colinas del Caminante",
				Fr = "Contreforts du voyageur"
			},
			["Fields of Ruin"] = new Entry
			{
				De = "Felder der Verwüstung",
				Es = "Campos de la Ruina",
				Fr = "Champs de ruine"
			},
			["Cursed Shore"] = new Entry
			{
				De = "Fluchküste",
				Es = "Ribera Maldita",
				Fr = "Rivage maudit"
			},
			["Gendarran Fields"] = new Entry
			{
				De = "Gendarran-Felder",
				Es = "Campos de Gendarran",
				Fr = "Champs de Gendarran"
			},
			["Timberline Falls"] = new Entry
			{
				De = "Baumgrenzen-Fälle",
				Es = "Cataratas de Linarbórea",
				Fr = "Chutes de la canopée"
			},
			["Diessa Plateau"] = new Entry
			{
				De = "Diessa-Plateau",
				Es = "Meseta de Diessa",
				Fr = "Plateau de Diessa"
			},
			["Caledon Forest"] = new Entry
			{
				De = "Caledon-Wald",
				Es = "Bosque de Caledon",
				Fr = "Forêt de Caledon"
			},
			["Bloodtide Coast"] = new Entry
			{
				De = "Blutstrom-Küste",
				Es = "Costa Mareasangrienta",
				Fr = "Côte de la marée sanglante"
			},
			["Frostgorge Sound"] = new Entry
			{
				De = "Eisklamm-Sund",
				Es = "Estrecho de Gorjaescarcha",
				Fr = "Détroit des gorges glacées"
			},
			["Fireheart Rise"] = new Entry
			{
				De = "Feuerherzhügel",
				Es = "Colina del Corazón de Fuego",
				Fr = "Montée de Flambecœur"
			},
			["Metrica Province"] = new Entry
			{
				De = "Provinz Metrica",
				Es = "Provincia de Métrica",
				Fr = "Province de Metrica"
			},
			["Kessex Hills"] = new Entry
			{
				De = "Kessex-Hügel",
				Es = "Colinas Kessex",
				Fr = "Collines de Kessex"
			},
			["Dredgehaunt Cliffs"] = new Entry
			{
				De = "Schauflerschreck-Klippen",
				Es = "Acantilados de Guaridadraga",
				Fr = "Falaises de Hantedraguerre"
			},
			["Plains of Ashford"] = new Entry
			{
				De = "Ebenen von Aschfurt",
				Es = "Llanuras de Ashford",
				Fr = "Plaines d'Ashford"
			},
			["Sparkfly Fen"] = new Entry
			{
				De = "Funkenschwärmersumpf",
				Es = "Pantano de las Centellas",
				Fr = "Marais de Lumillule"
			},
			["Harathi Hinterlands"] = new Entry
			{
				De = "Harathi-Hinterland",
				Es = "Interior Harathi",
				Fr = "Hinterlands harathis"
			},
			["Snowden Drifts"] = new Entry
			{
				De = "Schneekuhlenhöhen",
				Es = "Cúmulos de Guaridanieve",
				Fr = "Congères d'Antreneige"
			},
			["Blazeridge Steppes"] = new Entry
			{
				De = "Flammenkamm-Steppe",
				Es = "Estepas Crestafulgurante",
				Fr = "Les Steppes de la strie flamboyante"
			}
		};

		private static readonly Dictionary<string, Entry> LocationNames;

		public static string LocalizeMap(string en)
		{
			return Localize(MapNames, en, en);
		}

		public static string LocalizeLocation(string chatCode, string en)
		{
			return Localize(LocationNames, chatCode, en);
		}

		private static string Localize(Dictionary<string, Entry> table, string key, string en)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected I4, but got Unknown
			if (key == null || !table.TryGetValue(key, out var entry))
			{
				return en;
			}
			Locale value = GameService.Overlay.get_UserLocale().get_Value();
			switch (value - 1)
			{
			case 1:
				if (!string.IsNullOrEmpty(entry.De))
				{
					return entry.De;
				}
				return en;
			case 0:
				if (!string.IsNullOrEmpty(entry.Es))
				{
					return entry.Es;
				}
				return en;
			case 2:
				if (!string.IsNullOrEmpty(entry.Fr))
				{
					return entry.Fr;
				}
				return en;
			default:
				return en;
			}
		}

		static PsnaScheduleLocalization()
		{
			Dictionary<string, Entry> dictionary = new Dictionary<string, Entry>();
			dictionary["[&BKsHAAA=]"] = new Entry
			{
				De = "Blaue Oase",
				Es = "Oasis Azul",
				Fr = "Oasis bleue"
			};
			dictionary["[&BF0AAAA=]"] = new Entry
			{
				De = "Wegmarke Wendon",
				Es = "Punto de Ruta de Wendon",
				Fr = "Point de passage de Wendon"
			};
			dictionary["[&BO4CAAA=]"] = new Entry
			{
				De = "Wegmarke Fort der Dreifaltigkeit",
				Es = "Punto de Ruta de Fuerte Trinidad",
				Fr = "Point de passage de Fort Trinité"
			};
			dictionary["[&BIMAAAA=]"] = new Entry
			{
				De = "Altarbach-Handelsposten",
				Es = "Bazar de Arroyo del Altar",
				Fr = "Comptoir de l'Autel du Ruisseau"
			};
			dictionary["[&BF0GAAA=]"] = new Entry
			{
				De = "Felsnest",
				Es = "Rocaguarida",
				Fr = "Antreroche"
			};
			dictionary["[&BOcBAAA=]"] = new Entry
			{
				De = "Wegmarke Dorf Schuppenfang",
				Es = "Punto de Ruta de Aldea de Atrapaescamas",
				Fr = "Point de passage du Village d'Attrapécaille"
			};
			dictionary["[&BJQHAAA=]"] = new Entry
			{
				De = "Reparaturwerkstatt",
				Es = "Taller de Reparaciones",
				Fr = "Atelier de réparation"
			};
			dictionary["[&BJsCAAA=]"] = new Entry
			{
				De = "Schutzhafen",
				Es = "Muelles del Refugio",
				Fr = "Docks de l'Abri"
			};
			dictionary["[&BNUGAAA=]"] = new Entry
			{
				De = "Wegmarke Perlen-Inselchen",
				Es = "Punto de Ruta de Islote Perlado",
				Fr = "Point de passage de l'Îlot de perles"
			};
			dictionary["[&BHsBAAA=]"] = new Entry
			{
				De = "Wegmarke Dolyak-Pass",
				Es = "Punto de Ruta de Paso Dolyak",
				Fr = "Point de passage du Col du dolyak"
			};
			dictionary["[&BNMAAAA=]"] = new Entry
			{
				De = "Wegmarke Die Falkentore",
				Es = "Punto de Ruta de Puertas del Halcón",
				Fr = "Point de passage des Portails du faucon"
			};
			dictionary["[&BH8HAAA=]"] = new Entry
			{
				De = "Wegmarke Lager der Standhaftigkeit",
				Es = "Punto de Ruta del Campamento Determinación",
				Fr = "Point de passage du Camp de la détermination"
			};
			dictionary["[&BBEDAAA=]"] = new Entry
			{
				De = "Augur-Fackel",
				Es = "Antorcha del Augur",
				Fr = "Torche d'Augur"
			};
			dictionary["[&BJIBAAA=]"] = new Entry
			{
				De = "Wegmarke Feste der Wachsamen",
				Es = "Punto de Ruta de Fortaleza de la Vigilia",
				Fr = "Point de passage du Fort des Veilleurs"
			};
			dictionary["[&BEICAAA=]"] = new Entry
			{
				De = "Balddihof",
				Es = "Balddistead",
				Fr = "Balddistead"
			};
			dictionary["[&BBABAAA=]"] = new Entry
			{
				De = "Bovarin-Anwesen",
				Es = "Hacienda Bovarin",
				Fr = "Domaine de Bovarine"
			};
			dictionary["[&BIkHAAA=]"] = new Entry
			{
				De = "Azarrs Garten",
				Es = "Puerto de Azarr",
				Fr = "Tonnelle d'Azarr"
			};
			dictionary["[&BDoBAAA=]"] = new Entry
			{
				De = "Wegmarke Mabon-Markt",
				Es = "Punto de Ruta de Mabon",
				Fr = "Point de passage de Mabon"
			};
			dictionary["[&BO4CAAA=]"] = new Entry
			{
				De = "Wegmarke Fort der Dreifaltigkeit",
				Es = "Punto de Ruta de Fuerte Trinidad",
				Fr = "Point de passage de Fort Trinité"
			};
			dictionary["[&BC0AAAA=]"] = new Entry
			{
				De = "Flachschlammlager",
				Es = "Campamento de Llanofango",
				Fr = "Camp de Maraifangeux"
			};
			dictionary["[&BIUCAAA=]"] = new Entry
			{
				De = "Wegmarke Blaue Eisglanz",
				Es = "Punto de Ruta de Fulgor de Hielo Azul",
				Fr = "Point de passage de la Lumière froide"
			};
			dictionary["[&BCECAAA=]"] = new Entry
			{
				De = "Wegmarke Schneegrat-Lager",
				Es = "Punto de Ruta de Campamento del Cerro Nevado",
				Fr = "Point de passage du Camp de la Crête de neige"
			};
			dictionary["[&BIcHAAA=]"] = new Entry
			{
				De = "Erneuerungszuflucht",
				Es = "Refugio de Restauración",
				Fr = "Refuge de restauration"
			};
			dictionary["[&BEwDAAA=]"] = new Entry
			{
				De = "Wegmarke Wegstation der Löwengarde",
				Es = "Punto de Ruta de Estación Vial de la Guardia del León",
				Fr = "Point de passage du Dépôt de la Garde du Lion"
			};
			dictionary["[&BNIEAAA=]"] = new Entry
			{
				De = "Wegmarke Sammelpunkt",
				Es = "Punto de Ruta de Reunión",
				Fr = "Point de passage du Ralliement"
			};
			dictionary["[&BKYBAAA=]"] = new Entry
			{
				De = "Wegmarke Marschwacht-Freistatt",
				Es = "Punto de Ruta de Refugio de Vigilapantano",
				Fr = "Point de passage du Refuge de Miremarais"
			};
			dictionary["[&BIMCAAA=]"] = new Entry
			{
				De = "Wegmarke Kammfelslager",
				Es = "Punto de Ruta de Campamento de Rocacrestada",
				Fr = "Point de passage du Camp de Rochecrête"
			};
			dictionary["[&BA8CAAA=]"] = new Entry
			{
				De = "Haymal-Keil",
				Es = "Vísceras de Haymal",
				Fr = "Pointe d'Haymal"
			};
			dictionary["[&BEgAAAA=]"] = new Entry
			{
				De = "Wegmarke Desider Atum",
				Es = "Punto de Ruta de Desider Atum",
				Fr = "Point de passage de Desider Atum"
			};
			dictionary["[&BKgCAAA=]"] = new Entry
			{
				De = "Wegmarke Abfall-Senken",
				Es = "Punto de Ruta de Hondonadas de Deshechos",
				Fr = "Point de passage des Crevasses oubliées"
			};
			dictionary["[&BBkAAAA=]"] = new Entry
			{
				De = "Garrenhoff",
				Es = "Garenhoff",
				Fr = "Garenhoff"
			};
			dictionary["[&BGQCAAA=]"] = new Entry
			{
				De = "Travelens Wegmarke",
				Es = "Punto de Ruta de Travelen",
				Fr = "Point de passage de Travelen"
			};
			dictionary["[&BIMBAAA=]"] = new Entry
			{
				De = "Wegmarke Temperusspitze",
				Es = "Punto de Ruta de Punta Temperus",
				Fr = "Point de passage du Point de Temperus"
			};
			dictionary["[&BH4HAAA=]"] = new Entry
			{
				De = "Wohlstand-Stadt",
				Es = "Ciudad de Prosperidad",
				Fr = "Ville de Prospérité"
			};
			dictionary["[&BP0CAAA=]"] = new Entry
			{
				De = "Kastell Schattenlieb",
				Es = "Sombrabuena de Caer",
				Fr = "Caer Mande-Ombre"
			};
			dictionary["[&BKYAAAA=]"] = new Entry
			{
				De = "Wegmarke Schildklippe",
				Es = "Punto de Ruta de Cimaescudo",
				Fr = "Point de passage de Chimèrepavois"
			};
			dictionary["[&BDgDAAA=]"] = new Entry
			{
				De = "Mennerheim",
				Es = "Mennerheim",
				Fr = "Mennerheim"
			};
			dictionary["[&BPEBAAA=]"] = new Entry
			{
				De = "Dorf Ferrusato",
				Es = "Aldea Ferrusatos",
				Fr = "Village de Ferrusatos"
			};
			LocationNames = dictionary;
		}
	}
}
