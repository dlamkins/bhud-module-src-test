namespace Estreya.BlishHUD.EventTable.Contexts
{
	public struct AddCategory
	{
		public string Key { get; set; }

		public string Name { get; set; }

		public string Icon { get; set; }

		public bool ShowCombined { get; set; }

		public AddCategory()
		{
			Key = null;
			Name = null;
			Icon = null;
			ShowCombined = false;
		}
	}
}
