namespace System.Diagnostics.Metrics
{
	internal struct ObjectSequenceMany : IEquatable<ObjectSequenceMany>, IObjectSequence
	{
		private readonly object[] _values;

		public object this[int i]
		{
			get
			{
				return _values[i];
			}
			set
			{
				_values[i] = value;
			}
		}

		public ObjectSequenceMany(object[] values)
		{
			_values = values;
		}

		public bool Equals(ObjectSequenceMany other)
		{
			if (_values.Length != other._values.Length)
			{
				return false;
			}
			for (int i = 0; i < _values.Length; i++)
			{
				object obj = _values[i];
				object obj2 = other._values[i];
				if (obj == null)
				{
					if (obj2 != null)
					{
						return false;
					}
				}
				else if (!obj.Equals(obj2))
				{
					return false;
				}
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj is ObjectSequenceMany)
			{
				ObjectSequenceMany other = (ObjectSequenceMany)obj;
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = 0;
			for (int i = 0; i < _values.Length; i++)
			{
				num <<= 3;
				object obj = _values[i];
				if (obj != null)
				{
					num ^= obj.GetHashCode();
				}
			}
			return num;
		}
	}
}
