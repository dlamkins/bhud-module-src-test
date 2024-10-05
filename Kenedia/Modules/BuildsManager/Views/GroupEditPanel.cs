using System.ComponentModel;
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

namespace Kenedia.Modules.BuildsManager.Views
{
	public class GroupEditPanel : Kenedia.Modules.Core.Controls.Panel
	{
		private TagGroup _group;

		private readonly (Kenedia.Modules.Core.Controls.Label label, Kenedia.Modules.Core.Controls.TextBox textBox) _name;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _iconId;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _priority;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _x;

		private readonly bool _created;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _y;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _width;

		private readonly (Kenedia.Modules.Core.Controls.Label label, NumberBox numberBox) _height;

		private readonly Button _resetButton;

		private new readonly Kenedia.Modules.Core.Controls.Image _icon;

		private Blish_HUD.Controls.Container? _draggingStartParent;

		private Point _draggingStart;

		private bool _dragging;

		private bool _loading;

		public TagGroup Group
		{
			get
			{
				return _group;
			}
			set
			{
				Common.SetProperty(ref _group, value, new ValueChangedEventHandler<TagGroup>(OnTagChanged));
			}
		}

		public TemplateTags TemplateTags { get; set; }

		public TagGroups TagGroups { get; }

		public GroupEditPanel(TagGroups tagGroups)
		{
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			base.ContentPadding = new RectangleDimensions(5);
			TagGroups = tagGroups;
			_name = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.GroupName
			}, new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = this,
				Width = 200,
				Height = 32,
				SetLocalizedPlaceholder = () => strings.GroupName,
				Location = new Point(0, Control.Content.DefaultFont14.get_LineHeight() + 2),
				TextChangedAction = delegate(string txt)
				{
					if (!string.IsNullOrEmpty(txt))
					{
						Group.Name = txt;
					}
				}
			});
			_icon = new Kenedia.Modules.Core.Controls.Image
			{
				Parent = this,
				Size = new Point(32),
				Location = new Point(0, Control.Content.DefaultFont14.get_LineHeight() + 2),
				BackgroundColor = Color.get_Black() * 0.4f,
				Visible = false
			};
			_iconId = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.AssetId,
				Visible = false
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
				},
				Visible = false
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
				Location = new Point(0, _icon.Bottom + 25 + Control.Content.DefaultFont14.get_LineHeight() + 2),
				Height = 32,
				ValueChangedAction = delegate
				{
					SetPriority();
				}
			});
			_x = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => "X",
				HorizontalAlignment = HorizontalAlignment.Center,
				Visible = false
			}, new NumberBox
			{
				Parent = this,
				ShowButtons = false,
				Height = 25,
				ValueChangedAction = delegate
				{
					SetTextureRegion();
				},
				Visible = false
			});
			_y = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => "Y",
				HorizontalAlignment = HorizontalAlignment.Center,
				Visible = false
			}, new NumberBox
			{
				Parent = this,
				ShowButtons = false,
				Height = 25,
				ValueChangedAction = delegate
				{
					SetTextureRegion();
				},
				Visible = false
			});
			_width = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.Width,
				HorizontalAlignment = HorizontalAlignment.Center,
				Visible = false
			}, new NumberBox
			{
				Parent = this,
				ShowButtons = false,
				Height = 25,
				ValueChangedAction = delegate
				{
					SetTextureRegion();
				},
				Visible = false
			});
			_height = (new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.Height,
				HorizontalAlignment = HorizontalAlignment.Center,
				Visible = false
			}, new NumberBox
			{
				Parent = this,
				ShowButtons = false,
				Height = 25,
				ValueChangedAction = delegate
				{
					SetTextureRegion();
				},
				Visible = false
			});
			_resetButton = new Button
			{
				Parent = this,
				Text = strings.Reset,
				Height = 25,
				ClickAction = delegate
				{
					SetTextureRegionToTextureBounds(Group.Icon.Texture);
				},
				Visible = false
			};
			_created = true;
			base.Menu = new ContextMenuStrip();
			base.Menu.AddMenuItem(new ContextMenuItem(() => strings.Delete, delegate
			{
				RemoveTag(Group);
			}));
		}

		private void SetTextureRegion()
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			if (!_loading)
			{
				Group.TextureRegion = new Rectangle(_x.numberBox.Value, _y.numberBox.Value, _width.numberBox.Value, _height.numberBox.Value);
			}
		}

		private void RemoveTag(TagGroup group)
		{
			TagGroups.Remove(group);
		}

		private void OnTagChanged(object sender, ValueChangedEventArgs<TagGroup> e)
		{
			TagGroup group = e.OldValue;
			if (group != null)
			{
				group.PropertyChanged -= new PropertyChangedEventHandler(Group_TagChanged);
			}
			group = e.NewValue;
			if (group != null)
			{
				group.PropertyChanged += new PropertyChangedEventHandler(Group_TagChanged);
			}
			ApplyTag(group);
		}

		private void Group_TagChanged(object sender, PropertyChangedEventArgs e)
		{
			TagGroup group = sender as TagGroup;
			if (group != null)
			{
				ApplyTag(group);
			}
		}

		private void ApplyTag(TagGroup? group)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			_loading = true;
			Rectangle r = (Rectangle)(((_003F?)group?.TextureRegion) ?? ((_003F?)group?.Icon?.Bounds) ?? Rectangle.get_Empty());
			_icon.SourceRectangle = r;
			_priority.numberBox.Value = group?.Priority ?? 1;
			_name.textBox.Text = group?.Name;
			_icon.Texture = group?.Icon?.Texture;
			_iconId.numberBox.Value = group?.AssetId ?? 0;
			_x.numberBox.Value = r.X;
			_y.numberBox.Value = r.Y;
			_width.numberBox.Value = r.Width;
			_height.numberBox.Value = r.Height;
			_loading = false;
		}

		private void SetTextureRegionToTextureBounds(AsyncTexture2D icon)
		{
			if (icon != null)
			{
				_x.numberBox.Value = 0;
				_y.numberBox.Value = 0;
				_width.numberBox.Value = icon.Width;
				_height.numberBox.Value = icon.Height;
			}
		}

		private void SetPriority()
		{
			if (!_loading)
			{
				Group.Priority = _priority.numberBox.Value;
			}
		}

		private void SetIcon()
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			if (_loading)
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
				Group.AssetId = _iconId.numberBox.Value;
				Group.Icon.Texture = icon;
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (_created)
			{
				int x = base.ContentRegion.Width;
				_ = base.ContentRegion;
				_name.textBox.Width = x;
				_name.label.Location = new Point(_name.textBox.Left, 0);
				_name.label.Width = _name.textBox.Width;
				_icon.Location = new Point(_name.textBox.Right + 5, _name.textBox.Top);
				_iconId.numberBox.Location = new Point(_icon.Right + 5, _name.textBox.Top);
				_iconId.label.Location = new Point(_iconId.numberBox.Left, 0);
				_iconId.label.Width = _iconId.numberBox.Width;
				_priority.label.Location = new Point(_name.textBox.Left, _name.textBox.Bottom + 25);
				_priority.numberBox.Location = new Point(_name.textBox.Left, _priority.label.Bottom + 3);
				int num2 = (_priority.numberBox.Width = (_priority.label.Width = _name.textBox.Width));
				int amount = 5;
				int padding = 5;
				int w = (x - padding * amount) / amount;
				_x.label.Location = new Point(0, _priority.numberBox.Bottom + 25);
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
			}
		}
	}
}
