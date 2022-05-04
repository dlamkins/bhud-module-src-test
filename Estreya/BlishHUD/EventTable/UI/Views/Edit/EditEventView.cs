using System;
using System.Drawing;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Estreya.BlishHUD.EventTable.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Edit
{
	public class EditEventView : BaseView
	{
		private Event Event { get; set; }

		public event EventHandler<ValueEventArgs<Event>> SavePressed;

		public event EventHandler CancelPressed;

		public EditEventView(Event ev)
		{
			Event = ev;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
			//IL_063d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0644: Unknown result type (might be due to invalid IL or missing references)
			//IL_064b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_065d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0664: Unknown result type (might be due to invalid IL or missing references)
			//IL_0679: Expected O, but got Unknown
			Rectangle bounds = ((Container)parent).get_ContentRegion();
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Size(((Rectangle)(ref bounds)).get_Size());
			val2.set_FlowDirection((ControlFlowDirection)3);
			val2.set_ControlPadding(new Vector2(5f, 2f));
			val2.set_OuterControlPadding(new Vector2(20f, 15f));
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Height(bounds.Height - 50);
			((Container)val2).set_AutoSizePadding(new Point(0, 15));
			((Panel)val2).set_CanScroll(true);
			((Control)val2).set_Parent((Container)(object)parent);
			FlowPanel parentPanel = val2;
			RenderLabel((Panel)(object)parentPanel, "THIS IS A WIP WINDOW.\nEXPECT BUGS!", null, Color.get_Red());
			RenderEmptyLine((Panel)(object)parentPanel);
			RenderEmptyLine((Panel)(object)parentPanel);
			RenderProperty((Panel)(object)parentPanel, Event, (Event ev) => ev.Key, (Event ev) => false);
			RenderProperty((Panel)(object)parentPanel, Event, (Event ev) => ev.Name, (Event ev) => true);
			RenderPropertyWithChangedTypeValidation((Panel)(object)parentPanel, Event, (Event ev) => ev.Offset, (Event ev) => true, delegate(string val)
			{
				try
				{
					TimeSpan.Parse(val);
					return (true, null);
				}
				catch (Exception ex4)
				{
					return (false, ex4.Message);
				}
			});
			RenderPropertyWithChangedTypeValidation((Panel)(object)parentPanel, Event, (Event ev) => ev.Repeat, (Event ev) => true, delegate(string val)
			{
				try
				{
					TimeSpan.Parse(val);
					return (true, null);
				}
				catch (Exception ex3)
				{
					return (false, ex3.Message);
				}
			});
			RenderProperty((Panel)(object)parentPanel, Event, (Event ev) => ev.Location, (Event ev) => true);
			RenderProperty((Panel)(object)parentPanel, Event, (Event ev) => ev.Waypoint, (Event ev) => true);
			RenderProperty((Panel)(object)parentPanel, Event, (Event ev) => ev.Wiki, (Event ev) => true, null, null, MathHelper.Clamp((int)GameService.Content.get_DefaultFont14().MeasureString(Event.Wiki).Width + 20, 0, ((Control)parentPanel).get_Width()));
			RenderPropertyWithChangedTypeValidation((Panel)(object)parentPanel, Event, (Event ev) => ev.Duration, (Event ev) => true, delegate(string val)
			{
				try
				{
					int.Parse(val);
					return (true, null);
				}
				catch (Exception ex2)
				{
					return (false, ex2.Message);
				}
			});
			RenderProperty((Panel)(object)parentPanel, Event, (Event ev) => ev.Icon, (Event ev) => true);
			RenderPropertyWithValidation((Panel)(object)parentPanel, Event, (Event ev) => ev.BackgroundColorCode, (Event ev) => true, delegate(string val)
			{
				if (string.IsNullOrWhiteSpace(val))
				{
					return (true, null);
				}
				try
				{
					ColorTranslator.FromHtml(val);
					return (true, null);
				}
				catch (Exception ex)
				{
					return (false, ex.Message);
				}
			});
			RenderProperty((Panel)(object)parentPanel, Event, (Event ev) => ev.APICodeType, (Event ev) => true);
			RenderProperty((Panel)(object)parentPanel, Event, (Event ev) => ev.APICode, (Event ev) => true);
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)parent);
			val3.set_FlowDirection((ControlFlowDirection)5);
			((Control)val3).set_Location(new Point(0, ((Control)parentPanel).get_Bottom()));
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Control)val3).set_Height(((Rectangle)(ref bounds)).get_Bottom() - ((Control)parentPanel).get_Bottom());
			FlowPanel buttonPanel = val3;
			RenderButton((Panel)(object)buttonPanel, "Save", delegate
			{
				this.SavePressed?.Invoke(this, new ValueEventArgs<Event>(Event));
			});
			RenderButton((Panel)(object)buttonPanel, "Cancel", delegate
			{
				this.CancelPressed?.Invoke(this, EventArgs.Empty);
			});
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
