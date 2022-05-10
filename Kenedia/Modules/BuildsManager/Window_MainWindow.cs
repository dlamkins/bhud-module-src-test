using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager
{
	public class Window_MainWindow : StandardWindow
	{
		private Panel Templates_Panel;

		private Container_TabbedPanel Detail_Panel;

		private Control_Equipment Gear;

		private Control_Build Build;

		private Control_TemplateSelection _TemplateSelection;

		private Texture2D _EmptyTraitLine;

		private Texture2D _Delete;

		private Texture2D _DeleteHovered;

		private Texture2D ProfessionIcon;

		private SelectionPopUp ProfessionSelection;

		private List<SelectionPopUp.SelectionEntry> _Professions;

		private TextBox NameBox;

		private Label NameLabel;

		private Control_AddButton Add_Button;

		private BitmapFont Font;

		private void _TemplateSelection_TemplateChanged(object sender, TemplateChangedEvent e)
		{
			BuildsManager.ModuleInstance.Selected_Template = e.Template;
			((TextInputBase)Detail_Panel.TemplateBox).set_Text(BuildsManager.ModuleInstance.Selected_Template?.Build.TemplateCode);
			NameLabel.set_Text(e.Template.Name);
			((Control)NameLabel).Show();
			((Control)NameBox).Hide();
		}

		public Window_MainWindow(Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Expected O, but got Unknown
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Expected O, but got Unknown
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0496: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Expected O, but got Unknown
			//IL_0514: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0532: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Expected O, but got Unknown
			_EmptyTraitLine = Texture2DExtension.GetRegion(BuildsManager.TextureManager.getControlTexture(_Controls.PlaceHolder_Traitline), 0, 0, 647, 136);
			_Delete = BuildsManager.TextureManager.getControlTexture(_Controls.Delete);
			_DeleteHovered = BuildsManager.TextureManager.getControlTexture(_Controls.Delete_Hovered);
			ContentService cnt = new ContentService();
			Font = cnt.GetFont((FontFace)0, (FontSize)18, (FontStyle)0);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(260, ((Container)this).get_ContentRegion().Height));
			((Control)val).set_BackgroundColor(new Color(0, 0, 0, 50));
			Templates_Panel = val;
			Container_TabbedPanel container_TabbedPanel = new Container_TabbedPanel();
			((Control)container_TabbedPanel).set_Parent((Container)(object)this);
			Rectangle localBounds = ((Control)Templates_Panel).get_LocalBounds();
			((Control)container_TabbedPanel).set_Location(new Point(((Rectangle)(ref localBounds)).get_Right(), 40));
			int width = ((Container)this).get_ContentRegion().Width;
			localBounds = ((Control)Templates_Panel).get_LocalBounds();
			((Control)container_TabbedPanel).set_Size(new Point(width - ((Rectangle)(ref localBounds)).get_Right(), ((Container)this).get_ContentRegion().Height - 45));
			Detail_Panel = container_TabbedPanel;
			_Professions = new List<SelectionPopUp.SelectionEntry>();
			ProfessionSelection = new SelectionPopUp((Container)(object)this);
			foreach (API.Profession profession in BuildsManager.Data.Professions)
			{
				_Professions.Add(new SelectionPopUp.SelectionEntry
				{
					Object = profession,
					Texture = profession.IconBig.Texture,
					Header = profession.Name,
					Content = new List<string>(),
					ContentTextures = new List<Texture2D>()
				});
			}
			ProfessionSelection.List = _Professions;
			SelectionPopUp professionSelection = ProfessionSelection;
			professionSelection.Changed = (EventHandler)Delegate.Combine(professionSelection.Changed, (EventHandler)delegate
			{
				if (ProfessionSelection.SelectedProfession != null)
				{
					BuildsManager.ModuleInstance.Selected_Template = new Template();
					BuildsManager.ModuleInstance.Selected_Template.Profession = ProfessionSelection.SelectedProfession;
					BuildsManager.ModuleInstance.Selected_Template.Build.Profession = ProfessionSelection.SelectedProfession;
					BuildsManager.ModuleInstance.Selected_Template.SetChanged();
					ProfessionSelection.SelectedProfession = null;
				}
			});
			Control_AddButton control_AddButton = new Control_AddButton();
			((Control)control_AddButton).set_Parent((Container)(object)Templates_Panel);
			control_AddButton.Text = "Create";
			((Control)control_AddButton).set_Location(new Point(((Control)Templates_Panel).get_Width() - 130, 0));
			((Control)control_AddButton).set_Size(new Point(125, 35));
			Add_Button = control_AddButton;
			((Control)Add_Button).add_Click((EventHandler<MouseEventArgs>)Button_Click);
			Control_TemplateSelection control_TemplateSelection = new Control_TemplateSelection((Container)(object)this);
			((Control)control_TemplateSelection).set_Location(new Point(5, 40));
			((Control)control_TemplateSelection).set_Parent((Container)(object)Templates_Panel);
			_TemplateSelection = control_TemplateSelection;
			_TemplateSelection.TemplateChanged += _TemplateSelection_TemplateChanged;
			Container_TabbedPanel detail_Panel = Detail_Panel;
			List<Tab> list = new List<Tab>();
			Tab obj = new Tab
			{
				Name = "Build",
				Icon = BuildsManager.TextureManager.getIcon(_Icons.Template)
			};
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)Detail_Panel);
			((Control)val2).set_Visible(true);
			obj.Panel = val2;
			list.Add(obj);
			Tab obj2 = new Tab
			{
				Name = "Gear",
				Icon = BuildsManager.TextureManager.getIcon(_Icons.Helmet)
			};
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)Detail_Panel);
			((Control)val3).set_Visible(false);
			obj2.Panel = val3;
			list.Add(obj2);
			detail_Panel.Tabs = list;
			Detail_Panel.SelectedTab = Detail_Panel.Tabs[0];
			((Control)Detail_Panel.Tabs[0].Panel).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				((Control)Gear).set_Size(((Control)Detail_Panel.Tabs[0].Panel).get_Size().Add(new Point(0, -((Control)Detail_Panel.GearBox).get_Bottom() + 30)));
			});
			Control_Build control_Build = new Control_Build((Container)(object)Detail_Panel.Tabs[0].Panel);
			((Control)control_Build).set_Parent((Container)(object)Detail_Panel.Tabs[0].Panel);
			((Control)control_Build).set_Size(((Control)Detail_Panel.Tabs[0].Panel).get_Size());
			control_Build.Scale = 1.0;
			Build = control_Build;
			Control_Equipment control_Equipment = new Control_Equipment((Container)(object)Detail_Panel.Tabs[1].Panel);
			((Control)control_Equipment).set_Parent((Container)(object)Detail_Panel.Tabs[1].Panel);
			((Control)control_Equipment).set_Size(((Control)Detail_Panel.Tabs[1].Panel).get_Size());
			control_Equipment.Scale = 1.0;
			Gear = control_Equipment;
			((TextInputBase)Detail_Panel.TemplateBox).set_Text(BuildsManager.ModuleInstance.Selected_Template?.Build.ParseBuildCode());
			((TextInputBase)Detail_Panel.GearBox).set_Text(BuildsManager.ModuleInstance.Selected_Template?.Gear.TemplateCode);
			TextBox val4 = new TextBox();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(((Control)Detail_Panel).get_Location().X + 5 + 32, 0));
			((Control)val4).set_Height(35);
			((Control)val4).set_Width(((Control)Detail_Panel).get_Width() - 38 - 32 - 5);
			((TextInputBase)val4).set_Font(Font);
			((Control)val4).set_Visible(false);
			NameBox = val4;
			NameBox.add_EnterPressed((EventHandler<EventArgs>)NameBox_TextChanged);
			Label val5 = new Label();
			val5.set_Text("A Template Name");
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Location(new Point(((Control)Detail_Panel).get_Location().X + 5 + 32, 0));
			((Control)val5).set_Height(35);
			((Control)val5).set_Width(((Control)Detail_Panel).get_Width() - 38 - 32 - 5);
			val5.set_Font(Font);
			NameLabel = val5;
			((Control)NameLabel).add_Click((EventHandler<MouseEventArgs>)NameLabel_Click);
			BuildsManager.ModuleInstance.Selected_Template_Edit += Selected_Template_Edit;
			BuildsManager.ModuleInstance.Selected_Template_Changed += ModuleInstance_Selected_Template_Changed;
			BuildsManager.ModuleInstance.Templates_Loaded += Templates_Loaded;
			BuildsManager.ModuleInstance.Selected_Template_Redraw += Selected_Template_Redraw;
			Detail_Panel.TemplateBox.add_EnterPressed((EventHandler<EventArgs>)TemplateBox_EnterPressed);
			Detail_Panel.GearBox.add_EnterPressed((EventHandler<EventArgs>)GearBox_EnterPressed);
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)GlobalClick);
		}

		private void GlobalClick(object sender, MouseEventArgs e)
		{
			if (!((Control)NameBox).get_MouseOver())
			{
				((Control)NameBox).set_Visible(false);
				((Control)NameLabel).set_Visible(true);
			}
		}

		private void Selected_Template_Redraw(object sender, EventArgs e)
		{
			Build.SkillBar.ApplyBuild(sender, e);
			Gear.UpdateLayout();
		}

		private void GearBox_EnterPressed(object sender, EventArgs e)
		{
			GearTemplate gear = new GearTemplate(((TextInputBase)Detail_Panel.GearBox).get_Text());
			if (gear != null)
			{
				BuildsManager.ModuleInstance.Selected_Template.Gear = gear;
				BuildsManager.ModuleInstance.Selected_Template.SetChanged();
				BuildsManager.ModuleInstance.OnSelected_Template_Redraw(null, null);
			}
		}

		private void TemplateBox_EnterPressed(object sender, EventArgs e)
		{
			BuildTemplate build = new BuildTemplate(((TextInputBase)Detail_Panel.TemplateBox).get_Text());
			if (build == null || build.Profession == null)
			{
				return;
			}
			BuildsManager.ModuleInstance.Selected_Template.Build = build;
			BuildsManager.ModuleInstance.Selected_Template.Profession = build.Profession;
			foreach (SpecLine spec in BuildsManager.ModuleInstance.Selected_Template.Build.SpecLines)
			{
				if (spec.Specialization?.Elite ?? false)
				{
					BuildsManager.ModuleInstance.Selected_Template.Specialization = spec.Specialization;
					break;
				}
			}
			BuildsManager.ModuleInstance.Selected_Template.SetChanged();
			BuildsManager.ModuleInstance.OnSelected_Template_Redraw(null, null);
		}

		private void NameBox_TextChanged(object sender, EventArgs e)
		{
			BuildsManager.ModuleInstance.Selected_Template.Name = ((TextInputBase)NameBox).get_Text();
			BuildsManager.ModuleInstance.Selected_Template.Save();
			((Control)NameLabel).set_Visible(true);
			((Control)NameBox).set_Visible(false);
			NameLabel.set_Text(((TextInputBase)NameBox).get_Text());
		}

		private void NameLabel_Click(object sender, MouseEventArgs e)
		{
			((Control)NameLabel).set_Visible(false);
			((Control)NameBox).set_Visible(true);
			((TextInputBase)NameBox).set_Text(NameLabel.get_Text());
			((TextInputBase)NameBox).set_SelectionStart(0);
			((TextInputBase)NameBox).set_SelectionEnd(((TextInputBase)NameBox).get_Text().Length);
			((TextInputBase)NameBox).set_Focused(true);
		}

		private void Button_Click(object sender, MouseEventArgs e)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			((Control)ProfessionSelection).Show();
			((Control)ProfessionSelection).set_Location(((Control)Add_Button).get_Location().Add(new Point(((Control)Add_Button).get_Width() + 5, 0)));
			ProfessionSelection.SelectionType = SelectionPopUp.selectionType.Profession;
			ProfessionSelection.SelectionTarget = BuildsManager.ModuleInstance.Selected_Template.Profession;
			((Control)ProfessionSelection).set_Width(175);
			ProfessionSelection.SelectionTarget = null;
			ProfessionSelection.List = _Professions;
		}

		private void Templates_Loaded(object sender, EventArgs e)
		{
			((Control)_TemplateSelection).Invalidate();
		}

		private void ModuleInstance_Selected_Template_Changed(object sender, EventArgs e)
		{
			NameLabel.set_Text(BuildsManager.ModuleInstance.Selected_Template.Name);
			((TextInputBase)Detail_Panel.TemplateBox).set_Text(BuildsManager.ModuleInstance.Selected_Template.Build.TemplateCode);
			((TextInputBase)Detail_Panel.GearBox).set_Text(BuildsManager.ModuleInstance.Selected_Template.Gear.TemplateCode);
		}

		private void Selected_Template_Edit(object sender, EventArgs e)
		{
			BuildsManager.ModuleInstance.Selected_Template.Specialization = null;
			foreach (SpecLine spec in BuildsManager.ModuleInstance.Selected_Template.Build.SpecLines)
			{
				if (spec.Specialization?.Elite ?? false)
				{
					BuildsManager.ModuleInstance.Selected_Template.Specialization = spec.Specialization;
					break;
				}
			}
			((TextInputBase)Detail_Panel.TemplateBox).set_Text(BuildsManager.ModuleInstance.Selected_Template.Build.ParseBuildCode());
			((TextInputBase)Detail_Panel.GearBox).set_Text(BuildsManager.ModuleInstance.Selected_Template?.Gear.TemplateCode);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnClick(e);
			Rectangle localBounds = ((Control)Detail_Panel).get_LocalBounds();
			Rectangle rect = default(Rectangle);
			((Rectangle)(ref rect))._002Ector(((Rectangle)(ref localBounds)).get_Right() - 35, 44, 35, 35);
			if (((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				BuildsManager.ModuleInstance.Selected_Template.Delete();
				BuildsManager.ModuleInstance.Selected_Template = new Template();
			}
			((Control)ProfessionSelection).Hide();
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			Template template = BuildsManager.ModuleInstance.Selected_Template;
			if (template != null && template.Profession != null && template.Profession.Id == "Revenant")
			{
				Texture2D texture = Texture2DExtension.GetRegion(BuildsManager.TextureManager._Controls[9], 0, 0, 647, 136);
				Rectangle rect = default(Rectangle);
				((Rectangle)(ref rect))._002Ector(((Control)Detail_Panel).get_LocalBounds().X + 5, ((Control)Detail_Panel).get_LocalBounds().Y + ((Control)Detail_Panel).get_LocalBounds().Height / 2 - 50, ((Control)Detail_Panel).get_LocalBounds().Width - 10, Font.get_LineHeight() + 100);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, rect, (Rectangle?)texture.get_Bounds(), new Color(0, 0, 0, 175), 0f, default(Vector2), (SpriteEffects)0);
				Color color = Color.get_Black();
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 2, rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 1, rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 2, ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 1, ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Revenant is currently not supported! It is in development tho and hopefully comes soon!", Font, new Rectangle(((Control)Detail_Panel).get_LocalBounds().X + 10, ((Control)Detail_Panel).get_LocalBounds().Y + ((Control)Detail_Panel).get_LocalBounds().Height / 2, ((Control)Detail_Panel).get_LocalBounds().Width, Font.get_LineHeight()), Color.get_Red(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintBeforeChildren(spriteBatch, bounds);
			Rectangle rect = default(Rectangle);
			((Rectangle)(ref rect))._002Ector(((Control)Detail_Panel).get_Location().X, 44, ((Control)Detail_Panel).get_Width() - 38, 35);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _EmptyTraitLine, rect, (Rectangle?)_EmptyTraitLine.get_Bounds(), new Color(135, 135, 135, 255), 0f, default(Vector2), (SpriteEffects)0);
			Color color = Color.get_Black();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 2, rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 1, rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 2, ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 1, ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			Rectangle localBounds = ((Control)Detail_Panel).get_LocalBounds();
			((Rectangle)(ref rect))._002Ector(((Rectangle)(ref localBounds)).get_Right() - 35, 44, 35, 35);
			bool hovered = ((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, hovered ? _DeleteHovered : _Delete, rect, (Rectangle?)_Delete.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			((Control)this).set_BasicTooltipText(hovered ? "Delete Template" : null);
			if (BuildsManager.ModuleInstance.Selected_Template.Profession != null)
			{
				Template template = BuildsManager.ModuleInstance.Selected_Template;
				Texture2D texture = BuildsManager.TextureManager._Icons[0];
				if (template.Specialization != null)
				{
					texture = template.Specialization.ProfessionIconBig.Texture;
				}
				else if (template.Build.Profession != null)
				{
					texture = template.Build.Profession.IconBig.Texture;
				}
				((Rectangle)(ref rect))._002Ector(((Control)Detail_Panel).get_Location().X + 2, 46, 30, 30);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, rect, (Rectangle?)texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		protected override void DisposeControl()
		{
			((Control)_TemplateSelection).Dispose();
			((Control)NameBox).Dispose();
			((Control)NameLabel).Dispose();
			((WindowBase2)this).DisposeControl();
		}
	}
}
