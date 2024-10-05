using System.Collections.Generic;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.Controls.Selectables;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SkillsBar : Container
	{
		private readonly int _skillSize = 64;

		private readonly DetailedTexture _selectingFrame = new DetailedTexture(157147);

		private readonly DetailedTexture _aquaticTexture = new DetailedTexture(1988170);

		private readonly DetailedTexture _terrestrialTexture = new DetailedTexture(1988171);

		private readonly SkillSelector _skillSelector;

		public Dictionary<SkillSlotType, SkillSlotControl> Skills { get; } = new Dictionary<SkillSlotType, SkillSlotControl>();


		public TemplatePresenter TemplatePresenter { get; }

		public Data Data { get; }

		public SkillsBar(TemplatePresenter templatePresenter, Data data)
		{
			TemplatePresenter = templatePresenter;
			Data = data;
			base.Height = 80;
			base.Width = 500;
			_skillSelector = new SkillSelector
			{
				Parent = Control.Graphics.SpriteScreen,
				Visible = false
			};
			SkillSlotType[] enviroments = new SkillSlotType[2]
			{
				SkillSlotType.Terrestrial,
				SkillSlotType.Aquatic
			};
			SkillSlotType[] states = new SkillSlotType[2]
			{
				SkillSlotType.Active,
				SkillSlotType.Inactive
			};
			SkillSlotType[] slots = new SkillSlotType[5]
			{
				SkillSlotType.Heal,
				SkillSlotType.Utility_1,
				SkillSlotType.Utility_2,
				SkillSlotType.Utility_3,
				SkillSlotType.Elite
			};
			SkillSlotType[] array = states;
			foreach (SkillSlotType state in array)
			{
				SkillSlotType[] array2 = enviroments;
				foreach (SkillSlotType enviroment in array2)
				{
					SkillSlotType[] array3 = slots;
					for (int k = 0; k < array3.Length; k++)
					{
						SkillSlotType skillSlot = array3[k] | state | enviroment;
						Skills[skillSlot] = new SkillSlotControl(skillSlot, templatePresenter, data, _skillSelector)
						{
							Parent = this,
							ShowSelector = true
						};
					}
				}
			}
			TemplatePresenter.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(TemplatePresenter_ProfessionChanged);
			TemplatePresenter.TemplateChanged += new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
			TemplatePresenter.LegendSlotChanged += new ValueChangedEventHandler<LegendSlotType>(TemplatePresenter_LegendSlotChanged);
			SetSkillsVisibility();
		}

		private void TemplatePresenter_LegendSlotChanged(object sender, ValueChangedEventArgs<LegendSlotType> e)
		{
			SetSkillsVisibility();
		}

		private void TemplatePresenter_TemplateChanged(object sender, ValueChangedEventArgs<Template> e)
		{
			SetSkillsVisibility();
		}

		private void TemplatePresenter_ProfessionChanged(object sender, ValueChangedEventArgs<ProfessionType> e)
		{
			SetSkillsVisibility();
		}

		private void SetSkillsVisibility()
		{
			ProfessionType? professionType = TemplatePresenter.Template?.Profession;
			bool flag = professionType.HasValue && professionType.GetValueOrDefault() == ProfessionType.Revenant;
			if (flag)
			{
				LegendSlotType legendSlot = TemplatePresenter.LegendSlot;
				bool flag2 = ((legendSlot == LegendSlotType.AquaticInactive || legendSlot == LegendSlotType.TerrestrialInactive) ? true : false);
				flag = flag2;
			}
			SkillSlotType state = ((!flag) ? SkillSlotType.Active : SkillSlotType.Inactive);
			foreach (KeyValuePair<SkillSlotType, SkillSlotControl> skill in Skills)
			{
				skill.Value.Visible = skill.Key.HasFlag(state);
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_terrestrialTexture.Bounds = new Rectangle(5, 2, 42, 42);
			DetailedTexture aquaticTexture = _aquaticTexture;
			Rectangle bounds = _terrestrialTexture.Bounds;
			aquaticTexture.Bounds = new Rectangle(((Rectangle)(ref bounds)).get_Right() + _skillSize * 5 + 20, 2, 42, 42);
			Point size = default(Point);
			((Point)(ref size))._002Ector(_skillSize, _skillSize + 15);
			foreach (KeyValuePair<SkillSlotType, SkillSlotControl> spair in Skills)
			{
				int right;
				if (!spair.Key.IsTerrestrial())
				{
					bounds = _aquaticTexture.Bounds;
					right = ((Rectangle)(ref bounds)).get_Right();
				}
				else
				{
					bounds = _terrestrialTexture.Bounds;
					right = ((Rectangle)(ref bounds)).get_Right();
				}
				int left = right + 5;
				int xOffset = spair.Key.GetSlotPosition() * size.X;
				Skills[spair.Key].SetBounds(new Rectangle(left + xOffset, 0, size.X, size.Y));
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			_terrestrialTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.get_White());
			_aquaticTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.get_White());
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Skills.Values?.DisposeAll();
		}
	}
}
