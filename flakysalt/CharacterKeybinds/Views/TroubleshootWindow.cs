using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Mumble.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Views.UiElements;

namespace flakysalt.CharacterKeybinds.Views
{
	public class TroubleshootWindow
	{
		public StandardWindow WindowView;

		private StandardButton ToggleVisibilityButton;

		private StandardButton resetPositionButton;

		private StandardButton testClickerButton;

		private bool markerVisible;

		private List<DraggableMarker> markers = new List<DraggableMarker>();

		private CharacterKeybindsSettings settingsModel;

		public async void Init(CharacterKeybindsSettings settingsModel, ContentsManager ContentsManager)
		{
			this.settingsModel = settingsModel;
			AsyncTexture2D windowBackgroundTexture = AsyncTexture2D.FromAssetId(155997);
			Texture2D _emblem = ContentsManager.GetTexture("images/logo.png");
			TroubleshootWindow troubleshootWindow = this;
			StandardWindow val = new StandardWindow(windowBackgroundTexture, new Rectangle(25, 26, 560, 649), new Rectangle(40, 50, 540, 590), new Point(560, 400));
			((WindowBase2)val).set_Emblem(_emblem);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Troubleshoot Window");
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("flakysalt_TroubleshootWindow");
			((WindowBase2)val).set_CanClose(true);
			troubleshootWindow.WindowView = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Size(((Control)WindowView).get_Size());
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_ControlPadding(new Vector2(5f, 2f));
			val2.set_OuterControlPadding(new Vector2(0f, 15f));
			((Control)val2).set_Parent((Container)(object)WindowView);
			FlowPanel mainFlowPanel = val2;
			Label val3 = new Label();
			val3.set_Text("Only change the position of the markes if you erxperience problems. \n\nEnable the markers by pressing 'Toggle Marker Visibility'\nMove the marker to the following positions if they do not allign automatically:\n1. The Options menu tab\n2. The dropdown at the bottom right of the options menu\n3. The first entry of the dropdown when opening it\n4. The 'Yes' button of the confirmation popup when importing key binds");
			((Control)val3).set_Width(((Control)mainFlowPanel).get_Width());
			((Control)val3).set_Height(150);
			((Control)val3).set_Parent((Container)(object)mainFlowPanel);
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Size(((Control)WindowView).get_Size());
			val4.set_FlowDirection((ControlFlowDirection)0);
			val4.set_ControlPadding(new Vector2(5f, 2f));
			val4.set_OuterControlPadding(new Vector2(0f, 15f));
			((Control)val4).set_Parent((Container)(object)mainFlowPanel);
			FlowPanel buttonFlowPanel = val4;
			TroubleshootWindow troubleshootWindow2 = this;
			StandardButton val5 = new StandardButton();
			val5.set_Text("Toggle Marker Visibility");
			((Control)val5).set_Width(160);
			((Control)val5).set_Parent((Container)(object)buttonFlowPanel);
			troubleshootWindow2.ToggleVisibilityButton = val5;
			TroubleshootWindow troubleshootWindow3 = this;
			StandardButton val6 = new StandardButton();
			val6.set_Text("Reset Marker Positions");
			((Control)val6).set_Width(160);
			((Control)val6).set_Parent((Container)(object)buttonFlowPanel);
			troubleshootWindow3.resetPositionButton = val6;
			TroubleshootWindow troubleshootWindow4 = this;
			StandardButton val7 = new StandardButton();
			val7.set_Text("Test Markers");
			((Control)val7).set_BasicTooltipText("This will simulate the sequence of clicks");
			((Control)val7).set_Width(160);
			((Control)val7).set_Parent((Container)(object)buttonFlowPanel);
			troubleshootWindow4.testClickerButton = val7;
			((Control)resetPositionButton).add_Click((EventHandler<MouseEventArgs>)ResetMarkerPositions);
			((Control)testClickerButton).add_Click((EventHandler<MouseEventArgs>)TestClickerButton_Click);
			((Control)ToggleVisibilityButton).add_Click((EventHandler<MouseEventArgs>)ToggleVisibilityButton_Click);
			((Control)GameService.Graphics.get_SpriteScreen()).add_Resized((EventHandler<ResizedEventArgs>)SpriteScreen_Resized);
			((Control)WindowView).add_Hidden((EventHandler<EventArgs>)AutoClickWindow_Hidden);
			SpawnImportClickZones();
		}

