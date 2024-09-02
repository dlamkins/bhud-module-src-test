using System;
using System.Reflection;
using Blish_HUD.Controls;

namespace FarmingTracker
{
	public class Hacks
	{
		private const BindingFlags BINDING_FLAGS = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic;

		public static void ClearAndAddChildrenWithoutUiFlickering(ControlCollection<Control> children, Container parent)
		{
			try
			{
				ControlCollection<Control> oldChildren = parent.get_Children();
				foreach (Control item in children)
				{
					GetPrivateField(item, "_parent").SetValue(item, parent);
				}
				GetPrivateField(parent, "_children").SetValue(parent, children);
				((Control)parent).Invalidate();
				foreach (Control item2 in oldChildren)
				{
					item2.Dispose();
				}
			}
			catch (Exception e)
			{
				Module.Logger.Error(e, "Failed to add children to container with reflection");
			}
		}

		private static FieldInfo GetPrivateField(object target, string fieldName)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target", "The assignment target cannot be null.");
			}
			if (string.IsNullOrEmpty(fieldName))
			{
				throw new ArgumentException("The field name cannot be null or empty.", "fieldName");
			}
			Type type = target.GetType();
			FieldInfo fieldInfo;
			while ((fieldInfo = type.GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic)) == null && (type = type.BaseType) != null)
			{
			}
			return fieldInfo;
		}
	}
}
