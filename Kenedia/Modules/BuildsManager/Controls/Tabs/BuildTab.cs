using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.Controls.ProfessionSpecific;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.DataModels;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Tabs
{
	public class BuildTab : Container
	{
		private readonly DetailedTexture _specsBackground = new DetailedTexture(993592);

		private readonly DetailedTexture _skillsBackground = new DetailedTexture(155960);

		private readonly DetailedTexture _skillsBackgroundBottomBorder = new DetailedTexture(155987);

		private readonly DetailedTexture _skillsBackgroundTopBorder = new DetailedTexture(155989);

		private readonly ProfessionRaceSelection _professionRaceSelection;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _specializationsPanel;

		private readonly Kenedia.Modules.Core.Controls.Panel _professionSpecificsContainer;

		private readonly SkillsBar _skillbar;

		private readonly Dummy _dummy;

		private readonly ButtonImage _specIcon;

		private readonly ButtonImage _raceIcon;

		private readonly Kenedia.Modules.Core.Controls.TextBox _buildCodeBox;

		private readonly ImageButton _copyButton;

		private ProfessionSpecifics _professionSpecifics;

		public SpecLine SpecLine1 { get; }

		public SpecLine SpecLine2 { get; }

		public SpecLine SpecLine3 { get; }

		public TemplatePresenter TemplatePresenter { get; }

		public Data Data { get; }

		public BuildTab(TemplatePresenter templatePresenter, Data data)
		{
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			TemplatePresenter = templatePresenter;
			Data = data;
			base.ClipsBounds = false;
			_copyButton = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(2208345),
				HoveredTexture = AsyncTexture2D.FromAssetId(2208347),
				Size = new Point(26),
				ClickAction = async delegate
				{
					try
					{
						string s = _buildCodeBox.Text;
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
			_buildCodeBox = new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = this,
				Location = new Point(_copyButton.Right + 2, 0),
				EnterPressedAction = delegate(string txt)
				{
					TemplatePresenter.Template?.LoadBuildFromCode(txt, save: true);
				}
			};
			_professionSpecificsContainer = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = this,
				Location = new Point(0, _buildCodeBox.Bottom + 25),
				Width = 500,
				Height = 100,
				ZIndex = 13
			};
			_skillbar = new SkillsBar(templatePresenter, data)
			{
				Parent = this,
				Location = new Point(5, _professionSpecificsContainer.Bottom + 15),
				Width = 500,
				ZIndex = 12
			};
			_specIcon = new ButtonImage
			{
				Parent = this,
				Size = new Point(40),
				ZIndex = 15
			};
			_raceIcon = new ButtonImage
			{
				Parent = this,
				TextureSize = new Point(32),
				ZIndex = 15,
				Texture = (AsyncTexture2D)TexturesService.GetTextureFromRef("textures\\races\\none.png", "none"),
				Size = new Point(40)
			};
			_dummy = new Dummy
			{
				Parent = this,
				Location = new Point(0, _skillbar.Bottom + 20),
				Width = base.Width,
				Height = 20
			};
			_specializationsPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Location = new Point(0, _dummy.Bottom),
				Width = 800,
				HeightSizingMode = SizingMode.AutoSize,
				ControlPadding = new Vector2(1f),
				BackgroundColor = Color.get_Black() * 0.8f,
				AutoSizePadding = new Point(1),
				ZIndex = 10
			};
			SpecLine1 = new SpecLine(SpecializationSlotType.Line_1, TemplatePresenter, Data)
			{
				Parent = _specializationsPanel
			};
			SpecLine2 = new SpecLine(SpecializationSlotType.Line_2, TemplatePresenter, Data)
			{
				Parent = _specializationsPanel
			};
			SpecLine3 = new SpecLine(SpecializationSlotType.Line_3, TemplatePresenter, Data)
			{
				Parent = _specializationsPanel
			};
			_professionRaceSelection = new ProfessionRaceSelection(data)
			{
				Parent = this,
				Visible = false,
				ClipsBounds = false,
				ZIndex = 16
			};
			TemplatePresenter.TemplateChanged += new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
			TemplatePresenter.BuildCodeChanged += new EventHandler(TemplatePresenter_BuildCodeChanged);
			TemplatePresenter.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(TemplatePresenter_ProfessionChanged);
			TemplatePresenter.RaceChanged += new ValueChangedEventHandler<Races>(TemplatePresenter_RaceChanged);
			TemplatePresenter.EliteSpecializationChanged += new SpecializationChangedEventHandler(TemplatePresenter_EliteSpecializationChanged);
			WidthSizingMode = SizingMode.Fill;
			HeightSizingMode = SizingMode.Fill;
			SetSelectionTextures();
			SetProfessionSpecifics();
			_buildCodeBox.Text = TemplatePresenter?.Template?.ParseBuildCode();
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
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
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
			spriteBatch.DrawStringOnCtrl(this, "Loading Data. Please wait.", Control.Content.DefaultFont16, tR, Color.get_White(), wrap: false, HorizontalAlignment.Center);
			spriteBatch.End();
		}

		public override void RecalculateLayout()
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (_buildCodeBox != null)
			{
				_buildCodeBox.Width = base.Width - _buildCodeBox.Left;
			}
			if (_specializationsPanel != null)
			{
				_professionSpecificsContainer.Width = _specializationsPanel.Width;
				_skillbar.Width = _specializationsPanel.Width - 10;
				_dummy.Width = _specializationsPanel.Width;
				_specsBackground.Bounds = new Rectangle(0, _dummy.Bottom - 55, base.Width + 15, _dummy.Height + _specializationsPanel.Height + 34);
				_specsBackground.TextureRegion = new Rectangle(0, 0, 650, 450);
				_specIcon.Location = new Point(_specializationsPanel.Width - _specIcon.Width - 8, _professionSpecificsContainer.Top);
				_raceIcon.Location = new Point(_specializationsPanel.Width - _raceIcon.Width - 8, _specIcon.Bottom + 4);
				_skillsBackground.Bounds = new Rectangle(0, 40, base.Width, 230);
				_skillsBackground.TextureRegion = new Rectangle(20, 20, base.Width - 100, 220);
				DetailedTexture skillsBackgroundTopBorder = _skillsBackgroundTopBorder;
				Rectangle bounds = _skillsBackground.Bounds;
				int num = ((Rectangle)(ref bounds)).get_Left() - 5;
				bounds = _skillsBackground.Bounds;
				skillsBackgroundTopBorder.Bounds = new Rectangle(num, ((Rectangle)(ref bounds)).get_Top() - 20, _professionSpecificsContainer.Width / 2, 22);
				_skillsBackgroundTopBorder.TextureRegion = new Rectangle(35, 5, _professionSpecificsContainer.Width / 2, 22);
				DetailedTexture skillsBackgroundBottomBorder = _skillsBackgroundBottomBorder;
				bounds = _skillsBackground.Bounds;
				skillsBackgroundBottomBorder.Bounds = new Rectangle(0, ((Rectangle)(ref bounds)).get_Bottom() - 4, base.Width, 22);
				_skillsBackgroundBottomBorder.TextureRegion = new Rectangle(0, _skillsBackgroundBottomBorder.Texture.Height - 26, _skillsBackgroundBottomBorder.Texture.Width - 36, 22);
				if (_professionSpecifics != null)
				{
					_professionSpecifics.Width = _professionSpecificsContainer.Width;
					_professionSpecifics.Height = _professionSpecificsContainer.Height;
				}
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			_skillsBackground?.Draw(this, spriteBatch);
			_skillsBackgroundBottomBorder?.Draw(this, spriteBatch);
			_skillsBackgroundTopBorder?.Draw(this, spriteBatch);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			foreach (Control child in base.Children)
			{
				child?.Dispose();
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			if (_specIcon?.MouseOver ?? false)
			{
				_professionRaceSelection.Visible = true;
				_professionRaceSelection.Type = ProfessionRaceSelection.SelectionType.Profession;
				_professionRaceSelection.Location = base.RelativeMousePosition;
				_professionRaceSelection.OnClickAction = delegate(object value)
				{
					TemplatePresenter?.Template?.SetProfession((ProfessionType)value);
				};
			}
			if (_raceIcon?.MouseOver ?? false)
			{
				_professionRaceSelection.Visible = true;
				_professionRaceSelection.Type = ProfessionRaceSelection.SelectionType.Race;
				_professionRaceSelection.Location = base.RelativeMousePosition;
				_professionRaceSelection.OnClickAction = delegate(object value)
				{
					TemplatePresenter.Template?.SetRace((Races)value);
				};
			}
		}

		private void TemplatePresenter_BuildCodeChanged(object sender, EventArgs e)
		{
			_buildCodeBox.Text = TemplatePresenter?.Template?.ParseBuildCode();
		}

		private void TemplatePresenter_TemplateChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Template> e)
		{
			SetSelectionTextures();
			SetProfessionSpecifics();
			_buildCodeBox.Text = TemplatePresenter?.Template?.ParseBuildCode();
		}

		private void TemplatePresenter_EliteSpecializationChanged(object sender, SpecializationChangedEventArgs e)
		{
			SetSpecIcons();
			SetProfessionSpecifics();
		}

		private void TemplatePresenter_RaceChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Races> e)
		{
			SetRaceIcons();
		}

		private void TemplatePresenter_ProfessionChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<ProfessionType> e)
		{
			SetSpecIcons();
			SetProfessionSpecifics();
		}

		private void SetProfessionSpecifics()
		{
			_professionSpecifics?.Dispose();
			if (TemplatePresenter?.Template != null)
			{
				switch (TemplatePresenter?.Template.Profession)
				{
				case ProfessionType.Guardian:
					_professionSpecifics = new GuardianSpecifics(TemplatePresenter, Data);
					break;
				case ProfessionType.Warrior:
					_professionSpecifics = new WarriorSpecifics(TemplatePresenter, Data);
					break;
				case ProfessionType.Engineer:
					_professionSpecifics = new EngineerSpecifics(TemplatePresenter, Data);
					break;
				case ProfessionType.Ranger:
					_professionSpecifics = new RangerSpecifics(TemplatePresenter, Data);
					break;
				case ProfessionType.Thief:
					_professionSpecifics = new ThiefSpecifics(TemplatePresenter, Data);
					break;
				case ProfessionType.Elementalist:
					_professionSpecifics = new ElementalistSpecifics(TemplatePresenter, Data);
					break;
				case ProfessionType.Mesmer:
					_professionSpecifics = new MesmerSpecifics(TemplatePresenter, Data);
					break;
				case ProfessionType.Necromancer:
					_professionSpecifics = new NecromancerSpecifics(TemplatePresenter, Data);
					break;
				case ProfessionType.Revenant:
					_professionSpecifics = new RevenantSpecifics(TemplatePresenter, Data);
					break;
				}
				if (_professionSpecifics != null)
				{
					_professionSpecifics.Parent = _professionSpecificsContainer;
					_professionSpecifics.Width = _professionSpecificsContainer?.Width ?? 0;
					_professionSpecifics.Height = _professionSpecificsContainer?.Height ?? 0;
				}
			}
		}

		private void SetSelectionTextures()
		{
			SetSpecIcons();
			SetRaceIcons();
		}

		private void SetRaceIcons()
		{
			Race raceIcon = default(Race);
			_raceIcon.Texture = ((Data.Races?.TryGetValue((TemplatePresenter?.Template?.Race).GetValueOrDefault(Races.None), out raceIcon) ?? false) ? raceIcon.Icon : null);
			Race raceName = default(Race);
			_raceIcon.BasicTooltipText = ((Data.Races?.TryGetValue((TemplatePresenter?.Template?.Race).GetValueOrDefault(Races.None), out raceName) ?? false) ? raceName.Name : null);
		}

		private void SetSpecIcons()
		{
			Profession professionForIcon = default(Profession);
			_specIcon.Texture = TemplatePresenter?.Template?.EliteSpecialization?.ProfessionIconBig ?? ((Data.Professions?.TryGetValue((TemplatePresenter?.Template?.Profession).GetValueOrDefault(ProfessionType.Guardian), out professionForIcon) ?? false) ? professionForIcon.IconBig : null);
			Profession professionForName = default(Profession);
			_specIcon.BasicTooltipText = TemplatePresenter?.Template?.EliteSpecialization?.Name ?? ((Data.Professions?.TryGetValue((TemplatePresenter?.Template?.Profession).GetValueOrDefault(ProfessionType.Guardian), out professionForName) ?? false) ? professionForName.Name : null);
		}
	}
}