		private void TestClickerButton_Click(object sender, MouseEventArgs e)
		{
			Task.Run((Func<Task>)ClickInOrder);
		}

		private void AutoClickWindow_Hidden(object sender, EventArgs e)
		{
			markerVisible = false;
			foreach (DraggableMarker marker in markers)
			{
				((Control)marker).set_Visible(markerVisible);
			}
		}

		public void Dispose()
		{
			((Control)resetPositionButton).remove_Click((EventHandler<MouseEventArgs>)ResetMarkerPositions);
			((Control)ToggleVisibilityButton).remove_Click((EventHandler<MouseEventArgs>)ToggleVisibilityButton_Click);
			((Control)GameService.Graphics.get_SpriteScreen()).remove_Resized((EventHandler<ResizedEventArgs>)SpriteScreen_Resized);
			for (int i = 0; i < markers.Count; i++)
			{
				markers[i].OnMarkerReleased -= Marker_OnMarkerReleased;
				((Control)markers[i]).Dispose();
			}
			markers.Clear();
			StandardWindow windowView = WindowView;
			if (windowView != null)
			{
				((Control)windowView).Dispose();
			}
		}

		private void SpriteScreen_Resized(object sender, ResizedEventArgs e)
		{
			SetMarkerPositions();
		}

		private void UI_UISizeChanged(object sender, ValueEventArgs<UiSize> e)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			((Control)markers[0]).set_Location(new Point((int)((float)((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X * 0.6f), (int)((float)((Control)GameService.Graphics.get_SpriteScreen()).get_Size().Y * 0.6f)));
		}

		private void ToggleVisibilityButton_Click(object sender, MouseEventArgs e)
		{
			markerVisible = !markerVisible;
			foreach (DraggableMarker marker in markers)
			{
				((Control)marker).set_Visible(markerVisible);
			}
		}

		private void SetMarkerPositions()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < settingsModel.clickPositions.get_Value().Count; i++)
			{
				((Control)markers[i]).set_Location(ScreenScenter() + settingsModel.clickPositions.get_Value()[i]);
			}
		}

		private void ResetMarkerPositions(object sender, MouseEventArgs e)
		{
			settingsModel.clickPositions.set_Value(ClickPosLocations.importMarkerLocations);
			SetMarkerPositions();
		}

		private void SimulateClick_Click(object sender, MouseEventArgs e)
		{
			Task.Run((Func<Task>)ClickInOrder);
		}

		public async Task ClickInOrder()
		{
			ScreenNotification.ShowNotification("Switching keybinds... ", (NotificationType)6, (Texture2D)null, 3);
			Keys keyboardShortcut = settingsModel.optionsKeybind.get_Value().get_PrimaryKey();
			await Task.Delay(1000);
			Keyboard.Stroke((VirtualKeyShort)(short)keyboardShortcut, false);
			await Task.Delay(300);
			foreach (DraggableMarker marker in markers)
			{
				marker.SimulateClick();
				await Task.Delay(200);
			}
			await Task.Delay(500);
			Keyboard.Stroke((VirtualKeyShort)(short)keyboardShortcut, false);
		}

		private void SpawnImportClickZones()
		{
			for (int i = 0; i < 4; i++)
			{
				DraggableMarker draggableMarker = new DraggableMarker(i + 1);
				((Control)draggableMarker).set_Visible(false);
				DraggableMarker clickZone = draggableMarker;
				markers.Add(clickZone);
			}
			SetMarkerPositions();
			foreach (DraggableMarker marker in markers)
			{
				marker.OnMarkerReleased += Marker_OnMarkerReleased;
			}
		}

		private void Marker_OnMarkerReleased(object sender, Point e)
		{
			settingsModel.clickPositions.set_Value(markers.Select((DraggableMarker marker) => ((Control)marker).get_Location() - ScreenScenter()).ToList());
		}

		private Point ScreenScenter()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			return new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Size().X / 2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Size().Y / 2);
		}
	}
}
