using System.Collections.Generic;

namespace System.Diagnostics.Metrics
{
	internal delegate void MeasurementCallback<T>(Instrument instrument, T measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state) where T : struct;
}
