using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace BhModule.PathingCategoryExplorerPlugin
{
	public class PluginService
	{
		private const string _pathingNamespace = "bh.community.pathing";

		private ModuleManager _pathingModuleManager;

		private readonly List<Action> _hookDisposeActions = new List<Action>();

		private Action<Control, bool> _setPathingNodeChecked;

		private Func<Control, bool> _getPathingNodeCheckable;

		private Action<Control> _showAllCategories;

		private Action<Control> _deselectAdjacentNodes;

		private Action<Container> _disposeContainer;

		private bool _freezeConfirmation;

		private ModuleSettings Settings => PathingCategoryExplorerPluginModule.Instance.Settings;

		private bool DependenciesMet => PathingCategoryExplorerPluginModule.InstanceManager.get_DependenciesMet();

		public void Upadate()
		{
			if (DependenciesMet && _pathingModuleManager == null)
			{
				GetPathingModuleManager();
				BuildActions();
				HookCategoryContextMenu();
				HookConfirmationWindow();
				HookTreeNodeBaseDispose();
			}
		}

		public void Unload()
		{
			if (_pathingModuleManager != null)
			{
				_pathingModuleManager.remove_ModuleDisabled((EventHandler<EventArgs>)OnPathingUnload);
				OnPathingUnload(this, EventArgs.Empty);
			}
		}

		private void OnPathingUnload(object sender, EventArgs e)
		{
			foreach (Action hookDisposeAction in _hookDisposeActions)
			{
				hookDisposeAction();
			}
			_hookDisposeActions.Clear();
		}

		private void BuildActions()
		{
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			Type type = Assembly.GetAssembly(((object)_pathingModuleManager.get_ModuleInstance()).GetType()).GetType("BhModule.Community.Pathing.UI.Controls.TreeNodes.PathingCategoryNode");
			Type baseType = type.BaseType;
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Control));
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(bool));
			UnaryExpression unaryExpression = Expression.TypeAs(parameterExpression, baseType);
			MethodCallExpression ifTrue = Expression.Call(unaryExpression, baseType.GetMethod("CheckboxOnCheckedChanged", BindingFlags.Instance | BindingFlags.NonPublic), new Expression[2]
			{
				unaryExpression,
				Expression.New(typeof(CheckChangedEvent).GetConstructors()[0], parameterExpression2)
			});
			BinaryExpression right = Expression.NotEqual(Expression.Property(unaryExpression, "Checked"), parameterExpression2);
			ConditionalExpression body = Expression.IfThen(Expression.And(Expression.TypeIs(parameterExpression, baseType), right), ifTrue);
			Action<Control, bool> setPathingNodeChecked = Expression.Lambda<Action<Control, bool>>(body, new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
			_setPathingNodeChecked = delegate(Control ctrl, bool val)
			{
				if (val)
				{
					_freezeConfirmation = true;
				}
				setPathingNodeChecked(ctrl, val);
				if (_freezeConfirmation)
				{
					_freezeConfirmation = false;
				}
			};
			MemberExpression ifTrue2 = Expression.Property(unaryExpression, "Checkable");
			ConditionalExpression body2 = Expression.Condition(Expression.TypeIs(parameterExpression, baseType), ifTrue2, Expression.Constant(false));
			_getPathingNodeCheckable = Expression.Lambda<Func<Control, bool>>(body2, new ParameterExpression[1] { parameterExpression }).Compile();
			MethodCallExpression ifTrue3 = Expression.Call(Expression.TypeAs(parameterExpression, type), type.GetMethod("ShowAllSkippedCategories_LeftMouseButtonReleased", BindingFlags.Instance | BindingFlags.NonPublic), new Expression[2]
			{
				parameterExpression,
				Expression.Constant((object)new MouseEventArgs((MouseEventType)514))
			});
			ConditionalExpression body3 = Expression.IfThen(Expression.TypeIs(parameterExpression, type), ifTrue3);
			_showAllCategories = Expression.Lambda<Action<Control>>(body3, new ParameterExpression[1] { parameterExpression }).Compile();
			MethodCallExpression ifTrue4 = Expression.Call(unaryExpression, baseType.GetMethod("DeselectAdjacentNodesExcept"), unaryExpression);
			ConditionalExpression body4 = Expression.IfThen(Expression.TypeIs(parameterExpression, baseType), ifTrue4);
			_deselectAdjacentNodes = Expression.Lambda<Action<Control>>(body4, new ParameterExpression[1] { parameterExpression }).Compile();
			MethodInfo method = typeof(Container).GetMethod("DisposeControl", BindingFlags.Instance | BindingFlags.NonPublic);
			DynamicMethod dynamicMethod = new DynamicMethod("DisposeContainer", null, new Type[1] { typeof(Container) }, typeof(Container), skipVisibility: true);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.Emit(OpCodes.Call, method);
			iLGenerator.Emit(OpCodes.Ret);
			_disposeContainer = dynamicMethod.CreateDelegate<Action<Container>>();
		}

		private void GetPathingModuleManager()
		{
			_pathingModuleManager = GameService.Module.get_Modules().FirstOrDefault((ModuleManager m) => m.get_Manifest().get_Namespace() == "bh.community.pathing");
			_pathingModuleManager.add_ModuleDisabled((EventHandler<EventArgs>)OnPathingUnload);
		}

		private void HookCategoryContextMenu()
		{
			MethodInfo method = Assembly.GetAssembly(((object)_pathingModuleManager.get_ModuleInstance()).GetType()).GetType("BhModule.Community.Pathing.UI.Controls.TreeNodes.PathingNode").GetMethod("BuildDeselectAdjacentNodes", BindingFlags.Instance | BindingFlags.NonPublic);
			Hook hook = new Hook(method, new Action<Action<object>, object>(BuildContextMenu));
			_hookDisposeActions.Add(delegate
			{
				hook.Dispose();
			});
		}

		private void HookConfirmationWindow()
		{
			MethodInfo method = Assembly.GetAssembly(((object)_pathingModuleManager.get_ModuleInstance()).GetType()).GetType("BhModule.Community.Pathing.UI.Controls.TreeNodes.PathingCategoryNode").GetMethod("ShowConfirmationWindow", BindingFlags.Instance | BindingFlags.NonPublic);
			Hook hook = new Hook(method, new Action<Action<object>, object>(ShowConfirmationWindow));
			_hookDisposeActions.Add(delegate
			{
				hook.Dispose();
			});
		}

		private void HookTreeNodeBaseDispose()
		{
			MethodInfo method = Assembly.GetAssembly(((object)_pathingModuleManager.get_ModuleInstance()).GetType()).GetType("BhModule.Community.Pathing.UI.Controls.TreeNodes.TreeNodeBase").GetMethod("DisposeControl", BindingFlags.Instance | BindingFlags.NonPublic);
			Hook hook = new Hook(method, new Action<Action<object>, object>(DisposeTreeNodeBase), Settings.FixNodeExpansionBug.get_Value());
			Settings.FixNodeExpansionBug.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)applyHook);
			_hookDisposeActions.Add(delegate
			{
				Settings.FixNodeExpansionBug.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)applyHook);
				hook.Dispose();
			});
			void applyHook(object sender, ValueChangedEventArgs<bool> e)
			{
				if (e.get_NewValue())
				{
					hook.Apply();
				}
				else
				{
					hook.Undo();
				}
			}
		}

		private void DisposeTreeNodeBase(Action<object> dispose, object instance)
		{
			Container val = (Container)((instance is Container) ? instance : null);
			if (val != null)
			{
				_disposeContainer(val);
			}
			dispose(instance);
		}

		private void ShowConfirmationWindow(Action<object> show, object instance)
		{
			if (!_freezeConfirmation)
			{
				show(instance);
			}
		}

		private void BuildContextMenu(Action<object> BuildDeselectAdjacentNodes, object instance)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			Container pathingNode = (Container)((instance is Container) ? instance : null);
			if (pathingNode == null)
			{
				return;
			}
			BuildDeselectAdjacentNodes(pathingNode);
			if (Settings.AddSelectRecursively.get_Value())
			{
				ContextMenuStripItem val = new ContextMenuStripItem("Select Recursively");
				((Control)val).set_Parent((Container)(object)((Control)pathingNode).get_Menu());
				((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectRecursively(pathingNode, checkedValue: true);
				});
			}
			if (Settings.AddDeselectRecursively.get_Value())
			{
				ContextMenuStripItem val2 = new ContextMenuStripItem("Deselect Recursively");
				((Control)val2).set_Parent((Container)(object)((Control)pathingNode).get_Menu());
				((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectRecursively(pathingNode, checkedValue: false);
				});
			}
			if (Settings.AddDeselectAllOthers.get_Value())
			{
				ContextMenuStripItem val3 = new ContextMenuStripItem("Select The Path Exclusively");
				((Control)val3).set_Parent((Container)(object)((Control)pathingNode).get_Menu());
				((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					ActiveAllParentsAndSelf(pathingNode);
					DeselectAllOthers(pathingNode);
				});
			}
		}

		private void SelectRecursively(Container pathingNode, bool checkedValue)
		{
			Type type = ((object)pathingNode).GetType();
			_setPathingNodeChecked?.Invoke((Control)(object)pathingNode, checkedValue);
			_showAllCategories?.Invoke((Control)(object)pathingNode);
			foreach (Control child in pathingNode.get_Children())
			{
				if (!type.Equals(((object)child).GetType()))
				{
					continue;
				}
				Func<Control, bool> getPathingNodeCheckable = _getPathingNodeCheckable;
				if (getPathingNodeCheckable != null && getPathingNodeCheckable(child))
				{
					_setPathingNodeChecked?.Invoke(child, checkedValue);
					Container val = (Container)(object)((child is Container) ? child : null);
					if (val != null)
					{
						SelectRecursively(val, checkedValue);
					}
				}
			}
		}

		private void ActiveAllParentsAndSelf(Container pathingNode)
		{
			Type type = ((object)pathingNode).GetType();
			List<Container> list = new List<Container>(1) { pathingNode };
			while (((object)((Control)list.Last()).get_Parent()).GetType() == type)
			{
				list.Add(((Control)list.Last()).get_Parent());
			}
			list.Reverse();
			foreach (Container item in list)
			{
				_setPathingNodeChecked?.Invoke((Control)(object)item, arg2: true);
			}
		}

		private void DeselectAllOthers(Container pathingNode)
		{
			_deselectAdjacentNodes((Control)(object)pathingNode);
			if (!(((object)((Control)pathingNode).get_Parent()).GetType() != ((object)pathingNode).GetType()))
			{
				DeselectAllOthers(((Control)pathingNode).get_Parent());
			}
		}
	}
}
