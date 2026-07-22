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
			base.RecalculateLayout();
			_terrestrialTexture.Bounds = new Rectangle(5, 2, 42, 42);
			_aquaticTexture.Bounds = new Rectangle(_terrestrialTexture.Bounds.Right + _skillSize * 5 + 20, 2, 42, 42);
			Point size = new Point(_skillSize, _skillSize + 15);
			foreach (KeyValuePair<SkillSlotType, SkillSlotControl> spair in Skills)
			{
				int left = (spair.Key.IsTerrestrial() ? _terrestrialTexture.Bounds.Right : _aquaticTexture.Bounds.Right) + 5;
				int xOffset = spair.Key.GetSlotPosition() * size.X;
				Skills[spair.Key].SetBounds(new Rectangle(left + xOffset, 0, size.X, size.Y));
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
			_terrestrialTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.White);
			_aquaticTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.White);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (_skillSelector.Visible && _skillSelector.Anchor == null)
			{
				_skillSelector.Hide();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Skills.Values?.DisposeAll();
			_skillSelector?.Dispose();
		}
	}
}
