using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Controls;
using Kenedia.Modules.BuildsManager.Enums;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Strings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager
{
	public class Window_MainWindow : StandardWindow
	{
		private readonly Panel _templatesPanel;

		private readonly Container_TabbedPanel _detailPanel;

		private readonly Control_Equipment _gear;

		private readonly Control_Build _build;

		private readonly ControlTemplateSelection _templateSelection;

		private readonly Texture2D _emptyTraitLine;

		private readonly Texture2D _delete;

		private readonly Texture2D _deleteHovered;

		private readonly Texture2D _disclaimerBackground;

		private readonly SelectionPopUp _professionSelection;

		private readonly TextBox _nameBox;

		private readonly Label _nameLabel;

		private readonly Control_AddButton _addButton;

		private readonly BitmapFont _font;

		public readonly Control_AddButton ImportButton;

		public ControlTemplateSelection TemplateSelection;

		private List<SelectionPopUp.SelectionEntry> _professions;

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
			_emptyTraitLine = Texture2DExtension.GetRegion(BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.PlaceHolder_Traitline), 0, 0, 647, 136);
			_delete = BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.Delete);
			_deleteHovered = BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.Delete_Hovered);
			_disclaimerBackground = Texture2DExtension.GetRegion(BuildsManager.s_moduleInstance.TextureManager._Controls[9], 0, 0, 647, 136);
			_font = GameService.Content.get_DefaultFont18();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Size(new Point(260, ((Container)this).get_ContentRegion().Height));
			((Control)val).set_BackgroundColor(new Color(0, 0, 0, 50));
			_templatesPanel = val;
			Container_TabbedPanel container_TabbedPanel = new Container_TabbedPanel();
			((Control)container_TabbedPanel).set_Parent((Container)(object)this);
			Rectangle localBounds = ((Control)_templatesPanel).get_LocalBounds();
			((Control)container_TabbedPanel).set_Location(new Point(((Rectangle)(ref localBounds)).get_Right(), 40));
			int width = ((Container)this).get_ContentRegion().Width;
			localBounds = ((Control)_templatesPanel).get_LocalBounds();
			((Control)container_TabbedPanel).set_Size(new Point(width - ((Rectangle)(ref localBounds)).get_Right(), ((Container)this).get_ContentRegion().Height - 45));
			_detailPanel = container_TabbedPanel;
			_professions = new List<SelectionPopUp.SelectionEntry>();
			_professionSelection = new SelectionPopUp((Container)(object)this);
			foreach (API.Profession profession in BuildsManager.s_moduleInstance.Data.Professions)
			{
				_professions.Add(new SelectionPopUp.SelectionEntry
				{
					Object = profession,
					Texture = AsyncTexture2D.op_Implicit(profession.IconBig._AsyncTexture.get_Texture()),
					Header = profession.Name,
					Content = new List<string>(),
					ContentTextures = new List<AsyncTexture2D>()
				});
			}
			_professionSelection.List = _professions;
			_professionSelection.Changed += ProfessionSelection_Changed;
			Control_AddButton obj = new Control_AddButton
			{
				Texture = BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.Import),
				TextureHovered = BuildsManager.s_moduleInstance.TextureManager.getControlTexture(ControlTexture.Import_Hovered)
			};
			((Control)obj).set_Parent((Container)(object)_templatesPanel);
			obj.Text = string.Empty;
			((Control)obj).set_Location(new Point(((Control)_templatesPanel).get_Width() - 130 - 40, 0));
			((Control)obj).set_Size(new Point(35, 35));
			((Control)obj).set_BasicTooltipText($"Import 'BuildPad' builds from '{BuildsManager.s_moduleInstance.Paths.builds}config.ini'");
			((Control)obj).set_Visible(false);
			ImportButton = obj;
			((Control)ImportButton).add_Click((EventHandler<MouseEventArgs>)Import_Button_Click);
			Control_AddButton control_AddButton = new Control_AddButton();
			((Control)control_AddButton).set_Parent((Container)(object)_templatesPanel);
			control_AddButton.Text = common.Create;
			((Control)control_AddButton).set_Location(new Point(((Control)_templatesPanel).get_Width() - 130, 0));
			((Control)control_AddButton).set_Size(new Point(125, 35));
			_addButton = control_AddButton;
			((Control)_addButton).add_Click((EventHandler<MouseEventArgs>)Button_Click);
			BuildsManager.s_moduleInstance.LanguageChanged += ModuleInstance_LanguageChanged;
			ControlTemplateSelection controlTemplateSelection = new ControlTemplateSelection((Container)(object)this);
			((Control)controlTemplateSelection).set_Location(new Point(5, 40));
			((Control)controlTemplateSelection).set_Parent((Container)(object)_templatesPanel);
			_templateSelection = controlTemplateSelection;
			_templateSelection.TemplateChanged += TemplateSelection_TemplateChanged;
			Container_TabbedPanel detailPanel = _detailPanel;
			List<Tab> list = new List<Tab>();
			Tab obj2 = new Tab
			{
				Name = common.Build,
				Icon = BuildsManager.s_moduleInstance.TextureManager.getIcon(Icons.Template)
			};
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_detailPanel);
			((Control)val2).set_Visible(true);
			obj2.Panel = val2;
			list.Add(obj2);
			Tab obj3 = new Tab
			{
				Name = common.Gear,
				Icon = BuildsManager.s_moduleInstance.TextureManager.getIcon(Icons.Helmet)
			};
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)_detailPanel);
			((Control)val3).set_Visible(false);
			obj3.Panel = val3;
			list.Add(obj3);
			detailPanel.Tabs = list;
			_detailPanel.SelectedTab = _detailPanel.Tabs[0];
			((Control)_detailPanel.Tabs[0].Panel).add_Resized((EventHandler<ResizedEventArgs>)Panel_Resized);
			Control_Build control_Build = new Control_Build((Container)(object)_detailPanel.Tabs[0].Panel);
			((Control)control_Build).set_Parent((Container)(object)_detailPanel.Tabs[0].Panel);
			((Control)control_Build).set_Size(((Control)_detailPanel.Tabs[0].Panel).get_Size());
			control_Build.Scale = 1.0;
			_build = control_Build;
			Control_Equipment control_Equipment = new Control_Equipment((Container)(object)_detailPanel.Tabs[1].Panel);
			((Control)control_Equipment).set_Parent((Container)(object)_detailPanel.Tabs[1].Panel);
			((Control)control_Equipment).set_Size(((Control)_detailPanel.Tabs[1].Panel).get_Size());
			control_Equipment.Scale = 1.0;
			_gear = control_Equipment;
			((TextInputBase)_detailPanel.TemplateBox).set_Text(BuildsManager.s_moduleInstance.Selected_Template?.Build.ParseBuildCode());
			((TextInputBase)_detailPanel.GearBox).set_Text(BuildsManager.s_moduleInstance.Selected_Template?.Gear.TemplateCode);
			TextBox val4 = new TextBox();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(((Control)_detailPanel).get_Location().X + 5 + 32, 0));
			((Control)val4).set_Height(35);
			((Control)val4).set_Width(((Control)_detailPanel).get_Width() - 38 - 32 - 5);
			((TextInputBase)val4).set_Font(_font);
			((Control)val4).set_Visible(false);
			_nameBox = val4;
			_nameBox.add_EnterPressed((EventHandler<EventArgs>)NameBox_TextChanged);
			Label val5 = new Label();
			val5.set_Text("A Template Name");
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Location(new Point(((Control)_detailPanel).get_Location().X + 5 + 32, 0));
			((Control)val5).set_Height(35);
			((Control)val5).set_Width(((Control)_detailPanel).get_Width() - 38 - 32 - 5);
			val5.set_Font(_font);
			_nameLabel = val5;
			((Control)_nameLabel).add_Click((EventHandler<MouseEventArgs>)NameLabel_Click);
			BuildsManager.s_moduleInstance.Selected_Template_Edit += Selected_Template_Edit;
			BuildsManager.s_moduleInstance.Selected_Template_Changed += ModuleInstance_Selected_Template_Changed;
			BuildsManager.s_moduleInstance.Templates_Loaded += Templates_Loaded;
			BuildsManager.s_moduleInstance.Selected_Template_Redraw += Selected_Template_Redraw;
			_detailPanel.TemplateBox.add_EnterPressed((EventHandler<EventArgs>)TemplateBox_EnterPressed);
			_detailPanel.GearBox.add_EnterPressed((EventHandler<EventArgs>)GearBox_EnterPressed);
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)GlobalClick);
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
		}

		public void PlayerCharacter_NameChanged(object sender, ValueEventArgs<string> e)
		{
			if (BuildsManager.s_moduleInstance.ShowCurrentProfession.get_Value())
			{
				_templateSelection.SetSelection();
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			using (BuildsManager.s_moduleInstance.Selected_Template)
			{
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
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintBeforeChildren(spriteBatch, bounds);
			Rectangle rect = default(Rectangle);
			((Rectangle)(ref rect))._002Ector(((Control)_detailPanel).get_Location().X, 44, ((Control)_detailPanel).get_Width() - 38, 35);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _emptyTraitLine, rect, (Rectangle?)_emptyTraitLine.get_Bounds(), new Color(135, 135, 135, 255), 0f, default(Vector2), (SpriteEffects)0);
			Color color = Color.get_Black();
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 2, rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 1, rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 2, ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 1, ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			Rectangle localBounds = ((Control)_detailPanel).get_LocalBounds();
			((Rectangle)(ref rect))._002Ector(((Rectangle)(ref localBounds)).get_Right() - 35, 44, 35, 35);
			bool hovered = ((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition());
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, hovered ? _deleteHovered : _delete, rect, (Rectangle?)_delete.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			((Control)this).set_BasicTooltipText(hovered ? (common.Delete + " " + common.Template) : null);
			if (BuildsManager.s_moduleInstance.Selected_Template.Profession != null)
			{
				Template template = BuildsManager.s_moduleInstance.Selected_Template;
				Texture2D texture = BuildsManager.s_moduleInstance.TextureManager._Icons[0];
				if (template.Specialization != null)
				{
					texture = template.Specialization.ProfessionIconBig._AsyncTexture.get_Texture();
				}
				else if (template.Build.Profession != null)
				{
					texture = template.Build.Profession.IconBig._AsyncTexture.get_Texture();
				}
				((Rectangle)(ref rect))._002Ector(((Control)_detailPanel).get_Location().X + 2, 46, 30, 30);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, rect, (Rectangle?)texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).OnClick(e);
			Rectangle localBounds = ((Control)_detailPanel).get_LocalBounds();
			Rectangle rect = default(Rectangle);
			((Rectangle)(ref rect))._002Ector(((Rectangle)(ref localBounds)).get_Right() - 35, 44, 35, 35);
			if (((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition()) && BuildsManager.s_moduleInstance.Selected_Template.Path != null)
			{
				BuildsManager.s_moduleInstance.Selected_Template.Delete();
				BuildsManager.s_moduleInstance.Selected_Template = new Template();
			}
			((Control)_professionSelection).Hide();
		}

		protected override void DisposeControl()
		{
			((Control)_addButton).remove_Click((EventHandler<MouseEventArgs>)Button_Click);
			((Control)_addButton).Dispose();
			BuildsManager.s_moduleInstance.LanguageChanged -= ModuleInstance_LanguageChanged;
			BuildsManager.s_moduleInstance.Selected_Template_Edit -= Selected_Template_Edit;
			BuildsManager.s_moduleInstance.Selected_Template_Changed -= ModuleInstance_Selected_Template_Changed;
			BuildsManager.s_moduleInstance.Templates_Loaded -= Templates_Loaded;
			BuildsManager.s_moduleInstance.Selected_Template_Redraw -= Selected_Template_Redraw;
			((Control)_detailPanel.Tabs[0].Panel).remove_Resized((EventHandler<ResizedEventArgs>)Panel_Resized);
			_detailPanel.TemplateBox.remove_EnterPressed((EventHandler<EventArgs>)TemplateBox_EnterPressed);
			_detailPanel.GearBox.remove_EnterPressed((EventHandler<EventArgs>)GearBox_EnterPressed);
			Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)GlobalClick);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacter_NameChanged);
			_templateSelection.TemplateChanged -= TemplateSelection_TemplateChanged;
			((Control)_templateSelection).Dispose();
			_nameBox.remove_EnterPressed((EventHandler<EventArgs>)NameBox_TextChanged);
			((Control)_nameBox).Dispose();
			((Control)_nameLabel).remove_Click((EventHandler<MouseEventArgs>)NameLabel_Click);
			((Control)_nameLabel).Dispose();
			((Control)ImportButton).remove_Click((EventHandler<MouseEventArgs>)Import_Button_Click);
			((Control)ImportButton).Dispose();
			_professionSelection.Changed -= ProfessionSelection_Changed;
			((Control)_professionSelection).Dispose();
			((WindowBase2)this).DisposeControl();
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).OnShown(e);
			if (BuildsManager.s_moduleInstance.ShowCurrentProfession.get_Value())
			{
				_templateSelection.SetSelection();
			}
		}

		private void TemplateSelection_TemplateChanged(object sender, TemplateChangedEvent e)
		{
			BuildsManager.s_moduleInstance.Selected_Template = e.Template;
			((TextInputBase)_detailPanel.TemplateBox).set_Text(BuildsManager.s_moduleInstance.Selected_Template?.Build.TemplateCode);
			_nameLabel.set_Text(e.Template.Name);
			((Control)_nameLabel).Show();
			((Control)_nameBox).Hide();
		}

		private void Panel_Resized(object sender, ResizedEventArgs e)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			((Control)_gear).set_Size(((Control)_detailPanel.Tabs[0].Panel).get_Size().Add(new Point(0, -((Control)_detailPanel.GearBox).get_Bottom() + 30)));
		}

		private void ProfessionSelection_Changed(object sender, EventArgs e)
		{
			if (_professionSelection.SelectedProfession != null)
			{
				Template template = new Template
				{
					Profession = _professionSelection.SelectedProfession
				};
				template.Build.Profession = _professionSelection.SelectedProfession;
				BuildsManager.s_moduleInstance.Selected_Template = template;
				BuildsManager.s_moduleInstance.Templates.Add(BuildsManager.s_moduleInstance.Selected_Template);
				BuildsManager.s_moduleInstance.Selected_Template.SetChanged();
				_templateSelection.RefreshList();
				_professionSelection.SelectedProfession = null;
			}
		}

		private void Import_Button_Click(object sender, MouseEventArgs e)
		{
			BuildsManager.s_moduleInstance.ImportTemplates();
			_templateSelection.Refresh();
			((Control)ImportButton).Hide();
		}

		private void ModuleInstance_LanguageChanged(object sender, EventArgs e)
		{
			_addButton.Text = common.Create;
			ImportButton.Text = common.Create;
			_detailPanel.Tabs[0].Name = common.Build;
			_detailPanel.Tabs[1].Name = common.Gear;
		}

		private void GlobalClick(object sender, MouseEventArgs e)
		{
			if (!((Control)_nameBox).get_MouseOver())
			{
				((Control)_nameBox).set_Visible(false);
				((Control)_nameLabel).set_Visible(true);
			}
		}

		private void Selected_Template_Redraw(object sender, EventArgs e)
		{
			_build.SkillBar.ApplyBuild(sender, e);
			_gear.UpdateLayout();
		}

		private void GearBox_EnterPressed(object sender, EventArgs e)
		{
			GearTemplate gear = new GearTemplate(((TextInputBase)_detailPanel.GearBox).get_Text());
			if (gear != null)
			{
				BuildsManager.s_moduleInstance.Selected_Template.Gear = gear;
				BuildsManager.s_moduleInstance.Selected_Template.SetChanged();
				BuildsManager.s_moduleInstance.OnSelected_Template_Redraw(null, null);
			}
		}

		private void TemplateBox_EnterPressed(object sender, EventArgs e)
		{
			using BuildTemplate build = new BuildTemplate(((TextInputBase)_detailPanel.TemplateBox).get_Text());
			if (build == null || build.Profession == null)
			{
				return;
			}
			BuildsManager.s_moduleInstance.Selected_Template.Build = build;
			BuildsManager.s_moduleInstance.Selected_Template.Profession = build.Profession;
			foreach (SpecLine spec in BuildsManager.s_moduleInstance.Selected_Template.Build.SpecLines)
			{
				if (spec.Specialization?.Elite ?? false)
				{
					BuildsManager.s_moduleInstance.Selected_Template.Specialization = spec.Specialization;
					break;
				}
			}
			BuildsManager.s_moduleInstance.Selected_Template.SetChanged();
			BuildsManager.s_moduleInstance.OnSelected_Template_Redraw(null, null);
		}

		private void NameBox_TextChanged(object sender, EventArgs e)
		{
			BuildsManager.s_moduleInstance.Selected_Template.Name = ((TextInputBase)_nameBox).get_Text();
			BuildsManager.s_moduleInstance.Selected_Template.Save();
			((Control)_nameLabel).set_Visible(true);
			((Control)_nameBox).set_Visible(false);
			_nameLabel.set_Text(BuildsManager.s_moduleInstance.Selected_Template.Name);
			_templateSelection.RefreshList();
		}

		private void NameLabel_Click(object sender, MouseEventArgs e)
		{
			if (BuildsManager.s_moduleInstance.Selected_Template?.Path != null)
			{
				((Control)_nameLabel).set_Visible(false);
				((Control)_nameBox).set_Visible(true);
				((TextInputBase)_nameBox).set_Text(_nameLabel.get_Text());
				((TextInputBase)_nameBox).set_SelectionStart(0);
				((TextInputBase)_nameBox).set_SelectionEnd(((TextInputBase)_nameBox).get_Text().Length);
				((TextInputBase)_nameBox).set_Focused(true);
			}
		}

		private void Button_Click(object sender, MouseEventArgs e)
		{
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			_professions = new List<SelectionPopUp.SelectionEntry>();
			foreach (API.Profession profession in BuildsManager.s_moduleInstance.Data.Professions)
			{
				_professions.Add(new SelectionPopUp.SelectionEntry
				{
					Object = profession,
					Texture = AsyncTexture2D.op_Implicit(profession.IconBig._AsyncTexture.get_Texture()),
					Header = profession.Name,
					Content = new List<string>(),
					ContentTextures = new List<AsyncTexture2D>()
				});
			}
			_professionSelection.List = _professions;
			((Control)_professionSelection).Show();
			((Control)_professionSelection).set_Location(((Control)_addButton).get_Location().Add(new Point(((Control)_addButton).get_Width() + 5, 0)));
			_professionSelection.SelectionType = SelectionPopUp.selectionType.Profession;
			_professionSelection.SelectionTarget = BuildsManager.s_moduleInstance.Selected_Template.Profession;
			((Control)_professionSelection).set_Width(175);
			_professionSelection.SelectionTarget = null;
			_professionSelection.List = _professions;
		}

		private void Templates_Loaded(object sender, EventArgs e)
		{
			((Control)_templateSelection).Invalidate();
		}

		private void ModuleInstance_Selected_Template_Changed(object sender, EventArgs e)
		{
			_nameLabel.set_Text(BuildsManager.s_moduleInstance.Selected_Template.Name);
			((TextInputBase)_detailPanel.TemplateBox).set_Text(BuildsManager.s_moduleInstance.Selected_Template.Build.TemplateCode);
			((TextInputBase)_detailPanel.GearBox).set_Text(BuildsManager.s_moduleInstance.Selected_Template.Gear.TemplateCode);
		}

		private void Selected_Template_Edit(object sender, EventArgs e)
		{
			BuildsManager.s_moduleInstance.Selected_Template.Specialization = null;
			foreach (SpecLine spec in BuildsManager.s_moduleInstance.Selected_Template.Build.SpecLines)
			{
				if (spec.Specialization?.Elite ?? false)
				{
					BuildsManager.s_moduleInstance.Selected_Template.Specialization = spec.Specialization;
					break;
				}
			}
			((TextInputBase)_detailPanel.TemplateBox).set_Text(BuildsManager.s_moduleInstance.Selected_Template.Build.ParseBuildCode());
			((TextInputBase)_detailPanel.GearBox).set_Text(BuildsManager.s_moduleInstance.Selected_Template?.Gear.TemplateCode);
			_templateSelection.Refresh();
		}
	}
}
