using System;
using System.Collections.Generic;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.Controls_Old.GearPage.GearSlots;
using Kenedia.Modules.BuildsManager.DataModels;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Tabs
{
	public class GearTab : Blish_HUD.Controls.Container
	{
		private readonly Kenedia.Modules.Core.Controls.TextBox _gearCodeBox;

		private readonly ImageButton _copyButton;

		private readonly ButtonImage _framedSpecIcon;

		private readonly ButtonImage _raceIcon;

		private Rectangle _headerBounds;

		private Dictionary<TemplateSlotType, GearSlot> _templateSlots = new Dictionary<TemplateSlotType, GearSlot>();

		private TemplatePresenter _templatePresenter;

		private Blocker _blocker;

		private readonly DetailedTexture _terrestrialSet = new DetailedTexture(156323);

		private readonly DetailedTexture _alternateTerrestrialSet = new DetailedTexture(156324);

		private readonly DetailedTexture _aquaticSet = new DetailedTexture(156325);

		private readonly DetailedTexture _alternateAquaticSet = new DetailedTexture(156326);

		private readonly DetailedTexture _pve = new DetailedTexture(2229699, 2229700);

		private readonly DetailedTexture _pvp = new DetailedTexture(2229701, 2229702);

		private readonly ProfessionRaceSelection _professionRaceSelection;

		public TemplatePresenter TemplatePresenter
		{
			get
			{
				return _templatePresenter;
			}
			private set
			{
				Common.SetProperty(ref _templatePresenter, value, new ValueChangedEventHandler<TemplatePresenter>(OnTemplatePresenterChanged));
			}
		}

		public SelectionPanel SelectionPanel { get; }

		public Data Data { get; }

		public GearTab(TemplatePresenter templatePresenter, SelectionPanel selectionPanel, Data data)
		{
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			TemplatePresenter = templatePresenter;
			SelectionPanel = selectionPanel;
			Data = data;
			WidthSizingMode = SizingMode.Fill;
			HeightSizingMode = SizingMode.Fill;
			_blocker = new Blocker
			{
				Parent = this,
				CoveredControl = this,
				BackgroundColor = Color.get_Black() * 0.5f,
				BorderWidth = 3,
				Text = "Select a Template to view its details."
			};
			string gearCodeDisclaimer = strings.EquipmentCodeDisclaimer;
			_copyButton = new ImageButton
			{
				Parent = this,
				Location = new Point(0, 0),
				Texture = AsyncTexture2D.FromAssetId(2208345),
				HoveredTexture = AsyncTexture2D.FromAssetId(2208347),
				Size = new Point(26),
				SetLocalizedTooltip = () => gearCodeDisclaimer,
				ClickAction = async delegate
				{
					try
					{
						string s = _gearCodeBox.Text;
						if (s != null && !string.IsNullOrEmpty(s))
						{
							await ClipboardUtil.WindowsClipboardService.SetTextAsync(s);
						}
					}
					catch (ArgumentException)
					{
						ScreenNotification.ShowNotification("Failed to set the clipboard text!", ScreenNotification.NotificationType.Error);
					}
					catch
					{
					}
				}
			};
			_gearCodeBox = new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = this,
				Location = new Point(_copyButton.Right + 2, 0),
				EnterPressedAction = delegate(string txt)
				{
					TemplatePresenter.Template?.LoadGearFromCode(txt, save: true);
				},
				Font = GameService.Content.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size11, ContentService.FontStyle.Regular),
				SetLocalizedTooltip = () => gearCodeDisclaimer
			};
			_framedSpecIcon = new ButtonImage
			{
				Parent = this
			};
			_raceIcon = new ButtonImage
			{
				Parent = this,
				TextureSize = new Point(32),
				ZIndex = 15,
				Texture = (AsyncTexture2D)TexturesService.GetTextureFromRef("textures\\races\\none.png", "none"),
				Size = new Point(40)
			};
			_templateSlots = new Dictionary<TemplateSlotType, GearSlot>
			{
				{
					TemplateSlotType.Head,
					new ArmorSlot(TemplateSlotType.Head, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.Shoulder,
					new ArmorSlot(TemplateSlotType.Shoulder, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.Chest,
					new ArmorSlot(TemplateSlotType.Chest, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.Hand,
					new ArmorSlot(TemplateSlotType.Hand, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.Leg,
					new ArmorSlot(TemplateSlotType.Leg, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.Foot,
					new ArmorSlot(TemplateSlotType.Foot, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.MainHand,
					new WeaponSlot(TemplateSlotType.MainHand, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.OffHand,
					new WeaponSlot(TemplateSlotType.OffHand, this, TemplatePresenter, SelectionPanel, Data)
					{
						Height = 55
					}
				},
				{
					TemplateSlotType.AltMainHand,
					new WeaponSlot(TemplateSlotType.AltMainHand, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.AltOffHand,
					new WeaponSlot(TemplateSlotType.AltOffHand, this, TemplatePresenter, SelectionPanel, Data)
					{
						Height = 55
					}
				},
				{
					TemplateSlotType.AquaBreather,
					new ArmorSlot(TemplateSlotType.AquaBreather, this, TemplatePresenter, SelectionPanel, Data)
					{
						Height = 55
					}
				},
				{
					TemplateSlotType.Aquatic,
					new AquaticWeaponSlot(TemplateSlotType.Aquatic, this, TemplatePresenter, SelectionPanel, Data)
					{
						Height = 55
					}
				},
				{
					TemplateSlotType.AltAquatic,
					new AquaticWeaponSlot(TemplateSlotType.AltAquatic, this, TemplatePresenter, SelectionPanel, Data)
					{
						Height = 55
					}
				},
				{
					TemplateSlotType.Back,
					new BackSlot(TemplateSlotType.Back, this, TemplatePresenter, SelectionPanel, Data)
					{
						Width = 85
					}
				},
				{
					TemplateSlotType.Amulet,
					new AmuletSlot(TemplateSlotType.Amulet, this, TemplatePresenter, SelectionPanel, Data)
					{
						Width = 85
					}
				},
				{
					TemplateSlotType.Ring_1,
					new RingSlot(TemplateSlotType.Ring_1, this, TemplatePresenter, SelectionPanel, Data)
					{
						Width = 85
					}
				},
				{
					TemplateSlotType.Ring_2,
					new RingSlot(TemplateSlotType.Ring_2, this, TemplatePresenter, SelectionPanel, Data)
					{
						Width = 85
					}
				},
				{
					TemplateSlotType.Accessory_1,
					new AccessoireSlot(TemplateSlotType.Accessory_1, this, TemplatePresenter, SelectionPanel, Data)
					{
						Width = 85
					}
				},
				{
					TemplateSlotType.Accessory_2,
					new AccessoireSlot(TemplateSlotType.Accessory_2, this, TemplatePresenter, SelectionPanel, Data)
					{
						Width = 85
					}
				},
				{
					TemplateSlotType.PvpAmulet,
					new PvpAmuletSlot(TemplateSlotType.PvpAmulet, this, TemplatePresenter, SelectionPanel, Data)
					{
						Visible = false
					}
				},
				{
					TemplateSlotType.Nourishment,
					new NourishmentSlot(TemplateSlotType.Nourishment, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.Enhancement,
					new EnhancementSlot(TemplateSlotType.Enhancement, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.PowerCore,
					new PowerCoreSlot(TemplateSlotType.PowerCore, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.PveRelic,
					new RelicSlot(TemplateSlotType.PveRelic, this, TemplatePresenter, SelectionPanel, Data)
				},
				{
					TemplateSlotType.PvpRelic,
					new RelicSlot(TemplateSlotType.PvpRelic, this, TemplatePresenter, SelectionPanel, Data)
				}
			};
			(_templateSlots[TemplateSlotType.PveRelic] as RelicSlot).PairedSlot = _templateSlots[TemplateSlotType.PvpRelic] as RelicSlot;
			(_templateSlots[TemplateSlotType.PvpRelic] as RelicSlot).PairedSlot = _templateSlots[TemplateSlotType.PveRelic] as RelicSlot;
			List<GearSlot> armors = new List<GearSlot>();
			List<GearSlot> weapons = new List<GearSlot>();
			List<GearSlot> jewellery = new List<GearSlot>();
			foreach (GearSlot slot in _templateSlots.Values)
			{
				if (slot.Slot.IsArmor())
				{
					armors.Add(slot);
				}
				else if (slot.Slot.IsWeapon())
				{
					weapons.Add(slot);
				}
				else if (slot.Slot.IsJewellery())
				{
					jewellery.Add(slot);
				}
			}
			foreach (GearSlot item in armors)
			{
				item.SlotGroup = armors;
			}
			foreach (GearSlot item2 in weapons)
			{
				item2.SlotGroup = weapons;
			}
			foreach (GearSlot item3 in jewellery)
			{
				item3.SlotGroup = jewellery;
			}
			_professionRaceSelection = new ProfessionRaceSelection(data)
			{
				Parent = this,
				Visible = false,
				ClipsBounds = false,
				ZIndex = 16
			};
			ApplyTemplate();
		}

		private void TemplatePresenter_GearCodeChanged(object sender, EventArgs e)
		{
			_gearCodeBox.Text = TemplatePresenter?.Template?.GearCode;
		}

		private void TemplatePresenter_ProfessionChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<ProfessionType> e)
		{
			ApplyTemplate();
		}

		private void TemplatePresenter_TemplateChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Template> e)
		{
			ApplyTemplate();
		}

		private void OnTemplatePresenterChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<TemplatePresenter> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.TemplateChanged -= new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
				e.OldValue!.ProfessionChanged -= new ValueChangedEventHandler<ProfessionType>(TemplatePresenter_ProfessionChanged);
				e.OldValue!.RaceChanged -= new ValueChangedEventHandler<Races>(TemplatePresenter_RaceChanged);
				e.OldValue!.GearCodeChanged -= new EventHandler(TemplatePresenter_GearCodeChanged);
			}
			if (e.NewValue != null)
			{
				e.NewValue!.TemplateChanged += new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
				e.NewValue!.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(TemplatePresenter_ProfessionChanged);
				e.NewValue!.RaceChanged += new ValueChangedEventHandler<Races>(TemplatePresenter_RaceChanged);
				e.NewValue!.GearCodeChanged += new EventHandler(TemplatePresenter_GearCodeChanged);
			}
		}

		private void TemplatePresenter_RaceChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Races> e)
		{
			SetRaceIcon();
		}

		public override void RecalculateLayout()
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_048d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_0555: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			//IL_0571: Unknown result type (might be due to invalid IL or missing references)
			//IL_0577: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_062e: Unknown result type (might be due to invalid IL or missing references)
			//IL_066b: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0722: Unknown result type (might be due to invalid IL or missing references)
			//IL_075e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0775: Unknown result type (might be due to invalid IL or missing references)
			//IL_077a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0789: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_080d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0828: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (_gearCodeBox != null)
			{
				_gearCodeBox.Width = base.Width - _gearCodeBox.Left;
			}
			if (_templateSlots.Count > 0)
			{
				int secondColumn = _templateSlots[TemplateSlotType.AquaBreather].Width + 10;
				int gearSpacing = 8;
				int setSize = 36;
				Point p = Point.get_Zero();
				Point s = Point.get_Zero();
				_headerBounds = new Rectangle(0, _copyButton.Top, 300, _copyButton.Height);
				_framedSpecIcon.Size = new Point(40);
				_framedSpecIcon.Location = new Point(base.Right - _framedSpecIcon.Width - 13, ((Rectangle)(ref _headerBounds)).get_Bottom() + 26);
				_raceIcon.Size = new Point(40);
				_raceIcon.Location = new Point(_framedSpecIcon.Left, _framedSpecIcon.Bottom + 4);
				_pve.Bounds = new Rectangle(_framedSpecIcon.Left - _pve.Size.X - 10, _framedSpecIcon.Top, 64, 64);
				_pvp.Bounds = new Rectangle(_framedSpecIcon.Left - _pve.Size.X - 10, _framedSpecIcon.Top, 64, 64);
				_templateSlots[TemplateSlotType.Head].Location = new Point(0, _gearCodeBox.Bottom + 25);
				_templateSlots[TemplateSlotType.Shoulder].Location = new Point(_templateSlots[TemplateSlotType.Head].Left, _templateSlots[TemplateSlotType.Head].Bottom + gearSpacing);
				_templateSlots[TemplateSlotType.Chest].Location = new Point(_templateSlots[TemplateSlotType.Shoulder].Left, _templateSlots[TemplateSlotType.Shoulder].Bottom + gearSpacing);
				_templateSlots[TemplateSlotType.Hand].Location = new Point(_templateSlots[TemplateSlotType.Chest].Left, _templateSlots[TemplateSlotType.Chest].Bottom + gearSpacing);
				_templateSlots[TemplateSlotType.Leg].Location = new Point(_templateSlots[TemplateSlotType.Hand].Left, _templateSlots[TemplateSlotType.Hand].Bottom + gearSpacing);
				_templateSlots[TemplateSlotType.Foot].Location = new Point(_templateSlots[TemplateSlotType.Leg].Left, _templateSlots[TemplateSlotType.Leg].Bottom + gearSpacing);
				GearSlot gearSlot = _templateSlots[TemplateSlotType.Nourishment];
				Rectangle bounds = _pve.Bounds;
				gearSlot.Location = new Point(secondColumn, ((Rectangle)(ref bounds)).get_Bottom() + 20);
				_templateSlots[TemplateSlotType.Enhancement].Location = new Point(secondColumn, _templateSlots[TemplateSlotType.Nourishment].Bottom + 5);
				_templateSlots[TemplateSlotType.PowerCore].Location = new Point(secondColumn, _templateSlots[TemplateSlotType.Enhancement].Bottom + 20);
				_templateSlots[TemplateSlotType.PveRelic].Location = new Point(secondColumn, _templateSlots[TemplateSlotType.PowerCore].Bottom + 5);
				_templateSlots[TemplateSlotType.PvpAmulet].Location = new Point(_templateSlots[TemplateSlotType.Leg].Left, _templateSlots[TemplateSlotType.Leg].Bottom + gearSpacing);
				_templateSlots[TemplateSlotType.PvpRelic].Location = new Point(_templateSlots[TemplateSlotType.Hand].Left, _templateSlots[TemplateSlotType.Hand].Bottom + gearSpacing);
				GearSlot gearSlot2 = _templateSlots[TemplateSlotType.MainHand];
				Point val = default(Point);
				((Point)(ref val))._002Ector(_templateSlots[TemplateSlotType.Foot].Left, _templateSlots[TemplateSlotType.Foot].Bottom + 15);
				gearSlot2.Location = val;
				p = val;
				_templateSlots[TemplateSlotType.OffHand].Location = new Point(_templateSlots[TemplateSlotType.MainHand].Left + 4, _templateSlots[TemplateSlotType.MainHand].Bottom + 4);
				s = _templateSlots[TemplateSlotType.MainHand].Size;
				_terrestrialSet.Bounds = new Rectangle(p.X + s.Y / 2 - setSize / 2, p.Y + s.Y - setSize / 2 + 4, setSize, setSize);
				GearSlot gearSlot3 = _templateSlots[TemplateSlotType.AltMainHand];
				((Point)(ref val))._002Ector(_templateSlots[TemplateSlotType.OffHand].Left, _templateSlots[TemplateSlotType.OffHand].Bottom + 35);
				gearSlot3.Location = val;
				p = val;
				_templateSlots[TemplateSlotType.AltOffHand].Location = new Point(_templateSlots[TemplateSlotType.AltMainHand].Left + 4, _templateSlots[TemplateSlotType.AltMainHand].Bottom + 4);
				s = _templateSlots[TemplateSlotType.AltMainHand].Size;
				_alternateTerrestrialSet.Bounds = new Rectangle(p.X + s.Y / 2 - setSize / 2, p.Y + s.Y - setSize / 2 + 4, setSize, setSize);
				_templateSlots[TemplateSlotType.Back].Location = new Point(secondColumn, _templateSlots[TemplateSlotType.PveRelic].Bottom + 20);
				_templateSlots[TemplateSlotType.Accessory_1].Location = new Point(_templateSlots[TemplateSlotType.Back].Right + 3, _templateSlots[TemplateSlotType.Back].Top);
				_templateSlots[TemplateSlotType.Accessory_2].Location = new Point(_templateSlots[TemplateSlotType.Accessory_1].Right + 3, _templateSlots[TemplateSlotType.Back].Top);
				_templateSlots[TemplateSlotType.Amulet].Location = new Point(_templateSlots[TemplateSlotType.Back].Left, _templateSlots[TemplateSlotType.Back].Bottom + 3);
				_templateSlots[TemplateSlotType.Ring_1].Location = new Point(_templateSlots[TemplateSlotType.Amulet].Right + 3, _templateSlots[TemplateSlotType.Amulet].Top);
				_templateSlots[TemplateSlotType.Ring_2].Location = new Point(_templateSlots[TemplateSlotType.Ring_1].Right + 3, _templateSlots[TemplateSlotType.Amulet].Top);
				_templateSlots[TemplateSlotType.AquaBreather].Location = new Point(_templateSlots[TemplateSlotType.Back].Left, _templateSlots[TemplateSlotType.Amulet].Bottom + 20);
				_templateSlots[TemplateSlotType.Aquatic].Location = new Point(_templateSlots[TemplateSlotType.AquaBreather].Left, _templateSlots[TemplateSlotType.AquaBreather].Bottom + 15);
				Rectangle b = _templateSlots[TemplateSlotType.Aquatic].LocalBounds;
				_aquaticSet.Bounds = new Rectangle(((Rectangle)(ref b)).get_Left() + b.Height / 2 - setSize / 2, ((Rectangle)(ref b)).get_Bottom() - setSize / 2, setSize, setSize);
				_templateSlots[TemplateSlotType.AltAquatic].Location = new Point(_templateSlots[TemplateSlotType.Aquatic].Left, _templateSlots[TemplateSlotType.Aquatic].Bottom + 12);
				b = _templateSlots[TemplateSlotType.AltAquatic].LocalBounds;
				_alternateAquaticSet.Bounds = new Rectangle(((Rectangle)(ref b)).get_Left() + b.Height / 2 - setSize / 2, ((Rectangle)(ref b)).get_Bottom() - setSize / 2, setSize, setSize);
			}
		}

		public void ApplyTemplate()
		{
			_blocker.Visible = TemplatePresenter.Template == Template.Empty;
			_gearCodeBox.Text = TemplatePresenter?.Template?.ParseGearCode();
			ProfessionType professionType = TemplatePresenter?.Template?.Profession ?? GameService.Gw2Mumble.PlayerCharacter?.Profession ?? ProfessionType.Guardian;
			SetRaceIcon();
			SetSpecIcon(professionType);
			switch (professionType.GetArmorType())
			{
			case ItemWeightType.Heavy:
				_templateSlots[TemplateSlotType.AquaBreather].Item = Data.Armors[79895];
				_templateSlots[TemplateSlotType.Head].Item = Data.Armors[80384];
				_templateSlots[TemplateSlotType.Shoulder].Item = Data.Armors[80435];
				_templateSlots[TemplateSlotType.Chest].Item = Data.Armors[80254];
				_templateSlots[TemplateSlotType.Hand].Item = Data.Armors[80205];
				_templateSlots[TemplateSlotType.Leg].Item = Data.Armors[80277];
				_templateSlots[TemplateSlotType.Foot].Item = Data.Armors[80557];
				break;
			case ItemWeightType.Medium:
				_templateSlots[TemplateSlotType.AquaBreather].Item = Data.Armors[79838];
				_templateSlots[TemplateSlotType.Head].Item = Data.Armors[80296];
				_templateSlots[TemplateSlotType.Shoulder].Item = Data.Armors[80145];
				_templateSlots[TemplateSlotType.Chest].Item = Data.Armors[80578];
				_templateSlots[TemplateSlotType.Hand].Item = Data.Armors[80161];
				_templateSlots[TemplateSlotType.Leg].Item = Data.Armors[80252];
				_templateSlots[TemplateSlotType.Foot].Item = Data.Armors[80281];
				break;
			case ItemWeightType.Light:
				_templateSlots[TemplateSlotType.AquaBreather].Item = Data.Armors[79873];
				_templateSlots[TemplateSlotType.Head].Item = Data.Armors[80248];
				_templateSlots[TemplateSlotType.Shoulder].Item = Data.Armors[80131];
				_templateSlots[TemplateSlotType.Chest].Item = Data.Armors[80190];
				_templateSlots[TemplateSlotType.Hand].Item = Data.Armors[80111];
				_templateSlots[TemplateSlotType.Leg].Item = Data.Armors[80356];
				_templateSlots[TemplateSlotType.Foot].Item = Data.Armors[80399];
				break;
			}
			Template t = TemplatePresenter.Template;
			_templateSlots[TemplateSlotType.MainHand].Item = t?.MainHand?.Weapon;
			_templateSlots[TemplateSlotType.OffHand].Item = t?.OffHand?.Weapon;
			_templateSlots[TemplateSlotType.Aquatic].Item = t?.Aquatic?.Weapon;
			_templateSlots[TemplateSlotType.AltMainHand].Item = t?.AltMainHand?.Weapon;
			_templateSlots[TemplateSlotType.AltOffHand].Item = t?.AltOffHand?.Weapon;
			_templateSlots[TemplateSlotType.AltAquatic].Item = t?.AltAquatic?.Weapon;
			_templateSlots[TemplateSlotType.PvpAmulet].Item = t?.PvpAmulet?.PvpAmulet;
			_templateSlots[TemplateSlotType.Nourishment].Item = t?.Nourishment?.Nourishment;
			_templateSlots[TemplateSlotType.Enhancement].Item = t?.Enhancement?.Enhancement;
			_templateSlots[TemplateSlotType.PowerCore].Item = t?.PowerCore?.PowerCore;
			_templateSlots[TemplateSlotType.PveRelic].Item = t?.PveRelic?.Relic;
			_templateSlots[TemplateSlotType.PvpRelic].Item = t?.PvpRelic?.Relic;
			SetVisibility();
		}

		private void SetSpecIcon(ProfessionType professionType)
		{
			Kenedia.Modules.BuildsManager.DataModels.Professions.Profession professionForIcon = default(Kenedia.Modules.BuildsManager.DataModels.Professions.Profession);
			_framedSpecIcon.Texture = TemplatePresenter?.Template?.EliteSpecialization?.ProfessionIconBig ?? ((Data.Professions?.TryGetValue((TemplatePresenter?.Template?.Profession).GetValueOrDefault(ProfessionType.Guardian), out professionForIcon) ?? false) ? professionForIcon.IconBig : null);
			Kenedia.Modules.BuildsManager.DataModels.Professions.Profession professionForName = default(Kenedia.Modules.BuildsManager.DataModels.Professions.Profession);
			_framedSpecIcon.BasicTooltipText = TemplatePresenter?.Template?.EliteSpecialization?.Name ?? ((Data.Professions?.TryGetValue((TemplatePresenter?.Template?.Profession).GetValueOrDefault(ProfessionType.Guardian), out professionForName) ?? false) ? professionForName.Name : null);
		}

		private void SetRaceIcon()
		{
			Kenedia.Modules.BuildsManager.DataModels.Race raceIcon = default(Kenedia.Modules.BuildsManager.DataModels.Race);
			_raceIcon.Texture = ((Data.Races?.TryGetValue((TemplatePresenter?.Template?.Race).GetValueOrDefault(Races.None), out raceIcon) ?? false) ? raceIcon.Icon : null);
			Kenedia.Modules.BuildsManager.DataModels.Race raceName = default(Kenedia.Modules.BuildsManager.DataModels.Race);
			_raceIcon.BasicTooltipText = ((Data.Races?.TryGetValue((TemplatePresenter?.Template?.Race).GetValueOrDefault(Races.None), out raceName) ?? false) ? raceName.Name : null);
		}

		private void SetVisibility()
		{
			foreach (GearSlot slot in _templateSlots.Values)
			{
				GearSlot gearSlot = slot;
				bool flag = slot.Slot != TemplateSlotType.AltAquatic;
				if (!flag)
				{
					bool flag2;
					switch (TemplatePresenter.Template?.Profession)
					{
					default:
						flag2 = true;
						break;
					case ProfessionType.Engineer:
					case ProfessionType.Elementalist:
						flag2 = false;
						break;
					}
					flag = flag2;
				}
				bool visible;
				if (flag && TemplatePresenter.IsPvp)
				{
					bool flag2;
					switch (slot.Slot)
					{
					case TemplateSlotType.MainHand:
					case TemplateSlotType.OffHand:
					case TemplateSlotType.AltMainHand:
					case TemplateSlotType.AltOffHand:
					case TemplateSlotType.PvpAmulet:
					case TemplateSlotType.PvpRelic:
						flag2 = true;
						break;
					default:
						flag2 = false;
						break;
					}
					visible = flag2;
				}
				else
				{
					TemplateSlotType slot2 = slot.Slot;
					visible = slot2 != TemplateSlotType.PvpAmulet && slot2 != TemplateSlotType.PvpRelic;
				}
				gearSlot.Visible = visible;
			}
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			if (Data.IsLoaded)
			{
				base.Draw(spriteBatch, drawBounds, scissor);
				return;
			}
			Rectangle scissorRectangle = Rectangle.Intersect(scissor, base.AbsoluteBounds.WithPadding(_padding)).ScaleBy(Control.Graphics.UIScaleMultiplier);
			((GraphicsResource)spriteBatch).get_GraphicsDevice().set_ScissorRectangle(scissorRectangle);
			base.EffectBehind?.Draw(spriteBatch, drawBounds);
			spriteBatch.Begin(base.SpriteBatchParameters);
			Rectangle r = default(Rectangle);
			((Rectangle)(ref r))._002Ector(((Rectangle)(ref drawBounds)).get_Center().X - 32, ((Rectangle)(ref drawBounds)).get_Center().Y, 64, 64);
			Rectangle tR = default(Rectangle);
			((Rectangle)(ref tR))._002Ector(drawBounds.X, ((Rectangle)(ref r)).get_Bottom() + 10, drawBounds.Width, Control.Content.DefaultFont16.get_LineHeight());
			LoadingSpinnerUtil.DrawLoadingSpinner(this, spriteBatch, r);
			spriteBatch.DrawStringOnCtrl(this, (!Data.IsLoaded) ? "Loading Data. Please wait." : "Select or create a template", Control.Content.DefaultFont16, tR, Color.get_White(), wrap: false, HorizontalAlignment.Center);
			spriteBatch.End();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			if (TemplatePresenter.Template != null)
			{
				(TemplatePresenter.IsPve ? _pve : _pvp).Draw(this, spriteBatch, base.RelativeMousePosition);
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			if (TemplatePresenter.Template == null)
			{
				return;
			}
			_terrestrialSet.Draw(this, spriteBatch);
			_alternateTerrestrialSet.Draw(this, spriteBatch);
			if (TemplatePresenter.IsPve)
			{
				_aquaticSet.Draw(this, spriteBatch);
				if (_templateSlots[TemplateSlotType.AltAquatic].Visible)
				{
					_alternateAquaticSet.Draw(this, spriteBatch);
				}
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			if (((TemplatePresenter.Template != null && TemplatePresenter.IsPve) ? _pve : _pvp).Hovered)
			{
				TemplatePresenter.GameMode = (TemplatePresenter.IsPve ? GameModeType.PvP : GameModeType.PvE);
				SetVisibility();
				return;
			}
			if (_framedSpecIcon?.MouseOver ?? false)
			{
				_professionRaceSelection.Visible = true;
				_professionRaceSelection.Type = ProfessionRaceSelection.SelectionType.Profession;
				_professionRaceSelection.Location = base.RelativeMousePosition;
				_professionRaceSelection.OnClickAction = delegate(object value)
				{
					TemplatePresenter?.Template?.SetProfession((ProfessionType)value);
				};
				_professionRaceSelection.ZIndex = ZIndex + 10;
			}
			else if (_raceIcon?.MouseOver ?? false)
			{
				_professionRaceSelection.Visible = true;
				_professionRaceSelection.Type = ProfessionRaceSelection.SelectionType.Race;
				_professionRaceSelection.Location = base.RelativeMousePosition;
				_professionRaceSelection.OnClickAction = delegate(object value)
				{
					TemplatePresenter?.Template?.SetRace((Races)value);
				};
				_professionRaceSelection.ZIndex = ZIndex + 10;
			}
			base.OnClick(e);
		}

		private void TemplateChanged(object sender, PropertyChangedEventArgs e)
		{
			ApplyTemplate();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_templateSlots?.Values?.DisposeAll();
			_templateSlots?.Clear();
			_pve?.Dispose();
			_pvp?.Dispose();
		}
	}
}
