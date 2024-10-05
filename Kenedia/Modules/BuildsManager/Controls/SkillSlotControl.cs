using System.Collections.Generic;
using System.Linq;
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
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SkillSlotControl : Control
	{
		private Kenedia.Modules.BuildsManager.DataModels.Professions.Skill _skill;

		public DetailedTexture Selector { get; } = new DetailedTexture(157138, 157140);


		public AsyncTexture2D Texture => Skill?.Icon;

		public AsyncTexture2D HoveredFrameTexture { get; } = AsyncTexture2D.FromAssetId(157143);


		public AsyncTexture2D HoveredTexture { get; } = AsyncTexture2D.FromAssetId(157143);


		public AsyncTexture2D FallBackTexture { get; } = AsyncTexture2D.FromAssetId(157154);


		public AsyncTexture2D NoAquaticFlagTexture { get; } = AsyncTexture2D.FromAssetId(157145);


		public Rectangle TextureRegion { get; } = new Rectangle(14, 14, 100, 100);


		public Rectangle NoAquaticFlagTextureRegion { get; } = new Rectangle(16, 16, 96, 96);


		public Rectangle FallbackRegion { get; }

		public Rectangle FallbackBounds { get; private set; }

		public Rectangle SkillBounds { get; private set; }

		public Rectangle SelectorBounds { get; private set; }

		public Rectangle HoveredFrameTextureRegion { get; } = new Rectangle(8, 8, 112, 112);


		public Rectangle AutoCastTextureRegion { get; } = new Rectangle(6, 6, 52, 52);


		public SkillTooltip SkillTooltip { get; }

		public SkillSlotType SkillSlot { get; }

		public TemplatePresenter TemplatePresenter { get; }

		public Data Data { get; }

		public SkillSelector SkillSelector { get; }

		public Vector2 Origin { get; private set; } = Vector2.get_Zero();


		public float Rotation { get; private set; }

		public Color? BackgroundDrawColor { get; private set; }

		public Color Color { get; private set; }

		public Color? HoverDrawColor { get; private set; }

		public Color? DrawColor { get; private set; }

		public bool ShowSelector { get; set; }

		public bool IsSelectorHovered
		{
			get
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				if (ShowSelector)
				{
					Rectangle selectorBounds = SelectorBounds;
					return ((Rectangle)(ref selectorBounds)).Contains(base.RelativeMousePosition);
				}
				return false;
			}
		}

		public Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? Skill
		{
			get
			{
				return _skill;
			}
			set
			{
				Common.SetProperty(ref _skill, value, new ValueChangedEventHandler<Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>(OnSkillChanged));
			}
		}

		public SkillSlotControl(SkillSlotType skillSlot, TemplatePresenter templatePresenter, Data data, SkillSelector skillSelector)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
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

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			bool num = SkillSlot.IsTerrestrial();
			Color? backgroundDrawColor = BackgroundDrawColor;
			if (backgroundDrawColor.HasValue)
			{
				SpriteBatchExtensions.DrawOnCtrl(color: backgroundDrawColor.GetValueOrDefault(), spriteBatch: spriteBatch, ctrl: this, texture: ContentService.Textures.Pixel, destinationRectangle: SkillBounds, sourceRectangle: Rectangle.get_Empty(), rotation: Rotation, origin: Origin, effects: (SpriteEffects)0);
			}
			Rectangle skillBounds;
			int num2;
			if (base.MouseOver)
			{
				skillBounds = SkillBounds;
				num2 = (((Rectangle)(ref skillBounds)).Contains(base.RelativeMousePosition) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			bool hovered = (byte)num2 != 0;
			if (FallBackTexture != null || Texture != null)
			{
				Color = (Color)(((_003F?)((hovered && HoverDrawColor.HasValue) ? HoverDrawColor : DrawColor)) ?? Color.get_White());
				Color = Color.get_White();
				if (HoveredTexture != null && hovered)
				{
					spriteBatch.DrawOnCtrl(this, HoveredTexture, SkillBounds, TextureRegion, Color, Rotation, Origin, (SpriteEffects)0);
				}
				if (Texture != null)
				{
					spriteBatch.DrawOnCtrl(this, Texture, SkillBounds, TextureRegion, Color, Rotation, Origin, (SpriteEffects)0);
				}
				else
				{
					spriteBatch.DrawOnCtrl(this, FallBackTexture, (FallbackBounds == Rectangle.get_Empty()) ? SkillBounds : FallbackBounds, SkillBounds, Color, Rotation, Origin, (SpriteEffects)0);
				}
			}
			Color borderColor = Color.get_Black();
			Texture2D pixel = ContentService.Textures.Pixel;
			skillBounds = SkillBounds;
			int left = ((Rectangle)(ref skillBounds)).get_Left();
			skillBounds = SkillBounds;
			spriteBatch.DrawOnCtrl(this, pixel, new Rectangle(left, ((Rectangle)(ref skillBounds)).get_Top(), SkillBounds.Width, 1), Rectangle.get_Empty(), borderColor * 0.6f);
			Texture2D pixel2 = ContentService.Textures.Pixel;
			skillBounds = SkillBounds;
			int left2 = ((Rectangle)(ref skillBounds)).get_Left();
			skillBounds = SkillBounds;
			spriteBatch.DrawOnCtrl(this, pixel2, new Rectangle(left2, ((Rectangle)(ref skillBounds)).get_Bottom() - 1, SkillBounds.Width, 1), Rectangle.get_Empty(), borderColor * 0.6f);
			Texture2D pixel3 = ContentService.Textures.Pixel;
			skillBounds = SkillBounds;
			int left3 = ((Rectangle)(ref skillBounds)).get_Left();
			skillBounds = SkillBounds;
			spriteBatch.DrawOnCtrl(this, pixel3, new Rectangle(left3, ((Rectangle)(ref skillBounds)).get_Top(), 1, SkillBounds.Height), Rectangle.get_Empty(), borderColor * 0.6f);
			Texture2D pixel4 = ContentService.Textures.Pixel;
			skillBounds = SkillBounds;
			int num3 = ((Rectangle)(ref skillBounds)).get_Right() - 1;
			skillBounds = SkillBounds;
			spriteBatch.DrawOnCtrl(this, pixel4, new Rectangle(num3, ((Rectangle)(ref skillBounds)).get_Top(), 1, SkillBounds.Height), Rectangle.get_Empty(), borderColor * 0.6f);
			if (!num)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill? skill = Skill;
				if (skill != null && skill!.Flags.HasFlag(SkillFlag.NoUnderwater))
				{
					spriteBatch.DrawOnCtrl(this, NoAquaticFlagTexture, SkillBounds, NoAquaticFlagTextureRegion, Color, Rotation, Origin, (SpriteEffects)0);
					goto IL_038d;
				}
			}
			if (hovered && HoveredFrameTexture != null)
			{
				spriteBatch.DrawOnCtrl(this, HoveredFrameTexture, SkillBounds, HoveredFrameTextureRegion, Color, Rotation, Origin, (SpriteEffects)0);
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
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int selectorHeight = 15;
			SkillBounds = new Rectangle(new Point(0, selectorHeight - 2), new Point(base.Width, base.Height - selectorHeight));
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
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			SkillSelector.Anchor = this;
			SkillSelector.AnchorOffset = new Point(-2, 10);
			SkillSelector.ZIndex = ZIndex + 100;
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
				List<KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>> filteredSkills2 = Data.Professions[TemplatePresenter.Template.Profession].Skills.Where<KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>>((KeyValuePair<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> e) => e.Value.PaletteId > 0 && e.Value.Slot.HasValue && e.Value.Slot == slot && (e.Value.Specialization == 0 || TemplatePresenter.Template.HasSpecialization(e.Value.Specialization, out slot2))).ToList();
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
