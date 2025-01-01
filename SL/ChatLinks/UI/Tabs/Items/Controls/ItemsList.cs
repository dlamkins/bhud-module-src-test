using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using GuildWars2.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SL.ChatLinks.UI.Tabs.Items.Controls
{
	public sealed class ItemsList : FlowPanel
	{
		private readonly IReadOnlyDictionary<int, UpgradeComponent> _upgrades;

		private bool _loading;

		public event EventHandler<Item>? OptionClicked;

		public ItemsList(IReadOnlyDictionary<int, UpgradeComponent> upgrades)
			: this()
		{
			_upgrades = upgrades;
			((Panel)this).set_ShowTint(true);
			((Panel)this).set_ShowBorder(true);
			((Panel)this).set_CanScroll(true);
		}

		public void SetLoading(bool loading)
		{
			_loading = loading;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_loading)
			{
				Point location = ((Rectangle)(ref bounds)).get_Center() - new Point(32);
				Rectangle rect = default(Rectangle);
				((Rectangle)(ref rect))._002Ector(location, new Point(64));
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, rect);
			}
		}

		public void ClearOptions()
		{
			Task.Factory.StartNew(delegate(object state)
			{
				foreach (Control item in (IEnumerable<Control>)state)
				{
					item.remove_Click((EventHandler<MouseEventArgs>)OptionClick);
					item.Dispose();
				}
			}, ((Container)this).get_Children().ToList());
			using (((Control)this).SuspendLayoutContext())
			{
				((Container)this).ClearChildren();
			}
		}

		public void AddOption(Item item)
		{
			ItemsListOption itemsListOption = new ItemsListOption(item, _upgrades);
			((Control)itemsListOption).set_Parent((Container)(object)this);
			((Control)itemsListOption).add_Click((EventHandler<MouseEventArgs>)OptionClick);
		}

		public void SetOptions(IEnumerable<Item> items)
		{
			ClearOptions();
			foreach (Item item in items)
			{
				AddOption(item);
			}
			if (((Panel)this).get_CanScroll())
			{
				((Panel)this).set_CanScroll(false);
				((Panel)this).set_CanScroll(true);
			}
		}

		private void OptionClick(object? sender, MouseEventArgs e)
		{
			ItemsListOption clickedOption = sender as ItemsListOption;
			if (clickedOption != null)
			{
				this.OptionClicked?.Invoke(this, clickedOption.Item);
			}
		}
	}
}
