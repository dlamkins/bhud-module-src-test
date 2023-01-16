using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Utility;
using Neo.IronLua;

namespace BhModule.Community.Pathing.Scripting.Console
{
	internal class WatchTreeNode : TreeNode
	{
		private TreeNode _loadingNode;

		public string ObjectName { get; set; }

		public WatchTreeNode(string objectName)
		{
			ObjectName = objectName;
		}

		private void UpdateValueName(object luaObject)
		{
			base.Text = ObjectName + ": " + LuaObjectToString(luaObject);
		}

		private void UpdateTableName(LuaTable luaTable)
		{
			base.Text = $"{ObjectName} [{luaTable.Values.Count}]";
		}

		private static string LuaObjectToString(object luaObject)
		{
			if (luaObject != null)
			{
				StandardMarker marker = luaObject as StandardMarker;
				if (marker == null)
				{
					if (luaObject is Guid)
					{
						Guid guid = (Guid)luaObject;
						return guid.ToBase64String();
					}
					return luaObject.ToString();
				}
				return "Marker[" + marker.Guid.ToBase64String() + "]";
			}
			return "nil";
		}

		private WatchTreeNode CreateOrUpdateNode(string objectName, int targetIndex)
		{
			WatchTreeNode existingNode = null;
			foreach (object node in base.Nodes)
			{
				WatchTreeNode wtn = node as WatchTreeNode;
				if (wtn != null && wtn.ObjectName == objectName)
				{
					existingNode = wtn;
					break;
				}
			}
			if (existingNode == null)
			{
				existingNode = new WatchTreeNode(objectName);
				base.Nodes.Insert(targetIndex, existingNode);
			}
			return existingNode;
		}

		public void Refresh(object luaObject)
		{
			LuaTable luaTable = luaObject as LuaTable;
			if ((object)luaTable == null)
			{
				base.Nodes.Clear();
				UpdateValueName(luaObject);
				return;
			}
			UpdateTableName(luaTable);
			if (base.IsExpanded)
			{
				base.Nodes.Remove(_loadingNode);
				int index = 0;
				List<WatchTreeNode> remainingNodes = base.Nodes.OfType<WatchTreeNode>().ToList();
				foreach (KeyValuePair<object, object> value in luaTable)
				{
					WatchTreeNode node = CreateOrUpdateNode(LuaObjectToString(value.Key), index);
					node.Refresh(value.Value);
					remainingNodes.Remove(node);
					index++;
				}
				foreach (WatchTreeNode orphanNode in remainingNodes)
				{
					base.Nodes.Remove(orphanNode);
				}
			}
			else if (luaTable.Values.Any() && base.Nodes.Count == 0)
			{
				_loadingNode = new TreeNode("Loading...");
				base.Nodes.Add(_loadingNode);
			}
			else if (!luaTable.Values.Any() && base.Nodes.Count > 0)
			{
				base.Nodes.Remove(_loadingNode);
			}
		}
	}
}
