using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Universal_Search_Module.Strings;

namespace Universal_Search_Module.Controls.SearchResultItems
{
	public abstract class SearchResultItem : Control
	{
		private const int ICON_SIZE = 32;

		private const int ICON_PADDING = 2;

		private const int DEFAULT_WIDTH = 100;

		private const int DEFAULT_HEIGHT = 36;

		private static Texture2D _textureItemHover;

		private AsyncTexture2D _icon;

		private string _name;

		private string _description;

		private Rectangle _layoutIconBounds;

		private Rectangle _layoutNameBounds;

		private Rectangle _layoutDescriptionBounds;

		public AsyncTexture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				((Control)this).SetProperty<AsyncTexture2D>(ref _icon, value, false, "Icon");
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _name, value, false, "Name");
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _description, value, false, "Description");
			}
		}

		protected abstract string ChatLink { get; }

		static SearchResultItem()
		{
			_textureItemHover = UniversalSearchModule.ModuleInstance.ContentsManager.GetTexture("textures\\1234875.png");
		}

		public SearchResultItem()
			: this()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(100, 36));
		}

		protected override void OnClick(MouseEventArgs e)
		{
			Task.Run(async delegate
			{
				await ClickAction();
				await AfterClickAction();
			});
			((Control)this).OnClick(e);
		}

		protected virtual async Task ClickAction()
		{
			if (ChatLink == null)
			{
				return;
			}
			if (!(await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(ChatLink)))
			{
				ScreenNotification.ShowNotification(Common.SearchItem_FailedToCopy, (NotificationType)6, (Texture2D)null, 2);
				return;
			}
			if (UniversalSearchModule.ModuleInstance.SettingShowNotificationWhenLandmarkIsCopied.get_Value())
			{
				ScreenNotification.ShowNotification(Common.SearchItem_Copied, (NotificationType)0, (Texture2D)null, 2);
			}
			if (UniversalSearchModule.ModuleInstance.SettingHideWindowAfterSelection.get_Value())
			{
				((Control)((Control)this).get_Parent()).Hide();
			}
		}

		protected virtual async Task AfterClickAction()
		{
			if (UniversalSearchModule.ModuleInstance.SettingEnterSelectionIntoChatAutomatically.get_Value())
			{
				IGameChat chat = GameService.GameIntegration.get_Chat();
				chat.Send(await ClipboardUtil.get_WindowsClipboardService().GetTextAsync());
			}
		}

		protected virtual Tooltip BuildTooltip()
		{
			return null;
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Tooltip() == null)
			{
				((Control)this).set_Tooltip(BuildTooltip());
			}
			Tooltip tooltip = ((Control)this).get_Tooltip();
			if (tooltip != null)
			{
				Rectangle absoluteBounds = ((Control)this).get_AbsoluteBounds();
				tooltip.Show(((Rectangle)(ref absoluteBounds)).get_Location() + new Point(((Control)this).get_Width() + 5, 0));
			}
			((Control)this).OnMouseEntered(e);
		}

		public override void RecalculateLayout()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			_layoutIconBounds = new Rectangle(2, 2, 32, 32);
			int iconRight = ((Rectangle)(ref _layoutIconBounds)).get_Right() + 2;
			_layoutNameBounds = new Rectangle(iconRight, 0, base._size.X - iconRight, 20);
			_layoutDescriptionBounds = new Rectangle(iconRight, ((Rectangle)(ref _layoutNameBounds)).get_Bottom(), base._size.X - iconRight, 16);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			if (base._mouseOver)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureItemHover, bounds, Color.get_White() * 0.5f);
			}
			if (_icon != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_icon), _layoutIconBounds);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _name, Control.get_Content().get_DefaultFont14(), _layoutNameBounds, Color.get_White(), false, false, 1, (HorizontalAlignment)0, (VerticalAlignment)2);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _description, Control.get_Content().get_DefaultFont14(), _layoutDescriptionBounds, Colors.Chardonnay, false, false, 1, (HorizontalAlignment)0, (VerticalAlignment)0);
		}
	}
}
