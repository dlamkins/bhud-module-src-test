using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Strings;
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

		public Control_TemplateSelection _TemplateSelection;

		private Texture2D _EmptyTraitLine;

		private Texture2D _Delete;

		private Texture2D _DeleteHovered;

		private Texture2D ProfessionIcon;

		private Texture2D Disclaimer_Background;

		private SelectionPopUp ProfessionSelection;

		private List<SelectionPopUp.SelectionEntry> _Professions;

		private TextBox NameBox;

		private Label NameLabel;

		private Control_AddButton Add_Button;

		public Control_AddButton Import_Button;

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
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Expected O, but got Unknown
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Expected O, but got Unknown
			//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0532: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0601: Unknown result type (might be due to invalid IL or missing references)
			//IL_060d: Expected O, but got Unknown
			//IL_0625: Unknown result type (might be due to invalid IL or missing references)
			//IL_062a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0635: Unknown result type (might be due to invalid IL or missing references)
			//IL_063c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_065d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0665: Unknown result type (might be due to invalid IL or missing references)
			//IL_067e: Unknown result type (might be due to invalid IL or missing references)
			//IL_068f: Expected O, but got Unknown
			_EmptyTraitLine = Texture2DExtension.GetRegion(BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.PlaceHolder_Traitline), 0, 0, 647, 136);
			_Delete = BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.Delete);
			_DeleteHovered = BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.Delete_Hovered);
			Disclaimer_Background = Texture2DExtension.GetRegion(BuildsManager.ModuleInstance.TextureManager._Controls[9], 0, 0, 647, 136);
			Font = GameService.Content.get_DefaultFont18();
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
			foreach (API.Profession profession in BuildsManager.ModuleInstance.Data.Professions)
			{
				_Professions.Add(new SelectionPopUp.SelectionEntry
				{
					Object = profession,
					Texture = AsyncTexture2D.op_Implicit(profession.IconBig._AsyncTexture.get_Texture()),
					Header = profession.Name,
					Content = new List<string>(),
					ContentTextures = new List<AsyncTexture2D>()
				});
			}
			ProfessionSelection.List = _Professions;
			ProfessionSelection.Changed += ProfessionSelection_Changed;
			Control_AddButton obj = new Control_AddButton
			{
				Texture = BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.Import),
				TextureHovered = BuildsManager.ModuleInstance.TextureManager.getControlTexture(_Controls.Import_Hovered)
			};
			((Control)obj).set_Parent((Container)(object)Templates_Panel);
			obj.Text = "";
			((Control)obj).set_Location(new Point(((Control)Templates_Panel).get_Width() - 130 - 40, 0));
			((Control)obj).set_Size(new Point(35, 35));
			((Control)obj).set_BasicTooltipText($"Import 'BuildPad' builds from '{BuildsManager.ModuleInstance.Paths.builds}config.ini'");
			((Control)obj).set_Visible(false);
			Import_Button = obj;
			((Control)Import_Button).add_Click((EventHandler<MouseEventArgs>)Import_Button_Click);
			Control_AddButton control_AddButton = new Control_AddButton();
			((Control)control_AddButton).set_Parent((Container)(object)Templates_Panel);
			control_AddButton.Text = common.Create;
			((Control)control_AddButton).set_Location(new Point(((Control)Templates_Panel).get_Width() - 130, 0));
			((Control)control_AddButton).set_Size(new Point(125, 35));
			Add_Button = control_AddButton;
			((Control)Add_Button).add_Click((EventHandler<MouseEventArgs>)Button_Click);
			BuildsManager.ModuleInstance.LanguageChanged += ModuleInstance_LanguageChanged;
			Control_TemplateSelection control_TemplateSelection = new Control_TemplateSelection((Container)(object)this);
			((Control)control_TemplateSelection).set_Location(new Point(5, 40));
			((Control)control_TemplateSelection).set_Parent((Container)(object)Templates_Panel);
			_TemplateSelection = control_TemplateSelection;
			_TemplateSelection.TemplateChanged += _TemplateSelection_TemplateChanged;
			Container_TabbedPanel detail_Panel = Detail_Panel;
			List<Tab> list = new List<Tab>();
			Tab obj2 = new Tab
			{
				Name = common.Build,
				Icon = BuildsManager.ModuleInstance.TextureManager.getIcon(_Icons.Template)
			};
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)Detail_Panel);
			((Control)val2).set_Visible(true);
			obj2.Panel = val2;
			list.Add(obj2);
			Tab obj3 = new Tab
			{
				Name = common.Gear,
				Icon = BuildsManager.ModuleInstance.TextureManager.getIcon(_Icons.Helmet)
			};
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)Detail_Panel);
			((Control)val3).set_Visible(false);
			obj3.Panel = val3;
			list.Add(obj3);
			detail_Panel.Tabs = list;
			Detail_Panel.SelectedTab = Detail_Panel.Tabs[0];
			((Control)Detail_Panel.Tabs[0].Panel).add_Resized((EventHandler<ResizedEventArgs>)Panel_Resized);
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
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
		}

		private void Panel_Resized(object sender, ResizedEventArgs e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			((Control)Gear).set_Size(((Control)Detail_Panel.Tabs[0].Panel).get_Size().Add(new Point(0, -((Control)Detail_Panel.GearBox).get_Bottom() + 30)));
		}

		private void ProfessionSelection_Changed(object sender, EventArgs e)
		{
			if (ProfessionSelection.SelectedProfession != null)
			{
				Template template = new Template();
				template.Profession = ProfessionSelection.SelectedProfession;
				template.Build.Profession = ProfessionSelection.SelectedProfession;
				BuildsManager.ModuleInstance.Selected_Template = template;
				BuildsManager.ModuleInstance.Templates.Add(BuildsManager.ModuleInstance.Selected_Template);
				BuildsManager.ModuleInstance.Selected_Template.SetChanged();
				_TemplateSelection.RefreshList();
				ProfessionSelection.SelectedProfession = null;
			}
		}

		public void PlayerCharacter_NameChanged(object sender, ValueEventArgs<string> e)
		{
			if (BuildsManager.ModuleInstance.ShowCurrentProfession.get_Value())
			{
				_TemplateSelection.SetSelection();
			}
		}

		private void Import_Button_Click(object sender, MouseEventArgs e)
		{
			BuildsManager.ModuleInstance.ImportTemplates();
			_TemplateSelection.Refresh();
			((Control)Import_Button).Hide();
		}

		private void ModuleInstance_LanguageChanged(object sender, EventArgs e)
		{
			Add_Button.Text = common.Create;
			Import_Button.Text = common.Create;
			Detail_Panel.Tabs[0].Name = common.Build;
			Detail_Panel.Tabs[1].Name = common.Gear;
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
			NameLabel.set_Text(BuildsManager.ModuleInstance.Selected_Template.Name);
			_TemplateSelection.RefreshList();
		}

		private void NameLabel_Click(object sender, MouseEventArgs e)
		{
			if (BuildsManager.ModuleInstance.Selected_Template?.Path != null)
			{
				((Control)NameLabel).set_Visible(false);
				((Control)NameBox).set_Visible(true);
				((TextInputBase)NameBox).set_Text(NameLabel.get_Text());
				((TextInputBase)NameBox).set_SelectionStart(0);
				((TextInputBase)NameBox).set_SelectionEnd(((TextInputBase)NameBox).get_Text().Length);
				((TextInputBase)NameBox).set_Focused(true);
			}
		}

		private void Button_Click(object sender, MouseEventArgs e)
		{
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			_Professions = new List<SelectionPopUp.SelectionEntry>();
			foreach (API.Profession profession in BuildsManager.ModuleInstance.Data.Professions)
			{
				_Professions.Add(new SelectionPopUp.SelectionEntry
				{
					Object = profession,
					Texture = AsyncTexture2D.op_Implicit(profession.IconBig._AsyncTexture.get_Texture()),
					Header = profession.Name,
					Content = new List<string>(),
					ContentTextures = new List<AsyncTexture2D>()
				});
			}
			ProfessionSelection.List = _Professions;
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
			_TemplateSelection.Refresh();
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
			if (((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition()) && BuildsManager.ModuleInstance.Selected_Template.Path != null)
			{
				BuildsManager.ModuleInstance.Selected_Template.Delete();
				BuildsManager.ModuleInstance.Selected_Template = new Template();
			}
			((Control)ProfessionSelection).Hide();
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			_ = BuildsManager.ModuleInstance.Selected_Template;
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
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
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
			((Control)this).set_BasicTooltipText(hovered ? (common.Delete + " " + common.Template) : null);
			if (BuildsManager.ModuleInstance.Selected_Template.Profession != null)
			{
				Template template = BuildsManager.ModuleInstance.Selected_Template;
				Texture2D texture = BuildsManager.ModuleInstance.TextureManager._Icons[0];
				if (template.Specialization != null)
				{
					texture = template.Specialization.ProfessionIconBig._AsyncTexture.get_Texture();
				}
				else if (template.Build.Profession != null)
				{
					texture = template.Build.Profession.IconBig._AsyncTexture.get_Texture();
				}
				((Rectangle)(ref rect))._002Ector(((Control)Detail_Panel).get_Location().X + 2, 46, 30, 30);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, rect, (Rectangle?)texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		protected override void DisposeControl()
		{
			((Control)Add_Button).remove_Click((EventHandler<MouseEventArgs>)Button_Click);
			((Control)Add_Button).Dispose();
			BuildsManager.ModuleInstance.LanguageChanged -= ModuleInstance_LanguageChanged;
			BuildsManager.ModuleInstance.Selected_Template_Edit -= Selected_Template_Edit;
			BuildsManager.ModuleInstance.Selected_Template_Changed -= ModuleInstance_Selected_Template_Changed;
			BuildsManager.ModuleInstance.Templates_Loaded -= Templates_Loaded;
			BuildsManager.ModuleInstance.Selected_Template_Redraw -= Selected_Template_Redraw;
			((Control)Detail_Panel.Tabs[0].Panel).remove_Resized((EventHandler<ResizedEventArgs>)Panel_Resized);
			Detail_Panel.TemplateBox.remove_EnterPressed((EventHandler<EventArgs>)TemplateBox_EnterPressed);
			Detail_Panel.GearBox.remove_EnterPressed((EventHandler<EventArgs>)GearBox_EnterPressed);
			Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)GlobalClick);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			_TemplateSelection.TemplateChanged -= _TemplateSelection_TemplateChanged;
			((Control)_TemplateSelection).Dispose();
			NameBox.remove_EnterPressed((EventHandler<EventArgs>)NameBox_TextChanged);
			((Control)NameBox).Dispose();
			((Control)NameLabel).remove_Click((EventHandler<MouseEventArgs>)NameLabel_Click);
			((Control)NameLabel).Dispose();
			((Control)Import_Button).remove_Click((EventHandler<MouseEventArgs>)Import_Button_Click);
			((Control)Import_Button).Dispose();
			ProfessionSelection.Changed -= ProfessionSelection_Changed;
			((Control)ProfessionSelection).Dispose();
			((WindowBase2)this).DisposeControl();
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).OnShown(e);
			if (BuildsManager.ModuleInstance.ShowCurrentProfession.get_Value())
			{
				_TemplateSelection.SetSelection();
			}
		}
	}
}
