namespace MysticCrafting.Module.Recipe.TreeView.Nodes
{
	public interface IIngredient
	{
		int RequiredQuantity { get; }

		string FullPath { get; }
	}
}
