using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Microsoft.Extensions.Logging
{
	internal sealed class LoggerFactoryScopeProvider : IExternalScopeProvider
	{
		private sealed class Scope : IDisposable
		{
			private readonly LoggerFactoryScopeProvider _provider;

			private bool _isDisposed;

			public Scope Parent { get; }

			public object State { get; }

			internal Scope(LoggerFactoryScopeProvider provider, object state, Scope parent)
			{
				_provider = provider;
				State = state;
				Parent = parent;
			}

			public override string ToString()
			{
				return State?.ToString();
			}

			public void Dispose()
			{
				if (!_isDisposed)
				{
					_provider._currentScope.Value = Parent;
					_isDisposed = true;
				}
			}
		}

		private sealed class ActivityLogScope : IReadOnlyList<KeyValuePair<string, object>>, IReadOnlyCollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
		{
			private string _cachedToString;

			private const int MaxItems = 5;

			private readonly KeyValuePair<string, object>[] _items = new KeyValuePair<string, object>[5];

			public int Count { get; }

			public KeyValuePair<string, object> this[int index]
			{
				get
				{
					if (index >= Count)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					return _items[index];
				}
			}

			public ActivityLogScope(Activity activity, ActivityTrackingOptions activityTrackingOption)
			{
				int num = 0;
				if ((activityTrackingOption & ActivityTrackingOptions.SpanId) != 0)
				{
					_items[num++] = new KeyValuePair<string, object>("SpanId", activity.GetSpanId());
				}
				if ((activityTrackingOption & ActivityTrackingOptions.TraceId) != 0)
				{
					_items[num++] = new KeyValuePair<string, object>("TraceId", activity.GetTraceId());
				}
				if ((activityTrackingOption & ActivityTrackingOptions.ParentId) != 0)
				{
					_items[num++] = new KeyValuePair<string, object>("ParentId", activity.GetParentId());
				}
				if ((activityTrackingOption & ActivityTrackingOptions.TraceState) != 0)
				{
					_items[num++] = new KeyValuePair<string, object>("TraceState", activity.TraceStateString);
				}
				if ((activityTrackingOption & ActivityTrackingOptions.TraceFlags) != 0)
				{
					_items[num++] = new KeyValuePair<string, object>("TraceFlags", activity.ActivityTraceFlags);
				}
				Count = num;
			}

			public override string ToString()
			{
				if (_cachedToString == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(_items[0].Key);
					stringBuilder.Append(':');
					stringBuilder.Append(_items[0].Value);
					for (int i = 1; i < Count; i++)
					{
						stringBuilder.Append(", ");
						stringBuilder.Append(_items[i].Key);
						stringBuilder.Append(':');
						stringBuilder.Append(_items[i].Value);
					}
					_cachedToString = stringBuilder.ToString();
				}
				return _cachedToString;
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				int i = 0;
				while (i < Count)
				{
					yield return this[i];
					int num = i + 1;
					i = num;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private sealed class ActivityBaggageLogScopeWrapper : IEnumerable<KeyValuePair<string, object>>, IEnumerable
		{
			private struct BaggageEnumerator : IEnumerator<KeyValuePair<string, object>>, IDisposable, IEnumerator
			{
				private readonly IEnumerator<KeyValuePair<string, string>> _enumerator;

				public KeyValuePair<string, object> Current => new KeyValuePair<string, object>(_enumerator.Current.Key, _enumerator.Current.Value);

				object IEnumerator.Current => Current;

				public BaggageEnumerator(IEnumerator<KeyValuePair<string, string>> enumerator)
				{
					_enumerator = enumerator;
				}

				public void Dispose()
				{
					_enumerator.Dispose();
				}

				public bool MoveNext()
				{
					return _enumerator.MoveNext();
				}

				public void Reset()
				{
					_enumerator.Reset();
				}
			}

			private readonly IEnumerable<KeyValuePair<string, string>> _items;

			private StringBuilder _stringBuilder;

			public ActivityBaggageLogScopeWrapper(IEnumerable<KeyValuePair<string, string>> items)
			{
				_items = items;
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				return new BaggageEnumerator(_items.GetEnumerator());
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new BaggageEnumerator(_items.GetEnumerator());
			}

			public override string ToString()
			{
				lock (this)
				{
					IEnumerator<KeyValuePair<string, string>> enumerator = _items.GetEnumerator();
					if (!enumerator.MoveNext())
					{
						return string.Empty;
					}
					if (_stringBuilder == null)
					{
						_stringBuilder = new StringBuilder();
					}
					_stringBuilder.Append(enumerator.Current.Key);
					_stringBuilder.Append(':');
					_stringBuilder.Append(enumerator.Current.Value);
					while (enumerator.MoveNext())
					{
						_stringBuilder.Append(", ");
						_stringBuilder.Append(enumerator.Current.Key);
						_stringBuilder.Append(':');
						_stringBuilder.Append(enumerator.Current.Value);
					}
					string result = _stringBuilder.ToString();
					_stringBuilder.Clear();
					return result;
				}
			}
		}

		private readonly AsyncLocal<Scope> _currentScope = new AsyncLocal<Scope>();

		private readonly ActivityTrackingOptions _activityTrackingOption;

		public LoggerFactoryScopeProvider(ActivityTrackingOptions activityTrackingOption)
		{
			_activityTrackingOption = activityTrackingOption;
		}

		public void ForEachScope<TState>(Action<object, TState> callback, TState state)
		{
			if (_activityTrackingOption != 0)
			{
				Activity current2 = Activity.Current;
				if (current2 != null)
				{
					ActivityLogScope activityLogScope = current2.GetCustomProperty("__ActivityLogScope__") as ActivityLogScope;
					if (activityLogScope == null)
					{
						activityLogScope = new ActivityLogScope(current2, _activityTrackingOption);
						current2.SetCustomProperty("__ActivityLogScope__", activityLogScope);
					}
					callback(activityLogScope, state);
					if ((_activityTrackingOption & ActivityTrackingOptions.Tags) != 0 && current2.TagObjects.GetEnumerator().MoveNext())
					{
						callback(current2.TagObjects, state);
					}
					if ((_activityTrackingOption & ActivityTrackingOptions.Baggage) != 0)
					{
						IEnumerable<KeyValuePair<string, string>> baggage = current2.Baggage;
						if (baggage.GetEnumerator().MoveNext())
						{
							ActivityBaggageLogScopeWrapper orCreateActivityBaggageLogScopeWrapper = GetOrCreateActivityBaggageLogScopeWrapper(current2, baggage);
							callback(orCreateActivityBaggageLogScopeWrapper, state);
						}
					}
				}
			}
			Report(_currentScope.Value);
			void Report(Scope current)
			{
				if (current != null)
				{
					Report(current.Parent);
					callback(current.State, state);
				}
			}
		}

		private static ActivityBaggageLogScopeWrapper GetOrCreateActivityBaggageLogScopeWrapper(Activity activity, IEnumerable<KeyValuePair<string, string>> items)
		{
			ActivityBaggageLogScopeWrapper activityBaggageLogScopeWrapper = activity.GetCustomProperty("__ActivityBaggageItemsLogScope__") as ActivityBaggageLogScopeWrapper;
			if (activityBaggageLogScopeWrapper == null)
			{
				activityBaggageLogScopeWrapper = new ActivityBaggageLogScopeWrapper(items);
				activity.SetCustomProperty("__ActivityBaggageItemsLogScope__", activityBaggageLogScopeWrapper);
			}
			return activityBaggageLogScopeWrapper;
		}

		public IDisposable Push(object state)
		{
			Scope value = _currentScope.Value;
			Scope scope = new Scope(this, state, value);
			_currentScope.Value = scope;
			return scope;
		}
	}
}
