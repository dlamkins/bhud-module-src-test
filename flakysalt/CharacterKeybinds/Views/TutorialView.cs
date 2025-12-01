using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Model;
using flakysalt.CharacterKeybinds.Resources;
using flakysalt.CharacterKeybinds.Services;

namespace flakysalt.CharacterKeybinds.Views
{
	public class TutorialView : View
	{
		private Label descriptionTextBox;

		private Label headerTextBox;

		private Label panelCounterTextBox;

		private Image tutorialImage;

		private Image CloseButton;

		private StandardButton NextButton;

		private StandardButton PreviousButton;

		private FlowPanel panelCounterFlowPanel;

		private Panel mainPanel;

		private TutorialData data;

		private int currentPanelIndex;

		private readonly Action onCloseAction;

		private CharacterKeybindsSettings _settings;

		public static TutorialView Instance { get; private set; }

		public TutorialView(CharacterKeybindsSettings settings)
			: this()
		{
			Instance = this;
			_settings = settings;
			((View<IPresenter>)(object)this).Build((Container)(object)GameService.Graphics.get_SpriteScreen());
		}

		protected sealed override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Expected O, but got Unknown
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Expected O, but got Unknown
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Expected O, but got Unknown
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Expected O, but got Unknown
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Expected O, but got Unknown
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(((Control)buildPanel).get_Width());
			((Control)val).set_Height(((Control)buildPanel).get_Height());
			((Control)val).set_BackgroundColor(new Color(0, 0, 0, 230));
			((Control)val).set_Visible(false);
			mainPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)mainPanel);
			((Control)val2).set_Width(((Control)mainPanel).get_Width());
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Location(new Point(0, 50));
			val2.set_Font(GameService.Content.get_DefaultFont32());
			headerTextBox = val2;
			CalculateCenteredHorizontalPosition((Control)(object)mainPanel, (Control)(object)headerTextBox);
			Image val3 = new Image();
			((Control)val3).set_Parent((Container)(object)mainPanel);
			((Control)val3).set_Location(new Point(0, 150));
			((Control)val3).set_Width(1152);
			((Control)val3).set_Height(648);
			tutorialImage = val3;
			CalculateCenteredHorizontalPosition((Control)(object)mainPanel, (Control)(object)tutorialImage);
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)mainPanel);
			val4.set_HorizontalAlignment((HorizontalAlignment)1);
			val4.set_VerticalAlignment((VerticalAlignment)0);
			((Control)val4).set_Width(Math.Min(((Control)mainPanel).get_Width() - 100, 800));
			((Control)val4).set_Height(800);
			((Control)val4).set_Location(new Point(0, ((Control)tutorialImage).get_Bottom() + 50));
			val4.set_Font(GameService.Content.get_DefaultFont18());
			descriptionTextBox = val4;
			CalculateCenteredHorizontalPosition((Control)(object)mainPanel, (Control)(object)descriptionTextBox);
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)mainPanel);
			((Control)val5).set_Height(50);
			((Control)val5).set_Width(500);
			val5.set_ControlPadding(new Vector2(10f, 0f));
			val5.set_FlowDirection((ControlFlowDirection)0);
			((Control)val5).set_Location(new Point(0, ((Control)mainPanel).get_Bottom() - 200));
			panelCounterFlowPanel = val5;
			CalculateCenteredHorizontalPosition((Control)(object)mainPanel, (Control)(object)panelCounterFlowPanel);
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)panelCounterFlowPanel);
			((Control)val6).set_Width(150);
			((Control)val6).set_Height(((Control)panelCounterFlowPanel).get_Height());
			val6.set_Text(TutorialLoca.previousButtonText);
			PreviousButton = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)panelCounterFlowPanel);
			val7.set_HorizontalAlignment((HorizontalAlignment)1);
			((Control)val7).set_Width(150);
			((Control)val7).set_Height(((Control)panelCounterFlowPanel).get_Height());
			panelCounterTextBox = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)panelCounterFlowPanel);
			val8.set_Text(TutorialLoca.nextButtonText);
			((Control)val8).set_Width(150);
			((Control)val8).set_Height(((Control)panelCounterFlowPanel).get_Height());
			NextButton = val8;
			Image val9 = new Image(AsyncTexture2D.FromAssetId(156012));
			((Control)val9).set_Parent((Container)(object)mainPanel);
			((Control)val9).set_Width(64);
			((Control)val9).set_Height(64);
			((Control)val9).set_Location(new Point(((Control)mainPanel).get_Width() - 74, 10));
			((Control)val9).set_BasicTooltipText(TutorialLoca.closeButtonText);
			CloseButton = val9;
			((Control)CloseButton).add_Click((EventHandler<MouseEventArgs>)CloseButtonOnClick);
			((Control)mainPanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!((Control)PreviousButton).get_MouseOver() && !((Control)NextButton).get_MouseOver() && !((Control)CloseButton).get_MouseOver() && currentPanelIndex < data.Panels.Count - 1)
				{
					currentPanelIndex++;
					UpdateContent(data.Panels[currentPanelIndex]);
				}
			});
			((Control)NextButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (currentPanelIndex < data.Panels.Count - 1)
				{
					currentPanelIndex++;
					UpdateContent(data.Panels[currentPanelIndex]);
				}
			});
			((Control)PreviousButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (currentPanelIndex > 0)
				{
					currentPanelIndex--;
					UpdateContent(data.Panels[currentPanelIndex]);
				}
			});
			((Control)buildPanel).add_Resized((EventHandler<ResizedEventArgs>)OnResize);
		}

		private void OnResize(object sender, ResizedEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			((Control)mainPanel).set_Size(((Control)((Control)mainPanel).get_Parent()).get_Size());
			((Control)panelCounterFlowPanel).set_Location(new Point(((Control)panelCounterFlowPanel).get_Location().X, ((Control)mainPanel).get_Bottom() - 200));
			((Control)CloseButton).set_Location(new Point(((Control)mainPanel).get_Width() - 74, 10));
			((Control)headerTextBox).set_Width(((Control)mainPanel).get_Width());
			((Control)descriptionTextBox).set_Width(Math.Min(((Control)mainPanel).get_Width() - 100, 800));
			CalculateCenteredHorizontalPosition((Control)(object)mainPanel, (Control)(object)headerTextBox);
			CalculateCenteredHorizontalPosition((Control)(object)mainPanel, (Control)(object)tutorialImage);
			CalculateCenteredHorizontalPosition((Control)(object)mainPanel, (Control)(object)descriptionTextBox);
			CalculateCenteredHorizontalPosition((Control)(object)mainPanel, (Control)(object)panelCounterFlowPanel);
		}

		public void Show(TutorialData data)
		{
			this.data = data;
			currentPanelIndex = 0;
			headerTextBox.set_Text(data.Header);
			UpdateContent(data.Panels[currentPanelIndex]);
			((Control)mainPanel).Show();
			OnResize(this, null);
		}

		private void UpdateContent(TutorialPanel panelData)
		{
			((Control)NextButton).set_Enabled(currentPanelIndex < data.Panels.Count - 1);
			((Control)PreviousButton).set_Enabled(currentPanelIndex > 0);
			tutorialImage.set_Texture(AsyncTexture2D.op_Implicit(ContentService.Instance.GetTexture(panelData.ImagePath)));
			descriptionTextBox.set_Text(panelData.Description);
			panelCounterTextBox.set_Text($"{currentPanelIndex + 1} / {data.Panels.Count}");
		}

		private void CalculateCenteredHorizontalPosition(Control container, Control objectToCenter)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			objectToCenter.set_Location(new Point((container.get_Width() - objectToCenter.get_Width()) / 2, objectToCenter.get_Location().Y));
		}

		private void CloseButtonOnClick(object sender, EventArgs e)
		{
			((Control)mainPanel).Hide();
			_settings.experiencedFtue.set_Value(true);
		}
	}
}
