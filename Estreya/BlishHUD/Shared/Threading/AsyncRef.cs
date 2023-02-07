namespace Estreya.BlishHUD.Shared.Threading
{
	public class AsyncRef<T>
	{
		public T Value { get; set; }

		public AsyncRef()
		{
		}

		public AsyncRef(T value)
		{
			Value = value;
		}

		public override string ToString()
		{
			T value = Value;
			if (value != null)
			{
				return value.ToString();
			}
			return "";
		}

		public static implicit operator T(AsyncRef<T> r)
		{
			return r.Value;
		}

		public static implicit operator AsyncRef<T>(T value)
		{
			return new AsyncRef<T>(value);
		}
	}
}
