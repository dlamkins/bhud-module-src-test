using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.EventTable.Models;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.UI.Views;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class HelpView : BaseView
	{
		private const string DISCORD_USERNAME = "estreya";

		private static readonly Point PADDING = new Point(25, 25);

		private readonly string _apiUrl;

		private readonly Func<List<EventCategory>> _getEvents;

		private readonly List<string> _autocompleteAPIKeys = new List<string>();

		public HelpView(Func<List<EventCategory>> getEvents, string apiUrl, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService)
			: base(apiManager, iconService, translationService)
		{
			_getEvents = getEvents;
			_apiUrl = apiUrl;
		}

		protected override void InternalBuild(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width - PADDING.X * 2);
			((Control)val).set_Height(((Container)parent).get_ContentRegion().Height - PADDING.Y * 2);
			((Control)val).set_Location(new Point(PADDING.X, PADDING.Y));
			((Panel)val).set_CanScroll(true);
			val.set_FlowDirection((ControlFlowDirection)3);
			FlowPanel flowPanel = val;
			BuildEditEventSection(flowPanel);
			RenderEmptyLine((Panel)(object)flowPanel);
			BuildCustomEventSection(flowPanel);
			RenderEmptyLine((Panel)(object)flowPanel);
			BuildSettingSliderSection(flowPanel);
			RenderEmptyLine((Panel)(object)flowPanel);
			BuildNoEventsSection(flowPanel);
			RenderEmptyLine((Panel)(object)flowPanel);
			BuildAutocompleteEventSection(flowPanel);
			RenderEmptyLine((Panel)(object)flowPanel);
			BuildOnlyRemindersSection(flowPanel);
			RenderEmptyLine((Panel)(object)flowPanel);
			BuildDeletedEventAreaSection(flowPanel);
			RenderEmptyLine((Panel)(object)flowPanel);
			BuildLocalizationSection(flowPanel);
			RenderEmptyLine((Panel)(object)flowPanel);
			BuildMapMovementDoesNotWorkSection(flowPanel);
			RenderEmptyLine((Panel)(object)flowPanel);
			BuildQuestionNotFoundSection(flowPanel);
		}

		private void BuildAutocompleteEventSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel autocompleteEventsPanel = val;
			FormattedLabelBuilder labelBuilder = GetLabelBuilder((Panel)(object)parent).CreatePart("What events are qualified for autocompletion?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).MakeUnderlined();
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			});
			IEnumerable<Event> events = _getEvents().SelectMany((EventCategory ec) => ec.Events);
			foreach (string apiKey in _autocompleteAPIKeys)
			{
				foreach (Event ev2 in events.Where((Event ev) => ev.APICode == apiKey).DistinctBy((Event ev) => ev.Key))
				{
					labelBuilder.CreatePart("- " + ev2.Name, (Action<FormattedLabelPartBuilder>)delegate
					{
					}).CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
					{
					});
				}
			}
			labelBuilder.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("All other events ", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("CAN'T", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.MakeBold().MakeUnderlined();
			})
				.CreatePart(" be autocompleted as they are not available in the API.", (Action<FormattedLabelPartBuilder>)delegate
				{
				});
			((Control)labelBuilder.Build()).set_Parent((Container)(object)autocompleteEventsPanel);
		}

		private void BuildSettingSliderSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel panel = val;
			((Control)GetLabelBuilder((Panel)(object)parent).CreatePart("Why are my setting sliders moving when I move others?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).MakeUnderlined();
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("Some sliders have a connection. For example:", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("If you make the table bigger, it can't be moved as far to the right.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.Build()).set_Parent((Container)(object)panel);
		}

		private void BuildEditEventSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel panel = val;
			((Control)GetLabelBuilder((Panel)(object)parent).CreatePart("Can I edit the event names or colors?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).MakeUnderlined();
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("No.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.MakeBold();
			})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("All events are loaded in the backend from the following file: ", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("CLICK HERE", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetHyperLink("https://files.estreya.de/blish-hud/event-table/v1/events.json");
				})
				.Build()).set_Parent((Container)(object)panel);
		}

		private void BuildCustomEventSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel panel = val;
			((Control)GetLabelBuilder((Panel)(object)parent).CreatePart("Can I create custom events?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).MakeUnderlined();
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("Yes.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.MakeBold();
			})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("Head over to the tab called \"Estreya BlishHUD API\" and login there.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.Build()).set_Parent((Container)(object)panel);
		}

		private void BuildDeletedEventAreaSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel panel = val;
			((Control)GetLabelBuilder((Panel)(object)parent).CreatePart("I deleted my event area. Can I recover it?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).MakeUnderlined();
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("No.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.MakeBold();
			})
				.Build()).set_Parent((Container)(object)panel);
		}

		private void BuildNoEventsSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel panel = val;
			((Control)GetLabelBuilder((Panel)(object)parent).CreatePart("Why can't I see any events?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).MakeUnderlined();
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("There could be multiple reasons for this:", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("- You have deactivated all events for your event area.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("- The backend service is currently not available.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("\n    You can check the following url (if it responds, the backend service is fine): ", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("CLICK HERE", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.SetHyperLink(_apiUrl.TrimEnd('/') + "/events");
				})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("- You have abused the backend and are currently ratelimited (same result as the above answer).", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("In case you can't figure it out, ping estreya on BlishHUD Discord.", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.MakeBold();
				})
				.Build()).set_Parent((Container)(object)panel);
		}

		private void BuildMapMovementDoesNotWorkSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel panel = val;
			((Control)GetLabelBuilder((Panel)(object)parent).CreatePart("Why does the automatic map movement not work?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).MakeUnderlined();
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("The movement of the map is a complex mechanic that is very sensitive to wrong inputs.", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("As far as I could analyse, there are some systems/thrid party programs that tend to mess with the movement.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("Can I do something about that?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.MakeBold();
				})
				.CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("No, not yet.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.Build()).set_Parent((Container)(object)panel);
		}

		private void BuildLocalizationSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel panel = val;
			((Control)GetLabelBuilder((Panel)(object)parent).CreatePart("Why are some elements not translated?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).MakeUnderlined();
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("Translation is an ongoing process. Most elements will get their translation at some point.", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("If you are missing a specific translation over a long period, please ping ", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("estreya", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.MakeBold();
				})
				.CreatePart(" on BlishHUD Discord.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.Build()).set_Parent((Container)(object)panel);
		}

		private void BuildOnlyRemindersSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel panel = val;
			((Control)GetLabelBuilder((Panel)(object)parent).CreatePart("I don't need the table. Can I just have reminders?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).MakeUnderlined();
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("Yes, you can.", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("Disable the only event area you have.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("\n", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.CreatePart("Don't delete it! It will be back on the next restart.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.Build()).set_Parent((Container)(object)panel);
		}

		private void BuildQuestionNotFoundSection(FlowPanel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ShowBorder(true);
			Panel panel = val;
			((Control)GetLabelBuilder((Panel)(object)parent).CreatePart("My question was not listed?", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
			{
				builder.SetFontSize((FontSize)20).MakeUnderlined();
			}).CreatePart("\n \n", (Action<FormattedLabelPartBuilder>)delegate
			{
			}).CreatePart("Ping ", (Action<FormattedLabelPartBuilder>)delegate
			{
			})
				.CreatePart("estreya", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					builder.MakeBold();
				})
				.CreatePart(" on BlishHUD Discord.", (Action<FormattedLabelPartBuilder>)delegate
				{
				})
				.Build()).set_Parent((Container)(object)panel);
		}

		private FormattedLabelBuilder GetLabelBuilder(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return new FormattedLabelBuilder().SetWidth(((Container)parent).get_ContentRegion().Width - PADDING.X * 2).AutoSizeHeight().SetVerticalAlignment((VerticalAlignment)0);
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			IApiV2ObjectList<MapChest> mapchests = await ((IAllExpandableClient<MapChest>)(object)base.APIManager.get_Gw2ApiClient().get_V2().get_MapChests()).AllAsync(default(CancellationToken));
			IApiV2ObjectList<WorldBoss> worldbosses = await ((IAllExpandableClient<WorldBoss>)(object)base.APIManager.get_Gw2ApiClient().get_V2().get_WorldBosses()).AllAsync(default(CancellationToken));
			_autocompleteAPIKeys.AddRange(((IEnumerable<MapChest>)mapchests).Select((MapChest m) => m.get_Id()));
			_autocompleteAPIKeys.AddRange(((IEnumerable<WorldBoss>)worldbosses).Select((WorldBoss w) => w.get_Id()));
			return true;
		}
	}
}
