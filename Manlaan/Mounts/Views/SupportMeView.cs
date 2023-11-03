using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Manlaan.Mounts.Views
{
	internal class SupportMeView : View
	{
		private readonly TextureCache textureCache;

		public SupportMeView(TextureCache textureCache)
			: this()
		{
			this.textureCache = textureCache;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			val.set_Text("I don't expect anything in return, but if you want you can:\n- send some gold/items ingame: Bennieboj.2607\n- donate via Ko-fi:");
			((Control)val).set_Location(new Point(300, 300));
			((Control)val).set_Width(800);
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_HorizontalAlignment((HorizontalAlignment)0);
			((Control)val).set_Parent(buildPanel);
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Left(370);
			((Control)val2).set_Top(400);
			val2.set_Icon(AsyncTexture2D.op_Implicit(textureCache.GetImgFile(TextureCache.KofiTextureName)));
			((Control)val2).set_Height(60);
			((Control)val2).set_Width(130);
			((Control)val2).set_Parent(buildPanel);
			val2.set_Text("Ko-fi");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start("https://ko-fi.com/bennieboj");
			});
		}
	}
}
