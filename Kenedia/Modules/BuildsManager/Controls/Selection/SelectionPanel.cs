using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class SelectionPanel : Container
	{
		private readonly GearSelection _gearSelection;

		private readonly StatSelection _statSelection;

		private readonly DetailedTexture _backButton = new DetailedTexture(784268);

		private Control? _subAnchor;

		private Control? _mainAnchor;

		private Rectangle _backBounds;

		private Rectangle _backTextBounds;

		public BuildSelection BuildSelection { get; }

		public TemplatePresenter TemplatePresenter { get; private set; }

		public Data Data { get; }

		public Settings Settings { get; }

		public MainWindow MainWindow { get; set; }

		public string Title { get; set; }

		public SelectionTypes SelectionType
		{
			[CompilerGenerated]
			get
			{
				return _003CSelectionType_003Ek__BackingField;
			}
			set
			{
				if (Common.SetProperty(ref _003CSelectionType_003Ek__BackingField, value))
				{
					_gearSelection.Visible = _003CSelectionType_003Ek__BackingField == SelectionTypes.Items;
					BuildSelection.Visible = _003CSelectionType_003Ek__BackingField == SelectionTypes.Templates;
					_statSelection.Visible = _003CSelectionType_003Ek__BackingField == SelectionTypes.Stats;
				}
			}
		}

		public Control Anchor
		{
			[CompilerGenerated]
			get
			{
				return _003CAnchor_003Ek__BackingField;
			}
			private set
			{
				if (SelectionType == SelectionTypes.Templates)
				{
					_mainAnchor = value;
				}
				else
				{
					_subAnchor = value;
				}
				Pointer.Anchor = (_003CAnchor_003Ek__BackingField = ((SelectionType == SelectionTypes.Templates) ? _mainAnchor : _subAnchor));
			}
		}

		public GearSubSlotType SubSlotType { get; private set; }

		public Pointer Pointer { get; set; }

		public SelectionPanel(TemplatePresenter templatePresenter, TemplateCollection templates, TemplateTags templateTags, Data data, TemplateFactory templateFactory, Settings settings)
		{
			_003CSelectionType_003Ek__BackingField = SelectionTypes.Templates;
			base._002Ector();
			TemplatePresenter = templatePresenter;
			Data = data;
			Settings = settings;
			Pointer = new Pointer();
			base.ClipsBounds = false;
			HeightSizingMode = SizingMode.Fill;
			base.Width = 375;
			_gearSelection = new GearSelection(TemplatePresenter, data)
			{
				Parent = this,
				Visible = false,
				ZIndex = ZIndex
			};
			BuildSelection = new BuildSelection(templates, templateTags, data, templatePresenter, templateFactory, settings)
			{
				Parent = this,
				Visible = true,
				SelectionPanel = this,
				ZIndex = ZIndex
			};
			_statSelection = new StatSelection(TemplatePresenter, data)
			{
				Parent = this,
				Visible = false,
				ZIndex = ZIndex
			};
			GameService.Gw2Mumble.PlayerCharacter.NameChanged += ApplyAutoFilter;
			ApplyAutoFilter();
			TemplatePresenter.TemplateChanged += new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
		}

		public void ApplyAutoFilter(object sender = null, EventArgs e = null)
		{
			SelectFirstTemplate();
		}

		private void TemplatePresenter_TemplateChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Template> e)
		{
			Kenedia.Modules.Core.Models.ValueChangedEventArgs<Template> e2 = e;
			SetTemplateAnchor(BuildSelection.TemplateSelectables.FirstOrDefault((TemplateSelectable x) => x.Template == e2.NewValue));
		}

		private void SetAnchor(Control anchor, Rectangle? anchorBounds = null)
		{
			Anchor = anchor;
		}

		public void SetAnchor<T>(Control anchor, Rectangle anchorBounds, SelectionTypes selectionType, Enum slot, Enum subslot, Action<T> onClickAction, IReadOnlyList<int> statChoices = null, double? attributeAdjustment = null, string? title = null) where T : class
		{
			Action<T> onClickAction2 = onClickAction;
			Anchor = anchor;
			SelectionType = selectionType;
			Title = title ?? SelectionType.ToString();
			if (Anchor == null)
			{
				return;
			}
			switch (SelectionType)
			{
			case SelectionTypes.Items:
			{
				if (!Data.IsLoaded)
				{
					break;
				}
				if (((TemplateSlotType)(object)slot).GetGroupType() != _gearSelection.ActiveSlot.GetGroupType())
				{
					_gearSelection.Search.Text = string.Empty;
				}
				else if (subslot != null && (GearSubSlotType)(object)subslot != _gearSelection.SubSlotType)
				{
					_gearSelection.Search.Text = string.Empty;
				}
				_gearSelection.ActiveSlot = (TemplateSlotType)(object)slot;
				GearSubSlotType gearSubSlotType3 = (_gearSelection.SubSlotType = (SubSlotType = (GearSubSlotType)(object)subslot));
				_gearSelection.OnClickAction = delegate(object obj)
				{
					T val = obj as T;
					if (val != null)
					{
						onClickAction2(val);
					}
				};
				break;
			}
			case SelectionTypes.Stats:
				if (!Data.IsLoaded)
				{
					break;
				}
				_statSelection.OnClickAction = delegate(object obj)
				{
					T val2 = obj as T;
					if (val2 != null)
					{
						onClickAction2(val2);
					}
				};
				_statSelection.AttributeAdjustments = attributeAdjustment.GetValueOrDefault();
				_statSelection.StatChoices = statChoices;
				break;
			case SelectionTypes.Templates:
				BuildSelection.OnClickAction = delegate(object obj)
				{
					T val3 = obj as T;
					if (val3 != null)
					{
						onClickAction2(val3);
					}
				};
				break;
			}
		}

		public void SelectFirstTemplate()
		{
			TemplateSelectable selectable = BuildSelection?.GetFirstTemplateSelectable();
			if (selectable != null)
			{
				SetTemplateAnchor(selectable);
			}
			else
			{
				ResetAnchor();
			}
		}

		public void SetTemplateAnchor(Control anchor)
		{
			SelectionType = SelectionTypes.Templates;
			SetAnchor(anchor);
		}

		public void ResetAnchor()
		{
			SelectionType = SelectionTypes.Templates;
			SetAnchor(BuildSelection.TemplateSelectables.FirstOrDefault((TemplateSelectable e) => e.Template == TemplatePresenter.Template));
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			_backBounds = new Rectangle(10, 5, base.Width - 32, 55);
			_backButton.Bounds = new Rectangle(_backBounds.Left + 10, _backBounds.Top + 10, _backBounds.Height - 20, _backBounds.Height - 20);
			_backTextBounds = new Rectangle(_backButton.Bounds.Right + 10, _backBounds.Top + 10, _backBounds.Width - (_backButton.Bounds.Right + 10), _backBounds.Height - 20);
			if (_gearSelection != null)
			{
				_gearSelection.Location = new Point(10, _backBounds.Bottom + 10);
			}
			if (_statSelection != null)
			{
				_statSelection.Location = new Point(10, _backBounds.Bottom + 10);
			}
			if (BuildSelection != null)
			{
				BuildSelection.Location = new Point(10, 10);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			if (Anchor != null && Anchor.Visible && Anchor.Parent != null && Anchor.Parent.AbsoluteBounds.Contains(Anchor.AbsoluteBounds.Center))
			{
				if (SelectionType == SelectionTypes.Items)
				{
					DrawGearSelection(spriteBatch, bounds);
				}
				else if (SelectionType == SelectionTypes.Stats)
				{
					DrawStatSelection(spriteBatch, bounds);
				}
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (Anchor != null)
			{
				SelectionTypes selectionType = SelectionType;
				bool flag = (((uint)(selectionType - 2) <= 1u) ? true : false);
				if (flag && _backBounds.Contains(base.RelativeMousePosition))
				{
					ResetAnchor();
				}
			}
		}

		private void DrawGearSelection(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (_backBounds.Contains(base.RelativeMousePosition))
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _backBounds, ContentService.Colors.ColonialWhite * 0.3f);
			}
			_backButton.Draw(this, spriteBatch, base.RelativeMousePosition, Color.White);
			spriteBatch.DrawStringOnCtrl(this, Title, Control.Content.DefaultFont18, _backTextBounds, Color.White);
		}

		private void DrawStatSelection(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (_backBounds.Contains(base.RelativeMousePosition))
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _backBounds, ContentService.Colors.ColonialWhite * 0.3f);
			}
			_backButton.Draw(this, spriteBatch, base.RelativeMousePosition, Color.White);
			spriteBatch.DrawStringOnCtrl(this, Title, Control.Content.DefaultFont18, _backTextBounds, Color.White);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			Pointer?.Dispose();
			_backButton?.Dispose();
		}
	}
}
