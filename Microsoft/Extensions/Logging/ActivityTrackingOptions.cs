using System;

namespace Microsoft.Extensions.Logging
{
	[Flags]
	internal enum ActivityTrackingOptions
	{
		None = 0x0,
		SpanId = 0x1,
		TraceId = 0x2,
		ParentId = 0x4,
		TraceState = 0x8,
		TraceFlags = 0x10,
		Tags = 0x20,
		Baggage = 0x40
	}
}
