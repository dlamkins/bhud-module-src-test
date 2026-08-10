using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Diagnostics.Metrics
{
	internal abstract class ObservableInstrument<T> : Instrument where T : struct
	{
		public override bool IsObservable => true;

		protected ObservableInstrument(Meter meter, string name, string? unit, string? description)
			: this(meter, name, unit, description, (IEnumerable<KeyValuePair<string, object?>>?)null)
		{
		}

		protected ObservableInstrument(Meter meter, string name, string? unit, string? description, IEnumerable<KeyValuePair<string, object?>>? tags)
			: base(meter, name, unit, description, tags)
		{
			Instrument.ValidateTypeParameter<T>();
		}

		protected abstract IEnumerable<Measurement<T>> Observe();

		internal override void Observe(MeterListener listener)
		{
			object subscriptionState = GetSubscriptionState(listener);
			IEnumerable<Measurement<T>> enumerable = Observe();
			if (enumerable == null)
			{
				return;
			}
			foreach (Measurement<T> item in enumerable)
			{
				listener.NotifyMeasurement(this, item.Value, item.Tags, subscriptionState);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static IEnumerable<Measurement<T>> Observe(object callback)
		{
			Func<T> func = callback as Func<T>;
			if (func != null)
			{
				return new Measurement<T>[1]
				{
					new Measurement<T>(func())
				};
			}
			Func<Measurement<T>> func2 = callback as Func<Measurement<T>>;
			if (func2 != null)
			{
				return new Measurement<T>[1] { func2() };
			}
			return (callback as Func<IEnumerable<Measurement<T>>>)?.Invoke();
		}
	}
}
