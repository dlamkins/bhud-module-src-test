namespace System.Diagnostics
{
	internal readonly struct ActivityChangedEventArgs
	{
		public Activity? Previous { get; set; }

		public Activity? Current { get; set; }

		internal ActivityChangedEventArgs(Activity? previous, Activity? current)
		{
			Previous = previous;
			Current = current;
		}
	}
}
