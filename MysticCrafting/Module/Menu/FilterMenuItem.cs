using System;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using MysticCrafting.Module.Extensions;
using Newtonsoft.Json;

namespace MysticCrafting.Module.Menu
{
	public class FilterMenuItem : MenuItem
	{
		private readonly AsyncTexture2D _textureArrow = AsyncTexture2D.FromAssetId(156057);

		public Color TextColor = Color.get_White();

		public bool IsSelectAllOption;

		[JsonIgnore]
		public float ArrowRotation { get; set; } = -(float)Math.PI / 2f;


		private int LeftSidePadding
		{
			get
			{
				int leftSidePadding = 10;
				if (!((Container)this)._children.get_IsEmpty())
				{
					leftSidePadding += 16;
				}
				return leftSidePadding;
			}
		}

		private Rectangle FirstItemBoxRegion => new Rectangle(0, ((MenuItem)this).get_MenuItemHeight() / 2 - 16, 32, 32);

		public FilterMenuItem()
			: this("", null)
		{
		}

		public FilterMenuItem(string text)
			: this(text, null)
		{
		}

		public FilterMenuItem(string text, AsyncTexture2D icon)
			: this(text, icon)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			base._text = text;
			base._icon = icon;
		}

		protected override void OnCheckedChanged(CheckChangedEvent e)
		{
			Container parent = ((Control)this).get_Parent();
			Menu menu = (Menu)(object)((parent is Menu) ? parent : null);
			if (menu != null && IsSelectAllOption)
			{
				foreach (FilterMenuItem item in from i in menu.Items()
					where i != this
					select i)
				{
					item.SetCheckedSilently(((MenuItem)this).get_Checked());
				}
			}
			((MenuItem)this).OnCheckedChanged(e);
		}

		public void SetCheckedSilently(bool @checked)
		{
			base._checked = @checked;
		}

		private void DrawDropdownArrow(SpriteBatch spriteBatch)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			Vector2 origin = default(Vector2);
			((Vector2)(ref origin))._002Ector(8f, 8f);
			Rectangle destinationRectangle = default(Rectangle);
			((Rectangle)(ref destinationRectangle))._002Ector(13, ((MenuItem)this).get_MenuItemHeight() / 2, 16, 16);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureArrow), destinationRectangle, (Rectangle?)null, Color.get_White(), ArrowRotation, origin, (SpriteEffects)0);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			int leftSidePadding = LeftSidePadding;
			if (!((Container)this)._children.get_IsEmpty())
			{
				DrawDropdownArrow(spriteBatch);
			}
			TextureRegion2D texture = null;
			if (((MenuItem)this).get_CanCheck())
			{
				string state = (((MenuItem)this).get_Checked() ? "-checked" : "-unchecked");
				string extension = "";
				extension = ((!((Control)this).get_Enabled()) ? "-disabled" : extension);
				texture = Checkable.TextureRegionsCheckbox.First((TextureRegion2D cb) => cb.get_Name() == "checkbox/cb" + state + extension);
			}
			if (((MenuItem)this).get_Icon() != null && ((Container)this)._children.get_IsEmpty())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((MenuItem)this).get_Icon()), new Rectangle(leftSidePadding + 190, ((MenuItem)this).get_MenuItemHeight() / 2 - 16, 32, 32));
			}
			if (texture != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, texture, RectangleExtension.OffsetBy(FirstItemBoxRegion, leftSidePadding - 5, 0));
			}
			if (base._canCheck)
			{
				leftSidePadding += 32;
			}
			else if (!((Container)this)._children.get_IsEmpty())
			{
				leftSidePadding += 10;
			}
			else if (base._icon != null)
			{
				leftSidePadding += 42;
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, base._text, GameService.Content.get_DefaultFont16(), new Rectangle(leftSidePadding, 0, ((Control)this).get_Width() - (leftSidePadding - 10), ((MenuItem)this).get_MenuItemHeight()), TextColor, true, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
