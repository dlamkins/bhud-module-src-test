using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.Controls.Selectables;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific
{
	public class RevenantSpecifics : ProfessionSpecifics
	{
		private readonly Dictionary<LegendSlotType, LegendIcon> _legends = new Dictionary<LegendSlotType, LegendIcon>
		{
			{
				LegendSlotType.TerrestrialActive,
				new LegendIcon
				{
					LegendSlot = LegendSlotType.TerrestrialActive
				}
			},
			{
				LegendSlotType.TerrestrialInactive,
				new LegendIcon
				{
					LegendSlot = LegendSlotType.TerrestrialInactive
				}
			},
			{
				LegendSlotType.AquaticActive,
				new LegendIcon
				{
					LegendSlot = LegendSlotType.AquaticActive
				}
			},
			{
				LegendSlotType.AquaticInactive,
				new LegendIcon
				{
					LegendSlot = LegendSlotType.AquaticInactive
				}
			}
		};

		private readonly LegendSelector _legendSelector;

		private LegendSlotType _selectedLegendSlot;

		private LegendIcon _selectorAnchor;

		public ImageButton AquaticSwapButton { get; }

		public ImageButton TerrestrialSwapButton { get; }

		public RevenantSpecifics(TemplatePresenter template)
			: base(template)
		{
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			_legendSelector = new LegendSelector
			{
				Parent = Control.Graphics.SpriteScreen,
				Visible = false,
				OnClickAction = delegate(Legend legend)
				{
					base.TemplatePresenter.Template?.SetLegend(_selectedLegendSlot, legend);
					_legendSelector.Hide();
				}
			};
			foreach (LegendIcon value in _legends.Values)
			{
				value.Parent = this;
				value.LeftClickAction = new Action<LegendIcon>(SetSelector);
				value.RightClickAction = new Action<LegendIcon>(SetSelector);
			}
			AquaticSwapButton = new ImageButton
			{
				Parent = this,
				ClickAction = delegate
				{
					base.TemplatePresenter.SwapLegend();
				},
				Texture = AsyncTexture2D.FromAssetId(784346),
				ColorHovered = ContentService.Colors.ColonialWhite
			};
			TerrestrialSwapButton = new ImageButton
			{
				Parent = this,
				ClickAction = delegate
				{
					base.TemplatePresenter.SwapLegend();
				},
				Texture = AsyncTexture2D.FromAssetId(784346),
				ColorHovered = ContentService.Colors.ColonialWhite
			};
			base.TemplatePresenter.LegendSlotChanged += new ValueChangedEventHandler<LegendSlotType>(TemplatePresenter_LegendSlotChanged);
			ApplyCurrentLegendSlot();
		}

		private void TemplatePresenter_LegendSlotChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<LegendSlotType> e)
		{
			ApplyCurrentLegendSlot();
		}

		private void ApplyCurrentLegendSlot()
		{
			foreach (var item2 in from _003C_003Eh__TransparentIdentifier0 in Enumerable.Select(_legends, delegate(KeyValuePair<LegendSlotType, LegendIcon> legend)
				{
					LegendSlotType legendSlot = base.TemplatePresenter.LegendSlot;
					bool isActive2 = ((legendSlot == LegendSlotType.AquaticActive || legendSlot == LegendSlotType.TerrestrialActive) ? true : false);
					return new
					{
						legend = legend,
						isActive = isActive2
					};
				})
				select (_003C_003Eh__TransparentIdentifier0.legend, _003C_003Eh__TransparentIdentifier0.isActive))
			{
				KeyValuePair<LegendSlotType, LegendIcon> legend2 = item2.Item1;
				bool item = item2.Item2;
				LegendIcon value = legend2.Value;
				bool isActive;
				if (item)
				{
					LegendSlotType key = legend2.Key;
					bool flag = ((key == LegendSlotType.AquaticActive || key == LegendSlotType.TerrestrialActive) ? true : false);
					isActive = flag;
				}
				else
				{
					LegendSlotType key = legend2.Key;
					bool flag = ((key == LegendSlotType.AquaticInactive || key == LegendSlotType.TerrestrialInactive) ? true : false);
					isActive = flag;
				}
				value.IsActive = isActive;
			}
		}

		private void SetSelector(LegendIcon icon)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			_selectorAnchor = icon;
			_legendSelector.Anchor = icon;
			_legendSelector.ZIndex = ZIndex + 1000;
			_legendSelector.SelectedItem = icon.Legend;
			_legendSelector.LegendSlot = icon.LegendSlot;
			_legendSelector.AnchorOffset = new Point(0, 0);
			LegendSelector legendSelector = _legendSelector;
			legendSelector.Label = icon.LegendSlot switch
			{
				LegendSlotType.TerrestrialActive => "Terrestrial Active", 
				LegendSlotType.TerrestrialInactive => "Terrestrial Inactive", 
				LegendSlotType.AquaticActive => "Aquatic Active", 
				LegendSlotType.AquaticInactive => "Aquatic Inactive", 
				_ => "", 
			};
			GetSelectableLegends(icon.LegendSlot);
		}

		public override void RecalculateLayout()
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			if (TerrestrialSwapButton != null)
			{
				base.RecalculateLayout();
				int xOffset = 90;
				_legends[LegendSlotType.TerrestrialInactive].SetLocation(xOffset, 45);
				int legendSize = _legends[LegendSlotType.TerrestrialInactive].Width;
				TerrestrialSwapButton.SetBounds(new Rectangle(xOffset + 2 + legendSize - (int)((double)legendSize / 1.5) / 2, 40 + legendSize / 2, (int)((double)legendSize / 1.5), (int)((double)legendSize / 1.5)));
				_legends[LegendSlotType.TerrestrialActive].SetLocation(xOffset + 5 + legendSize, 45);
				xOffset += 10 + legendSize + 320;
				_legends[LegendSlotType.AquaticInactive].SetLocation(xOffset, 45);
				AquaticSwapButton.SetBounds(new Rectangle(xOffset + 2 + legendSize - (int)((double)legendSize / 1.5) / 2, 40 + legendSize / 2, (int)((double)legendSize / 1.5), (int)((double)legendSize / 1.5)));
				_legends[LegendSlotType.AquaticActive].SetLocation(xOffset + 5 + legendSize, 45);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
		}

		protected override void ApplyTemplate()
		{
			if (base.TemplatePresenter?.Template == null)
			{
				return;
			}
			base.ApplyTemplate();
			foreach (LegendSlotType slot in Enum.GetValues(typeof(LegendSlotType)))
			{
				_legends[slot].Legend = base.TemplatePresenter?.Template?.Legends?[slot];
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			Rectangle r = _legends[LegendSlotType.TerrestrialActive].LocalBounds;
			spriteBatch.DrawStringOnCtrl(this, (base.TemplatePresenter.LegendSlot == LegendSlotType.TerrestrialActive) ? strings.ActiveLegend : strings.InactiveLegend, Control.Content.DefaultFont16, new Rectangle(((Rectangle)(ref r)).get_Right() + 5, ((Rectangle)(ref r)).get_Center().Y - Control.Content.DefaultFont14.get_LineHeight() / 2, 100, Control.Content.DefaultFont16.get_LineHeight()), Color.get_White());
			r = _legends[LegendSlotType.AquaticActive].LocalBounds;
			spriteBatch.DrawStringOnCtrl(this, (base.TemplatePresenter.LegendSlot == LegendSlotType.TerrestrialActive) ? strings.ActiveLegend : strings.InactiveLegend, Control.Content.DefaultFont16, new Rectangle(((Rectangle)(ref r)).get_Right() + 5, ((Rectangle)(ref r)).get_Center().Y - Control.Content.DefaultFont14.get_LineHeight() / 2, 100, Control.Content.DefaultFont16.get_LineHeight()), Color.get_White());
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_legendSelector?.Dispose();
		}

		private LegendSlotType GetOtherSlot(LegendSlotType? slot = null)
		{
			LegendSlotType valueOrDefault = slot.GetValueOrDefault();
			if (!slot.HasValue)
			{
				valueOrDefault = base.TemplatePresenter.LegendSlot;
				slot = valueOrDefault;
			}
			return slot switch
			{
				LegendSlotType.AquaticActive => LegendSlotType.AquaticInactive, 
				LegendSlotType.AquaticInactive => LegendSlotType.AquaticActive, 
				LegendSlotType.TerrestrialActive => LegendSlotType.TerrestrialInactive, 
				LegendSlotType.TerrestrialInactive => LegendSlotType.TerrestrialActive, 
				null => throw new NotImplementedException(), 
				_ => throw new NotImplementedException(), 
			};
		}

		private void GetSelectableLegends(LegendSlotType legendSlot)
		{
			_selectedLegendSlot = legendSlot;
			IEnumerable<Legend> legends = from e in BuildsManager.Data.Professions[ProfessionType.Revenant].Legends
				where e.Value.Specialization == 0 || e.Value.Specialization == base.TemplatePresenter.Template.EliteSpecialization?.Id
				select e.Value;
			_legendSelector.SetItems(legends);
		}
	}
}
