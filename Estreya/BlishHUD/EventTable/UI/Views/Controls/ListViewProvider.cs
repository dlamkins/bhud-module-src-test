using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	internal class ListViewProvider<T> : ControlProvider<List<T>, List<T>>
	{
		private class ListViewControl<TCtrl> : Panel
		{
			private const int LABEL_WIDTH = 150;

			private static MethodInfo _lambdaFunction;

			public TCtrl Control { get; }

			private static MethodInfo LambdaFunction
			{
				get
				{
					if (_lambdaFunction == null)
					{
						_lambdaFunction = (from method in typeof(Expression).GetMethods()
							where method.IsGenericMethodDefinition && (from p in method.GetParameters()
								select p.Name).SequenceEqual(new string[2] { "body", "parameters" })
							select method).First();
					}
					return _lambdaFunction;
				}
			}

			public event EventHandler DeleteRequested;

			public ListViewControl(TCtrl control, int width)
				: this()
			{
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0103: Unknown result type (might be due to invalid IL or missing references)
				//IL_0106: Unknown result type (might be due to invalid IL or missing references)
				//IL_0110: Unknown result type (might be due to invalid IL or missing references)
				//IL_011b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0124: Expected O, but got Unknown
				//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e0: Expected O, but got Unknown
				//IL_0289: Unknown result type (might be due to invalid IL or missing references)
				//IL_0290: Expected O, but got Unknown
				//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f9: Expected O, but got Unknown
				ListViewControl<TCtrl> listViewControl = this;
				Control = control;
				PropertyInfo[] properties = Control.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				int y = 0;
				PropertyInfo[] array = properties;
				foreach (PropertyInfo property in array)
				{
					if (!property.CanWrite)
					{
						continue;
					}
					TypeOverrideAttribute typeOverrideAttribute = property.GetCustomAttribute<TypeOverrideAttribute>();
					ParameterExpression par = Expression.Parameter(typeof(TCtrl), "x");
					MemberExpression col = Expression.Property(par, property.Name);
					Type func = typeof(Func<, >).MakeGenericType(typeof(TCtrl), property.PropertyType);
					Expression lambda = (Expression)LambdaFunction.MakeGenericMethod(func).Invoke(null, new object[2]
					{
						col,
						new ParameterExpression[1] { par }
					});
					try
					{
						Label val = new Label();
						val.set_Text(property.Name);
						((Control)val).set_Parent((Container)(object)this);
						((Control)val).set_Location(new Point(0, y));
						((Control)val).set_Width(150);
						val.set_WrapText(true);
						Label label = val;
						Control ctrl2 = ((typeOverrideAttribute == null) ? ((Control)typeof(ControlHandler).GetMethod("CreateFromProperty").MakeGenericMethod(typeof(TCtrl), property.PropertyType).Invoke(null, new object[9]
						{
							control,
							lambda,
							(Func<TCtrl, bool>)((TCtrl ctrl) => true),
							null,
							null,
							width - 150 - 40,
							-1,
							150,
							y
						})) : ((Control)typeof(ControlHandler).GetMethod("CreateFromPropertyWithChangedTypeValidation").MakeGenericMethod(typeof(TCtrl), property.PropertyType, typeOverrideAttribute.Type).Invoke(null, new object[9]
						{
							control,
							lambda,
							(Func<TCtrl, bool>)((TCtrl ctrl) => true),
							null,
							null,
							width - 150 - 40,
							-1,
							150,
							y
						})));
						ctrl2.set_Parent((Container)(object)this);
						y += Math.Max(ctrl2.get_Height(), ((Control)label).get_Height()) + 5;
					}
					catch (Exception)
					{
					}
				}
				StandardButton val2 = new StandardButton();
				val2.set_Text("Delete");
				((Control)val2).set_Location(new Point(0, y));
				((Control)val2).set_Width(width - 40);
				((Control)val2).set_Parent((Container)(object)this);
				StandardButton deleteButton = val2;
				((Control)deleteButton).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					listViewControl.DeleteRequested?.Invoke(deleteButton, EventArgs.Empty);
				});
			}
		}

		private const int BUTTON_HEIGHT = 30;

		public override Control CreateControl(BoxedValue<List<T>> value, Func<List<T>, bool> isEnabled, Func<List<T>, bool> isValid, (float Min, float Max)? range, int width, int height, int x, int y)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Expected O, but got Unknown
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Width(width);
			((Control)val).set_Height(350);
			Panel mainPanel = val;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)mainPanel);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(((Control)mainPanel).get_Width());
			((Control)val2).set_Height(((Control)mainPanel).get_Height() - 30);
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val2).set_CanScroll(true);
			val2.set_ControlPadding(new Vector2(0f, 20f));
			FlowPanel flowPanel = val2;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)mainPanel);
			((Control)val3).set_Location(new Point(((Control)flowPanel).get_Left(), ((Control)flowPanel).get_Bottom()));
			((Control)val3).set_Height(30);
			((Control)val3).set_Width(((Control)mainPanel).get_Width());
			val3.set_FlowDirection((ControlFlowDirection)5);
			FlowPanel buttonPanel = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Add");
			((Control)val4).set_Parent((Container)(object)buttonPanel);
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ListViewControl<T> listViewControl2 = GetListViewControl(flowPanel, width, default(T));
				listViewControl2.DeleteRequested += delegate
				{
					value.Value.Remove(listViewControl2.Control);
					((Container)flowPanel).RemoveChild((Control)(object)listViewControl2);
				};
				value.Value.Add(listViewControl2.Control);
			});
			value.Value.ForEach(delegate(T item)
			{
				ListViewControl<T> listViewControl = GetListViewControl(flowPanel, width, item);
				listViewControl.DeleteRequested += delegate
				{
					value.Value.Remove(listViewControl.Control);
					((Container)flowPanel).RemoveChild((Control)(object)listViewControl);
				};
			});
			return (Control)(object)mainPanel;
		}

		private ListViewControl<T> GetListViewControl(FlowPanel parent, int width, T value)
		{
			T val = value;
			if (val == null)
			{
				value = (T)Activator.CreateInstance(typeof(T));
			}
			ListViewControl<T> listViewControl = new ListViewControl<T>(value, width);
			((Control)listViewControl).set_Parent((Container)(object)parent);
			((Container)listViewControl).set_WidthSizingMode((SizingMode)2);
			((Container)listViewControl).set_HeightSizingMode((SizingMode)1);
			((Panel)listViewControl).set_ShowBorder(true);
			return listViewControl;
		}
	}
}
