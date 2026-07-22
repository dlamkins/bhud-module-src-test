using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterRoutineStepRow : Kenedia.Modules.Core.Controls.Panel
	{
		private const int HandleWidth = 14;

		private const int ButtonSize = 24;

		private const int ButtonSpacing = 6;

		private static readonly Color BackgroundDefault = new Color(40, 40, 40, 150);

		private static readonly Color BackgroundCompleted = new Color(40, 80, 40, 150);

		private static readonly Color BackgroundEditing = new Color(50, 50, 70, 180);

		private static readonly Color BackgroundTracked = new Color(80, 70, 30, 170);

		private readonly CharacterRoutineService _service;

		private readonly ObservableCollection<Character_Model> _characterModels;

		private readonly CharacterRoutineStep _step;

		private readonly Action<CharacterRoutineStepRow> _onDragStartRequested;

		private readonly Kenedia.Modules.Core.Controls.Checkbox _completionCheckbox;

		private readonly Kenedia.Modules.Core.Controls.Checkbox _enabledCheckbox;

		private readonly Kenedia.Modules.Core.Controls.Label _nameLabel;

		private readonly Kenedia.Modules.Core.Controls.Label _descriptionLabel;

		private readonly ImageButton _switchButton;

		private readonly ImageButton _editButton;

		private readonly ImageButton _removeButton;

		private readonly AutoSuggestComboBox<Character_Model> _editCharacterSuggestionBox;

		private readonly Blish_HUD.Controls.TextBox _editDescriptionBox;

		private readonly ImageButton _saveButton;

		private readonly ImageButton _cancelButton;

		private bool _isEditing;

		private bool _hasAppliedModeVisibility;

		private bool _handleHovered;

		private bool _syncingCheckboxes;

		public CharacterRoutineStep Step => _step;

		public bool IsDragging { get; set; }

		public CharacterRoutineStepRow(CharacterRoutineService service, ObservableCollection<Character_Model> characterModels, CharacterRoutineStep step, Action<CharacterRoutineStepRow> onDragStartRequested)
		{
			_service = service;
			_characterModels = characterModels;
			_step = step;
			_onDragStartRequested = onDragStartRequested;
			base.Height = 32;
			_step.PropertyChanged += new PropertyChangedEventHandler(Step_PropertyChanged);
			_service.State.TrackedStep.Changed += new EventHandler<StateVarChangedEventArgs<CharacterRoutineStep>>(TrackedStep_Changed);
			_completionCheckbox = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = this,
				Checked = _step.IsCompleted,
				Location = new Point(18, 8),
				Width = 20,
				CheckedChangedAction = delegate(bool completed)
				{
					if (!_syncingCheckboxes)
					{
						_service.SetRoutineStepCompletion(_step, completed);
					}
				}
			};
			_enabledCheckbox = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = this,
				Checked = _step.Enabled,
				Location = new Point(14, 8),
				Width = 20,
				Visible = false,
				CheckedChangedAction = delegate(bool enabled)
				{
					if (!_syncingCheckboxes)
					{
						_service.SetRoutineStepEnabled(_step, enabled);
					}
				}
			};
			int labelX = 42;
			int descX = labelX + 165;
			_nameLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Location = new Point(labelX, 0),
				Width = 160,
				Height = 32,
				VerticalAlignment = VerticalAlignment.Middle,
				Font = Control.Content.DefaultFont14,
				TextColor = ContentService.Colors.ColonialWhite
			};
			_descriptionLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Location = new Point(descX, 0),
				Width = 250,
				Height = 32,
				VerticalAlignment = VerticalAlignment.Middle,
				Font = Control.Content.DefaultFont12,
				TextColor = Color.LightGray
			};
			_switchButton = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(157092),
				ClickedTexture = AsyncTexture2D.FromAssetId(157093),
				HoveredTexture = AsyncTexture2D.FromAssetId(157094),
				Size = new Point(24, 24),
				BasicTooltipText = string.Format(strings.Switch, strings.Character),
				ClickAction = delegate
				{
					_service.RequestSwitchToCharacter(_step.CharacterName);
				}
			};
			_editButton = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(2175779),
				Size = new Point(24, 24),
				BasicTooltipText = strings.EditEntry,
				ClickAction = delegate
				{
					SetEditMode(editing: true);
				}
			};
			_removeButton = new ImageButton
			{
				Parent = this,
				Texture = AsyncTexture2D.FromAssetId(2175783),
				HoveredTexture = AsyncTexture2D.FromAssetId(2175782),
				ClickedTexture = AsyncTexture2D.FromAssetId(2175784),
				Size = new Point(24, 24),
				BasicTooltipText = strings.RemoveRoutineStep,
				ClickAction = delegate
				{
					ConfirmRemoveRoutineStep();
				}
			};
			_editCharacterSuggestionBox = new AutoSuggestComboBox<Character_Model>
			{
				Parent = this,
				PlaceholderText = strings.SearchCharacterName,
				Location = new Point(labelX - 8, 2),
				Width = 155,
				Height = 28,
				MaxSuggestionHeight = 300,
				SelectableFactory = (Character_Model character) => new CharacterSelectable(_editCharacterSuggestionBox, character),
				Items = characterModels,
				AllowBlankSelection = true,
				BlankSelectionText = strings.Unassigned
			};
			_editDescriptionBox = new Blish_HUD.Controls.TextBox
			{
				Parent = this,
				PlaceholderText = strings.RoutineStepDescriptionPlaceholder,
				Location = new Point(descX - 8, 2),
				Width = 326,
				Height = 28
			};
			_saveButton = new ButtonImage
			{
				Parent = this,
				BasicTooltipText = strings.Save,
				Texture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Save, "Save"),
				HoveredTexture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Save_Hovered, "Save_Hovered"),
				ClickedTexture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Save_Active, "Save_Active"),
				Size = new Point(24, 24),
				ClickAction = delegate
				{
					_service.UpdateRoutineStep(_step, _editCharacterSuggestionBox.Selected?.Name, _editDescriptionBox.Text);
					SetEditMode(editing: false);
				}
			};
			_cancelButton = new ButtonImage
			{
				Parent = this,
				BasicTooltipText = strings.Cancel,
				Texture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Cancel, "Cancel"),
				HoveredTexture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Cancel_Hovered, "Cancel_Hovered"),
				ClickedTexture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Cancel_Active, "Cancel_Active"),
				Size = new Point(24, 24),
				ClickAction = delegate
				{
					SetEditMode(editing: false);
					RefreshFromStep();
				}
			};
			RefreshFromStep();
			SetEditMode(editing: false);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			base.OnResized(e);
			int x = base.Width - 5;
			ImageButton removeButton = _removeButton;
			if (removeButton != null)
			{
				removeButton.Location = new Point(x -= 24, (base.Height - 24) / 2);
			}
			ImageButton editButton = _editButton;
			if (editButton != null)
			{
				editButton.Location = new Point(x -= 30, (base.Height - 24) / 2);
			}
			ImageButton switchButton = _switchButton;
			if (switchButton != null)
			{
				switchButton.Location = new Point(x -= 30, (base.Height - 24) / 2);
			}
			ImageButton saveButton = _saveButton;
			if (saveButton != null)
			{
				saveButton.Location = _editButton?.Location ?? Point.Zero;
			}
			ImageButton cancelButton = _cancelButton;
			if (cancelButton != null)
			{
				cancelButton.Location = _removeButton?.Location ?? Point.Zero;
			}
			Kenedia.Modules.Core.Controls.Label descriptionLabel = _descriptionLabel;
			if (descriptionLabel != null)
			{
				descriptionLabel.Size = new Point(Math.Max(0, _switchButton?.Left ?? (-_descriptionLabel.Left - 12)), _descriptionLabel.Height);
			}
			Blish_HUD.Controls.TextBox editDescriptionBox = _editDescriptionBox;
			if (editDescriptionBox != null)
			{
				editDescriptionBox.Size = new Point(Math.Max(0, (_switchButton?.Right ?? 0) - _editDescriptionBox.Left), _editDescriptionBox.Height);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
			if (IsDragging)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Rectangle.Empty, Color.Black * 0.3f);
			}
			if (!_isEditing)
			{
				DrawDragHandle(spriteBatch);
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			base.OnMouseMoved(e);
			_handleHovered = !_isEditing && base.RelativeMousePosition.X < 14;
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			base.OnMouseLeft(e);
			_handleHovered = false;
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			base.OnLeftMouseButtonPressed(e);
			if (!_isEditing && base.RelativeMousePosition.X < 14)
			{
				_onDragStartRequested?.Invoke(this);
			}
		}

		protected override void DisposeControl()
		{
			_step.PropertyChanged -= new PropertyChangedEventHandler(Step_PropertyChanged);
			_service.State.TrackedStep.Changed -= new EventHandler<StateVarChangedEventArgs<CharacterRoutineStep>>(TrackedStep_Changed);
			base.DisposeControl();
		}

		private async void ConfirmRemoveRoutineStep()
		{
			string stepName = ((!string.IsNullOrWhiteSpace(_step.CharacterName)) ? _step.CharacterName : ((!string.IsNullOrWhiteSpace(_step.Description)) ? _step.Description : strings.Unassigned));
			if (await new BaseDialog(strings.DeleteConfirmationTitle, string.Format(strings.ConfirmCharacterRoutineStepDelete, stepName))
			{
				DesiredWidth = 360,
				AutoSize = true
			}.ShowDialog() == DialogResult.OK)
			{
				_service.RemoveRoutineStep(_step);
			}
		}

		private void DrawDragHandle(SpriteBatch spriteBatch)
		{
			Color color = (_handleHovered ? new Color(200, 200, 200) : new Color(100, 100, 100));
			int totalWidth = 7;
			int totalHeight = 12;
			int startX = (14 - totalWidth) / 2;
			int startY = (base.Height - totalHeight) / 2;
			for (int row = 0; row < 3; row++)
			{
				for (int col = 0; col < 2; col++)
				{
					int x = startX + col * 5;
					int y = startY + row * 5;
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(x, y, 2, 2), Rectangle.Empty, color);
				}
			}
		}

		private void SetEditMode(bool editing)
		{
			if (_isEditing == editing && _hasAppliedModeVisibility)
			{
				return;
			}
			_isEditing = editing;
			if (_isEditing)
			{
				_editCharacterSuggestionBox.Selected = _characterModels.FirstOrDefault((Character_Model c) => c.Name.Equals(_step.CharacterName, StringComparison.OrdinalIgnoreCase));
				_editCharacterSuggestionBox.Text = _editCharacterSuggestionBox.Selected?.Name ?? _step.CharacterName ?? string.Empty;
				_editDescriptionBox.Text = _step.Description ?? string.Empty;
			}
			_completionCheckbox.Visible = !_isEditing;
			_nameLabel.Visible = !_isEditing;
			_descriptionLabel.Visible = !_isEditing && !string.IsNullOrEmpty(_step.Description);
			_switchButton.Visible = !_isEditing;
			_editButton.Visible = !_isEditing;
			_removeButton.Visible = !_isEditing;
			_enabledCheckbox.Visible = _isEditing;
			_editCharacterSuggestionBox.Visible = _isEditing;
			_editDescriptionBox.Visible = _isEditing;
			_saveButton.Visible = _isEditing;
			_cancelButton.Visible = _isEditing;
			if (_isEditing)
			{
				_handleHovered = false;
			}
			_hasAppliedModeVisibility = true;
			RefreshVisualState();
		}

		private void RefreshFromStep()
		{
			_syncingCheckboxes = true;
			_completionCheckbox.Checked = _step.IsCompleted;
			_enabledCheckbox.Checked = _step.Enabled;
			_syncingCheckboxes = false;
			_nameLabel.Text = _step.CharacterName ?? string.Empty;
			_descriptionLabel.Text = _step.Description ?? string.Empty;
			_descriptionLabel.Visible = !_isEditing && !string.IsNullOrEmpty(_step.Description);
			if (_isEditing)
			{
				_editCharacterSuggestionBox.Selected = _characterModels.FirstOrDefault((Character_Model c) => c.Name.Equals(_step.CharacterName, StringComparison.OrdinalIgnoreCase));
				_editCharacterSuggestionBox.Text = _editCharacterSuggestionBox.Selected?.Name ?? _step.CharacterName ?? string.Empty;
				_editDescriptionBox.Text = _step.Description ?? string.Empty;
			}
			RefreshVisualState();
		}

		private Color ResolveBackgroundColor()
		{
			bool isTracked = !_isEditing && !_step.IsCompleted && _service.GetTrackedStepForSelectedRoutine() == _step;
			if (!_isEditing)
			{
				if (!_step.IsCompleted)
				{
					if (!isTracked)
					{
						return BackgroundDefault;
					}
					return BackgroundTracked;
				}
				return BackgroundCompleted;
			}
			return BackgroundEditing;
		}

		private void RefreshVisualState()
		{
			base.BackgroundColor = ResolveBackgroundColor();
		}

		private void Step_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "CharacterName" || e.PropertyName == "Description" || e.PropertyName == "Completed" || e.PropertyName == "Enabled" || string.IsNullOrEmpty(e.PropertyName))
			{
				RefreshFromStep();
			}
		}

		private void TrackedStep_Changed(object sender, StateVarChangedEventArgs<CharacterRoutineStep> e)
		{
			if (e.NewValue == _step || e.OldValue == _step)
			{
				RefreshVisualState();
			}
		}
	}
}
