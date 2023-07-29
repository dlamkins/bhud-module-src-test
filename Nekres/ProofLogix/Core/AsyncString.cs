using System;

namespace Nekres.ProofLogix.Core
{
	public class AsyncString
	{
		private string _string;

		public string String
		{
			get
			{
				return _string;
			}
			set
			{
				if (!string.Equals(_string, value))
				{
					_string = value;
					this.Changed?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public event EventHandler<EventArgs> Changed;

		public AsyncString()
		{
			_string = string.Empty;
		}

		public AsyncString(string str)
		{
			_string = str;
		}

		public bool Equals(string str)
		{
			return string.Equals(this, str);
		}

		public bool Equals(AsyncString str)
		{
			return string.Equals(this, str);
		}

		public static implicit operator string(AsyncString obj)
		{
			return obj.ToString();
		}

		public static implicit operator AsyncString(string obj)
		{
			return new AsyncString(obj);
		}

		public override string ToString()
		{
			return String;
		}
	}
}
