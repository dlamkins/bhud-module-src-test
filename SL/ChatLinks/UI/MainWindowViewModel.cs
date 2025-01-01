using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using SL.Common;

namespace SL.ChatLinks.UI
{
	public sealed class MainWindowViewModel : ViewModel
	{
		[CompilerGenerated]
		private IViewsFactory _003CviewFactory_003EP;

		public string Id => "sliekens.chat-links.main-window";

		public string Title => "Chat Links";

		public AsyncTexture2D BackgroundTexture => AsyncTexture2D.FromAssetId(155985);

		public AsyncTexture2D EmblemTexture => AsyncTexture2D.FromAssetId(2237584);

		public MainWindowViewModel(IViewsFactory viewFactory)
		{
			_003CviewFactory_003EP = viewFactory;
			base._002Ector();
		}

		public IEnumerable<Tab> Tabs()
		{
			yield return new Tab(AsyncTexture2D.FromAssetId(156699), (Func<IView>)_003CviewFactory_003EP.CreateItemsTabView, "Items", (int?)1);
		}
	}
}
