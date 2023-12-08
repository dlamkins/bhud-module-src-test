using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Models;
using Estreya.BlishHUD.Shared.Services;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Estreya.BlishHUD.Shared.UI.Views.Settings
{
	public class ServiceSettingsView : BaseSettingsView
	{
		private const int LABEL_VALUE_X_LOCATION = 200;

		private readonly Func<Task> _reloadCalledAction;

		private readonly IEnumerable<ManagedService> _stateList;

		protected override Dictionary<ControlType, BitmapFont> ControlFonts
		{
			get
			{
				Dictionary<ControlType, BitmapFont> controlFonts = base.ControlFonts;
				controlFonts[ControlType.Label] = GameService.Content.get_DefaultFont18();
				return controlFonts;
			}
		}

		public ServiceSettingsView(IEnumerable<ManagedService> stateList, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService, Func<Task> reloadCalledAction = null)
			: base(apiManager, iconService, translationService, settingEventService)
		{
			_stateList = stateList;
			_reloadCalledAction = reloadCalledAction;
		}

		protected override void BuildView(FlowPanel parent)
		{
			((Panel)parent).set_CanScroll(true);
			foreach (ManagedService state in _stateList)
			{
				RenderState(state, parent);
			}
			if (_reloadCalledAction != null)
			{
				RenderEmptyLine((Panel)(object)parent);
				RenderEmptyLine((Panel)(object)parent);
				RenderButtonAsync((Panel)(object)parent, "Reload", _reloadCalledAction);
			}
		}

		private void RenderState(ManagedService managedService, FlowPanel parent)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			bool isAPIState = IsAPIState(managedService);
			string title = managedService.GetType().Name;
			if (isAPIState)
			{
				title += " - API State";
			}
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			((Panel)val).set_ShowBorder(true);
			((Panel)val).set_Title(title);
			FlowPanel stateGroup = val;
			((Control)stateGroup).set_Width(((Container)parent).get_ContentRegion().Width - (int)stateGroup.get_OuterControlPadding().X * 2);
			ServiceConfiguration configuration = (ServiceConfiguration)managedService.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Last((PropertyInfo x) => x.Name == "Configuration")
				.GetValue(managedService);
			string value = JsonConvert.SerializeObject(configuration, Formatting.Indented, new JsonSerializerSettings
			{
				Converters = new JsonConverter[1]
				{
					new StringEnumConverter()
				}
			});
			int? valueXLocation = 200;
			Color? val2 = null;
			Color? textColorTitle = val2;
			val2 = null;
			RenderLabel((Panel)(object)stateGroup, "Configuration:", value, textColorTitle, val2, valueXLocation);
			string value2 = managedService.Running.ToString();
			val2 = (managedService.Running ? Color.get_Green() : Color.get_Red());
			RenderLabel(valueXLocation: 200, parent: (Panel)(object)stateGroup, title: "Running:", value: value2, textColorTitle: null, textColorValue: val2);
			if (isAPIState)
			{
				bool loading = (bool)managedService.GetType().GetProperty("Loading").GetValue(managedService);
				bool finished = managedService.Running && !loading;
				string value3 = finished.ToString();
				val2 = (finished ? Color.get_Green() : Color.get_Red());
				RenderLabel(valueXLocation: 200, parent: (Panel)(object)stateGroup, title: "Loading finished:", value: value3, textColorTitle: null, textColorValue: val2);
				string progressText = (string)managedService.GetType().GetProperty("ProgressText").GetValue(managedService);
				valueXLocation = 200;
				val2 = null;
				Color? textColorTitle2 = val2;
				val2 = null;
				RenderLabel((Panel)(object)stateGroup, "Progress Text:", progressText, textColorTitle2, val2, valueXLocation);
			}
			RenderEmptyLine((Panel)(object)stateGroup, (int)stateGroup.get_OuterControlPadding().X);
		}

		private bool IsAPIState(ManagedService managedService)
		{
			List<Type> baseTypes = new List<Type>();
			Type baseType = managedService.GetType().BaseType;
			while (baseType != null)
			{
				if (baseType.IsGenericType)
				{
					baseTypes.Add(baseType.GetGenericTypeDefinition());
				}
				else
				{
					baseTypes.Add(baseType);
				}
				baseType = baseType.BaseType;
			}
			if (managedService.GetType().BaseType.IsGenericType)
			{
				return baseTypes.Contains(typeof(APIService<>));
			}
			return false;
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
