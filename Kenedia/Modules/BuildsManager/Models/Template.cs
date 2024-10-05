using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Blish_HUD;
using Gw2Sharp;
using Gw2Sharp.ChatLinks;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.TemplateEntries;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager.Models
{
	[DataContract]
	public class Template : IDisposable
	{
		private readonly System.Timers.Timer _timer;

		private bool _isDisposed;

		private Races _race = Races.None;

		private ProfessionType _profession = ProfessionType.Guardian;

		private string _name = strings.NewTemplate;

		private string _description = string.Empty;

		private string _savedBuildCode = string.Empty;

		private string _savedGearCode = string.Empty;

		[JsonProperty("Tags")]
		[DataMember]
		private UniqueObservableCollection<string> _tags;

		private CancellationTokenSource? _cancellationTokenSource;

		private bool _saveRequested;

		private List<BuildSpecialization> _specializations;

		[JsonProperty("LastModified")]
		[DataMember]
		private string _lastModified = DateTime.Now.ToString("d");

		public Data Data { get; }

		public Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? SavedEliteSpecialization { get; private set; }

		public string FilePath => BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.Paths.TemplatesPath + Common.MakeValidFileName(Name.Trim(), '_') + ".json";

		[DataMember]
		public ProfessionType Profession
		{
			get
			{
				return _profession;
			}
			private set
			{
				Common.SetProperty(ref _profession, value, new ValueChangedEventHandler<ProfessionType>(OnProfessionChanged));
			}
		}

		[DataMember]
		public Races Race
		{
			get
			{
				return _race;
			}
			private set
			{
				Common.SetProperty(ref _race, value, new ValueChangedEventHandler<Races>(OnRaceChanged));
			}
		}

		public UniqueObservableCollection<string> Tags
		{
			get
			{
				return _tags;
			}
			private set
			{
				Common.SetProperty<UniqueObservableCollection<string>>(ref _tags, value, new ValueChangedEventHandler<UniqueObservableCollection<string>>(OnTagsListChanged));
			}
		}

		[DataMember]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				Common.SetProperty(ref _name, value, new ValueChangedEventHandler<string>(OnNameChanged));
			}
		}

		[DataMember]
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				Common.SetProperty(ref _description, value, new Action(OnDescriptionChanged));
			}
		}

		[DataMember]
		public string? BuildCode
		{
			get
			{
				if (Loaded)
				{
					return ParseBuildCode();
				}
				return _savedBuildCode;
			}
			set
			{
				LoadBuildFromCode(value);
			}
		}

		[DataMember]
		public string? GearCode
		{
			get
			{
				if (Loaded)
				{
					return ParseGearCode();
				}
				return _savedGearCode;
			}
			set
			{
				LoadGearFromCode(value);
			}
		}

		[DataMember]
		public int? EliteSpecializationId
		{
			get
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization specialization = Specializations.Specialization3.Specialization;
				if (specialization == null)
				{
					return SavedEliteSpecialization?.Id;
				}
				return specialization.Id;
			}
		}

		public Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? EliteSpecialization => Specializations.Specialization3?.Specialization ?? SavedEliteSpecialization;

		public RangerPets Pets { get; } = new RangerPets();


		public SkillCollection Skills { get; } = new SkillCollection();


		public LegendCollection Legends { get; } = new LegendCollection();


		public Specializations Specializations { get; } = new Specializations();


		public BuildSpecialization? this[SpecializationSlotType slot] => slot switch
		{
			SpecializationSlotType.Line_1 => Specializations.Specialization1, 
			SpecializationSlotType.Line_2 => Specializations.Specialization2, 
			SpecializationSlotType.Line_3 => Specializations.Specialization3, 
			_ => null, 
		};

		public TemplateEntry? this[TemplateSlotType slot]
		{
			get
			{
				if (slot != TemplateSlotType.None)
				{
					return (TemplateEntry)GetType().GetProperty(slot.ToString()).GetValue(this);
				}
				return null;
			}
		}

		public ArmorTemplateEntry Head { get; } = new ArmorTemplateEntry(TemplateSlotType.Head);


		public ArmorTemplateEntry Shoulder { get; } = new ArmorTemplateEntry(TemplateSlotType.Shoulder);


		public ArmorTemplateEntry Chest { get; } = new ArmorTemplateEntry(TemplateSlotType.Chest);


		public ArmorTemplateEntry Hand { get; } = new ArmorTemplateEntry(TemplateSlotType.Hand);


		public ArmorTemplateEntry Leg { get; } = new ArmorTemplateEntry(TemplateSlotType.Leg);


		public ArmorTemplateEntry Foot { get; } = new ArmorTemplateEntry(TemplateSlotType.Foot);


		public ArmorTemplateEntry AquaBreather { get; } = new ArmorTemplateEntry(TemplateSlotType.AquaBreather);


		public WeaponTemplateEntry MainHand { get; } = new WeaponTemplateEntry(TemplateSlotType.MainHand);


		public WeaponTemplateEntry OffHand { get; } = new WeaponTemplateEntry(TemplateSlotType.OffHand);


		public AquaticWeaponTemplateEntry Aquatic { get; } = new AquaticWeaponTemplateEntry(TemplateSlotType.Aquatic);


		public WeaponTemplateEntry AltMainHand { get; } = new WeaponTemplateEntry(TemplateSlotType.AltMainHand);


		public WeaponTemplateEntry AltOffHand { get; } = new WeaponTemplateEntry(TemplateSlotType.AltOffHand);


		public AquaticWeaponTemplateEntry AltAquatic { get; } = new AquaticWeaponTemplateEntry(TemplateSlotType.AltAquatic);


		public BackTemplateEntry Back { get; } = new BackTemplateEntry(TemplateSlotType.Back);


		public AmuletTemplateEntry Amulet { get; } = new AmuletTemplateEntry(TemplateSlotType.Amulet);


		public AccessoireTemplateEntry Accessory_1 { get; } = new AccessoireTemplateEntry(TemplateSlotType.Accessory_1);


		public AccessoireTemplateEntry Accessory_2 { get; } = new AccessoireTemplateEntry(TemplateSlotType.Accessory_2);


		public RingTemplateEntry Ring_1 { get; } = new RingTemplateEntry(TemplateSlotType.Ring_1);


		public RingTemplateEntry Ring_2 { get; } = new RingTemplateEntry(TemplateSlotType.Ring_2);


		public PvpAmuletTemplateEntry PvpAmulet { get; } = new PvpAmuletTemplateEntry(TemplateSlotType.PvpAmulet);


		public NourishmentTemplateEntry Nourishment { get; } = new NourishmentTemplateEntry(TemplateSlotType.Nourishment);


		public EnhancementTemplateEntry Enhancement { get; } = new EnhancementTemplateEntry(TemplateSlotType.Enhancement);


		public PowerCoreTemplateEntry PowerCore { get; } = new PowerCoreTemplateEntry(TemplateSlotType.PowerCore);


		public PveRelicTemplateEntry PveRelic { get; } = new PveRelicTemplateEntry(TemplateSlotType.PveRelic);


		public PvpRelicTemplateEntry PvpRelic { get; } = new PvpRelicTemplateEntry(TemplateSlotType.PvpRelic);


		public Dictionary<TemplateSlotType, TemplateEntry> Weapons { get; }

		public bool TriggerAutoSave { get; set; }

		public string LastModified
		{
			get
			{
				return _lastModified;
			}
			set
			{
				Common.SetProperty(ref _lastModified, value, new ValueChangedEventHandler<string>(OnLastModifiedChanged));
			}
		}

		public bool Loaded { get; set; }

		public Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? this[SkillSlotType slot] => Skills[slot];

		public event EventHandler? GearCodeChanged;

		public event EventHandler? BuildCodeChanged;

		public event ValueChangedEventHandler<Races>? RaceChanged;

		public event ValueChangedEventHandler<string>? NameChanged;

		public event ValueChangedEventHandler<string>? LastModifiedChanged;

		public event ValueChangedEventHandler<ProfessionType>? ProfessionChanged;

		public event PetChangedEventHandler? PetChanged;

		public event SkillChangedEventHandler? SkillChanged;

		public event TraitChangedEventHandler? TraitChanged;

		public event SpecializationChangedEventHandler? SpecializationChanged;

		public event SpecializationChangedEventHandler? EliteSpecializationChanged;

		public event LegendChangedEventHandler? LegendChanged;

		public event TemplateSlotChangedEventHandler? TemplateSlotChanged;

		public Template(Data data)
		{
			Data = data;
			_timer = new System.Timers.Timer(1000.0);
			_timer.Elapsed += OnTimerElapsed;
			Weapons = new Dictionary<TemplateSlotType, TemplateEntry>
			{
				{
					TemplateSlotType.MainHand,
					MainHand
				},
				{
					TemplateSlotType.OffHand,
					OffHand
				},
				{
					TemplateSlotType.AltMainHand,
					AltMainHand
				},
				{
					TemplateSlotType.AltOffHand,
					AltOffHand
				},
				{
					TemplateSlotType.Aquatic,
					Aquatic
				},
				{
					TemplateSlotType.AltAquatic,
					AltAquatic
				}
			};
			MainHand.PairedWeapon = OffHand;
			OffHand.PairedWeapon = MainHand;
			AltMainHand.PairedWeapon = AltOffHand;
			AltOffHand.PairedWeapon = AltMainHand;
			Profession = GameService.Gw2Mumble.PlayerCharacter?.Profession ?? Profession;
			Tags = new UniqueObservableCollection<string>();
			Tags.CollectionChanged += Tags_CollectionChanged;
		}

		public Template(Data data, string? buildCode, string? gearCode)
			: this(data)
		{
			LoadFromCode(buildCode, gearCode);
		}

		[JsonConstructor]
		public Template(Data data, string name, string buildCode, string gearCode, string description, UniqueObservableCollection<string> tags, Races? race, ProfessionType? profession, int? elitespecId)
			: this(data)
		{
			_name = name;
			_race = race.GetValueOrDefault(Races.None);
			_profession = profession.GetValueOrDefault(ProfessionType.Guardian);
			SavedEliteSpecialization = Data.Professions[Profession]?.Specializations.FirstOrDefault<KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization>>((KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization> e) => e.Value.Id == elitespecId).Value;
			_description = description;
			Tags = tags ?? _tags;
			_savedBuildCode = buildCode;
			_savedGearCode = gearCode;
			SetArmorItems();
		}

		private void OnLastModifiedChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<string> e)
		{
			this.LastModifiedChanged?.Invoke(this, e);
			RequestSave("OnLastModifiedChanged");
		}

		private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			if (_saveRequested)
			{
				_timer.Stop();
				await Save();
			}
		}

		private void RequestSave([CallerMemberName] string name = "unkown")
		{
			_saveRequested = !string.IsNullOrEmpty(Name) && TriggerAutoSave && Loaded;
			if (_saveRequested)
			{
				_timer.Stop();
				_timer.Start();
			}
		}

		private void OnTagsListChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<UniqueObservableCollection<string>> e)
		{
			if (e.NewValue != null)
			{
				e.NewValue!.CollectionChanged += Tags_CollectionChanged;
			}
			if (e.OldValue != null)
			{
				e.OldValue!.CollectionChanged -= Tags_CollectionChanged;
			}
		}

		private void Tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			RequestSave("Tags_CollectionChanged");
		}

		private void OnNameChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<string> e)
		{
			RequestSave("OnNameChanged");
			this.NameChanged?.Invoke(this, e);
		}

		private void OnGearChanged(object sender, TemplateSlotChangedEventArgs e)
		{
			this.TemplateSlotChanged?.Invoke(sender, e);
			OnGearCodeChanged();
			RequestSave("OnGearChanged");
		}

		public void SetItem<T>(TemplateSlotType slot, TemplateSubSlotType subSlot, T obj)
		{
			TemplateEntry entry = this[slot];
			if (!entry.SetValue(slot, subSlot, obj))
			{
				return;
			}
			if (subSlot == TemplateSubSlotType.Item && slot.IsWeapon())
			{
				Kenedia.Modules.BuildsManager.DataModels.Items.Weapon weapon = obj as Kenedia.Modules.BuildsManager.DataModels.Items.Weapon;
				if (weapon != null)
				{
					SetWeapon(slot, subSlot, weapon);
				}
			}
			OnGearChanged(entry, new TemplateSlotChangedEventArgs(slot, subSlot, obj));
		}

		private void SetWeapon(TemplateSlotType slot, TemplateSubSlotType subSlot, Kenedia.Modules.BuildsManager.DataModels.Items.Weapon? weapon)
		{
			TemplateSlotType? offhand2 = slot.GetOffhand();
			if (offhand2.HasValue)
			{
				TemplateSlotType offhand = offhand2.GetValueOrDefault();
				WeaponTemplateEntry offhandEntry = this[offhand] as WeaponTemplateEntry;
				if (offhandEntry == null)
				{
					return;
				}
				if (weapon?.WeaponType.IsTwoHanded() ?? false)
				{
					if (offhandEntry.SetValue(offhand, subSlot, weapon))
					{
						OnGearChanged(offhandEntry, new TemplateSlotChangedEventArgs(offhand, subSlot, weapon));
					}
				}
				else if (offhandEntry.Weapon?.WeaponType.IsTwoHanded() ?? false)
				{
					bool? flag = weapon?.WeaponType.IsOneHanded();
					if (flag.HasValue && flag.GetValueOrDefault() && offhandEntry.SetValue(offhand, subSlot, null))
					{
						OnGearChanged(offhandEntry, new TemplateSlotChangedEventArgs(offhand, subSlot, weapon));
					}
				}
			}
			else
			{
				if (!slot.IsOffhand())
				{
					return;
				}
				offhand2 = slot.GetMainHand();
				if (!offhand2.HasValue)
				{
					return;
				}
				TemplateSlotType mainHand = offhand2.GetValueOrDefault();
				if (!(weapon?.WeaponType.IsOneHanded() ?? false))
				{
					return;
				}
				WeaponTemplateEntry mainHandEntry = this[mainHand] as WeaponTemplateEntry;
				if (mainHandEntry != null)
				{
					bool? flag = mainHandEntry.Weapon?.WeaponType.IsTwoHanded();
					if (flag.HasValue && flag.GetValueOrDefault() && mainHandEntry.SetValue(mainHand, subSlot, null))
					{
						OnGearChanged(mainHandEntry, new TemplateSlotChangedEventArgs(mainHand, subSlot, weapon));
					}
				}
			}
		}

		public void SetProfession(ProfessionType profession)
		{
			Profession = profession;
			SetSpecialization(SpecializationSlotType.Line_1, null);
			SetSpecialization(SpecializationSlotType.Line_2, null);
			SetSpecialization(SpecializationSlotType.Line_3, null);
			Pets.Wipe();
			Legends.Wipe();
			SavedEliteSpecialization = null;
			RemoveInvalidSkillsBasedOnSpec();
		}

		private void OnProfessionChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<ProfessionType> e)
		{
			SetArmorItems();
			RemoveInvalidGearCombinations();
			this.ProfessionChanged?.Invoke(this, e);
			OnBuildCodeChanged();
		}

		private void SetArmorItems()
		{
			if (Data != null)
			{
				switch (Profession.GetArmorType())
				{
				case ItemWeightType.Heavy:
					AquaBreather.SetValue(TemplateSlotType.AquaBreather, TemplateSubSlotType.Item, Data.Armors[79895]);
					Head.SetValue(TemplateSlotType.Head, TemplateSubSlotType.Item, Data.Armors[80384]);
					Shoulder.SetValue(TemplateSlotType.Shoulder, TemplateSubSlotType.Item, Data.Armors[80435]);
					Chest.SetValue(TemplateSlotType.Chest, TemplateSubSlotType.Item, Data.Armors[80254]);
					Hand.SetValue(TemplateSlotType.Hand, TemplateSubSlotType.Item, Data.Armors[80205]);
					Leg.SetValue(TemplateSlotType.Leg, TemplateSubSlotType.Item, Data.Armors[80277]);
					Foot.SetValue(TemplateSlotType.Foot, TemplateSubSlotType.Item, Data.Armors[80557]);
					break;
				case ItemWeightType.Medium:
					AquaBreather.SetValue(TemplateSlotType.AquaBreather, TemplateSubSlotType.Item, Data.Armors[79838]);
					Head.SetValue(TemplateSlotType.Head, TemplateSubSlotType.Item, Data.Armors[80296]);
					Shoulder.SetValue(TemplateSlotType.Shoulder, TemplateSubSlotType.Item, Data.Armors[80145]);
					Chest.SetValue(TemplateSlotType.Chest, TemplateSubSlotType.Item, Data.Armors[80578]);
					Hand.SetValue(TemplateSlotType.Hand, TemplateSubSlotType.Item, Data.Armors[80161]);
					Leg.SetValue(TemplateSlotType.Leg, TemplateSubSlotType.Item, Data.Armors[80252]);
					Foot.SetValue(TemplateSlotType.Foot, TemplateSubSlotType.Item, Data.Armors[80281]);
					break;
				case ItemWeightType.Light:
					AquaBreather.SetValue(TemplateSlotType.AquaBreather, TemplateSubSlotType.Item, Data.Armors[79873]);
					Head.SetValue(TemplateSlotType.Head, TemplateSubSlotType.Item, Data.Armors[80248]);
					Shoulder.SetValue(TemplateSlotType.Shoulder, TemplateSubSlotType.Item, Data.Armors[80131]);
					Chest.SetValue(TemplateSlotType.Chest, TemplateSubSlotType.Item, Data.Armors[80190]);
					Hand.SetValue(TemplateSlotType.Hand, TemplateSubSlotType.Item, Data.Armors[80111]);
					Leg.SetValue(TemplateSlotType.Leg, TemplateSubSlotType.Item, Data.Armors[80356]);
					Foot.SetValue(TemplateSlotType.Foot, TemplateSubSlotType.Item, Data.Armors[80399]);
					break;
				}
			}
		}

		private void OnRaceChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Races> e)
		{
			RemoveInvalidSkillsBasedOnRace();
			this.RaceChanged?.Invoke(this, e);
			OnBuildCodeChanged();
		}

		public void LoadFromCode(string? build = null, string? gear = null, bool save = false)
		{
			if (build != null)
			{
				LoadBuildFromCode(build, save);
			}
			if (gear != null)
			{
				LoadGearFromCode(gear, save);
			}
		}

		public void LoadBuildFromCode(string? code, bool save = false)
		{
			if (code != null && Gw2ChatLink.TryParse(code, out var chatlink))
			{
				BuildChatLink build = new BuildChatLink();
				build.Parse(chatlink.ToArray());
				Profession = build.Profession;
				LoadSpecializationFromCode(build.Profession, SpecializationSlotType.Line_1, build.Specialization1Id, build.Specialization1Trait1Index, build.Specialization1Trait2Index, build.Specialization1Trait3Index);
				LoadSpecializationFromCode(build.Profession, SpecializationSlotType.Line_2, build.Specialization2Id, build.Specialization2Trait1Index, build.Specialization2Trait2Index, build.Specialization2Trait3Index);
				LoadSpecializationFromCode(build.Profession, SpecializationSlotType.Line_3, build.Specialization3Id, build.Specialization3Trait1Index, build.Specialization3Trait2Index, build.Specialization3Trait3Index);
				if (Profession == ProfessionType.Ranger)
				{
					Pets.LoadFromCode(build.RangerTerrestrialPet1Id, build.RangerTerrestrialPet2Id, build.RangerAquaticPet1Id, build.RangerAquaticPet2Id);
				}
				if (Profession == ProfessionType.Revenant)
				{
					SetLegend(LegendSlotType.TerrestrialInactive, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.FromByte(build.RevenantInactiveTerrestrialLegend));
					SetSkill(SkillSlotType.Inactive | SkillSlotType.Terrestrial | SkillSlotType.Heal, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.FromByte(build.RevenantInactiveTerrestrialLegend)?.Heal);
					SetSkill(SkillSlotType.Inactive | SkillSlotType.Terrestrial | SkillSlotType.Utility_1, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.RevenantInactiveTerrestrialUtility1SkillPaletteId, Legends[LegendSlotType.TerrestrialInactive]));
					SetSkill(SkillSlotType.Inactive | SkillSlotType.Terrestrial | SkillSlotType.Utility_2, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.RevenantInactiveTerrestrialUtility2SkillPaletteId, Legends[LegendSlotType.TerrestrialInactive]));
					SetSkill(SkillSlotType.Inactive | SkillSlotType.Terrestrial | SkillSlotType.Utility_3, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.RevenantInactiveTerrestrialUtility3SkillPaletteId, Legends[LegendSlotType.TerrestrialInactive]));
					SetSkill(SkillSlotType.Inactive | SkillSlotType.Terrestrial | SkillSlotType.Elite, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.FromByte(build.RevenantInactiveTerrestrialLegend)?.Elite);
					SetLegend(LegendSlotType.AquaticInactive, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.FromByte(build.RevenantInactiveAquaticLegend));
					SetSkill(SkillSlotType.Inactive | SkillSlotType.Aquatic | SkillSlotType.Heal, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.FromByte(build.RevenantInactiveAquaticLegend)?.Heal);
					SetSkill(SkillSlotType.Inactive | SkillSlotType.Aquatic | SkillSlotType.Utility_1, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.RevenantInactiveAquaticUtility1SkillPaletteId, Legends[LegendSlotType.AquaticInactive]));
					SetSkill(SkillSlotType.Inactive | SkillSlotType.Aquatic | SkillSlotType.Utility_2, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.RevenantInactiveAquaticUtility2SkillPaletteId, Legends[LegendSlotType.AquaticInactive]));
					SetSkill(SkillSlotType.Inactive | SkillSlotType.Aquatic | SkillSlotType.Utility_3, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.RevenantInactiveAquaticUtility3SkillPaletteId, Legends[LegendSlotType.AquaticInactive]));
					SetSkill(SkillSlotType.Inactive | SkillSlotType.Aquatic | SkillSlotType.Elite, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.FromByte(build.RevenantInactiveAquaticLegend)?.Elite);
					SetLegend(LegendSlotType.TerrestrialActive, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.FromByte(build.RevenantActiveTerrestrialLegend));
					SetSkill(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Heal, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.TerrestrialHealingSkillPaletteId, Legends[LegendSlotType.TerrestrialActive]));
					SetSkill(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Utility_1, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.TerrestrialUtility1SkillPaletteId, Legends[LegendSlotType.TerrestrialActive]));
					SetSkill(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Utility_2, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.TerrestrialUtility2SkillPaletteId, Legends[LegendSlotType.TerrestrialActive]));
					SetSkill(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Utility_3, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.TerrestrialUtility3SkillPaletteId, Legends[LegendSlotType.TerrestrialActive]));
					SetSkill(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Elite, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.TerrestrialEliteSkillPaletteId, Legends[LegendSlotType.TerrestrialActive]));
					SetLegend(LegendSlotType.AquaticActive, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.FromByte(build.RevenantActiveAquaticLegend));
					SetSkill(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Heal, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.AquaticHealingSkillPaletteId, Legends[LegendSlotType.AquaticActive]));
					SetSkill(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Utility_1, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.AquaticUtility1SkillPaletteId, Legends[LegendSlotType.AquaticActive]));
					SetSkill(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Utility_2, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.AquaticUtility2SkillPaletteId, Legends[LegendSlotType.AquaticActive]));
					SetSkill(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Utility_3, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.AquaticUtility3SkillPaletteId, Legends[LegendSlotType.AquaticActive]));
					SetSkill(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Elite, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort(build.AquaticEliteSkillPaletteId, Legends[LegendSlotType.AquaticActive]));
				}
				else
				{
					SetSkill(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Heal, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill.FromUShort(build.TerrestrialHealingSkillPaletteId, build.Profession));
					SetSkill(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Utility_1, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill.FromUShort(build.TerrestrialUtility1SkillPaletteId, build.Profession));
					SetSkill(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Utility_2, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill.FromUShort(build.TerrestrialUtility2SkillPaletteId, build.Profession));
					SetSkill(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Utility_3, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill.FromUShort(build.TerrestrialUtility3SkillPaletteId, build.Profession));
					SetSkill(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Elite, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill.FromUShort(build.TerrestrialEliteSkillPaletteId, build.Profession));
					SetSkill(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Heal, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill.FromUShort(build.AquaticHealingSkillPaletteId, build.Profession));
					SetSkill(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Utility_1, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill.FromUShort(build.AquaticUtility1SkillPaletteId, build.Profession));
					SetSkill(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Utility_2, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill.FromUShort(build.AquaticUtility2SkillPaletteId, build.Profession));
					SetSkill(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Utility_3, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill.FromUShort(build.AquaticUtility3SkillPaletteId, build.Profession));
					SetSkill(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Elite, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill.FromUShort(build.AquaticEliteSkillPaletteId, build.Profession));
				}
				SetArmorItems();
			}
			if (save)
			{
				RequestSave("LoadBuildFromCode");
			}
		}

		public void LoadGearFromCode(string? code, bool save = false)
		{
			GearChatCode.LoadTemplateFromChatCode(this, code);
			RemoveInvalidGearCombinations();
			OnGearCodeChanged();
			if (save)
			{
				RequestSave("LoadGearFromCode");
			}
		}

		public string? ParseBuildCode()
		{
			BuildChatLink build = new BuildChatLink
			{
				Profession = Profession,
				RangerAquaticPet1Id = Pets.GetPetByte(PetSlotType.Aquatic_1),
				RangerAquaticPet2Id = Pets.GetPetByte(PetSlotType.Aquatic_2),
				RangerTerrestrialPet1Id = Pets.GetPetByte(PetSlotType.Terrestrial_1),
				RangerTerrestrialPet2Id = Pets.GetPetByte(PetSlotType.Terrestrial_2),
				Specialization1Id = Specializations.Specialization1.GetSpecializationByte(),
				Specialization1Trait1Index = Specializations.Specialization1.GetTraitByte(TraitTierType.Adept),
				Specialization1Trait2Index = Specializations.Specialization1.GetTraitByte(TraitTierType.Master),
				Specialization1Trait3Index = Specializations.Specialization1.GetTraitByte(TraitTierType.GrandMaster),
				Specialization2Id = Specializations.Specialization2.GetSpecializationByte(),
				Specialization2Trait1Index = Specializations.Specialization2.GetTraitByte(TraitTierType.Adept),
				Specialization2Trait2Index = Specializations.Specialization2.GetTraitByte(TraitTierType.Master),
				Specialization2Trait3Index = Specializations.Specialization2.GetTraitByte(TraitTierType.GrandMaster),
				Specialization3Id = Specializations.Specialization3.GetSpecializationByte(),
				Specialization3Trait1Index = Specializations.Specialization3.GetTraitByte(TraitTierType.Adept),
				Specialization3Trait2Index = Specializations.Specialization3.GetTraitByte(TraitTierType.Master),
				Specialization3Trait3Index = Specializations.Specialization3.GetTraitByte(TraitTierType.GrandMaster)
			};
			if (Profession == ProfessionType.Revenant)
			{
				build.RevenantActiveTerrestrialLegend = Legends.GetLegendByte(LegendSlotType.TerrestrialActive);
				build.RevenantInactiveTerrestrialLegend = Legends.GetLegendByte(LegendSlotType.TerrestrialInactive);
				build.RevenantInactiveTerrestrialUtility1SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Inactive | SkillSlotType.Terrestrial | SkillSlotType.Utility_1);
				build.RevenantInactiveTerrestrialUtility2SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Inactive | SkillSlotType.Terrestrial | SkillSlotType.Utility_2);
				build.RevenantInactiveTerrestrialUtility3SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Inactive | SkillSlotType.Terrestrial | SkillSlotType.Utility_3);
				build.RevenantActiveAquaticLegend = Legends.GetLegendByte(LegendSlotType.AquaticActive);
				build.RevenantInactiveAquaticLegend = Legends.GetLegendByte(LegendSlotType.AquaticInactive);
				build.RevenantInactiveAquaticUtility1SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Inactive | SkillSlotType.Aquatic | SkillSlotType.Utility_1);
				build.RevenantInactiveAquaticUtility2SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Inactive | SkillSlotType.Aquatic | SkillSlotType.Utility_2);
				build.RevenantInactiveAquaticUtility3SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Inactive | SkillSlotType.Aquatic | SkillSlotType.Utility_3);
			}
			build.TerrestrialHealingSkillPaletteId = Skills.GetPaletteId(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Heal);
			build.TerrestrialUtility1SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Utility_1);
			build.TerrestrialUtility2SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Utility_2);
			build.TerrestrialUtility3SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Utility_3);
			build.TerrestrialEliteSkillPaletteId = Skills.GetPaletteId(SkillSlotType.Active | SkillSlotType.Terrestrial | SkillSlotType.Elite);
			build.AquaticHealingSkillPaletteId = Skills.GetPaletteId(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Heal);
			build.AquaticUtility1SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Utility_1);
			build.AquaticUtility2SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Utility_2);
			build.AquaticUtility3SkillPaletteId = Skills.GetPaletteId(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Utility_3);
			build.AquaticEliteSkillPaletteId = Skills.GetPaletteId(SkillSlotType.Active | SkillSlotType.Aquatic | SkillSlotType.Elite);
			byte[] bytes = build.ToArray();
			build.Parse(bytes.Concat(new byte[2]).ToArray());
			return build.ToString();
		}

		public string? ParseGearCode()
		{
			return GearChatCode.GetGearChatCode(this);
		}

		private void OnDescriptionChanged()
		{
			RequestSave("OnDescriptionChanged");
		}

		public async Task Save(int timeToWait = 1000)
		{
			if (!Loaded)
			{
				return;
			}
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			try
			{
				await Task.Delay(timeToWait, _cancellationTokenSource!.Token);
				if (!_cancellationTokenSource!.Token.IsCancellationRequested)
				{
					string path = BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.Paths.TemplatesPath;
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					string json = JsonConvert.SerializeObject((object)this, SerializerSettings.Default);
					string filePath = path + "\\" + Common.MakeValidFileName(Name.Trim(), '_') + ".json";
					System.IO.File.WriteAllText(filePath, json);
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Saved " + Name + " in " + filePath);
					_saveRequested = false;
					_timer.Stop();
				}
			}
			catch (Exception ex)
			{
				if (!_cancellationTokenSource!.Token.IsCancellationRequested)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn(ex.ToString());
				}
			}
		}

		public async Task<bool> ChangeName(string name)
		{
			string path = FilePath;
			if (!(await FileExtension.WaitForFileUnlock(path)))
			{
				return false;
			}
			try
			{
				if (System.IO.File.Exists(path))
				{
					System.IO.File.Delete(path);
				}
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn(ex.ToString());
			}
			Name = name;
			RequestSave("ChangeName");
			return true;
		}

		public async Task<bool> Delete()
		{
			if (!(await FileExtension.WaitForFileUnlock(FilePath)))
			{
				return false;
			}
			try
			{
				if (System.IO.File.Exists(FilePath))
				{
					System.IO.File.Delete(FilePath);
				}
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Warn(ex.ToString());
			}
			return true;
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
			}
		}

		public void Load()
		{
			if (!Loaded)
			{
				GearCode = _savedGearCode;
				BuildCode = _savedBuildCode;
				Loaded = true;
			}
		}

		private void LoadSpecializationFromCode(ProfessionType profession, SpecializationSlotType slot, byte specId, byte adept, byte master, byte grandMaster)
		{
			BuildSpecialization buildSpecialization = slot switch
			{
				SpecializationSlotType.Line_1 => Specializations.Specialization1, 
				SpecializationSlotType.Line_2 => Specializations.Specialization2, 
				SpecializationSlotType.Line_3 => Specializations.Specialization3, 
				_ => null, 
			};
			if (buildSpecialization != null)
			{
				SetSpecialization(slot, Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization.FromByte(specId, profession));
				SetTrait(buildSpecialization, Kenedia.Modules.BuildsManager.DataModels.Professions.Trait.FromByte(adept, buildSpecialization.Specialization, TraitTierType.Adept), TraitTierType.Adept);
				SetTrait(buildSpecialization, Kenedia.Modules.BuildsManager.DataModels.Professions.Trait.FromByte(master, buildSpecialization.Specialization, TraitTierType.Master), TraitTierType.Master);
				SetTrait(buildSpecialization, Kenedia.Modules.BuildsManager.DataModels.Professions.Trait.FromByte(grandMaster, buildSpecialization.Specialization, TraitTierType.GrandMaster), TraitTierType.GrandMaster);
			}
		}

		private void OnSpecializationChanged(object sender, SpecializationChangedEventArgs e)
		{
			this.SpecializationChanged?.Invoke(sender, e);
			if (e.Slot == SpecializationSlotType.Line_3)
			{
				RemoveInvalidSkillsBasedOnSpec();
				this.EliteSpecializationChanged?.Invoke(sender, e);
			}
			OnBuildCodeChanged();
		}

		public bool HasSpecialization(int? id, out BuildSpecialization slot)
		{
			if (id.HasValue)
			{
				foreach (BuildSpecialization spec in Specializations)
				{
					Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization specialization = spec.Specialization;
					if ((specialization != null) ? (specialization.Id == id) : (!id.HasValue))
					{
						slot = spec;
						return true;
					}
				}
			}
			slot = null;
			return false;
		}

		public bool HasSpecialization(Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? specialization, out BuildSpecialization slot)
		{
			return HasSpecialization(specialization?.Id, out slot);
		}

		public void SetSpecialization(SpecializationSlotType slot, Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization? specialization)
		{
			BuildSpecialization newSpecline = this?[slot];
			if (newSpecline == null)
			{
				return;
			}
			if (HasSpecialization(specialization, out var currentSpecline) && currentSpecline.SpecializationSlot != slot)
			{
				bool? flag = newSpecline.Specialization?.Elite;
				Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization currentSpecialization = ((flag.HasValue && flag.GetValueOrDefault() && currentSpecline.SpecializationSlot != SpecializationSlotType.Line_3) ? null : newSpecline.Specialization);
				flag = currentSpecline.Specialization?.Elite;
				Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization newSpecialization = ((flag.HasValue && flag.GetValueOrDefault() && newSpecline.SpecializationSlot != SpecializationSlotType.Line_3) ? null : currentSpecline.Specialization);
				Kenedia.Modules.BuildsManager.DataModels.Professions.Trait[] currentTraits = newSpecline.Traits.ToArray();
				Kenedia.Modules.BuildsManager.DataModels.Professions.Trait[] newTraits = currentSpecline.Traits.ToArray();
				newSpecline.Specialization = newSpecialization;
				SetTrait(newSpecline, newTraits[0], TraitTierType.Adept);
				SetTrait(newSpecline, newTraits[1], TraitTierType.Master);
				SetTrait(newSpecline, newTraits[2], TraitTierType.GrandMaster);
				currentSpecline.Specialization = currentSpecialization;
				SetTrait(currentSpecline, currentTraits[0], TraitTierType.Adept);
				SetTrait(currentSpecline, currentTraits[1], TraitTierType.Master);
				SetTrait(currentSpecline, currentTraits[2], TraitTierType.GrandMaster);
				OnSpecializationChanged(this, new SpecializationChangedEventArgs(currentSpecline.SpecializationSlot, newSpecialization, currentSpecialization));
				OnSpecializationChanged(this, new SpecializationChangedEventArgs(newSpecline.SpecializationSlot, currentSpecialization, newSpecialization));
			}
			else
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Specialization previousSpecialization = newSpecline.Specialization;
				newSpecline.Specialization = specialization;
				if (slot == SpecializationSlotType.Line_3)
				{
					SavedEliteSpecialization = specialization;
				}
				SetTrait(currentSpecline, null, TraitTierType.Adept);
				SetTrait(currentSpecline, null, TraitTierType.Master);
				SetTrait(currentSpecline, null, TraitTierType.GrandMaster);
				OnSpecializationChanged(this, new SpecializationChangedEventArgs(slot, previousSpecialization, specialization));
			}
		}

		public void SetTrait(SpecializationSlotType spec, Kenedia.Modules.BuildsManager.DataModels.Professions.Trait? trait, TraitTierType tier)
		{
			BuildSpecialization buildSpec = this[spec];
			if (buildSpec != null)
			{
				SetTrait(buildSpec, trait, tier);
			}
		}

		public void SetTrait(BuildSpecialization spec, Kenedia.Modules.BuildsManager.DataModels.Professions.Trait? trait, TraitTierType tier)
		{
			if (spec != null)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Trait previousTrait = null;
				switch (tier)
				{
				case TraitTierType.Adept:
					previousTrait = spec.Traits.Adept;
					spec.Traits.Adept = trait;
					break;
				case TraitTierType.Master:
					previousTrait = spec.Traits.Master;
					spec.Traits.Master = trait;
					break;
				case TraitTierType.GrandMaster:
					previousTrait = spec.Traits.GrandMaster;
					spec.Traits.GrandMaster = trait;
					break;
				}
				OnTraitChanged(new TraitChangedEventArgs(spec.SpecializationSlot, tier, previousTrait, trait));
			}
		}

		private void OnTraitChanged(TraitChangedEventArgs e)
		{
			this.TraitChanged?.Invoke(this, e);
			OnBuildCodeChanged();
		}

		public void SetLegend(LegendSlotType slot, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend? legend)
		{
			SkillSlotType state = SkillSlotType.Active;
			SkillSlotType otherState = SkillSlotType.Active;
			SkillSlotType enviroment = SkillSlotType.Terrestrial;
			switch (slot)
			{
			case LegendSlotType.AquaticActive:
				state = SkillSlotType.Active;
				otherState = SkillSlotType.Inactive;
				enviroment = SkillSlotType.Aquatic;
				break;
			case LegendSlotType.AquaticInactive:
				state = SkillSlotType.Inactive;
				otherState = SkillSlotType.Active;
				enviroment = SkillSlotType.Aquatic;
				break;
			case LegendSlotType.TerrestrialActive:
				state = SkillSlotType.Active;
				otherState = SkillSlotType.Inactive;
				enviroment = SkillSlotType.Terrestrial;
				break;
			case LegendSlotType.TerrestrialInactive:
				state = SkillSlotType.Inactive;
				otherState = SkillSlotType.Active;
				enviroment = SkillSlotType.Terrestrial;
				break;
			}
			LegendSlotType otherSlot = slot switch
			{
				LegendSlotType.AquaticActive => LegendSlotType.AquaticInactive, 
				LegendSlotType.AquaticInactive => LegendSlotType.AquaticActive, 
				LegendSlotType.TerrestrialActive => LegendSlotType.TerrestrialInactive, 
				LegendSlotType.TerrestrialInactive => LegendSlotType.TerrestrialActive, 
				_ => LegendSlotType.TerrestrialActive, 
			};
			if (Legends[otherSlot]?.Id == legend?.Id)
			{
				Legends.SetLegend(otherSlot, Legends[slot]);
				SetLegendSkills(otherState, enviroment, Legends[otherSlot]);
			}
			Kenedia.Modules.BuildsManager.DataModels.Professions.Legend temp = Legends[slot];
			Legends.SetLegend(slot, legend);
			SetLegendSkills(state, enviroment, legend);
			this.LegendChanged?.Invoke(this, new LegendChangedEventArgs(slot, temp, legend));
		}

		private void SetLegendSkills(SkillSlotType state, SkillSlotType enviroment, Kenedia.Modules.BuildsManager.DataModels.Professions.Legend? legend)
		{
			SetSkill(state | enviroment | SkillSlotType.Heal, legend?.Heal);
			SetSkill(state | enviroment | SkillSlotType.Elite, legend?.Elite);
			List<int?> paletteIds = new List<int?>(3)
			{
				Skills[state | enviroment | SkillSlotType.Utility_1]?.PaletteId,
				Skills[state | enviroment | SkillSlotType.Utility_2]?.PaletteId,
				Skills[state | enviroment | SkillSlotType.Utility_3]?.PaletteId
			};
			int?[] missingIds = new List<int?>(3) { 4614, 4651, 4564 }.Except(paletteIds).ToArray();
			SkillSlotType[] array = new SkillSlotType[3]
			{
				SkillSlotType.Utility_1,
				SkillSlotType.Utility_2,
				SkillSlotType.Utility_3
			};
			for (int j = 0; j < array.Length; j++)
			{
				SkillSlotType skillSlot = state | enviroment | array[j];
				int? paletteId = Skills[skillSlot]?.PaletteId;
				SetSkill(skillSlot, paletteId.HasValue ? Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort((ushort)paletteId.Value, legend) : null);
			}
			for (int k = 0; k < missingIds.Length; k++)
			{
				for (int i = 0; i < array.Length; i++)
				{
					SkillSlotType skillSlot2 = state | enviroment | array[i];
					if (Skills[skillSlot2] == null)
					{
						SetSkill(skillSlot2, Skills[skillSlot2] ?? Kenedia.Modules.BuildsManager.DataModels.Professions.Legend.SkillFromUShort((ushort)missingIds[k].Value, legend));
						break;
					}
				}
			}
		}

		private void OnLegendChanged(object sender, LegendChangedEventArgs e)
		{
			this.LegendChanged?.Invoke(sender, e);
			OnBuildCodeChanged();
		}

		private void RemoveInvalidGearCombinations()
		{
			List<TemplateSlotType> wipeWeapons = new List<TemplateSlotType>();
			List<ItemWeaponType> professionWeapons = Data.Professions[Profession]?.Weapons.Select<KeyValuePair<Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon.WeaponType, Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon>, ItemWeaponType>((KeyValuePair<Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon.WeaponType, Kenedia.Modules.BuildsManager.DataModels.Professions.Weapon> e) => e.Value.Type.ToItemWeaponType()).ToList() ?? new List<ItemWeaponType>();
			foreach (KeyValuePair<TemplateSlotType, TemplateEntry> slot2 in Weapons)
			{
				TemplateSlotType key = slot2.Key;
				if (key != TemplateSlotType.Aquatic && key != TemplateSlotType.AltAquatic)
				{
					Kenedia.Modules.BuildsManager.DataModels.Items.Weapon weapon4 = (slot2.Value as WeaponTemplateEntry)?.Weapon;
					if (weapon4 != null && !professionWeapons.Contains(weapon4.WeaponType))
					{
						wipeWeapons.Add(slot2.Key);
					}
				}
				else
				{
					Kenedia.Modules.BuildsManager.DataModels.Items.Weapon weapon3 = (slot2.Value as AquaticWeaponTemplateEntry)?.Weapon;
					if (weapon3 != null && !professionWeapons.Contains(weapon3.WeaponType))
					{
						wipeWeapons.Add(slot2.Key);
					}
				}
			}
			foreach (TemplateSlotType slot in wipeWeapons)
			{
				if (slot != TemplateSlotType.Aquatic && slot != TemplateSlotType.AltAquatic)
				{
					WeaponTemplateEntry weapon2 = Weapons[slot] as WeaponTemplateEntry;
					if (weapon2 != null)
					{
						BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Info($"Remove {weapon2.Weapon?.Name} because we can not wield it with our current profession '{Profession}'.");
						SetItem<Kenedia.Modules.BuildsManager.DataModels.Items.Weapon>(slot, TemplateSubSlotType.Item, null);
					}
				}
				else
				{
					AquaticWeaponTemplateEntry weapon = Weapons[slot] as AquaticWeaponTemplateEntry;
					if (weapon != null)
					{
						BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Info($"Remove {weapon.Weapon?.Name} because we can not wield it with our current profession '{Profession}'.");
						SetItem<Kenedia.Modules.BuildsManager.DataModels.Items.Weapon>(slot, TemplateSubSlotType.Item, null);
					}
				}
			}
		}

		private void RemoveInvalidSkillsBasedOnRace()
		{
			List<int> invalidSkills = Data.Races.Values.Where((Kenedia.Modules.BuildsManager.DataModels.Race x) => x.Id != Race).SelectMany((Kenedia.Modules.BuildsManager.DataModels.Race x) => x.Skills.Values.Select((Kenedia.Modules.BuildsManager.DataModels.Professions.Skill e) => e.Id)).ToList();
			foreach (SkillSlotType slot in (from x in Skills
				where x.Value != null && invalidSkills.Contains(x.Value.Id)
				select x.Key).ToList())
			{
				SetSkill(slot, null);
			}
		}

		private void RemoveInvalidSkillsBasedOnSpec()
		{
			if (Profession == ProfessionType.Revenant)
			{
				List<LegendSlotType> wipeLegends = new List<LegendSlotType>();
				foreach (Kenedia.Modules.BuildsManager.DataModels.Professions.Legend legend in Legends)
				{
					if (legend != null && (legend == null || legend.Specialization != 0) && legend?.Specialization != EliteSpecialization?.Id)
					{
						wipeLegends.Add(Legends[legend]);
					}
				}
				foreach (LegendSlotType slot2 in wipeLegends)
				{
					Kenedia.Modules.BuildsManager.DataModels.Professions.Legend legend2 = Legends[slot2];
					SetLegend(slot2, null);
					OnLegendChanged(this, new LegendChangedEventArgs(slot2, legend2, null));
				}
				return;
			}
			IEnumerable<int?> specIds = from x in Specializations
				where x != null
				select x?.Specialization?.Id;
			List<SkillSlotType> wipeSlots = new List<SkillSlotType>();
			foreach (KeyValuePair<SkillSlotType, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> s in Skills)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill skill = s.Value;
				bool profMatch = skill?.Professions.Contains(Profession) ?? false;
				int num2;
				if ((skill?.Specialization ?? 0) != 0 || 1 == 0)
				{
					int? num = skill?.Specialization;
					if (num.HasValue)
					{
						int specId = num.GetValueOrDefault();
						num2 = (specIds.Contains(specId) ? 1 : 0);
					}
					else
					{
						num2 = 0;
					}
				}
				else
				{
					num2 = 1;
				}
				bool specMatch = (byte)num2 != 0;
				bool isRacial = Data?.Races[Race].Skills.Any<KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>>((KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> x) => x.Value.Id == s.Value?.Id) ?? false;
				if ((!profMatch || !specMatch) && !isRacial)
				{
					wipeSlots.Add(s.Key);
				}
			}
			foreach (SkillSlotType slot in wipeSlots)
			{
				SetSkill(slot, null);
			}
		}

		public void SetSkill(SkillSlotType skillSlot, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? skill)
		{
			SkillSlotType enviromentState = skillSlot.GetEnviromentState();
			if (skill == null || skillSlot.IsTerrestrial() || (((skill != null) ? new bool?(!skill!.Flags.HasFlag(SkillFlag.NoUnderwater)) : null) ?? false) || Profession == ProfessionType.Revenant)
			{
				if (skill != null && Skills.HasSkill(skill, enviromentState))
				{
					SkillSlotType slot = Skills.GetSkillSlot(skill, enviromentState);
					Skills[slot] = Skills[skillSlot];
					Skills[skillSlot] = Skills[slot];
				}
				Skills[skillSlot] = skill;
			}
			OnSkillChanged(skillSlot, skill);
		}

		private void OnSkillChanged(SkillSlotType skillSlot, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? skill)
		{
			this.SkillChanged?.Invoke(this, new SkillChangedEventArgs(skillSlot, skill));
			OnBuildCodeChanged();
		}

		public void SetPet(PetSlotType slot, Kenedia.Modules.BuildsManager.DataModels.Professions.Pet pet)
		{
			Kenedia.Modules.BuildsManager.DataModels.Professions.Pet oldpet = Pets[slot];
			if (Pets.SetPet(slot, pet))
			{
				OnPetChanged(new PetChangedEventArgs(slot, pet, oldpet));
			}
		}

		private void OnPetChanged(PetChangedEventArgs e)
		{
			this.PetChanged?.Invoke(this, e);
		}

		private void OnBuildCodeChanged()
		{
			this.BuildCodeChanged?.Invoke(this, EventArgs.Empty);
			RequestSave("OnBuildCodeChanged");
		}

		private void OnGearCodeChanged()
		{
			this.GearCodeChanged?.Invoke(this, EventArgs.Empty);
			RequestSave("OnGearCodeChanged");
		}

		public void SetRace(Races race)
		{
			Race = race;
		}

		public void SetGroup<T>(TemplateSlotType templateSlot, TemplateSubSlotType subSlot, T obj, bool overrideExisting)
		{
			TemplateSlotType[] slots2 = templateSlot.GetGroup();
			switch (subSlot)
			{
			case TemplateSubSlotType.Item:
			{
				if (obj == null)
				{
					SetWeapons(null, overrideExisting, slots2);
					break;
				}
				Kenedia.Modules.BuildsManager.DataModels.Items.Weapon weapon2 = obj as Kenedia.Modules.BuildsManager.DataModels.Items.Weapon;
				if (weapon2 != null)
				{
					SetWeapons(weapon2, overrideExisting, templateSlot.GetWeaponGroup());
					break;
				}
				BaseItem item = obj as BaseItem;
				if (item == null)
				{
					break;
				}
				TemplateSlotType[] array = slots2;
				foreach (TemplateSlotType slot in array)
				{
					IItemTemplateEntry entry = this[slot];
					if (entry != null)
					{
						SetItem(slot, subSlot, overrideExisting ? item : (entry?.Item ?? item));
					}
				}
				break;
			}
			case TemplateSubSlotType.Stat:
			{
				TemplateSlotType[] array;
				if (obj == null)
				{
					array = slots2;
					foreach (TemplateSlotType slot3 in array)
					{
						IStatTemplateEntry entry3 = this[slot3] as IStatTemplateEntry;
						if (entry3 != null)
						{
							SetItem(slot3, subSlot, overrideExisting ? null : (entry3?.Stat ?? null));
						}
					}
					break;
				}
				Stat stat = obj as Stat;
				if (stat == null)
				{
					break;
				}
				array = slots2;
				foreach (TemplateSlotType slot2 in array)
				{
					IStatTemplateEntry entry2 = this[slot2] as IStatTemplateEntry;
					if (entry2 != null)
					{
						SetItem(slot2, subSlot, overrideExisting ? stat : (entry2?.Stat ?? stat));
					}
				}
				break;
			}
			case TemplateSubSlotType.Rune:
			{
				TemplateSlotType[] array;
				if (obj == null)
				{
					array = slots2;
					foreach (TemplateSlotType slot5 in array)
					{
						IRuneTemplateEntry entry5 = this[slot5] as IRuneTemplateEntry;
						if (entry5 != null)
						{
							SetItem(slot5, subSlot, overrideExisting ? null : (entry5?.Rune ?? null));
						}
					}
					break;
				}
				Rune rune = obj as Rune;
				if (rune == null)
				{
					break;
				}
				array = slots2;
				foreach (TemplateSlotType slot4 in array)
				{
					IRuneTemplateEntry entry4 = this[slot4] as IRuneTemplateEntry;
					if (entry4 != null)
					{
						SetItem(slot4, subSlot, overrideExisting ? rune : (entry4?.Rune ?? rune));
					}
				}
				break;
			}
			case TemplateSubSlotType.Sigil1:
			case TemplateSubSlotType.Sigil2:
				SetSigils(templateSlot, overrideExisting, obj);
				break;
			case TemplateSubSlotType.PvpSigil:
			{
				TemplateSlotType[] array;
				if (obj == null)
				{
					array = slots2;
					foreach (TemplateSlotType slot7 in array)
					{
						IPvpSigilTemplateEntry entry7 = this[slot7] as IPvpSigilTemplateEntry;
						if (entry7 != null)
						{
							SetItem(slot7, subSlot, overrideExisting ? null : (entry7?.PvpSigil ?? null));
						}
					}
					break;
				}
				Sigil pvpSigil = obj as Sigil;
				if (pvpSigil == null)
				{
					break;
				}
				array = slots2;
				foreach (TemplateSlotType slot6 in array)
				{
					IPvpSigilTemplateEntry entry6 = this[slot6] as IPvpSigilTemplateEntry;
					if (entry6 != null)
					{
						SetItem(slot6, subSlot, overrideExisting ? pvpSigil : (entry6?.PvpSigil ?? pvpSigil));
					}
				}
				break;
			}
			case TemplateSubSlotType.Infusion1:
			{
				if (obj == null)
				{
					SetAllInfusions(subSlot, null, overrideExisting, slots2);
					break;
				}
				Infusion infusion2 = obj as Infusion;
				if (infusion2 != null)
				{
					SetAllInfusions(subSlot, infusion2, overrideExisting, slots2);
				}
				break;
			}
			case TemplateSubSlotType.Infusion2:
			{
				if (obj == null)
				{
					SetAllInfusions(subSlot, null, overrideExisting, slots2);
					break;
				}
				Infusion infusion3 = obj as Infusion;
				if (infusion3 != null)
				{
					SetAllInfusions(subSlot, infusion3, overrideExisting, slots2);
				}
				break;
			}
			case TemplateSubSlotType.Infusion3:
			{
				if (obj == null)
				{
					SetAllInfusions(subSlot, null, overrideExisting, slots2);
					break;
				}
				Infusion infusion4 = obj as Infusion;
				if (infusion4 != null)
				{
					SetAllInfusions(subSlot, infusion4, overrideExisting, slots2);
				}
				break;
			}
			case TemplateSubSlotType.Enrichment:
			{
				TemplateSlotType[] array = slots2;
				foreach (TemplateSlotType slot8 in array)
				{
					IEnrichmentTemplateEntry entry8 = this[slot8] as IEnrichmentTemplateEntry;
					if (entry8 == null)
					{
						continue;
					}
					if (obj == null)
					{
						SetItem(slot8, subSlot, overrideExisting ? null : (entry8?.Enrichment ?? null));
						continue;
					}
					Enrichment enrichment = obj as Enrichment;
					if (enrichment != null)
					{
						SetItem(slot8, subSlot, overrideExisting ? enrichment : (entry8?.Enrichment ?? enrichment));
					}
				}
				break;
			}
			}
			void SetAllInfusions(TemplateSubSlotType subSlot, Infusion? infusion, bool overrideExisting, TemplateSlotType[] slots)
			{
				foreach (TemplateSlotType slot9 in slots)
				{
					TemplateEntry templateEntry = this[slot9];
					ITripleInfusionTemplateEntry entry11 = templateEntry as ITripleInfusionTemplateEntry;
					if (entry11 == null)
					{
						IDoubleInfusionTemplateEntry entry10 = templateEntry as IDoubleInfusionTemplateEntry;
						if (entry10 == null)
						{
							ISingleInfusionTemplateEntry entry9 = templateEntry as ISingleInfusionTemplateEntry;
							if (entry9 != null)
							{
								SetItem(slot9, TemplateSubSlotType.Infusion1, overrideExisting ? infusion : (entry9?.Infusion1 ?? infusion));
							}
						}
						else
						{
							SetItem(slot9, TemplateSubSlotType.Infusion1, overrideExisting ? infusion : (entry10?.Infusion1 ?? infusion));
							SetItem(slot9, TemplateSubSlotType.Infusion2, overrideExisting ? infusion : (entry10?.Infusion2 ?? infusion));
						}
					}
					else
					{
						SetItem(slot9, TemplateSubSlotType.Infusion1, overrideExisting ? infusion : (entry11?.Infusion1 ?? infusion));
						SetItem(slot9, TemplateSubSlotType.Infusion2, overrideExisting ? infusion : (entry11?.Infusion2 ?? infusion));
						SetItem(slot9, TemplateSubSlotType.Infusion3, overrideExisting ? infusion : (entry11?.Infusion3 ?? infusion));
					}
				}
			}
			void SetWeapons(Kenedia.Modules.BuildsManager.DataModels.Items.Weapon? weapon, bool overrideExisting, TemplateSlotType[] slots)
			{
				if (templateSlot.IsAquatic())
				{
					TemplateSlotType[] array2 = slots;
					foreach (TemplateSlotType slot12 in array2)
					{
						IWeaponTemplateEntry entry14 = this[slot12] as IWeaponTemplateEntry;
						if (entry14 != null)
						{
							SetItem(slot12, TemplateSubSlotType.Item, overrideExisting ? weapon : (entry14?.Weapon ?? weapon));
						}
					}
				}
				else if (weapon?.WeaponType.IsTwoHanded() ?? false)
				{
					slots = new TemplateSlotType[2]
					{
						TemplateSlotType.MainHand,
						TemplateSlotType.AltMainHand
					};
					TemplateSlotType[] array2 = slots;
					foreach (TemplateSlotType slot11 in array2)
					{
						IWeaponTemplateEntry entry13 = this[slot11] as IWeaponTemplateEntry;
						if (entry13 != null)
						{
							SetItem(slot11, TemplateSubSlotType.Item, overrideExisting ? weapon : (entry13?.Weapon ?? weapon));
						}
					}
				}
				else
				{
					TemplateSlotType[] array2 = slots;
					foreach (TemplateSlotType slot10 in array2)
					{
						IWeaponTemplateEntry entry12 = this[slot10] as IWeaponTemplateEntry;
						if (entry12 != null)
						{
							SetItem(slot10, TemplateSubSlotType.Item, overrideExisting ? weapon : (entry12?.Weapon ?? weapon));
						}
					}
				}
			}
		}

		private void SetSigils(TemplateSlotType templateSlot, bool overrideExisting, object obj)
		{
			templateSlot.GetGroup();
			Sigil sigil5 = ((obj != null) ? (templateSlot switch
			{
				TemplateSlotType.MainHand => (this[TemplateSlotType.MainHand] as ISingleSigilTemplateEntry).Sigil1, 
				TemplateSlotType.OffHand => (this[TemplateSlotType.OffHand] as ISingleSigilTemplateEntry).Sigil1, 
				TemplateSlotType.AltMainHand => (this[TemplateSlotType.AltMainHand] as ISingleSigilTemplateEntry).Sigil1, 
				TemplateSlotType.AltOffHand => (this[TemplateSlotType.AltOffHand] as ISingleSigilTemplateEntry).Sigil1, 
				TemplateSlotType.Aquatic => (this[TemplateSlotType.Aquatic] as IDoubleSigilTemplateEntry).Sigil1, 
				TemplateSlotType.AltAquatic => (this[TemplateSlotType.AltAquatic] as IDoubleSigilTemplateEntry).Sigil1, 
				_ => null, 
			}) : null);
			Sigil sigil3 = sigil5;
			sigil5 = ((obj != null) ? (templateSlot switch
			{
				TemplateSlotType.MainHand => (this[TemplateSlotType.OffHand] as ISingleSigilTemplateEntry).Sigil1, 
				TemplateSlotType.OffHand => (this[TemplateSlotType.OffHand] as ISingleSigilTemplateEntry).Sigil1, 
				TemplateSlotType.AltMainHand => (this[TemplateSlotType.AltOffHand] as ISingleSigilTemplateEntry).Sigil1, 
				TemplateSlotType.AltOffHand => (this[TemplateSlotType.AltOffHand] as ISingleSigilTemplateEntry).Sigil1, 
				TemplateSlotType.Aquatic => (this[TemplateSlotType.Aquatic] as IDoubleSigilTemplateEntry).Sigil2, 
				TemplateSlotType.AltAquatic => (this[TemplateSlotType.AltAquatic] as IDoubleSigilTemplateEntry).Sigil2, 
				_ => null, 
			}) : null);
			Sigil sigil4 = sigil5;
			SetSigils(overrideExisting, sigil3, sigil4);
			void SetSigils(bool overrideExisting, Sigil sigil1, Sigil sigil2)
			{
				SetItem(TemplateSlotType.MainHand, TemplateSubSlotType.Sigil1, overrideExisting ? sigil1 : ((this[TemplateSlotType.MainHand] as ISingleSigilTemplateEntry).Sigil1 ?? sigil1));
				SetItem(TemplateSlotType.OffHand, TemplateSubSlotType.Sigil1, overrideExisting ? sigil2 : ((this[TemplateSlotType.OffHand] as ISingleSigilTemplateEntry).Sigil1 ?? sigil2));
				SetItem(TemplateSlotType.AltMainHand, TemplateSubSlotType.Sigil1, overrideExisting ? sigil1 : ((this[TemplateSlotType.AltMainHand] as ISingleSigilTemplateEntry).Sigil1 ?? sigil1));
				SetItem(TemplateSlotType.AltOffHand, TemplateSubSlotType.Sigil1, overrideExisting ? sigil2 : ((this[TemplateSlotType.AltOffHand] as ISingleSigilTemplateEntry).Sigil1 ?? sigil2));
				SetItem(TemplateSlotType.Aquatic, TemplateSubSlotType.Sigil1, overrideExisting ? sigil1 : ((this[TemplateSlotType.Aquatic] as ISingleSigilTemplateEntry).Sigil1 ?? sigil1));
				SetItem(TemplateSlotType.Aquatic, TemplateSubSlotType.Sigil2, overrideExisting ? sigil2 : ((this[TemplateSlotType.Aquatic] as IDoubleSigilTemplateEntry).Sigil2 ?? sigil2));
				SetItem(TemplateSlotType.AltAquatic, TemplateSubSlotType.Sigil1, overrideExisting ? sigil1 : ((this[TemplateSlotType.AltAquatic] as ISingleSigilTemplateEntry).Sigil1 ?? sigil1));
				SetItem(TemplateSlotType.AltAquatic, TemplateSubSlotType.Sigil2, overrideExisting ? sigil2 : ((this[TemplateSlotType.AltAquatic] as IDoubleSigilTemplateEntry).Sigil2 ?? sigil2));
			}
		}
	}
}
