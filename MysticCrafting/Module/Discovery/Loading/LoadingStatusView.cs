using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Services.API;

namespace MysticCrafting.Module.Discovery.Loading
{
	public class LoadingStatusView : View
	{
		public Label _nameLabel;

		public Label _dateLabel;

		public LoadingSpinner _loadingSpinner;

		public Image _statusImage;

		private readonly IList<IApiService> _apiServices;

		public LoadingStatusView(IList<IApiService> apiServices)
			: this()
		{
			_apiServices = apiServices;
			((View)this).WithPresenter((IPresenter)(object)new LoadingStatusPresenter(this, apiServices));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Expected O, but got Unknown
			Image val = new Image();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(16, 16));
			val.set_Tint(Color.get_White());
			val.set_Texture(ServiceContainer.TextureRepository.GetRefTexture("157330-cantint.png"));
			((Control)val).set_Location(new Point(110, 12));
			((Control)val).set_Visible(true);
			_statusImage = val;
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel);
			val2.set_Text("GW2 API");
			((Control)val2).set_Location(new Point(130, 10));
			((Control)val2).set_Size(new Point(200, 20));
			val2.set_ShowShadow(true);
			_nameLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Location(new Point(190, 10));
			val3.set_TextColor(Color.get_White() * 0.7f);
			((Control)val3).set_Size(new Point(200, 20));
			val3.set_ShowShadow(true);
			((Control)val3).set_Visible(false);
			_dateLabel = val3;
			LoadingSpinner val4 = new LoadingSpinner();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Size(new Point(25, 25));
			((Control)val4).set_Location(new Point(105, 7));
			((Control)val4).set_Visible(true);
			_loadingSpinner = val4;
			BuildMenuStrip();
		}

		public void BuildMenuStrip()
		{
			ContextMenuStrip menuStrip = new LoadingStatusContextMenuHelper().BuildContextMenu(_apiServices.Where((IApiService s) => s.CanReloadManually).ToList());
			((Control)_statusImage).set_Menu(menuStrip);
			((Control)_nameLabel).set_Menu(menuStrip);
			((Control)_dateLabel).set_Menu(menuStrip);
			((Control)_loadingSpinner).set_Menu(menuStrip);
		}
	}
}
