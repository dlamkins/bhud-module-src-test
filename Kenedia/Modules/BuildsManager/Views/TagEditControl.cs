using System.ComponentModel;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class TagEditControl : Kenedia.Modules.Core.Controls.Panel
	{
		private TemplateTag _tag;

		private readonly (Kenedia.Modules.Core.Controls.Label label, Kenedia.Modules.Core.Controls.TextBox textBox) _name;

		private readonly (Kenedia.Modules.Core.Controls.Label label, Kenedia.Modules.Core.Controls.TextBox textBox) _group;

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

		public TagEditControl()
		{
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			base.Height = 200;
			base.CanCollapse = true;
			base.Collapsed = true;
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
					if (!string.IsNullOrEmpty(txt))
					{
						Tag.Name = txt;
						base.Title = txt;
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
				Location = new Point(0, _icon.Bottom + 5 + Control.Content.DefaultFont14.get_LineHeight() + 2),
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
			}, new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = this,
				Width = 200,
				Height = 32,
				SetLocalizedPlaceholder = () => strings.Group,
				Location = new Point(0, _icon.Bottom + 5 + Control.Content.DefaultFont14.get_LineHeight() + 2),
				TextChangedAction = delegate(string txt)
				{
					if (!string.IsNullOrEmpty(txt))
					{
						Tag.Group = txt;
					}
				}
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
					SetIcon();
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
					SetIcon();
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
					SetIcon();
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
					SetIcon();
				}
			});
			_resetButton = new Button
			{
				Parent = this,
				Text = strings.Reset,
				Height = 25,
				ClickAction = delegate
				{
					SetTextureRegionToTextureBounds(Tag.Icon.Texture);
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
		}

		private void SetPriority()
		{
			Tag.Priority = _priority.numberBox.Value;
		}

		private void RemoveTag(TemplateTag tag)
		{
			TemplateTags.Remove(Tag);
			Dispose();
		}

		private void OnTagChanged(object sender, ValueChangedEventArgs<TemplateTag> e)
		{
			TemplateTag tag = e.NewValue;
			if (tag != null)
			{
				tag.PropertyChanged += new PropertyChangedEventHandler(Tag_TagChanged);
				ApplyTag(tag);
			}
		}

		private void Tag_TagChanged(object sender, PropertyChangedEventArgs e)
		{
			TemplateTag tag = sender as TemplateTag;
			if (tag != null)
			{
				ApplyTag(tag);
			}
		}

		private void ApplyTag(TemplateTag tag)
		{
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			base.Title = tag?.Name + $" [{strings.Priority}: {tag?.Priority}]";
			base.TitleIcon = tag?.Icon?.Texture;
			Rectangle r = (Rectangle)(((_003F?)tag?.TextureRegion) ?? ((_003F?)tag?.Icon?.Bounds) ?? Rectangle.get_Empty());
			_icon.SourceRectangle = r;
			base.TitleTextureRegion = r;
			_priority.numberBox.Value = tag?.Priority ?? 1;
			_group.textBox.Text = tag?.Group;
			_name.textBox.Text = tag?.Name;
			_icon.Texture = tag?.Icon?.Texture;
			_iconId.numberBox.Value = tag?.AssetId ?? 0;
			_x.numberBox.Value = r.X;
			_y.numberBox.Value = r.Y;
			_width.numberBox.Value = r.Width;
			_height.numberBox.Value = r.Height;
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

		private void SetIcon()
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D icon = AsyncTexture2D.FromAssetId(_iconId.numberBox.Value);
			if (icon != null)
			{
				_icon.SourceRectangle = ((icon != _icon.Texture) ? new Rectangle(0, 0, icon.Width, icon.Height) : new Rectangle(_x.numberBox.Value, _y.numberBox.Value, _width.numberBox.Value, _height.numberBox.Value));
				if (icon != _icon.Texture)
				{
					SetTextureRegionToTextureBounds(icon);
				}
				_icon.Texture = icon;
				base.TitleIcon = icon;
				Tag.AssetId = _iconId.numberBox.Value;
				Tag.Icon.Texture = icon;
				Rectangle? val3 = (base.TitleTextureRegion = (Tag.TextureRegion = _icon.SourceRectangle));
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
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_0466: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
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
				_group.label.Location = new Point(_group.textBox.Left, _name.textBox.Bottom + 5);
				_priority.label.Location = new Point(_icon.Left, _group.label.Top);
				_priority.numberBox.Location = new Point(_icon.Left, _group.textBox.Top);
				int num3 = (_priority.numberBox.Width = (_priority.label.Width = _iconId.numberBox.Width + _icon.Width + 3));
				int amount = 5;
				int padding = 5;
				int w = (x - padding * amount) / amount;
				_x.label.Location = new Point(0, _group.textBox.Bottom + 5);
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

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateContainer(gameTime);
			_dragging = _dragging && base.MouseOver;
			if (_dragging)
			{
				base.Location = Control.Input.Mouse.Position.Add(new Point(-_draggingStart.X, -_draggingStart.Y));
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			base.OnLeftMouseButtonReleased(e);
			if (_dragging)
			{
				_dragging = false;
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			base.OnLeftMouseButtonPressed(e);
		}

		private void StartDrag()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			Rectangle contentRegion = base.ContentRegion;
			Rectangle iconBounds = default(Rectangle);
			((Rectangle)(ref iconBounds))._002Ector(((Rectangle)(ref contentRegion)).get_Left(), 0, 36, 36);
			if (((Rectangle)(ref iconBounds)).Contains(base.RelativeMousePosition))
			{
				_dragging = true;
				_draggingStart = (_dragging ? base.RelativeMousePosition : Point.get_Zero());
				if (_draggingStartParent == null)
				{
					_draggingStartParent = base.Parent;
				}
				base.Parent = Control.Graphics.SpriteScreen;
				base.Location = Control.Graphics.SpriteScreen.RelativeMousePosition;
				ZIndex = (base.Parent?.ZIndex ?? 100) + 100;
			}
		}
	}
}
