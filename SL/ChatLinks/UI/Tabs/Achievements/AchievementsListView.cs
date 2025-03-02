using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace SL.ChatLinks.UI.Tabs.Achievements
{
	public sealed class AchievementsListView : View, IDisposable
	{
		[CompilerGenerated]
		private ObservableCollection<AchievementTileViewModel> _003Cachievements_003EP;

		private readonly FlowPanel _achievementsPanel;

		public AchievementsListView(ObservableCollection<AchievementTileViewModel> achievements)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			_003Cachievements_003EP = achievements;
			_achievementsPanel = new FlowPanel();
			((View)this)._002Ector();
		}

		protected override Task<bool> Load(IProgress<string> progress)
		{
			return ((View<IPresenter>)this).Load(progress);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			((Control)_achievementsPanel).set_Parent(buildPanel);
			_achievementsPanel.set_FlowDirection((ControlFlowDirection)0);
			((Container)_achievementsPanel).set_WidthSizingMode((SizingMode)2);
			((Container)_achievementsPanel).set_HeightSizingMode((SizingMode)1);
			_achievementsPanel.set_ControlPadding(new Vector2(9f, 7f));
			_achievementsPanel.set_OuterControlPadding(new Vector2(0f, 12f));
			foreach (AchievementTileViewModel item in _003Cachievements_003EP)
			{
				((Control)new AchievementTile(item)).set_Parent((Container)(object)_achievementsPanel);
			}
		}

		protected override void Unload()
		{
			Dispose();
		}

		public void Dispose()
		{
			while (((Container)_achievementsPanel).get_Children().get_Count() > 0)
			{
				((Container)_achievementsPanel).get_Children().get_Item(0).Dispose();
			}
			((Control)_achievementsPanel).Dispose();
		}
	}
}
