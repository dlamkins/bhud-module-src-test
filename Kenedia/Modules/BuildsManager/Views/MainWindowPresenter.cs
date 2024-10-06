using System;
using Kenedia.Modules.BuildsManager.Models;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class MainWindowPresenter
	{
		public Type? SelectedTabType { get; set; }

		public Type? SelectedTemplateTabType { get; set; }

		public TagGroup? SelectedGroup { get; set; }

		public TemplateTag? SelectedTag { get; set; }
	}
}
