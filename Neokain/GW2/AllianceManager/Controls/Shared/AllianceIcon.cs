using System;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Neokain.GW2.WebClient.Models.Alliances;

namespace Neokain.GW2.AllianceManager.Controls.Shared
{
	internal class AllianceIcon : CustomIcon
	{
		public AllianceIcon(AllianceDetailDto alliance)
		{
			if (alliance == null)
			{
				throw new ArgumentNullException("alliance");
			}
			((Image)this).set_Texture(AsyncTexture2D.FromAssetId(155052));
		}
	}
}
