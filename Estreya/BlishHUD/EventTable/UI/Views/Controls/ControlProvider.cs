using System;
using System.Collections.Generic;
using Blish_HUD.Controls;

namespace Estreya.BlishHUD.EventTable.UI.Views.Controls
{
	internal abstract class ControlProvider
	{
	}
	internal abstract class ControlProvider<T, TValidationType> : ControlProvider
	{
		public List<Type> Types = new List<Type>();

		internal ControlProvider()
		{
			Types.Add(typeof(T));
		}

		public Control CreateControl(BoxedValue<T> value, int width, int height, int x, int y)
		{
			return CreateControl(value, null, null, null, width, height, x, y);
		}

		public abstract Control CreateControl(BoxedValue<T> value, Func<T, bool> isEnabled, Func<TValidationType, bool> isValid, (float Min, float Max)? range, int width, int height, int x, int y);
	}
	internal abstract class ControlProvider<T1, T2, TValidationType> : ControlProvider<T1, TValidationType>
	{
		internal ControlProvider()
		{
			Types.Add(typeof(T2));
		}

		public Control CreateControl(BoxedValue<T2> value, int width, int height, int x, int y)
		{
			return CreateControl(value, null, null, null, width, height, x, y);
		}

		public abstract Control CreateControl(BoxedValue<T2> value, Func<T2, bool> isEnabled, Func<TValidationType, bool> isValid, (float Min, float Max)? range, int width, int height, int x, int y);
	}
	internal abstract class ControlProvider<T1, T2, T3, TValidationType> : ControlProvider<T1, T2, TValidationType>
	{
		internal ControlProvider()
		{
			Types.Add(typeof(T3));
		}

		public Control CreateControl(BoxedValue<T3> value, int width, int height, int x, int y)
		{
			return CreateControl(value, null, null, null, width, height, x, y);
		}

		public abstract Control CreateControl(BoxedValue<T3> value, Func<T3, bool> isEnabled, Func<TValidationType, bool> isValid, (float Min, float Max)? range, int width, int height, int x, int y);
	}
}
