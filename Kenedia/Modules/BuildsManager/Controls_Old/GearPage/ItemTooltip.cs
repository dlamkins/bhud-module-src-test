using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls_Old.GearPage
{
	public class ItemTooltip : Tooltip
	{
		private readonly Kenedia.Modules.Core.Controls.Image _image;

		private readonly Kenedia.Modules.Core.Controls.Label _title;

		private readonly Kenedia.Modules.Core.Controls.Label _id;

		private readonly Kenedia.Modules.Core.Controls.Label _description;

		private readonly Kenedia.Modules.Core.Controls.Label _commentLabel;

		private BaseItem? _item;

		private Stat? _stat;

		private string _comment;

		private Color _frameColor = Color.get_Transparent();

		private Func<string> _setLocalizedComment;

		public BaseItem? Item
		{
			get
			{
				return _item;
			}
			set
			{
				Common.SetProperty<BaseItem>(ref _item, value, new ValueChangedEventHandler<BaseItem>(ApplyItem));
			}
		}

		public Stat? Stat
		{
			get
			{
				return _stat;
			}
			set
			{
				Common.SetProperty<Stat>(ref _stat, value, new ValueChangedEventHandler<Stat>(ApplyStat));
			}
		}

		public string Comment
		{
			get
			{
				return _comment;
			}
			set
			{
				Common.SetProperty(ref _comment, value, new ValueChangedEventHandler<string>(ApplyComment));
			}
		}

		public Func<string> SetLocalizedComment
		{
			get
			{
				return _setLocalizedComment;
			}
			set
			{
				Common.SetProperty<Func<string>>(ref _setLocalizedComment, value, new ValueChangedEventHandler<Func<string>>(ApplyLocalizedComment));
			}
		}

		public ItemTooltip()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			base.AutoSizePadding = new Point(5);
			_image = new Kenedia.Modules.Core.Controls.Image
			{
				Parent = this,
				Size = new Point(48),
				Location = new Point(2)
			};
			_title = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = Control.Content.DefaultFont16.get_LineHeight(),
				AutoSizeWidth = true,
				Location = new Point(_image.Right + 10, _image.Top),
				Font = Control.Content.DefaultFont16
			};
			_id = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = Control.Content.DefaultFont12.get_LineHeight(),
				AutoSizeWidth = true,
				Location = new Point(_image.Right + 10, _title.Bottom),
				Font = Control.Content.DefaultFont12,
				TextColor = Color.get_White() * 0.8f
			};
			_description = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Width = 300,
				AutoSizeHeight = true,
				Location = new Point(_image.Left, _image.Bottom + 10),
				Font = Control.Content.DefaultFont14,
				WrapText = true
			};
			_commentLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Width = 300,
				AutoSizeHeight = true,
				Location = new Point(_description.Left, _description.Bottom + 10),
				Font = Control.Content.DefaultFont12,
				TextColor = Color.get_DarkGray(),
				WrapText = true,
				Visible = false
			};
			LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
		}

		private void ApplyLocalizedComment(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Func<string>> e)
		{
			Comment = SetLocalizedComment?.Invoke();
		}

		private void ApplyComment(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<string> e)
		{
			_commentLabel.Text = Comment;
			_commentLabel.Visible = !string.IsNullOrEmpty(Comment);
		}

		private void UserLocale_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			ApplyItem(this, null);
			Comment = SetLocalizedComment?.Invoke();
		}

		private void ApplyStat(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Stat> e)
		{
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			if (Stat == null)
			{
				_description.Text = string.Empty;
				return;
			}
			ItemType? itemType = Item?.Type;
			if (!itemType.HasValue)
			{
				return;
			}
			switch (itemType.GetValueOrDefault())
			{
			case ItemType.Armor:
			{
				Armor armor = Item as Armor;
				if (armor != null)
				{
					_description.Text = Stat?.Name + Environment.NewLine + Stat?.Attributes.ToString(armor.AttributeAdjustment);
					_description.TextColor = Color.get_Lime();
				}
				break;
			}
			case ItemType.Weapon:
			{
				Weapon weapon = Item as Weapon;
				if (weapon != null)
				{
					_description.Text = Stat?.Name + Environment.NewLine + Stat?.Attributes.ToString(weapon.AttributeAdjustment);
					_description.TextColor = Color.get_Lime();
				}
				break;
			}
			case ItemType.Back:
			case ItemType.Trinket:
			{
				Trinket trinket = Item as Trinket;
				if (trinket != null)
				{
					_description.Text = Stat?.Name + Environment.NewLine + Stat?.Attributes.ToString(trinket.AttributeAdjustment);
					_description.TextColor = Color.get_Lime();
				}
				break;
			}
			}
		}

		private void ApplyItem(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<BaseItem> e)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			_image.Texture = TexturesService.GetAsyncTexture(Item?.AssetId);
			_frameColor = Item?.Rarity.GetColor() ?? Color.get_Transparent();
			_title.Text = Item?.Name;
			_id.Text = $"{strings.ItemId}: {Item?.Id}";
			_title.TextColor = Item?.Rarity.GetColor() ?? Color.get_White();
			if (_image.Texture != null)
			{
				int padding = _image.Texture.Width / 16;
				_image.SourceRectangle = new Rectangle(padding, padding, _image.Texture.Width - padding * 2, _image.Texture.Height - padding * 2);
			}
			ItemType? itemType = Item?.Type;
			if (!itemType.HasValue)
			{
				return;
			}
			switch (itemType.GetValueOrDefault())
			{
			case ItemType.PowerCore:
			case ItemType.Relic:
				_description.Text = Item!.Description ?? strings.MissingInfoFromAPI;
				break;
			case ItemType.Consumable:
			{
				Nourishment nourishment = Item as Nourishment;
				if (nourishment != null)
				{
					_description.Text = nourishment?.Details?.Description ?? strings.MissingInfoFromAPI;
					break;
				}
				Enhancement enhancement = Item as Enhancement;
				if (enhancement != null)
				{
					_description.Text = enhancement?.Details?.Description ?? strings.MissingInfoFromAPI;
				}
				break;
			}
			case ItemType.Armor:
			case ItemType.Back:
			case ItemType.Trinket:
			case ItemType.Weapon:
				ApplyStat(sender, null);
				break;
			case ItemType.UpgradeComponent:
			{
				Rune rune = Item as Rune;
				if (rune != null)
				{
					_description.Text = rune.Bonus;
					break;
				}
				Sigil sigil = Item as Sigil;
				if (sigil != null)
				{
					_description.Text = sigil.Buff ?? strings.MissingInfoFromAPI;
					break;
				}
				Infusion infusion = Item as Infusion;
				if (infusion != null)
				{
					_description.Text = infusion.DisplayText ?? strings.MissingInfoFromAPI;
					break;
				}
				Enrichment enrichment = Item as Enrichment;
				if (enrichment != null)
				{
					_description.Text = enrichment.DisplayText ?? strings.MissingInfoFromAPI;
				}
				break;
			}
			case ItemType.PvpAmulet:
			{
				PvpAmulet pvpAmulet = Item as PvpAmulet;
				if (pvpAmulet != null)
				{
					_description.Text = pvpAmulet?.AttributesString;
					_description.TextColor = Color.get_Lime();
				}
				break;
			}
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_commentLabel?.SetLocation(new Point(_description?.Left ?? 0, (_description?.Bottom ?? 0) + 2));
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (Item != null)
			{
				base.Draw(spriteBatch, drawBounds, scissor);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			if (_image?.Texture != null)
			{
				spriteBatch.DrawFrame(this, _image.LocalBounds.Add(2, 2, 6, 4), _frameColor, 2);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			LocalizingService.LocaleChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
		}
	}
}
