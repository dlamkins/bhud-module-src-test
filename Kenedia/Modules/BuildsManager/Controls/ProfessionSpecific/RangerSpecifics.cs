using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Controls.Selectables;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific
{
	public class RangerSpecifics : ProfessionSpecifics
	{
		private readonly PetSelector _petSelector;

		private readonly Dictionary<PetSlotType, PetControl> _pets = new Dictionary<PetSlotType, PetControl>
		{
			{
				PetSlotType.Terrestrial_1,
				new PetControl
				{
					PetSlot = PetSlotType.Terrestrial_1
				}
			},
			{
				PetSlotType.Terrestrial_2,
				new PetControl
				{
					PetSlot = PetSlotType.Terrestrial_2
				}
			},
			{
				PetSlotType.Aquatic_1,
				new PetControl
				{
					PetSlot = PetSlotType.Aquatic_1
				}
			},
			{
				PetSlotType.Aquatic_2,
				new PetControl
				{
					PetSlot = PetSlotType.Aquatic_2
				}
			}
		};

		private Point _petSize = new Point(120);

		private PetControl? _selectorAnchor;

		protected override SkillIcon[] Skills { get; } = new SkillIcon[5]
		{
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon(),
			new SkillIcon()
		};


		public RangerSpecifics(TemplatePresenter template)
			: base(template)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			foreach (KeyValuePair<PetSlotType, PetControl> pet2 in _pets)
			{
				pet2.Value.Parent = this;
				pet2.Value.RightClickAction = new Action<PetControl>(Pet_Click);
				pet2.Value.LeftClickAction = new Action<PetControl>(Pet_Click);
			}
			_petSelector = new PetSelector
			{
				Parent = Control.Graphics.SpriteScreen,
				Visible = false,
				OnClickAction = delegate(Pet pet)
				{
					PetSlotType slot = _selectorAnchor!.PetSlot switch
					{
						PetSlotType.Aquatic_1 => PetSlotType.Aquatic_2, 
						PetSlotType.Aquatic_2 => PetSlotType.Aquatic_1, 
						PetSlotType.Terrestrial_1 => PetSlotType.Terrestrial_2, 
						_ => PetSlotType.Terrestrial_1, 
					};
					if (base.TemplatePresenter.Template.Pets[slot] != pet)
					{
						_selectorAnchor!.Pet = pet;
						base.TemplatePresenter.Template.SetPet(_selectorAnchor!.PetSlot, pet);
					}
					_petSelector?.Hide();
				}
			};
		}

		private void Pet_Click(PetControl sender)
		{
			SetSelector(sender);
		}

		private void Mouse_RightMouseButtonPressed(object sender, MouseEventArgs e)
		{
		}

		public override void RecalculateLayout()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int xOffset = 60;
			_pets[PetSlotType.Terrestrial_1].SetBounds(new Rectangle(xOffset, 0, _petSize.X, _petSize.Y));
			_pets[PetSlotType.Terrestrial_2].SetBounds(new Rectangle(xOffset + 125, 0, _petSize.X, _petSize.Y));
			xOffset = 439;
			_pets[PetSlotType.Aquatic_1].SetBounds(new Rectangle(xOffset, 0, _petSize.X, _petSize.Y));
			_pets[PetSlotType.Aquatic_2].SetBounds(new Rectangle(xOffset + 125, 0, _petSize.X, _petSize.Y));
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
		}

		private void SetSelector(PetControl skillIcon)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			_selectorAnchor = skillIcon;
			_petSelector.Anchor = skillIcon;
			_petSelector.ZIndex = ZIndex + 1000;
			_petSelector.SelectedItem = skillIcon.Pet;
			_petSelector.AnchorOffset = new Point(0, 20);
			PetSelector petSelector = _petSelector;
			string label;
			switch (skillIcon.PetSlot)
			{
			case PetSlotType.Terrestrial_1:
			case PetSlotType.Terrestrial_2:
				label = strings.TerrestrialPets;
				break;
			case PetSlotType.Aquatic_1:
			case PetSlotType.Aquatic_2:
				label = strings.AquaticPets;
				break;
			default:
				label = string.Empty;
				break;
			}
			petSelector.Label = label;
			GetSelectablePets();
		}

		protected override void ApplyTemplate()
		{
			base.ApplyTemplate();
			if (base.TemplatePresenter == null)
			{
				return;
			}
			foreach (PetControl pet in _pets.Values)
			{
				pet.Pet = base.TemplatePresenter?.Template?.Pets?[pet.PetSlot];
			}
		}

		private void GetSelectablePets()
		{
			if (base.TemplatePresenter != null)
			{
				PetSlotType petSlot = _selectorAnchor!.PetSlot;
				bool flag2 = (((uint)(petSlot - 2) <= 1u) ? true : false);
				Enviroment flag = (flag2 ? Enviroment.Terrestrial : Enviroment.Aquatic);
				_petSelector.SetItems(from e in BuildsManager.Data.Pets.Values
					where e.Enviroment.HasFlag(flag)
					orderby e.Order
					select e);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_petSelector?.Dispose();
			Control.Input.Mouse.RightMouseButtonPressed -= Mouse_RightMouseButtonPressed;
		}
	}
}
