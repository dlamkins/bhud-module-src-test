namespace gw2stacks_blish.data
{
	internal class MiscAdvice
	{
		public int itemId;

		public int minCount;

		public string advice;

		public MiscAdvice(int itemId_, int count_, string advice_)
		{
			itemId = itemId_;
			minCount = count_;
			advice = advice_;
		}

		public MiscAdvice()
		{
			itemId = 0;
			minCount = 0;
			advice = null;
		}
	}
}
