using System.Collections.Generic;

namespace gw2stacks_blish.data
{
	internal class CraftingMiscAdvice
	{
		public Dictionary<int, int> idCountMapping;

		public string advice;

		public int outputId;

		public CraftingMiscAdvice(Dictionary<int, int> idCountMapping, string advice, int outputId)
		{
			this.idCountMapping = idCountMapping;
			this.advice = advice;
			this.outputId = outputId;
		}

		public CraftingMiscAdvice()
		{
			idCountMapping = new Dictionary<int, int>();
			advice = null;
			outputId = 0;
		}
	}
}
