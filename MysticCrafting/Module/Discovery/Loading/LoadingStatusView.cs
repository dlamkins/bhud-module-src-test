using System.Collections.Generic;
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

		private readonly IList<IApiService> _recurringServices;

		public LoadingStatusView(IList<IApiService> recurringServices)
			: this()
		{
			_recurringServices = recurringServices;
			((View)this).WithPresenter((IPresenter)(object)new LoadingStatusPresenter(this, recurringServices));
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
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Expected O, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Expected O, but got Unknown
			Image val = new Image();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Size(new Point(16, 16));
			val.set_Tint(Color.get_White());
			val.set_Texture(ServiceContainer.TextureRepository.GetRefTexture("157330-cantint.png"));
			((Control)val).set_Location(new Point(10, 12));
			((Control)val).set_Visible(true);
			_statusImage = val;
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel);
			val2.set_Text("GW2 API");
			((Control)val2).set_Location(new Point(30, 10));
			((Control)val2).set_Size(new Point(200, 20));
			val2.set_ShowShadow(true);
			_nameLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Location(new Point(90, 10));
			val3.set_TextColor(Color.get_White() * 0.7f);
			((Control)val3).set_Size(new Point(200, 20));
			val3.set_ShowShadow(true);
			((Control)val3).set_Visible(false);
			_dateLabel = val3;
			LoadingSpinner val4 = new LoadingSpinner();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Size(new Point(25, 25));
			((Control)val4).set_Location(new Point(140, 7));
			((Control)val4).set_Visible(false);
			_loadingSpinner = val4;
		}
	}
}
