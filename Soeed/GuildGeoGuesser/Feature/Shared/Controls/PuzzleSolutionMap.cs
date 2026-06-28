using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class PuzzleSolutionMap : Image
	{
		public const int MAP_SIZE = 768;

		private int _cacheBuster;

		public PuzzleSolutionMap(Puzzle puzzle, bool showAuthorView = false)
			: this()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(768, 768));
			UpdateTexture(puzzle, showAuthorView);
		}

		public void RefreshMap(Puzzle puzzle, bool showAuthorView = false)
		{
			_cacheBuster++;
			Account? account = Service.UserManager.Account;
			string accountName = ((account != null) ? account!.get_Name() : null);
			if (showAuthorView && accountName != null && puzzle.IsAuthor(accountName))
			{
				string fileName = puzzle.Id + "author_solution.png";
				Service.Textures.ForceRefreshTexture(fileName);
			}
			UpdateTexture(puzzle, showAuthorView);
		}

		private void UpdateTexture(Puzzle puzzle, bool showAuthorView)
		{
			Account? account = Service.UserManager.Account;
			string accountName = ((account != null) ? account!.get_Name() : null);
			if (showAuthorView && accountName != null && puzzle.IsAuthor(accountName))
			{
				base._texture = puzzle.GetAuthorSolutionMapTexture(accountName, _cacheBuster);
			}
			else
			{
				base._texture = puzzle.GetSolutionMapTexture(accountName);
			}
		}
	}
}
