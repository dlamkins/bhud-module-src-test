using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Universal_Search_Module.Models;
using Universal_Search_Module.Strings;

namespace Universal_Search_Module.Controls.SearchResultItems
{
	public class LandmarkSearchResultItem : SearchResultItem
	{
		private const string POI_FILE = "https://render.guildwars2.com/file/25B230711176AB5728E86F5FC5F0BFAE48B32F6E/97461.png";

		private const string WAYPOINT_FILE = "https://render.guildwars2.com/file/32633AF8ADEA696A1EF56D3AE32D617B10D3AC57/157353.png";

		private const string VISTA_FILE = "https://render.guildwars2.com/file/A2C16AF497BA3A0903A0499FFBAF531477566F10/358415.png";

		private readonly IEnumerable<Landmark> _waypoints;

		private Landmark _landmark;

		protected override string ChatLink => Landmark?.PointOfInterest.ChatLink;

		public Landmark Landmark
		{
			get
			{
				return _landmark;
			}
			set
			{
				if (((Control)this).SetProperty<Landmark>(ref _landmark, value, false, "Landmark") && _landmark != null)
				{
					base.Icon = GetTextureForLandmarkAsync(_landmark.PointOfInterest);
					base.Name = _landmark.PointOfInterest.Name;
					base.Description = _landmark.PointOfInterest.ChatLink;
				}
			}
		}

		public LandmarkSearchResultItem(IEnumerable<Landmark> waypoints)
		{
			_waypoints = waypoints;
		}

		protected override Tooltip BuildTooltip()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected O, but got Unknown
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Expected O, but got Unknown
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			Tooltip val = new Tooltip();
			val.set_CurrentControl((Control)(object)this);
			Tooltip tooltip = val;
			Label val2 = new Label();
			val2.set_Text(base.Name);
			val2.set_Font(Control.get_Content().get_DefaultFont16());
			((Control)val2).set_Location(new Point(10, 10));
			((Control)val2).set_Height(11);
			val2.set_TextColor(Colors.Chardonnay);
			val2.set_ShowShadow(true);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val2).set_Parent((Container)(object)tooltip);
			Label detailsName = val2;
			Label val3 = new Label();
			val3.set_Text(Common.Landmark_Details_CopyChatCode);
			val3.set_Font(Control.get_Content().get_DefaultFont16());
			((Control)val3).set_Location(new Point(10, ((Control)detailsName).get_Bottom() + 5));
			val3.set_TextColor(Color.get_White());
			val3.set_ShowShadow(true);
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Parent((Container)(object)tooltip);
			Label detailsHintCopyChatCode = val3;
			Label val4 = new Label();
			val4.set_Text(Common.Landmark_Details_ClosestWaypoint);
			val4.set_Font(Control.get_Content().get_DefaultFont16());
			((Control)val4).set_Location(new Point(10, ((Control)detailsHintCopyChatCode).get_Bottom() + 12));
			((Control)val4).set_Height(11);
			val4.set_TextColor(Colors.Chardonnay);
			val4.set_ShadowColor(Color.get_Black());
			val4.set_ShowShadow(true);
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			((Control)val4).set_Parent((Container)(object)tooltip);
			Label detailsClosestWaypointTitle = val4;
			Landmark closestWaypont = ClosestWaypoint();
			Label val5 = new Label();
			val5.set_Text(closestWaypont.Map.Name + ": " + closestWaypont.PointOfInterest.Name);
			val5.set_Font(Control.get_Content().get_DefaultFont14());
			((Control)val5).set_Location(new Point(10, ((Control)detailsClosestWaypointTitle).get_Bottom() + 5));
			val5.set_TextColor(Color.get_White());
			val5.set_ShadowColor(Color.get_Black());
			val5.set_ShowShadow(true);
			val5.set_AutoSizeWidth(true);
			val5.set_AutoSizeHeight(true);
			((Control)val5).set_Parent((Container)(object)tooltip);
			Label detailsClosestWaypoint = val5;
			Label val6 = new Label();
			val6.set_Text(Common.Landmark_Details_CopyClosestWaypoint);
			val6.set_Font(Control.get_Content().get_DefaultFont14());
			((Control)val6).set_Location(new Point(10, ((Control)detailsClosestWaypoint).get_Bottom() + 5));
			val6.set_TextColor(Color.get_White());
			val6.set_ShadowColor(Color.get_Black());
			val6.set_ShowShadow(true);
			val6.set_AutoSizeWidth(true);
			val6.set_AutoSizeHeight(true);
			((Control)val6).set_Visible(true);
			((Control)val6).set_Parent((Container)(object)tooltip);
			return tooltip;
		}

		protected override async Task ClickAction()
		{
			if ((int)GameService.Input.get_Keyboard().get_ActiveModifiers() == 4)
			{
				if (!(await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(ClosestWaypoint().PointOfInterest.ChatLink)))
				{
					ScreenNotification.ShowNotification(Common.Landmark_FailedToCopy, (NotificationType)6, (Texture2D)null, 2);
					return;
				}
				if (UniversalSearchModule.ModuleInstance.SettingShowNotificationWhenLandmarkIsCopied.get_Value())
				{
					ScreenNotification.ShowNotification(Common.Landmark_WaypointCopied, (NotificationType)0, (Texture2D)null, 2);
				}
				if (UniversalSearchModule.ModuleInstance.SettingHideWindowAfterSelection.get_Value())
				{
					((Control)((Control)this).get_Parent()).Hide();
				}
			}
			else
			{
				await base.ClickAction();
			}
		}

		private Landmark ClosestWaypoint()
		{
			return (from waypoint in _waypoints
				select (Math.Sqrt(Math.Pow(Landmark.PointOfInterest.Coord.X - waypoint.PointOfInterest.Coord.X, 2.0) + Math.Pow(Landmark.PointOfInterest.Coord.Y - waypoint.PointOfInterest.Coord.Y, 2.0)), waypoint) into x
				orderby x.Item1
				select x).First().Item2;
		}

		private AsyncTexture2D GetTextureForLandmarkAsync(ContinentFloorRegionMapPoi landmark)
		{
			string imgUrl = string.Empty;
			switch (landmark.Type.Value)
			{
			case PoiType.Landmark:
				imgUrl = "https://render.guildwars2.com/file/25B230711176AB5728E86F5FC5F0BFAE48B32F6E/97461.png";
				break;
			case PoiType.Waypoint:
				imgUrl = "https://render.guildwars2.com/file/32633AF8ADEA696A1EF56D3AE32D617B10D3AC57/157353.png";
				break;
			case PoiType.Vista:
				imgUrl = "https://render.guildwars2.com/file/A2C16AF497BA3A0903A0499FFBAF531477566F10/358415.png";
				break;
			case PoiType.Unknown:
			case PoiType.Unlock:
			{
				RenderUrl? icon = landmark.Icon;
				if (!string.IsNullOrEmpty(icon.HasValue ? ((string)icon.GetValueOrDefault()) : null))
				{
					icon = landmark.Icon;
					imgUrl = (icon.HasValue ? ((string)icon.GetValueOrDefault()) : null);
					break;
				}
				return AsyncTexture2D.op_Implicit(Textures.get_Error());
			}
			}
			return Control.get_Content().GetRenderServiceTexture(imgUrl);
		}
	}
}
