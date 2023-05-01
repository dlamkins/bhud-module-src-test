using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class ReorderEventsView : BaseView
	{
		private static Point MAIN_PADDING = new Point(20, 20);

		private static readonly Logger Logger = Logger.GetLogger<ReorderEventsView>();

		private readonly List<EventCategory> _allEvents;

		private readonly List<string> _order;

		private readonly EventAreaConfiguration _areaConfiguration;

		private Panel Panel { get; set; }

		public event EventHandler<(EventAreaConfiguration AreaConfiguration, string[] CategoryKeys)> SaveClicked;

		public ReorderEventsView(List<EventCategory> allEvents, List<string> order, EventAreaConfiguration areaConfiguration, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, BitmapFont font = null)
			: base(apiManager, iconService, translationService, font)
		{
			_allEvents = allEvents;
			_order = order;
			_areaConfiguration = areaConfiguration;
		}

		private void DrawEntries(ListView<EventCategory> listView)
		{
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			((Container)listView).ClearChildren();
			foreach (EventCategory eventCategory in from ec in _allEvents
				group ec by ec.Key into g
				select g.First() into x
				orderby _order.IndexOf(x.Key)
				select x)
			{
				ListEntry<EventCategory> listEntry = new ListEntry<EventCategory>(eventCategory.Name);
				((Control)listEntry).set_Parent((Container)(object)listView);
				((Control)listEntry).set_Width(((Control)listView).get_Width() - 20);
				listEntry.DragDrop = true;
				listEntry.TextColor = Color.get_White();
				listEntry.Data = eventCategory;
				listEntry.Alignment = (HorizontalAlignment)1;
				ListEntry<EventCategory> entry = listEntry;
				if (eventCategory.Icon != null)
				{
					entry.Icon = base.IconService.GetIcon(eventCategory.Icon);
				}
			}
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Location(new Point(MAIN_PADDING.X, MAIN_PADDING.Y));
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - MAIN_PADDING.Y);
			((Control)val).set_Height(((Container)parent).get_ContentRegion().Height - MAIN_PADDING.X);
			val.set_CanScroll(true);
			Panel = val;
			Rectangle contentRegion = ((Container)Panel).get_ContentRegion();
			ListView<EventCategory> listView2 = new ListView<EventCategory>();
			((Control)listView2).set_Parent((Container)(object)Panel);
			((Control)listView2).set_Location(new Point(((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, contentRegion.Y));
			((Container)listView2).set_WidthSizingMode((SizingMode)0);
			((Container)listView2).set_HeightSizingMode((SizingMode)0);
			ListView<EventCategory> listView = listView2;
			((Control)listView).set_Size(new Point(contentRegion.Width - ((Control)listView).get_Left() - MAIN_PADDING.X, contentRegion.Height - 32));
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)Panel);
			((Control)val2).set_Location(new Point(((Control)listView).get_Left(), ((Control)listView).get_Bottom()));
			((Control)val2).set_Size(new Point(((Control)listView).get_Width(), 26));
			Panel buttons = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Save");
			((Control)val3).set_Parent((Container)(object)buttons);
			((Control)val3).set_Right(((Control)buttons).get_Width());
			((Control)val3).set_Bottom(((Control)buttons).get_Height());
			StandardButton saveButton = val3;
			((Control)saveButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				Logger.Debug("Save reordered categories.");
				List<EventCategory> orderedCategories = (from child in ((Container)listView).get_Children().ToList()
					select ((ListEntry<EventCategory>)(object)child).Data).ToList();
				List<EventCategory> currentCategories = _allEvents;
				foreach (EventCategory category in orderedCategories)
				{
					int oldIndex = currentCategories.IndexOf(currentCategories.Where((EventCategory ec) => ec.Key == category.Key).First());
					int newIndex = orderedCategories.IndexOf(category);
					currentCategories.RemoveAt(oldIndex);
					if (newIndex > oldIndex)
					{
						newIndex--;
					}
					currentCategories.Insert(newIndex, category);
				}
				this.SaveClicked?.Invoke(this, (_areaConfiguration, currentCategories.Select((EventCategory x) => x.Key).ToArray()));
			});
			StandardButton val4 = new StandardButton();
			val4.set_Text("Reset");
			((Control)val4).set_Parent((Container)(object)buttons);
			((Control)val4).set_Right(((Control)saveButton).get_Left());
			((Control)val4).set_Bottom(((Control)buttons).get_Height());
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Logger.Debug("Reset current view");
				DrawEntries(listView);
			});
			DrawEntries(listView);
		}
	}
}
