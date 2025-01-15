using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace SL.ChatLinks.UI.Tabs.Items
{
	public sealed class ChatLinkEditorView : View
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
	}
}
