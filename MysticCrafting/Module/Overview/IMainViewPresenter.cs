using Blish_HUD.Graphics.UI;
using MysticCrafting.Module.Menu;

namespace MysticCrafting.Module.Overview
{
	public interface IMainViewPresenter : IPresenter
	{
		void InitializeMenu();

		void SearchAsync(string text);

		void GoToMenuItem(CategoryMenuItem menuItem);

		void ReloadItemList();
	}
}
