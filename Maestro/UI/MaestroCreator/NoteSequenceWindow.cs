using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.MaestroCreator
{
	public class NoteSequenceWindow : StandardWindow
	{
		private static class Layout
		{
			public const int DefaultWidth = 700;

			public const int DefaultHeight = 450;

			public const int MaxWidth = 1200;

			public const int MaxHeight = 800;

			public const int ContentPaddingX = 15;
		}

		private NoteSequencePanel _panel;

		private static Texture2D _backgroundTexture;

		public event EventHandler PanelReturned;

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(1200, 800));
		}

		public NoteSequenceWindow(NoteSequencePanel panel)
			: this(GetBackground(), new Rectangle(0, 0, 1200, 800), new Rectangle(15, 20, 1170, 780))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Title("Maestro Creator");
			((WindowBase2)this).set_Subtitle("Notes");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("creator-emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("MaestroNoteSequenceWindow_v1");
			((WindowBase2)this).set_CanResize(true);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Size(new Point(700, 450));
			_panel = panel;
			((Control)_panel).set_Parent((Container)(object)this);
			((Control)_panel).set_Location(new Point(0, 2));
			_panel.SetExpanded(expanded: true);
			UpdatePanelSize();
		}

		public override void RecalculateLayout()
		{
			((WindowBase2)this).RecalculateLayout();
			UpdatePanelSize();
		}

		private void UpdatePanelSize()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (_panel != null)
			{
				int contentWidth = ((Container)this).get_ContentRegion().Width;
				int contentHeight = ((Container)this).get_ContentRegion().Height - 2;
				if (contentWidth > 0 && contentHeight > 0)
				{
					_panel.ResizeTo(contentWidth, contentHeight);
				}
			}
		}

		public NoteSequencePanel DetachPanel()
		{
			NoteSequencePanel panel = _panel;
			if (panel != null)
			{
				((Control)panel).set_Parent((Container)null);
				panel.SetExpanded(expanded: false);
				_panel = null;
			}
			return panel;
		}

		public override void Hide()
		{
			((WindowBase2)this).Hide();
			this.PanelReturned?.Invoke(this, EventArgs.Empty);
		}

		protected override void DisposeControl()
		{
			_panel = null;
			((WindowBase2)this).DisposeControl();
		}
	}
}
