using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class MainPanel : Panel
	{
		private const int Spacing = 10;

		private readonly AreasPanel _areasPanel;

		private readonly WindowTab _tab;

		public AreaView? Selected
		{
			get
			{
				if (GameService.Overlay.get_BlishHudWindow().get_SelectedTab() != _tab || !((Control)GameService.Overlay.get_BlishHudWindow()).get_Visible())
				{
					return null;
				}
				return _areasPanel.Selected;
			}
		}

		public MainPanel()
			: this()
		{
			_tab = GameService.Overlay.get_BlishHudWindow().AddTab("Custom Combat Text", AsyncTexture2D.FromAssetId(1414035), (Panel)(object)this, CTextModule.Name.GetHashCode());
			AreasPanel areasPanel = new AreasPanel();
			((Control)areasPanel).set_Parent((Container)(object)this);
			_areasPanel = areasPanel;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (_areasPanel != null)
			{
				((Control)_areasPanel).set_Location(new Point(10, 10));
				AreasPanel areasPanel = _areasPanel;
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				((Control)areasPanel).set_Size(((Rectangle)(ref contentRegion)).get_Size() - new Point(20, 20));
			}
		}

		protected override void DisposeControl()
		{
			GameService.Overlay.get_BlishHudWindow().RemoveTab(_tab);
			((Panel)this).DisposeControl();
		}
	}
}
