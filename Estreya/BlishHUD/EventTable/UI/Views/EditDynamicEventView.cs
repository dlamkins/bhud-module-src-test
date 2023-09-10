using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Threading.Events;
using Estreya.BlishHUD.Shared.UI.Views;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class EditDynamicEventView : BaseView
	{
		private Map _selectedMap;

		private List<Map> _maps;

		private DynamicEvent _dynamicEvent;

		private bool _isRemoveVisibile;

		public event AsyncEventHandler<DynamicEvent> SaveClicked;

		public event AsyncEventHandler<DynamicEvent> RemoveClicked;

		public event EventHandler CloseRequested;

		public EditDynamicEventView(DynamicEvent dynamicEvent, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService)
			: base(apiManager, iconService, translationService)
		{
			_dynamicEvent = dynamicEvent;
			_isRemoveVisibile = _dynamicEvent != null && _dynamicEvent.IsCustom;
		}

		private Vector3 GetCurrentPosition()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if (_selectedMap == null)
			{
				return Vector3.get_Zero();
			}
			Vector2 v2 = _selectedMap.WorldMeterCoordsToMapCoords(GameService.Gw2Mumble.get_PlayerCharacter().get_Position());
			return new Vector3(v2.X, v2.Y, GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z.ToInches());
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Expected O, but got Unknown
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Expected O, but got Unknown
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Expected O, but got Unknown
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Expected O, but got Unknown
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0546: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			int currentMapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			if (_selectedMap == null)
			{
				_selectedMap = _maps.FirstOrDefault((Map m) => m.get_Id() == currentMapId);
			}
			Vector3 currentPosition = GetCurrentPosition();
			if (_dynamicEvent == null)
			{
				_dynamicEvent = new DynamicEvent
				{
					Name = string.Empty,
					IsCustom = true,
					ID = Guid.NewGuid().ToString(),
					MapId = currentMapId,
					Location = new DynamicEvent.DynamicEventLocation
					{
						Points = new float[1][] { new float[3] { currentPosition.X, currentPosition.Y, 0f } },
						Center = new float[3] { currentPosition.X, currentPosition.Y, currentPosition.Z },
						ZRange = new float[2] { -50f, 50f }
					}
				};
			}
			((Container)parent).ClearChildren();
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Control)val2).set_Width(((Container)parent).get_ContentRegion().Width);
			((Panel)val2).set_CanScroll(true);
			((Control)val2).set_Height(((Container)parent).get_ContentRegion().Height - 39);
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_OuterControlPadding(new Vector2(20f, 20f));
			val2.set_ControlPadding(new Vector2(0f, 10f));
			FlowPanel flowPanel = val2;
			((Control)RenderTextbox((Panel)(object)flowPanel, Point.get_Zero(), ((Container)flowPanel).get_ContentRegion().Width - (int)flowPanel.get_OuterControlPadding().X * 2, _dynamicEvent.Name, "Name", delegate(string val)
			{
				_dynamicEvent.Name = val;
			})).set_BasicTooltipText("Defines the name of the dynamic event. It is shown in the tooltip on the (mini)map.");
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)flowPanel);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(((Container)flowPanel).get_ContentRegion().Width - (int)flowPanel.get_OuterControlPadding().X * 2);
			Panel mapSelectionGroup = val3;
			((Control)RenderLabel(mapSelectionGroup, "Map").TitleLabel).set_Width(base.LABEL_WIDTH);
			Dropdown<string> dropdown = RenderDropdown(mapSelectionGroup, new Point(base.LABEL_WIDTH, 0), ((Container)mapSelectionGroup).get_ContentRegion().Width - base.LABEL_WIDTH, _maps.OrderBy((Map m) => m.get_Name()).Select(GetMapDisplayName).ToArray(), (_selectedMap == null) ? null : GetMapDisplayName(_selectedMap), delegate(string newMapDisplayName)
			{
				if (int.TryParse(new Regex(".*?\\( ID: (\\d+) \\)").Match(newMapDisplayName).Groups[1].Value, out var id))
				{
					_selectedMap = _maps.FirstOrDefault((Map m) => m.get_Id() == id);
				}
				InternalBuild(parent);
			});
			((Control)dropdown).set_Enabled(false);
			dropdown.PanelHeight = 200;
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)flowPanel);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			((Control)val4).set_Width(((Container)flowPanel).get_ContentRegion().Width - (int)flowPanel.get_OuterControlPadding().X * 2);
			Panel typeSelectionGroup = val4;
			((Control)RenderLabel(typeSelectionGroup, "Type").TitleLabel).set_Width(base.LABEL_WIDTH);
			Dropdown<string> dropdown2 = RenderDropdown(typeSelectionGroup, new Point(base.LABEL_WIDTH, 0), ((Container)typeSelectionGroup).get_ContentRegion().Width - base.LABEL_WIDTH, new string[3] { "sphere", "cylinder", "poly" }, _dynamicEvent.Location.Type, delegate(string newType)
			{
				_dynamicEvent.Location.Type = newType;
			});
			dropdown2.PanelHeight = 200;
			((Control)dropdown2).set_BasicTooltipText("Defines the type of the type of the dynamic event.");
			RenderLevelGroup(flowPanel, _dynamicEvent);
			RenderColorGroup(flowPanel, _dynamicEvent);
			FlowPanel val5 = new FlowPanel();
			((Control)val5).set_Parent((Container)(object)flowPanel);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			((Control)val5).set_Width(((Container)flowPanel).get_ContentRegion().Width - (int)flowPanel.get_OuterControlPadding().X * 2);
			FlowPanel typePropertiesPanel = val5;
			RenderTypeProperties(typePropertiesPanel, _dynamicEvent);
			dropdown2.ValueChanged += delegate
			{
				RenderTypeProperties(typePropertiesPanel, _dynamicEvent);
			};
			Func<Task> saveAction = async delegate
			{
				await (this.SaveClicked?.Invoke(this, _dynamicEvent) ?? Task.CompletedTask);
			};
			Func<Task> saveAndCloseAction = async delegate
			{
				await saveAction();
				this.CloseRequested?.Invoke(this, EventArgs.Empty);
			};
			Func<Task> removeAction = async delegate
			{
				await (this.RemoveClicked?.Invoke(this, _dynamicEvent) ?? Task.CompletedTask);
				this.CloseRequested?.Invoke(this, EventArgs.Empty);
			};
			Button saveButton = RenderButtonAsync(parent, "Save", saveAction);
			((Control)saveButton).set_Location(new Point(((Control)parent).get_Right() - ((Control)saveButton).get_Width() - (int)flowPanel.get_OuterControlPadding().X, ((Control)parent).get_Bottom() - ((Control)saveButton).get_Height()));
			Button saveAndCloseButton = RenderButtonAsync(parent, "Save & Close", saveAndCloseAction);
			((Control)saveAndCloseButton).set_Right(((Control)saveButton).get_Left() - 2);
			((Control)saveAndCloseButton).set_Top(((Control)saveButton).get_Top());
			if (_isRemoveVisibile)
			{
				((Control)RenderButtonAsync(parent, "Remove", removeAction)).set_Location(new Point(((Control)parent).get_Left() + (int)flowPanel.get_OuterControlPadding().X, ((Control)saveAndCloseButton).get_Top()));
			}
		}

		private void RenderLevelGroup(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
			Panel levelGroup = val2;
			((Control)RenderLabel(levelGroup, "Level").TitleLabel).set_Width(base.LABEL_WIDTH);
			((Control)RenderTextbox(levelGroup, new Point(base.LABEL_WIDTH, 0), ((Container)levelGroup).get_ContentRegion().Width - base.LABEL_WIDTH, dynamicEvent.Level.ToString(), "Level", delegate(string val)
			{
				if (string.IsNullOrWhiteSpace(val))
				{
					val = "0";
				}
				dynamicEvent.Level = int.Parse(val);
			}, null, clearOnEnter: false, delegate(string oldVal, string newVal)
			{
				if (string.IsNullOrWhiteSpace(newVal))
				{
					return Task.FromResult(result: true);
				}
				if (!int.TryParse(newVal, out var result))
				{
					ShowError("Not a valid int value.");
					return Task.FromResult(result: false);
				}
				if (result < 0)
				{
					ShowError("Level can't be negative.");
					return Task.FromResult(result: false);
				}
				if (result > 80)
				{
					ShowError("Level can't be larger than 80.");
					return Task.FromResult(result: false);
				}
				return Task.FromResult(result: true);
			})).set_BasicTooltipText("Defines the recommended level of the dynamic event.");
		}

		private void RenderColorGroup(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
			Panel group = val2;
			((Control)RenderLabel(group, "Color").TitleLabel).set_Width(base.LABEL_WIDTH);
			((Control)RenderTextbox(group, new Point(base.LABEL_WIDTH, 0), ((Container)group).get_ContentRegion().Width - base.LABEL_WIDTH, dynamicEvent.ColorCode, "Color", delegate(string val)
			{
				dynamicEvent.ColorCode = val;
			})).set_BasicTooltipText("Defines the color of the dynamic event. Empty = Default color");
		}

		private void RenderTypeProperties(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			((Container)parent).ClearChildren();
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_Title("Location");
			((Panel)val).set_CanCollapse(true);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			val.set_ControlPadding(new Vector2(0f, 2f));
			FlowPanel locationProperties = val;
			RenderLocationCenter(locationProperties, dynamicEvent);
			RenderEmptyLine((Panel)(object)locationProperties, 20);
			switch (dynamicEvent.Location.Type?.ToLowerInvariant())
			{
			case "poly":
				RenderPolygoneProperties(locationProperties, dynamicEvent);
				break;
			case "cylinder":
				RenderCylinderProperties(locationProperties, dynamicEvent);
				break;
			case "sphere":
				RenderSphereProperties(locationProperties, dynamicEvent);
				break;
			}
		}

		private void RenderLocationCenter(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Control)val2).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Panel)val2).set_Title("Center");
			val2.set_OuterControlPadding(new Vector2(20f, 20f));
			val2.set_ControlPadding(new Vector2(2f, 0f));
			((Panel)val2).set_CanCollapse(true);
			val2.set_FlowDirection((ControlFlowDirection)2);
			FlowPanel centerSectionPanel = val2;
			TextBox xPos = RenderTextbox((Panel)(object)centerSectionPanel, Point.get_Zero(), 150, dynamicEvent.Location.Center[0].ToString(CultureInfo.CurrentCulture), "X Position", delegate(string val)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(val))
					{
						val = "0";
					}
					dynamicEvent.Location.Center[0] = float.Parse(val, CultureInfo.CurrentCulture);
				}
				catch (Exception ex3)
				{
					ShowError(ex3.Message);
				}
			});
			((Control)xPos).set_BasicTooltipText("Defines the center position on the x-axis.");
			TextBox yPos = RenderTextbox((Panel)(object)centerSectionPanel, Point.get_Zero(), 150, dynamicEvent.Location.Center[1].ToString(CultureInfo.CurrentCulture), "Y Position", delegate(string val)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(val))
					{
						val = "0";
					}
					dynamicEvent.Location.Center[1] = float.Parse(val, CultureInfo.CurrentCulture);
				}
				catch (Exception ex2)
				{
					ShowError(ex2.Message);
				}
			});
			((Control)yPos).set_BasicTooltipText("Defines the center position on the y-axis.");
			TextBox zPos = RenderTextbox((Panel)(object)centerSectionPanel, Point.get_Zero(), 150, dynamicEvent.Location.Center[2].ToString(CultureInfo.CurrentCulture), "Z Position", delegate(string val)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(val))
					{
						val = "0";
					}
					dynamicEvent.Location.Center[2] = float.Parse(val, CultureInfo.CurrentCulture);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			});
			((Control)zPos).set_BasicTooltipText("Defines the center position on the z-axis.");
			((Control)RenderButton((Panel)(object)centerSectionPanel, "Set current Position", delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Vector3 currentPosition = GetCurrentPosition();
				((TextInputBase)xPos).set_Text(currentPosition.X.ToString(CultureInfo.CurrentCulture));
				((TextInputBase)yPos).set_Text(currentPosition.Y.ToString(CultureInfo.CurrentCulture));
				((TextInputBase)zPos).set_Text(currentPosition.Z.ToString(CultureInfo.CurrentCulture));
			})).set_BasicTooltipText("Sets the position values to the current position reported by mumblelink.");
		}

		private void RenderCylinderProperties(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			if (dynamicEvent != null && !(dynamicEvent.Location.Type.ToLowerInvariant() != "cylinder"))
			{
				RenderHeightSection(parent, dynamicEvent);
				RenderRadiusSection(parent, dynamicEvent);
			}
		}

		private void RenderSphereProperties(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			if (dynamicEvent != null && !(dynamicEvent.Location.Type.ToLowerInvariant() != "sphere"))
			{
				RenderRadiusSection(parent, dynamicEvent);
			}
		}

		private void RenderHeightSection(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val2).set_Title("Height");
			((Panel)val2).set_CanCollapse(true);
			val2.set_OuterControlPadding(new Vector2(20f, 20f));
			val2.set_ControlPadding(new Vector2(0f, 2f));
			FlowPanel heightPanelWrapper = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)heightPanelWrapper);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(((Container)heightPanelWrapper).get_ContentRegion().Width - (int)heightPanelWrapper.get_OuterControlPadding().X * 2);
			Panel group = val3;
			((Control)RenderLabel(group, "Height").TitleLabel).set_Width(base.LABEL_WIDTH);
			((Control)RenderTextbox(group, new Point(base.LABEL_WIDTH, 0), ((Container)group).get_ContentRegion().Width - base.LABEL_WIDTH, dynamicEvent.Location.Height.ToString(CultureInfo.CurrentCulture), "Height", delegate(string val)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(val))
					{
						val = "0";
					}
					dynamicEvent.Location.Height = float.Parse(val, CultureInfo.CurrentCulture);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			})).set_BasicTooltipText("Defines the height of the dynamic event in inches.");
			RenderEmptyLine((Panel)(object)heightPanelWrapper, (int)heightPanelWrapper.get_OuterControlPadding().X);
		}

		private void RenderRadiusSection(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val2).set_Title("Radius");
			((Panel)val2).set_CanCollapse(true);
			val2.set_OuterControlPadding(new Vector2(20f, 20f));
			val2.set_ControlPadding(new Vector2(0f, 2f));
			FlowPanel panelWrapper = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)panelWrapper);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(((Container)panelWrapper).get_ContentRegion().Width - (int)panelWrapper.get_OuterControlPadding().X * 2);
			Panel group = val3;
			((Control)RenderLabel(group, "Radius").TitleLabel).set_Width(base.LABEL_WIDTH);
			((Control)RenderTextbox(group, new Point(base.LABEL_WIDTH, 0), ((Container)group).get_ContentRegion().Width - base.LABEL_WIDTH, dynamicEvent.Location.Radius.ToString(CultureInfo.CurrentCulture), "Radius", delegate(string val)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(val))
					{
						val = "0";
					}
					dynamicEvent.Location.Radius = float.Parse(val, CultureInfo.CurrentCulture);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			})).set_BasicTooltipText("Defines the radius of the dynamic event in inches.");
			RenderEmptyLine((Panel)(object)panelWrapper, (int)panelWrapper.get_OuterControlPadding().X);
		}

		private void RenderPolygoneProperties(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Expected O, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Expected O, but got Unknown
			if (dynamicEvent != null && !(dynamicEvent.Location.Type.ToLowerInvariant() != "poly"))
			{
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent((Container)(object)parent);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
				val.set_FlowDirection((ControlFlowDirection)3);
				((Panel)val).set_Title("Points");
				((Panel)val).set_CanCollapse(true);
				val.set_OuterControlPadding(new Vector2(20f, 20f));
				val.set_ControlPadding(new Vector2(0f, 2f));
				FlowPanel pointsPanelWrapper = val;
				FlowPanel val2 = new FlowPanel();
				((Control)val2).set_Parent((Container)(object)pointsPanelWrapper);
				((Container)val2).set_HeightSizingMode((SizingMode)1);
				((Control)val2).set_Width(((Container)pointsPanelWrapper).get_ContentRegion().Width - (int)pointsPanelWrapper.get_OuterControlPadding().X * 2);
				val2.set_FlowDirection((ControlFlowDirection)3);
				FlowPanel pointsPanel = val2;
				RenderPolygonePoints(pointsPanel, dynamicEvent);
				RenderEmptyLine((Panel)(object)pointsPanelWrapper, (int)pointsPanelWrapper.get_OuterControlPadding().Y);
				FlowPanel val3 = new FlowPanel();
				((Control)val3).set_Parent((Container)(object)parent);
				((Container)val3).set_HeightSizingMode((SizingMode)1);
				((Control)val3).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
				val3.set_FlowDirection((ControlFlowDirection)3);
				((Panel)val3).set_Title("Z Range");
				((Panel)val3).set_CanCollapse(true);
				val3.set_OuterControlPadding(new Vector2(20f, 20f));
				val3.set_ControlPadding(new Vector2(0f, 2f));
				FlowPanel zRangePanel = val3;
				RenderPolygoneZRange(zRangePanel, dynamicEvent);
				RenderEmptyLine((Panel)(object)zRangePanel, 20);
			}
		}

		private void RenderPolygoneZRange(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Expected O, but got Unknown
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
			Panel topGroup = val2;
			((Control)RenderLabel(topGroup, "Top").TitleLabel).set_Width(base.LABEL_WIDTH);
			((Control)RenderTextbox(topGroup, new Point(base.LABEL_WIDTH, 0), ((Container)topGroup).get_ContentRegion().Width - base.LABEL_WIDTH, dynamicEvent.Location.ZRange[1].ToString(CultureInfo.CurrentCulture), "Top Z Position", delegate(string val)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(val))
					{
						val = "0";
					}
					dynamicEvent.Location.ZRange[1] = float.Parse(val, CultureInfo.CurrentCulture);
				}
				catch (Exception ex2)
				{
					ShowError(ex2.Message);
				}
			})).set_BasicTooltipText("Defines the top boundary of the z-axis. The vaule is a offset from the point z-axis.");
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)parent);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
			Panel bottomGroup = val3;
			((Control)RenderLabel(bottomGroup, "Bottom").TitleLabel).set_Width(base.LABEL_WIDTH);
			((Control)RenderTextbox(bottomGroup, new Point(base.LABEL_WIDTH, 0), ((Container)bottomGroup).get_ContentRegion().Width - base.LABEL_WIDTH, dynamicEvent.Location.ZRange[0].ToString(CultureInfo.CurrentCulture), "Bottom Z Position", delegate(string val)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(val))
					{
						val = "0";
					}
					dynamicEvent.Location.ZRange[0] = float.Parse(val, CultureInfo.CurrentCulture);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			})).set_BasicTooltipText("Defines the lower boundary of the z-axis. The vaule is a offset from the point z-axis.");
		}

		private void RenderPolygonePoints(FlowPanel parent, DynamicEvent dynamicEvent)
		{
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Expected O, but got Unknown
			((Container)parent).ClearChildren();
			FlowPanel lastPointSection = null;
			if (dynamicEvent != null)
			{
				for (int i = 0; i < dynamicEvent.Location.Points.Length; i++)
				{
					if (dynamicEvent.Location.Points[i].Length < 3)
					{
						Array.Resize(ref dynamicEvent.Location.Points[i], 3);
					}
					int index = i;
					lastPointSection = AddPolygonePointSection(parent, dynamicEvent.Location.Points[i]);
					Button button = RenderButton((Panel)(object)lastPointSection, "U", delegate
					{
						if (index > 0)
						{
							dynamicEvent.Location.Points.MoveEntry(index, index - 1);
							RenderPolygonePoints(parent, dynamicEvent);
						}
					}, () => index == 0);
					((Control)button).set_Width(30);
					((Control)button).set_BasicTooltipText("Moves this point entry up in the list.");
					Button button2 = RenderButton((Panel)(object)lastPointSection, "D", delegate
					{
						if (index <= dynamicEvent.Location.Points.Length - 2)
						{
							dynamicEvent.Location.Points.MoveEntry(index, index + 1);
							RenderPolygonePoints(parent, dynamicEvent);
						}
					}, () => index == dynamicEvent.Location.Points.Length - 1);
					((Control)button2).set_Width(30);
					((Control)button2).set_BasicTooltipText("Moves this point entry down in the list.");
					Control lastChild = ((IEnumerable<Control>)((Container)lastPointSection).get_Children()).Last();
					Button button3 = RenderButton((Panel)(object)lastPointSection, base.TranslationService.GetTranslation("editDynamicEventView-btn-remove", "Remove"), delegate
					{
						List<float[]> list = dynamicEvent.Location.Points.ToList();
						list.RemoveAt(index);
						dynamicEvent.Location.Points = list.ToArray();
						RenderPolygonePoints(parent, dynamicEvent);
					}, () => dynamicEvent.Location.Points.Length == 1);
					((Control)button3).set_Left(lastChild.get_Right() + 10);
					((Control)button3).set_Width(120);
					button3.Icon = base.IconService.GetIcon("1444524.png");
					button3.ResizeIcon = false;
					((Control)button3).set_BasicTooltipText("Removes the entry from the list.");
				}
			}
			int? obj;
			if (lastPointSection == null)
			{
				obj = null;
			}
			else
			{
				Control obj2 = ((IEnumerable<Control>)((Container)lastPointSection).get_Children()).LastOrDefault();
				obj = ((obj2 != null) ? new int?(obj2.get_Left()) : null);
			}
			int? num = obj;
			int x = num.GetValueOrDefault();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(x + 120);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			Panel addButtonPanel = val;
			Button button4 = RenderButton(addButtonPanel, base.TranslationService.GetTranslation("manageReminderTimesView-btn-add", "Add"), delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				Vector3 currentPosition = GetCurrentPosition();
				dynamicEvent.Location.Points = new List<float[]>(dynamicEvent.Location.Points) { new float[3] { currentPosition.X, currentPosition.Y, 0f } }.ToArray();
				RenderPolygonePoints(parent, dynamicEvent);
			});
			((Control)button4).set_Left(x);
			((Control)button4).set_Width(120);
			button4.Icon = base.IconService.GetIcon("1444520.png");
			button4.ResizeIcon = false;
			((Control)button4).set_BasicTooltipText("Adds a new entry to the list.");
		}

		private FlowPanel AddPolygonePointSection(FlowPanel parent, float[] point)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)parent);
			((Control)val2).set_Width(((Container)parent).get_ContentRegion().Width - (int)parent.get_OuterControlPadding().X * 2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(2f, 0f));
			val2.set_FlowDirection((ControlFlowDirection)2);
			FlowPanel positionSectionPanel = val2;
			TextBox xPos = RenderTextbox((Panel)(object)positionSectionPanel, Point.get_Zero(), 150, point[0].ToString(CultureInfo.CurrentCulture), "X Position", delegate(string val)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(val))
					{
						val = "0";
					}
					point[0] = float.Parse(val, CultureInfo.CurrentCulture);
				}
				catch (Exception ex3)
				{
					ShowError(ex3.Message);
				}
			});
			((Control)xPos).set_BasicTooltipText("Defines the position on the x-axis.");
			TextBox yPos = RenderTextbox((Panel)(object)positionSectionPanel, new Point(((Control)xPos).get_Right() + 5, 0), 150, point[1].ToString(CultureInfo.CurrentCulture), "Y Position", delegate(string val)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(val))
					{
						val = "0";
					}
					point[1] = float.Parse(val, CultureInfo.CurrentCulture);
				}
				catch (Exception ex2)
				{
					ShowError(ex2.Message);
				}
			});
			((Control)yPos).set_BasicTooltipText("Defines the position on the y-axis.");
			TextBox zPos = RenderTextbox((Panel)(object)positionSectionPanel, new Point(((Control)yPos).get_Right() + 5, 0), 150, point[2].ToString(CultureInfo.CurrentCulture), "Z Position", delegate(string val)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(val))
					{
						val = "0";
					}
					point[2] = float.Parse(val, CultureInfo.CurrentCulture);
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			});
			((Control)zPos).set_BasicTooltipText("Defines the position on the y-axis. A value of 0 uses the same value as the center.");
			((Control)RenderButton((Panel)(object)positionSectionPanel, "Set current Position", delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Vector3 currentPosition = GetCurrentPosition();
				((TextInputBase)xPos).set_Text(currentPosition.X.ToString(CultureInfo.CurrentCulture));
				((TextInputBase)yPos).set_Text(currentPosition.Y.ToString(CultureInfo.CurrentCulture));
				((TextInputBase)zPos).set_Text(currentPosition.Z.ToString(CultureInfo.CurrentCulture));
			})).set_BasicTooltipText("Sets the position values to the current position reported by mumblelink.");
			return positionSectionPanel;
		}

		private string GetMapDisplayName(Map map)
		{
			return $"{map.get_Name().Trim()} ( ID: {map.get_Id()} )";
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			_maps = ((IEnumerable<Map>)(await ((IAllExpandableClient<Map>)(object)base.APIManager.get_Gw2ApiClient().get_V2().get_Maps()).AllAsync(default(CancellationToken)))).ToList();
			return true;
		}

		protected override void Unload()
		{
			base.Unload();
			_selectedMap = null;
			_maps = null;
			_dynamicEvent = null;
		}
	}
}
