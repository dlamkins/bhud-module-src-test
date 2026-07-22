using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class TemplateSelectable : Kenedia.Modules.Core.Controls.Panel
	{
		private readonly AsyncTexture2D _lineTexture = AsyncTexture2D.FromAssetId(605025);

		private readonly AsyncTexture2D _textureVignette = AsyncTexture2D.FromAssetId(605003);

		private readonly AsyncTexture2D _textureCornerButton = AsyncTexture2D.FromAssetId(605011);

		private readonly AsyncTexture2D _textureBottomSectionSeparator = AsyncTexture2D.FromAssetId(157218);

		private readonly BitmapFont _nameFont = Control.Content.DefaultFont14;

		private readonly BitmapFont _lastModifiedFont = UI.GetFont(ContentService.FontSize.Size11, ContentService.FontStyle.Regular);

		private readonly BitmapFont _notificationFont = UI.GetFont(ContentService.FontSize.Size14, ContentService.FontStyle.Regular);

		private readonly ImageButton _editButton;

		private readonly Kenedia.Modules.Core.Controls.TextBox _nameEdit;

		private readonly bool _created;

		private readonly Kenedia.Modules.Core.Controls.Label _name;

		private readonly Kenedia.Modules.Core.Controls.Label _lastModified;

		private readonly List<TagTexture> _tagTextures = new List<TagTexture>();

		private Rectangle _separatorBounds;

		private Rectangle _editBounds;

		private Rectangle _specBounds;

		private Rectangle _raceBounds;

		private Rectangle _raceAndSpecBounds;

		private Rectangle _leftAccentBorderBounds;

		private Rectangle _rightAccentBorderBounds;

		private Rectangle _bottomBounds;

		private Rectangle _vignetteBounds;

		private Rectangle _tagBounds;

		private double _animationStart;

		private double _animationDuration = 1500.0;

		private float _animationOpacityStep = 1f;

		private bool _animationRunning;

		private AsyncTexture2D _raceTexture;

		private AsyncTexture2D _specTexture;

		public Action DisposeAction { get; set; }

		public Action OnClickAction { get; set; }

		public Action OnNameChangedAction { get; set; }

		public Template Template
		{
			[CompilerGenerated]
			get
			{
				return _003CTemplate_003Ek__BackingField;
			}
			set
			{
				Template temp = _003CTemplate_003Ek__BackingField;
				if (Common.SetProperty(_003CTemplate_003Ek__BackingField, value, delegate(Template v)
				{
					_003CTemplate_003Ek__BackingField = v;
				}, new Action(ApplyTemplate)))
				{
					if (temp != null)
					{
						temp.RaceChanged -= new ValueChangedEventHandler<Races>(Template_RaceChanged);
					}
					if (_003CTemplate_003Ek__BackingField != null)
					{
						_003CTemplate_003Ek__BackingField.RaceChanged += new ValueChangedEventHandler<Races>(Template_RaceChanged);
					}
					if (temp != null)
					{
						temp.ProfessionChanged -= new ValueChangedEventHandler<ProfessionType>(Template_ProfessionChanged);
					}
					if (_003CTemplate_003Ek__BackingField != null)
					{
						_003CTemplate_003Ek__BackingField.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(Template_ProfessionChanged);
					}
					if (temp != null)
					{
						temp.EliteSpecializationChanged -= new SpecializationChangedEventHandler(Template_EliteSpecializationChanged);
					}
					if (_003CTemplate_003Ek__BackingField != null)
					{
						_003CTemplate_003Ek__BackingField.EliteSpecializationChanged += new SpecializationChangedEventHandler(Template_EliteSpecializationChanged);
					}
					if (temp != null)
					{
						temp.Tags.CollectionChanged -= Tags_CollectionChanged;
					}
					if (_003CTemplate_003Ek__BackingField != null)
					{
						_003CTemplate_003Ek__BackingField.Tags.CollectionChanged += Tags_CollectionChanged;
					}
					if (temp != null)
					{
						temp.LastModifiedChanged -= new ValueChangedEventHandler<string>(Template_LastModifiedChanged);
					}
					if (_003CTemplate_003Ek__BackingField != null)
					{
						_003CTemplate_003Ek__BackingField.LastModifiedChanged += new ValueChangedEventHandler<string>(Template_LastModifiedChanged);
					}
				}
			}
		}

		public TemplatePresenter TemplatePresenter { get; }

		public TemplateCollection Templates { get; }

		public Data Data { get; }

		public TemplateTags TemplateTags { get; }

		public TemplateFactory TemplateFactory { get; }

		public TemplateSelectable(TemplatePresenter templatePresenter, TemplateCollection templates, Data data, TemplateTags templateTags, TemplateFactory templateFactory)
		{
			Data = data;
			Templates = templates;
			TemplateTags = templateTags;
			TemplateFactory = templateFactory;
			TemplatePresenter = templatePresenter;
			base.Height = 85;
			base.BorderWidth = new RectangleDimensions(3);
			base.BorderColor = Color.Black;
			_name = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = _nameFont.LineHeight,
				Font = _nameFont,
				WrapText = true,
				VerticalAlignment = VerticalAlignment.Middle
			};
			_lastModified = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = _lastModifiedFont.LineHeight,
				Font = _lastModifiedFont,
				TextColor = Color.White * 0.7f,
				WrapText = false,
				VerticalAlignment = VerticalAlignment.Middle
			};
			_nameEdit = new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = this,
				Height = _nameFont.LineHeight,
				Font = _nameFont,
				Visible = false,
				HideBackground = true,
				EnterPressedAction = delegate(string txt)
				{
					string moddedtxt = txt.Trim().ToLower();
					Template template2 = Templates.Where((Template e) => e.Name.ToLower() == moddedtxt).FirstOrDefault();
					if (template2 == null || template2 == Template)
					{
						Template?.ChangeName(txt);
						ToggleEditMode(enable: false);
						OnNameChangedAction?.Invoke();
					}
					else
					{
						_nameEdit.Focused = true;
					}
				},
				TextChangedAction = delegate(string txt)
				{
					string txt2 = txt;
					txt2 = txt2.Trim().ToLower();
					Template template = Templates.Where((Template e) => e.Name.ToLower() == txt2).FirstOrDefault();
					_nameEdit.ForeColor = ((template == null || template == Template) ? Color.White : Color.Red);
				}
			};
			_editButton = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(2175779),
				DisabledTexture = AsyncTexture2D.FromAssetId(2175780),
				TextureRectangle = new Rectangle(2, 2, 28, 28),
				Size = new Point(20),
				ClickAction = delegate
				{
					ToggleEditMode(!_nameEdit.Visible);
				},
				SetLocalizedTooltip = () => strings.Rename
			};
			Control.Input.Mouse.LeftMouseButtonPressed += Mouse_LeftMouseButtonPressed;
			SetTooltip();
			base.Menu = new ContextMenuStrip();
			base.Menu.AddMenuItem(new ContextMenuItem(() => strings.Rename, delegate
			{
				ToggleEditMode(enable: true);
			}));
			base.Menu.AddMenuItem(new ContextMenuItem(() => strings.Duplicate, new Action(DuplicateTemplate)));
			base.Menu.AddMenuItem(new ContextMenuItem(() => strings.Delete, new Action(DeleteTemplate)));
			_created = true;
			LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(LocalizingService_OnLocaleChanged);
			templateTags.TagChanged += new PropertyChangedEventHandler(TemplateTags_TagChanged);
			templateTags.TagRemoved += new EventHandler<TemplateTag>(TemplateTags_TagRemoved);
			if (Data.IsLoaded)
			{
				ApplyTemplate();
			}
			Data.Loaded += new EventHandler(Data_Loaded);
		}

		private void Data_Loaded(object sender, EventArgs e)
		{
			ApplyTemplate();
		}

		private void TemplateTags_TagRemoved(object sender, TemplateTag e)
		{
			SetTagTextures();
		}

		private void TemplateTags_TagChanged(object sender, PropertyChangedEventArgs e)
		{
			SetTagTextures();
		}

		private void LocalizingService_OnLocaleChanged(object arg1, Blish_HUD.ValueChangedEventArgs<Locale> args)
		{
			SetTooltip();
		}

		private async void DeleteTemplate()
		{
			if (await new BaseDialog(strings.Warning, string.Format(strings.ConfirmTemplateDelete, Template?.Name))
			{
				DesiredWidth = 300,
				AutoSize = true
			}.ShowDialog() == DialogResult.OK)
			{
				Templates.Remove(Template);
				Template?.Delete();
			}
		}

		private void DuplicateTemplate()
		{
			string name = Templates.GetNewName((Template?.Name ?? strings.NewTemplate) + " - " + strings.Copy);
			if (Templates.Where((Template e) => e.Name == name).Count() == 0)
			{
				Template t;
				Templates.Add(t = TemplateFactory.CreateTemplate(name, Template?.BuildCode, Template?.GearCode));
				t.RequestSave("DuplicateTemplate");
			}
			else
			{
				ScreenNotification.ShowNotification(string.Format(strings.TemplateExistsAlready, name));
			}
		}

		private void SetTooltip()
		{
			string txt = strings.CopyBuildTemplateCode;
			foreach (Control c in base.Children)
			{
				if (c != _editButton)
				{
					c.BasicTooltipText = txt;
				}
			}
			base.BasicTooltipText = txt;
		}

		private void Template_LastModifiedChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<string> e)
		{
			SetLastModifiedText(e.NewValue);
		}

		private void SetLastModifiedText(string? date)
		{
			string date2 = date;
			_lastModified.SetLocalizedText = () => string.Format(strings.LastModified, date2 ?? string.Empty);
		}

		private void Tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			SetTagTextures();
			RecalculateLayout();
		}

		private void Template_EliteSpecializationChanged(object sender, SpecializationChangedEventArgs e)
		{
			ApplyTemplate();
		}

		private void Template_ProfessionChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<ProfessionType> e)
		{
			ApplyTemplate();
		}

		private void Template_LoadedBuildFromCode(object sender, EventArgs e)
		{
			ApplyTemplate();
		}

		private void Template_RaceChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Races> e)
		{
			_raceTexture = (Data.Races.TryGetValue(Template?.Race ?? Races.None, out var race) ? TexturesService.GetTextureFromRef(race.IconPath) : null);
		}

		public void ToggleEditMode(bool enable)
		{
			if (enable)
			{
				_nameEdit.Text = _name.Text;
				_nameEdit.Focused = true;
				_nameEdit.SelectionStart = 0;
				_nameEdit.SelectionEnd = _nameEdit.Text.Length;
			}
			_name.Text = Template?.Name ?? "No Name";
			_nameEdit.Visible = enable;
			_name.Visible = !enable;
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			string txt = null;
			spriteBatch.DrawOnCtrl(this, _textureVignette, _vignetteBounds, _textureVignette.Bounds, Color.Black, 0f, Vector2.Zero);
			if ((Template?.Profession).HasValue)
			{
				AsyncTexture2D prof = _specTexture;
				if (prof != null)
				{
					spriteBatch.DrawOnCtrl(this, prof, _specBounds, prof.Bounds, Color.White, 0f, Vector2.Zero);
				}
				if (_raceTexture != null)
				{
					spriteBatch.DrawOnCtrl(this, _raceTexture, _raceBounds, _raceTexture.Bounds, Color.White, 0f, Vector2.Zero);
				}
			}
			int amount = 0;
			for (int i = 0; i < _tagTextures.Count; i++)
			{
				TagTexture tagTexture = _tagTextures[i];
				if (_tagBounds.Contains(tagTexture.Bounds))
				{
					if (_tagTextures.Count - amount > 1 && !_tagBounds.Contains(_tagTextures[i + 1].Bounds))
					{
						spriteBatch.DrawStringOnCtrl(this, $"+{_tagTextures.Count - amount}", Control.Content.DefaultFont14, tagTexture.Bounds, ContentService.Colors.OldLace, wrap: false, HorizontalAlignment.Center);
						if (tagTexture.Bounds.Contains(base.RelativeMousePosition))
						{
							txt = string.Join(Environment.NewLine, from e in _tagTextures.Skip(amount).Take(_tagTextures.Count - amount)
								select e.Tag.Name);
						}
						break;
					}
					tagTexture.Draw(this, spriteBatch, base.RelativeMousePosition);
					if (tagTexture.Hovered)
					{
						txt = tagTexture.Tag.Name;
					}
					amount++;
					continue;
				}
				spriteBatch.DrawStringOnCtrl(this, $"+{_tagTextures.Count - amount}", Control.Content.DefaultFont14, tagTexture.Bounds, ContentService.Colors.OldLace);
				break;
			}
			base.BasicTooltipText = txt;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			Texture2D pixel = ContentService.Textures.Pixel;
			Rectangle destinationRectangle = base.ContentRegion.Add(0, 0, 0, -28);
			Rectangle? sourceRectangle = Rectangle.Empty;
			Template template = Template;
			spriteBatch.DrawOnCtrl(this, pixel, destinationRectangle, sourceRectangle, (template != null) ? (template.Profession.GetWikiColor() * 0.3f) : Color.Transparent);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _vignetteBounds, Rectangle.Empty, Color.Black * 0.15f);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(base.ContentRegion.X, base.ContentRegion.Bottom - 28, base.ContentRegion.Width, 28), Rectangle.Empty, Color.Black * 0.3f);
			base.PaintBeforeChildren(spriteBatch, bounds);
			bool isActive = BaseModule<BuildsManager, MainWindow, Settings, Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.ModuleInstance.SelectedTemplate == Template;
			spriteBatch.DrawFrame(this, bounds, isActive ? ContentService.Colors.ColonialWhite : Color.Transparent, 2);
			spriteBatch.DrawOnCtrl(this, _textureBottomSectionSeparator, _separatorBounds, _textureBottomSectionSeparator.Bounds, Color.Black, 0f, Vector2.Zero);
			spriteBatch.DrawOnCtrl(this, _lineTexture, _leftAccentBorderBounds, _lineTexture.Bounds, Color.Black * 0.6f, 0f, Vector2.Zero);
			spriteBatch.DrawOnCtrl(this, _lineTexture, _rightAccentBorderBounds, _lineTexture.Bounds, Color.Black * 0.6f, 0f, Vector2.Zero, SpriteEffects.FlipVertically);
			spriteBatch.DrawOnCtrl(this, _textureCornerButton, _raceAndSpecBounds, _textureCornerButton.Bounds, Color.Black, 0f, Vector2.Zero);
			spriteBatch.DrawOnCtrl(this, _textureCornerButton, _editBounds, _textureCornerButton.Bounds, Color.Black, 0f, Vector2.Zero);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			if (!_created)
			{
				return;
			}
			Rectangle contentBounds = new Rectangle(base.ContentRegion.Left + 2, base.ContentRegion.Top + 2, base.ContentRegion.Width - 4, base.ContentRegion.Height - 4);
			_vignetteBounds = new Rectangle(contentBounds.Left, contentBounds.Top, 55, 55);
			_name?.SetLocation(new Point(_vignetteBounds.Right + 5, contentBounds.Top + 2));
			_name?.SetSize(new Point(contentBounds.Width - _vignetteBounds.Width - 5, _vignetteBounds.Height - _lastModifiedFont.LineHeight - _name.Top));
			_nameEdit?.SetLocation(_name.Location);
			_nameEdit?.SetSize(_name.Size);
			_lastModified?.SetLocation(new Point(_name.Left, _name.Bottom));
			_lastModified?.SetSize(new Point(contentBounds.Width - _vignetteBounds.Width - 5, _lastModifiedFont.LineHeight));
			_separatorBounds = new Rectangle(2, _vignetteBounds.Bottom - 2, contentBounds.Width, 8);
			_bottomBounds = new Rectangle(_separatorBounds.Left, _vignetteBounds.Bottom + 1, _separatorBounds.Width, base.ContentRegion.Height - 4 - _vignetteBounds.Bottom);
			_editBounds = new Rectangle(_bottomBounds.Right - _bottomBounds.Height, _bottomBounds.Top, _bottomBounds.Height, _bottomBounds.Height);
			_specBounds = _vignetteBounds.Add(2, 2, -4, -4);
			_raceBounds = new Rectangle(_bottomBounds.Location.Add(new Point(1)), new Point(_bottomBounds.Height - 2));
			_raceAndSpecBounds = new Rectangle(_bottomBounds.Left, _bottomBounds.Top, _raceBounds.Right - _specBounds.Left + 5, _bottomBounds.Height);
			_leftAccentBorderBounds = new Rectangle(_raceAndSpecBounds.Right - 8, _bottomBounds.Top, 16, _bottomBounds.Height + 3);
			_rightAccentBorderBounds = new Rectangle(_editBounds.Left - 8, _bottomBounds.Top, 16, _bottomBounds.Height + 3);
			_editButton?.SetLocation(_editBounds.Location.Add(new Point(2)));
			_editButton?.SetSize(_editBounds.Size.Add(new Point(-4)));
			_tagBounds = new Rectangle(_leftAccentBorderBounds.Right - 6, _bottomBounds.Top, _rightAccentBorderBounds.Left - (_leftAccentBorderBounds.Right - 10), _bottomBounds.Height);
			for (int i = 0; i < _tagTextures.Count; i++)
			{
				TagTexture tagTexture = _tagTextures[i];
				if (tagTexture?.Tag != null)
				{
					tagTexture.Bounds = new Rectangle(_leftAccentBorderBounds.Right - 6 + i * (_bottomBounds.Height + 3), _bottomBounds.Top, _bottomBounds.Height, _bottomBounds.Height);
				}
			}
		}

		protected override async void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (_name.MouseOver && e.IsDoubleClick)
			{
				ToggleEditMode(enable: true);
			}
			else if (Control.Input.Keyboard.KeysDown.Contains(Keys.LeftControl))
			{
				try
				{
					SetNotification("Build Code copied!", Color.LimeGreen, 350.0);
					string s = Template?.BuildCode;
					if (s != null && !string.IsNullOrEmpty(s))
					{
						await ClipboardUtil.WindowsClipboardService.SetTextAsync(s);
					}
				}
				catch (Exception)
				{
				}
			}
			else
			{
				TemplatePresenter.SetTemplate(Template);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Control.Input.Mouse.LeftMouseButtonPressed -= Mouse_LeftMouseButtonPressed;
			TemplateTags.TagRemoved -= new EventHandler<TemplateTag>(TemplateTags_TagRemoved);
			TemplateTags.TagChanged -= new PropertyChangedEventHandler(TemplateTags_TagChanged);
			DisposeAction?.Invoke();
			_lineTexture?.Dispose();
			_textureVignette?.Dispose();
			_textureCornerButton?.Dispose();
			_textureBottomSectionSeparator?.Dispose();
			_tagTextures?.DisposeAll();
			_tagTextures?.Clear();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (!_animationRunning)
			{
				return;
			}
			if (gameTime.TotalGameTime.TotalMilliseconds - _animationStart < _animationDuration)
			{
				double timepast = gameTime.TotalGameTime.TotalMilliseconds - _animationStart;
				if (timepast > 5.0)
				{
					_name.Opacity = 1f - (float)(timepast / 5.0 * (double)_animationOpacityStep);
				}
			}
			else
			{
				SetName();
			}
		}

		private void SetNotification(string v, Color color, double duration = 1500.0)
		{
			_animationDuration = duration;
			_animationStart = Common.Now;
			_animationOpacityStep = (float)(1.0 / (_animationDuration / 5.0));
			_name.TextColor = color;
			_name.WrapText = false;
			_name.Font = _notificationFont;
			_name.HorizontalAlignment = HorizontalAlignment.Left;
			_name.Text = v;
			_animationRunning = true;
		}

		private void SetName()
		{
			_name.TextColor = Color.White;
			_name.Font = _nameFont;
			_name.WrapText = true;
			_name.HorizontalAlignment = HorizontalAlignment.Left;
			_name.Opacity = 1f;
			_name.Text = Template.Name;
			_animationRunning = false;
		}

		private void ApplyTemplate()
		{
			_name.Text = Template?.Name;
			_raceTexture = (Data.Races.TryGetValue(Template?.Race ?? Races.None, out var race) ? TexturesService.GetTextureFromRef(race.IconPath) : null);
			_specTexture = ((Template != null) ? TexturesService.GetAsyncTexture(Template?.EliteSpecialization?.ProfessionIconBigAssetId ?? (Data.Professions.TryGetValue((Template?.Profession).Value, out var profession) ? new int?(profession.IconBigAssetId) : null)) : null);
			SetLastModifiedText(Template?.LastModified);
			SetTagTextures();
			if (Template != null)
			{
				RecalculateLayout();
			}
		}

		private void SetTagTextures()
		{
			_tagTextures.Clear();
			if (Template == null)
			{
				return;
			}
			Point s = new Point(20);
			Rectangle r = new Rectangle(_tagBounds.X, _tagBounds.Y, s.X, s.Y);
			foreach (string t in Template.Tags)
			{
				TemplateTag tag = TemplateTags.FirstOrDefault((TemplateTag x) => x.Name == t);
				if (tag != null)
				{
					List<TagTexture> tagTextures = _tagTextures;
					TagTexture obj = new TagTexture(tag.Icon.Texture)
					{
						Tag = tag
					};
					r = (obj.Bounds = r.Add(s.X, 0, 0, 0));
					obj.TextureRegion = tag.TextureRegion ?? Rectangle.Empty;
					tagTextures.Add(obj);
				}
			}
		}

		private void Mouse_LeftMouseButtonPressed(object sender, MouseEventArgs e)
		{
			if (!_editButton.MouseOver && !_nameEdit.MouseOver && _nameEdit.Visible)
			{
				ToggleEditMode(enable: false);
				SetName();
			}
		}

		private void TemplateChanged(object sender, PropertyChangedEventArgs e)
		{
			ApplyTemplate();
		}
	}
}
