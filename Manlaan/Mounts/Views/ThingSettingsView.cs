using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Manlaan.Mounts.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mounts;

namespace Manlaan.Mounts.Views
{
	public class ThingSettingsView : Container
	{
		private int orderWidth = 80;

		private Panel panel;

		protected ThingsSettings CurrentThingSettings;

		public ThingSettingsView(ThingsSettings currentThingSettings)
			: this()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			CurrentThingSettings = currentThingSettings;
			((Control)this).set_Width(600);
			((Control)this).set_Height(600);
			Panel val = new Panel();
			val.set_CanScroll(false);
			((Control)val).set_Width(600);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)this);
			panel = val;
			BuildThingSettingsPanel();
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Draw(spriteBatch, drawBounds, scissor);
		}

		protected void BuildThingSettingsPanel()
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Expected O, but got Unknown
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			((Container)panel).ClearChildren();
			int curY = 0;
			List<Thing> thingsNotYetInSettings = (from t in Module._things
				where t.IsAvailable
				where !CurrentThingSettings.Things.Any((Thing tt) => tt.Equals(t))
				select t).ToList();
			if (thingsNotYetInSettings.Any())
			{
				Dropdown val = new Dropdown();
				((Control)val).set_Location(new Point(0, 0));
				((Control)val).set_Width(orderWidth);
				((Control)val).set_Parent((Container)(object)panel);
				((Control)val).set_BasicTooltipText("Only things that have a keybind in the General Settings tab will show up here.");
				Dropdown addThing_Select = val;
				thingsNotYetInSettings.ForEach(delegate(Thing t)
				{
					addThing_Select.get_Items().Add(t.DisplayName);
				});
				addThing_Select.set_SelectedItem(thingsNotYetInSettings.FirstOrDefault()?.DisplayName);
				StandardButton val2 = new StandardButton();
				((Control)val2).set_Parent((Container)(object)panel);
				((Control)val2).set_Location(new Point(((Control)addThing_Select).get_Right(), ((Control)addThing_Select).get_Top()));
				val2.set_Text(Strings.Add);
				((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					CurrentThingSettings.AddThing(Module._things.Single((Thing t) => t.DisplayName == addThing_Select.get_SelectedItem()));
					BuildThingSettingsPanel();
				});
				curY = ((Control)addThing_Select).get_Bottom();
			}
			if (!CurrentThingSettings.Things.Any())
			{
				Label val3 = new Label();
				((Control)val3).set_Location(new Point(0, curY));
				val3.set_AutoSizeWidth(true);
				val3.set_AutoSizeHeight(false);
				((Control)val3).set_Parent((Container)(object)panel);
				val3.set_TextColor(Color.get_Red());
				val3.set_Font(GameService.Content.get_DefaultFont18());
				val3.set_Text("You need to configure something or this context is pointless.");
			}
			foreach (var thingItemAndIndex in CurrentThingSettings.Things.Select((Thing value, int i) => new { i, value }))
			{
				Thing thing = thingItemAndIndex.value;
				int index = thingItemAndIndex.i;
				bool isAvailable = thing.IsAvailable;
				int curX = ((index % 2 != 0) ? 300 : 0);
				curY += ((index % 2 == 0) ? 30 : 0);
				Label val4 = new Label();
				((Control)val4).set_Location(new Point(curX, curY));
				val4.set_AutoSizeWidth(true);
				val4.set_AutoSizeHeight(false);
				((Control)val4).set_Parent((Container)(object)panel);
				val4.set_TextColor(isAvailable ? Color.get_White() : Color.get_Red());
				((Control)val4).set_BasicTooltipText(isAvailable ? null : "No keybind is set in the General Settings tab");
				val4.set_Text($"{index + 1}. {thing.Name}");
				Label thingInSettings_Label = val4;
				StandardButton val5 = new StandardButton();
				((Control)val5).set_Parent((Container)(object)panel);
				((Control)val5).set_Location(new Point(((Control)thingInSettings_Label).get_Right(), ((Control)thingInSettings_Label).get_Top()));
				val5.set_Text(Strings.Delete);
				((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					CurrentThingSettings.RemoveThing(thing);
					BuildThingSettingsPanel();
				});
			}
		}
	}
}
