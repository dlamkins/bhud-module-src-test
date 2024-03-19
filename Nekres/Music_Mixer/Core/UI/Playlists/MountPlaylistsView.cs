using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Music_Mixer.Core.Services.Data;
using Nekres.Music_Mixer.Core.UI.Library;

namespace Nekres.Music_Mixer.Core.UI.Playlists
{
	public class MountPlaylistsView : View
	{
		private Dictionary<MountType, Texture2D> _icons;

		public MountPlaylistsView()
			: this()
		{
			_icons = Enum.GetValues(typeof(MountType)).Cast<MountType>().Skip(1)
				.ToDictionary((MountType x) => x, (MountType x) => MusicMixer.Instance.ContentsManager.GetTexture("skills/" + ((object)(MountType)(ref x)).ToString().ToLowerInvariant() + "_skill.png"));
		}

		protected override void Unload()
		{
			foreach (Texture2D value in _icons.Values)
			{
				if (value != null)
				{
					((GraphicsResource)value).Dispose();
				}
			}
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(150);
			((Control)val).set_Height(buildPanel.get_ContentRegion().Height);
			val.set_Title("Playlists");
			Panel playlistMenuPanel = val;
			Menu val2 = new Menu();
			((Control)val2).set_Parent((Container)(object)playlistMenuPanel);
			((Control)val2).set_Width(((Container)playlistMenuPanel).get_ContentRegion().Width);
			((Control)val2).set_Height(((Container)playlistMenuPanel).get_ContentRegion().Height);
			Menu menu = val2;
			ViewContainer val3 = new ViewContainer();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Left(((Control)menu).get_Right() + 4);
			((Control)val3).set_Width(buildPanel.get_ContentRegion().Width - ((Control)menu).get_Width() - 4);
			((Control)val3).set_Height(buildPanel.get_ContentRegion().Height);
			ViewContainer bgmLibraryContainer = val3;
			foreach (MountType item in Enum.GetValues(typeof(MountType)).Cast<MountType>().Skip(1))
			{
				MountType mountType = item;
				string name = ((object)(MountType)(ref mountType)).ToString().SplitCamelCase();
				MenuItem val4 = new MenuItem();
				val4.set_Text(name);
				((Control)val4).set_Parent((Container)(object)menu);
				val4.set_Icon(AsyncTexture2D.op_Implicit(_icons[mountType]));
				((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					MusicMixer.Instance.Resources.PlayMenuItemClick();
					if (!MusicMixer.Instance.Data.GetMountPlaylist(mountType, out var playlist))
					{
						playlist = new Playlist
						{
							ExternalId = ((object)(MountType)(ref mountType)).ToString(),
							Enabled = true,
							Tracks = new List<AudioSource>()
						};
					}
					bgmLibraryContainer.Show((IView)(object)new BgmLibraryView(playlist, name));
				});
			}
			((View<IPresenter>)this).Build(buildPanel);
		}
	}
}
