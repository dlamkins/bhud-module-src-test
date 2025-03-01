using System;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ChatLinkEditorView : View, IDisposable
	{
		private readonly ChatLinkEditor _chatLinkEditor = new ChatLinkEditor(viewModel);

		public ChatLinkEditorView(ChatLinkEditorViewModel viewModel)
			: this()
		{
		}

		protected override void Build(Container buildPanel)
		{
			((Control)_chatLinkEditor).set_Parent(buildPanel);
		}

		protected override void Unload()
		{
			Dispose();
		}

		public void Dispose()
		{
			((Control)_chatLinkEditor).Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
