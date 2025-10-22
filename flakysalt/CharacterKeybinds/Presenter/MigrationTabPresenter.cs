using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Graphics.UI;
using flakysalt.CharacterKeybinds.Model;
using flakysalt.CharacterKeybinds.Views;

namespace flakysalt.CharacterKeybinds.Presenter
{
	public class MigrationTabPresenter : Presenter<KeybindMigrationTab, MigrationTabModel>, IDisposable
	{
		public MigrationTabPresenter(KeybindMigrationTab view, MigrationTabModel model)
			: base(view, model)
		{
			KeybindMigrationTab view2 = base.get_View();
			view2.OnDeleteClicked = (EventHandler)Delegate.Combine(view2.OnDeleteClicked, new EventHandler(View_OnDeleteClicked));
			KeybindMigrationTab view3 = base.get_View();
			view3.OnMigrateClicked = (EventHandler)Delegate.Combine(view3.OnMigrateClicked, (EventHandler)delegate
			{
				View_OnMigrateClicked();
			});
		}

		private async Task View_OnMigrateClicked()
		{
			List<string> migrationTaskResult = await base.get_Model().MigrateKeybindings();
			base.get_View().SetMigrationResult(migrationTaskResult);
		}

		private void View_OnDeleteClicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			KeybindMigrationTab view = base.get_View();
			view.OnDeleteClicked = (EventHandler)Delegate.Remove(view.OnDeleteClicked, new EventHandler(View_OnDeleteClicked));
		}
	}
}
