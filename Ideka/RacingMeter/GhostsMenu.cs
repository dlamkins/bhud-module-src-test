using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class GhostsMenu : SelectablesMenu<FullGhost>
	{
		private static readonly Color SelfBoardColor = Color.get_Blue() * 0.05f;

		protected override string? ExtractId(FullGhost? item)
		{
			return item?.Meta.Id;
		}

		public void SetLeaderboard(Leaderboard board)
		{
			Leaderboard board2 = board;
			Repopulate(delegate
			{
				//IL_0134: Unknown result type (might be due to invalid IL or missing references)
				if (!board2.Places.Any())
				{
					Placeholder(Strings.Nothing);
				}
				else
				{
					_menu.set_CanSelect(true);
					int num = 0;
					foreach (KeyValuePair<int, MetaGhost> place in board2.Places)
					{
						var (num3, meta) = place;
						if (num3 != num)
						{
							OnelineMenuItem onelineMenuItem = new OnelineMenuItem("");
							((Control)onelineMenuItem).set_Parent((Container)(object)_menu);
							((Control)onelineMenuItem).set_Enabled(false);
						}
						num = num3 + 1;
						GhostsMenu ghostsMenu = this;
						string id = meta.Id;
						OnelineMenuItem onelineMenuItem2 = new OnelineMenuItem($"{num3 + 1}. {meta.Time.Formatted()} - {meta.AccountName}");
						((Control)onelineMenuItem2).set_Parent((Container)(object)_menu);
						((Control)onelineMenuItem2).set_BasicTooltipText(meta.AccountName);
						OnelineMenuItem onelineMenuItem3 = ghostsMenu.SetItem(id, onelineMenuItem2);
						if (RacingModule.Server.User?.AccountId == meta.UserId)
						{
							((Control)onelineMenuItem3).set_BackgroundColor(SelfBoardColor);
						}
						((MenuItem)onelineMenuItem3).add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
						{
							Select(new FullGhost(meta, null));
						});
					}
				}
			});
		}

		public void SetLocalGhosts(IEnumerable<FullGhost> ghosts)
		{
			IEnumerable<FullGhost> ghosts2 = ghosts;
			Repopulate(delegate
			{
				if (!ghosts2.Any())
				{
					Placeholder(Strings.Nothing);
				}
				else
				{
					_menu.set_CanSelect(true);
					foreach (var item2 in ghosts2.OrderBy((FullGhost g) => g.Meta.Time).Enumerate())
					{
						int item = item2.index;
						FullGhost ghost = item2.item;
						GhostsMenu ghostsMenu = this;
						string id = ghost.Meta.Id;
						OnelineMenuItem onelineMenuItem = new OnelineMenuItem($"{item + 1}. {ghost.Meta.Time.Formatted()}");
						((Control)onelineMenuItem).set_Parent((Container)(object)_menu);
						((MenuItem)ghostsMenu.SetItem(id, onelineMenuItem)).add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate
						{
							Select(ghost);
						});
					}
				}
			});
		}
	}
}
