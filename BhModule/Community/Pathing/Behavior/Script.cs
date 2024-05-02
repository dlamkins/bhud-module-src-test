using System;
using System.Linq;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Neo.IronLua;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior
{
	internal class Script : Behavior<IPathingEntity>, ICanFocus, ICanInteract, ICanFilter
	{
		private static readonly Logger Logger = Logger.GetLogger<Script>();

		public const string PRIMARY_ATTR_NAME = "script";

		private const string ATTR_TICK = "script-tick";

		private const string ATTR_FOCUS = "script-focus";

		private const string ATTR_TRIGGER = "script-trigger";

		private const string ATTR_FILTER = "script-filter";

		private const string ATTR_ONCE = "script-once";

		private const int TICK_FREQUENCY = 0;

		private double _nextTick;

		private readonly IPackState _packState;

		public (string Name, object[] Args) TickFunc { get; set; }

		public (string Name, object[] Args) FocusFunc { get; set; }

		public (string Name, object[] Args) TriggerFunc { get; set; }

		public (string Name, object[] Args) FilterFunc { get; set; }

		public (string Name, object[] Args) OnceFunc { get; set; }

		public Script(string tickFunc, string focusFunc, string triggerFunc, string filterFunc, string onceFunc, IPathingEntity entity, IPackState packState)
			: base(entity)
		{
			TickFunc = SplitFunc(tickFunc);
			FocusFunc = SplitFunc(focusFunc);
			TriggerFunc = SplitFunc(triggerFunc);
			FilterFunc = SplitFunc(filterFunc);
			OnceFunc = SplitFunc(onceFunc);
			_packState = packState;
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, IPathingEntity entity, IPackState packState)
		{
			IAttribute tickAttr;
			IAttribute focusAttr;
			IAttribute triggerAttr;
			IAttribute filterAttr;
			IAttribute onceAttr;
			return new Script(attributes.TryGetAttribute("script-tick", out tickAttr) ? tickAttr.GetValueAsString() : null, attributes.TryGetAttribute("script-focus", out focusAttr) ? focusAttr.GetValueAsString() : null, attributes.TryGetAttribute("script-trigger", out triggerAttr) ? triggerAttr.GetValueAsString() : null, attributes.TryGetAttribute("script-filter", out filterAttr) ? filterAttr.GetValueAsString() : null, attributes.TryGetAttribute("script-once", out onceAttr) ? onceAttr.GetValueAsString() : null, entity, packState);
		}

		public void Focus()
		{
			if (FocusFunc.Name != null)
			{
				_packState.Module.ScriptEngine.CallFunction(FocusFunc.Name, new object[2] { _pathingEntity, true }.Concat(FocusFunc.Args));
			}
			if (TriggerFunc.Name != null)
			{
				_packState.UiStates.Interact.ShowInteract(_pathingEntity, "Will trigger a script {0}");
			}
		}

		public void Unfocus()
		{
			if (FocusFunc.Name != null)
			{
				_packState.Module.ScriptEngine.CallFunction(FocusFunc.Name, new object[2] { _pathingEntity, false }.Concat(FocusFunc.Args));
			}
			_packState.UiStates.Interact.DisconnectInteract(_pathingEntity);
		}

		public void Interact(bool autoTriggered)
		{
			if (TriggerFunc.Name != null)
			{
				_packState.Module.ScriptEngine.CallFunction(TriggerFunc.Name, new object[2] { _pathingEntity, autoTriggered }.Concat(TriggerFunc.Args));
			}
		}

		public bool IsFiltered()
		{
			if (FilterFunc.Name != null)
			{
				LuaResult filterCall = _packState.Module.ScriptEngine.CallFunction(FilterFunc.Name, new object[1] { _pathingEntity }.Concat(FilterFunc.Args));
				if (filterCall != LuaResult.Empty)
				{
					return filterCall.ToBoolean();
				}
			}
			return false;
		}

		private object GetValueFromString(string value)
		{
			if (value[0] != '\'' && value[0] != '"')
			{
				if (value.Equals("nil"))
				{
					return null;
				}
				if (value.Contains("."))
				{
					if (float.TryParse(value, out var floatResult))
					{
						return floatResult;
					}
					return value;
				}
				if (int.TryParse(value, out var intResult))
				{
					return intResult;
				}
				if (string.Equals(value, "true", StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
				if (string.Equals(value, "false", StringComparison.InvariantCultureIgnoreCase))
				{
					return false;
				}
			}
			return value.Trim('\'', '"');
		}

		private (string Name, object[] Args) SplitFunc(string func)
		{
			if (!string.IsNullOrWhiteSpace(func))
			{
				try
				{
					if (func.Contains("("))
					{
						string[] array = func.Split('(');
						func = array[0];
						string argStr = array[1].Trim(' ', ')');
						if (argStr.Length > 0)
						{
							object[] args = argStr.Split(',').Select(GetValueFromString).ToArray();
							return (func, args);
						}
					}
					return (func, Array.Empty<object>());
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Pathable '" + _pathingEntity.Guid.ToBase64String() + "' has an invalid script attribute value of '" + func + "'.");
				}
			}
			return (null, null);
		}

		public string FilterReason()
		{
			return "Hidden by a script.";
		}

		public override void Update(GameTime gameTime)
		{
			if (!_packState.Module.PackInitiator.IsLoading)
			{
				if (OnceFunc.Name != null)
				{
					_packState.Module.ScriptEngine.CallFunction(OnceFunc.Name, new object[1] { _pathingEntity }.Concat(OnceFunc.Args));
					OnceFunc = (null, null);
				}
				if (TickFunc.Name != null && gameTime.get_TotalGameTime().TotalMilliseconds > _nextTick)
				{
					_nextTick = gameTime.get_TotalGameTime().TotalMilliseconds + 0.0;
					_packState.Module.ScriptEngine.CallFunction(TickFunc.Name, new object[2] { _pathingEntity, gameTime }.Concat(TickFunc.Args));
				}
			}
		}

		public override void Unload()
		{
			_packState.UiStates.Interact.DisconnectInteract(_pathingEntity);
		}
	}
}
