using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.Controls.Selectables;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SkillSlotControl : Control
	{
		public DetailedTexture Selector { get; } = new DetailedTexture(157138, 157140);


		public AsyncTexture2D Texture => TexturesService.GetAsyncTexture(Skill?.IconAssetId);

		public AsyncTexture2D HoveredFrameTexture { get; } = AsyncTexture2D.FromAssetId(157143);


		public AsyncTexture2D HoveredTexture { get; } = AsyncTexture2D.FromAssetId(157143);


		public AsyncTexture2D FallBackTexture { get; } = AsyncTexture2D.FromAssetId(157154);


		public AsyncTexture2D NoAquaticFlagTexture { get; } = AsyncTexture2D.FromAssetId(157145);


		public Microsoft.Xna.Framework.Rectangle TextureRegion { get; } = new Microsoft.Xna.Framework.Rectangle(14, 14, 100, 100);


		public Microsoft.Xna.Framework.Rectangle NoAquaticFlagTextureRegion { get; } = new Microsoft.Xna.Framework.Rectangle(16, 16, 96, 96);


		public Microsoft.Xna.Framework.Rectangle FallbackRegion { get; }

		public Microsoft.Xna.Framework.Rectangle FallbackBounds { get; private set; }

		public Microsoft.Xna.Framework.Rectangle SkillBounds { get; private set; }

		public Microsoft.Xna.Framework.Rectangle SelectorBounds { get; private set; }

		public Microsoft.Xna.Framework.Rectangle HoveredFrameTextureRegion { get; } = new Microsoft.Xna.Framework.Rectangle(8, 8, 112, 112);


		public Microsoft.Xna.Framework.Rectangle AutoCastTextureRegion { get; } = new Microsoft.Xna.Framework.Rectangle(6, 6, 52, 52);


		public SkillTooltip SkillTooltip { get; }

		public SkillSlotType SkillSlot { get; }

		public TemplatePresenter TemplatePresenter { get; }

		public Data Data { get; }

		public SkillSelector SkillSelector { get; }

		public Vector2 Origin { get; private set; } = Vector2.Zero;


		public float Rotation { get; private set; }

		public Microsoft.Xna.Framework.Color? BackgroundDrawColor { get; private set; }

		public Microsoft.Xna.Framework.Color Color { get; private set; }

		public Microsoft.Xna.Framework.Color? HoverDrawColor { get; private set; }

		public Microsoft.Xna.Framework.Color? DrawColor { get; private set; }

		public bool ShowSelector { get; set; }

		public bool IsSelectorHovered
		{
			get
			{
				if (ShowSelector)
				{
					return SelectorBounds.Contains(base.RelativeMousePosition);
				}
				return false;
			}
		}

		public Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? Skill
		{
			[CompilerGenerated]
			get
			{
				return _003CSkill_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSkill_003Ek__BackingField, value, delegate(Kenedia.Modules.BuildsManager.DataModels.Professions.Skill v)
				{
					_003CSkill_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>(OnSkillChanged));
			}
		}

		public SkillSlotControl(SkillSlotType skillSlot, TemplatePresenter templatePresenter, Data data, SkillSelector skillSelector)
		{
			SkillSlot = skillSlot;
			TemplatePresenter = templatePresenter;
			Data = data;
			SkillSelector = skillSelector;
			base.Tooltip = (SkillTooltip = new SkillTooltip());
			base.Size = new Point(64);
			TemplatePresenter.TemplateChanged += new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
			TemplatePresenter.SkillChanged += new SkillChangedEventHandler(TemplatePresenter_SkillChanged);
			Skill = TemplatePresenter?.Template?[SkillSlot];
		}

		private void TemplatePresenter_TemplateChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Template> e)
		{
			Skill = TemplatePresenter?.Template?[SkillSlot];
		}

		private void TemplatePresenter_SkillChanged(object sender, SkillChangedEventArgs e)
		{
			Skill = TemplatePresenter?.Template?[SkillSlot];
		}

		private void OnSkillChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> e)
		{
			SkillTooltip.Skill = e.NewValue;
		}

		protected override void Paint(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle bounds)
		{
			bool num = SkillSlot.IsTerrestrial();
			Microsoft.Xna.Framework.Color? backgroundDrawColor = BackgroundDrawColor;
			if (backgroundDrawColor.HasValue)
			{
				SpriteBatchExtensions.DrawOnCtrl(color: backgroundDrawColor.GetValueOrDefault(), spriteBatch: spriteBatch, ctrl: this, texture: ContentService.Textures.Pixel, destinationRectangle: SkillBounds, sourceRectangle: Microsoft.Xna.Framework.Rectangle.Empty, rotation: Rotation, origin: Origin);
			}
			bool hovered = base.MouseOver && SkillBounds.Contains(base.RelativeMousePosition);
			if (FallBackTexture != null || Texture != null)
			{
				Color = ((hovered && HoverDrawColor.HasValue) ? HoverDrawColor : DrawColor) ?? Microsoft.Xna.Framework.Color.White;
				Color = Microsoft.Xna.Framework.Color.White;
				if (HoveredTexture != null && hovered)
				{
					spriteBatch.DrawOnCtrl(this, HoveredTexture, SkillBounds, TextureRegion, Color, Rotation, Origin);
				}
				if (Texture != null)
				{
					spriteBatch.DrawOnCtrl(this, Texture, SkillBounds, TextureRegion, Color, Rotation, Origin);
				}
				else
				{
					spriteBatch.DrawOnCtrl(this, FallBackTexture, (FallbackBounds == Microsoft.Xna.Framework.Rectangle.Empty) ? SkillBounds : FallbackBounds, SkillBounds, Color, Rotation, Origin);
				}
			}
			Microsoft.Xna.Framework.Color borderColor = Microsoft.Xna.Framework.Color.Black;
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Microsoft.Xna.Framework.Rectangle(SkillBounds.Left, SkillBounds.Top, SkillBounds.Width, 1), Microsoft.Xna.Framework.Rectangle.Empty, borderColor * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Microsoft.Xna.Framework.Rectangle(SkillBounds.Left, SkillBounds.Bottom - 1, SkillBounds.Width, 1), Microsoft.Xna.Framework.Rectangle.Empty, borderColor * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Microsoft.Xna.Framework.Rectangle(SkillBounds.Left, SkillBounds.Top, 1, SkillBounds.Height), Microsoft.Xna.Framework.Rectangle.Empty, borderColor * 0.6f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Microsoft.Xna.Framework.Rectangle(SkillBounds.Right - 1, SkillBounds.Top, 1, SkillBounds.Height), Microsoft.Xna.Framework.Rectangle.Empty, borderColor * 0.6f);
			if (!num)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? skill = Skill;
				if (skill != null && skill!.Flags.HasFlag(SkillFlag.NoUnderwater))
				{
					spriteBatch.DrawOnCtrl(this, NoAquaticFlagTexture, SkillBounds, NoAquaticFlagTextureRegion, Color, Rotation, Origin);
					goto IL_038d;
				}
			}
			if (hovered && HoveredFrameTexture != null)
			{
				spriteBatch.DrawOnCtrl(this, HoveredFrameTexture, SkillBounds, HoveredFrameTextureRegion, Color, Rotation, Origin);
			}
			goto IL_038d;
			IL_038d:
			if (ShowSelector)
			{
				Selector.Draw(this, spriteBatch, base.RelativeMousePosition);
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int selectorHeight = 15;
			SkillBounds = new Microsoft.Xna.Framework.Rectangle(new Point(0, selectorHeight - 2), new Point(base.Width, base.Height - selectorHeight));
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			base.Tooltip?.Dispose();
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			SetSelector();
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			base.OnRightMouseButtonPressed(e);
			SetSelector();
		}

		private void SetSelector()
		{
			SkillSelector.Anchor = this;
			SkillSelector.AnchorOffset = new Point(-2, 10);
			SkillSelector.ZIndex = Selector<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>.GetAnchorRootZIndex(this) + 1000;
			SkillSelector.SelectedItem = Skill;
			SkillSlotType slot = SkillSlot;
			SkillSelector.Label = strings.ResourceManager.GetString(Regex.Replace((slot & ~(SkillSlotType.Active | SkillSlotType.Inactive | SkillSlotType.Terrestrial | SkillSlotType.Aquatic)).ToString().Trim() ?? "", "[_0-9]", "") + "Skills");
			SkillSelector.Enviroment = ((!SkillSlot.HasFlag(SkillSlotType.Aquatic)) ? Enviroment.Terrestrial : Enviroment.Aquatic);
			SkillSelector.OnClickAction = delegate(Kenedia.Modules.BuildsManager.DataModels.Professions.Skill skill)
			{
				TemplatePresenter?.Template.SetSkill(SkillSlot, skill);
				SkillSelector.Hide();
			};
			GetSelectableSkills(SkillSlot);
			SkillSelector.Show();
		}

		private void GetSelectableSkills(SkillSlotType skillSlot)
		{
			if (((TemplatePresenter?.Template?.Profession ?? ((ProfessionType)0)) == (ProfessionType)0) ? true : false)
			{
				return;
			}
			SkillSlot slot = (skillSlot.HasFlag(SkillSlotType.Utility_1) ? Gw2Sharp.WebApi.V2.Models.SkillSlot.Utility : (skillSlot.HasFlag(SkillSlotType.Utility_2) ? Gw2Sharp.WebApi.V2.Models.SkillSlot.Utility : (skillSlot.HasFlag(SkillSlotType.Utility_3) ? Gw2Sharp.WebApi.V2.Models.SkillSlot.Utility : (skillSlot.HasFlag(SkillSlotType.Heal) ? Gw2Sharp.WebApi.V2.Models.SkillSlot.Heal : Gw2Sharp.WebApi.V2.Models.SkillSlot.Elite))));
			TemplatePresenter templatePresenter = TemplatePresenter;
			if (templatePresenter == null || templatePresenter.Template?.Profession != ProfessionType.Revenant)
			{
				BuildSpecialization slot2;
				List<KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>> filteredSkills2 = Data.Professions[TemplatePresenter.Template.Profession].Skills.Where((KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> e) => e.Value.PaletteId > 0 && e.Value.Slot.HasValue && e.Value.Slot == slot && (e.Value.Specialization == 0 || TemplatePresenter.Template.HasSpecialization(e.Value.Specialization, out slot2))).ToList();
				List<KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>> racialSkills = ((TemplatePresenter.Template.Race == Races.None) ? new List<KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>>() : Data.Races[TemplatePresenter.Template.Race]?.Skills.Where<KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>>((KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> e) => e.Value.PaletteId > 0 && e.Value.Slot.HasValue && e.Value.Slot == slot).ToList());
				if (racialSkills != null)
				{
					filteredSkills2.AddRange(racialSkills);
				}
				SkillSelector.SetItems(from e in filteredSkills2
					orderby e.Value.Categories
					select e.Value);
				return;
			}
			List<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> filteredSkills = new List<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>();
			SkillSlotType enviromentState = skillSlot.GetEnviromentState();
			Kenedia.Modules.BuildsManager.DataModels.Professions.Legend skills = TemplatePresenter.Template?.Legends[enviromentState switch
			{
				SkillSlotType.Active | SkillSlotType.Aquatic => LegendSlotType.AquaticActive, 
				SkillSlotType.Inactive | SkillSlotType.Aquatic => LegendSlotType.AquaticInactive, 
				SkillSlotType.Active | SkillSlotType.Terrestrial => LegendSlotType.TerrestrialActive, 
				SkillSlotType.Inactive | SkillSlotType.Terrestrial => LegendSlotType.TerrestrialInactive, 
				_ => LegendSlotType.TerrestrialActive, 
			}];
			if (skills != null)
			{
				switch (slot)
				{
				case Gw2Sharp.WebApi.V2.Models.SkillSlot.Heal:
					filteredSkills.Add(skills.Heal);
					break;
				case Gw2Sharp.WebApi.V2.Models.SkillSlot.Elite:
					filteredSkills.Add(skills.Elite);
					break;
				case Gw2Sharp.WebApi.V2.Models.SkillSlot.Utility:
					filteredSkills.AddRange(skills.Utilities.Select<KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>((KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> e) => e.Value));
					break;
				}
			}
			SkillSelector.SetItems(filteredSkills);
		}
	}
}
