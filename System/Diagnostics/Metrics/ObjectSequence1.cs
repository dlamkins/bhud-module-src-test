namespace System.Diagnostics.Metrics
{
	internal struct ObjectSequence1 : IEquatable<ObjectSequence1>, IObjectSequence
	{
		public object Value1;

		public object this[int i]
		{
			get
			{
				if (i == 0)
				{
					return Value1;
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				if (i == 0)
				{
					Value1 = value;
					return;
				}
				throw new IndexOutOfRangeException();
			}
		}

		public ObjectSequence1(object value1)
		{
			Value1 = value1;
		}

		public override int GetHashCode()
		{
			return Value1?.GetHashCode() ?? 0;
		}

		public bool Equals(ObjectSequence1 other)
		{
			if (Value1 != null)
			{
				return Value1.Equals(other.Value1);
			}
			return other.Value1 == null;
		}

		public override bool Equals(object obj)
		{
			if (obj is ObjectSequence1)
			{
				ObjectSequence1 other = (ObjectSequence1)obj;
				return Equals(other);
			}
			return false;
		}
	}
}
