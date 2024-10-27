using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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

		private Template _template;

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
			get
			{
				return _template;
			}
			set
			{
				Template temp = _template;
				if (Common.SetProperty(ref _template, value, new Action(ApplyTemplate)))
				{
					if (temp != null)
					{
						temp.RaceChanged -= new ValueChangedEventHandler<Races>(Template_RaceChanged);
					}
					if (_template != null)
					{
						_template.RaceChanged += new ValueChangedEventHandler<Races>(Template_RaceChanged);
					}
					if (temp != null)
					{
						temp.ProfessionChanged -= new ValueChangedEventHandler<ProfessionType>(Template_ProfessionChanged);
					}
					if (_template != null)
					{
						_template.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(Template_ProfessionChanged);
					}
					if (temp != null)
					{
						temp.EliteSpecializationChanged -= new SpecializationChangedEventHandler(Template_EliteSpecializationChanged);
					}
					if (_template != null)
					{
						_template.EliteSpecializationChanged += new SpecializationChangedEventHandler(Template_EliteSpecializationChanged);
					}
					if (temp != null)
					{
						temp.Tags.CollectionChanged -= Tags_CollectionChanged;
					}
					if (_template != null)
					{
						_template.Tags.CollectionChanged += Tags_CollectionChanged;
					}
					if (temp != null)
					{
						temp.LastModifiedChanged -= new ValueChangedEventHandler<string>(Template_LastModifiedChanged);
					}
					if (_template != null)
					{
						_template.LastModifiedChanged += new ValueChangedEventHandler<string>(Template_LastModifiedChanged);
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
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			Data = data;
			Templates = templates;
			TemplateTags = templateTags;
			TemplateFactory = templateFactory;
			TemplatePresenter = templatePresenter;
			base.Height = 85;
			base.BorderWidth = new RectangleDimensions(3);
			base.BorderColor = Color.get_Black();
			_name = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = _nameFont.get_LineHeight(),
				Font = _nameFont,
				WrapText = true,
				VerticalAlignment = VerticalAlignment.Middle
			};
			_lastModified = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = _lastModifiedFont.get_LineHeight(),
				Font = _lastModifiedFont,
				TextColor = Color.get_White() * 0.7f,
				WrapText = false,
				VerticalAlignment = VerticalAlignment.Middle
			};
			_nameEdit = new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = this,
				Height = _nameFont.get_LineHeight(),
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
					//IL_0052: Unknown result type (might be due to invalid IL or missing references)
					//IL_0059: Unknown result type (might be due to invalid IL or missing references)
					string txt2 = txt;
					txt2 = txt2.Trim().ToLower();
					Template template = Templates.Where((Template e) => e.Name.ToLower() == txt2).FirstOrDefault();
					_nameEdit.ForeColor = ((template == null || template == Template) ? Color.get_White() : Color.get_Red());
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
			_raceTexture = (Data.Races.TryGetValue(_template?.Race ?? Races.None, out var race) ? race.Icon : null);
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			string txt = null;
			spriteBatch.DrawOnCtrl(this, _textureVignette, _vignetteBounds, _textureVignette.Bounds, Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			if ((Template?.Profession).HasValue)
			{
				AsyncTexture2D prof = _specTexture;
				if (prof != null)
				{
					spriteBatch.DrawOnCtrl(this, prof, _specBounds, prof.Bounds, Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
				}
				if (_raceTexture != null)
				{
					spriteBatch.DrawOnCtrl(this, _raceTexture, _raceBounds, _raceTexture.Bounds, Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
				}
			}
			int amount = 0;
			for (int i = 0; i < _tagTextures.Count; i++)
			{
				TagTexture tagTexture = _tagTextures[i];
				if (((Rectangle)(ref _tagBounds)).Contains(tagTexture.Bounds))
				{
					if (_tagTextures.Count - amount > 1 && !((Rectangle)(ref _tagBounds)).Contains(_tagTextures[i + 1].Bounds))
					{
						spriteBatch.DrawStringOnCtrl(this, $"+{_tagTextures.Count - amount}", Control.Content.DefaultFont14, tagTexture.Bounds, ContentService.Colors.OldLace, wrap: false, HorizontalAlignment.Center);
						Rectangle bounds2 = tagTexture.Bounds;
						if (((Rectangle)(ref bounds2)).Contains(base.RelativeMousePosition))
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
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			Texture2D pixel = ContentService.Textures.Pixel;
			Rectangle destinationRectangle = base.ContentRegion.Add(0, 0, 0, -28);
			Rectangle? sourceRectangle = Rectangle.get_Empty();
			Template template = Template;
			spriteBatch.DrawOnCtrl(this, pixel, destinationRectangle, sourceRectangle, (template != null) ? (template.Profession.GetWikiColor() * 0.3f) : Color.get_Transparent());
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _vignetteBounds, Rectangle.get_Empty(), Color.get_Black() * 0.15f);
			Texture2D pixel2 = ContentService.Textures.Pixel;
			int x = base.ContentRegion.X;
			Rectangle contentRegion = base.ContentRegion;
			spriteBatch.DrawOnCtrl(this, pixel2, new Rectangle(x, ((Rectangle)(ref contentRegion)).get_Bottom() - 28, base.ContentRegion.Width, 28), Rectangle.get_Empty(), Color.get_Black() * 0.3f);
			base.PaintBeforeChildren(spriteBatch, bounds);
			bool isActive = BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.SelectedTemplate == _template;
			spriteBatch.DrawFrame(this, bounds, isActive ? ContentService.Colors.ColonialWhite : Color.get_Transparent(), 2);
			spriteBatch.DrawOnCtrl(this, _textureBottomSectionSeparator, _separatorBounds, _textureBottomSectionSeparator.Bounds, Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			spriteBatch.DrawOnCtrl(this, _lineTexture, _leftAccentBorderBounds, _lineTexture.Bounds, Color.get_Black() * 0.6f, 0f, Vector2.get_Zero(), (SpriteEffects)0);
			spriteBatch.DrawOnCtrl(this, _lineTexture, _rightAccentBorderBounds, _lineTexture.Bounds, Color.get_Black() * 0.6f, 0f, Vector2.get_Zero(), (SpriteEffects)2);
			spriteBatch.DrawOnCtrl(this, _textureCornerButton, _raceAndSpecBounds, _textureCornerButton.Bounds, Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			spriteBatch.DrawOnCtrl(this, _textureCornerButton, _editBounds, _textureCornerButton.Bounds, Color.get_Black(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
		}

		public override void RecalculateLayout()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (!_created)
			{
				return;
			}
			Rectangle contentRegion = base.ContentRegion;
			int num = ((Rectangle)(ref contentRegion)).get_Left() + 2;
			contentRegion = base.ContentRegion;
			Rectangle contentBounds = default(Rectangle);
			((Rectangle)(ref contentBounds))._002Ector(num, ((Rectangle)(ref contentRegion)).get_Top() + 2, base.ContentRegion.Width - 4, base.ContentRegion.Height - 4);
			_vignetteBounds = new Rectangle(((Rectangle)(ref contentBounds)).get_Left(), ((Rectangle)(ref contentBounds)).get_Top(), 55, 55);
			_name?.SetLocation(new Point(((Rectangle)(ref _vignetteBounds)).get_Right() + 5, ((Rectangle)(ref contentBounds)).get_Top() + 2));
			_name?.SetSize(new Point(contentBounds.Width - _vignetteBounds.Width - 5, _vignetteBounds.Height - _lastModifiedFont.get_LineHeight() - _name.Top));
			_nameEdit?.SetLocation(_name.Location);
			_nameEdit?.SetSize(_name.Size);
			_lastModified?.SetLocation(new Point(_name.Left, _name.Bottom));
			_lastModified?.SetSize(new Point(contentBounds.Width - _vignetteBounds.Width - 5, _lastModifiedFont.get_LineHeight()));
			_separatorBounds = new Rectangle(2, ((Rectangle)(ref _vignetteBounds)).get_Bottom() - 2, contentBounds.Width, 8);
			_bottomBounds = new Rectangle(((Rectangle)(ref _separatorBounds)).get_Left(), ((Rectangle)(ref _vignetteBounds)).get_Bottom() + 1, _separatorBounds.Width, base.ContentRegion.Height - 4 - ((Rectangle)(ref _vignetteBounds)).get_Bottom());
			_editBounds = new Rectangle(((Rectangle)(ref _bottomBounds)).get_Right() - _bottomBounds.Height, ((Rectangle)(ref _bottomBounds)).get_Top(), _bottomBounds.Height, _bottomBounds.Height);
			_specBounds = _vignetteBounds.Add(2, 2, -4, -4);
			_raceBounds = new Rectangle(((Rectangle)(ref _bottomBounds)).get_Location().Add(new Point(1)), new Point(_bottomBounds.Height - 2));
			_raceAndSpecBounds = new Rectangle(((Rectangle)(ref _bottomBounds)).get_Left(), ((Rectangle)(ref _bottomBounds)).get_Top(), ((Rectangle)(ref _raceBounds)).get_Right() - ((Rectangle)(ref _specBounds)).get_Left() + 5, _bottomBounds.Height);
			_leftAccentBorderBounds = new Rectangle(((Rectangle)(ref _raceAndSpecBounds)).get_Right() - 8, ((Rectangle)(ref _bottomBounds)).get_Top(), 16, _bottomBounds.Height + 3);
			_rightAccentBorderBounds = new Rectangle(((Rectangle)(ref _editBounds)).get_Left() - 8, ((Rectangle)(ref _bottomBounds)).get_Top(), 16, _bottomBounds.Height + 3);
			_editButton?.SetLocation(((Rectangle)(ref _editBounds)).get_Location().Add(new Point(2)));
			_editButton?.SetSize(((Rectangle)(ref _editBounds)).get_Size().Add(new Point(-4)));
			_tagBounds = new Rectangle(((Rectangle)(ref _leftAccentBorderBounds)).get_Right() - 6, ((Rectangle)(ref _bottomBounds)).get_Top(), ((Rectangle)(ref _rightAccentBorderBounds)).get_Left() - (((Rectangle)(ref _leftAccentBorderBounds)).get_Right() - 10), _bottomBounds.Height);
			for (int i = 0; i < _tagTextures.Count; i++)
			{
				TagTexture tagTexture = _tagTextures[i];
				if (tagTexture?.Tag != null)
				{
					tagTexture.Bounds = new Rectangle(((Rectangle)(ref _leftAccentBorderBounds)).get_Right() - 6 + i * (_bottomBounds.Height + 3), ((Rectangle)(ref _bottomBounds)).get_Top(), _bottomBounds.Height, _bottomBounds.Height);
				}
			}
		}

		protected override async void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (Control.Input.Keyboard.KeysDown.Contains((Keys)162))
			{
				try
				{
					SetNotification("Build Code copied!", Color.get_LimeGreen(), 350.0);
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
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _animationStart < _animationDuration)
			{
				double timepast = gameTime.get_TotalGameTime().TotalMilliseconds - _animationStart;
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
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_name.TextColor = Color.get_White();
			_name.Font = _nameFont;
			_name.WrapText = true;
			_name.HorizontalAlignment = HorizontalAlignment.Left;
			_name.Opacity = 1f;
			_name.Text = _template.Name;
			_animationRunning = false;
		}

		private void ApplyTemplate()
		{
			_name.Text = Template?.Name;
			_raceTexture = (Data.Races.TryGetValue(Template?.Race ?? Races.None, out var race) ? race.Icon : null);
			_specTexture = ((Template == null) ? null : (Template?.EliteSpecialization?.ProfessionIconBig ?? (Data.Professions.TryGetValue((Template?.Profession).Value, out var profession) ? profession.IconBig : null)));
			SetLastModifiedText(Template.LastModified);
			SetTagTextures();
			if (Template != null)
			{
				RecalculateLayout();
			}
		}

		private void SetTagTextures()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			_tagTextures.Clear();
			if (_template == null)
			{
				return;
			}
			Point s = default(Point);
			((Point)(ref s))._002Ector(20);
			Rectangle r = default(Rectangle);
			((Rectangle)(ref r))._002Ector(_tagBounds.X, _tagBounds.Y, s.X, s.Y);
			foreach (string t in _template.Tags)
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
					obj.TextureRegion = (Rectangle)(((_003F?)tag.TextureRegion) ?? Rectangle.get_Empty());
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
