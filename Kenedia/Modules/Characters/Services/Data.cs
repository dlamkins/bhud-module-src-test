using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Characters.Enums;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters.Services
{
	public class Data
	{
		public class CraftingProfession
		{
			public AsyncTexture2D Icon { get; set; }

			public int Id { get; set; }

			public string APIId { get; set; }

			public int MaxRating { get; set; }

			public Dictionary<Locale, string> Names { get; set; } = new Dictionary<Locale, string>();


			public string Name
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					//IL_0012: Invalid comparison between Unknown and I4
					//IL_0014: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Invalid comparison between Unknown and I4
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_002a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					Locale value = GameService.Overlay.get_UserLocale().get_Value();
					Locale locale = (Locale)(((int)value != 4 && (int)value != 5) ? ((int)GameService.Overlay.get_UserLocale().get_Value()) : 0);
					if (!Names.TryGetValue(locale, out var name))
					{
						return "No Name Set.";
					}
					return name;
				}
			}
		}

		public class Profession
		{
			private AsyncTexture2D _icon;

			private AsyncTexture2D _iconBig;

			public Color Color { get; set; }

			public ArmorWeight WeightClass { get; set; }

			public int Id { get; set; }

			public string APIId { get; set; }

			public AsyncTexture2D Icon
			{
				get
				{
					return _icon;
				}
				set
				{
					_icon = value;
					if (_icon != null)
					{
						_icon.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)Icon_TextureSwapped);
					}
				}
			}

			public AsyncTexture2D IconBig
			{
				get
				{
					return _iconBig;
				}
				set
				{
					_iconBig = value;
					if (_iconBig != null)
					{
						_iconBig.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)IconBig_TextureSwapped);
					}
				}
			}

			public Dictionary<Locale, string> Names { get; set; } = new Dictionary<Locale, string>();


			public string Name
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					//IL_0012: Invalid comparison between Unknown and I4
					//IL_0014: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Invalid comparison between Unknown and I4
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_002a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					Locale value = GameService.Overlay.get_UserLocale().get_Value();
					Locale locale = (Locale)(((int)value != 4 && (int)value != 5) ? ((int)GameService.Overlay.get_UserLocale().get_Value()) : 0);
					if (!Names.TryGetValue(locale, out var name))
					{
						return "No Name Set.";
					}
					return name;
				}
			}

			private void IconBig_TextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
			{
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				if (e.get_NewValue() != null)
				{
					IconBig.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)IconBig_TextureSwapped);
					IconBig.SwapTexture(Texture2DExtension.GetRegion(IconBig.get_Texture(), new Rectangle(5, 5, IconBig.get_Width() - 10, IconBig.get_Height() - 10)));
				}
			}

			private void Icon_TextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
			{
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				if (e.get_NewValue() != null)
				{
					Icon.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)Icon_TextureSwapped);
					Icon.SwapTexture(Texture2DExtension.GetRegion(Icon.get_Texture(), new Rectangle(5, 5, Icon.get_Width() - 10, Icon.get_Height() - 10)));
				}
			}
		}

		public class Specialization
		{
			public int Id { get; set; }

			public int APIId { get; set; }

			public ProfessionType Profession { get; set; }

			public AsyncTexture2D Icon { get; set; }

			public AsyncTexture2D IconBig { get; set; }

			public Dictionary<Locale, string> Names { get; set; } = new Dictionary<Locale, string>();


			public string Name
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					//IL_0012: Invalid comparison between Unknown and I4
					//IL_0014: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Invalid comparison between Unknown and I4
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_002a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					Locale value = GameService.Overlay.get_UserLocale().get_Value();
					Locale locale = (Locale)(((int)value != 4 && (int)value != 5) ? ((int)GameService.Overlay.get_UserLocale().get_Value()) : 0);
					if (!Names.TryGetValue(locale, out var name))
					{
						return "No Name Set.";
					}
					return name;
				}
			}
		}

		public class Race
		{
			public int Id { get; set; }

			public string APIId { get; set; }

			public Dictionary<Locale, string> Names { get; set; } = new Dictionary<Locale, string>();


			public string Name
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					//IL_0012: Invalid comparison between Unknown and I4
					//IL_0014: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Invalid comparison between Unknown and I4
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_002a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					Locale value = GameService.Overlay.get_UserLocale().get_Value();
					Locale locale = (Locale)(((int)value != 4 && (int)value != 5) ? ((int)GameService.Overlay.get_UserLocale().get_Value()) : 0);
					if (!Names.TryGetValue(locale, out var name))
					{
						return "No Name Set.";
					}
					return name;
				}
			}

			public AsyncTexture2D Icon { get; set; }
		}

		private readonly ContentsManager _contentsManager;

		private readonly PathCollection _paths;

		public Dictionary<int, Map> Maps { get; private set; } = new Dictionary<int, Map>();


		public Dictionary<int, CraftingProfession> CrafingProfessions { get; } = new Dictionary<int, CraftingProfession>
		{
			{
				0,
				new CraftingProfession
				{
					Icon = AsyncTexture2D.FromAssetId(154983),
					Id = 0,
					APIId = "Unknown",
					MaxRating = 0,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Unbekannt"
						},
						{
							(Locale)0,
							"Unknown"
						},
						{
							(Locale)1,
							"Desconocido"
						},
						{
							(Locale)3,
							"Inconnu"
						}
					}
				}
			},
			{
				1,
				new CraftingProfession
				{
					Icon = AsyncTexture2D.FromAssetId(102463),
					Id = 1,
					APIId = "Artificer",
					MaxRating = 500,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Konstrukteur"
						},
						{
							(Locale)0,
							"Artificer"
						},
						{
							(Locale)1,
							"Artificiero"
						},
						{
							(Locale)3,
							"Artificier"
						}
					}
				}
			},
			{
				2,
				new CraftingProfession
				{
					Icon = AsyncTexture2D.FromAssetId(102461),
					Id = 2,
					APIId = "Armorsmith",
					MaxRating = 500,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Rüstungsschmied"
						},
						{
							(Locale)0,
							"Armorsmith"
						},
						{
							(Locale)1,
							"Forjador de armaduras"
						},
						{
							(Locale)3,
							"Forgeron d'armures"
						}
					}
				}
			},
			{
				3,
				new CraftingProfession
				{
					Icon = AsyncTexture2D.FromAssetId(102465),
					Id = 3,
					APIId = "Chef",
					MaxRating = 500,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Chefkoch"
						},
						{
							(Locale)0,
							"Chef"
						},
						{
							(Locale)1,
							"Cocinero"
						},
						{
							(Locale)3,
							"Maître-queux"
						}
					}
				}
			},
			{
				4,
				new CraftingProfession
				{
					Icon = AsyncTexture2D.FromAssetId(102458),
					Id = 4,
					APIId = "Jeweler",
					MaxRating = 400,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Juwelier"
						},
						{
							(Locale)0,
							"Jeweler"
						},
						{
							(Locale)1,
							"Joyero"
						},
						{
							(Locale)3,
							"Bijoutier"
						}
					}
				}
			},
			{
				5,
				new CraftingProfession
				{
					Icon = AsyncTexture2D.FromAssetId(102462),
					Id = 5,
					APIId = "Huntsman",
					MaxRating = 500,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Waidmann"
						},
						{
							(Locale)0,
							"Huntsman"
						},
						{
							(Locale)1,
							"Cazador"
						},
						{
							(Locale)3,
							"Chasseur"
						}
					}
				}
			},
			{
				6,
				new CraftingProfession
				{
					Icon = AsyncTexture2D.FromAssetId(102464),
					Id = 6,
					APIId = "Leatherworker",
					MaxRating = 500,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Lederer"
						},
						{
							(Locale)0,
							"Leatherworker"
						},
						{
							(Locale)1,
							"Peletero"
						},
						{
							(Locale)3,
							"Travailleur du cuir"
						}
					}
				}
			},
			{
				7,
				new CraftingProfession
				{
					Icon = AsyncTexture2D.FromAssetId(1293677),
					Id = 7,
					APIId = "Scribe",
					MaxRating = 400,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Schreiber"
						},
						{
							(Locale)0,
							"Scribe"
						},
						{
							(Locale)1,
							"Escriba"
						},
						{
							(Locale)3,
							"Illustrateur"
						}
					}
				}
			},
			{
				8,
				new CraftingProfession
				{
					Icon = AsyncTexture2D.FromAssetId(102459),
					Id = 8,
					APIId = "Tailor",
					MaxRating = 500,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Schneider"
						},
						{
							(Locale)0,
							"Tailor"
						},
						{
							(Locale)1,
							"Sastre"
						},
						{
							(Locale)3,
							"Tailleur"
						}
					}
				}
			},
			{
				9,
				new CraftingProfession
				{
					Icon = AsyncTexture2D.FromAssetId(102460),
					Id = 9,
					APIId = "Weaponsmith",
					MaxRating = 500,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Waffenschmied"
						},
						{
							(Locale)0,
							"Weaponsmith"
						},
						{
							(Locale)1,
							"Armero"
						},
						{
							(Locale)3,
							"Forgeron d'armes"
						}
					}
				}
			}
		};


		public Dictionary<ProfessionType, Profession> Professions { get; } = new Dictionary<ProfessionType, Profession>
		{
			{
				(ProfessionType)1,
				new Profession
				{
					Icon = AsyncTexture2D.FromAssetId(156634),
					IconBig = AsyncTexture2D.FromAssetId(156633),
					Id = 1,
					APIId = "Guardian",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Wächter"
						},
						{
							(Locale)0,
							"Guardian"
						},
						{
							(Locale)1,
							"Guardián"
						},
						{
							(Locale)3,
							"Gardien"
						}
					},
					Color = new Color(0, 180, 255),
					WeightClass = ArmorWeight.Heavy
				}
			},
			{
				(ProfessionType)2,
				new Profession
				{
					Icon = AsyncTexture2D.FromAssetId(156643),
					IconBig = AsyncTexture2D.FromAssetId(156642),
					Id = 2,
					APIId = "Warrior",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Krieger"
						},
						{
							(Locale)0,
							"Warrior"
						},
						{
							(Locale)1,
							"Guerrero"
						},
						{
							(Locale)3,
							"Guerrier"
						}
					},
					Color = new Color(247, 157, 0),
					WeightClass = ArmorWeight.Heavy
				}
			},
			{
				(ProfessionType)3,
				new Profession
				{
					Icon = AsyncTexture2D.FromAssetId(156632),
					IconBig = AsyncTexture2D.FromAssetId(156631),
					Id = 3,
					APIId = "Engineer",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Ingenieur"
						},
						{
							(Locale)0,
							"Engineer"
						},
						{
							(Locale)1,
							"Ingeniero"
						},
						{
							(Locale)3,
							"Ingénieur"
						}
					},
					Color = new Color(255, 222, 0),
					WeightClass = ArmorWeight.Medium
				}
			},
			{
				(ProfessionType)4,
				new Profession
				{
					Icon = AsyncTexture2D.FromAssetId(156640),
					IconBig = AsyncTexture2D.FromAssetId(156639),
					Id = 4,
					APIId = "Ranger",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Waldläufer"
						},
						{
							(Locale)0,
							"Ranger"
						},
						{
							(Locale)1,
							"Guardabosques"
						},
						{
							(Locale)3,
							"Rôdeur"
						}
					},
					Color = new Color(234, 255, 0),
					WeightClass = ArmorWeight.Medium
				}
			},
			{
				(ProfessionType)5,
				new Profession
				{
					Icon = AsyncTexture2D.FromAssetId(156641),
					IconBig = AsyncTexture2D.FromAssetId(103581),
					Id = 5,
					APIId = "Thief",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Dieb"
						},
						{
							(Locale)0,
							"Thief"
						},
						{
							(Locale)1,
							"Ladrón"
						},
						{
							(Locale)3,
							"Voleur"
						}
					},
					Color = new Color(255, 83, 0),
					WeightClass = ArmorWeight.Medium
				}
			},
			{
				(ProfessionType)6,
				new Profession
				{
					Icon = AsyncTexture2D.FromAssetId(156630),
					IconBig = AsyncTexture2D.FromAssetId(156629),
					Id = 6,
					APIId = "Elementalist",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Elementarmagier"
						},
						{
							(Locale)0,
							"Elementalist"
						},
						{
							(Locale)1,
							"Elementalista"
						},
						{
							(Locale)3,
							"Élémentaliste"
						}
					},
					Color = new Color(247, 0, 116),
					WeightClass = ArmorWeight.Light
				}
			},
			{
				(ProfessionType)7,
				new Profession
				{
					Icon = AsyncTexture2D.FromAssetId(156636),
					IconBig = AsyncTexture2D.FromAssetId(156635),
					Id = 7,
					APIId = "Mesmer",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Mesmer"
						},
						{
							(Locale)0,
							"Mesmer"
						},
						{
							(Locale)1,
							"Hipnotizador"
						},
						{
							(Locale)3,
							"Envoûteur"
						}
					},
					Color = new Color(255, 0, 240),
					WeightClass = ArmorWeight.Light
				}
			},
			{
				(ProfessionType)8,
				new Profession
				{
					Icon = AsyncTexture2D.FromAssetId(156638),
					IconBig = AsyncTexture2D.FromAssetId(156637),
					Id = 8,
					APIId = "Necromancer",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Nekromant"
						},
						{
							(Locale)0,
							"Necromancer"
						},
						{
							(Locale)1,
							"Nigromante"
						},
						{
							(Locale)3,
							"Nécromant"
						}
					},
					Color = new Color(192, 255, 0),
					WeightClass = ArmorWeight.Light
				}
			},
			{
				(ProfessionType)9,
				new Profession
				{
					Icon = AsyncTexture2D.FromAssetId(961390),
					IconBig = AsyncTexture2D.FromAssetId(965717),
					Id = 9,
					APIId = "Revenant",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Widergänger"
						},
						{
							(Locale)0,
							"Revenant"
						},
						{
							(Locale)1,
							"Retornado"
						},
						{
							(Locale)3,
							"Revenant"
						}
					},
					Color = new Color(255, 0, 0),
					WeightClass = ArmorWeight.Heavy
				}
			}
		};


		public Dictionary<Kenedia.Modules.Characters.Enums.SpecializationType, Specialization> Specializations { get; } = new Dictionary<Kenedia.Modules.Characters.Enums.SpecializationType, Specialization>
		{
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Druid,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1128574),
					Icon = AsyncTexture2D.FromAssetId(1128575),
					Id = 5,
					Profession = (ProfessionType)4,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Druide"
						},
						{
							(Locale)0,
							"Druid"
						},
						{
							(Locale)1,
							"Druida"
						},
						{
							(Locale)3,
							"Druide"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Daredevil,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1128570),
					Icon = AsyncTexture2D.FromAssetId(1128571),
					Id = 7,
					Profession = (ProfessionType)5,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Draufgänger"
						},
						{
							(Locale)0,
							"Daredevil"
						},
						{
							(Locale)1,
							"Temerario"
						},
						{
							(Locale)3,
							"Fracasseur"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Berserker,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1128566),
					Icon = AsyncTexture2D.FromAssetId(1128567),
					Id = 18,
					Profession = (ProfessionType)2,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Berserker"
						},
						{
							(Locale)0,
							"Berserker"
						},
						{
							(Locale)1,
							"Berserker"
						},
						{
							(Locale)3,
							"Berserker"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Dragonhunter,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1128572),
					Icon = AsyncTexture2D.FromAssetId(1128573),
					Id = 27,
					Profession = (ProfessionType)1,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Drachenjäger"
						},
						{
							(Locale)0,
							"Dragonhunter"
						},
						{
							(Locale)1,
							"Cazadragones"
						},
						{
							(Locale)3,
							"Draconnier"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Reaper,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1128578),
					Icon = AsyncTexture2D.FromAssetId(1128579),
					Id = 34,
					Profession = (ProfessionType)8,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Schnitter"
						},
						{
							(Locale)0,
							"Reaper"
						},
						{
							(Locale)1,
							"Segador"
						},
						{
							(Locale)3,
							"Faucheur"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Chronomancer,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1128568),
					Icon = AsyncTexture2D.FromAssetId(1128569),
					Id = 40,
					Profession = (ProfessionType)7,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Chronomant"
						},
						{
							(Locale)0,
							"Chronomancer"
						},
						{
							(Locale)1,
							"Cronomante"
						},
						{
							(Locale)3,
							"Chronomancien"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Scrapper,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1128580),
					Icon = AsyncTexture2D.FromAssetId(1128581),
					Id = 43,
					Profession = (ProfessionType)3,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Schrotter"
						},
						{
							(Locale)0,
							"Scrapper"
						},
						{
							(Locale)1,
							"Chatarrero"
						},
						{
							(Locale)3,
							"Mécatronicien"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Tempest,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1128582),
					Icon = AsyncTexture2D.FromAssetId(1128583),
					Id = 48,
					Profession = (ProfessionType)6,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Sturmbote"
						},
						{
							(Locale)0,
							"Tempest"
						},
						{
							(Locale)1,
							"Tempestad"
						},
						{
							(Locale)3,
							"Cataclyste"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Herald,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1128576),
					Icon = AsyncTexture2D.FromAssetId(1128577),
					Id = 52,
					Profession = (ProfessionType)9,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Herold"
						},
						{
							(Locale)0,
							"Herald"
						},
						{
							(Locale)1,
							"Heraldo"
						},
						{
							(Locale)3,
							"Héraut"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Soulbeast,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1770214),
					Icon = AsyncTexture2D.FromAssetId(1770215),
					Id = 55,
					Profession = (ProfessionType)4,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Seelenwandler"
						},
						{
							(Locale)0,
							"Soulbeast"
						},
						{
							(Locale)1,
							"Bestialma"
						},
						{
							(Locale)3,
							"Animorphe"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Weaver,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1670505),
					Icon = AsyncTexture2D.FromAssetId(1670506),
					Id = 56,
					Profession = (ProfessionType)6,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Weber"
						},
						{
							(Locale)0,
							"Weaver"
						},
						{
							(Locale)1,
							"Tejedor"
						},
						{
							(Locale)3,
							"Tissesort"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Holosmith,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1770224),
					Icon = AsyncTexture2D.FromAssetId(1770225),
					Id = 57,
					Profession = (ProfessionType)3,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Holoschmied"
						},
						{
							(Locale)0,
							"Holosmith"
						},
						{
							(Locale)1,
							"Holoartesano"
						},
						{
							(Locale)3,
							"Holographiste"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Deadeye,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1770212),
					Icon = AsyncTexture2D.FromAssetId(1770213),
					Id = 58,
					Profession = (ProfessionType)5,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Scharfschütze"
						},
						{
							(Locale)0,
							"Deadeye"
						},
						{
							(Locale)1,
							"Certero"
						},
						{
							(Locale)3,
							"Sniper"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Mirage,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1770216),
					Icon = AsyncTexture2D.FromAssetId(1770217),
					Id = 59,
					Profession = (ProfessionType)7,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Illusionist"
						},
						{
							(Locale)0,
							"Mirage"
						},
						{
							(Locale)1,
							"Quimérico"
						},
						{
							(Locale)3,
							"Mirage"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Scourge,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1770220),
					Icon = AsyncTexture2D.FromAssetId(1770221),
					Id = 60,
					Profession = (ProfessionType)8,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Pestbringer"
						},
						{
							(Locale)0,
							"Scourge"
						},
						{
							(Locale)1,
							"Azotador"
						},
						{
							(Locale)3,
							"Fléau"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Spellbreaker,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1770222),
					Icon = AsyncTexture2D.FromAssetId(1770223),
					Id = 61,
					Profession = (ProfessionType)2,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Bannbrecher"
						},
						{
							(Locale)0,
							"Spellbreaker"
						},
						{
							(Locale)1,
							"Rompehechizos"
						},
						{
							(Locale)3,
							"Brisesort"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Firebrand,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1770210),
					Icon = AsyncTexture2D.FromAssetId(1770211),
					Id = 62,
					Profession = (ProfessionType)1,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Aufwiegler"
						},
						{
							(Locale)0,
							"Firebrand"
						},
						{
							(Locale)1,
							"Abrasador"
						},
						{
							(Locale)3,
							"Incendiaire"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Renegade,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(1770218),
					Icon = AsyncTexture2D.FromAssetId(1770219),
					Id = 63,
					Profession = (ProfessionType)9,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Abtrünniger"
						},
						{
							(Locale)0,
							"Renegade"
						},
						{
							(Locale)1,
							"Renegado"
						},
						{
							(Locale)3,
							"Renégat"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Harbinger,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(2479359),
					Icon = AsyncTexture2D.FromAssetId(2479361),
					Id = 64,
					Profession = (ProfessionType)8,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Vorbote"
						},
						{
							(Locale)0,
							"Harbinger"
						},
						{
							(Locale)1,
							"Augurador"
						},
						{
							(Locale)3,
							"Augure"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Willbender,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(2479351),
					Icon = AsyncTexture2D.FromAssetId(2479353),
					Id = 65,
					Profession = (ProfessionType)1,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Willensverdreher"
						},
						{
							(Locale)0,
							"Willbender"
						},
						{
							(Locale)1,
							"Subyugador"
						},
						{
							(Locale)3,
							"Subjugueur"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Virtuoso,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(2479355),
					Icon = AsyncTexture2D.FromAssetId(2479357),
					Id = 66,
					Profession = (ProfessionType)7,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Virtuose"
						},
						{
							(Locale)0,
							"Virtuoso"
						},
						{
							(Locale)1,
							"Virtuoso"
						},
						{
							(Locale)3,
							"Virtuose"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Catalyst,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(2491555),
					Icon = AsyncTexture2D.FromAssetId(2491557),
					Id = 67,
					Profession = (ProfessionType)6,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Katalysierer"
						},
						{
							(Locale)0,
							"Catalyst"
						},
						{
							(Locale)1,
							"Catalizador"
						},
						{
							(Locale)3,
							"Catalyseur"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Bladesworn,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(2491563),
					Icon = AsyncTexture2D.FromAssetId(2491565),
					Id = 68,
					Profession = (ProfessionType)2,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Klingengeschworener"
						},
						{
							(Locale)0,
							"Bladesworn"
						},
						{
							(Locale)1,
							"Jurafilos"
						},
						{
							(Locale)3,
							"Jurelame"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Vindicator,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(2491559),
					Icon = AsyncTexture2D.FromAssetId(2491561),
					Id = 69,
					Profession = (ProfessionType)9,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Rechtssuchender"
						},
						{
							(Locale)0,
							"Vindicator"
						},
						{
							(Locale)1,
							"Justiciero"
						},
						{
							(Locale)3,
							"Justicier"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Mechanist,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(2503656),
					Icon = AsyncTexture2D.FromAssetId(2503658),
					Id = 70,
					Profession = (ProfessionType)3,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Mech-Lenker"
						},
						{
							(Locale)0,
							"Mechanist"
						},
						{
							(Locale)1,
							"Mechanista"
						},
						{
							(Locale)3,
							"Méchamancien"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Specter,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(2503664),
					Icon = AsyncTexture2D.FromAssetId(2503666),
					Id = 71,
					Profession = (ProfessionType)5,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Phantom"
						},
						{
							(Locale)0,
							"Specter"
						},
						{
							(Locale)1,
							"Espectro"
						},
						{
							(Locale)3,
							"Spectre"
						}
					}
				}
			},
			{
				Kenedia.Modules.Characters.Enums.SpecializationType.Untamed,
				new Specialization
				{
					IconBig = AsyncTexture2D.FromAssetId(2503660),
					Icon = AsyncTexture2D.FromAssetId(2503662),
					Id = 72,
					Profession = (ProfessionType)4,
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Ungezähmter"
						},
						{
							(Locale)0,
							"Untamed"
						},
						{
							(Locale)1,
							"Indómito"
						},
						{
							(Locale)3,
							"Indomptable"
						}
					}
				}
			}
		};


		public Dictionary<RaceType, Race> Races { get; } = new Dictionary<RaceType, Race>
		{
			{
				(RaceType)0,
				new Race
				{
					Id = 0,
					APIId = "Asura",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Asura"
						},
						{
							(Locale)0,
							"Asura"
						},
						{
							(Locale)1,
							"Asura"
						},
						{
							(Locale)3,
							"Asura"
						}
					}
				}
			},
			{
				(RaceType)1,
				new Race
				{
					Id = 1,
					APIId = "Charr",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Charr"
						},
						{
							(Locale)0,
							"Charr"
						},
						{
							(Locale)1,
							"Charr"
						},
						{
							(Locale)3,
							"Charr"
						}
					}
				}
			},
			{
				(RaceType)2,
				new Race
				{
					Id = 2,
					APIId = "Human",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Mensch"
						},
						{
							(Locale)0,
							"Human"
						},
						{
							(Locale)1,
							"Humano"
						},
						{
							(Locale)3,
							"Humain"
						}
					}
				}
			},
			{
				(RaceType)3,
				new Race
				{
					Id = 3,
					APIId = "Norn",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Norn"
						},
						{
							(Locale)0,
							"Norn"
						},
						{
							(Locale)1,
							"Norn"
						},
						{
							(Locale)3,
							"Norn"
						}
					}
				}
			},
			{
				(RaceType)4,
				new Race
				{
					Id = 4,
					APIId = "Sylvari",
					Names = new Dictionary<Locale, string>
					{
						{
							(Locale)2,
							"Sylvari"
						},
						{
							(Locale)0,
							"Sylvari"
						},
						{
							(Locale)1,
							"Sylvari"
						},
						{
							(Locale)3,
							"Sylvari"
						}
					}
				}
			}
		};


		public Data(ContentsManager contentsManager, PathCollection paths)
		{
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_064e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0777: Unknown result type (might be due to invalid IL or missing references)
			//IL_080a: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0936: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
			_contentsManager = contentsManager;
			_paths = paths;
		}

		public Map GetMapById(int id)
		{
			if (!Maps.ContainsKey(id))
			{
				return new Map
				{
					Name = "Unknown Map",
					Id = 0
				};
			}
			return Maps[id];
		}

		public async Task Load()
		{
			string path = _paths.ModuleDataPath + "Maps.json";
			BaseModule<Characters, MainWindow, Settings>.Logger.Debug("Trying to load Maps from " + path);
			try
			{
				if (File.Exists(path))
				{
					string jsonString = await new StreamReader(path).ReadToEndAsync();
					if (jsonString != null && jsonString != string.Empty)
					{
						Maps = JsonConvert.DeserializeObject<Dictionary<int, Map>>(jsonString);
						BaseModule<Characters, MainWindow, Settings>.Logger.Debug("Loaded Maps from " + path);
					}
				}
			}
			catch (Exception ex)
			{
				BaseModule<Characters, MainWindow, Settings>.Logger.Warn($"{ex}");
			}
			Races[(RaceType)0].Icon = AsyncTexture2D.op_Implicit(_contentsManager.GetTexture("textures\\races\\asura.png"));
			Races[(RaceType)1].Icon = AsyncTexture2D.op_Implicit(_contentsManager.GetTexture("textures\\races\\charr.png"));
			Races[(RaceType)2].Icon = AsyncTexture2D.op_Implicit(_contentsManager.GetTexture("textures\\races\\human.png"));
			Races[(RaceType)3].Icon = AsyncTexture2D.op_Implicit(_contentsManager.GetTexture("textures\\races\\norn.png"));
			Races[(RaceType)4].Icon = AsyncTexture2D.op_Implicit(_contentsManager.GetTexture("textures\\races\\sylvari.png"));
		}
	}
}
