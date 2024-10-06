using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class TagEditPanel : Kenedia.Modules.Core.Controls.Panel
	{
		private TemplateTag _tag;

		private readonly (Kenedia.Modules.Core.Controls.Label label, Kenedia.Modules.Core.Controls.TextBox textBox) _name;

		private readonly (Kenedia.Modules.Core.Controls.Label label, Kenedia.Modules.Core.Controls.Dropdown textBox) _group;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _iconId;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _priority;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _x;

		private readonly bool _created;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _y;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _width;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _height;

		private readonly Button _resetButton;

		private readonly Button _deleteButton;

		private new readonly Kenedia.Modules.Core.Controls.Image _icon;

		private Blish_HUD.Controls.Container? _draggingStartParent;

		private Point _draggingStart;

		private bool _dragging;

		private bool _loading;

		public TemplateTag Tag
		{
			get
			{
				return _tag;
			}
			set
			{
				Common.SetProperty(ref _tag, value, new ValueChangedEventHandler<TemplateTag>(OnTagChanged));
			}
		}

		public TemplateTags TemplateTags { get; set; }

		public TagGroups TagGroups { get; }

		public TagEditPanel(TagGroups tagGroups)
		{
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			TagGroups = tagGroups;
			base.ContentPadding = new RectangleDimensions(5);
			_name = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.TagName
			}, new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = this,
				Width = 200,
				Height = 32,
				SetLocalizedPlaceholder = () => strings.TagName,
				Location = new Point(0, Control.Content.DefaultFont14.get_LineHeight() + 2),
				TextChangedAction = delegate(string txt)
				{
					if (!string.IsNullOrEmpty(txt) && Tag != null)
					{
						Tag.Name = txt;
					}
				}
			});
			_icon = new Kenedia.Modules.Core.Controls.Image
			{
				Parent = this,
				Size = new Point(32),
				Location = new Point(0, Control.Content.DefaultFont14.get_LineHeight() + 2),
				BackgroundColor = Color.get_Black() * 0.4f
			};
			_iconId = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.AssetId
			}, new NumberBox
			{
				Parent = this,
				Width = 100,
				ShowButtons = false,
				Location = new Point(0, Control.Content.DefaultFont14.get_LineHeight() + 2),
				Height = 32,
				ValueChangedAction = delegate
				{
					SetIcon();
				}
			});
			_priority = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.Priority
			}, new NumberBox
			{
				Parent = this,
				Width = 100,
				ShowButtons = true,
				MinValue = 0,
				Location = new Point(0, _icon.Bottom + 25 + Control.Content.DefaultFont14.get_LineHeight() + 2),
				Height = 32,
				ValueChangedAction = delegate
				{
					SetPriority();
				}
			});
			_group = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.Group
			}, new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = this,
				Width = 200,
				Height = 32,
				Location = new Point(0, _icon.Bottom + 25 + Control.Content.DefaultFont14.get_LineHeight() + 2),
				ValueChangedAction = new Action<string>(SetGroup)
			});
			_x = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => "X",
				HorizontalAlignment = HorizontalAlignment.Center
			}, new NumberBox
			{
				Parent = this,
				ShowButtons = false,
				Height = 25,
				ValueChangedAction = delegate
				{
					SetTextureRegion();
				}
			});
			_y = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => "Y",
				HorizontalAlignment = HorizontalAlignment.Center
			}, new NumberBox
			{
				Parent = this,
				ShowButtons = false,
				Height = 25,
				ValueChangedAction = delegate
				{
					SetTextureRegion();
				}
			});
			_width = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.Width,
				HorizontalAlignment = HorizontalAlignment.Center
			}, new NumberBox
			{
				Parent = this,
				ShowButtons = false,
				Height = 25,
				ValueChangedAction = delegate
				{
					SetTextureRegion();
				}
			});
			_height = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.Height,
				HorizontalAlignment = HorizontalAlignment.Center
			}, new NumberBox
			{
				Parent = this,
				ShowButtons = false,
				Height = 25,
				ValueChangedAction = delegate
				{
					SetTextureRegion();
				}
			});
			_resetButton = new Button
			{
				Parent = this,
				Text = strings.Reset,
				Height = 25,
				ClickAction = delegate
				{
					SetTextureRegionToTextureBounds(Tag?.Icon?.Texture);
				}
			};
			_deleteButton = new Button
			{
				Parent = this,
				Text = strings.Delete,
				Height = 25,
				ClickAction = delegate
				{
					RemoveTag(Tag);
				},
				Visible = false
			};
			_created = true;
			base.Menu = new ContextMenuStrip();
			base.Menu.AddMenuItem(new ContextMenuItem(() => strings.Delete, delegate
			{
				RemoveTag(Tag);
			}));
			SetGroupDropdownItems();
			TagGroups.GroupAdded += new EventHandler<TagGroup>(TagGroups_GroupAdded);
			TagGroups.GroupChanged += new PropertyChangedEventHandler(TagGroups_GroupChanged);
			TagGroups.GroupRemoved += new EventHandler<TagGroup>(TagGroups_GroupRemoved);
			ApplyTag();
		}

		private void SetGroupDropdownItems()
		{
			_group.textBox.Items.Clear();
			_group.textBox.Items.Add(string.Empty);
			foreach (TagGroup group in TagGroups)
			{
				_group.textBox.Items.Add(group.Name);
			}
			_group.textBox.SelectedItem = Tag?.Group;
		}

		private void TagGroups_GroupRemoved(object sender, TagGroup e)
		{
			SetGroupDropdownItems();
		}

		private void TagGroups_GroupChanged(object sender, PropertyChangedEventArgs e)
		{
			SetGroupDropdownItems();
		}

		private void TagGroups_GroupAdded(object sender, TagGroup e)
		{
			SetGroupDropdownItems();
		}

		private void SetGroup(string txt)
		{
			if (!_loading && Tag != null)
			{
				Tag.Group = txt;
			}
		}

		private void SetTextureRegion()
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			if (!_loading && Tag != null)
			{
				Tag.TextureRegion = new Rectangle(_x.numberBox.Value, _y.numberBox.Value, _width.numberBox.Value, _height.numberBox.Value);
			}
		}

		private void SetPriority()
		{
			if (!_loading && Tag != null)
			{
				Tag.Priority = _priority.numberBox.Value;
			}
		}

		private void RemoveTag(TemplateTag tag)
		{
			TemplateTags.Remove(Tag);
		}

		private void OnTagChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<TemplateTag> e)
		{
			TemplateTag tag = e.OldValue;
			if (tag != null)
			{
				tag.PropertyChanged -= new PropertyChangedEventHandler(Tag_TagChanged);
			}
			tag = e.NewValue;
			if (tag != null)
			{
				tag.PropertyChanged += new PropertyChangedEventHandler(Tag_TagChanged);
			}
			ApplyTag(tag);
		}

		private void Tag_TagChanged(object sender, PropertyChangedEventArgs e)
		{
			TemplateTag tag = sender as TemplateTag;
			if (tag != null)
			{
				ApplyTag(tag);
			}
		}

		private void ApplyTag(TemplateTag? tag = null)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			_loading = true;
			bool hasTag = tag != null;
			Rectangle r = (Rectangle)(((_003F?)tag?.TextureRegion) ?? ((_003F?)tag?.Icon?.Bounds) ?? Rectangle.get_Empty());
			_icon.SourceRectangle = r;
			_priority.numberBox.Value = tag?.Priority ?? 1;
			_priority.numberBox.Enabled = hasTag;
			_group.textBox.SelectedItem = tag?.Group;
			_group.textBox.Enabled = hasTag;
			_name.textBox.Text = tag?.Name;
			_name.textBox.Enabled = hasTag;
			_icon.Texture = tag?.Icon?.Texture;
			_iconId.numberBox.Value = tag?.AssetId ?? 0;
			_iconId.numberBox.Enabled = hasTag;
			_x.numberBox.Value = r.X;
			_x.numberBox.Enabled = hasTag;
			_y.numberBox.Value = r.Y;
			_y.numberBox.Enabled = hasTag;
			_width.numberBox.Value = r.Width;
			_width.numberBox.Enabled = hasTag;
			_height.numberBox.Value = r.Height;
			_height.numberBox.Enabled = hasTag;
			_loading = false;
		}

		private void SetTextureRegionToTextureBounds(AsyncTexture2D icon)
		{
			if (!_loading && icon != null)
			{
				_x.numberBox.Value = 0;
				_y.numberBox.Value = 0;
				_width.numberBox.Value = icon.Width;
				_height.numberBox.Value = icon.Height;
			}
		}

		private void SetIcon()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			if (_loading || Tag == null)
			{
				return;
			}
			AsyncTexture2D icon = AsyncTexture2D.FromAssetId(_iconId.numberBox.Value);
			if (icon != null)
			{
				_icon.SourceRectangle = ((icon != _icon.Texture) ? new Rectangle(0, 0, icon.Width, icon.Height) : new Rectangle(_x.numberBox.Value, _y.numberBox.Value, _width.numberBox.Value, _height.numberBox.Value));
				if (icon != _icon.Texture)
				{
					SetTextureRegionToTextureBounds(icon);
				}
				_icon.Texture = icon;
				Tag.AssetId = _iconId.numberBox.Value;
				Tag.Icon.Texture = icon;
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0394: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0468: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (_created)
			{
				int x = base.ContentRegion.Width;
				_ = base.ContentRegion;
				_name.textBox.Width = x - (_iconId.numberBox.Width + _icon.Width + 5);
				_name.label.Location = new Point(_name.textBox.Left, 0);
				_name.label.Width = _name.textBox.Width;
				_icon.Location = new Point(_name.textBox.Right + 5, _name.textBox.Top);
				_iconId.numberBox.Location = new Point(_icon.Right + 5, _name.textBox.Top);
				_iconId.label.Location = new Point(_iconId.numberBox.Left, 0);
				_iconId.label.Width = _iconId.numberBox.Width;
				_group.textBox.Width = x - (_iconId.numberBox.Width + _icon.Width + 5);
				_group.label.Location = new Point(_group.textBox.Left, _name.textBox.Bottom + 25);
				_priority.label.Location = new Point(_icon.Left, _group.label.Top);
				_priority.numberBox.Location = new Point(_icon.Left, _group.textBox.Top);
				int num3 = (_priority.numberBox.Width = (_priority.label.Width = _iconId.numberBox.Width + _icon.Width + 3));
				int amount = 5;
				int padding = 5;
				int w = (x - padding * amount) / amount;
				_x.label.Location = new Point(0, _group.textBox.Bottom + 25);
				_x.label.Width = w;
				_x.numberBox.Location = new Point(_x.label.Left, _x.label.Bottom);
				_x.numberBox.Width = w;
				_y.label.Location = new Point(_x.numberBox.Right + 5, _x.label.Top);
				_y.label.Width = w;
				_y.numberBox.Location = new Point(_y.label.Left, _y.label.Bottom);
				_y.numberBox.Width = w;
				_width.label.Location = new Point(_y.numberBox.Right + 5, _y.label.Top);
				_width.label.Width = w;
				_width.numberBox.Location = new Point(_width.label.Left, _width.label.Bottom);
				_width.numberBox.Width = w;
				_height.label.Location = new Point(_width.numberBox.Right + 5, _width.label.Top);
				_height.label.Width = w;
				_height.numberBox.Location = new Point(_height.label.Left, _height.label.Bottom);
				_height.numberBox.Width = w;
				_resetButton.Location = new Point(_height.numberBox.Right + 5, _height.numberBox.Top);
				_resetButton.Width = w;
				_deleteButton.Location = new Point(_height.numberBox.Right + 5, _group.textBox.Top);
				_deleteButton.Width = w;
			}
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			if (Tag == null)
			{
				Rectangle scissorRectangle = Rectangle.Intersect(scissor, base.AbsoluteBounds.WithPadding(_padding)).ScaleBy(Control.Graphics.UIScaleMultiplier);
				((GraphicsResource)spriteBatch).get_GraphicsDevice().set_ScissorRectangle(scissorRectangle);
				base.EffectBehind?.Draw(spriteBatch, drawBounds);
				spriteBatch.Begin(base.SpriteBatchParameters);
				ShapeExtensions.FillRectangle(spriteBatch, RectangleF.op_Implicit(base.AbsoluteBounds), (Color)(((_003F?)base.BackgroundColor) ?? (Color.get_Black() * 0.5f)), 0f);
				spriteBatch.DrawStringOnCtrl(this, strings.SelectTagToEdit, Control.Content.DefaultFont18, drawBounds, Color.get_White(), wrap: false, HorizontalAlignment.Center);
				spriteBatch.End();
			}
			else
			{
				base.Draw(spriteBatch, drawBounds, scissor);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
		}
	}
}
