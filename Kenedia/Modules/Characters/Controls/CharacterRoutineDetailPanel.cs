using System;
using System.Collections.ObjectModel;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Services;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterRoutineDetailPanel : Kenedia.Modules.Core.Controls.FlowPanel
	{
		private readonly TextureManager _textureManager;

		private readonly CharacterRoutineService _service;

		private readonly Settings _settings;

		private readonly ObservableCollection<Character_Model> _characterModels;

		private readonly Kenedia.Modules.Core.Controls.Label _placeholderLabel;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _contentPanel;

		private Kenedia.Modules.Core.Controls.Label _nameLabel;

		private readonly Blish_HUD.Controls.TextBox _nameBox;

		private Kenedia.Modules.Core.Controls.Label _resetLabel;

		private readonly Kenedia.Modules.Core.Controls.Dropdown _resetDropdown;

		private readonly AutoSuggestComboBox<Character_Model> _characterSuggestionBox;

		private readonly CharacterRoutineStepsPanel _stepsPanel;

		private CharacterRoutineModel _boundList;

		private bool _syncingListMetadata;

		private bool _syncingCharacterControls;

		private ButtonImage _copyButton;

		private ButtonImage _deleteButton;

		private Character_Model _selectedCharacter;

		private bool _pendingInitialLayout = true;

		public CharacterSwapping CharacterSwapping { get; }

		public CharacterRoutineDetailPanel(TextureManager textureManager, CharacterRoutineService service, CharacterSwapping characterSwapping, Settings settings, ObservableCollection<Character_Model> characterModels, int width)
		{
			_textureManager = textureManager;
			_service = service;
			_settings = settings;
			CharacterSwapping = characterSwapping;
			_characterModels = characterModels;
			base.Width = width;
			HeightSizingMode = SizingMode.Fill;
			base.FlowDirection = ControlFlowDirection.SingleTopToBottom;
			base.ControlPadding = new Vector2(0f, 5f);
			base.OuterControlPadding = new Vector2(5f);
			_placeholderLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Text = strings.CharacterRoutinePlaceholder,
				Font = Control.Content.DefaultFont16,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				TextColor = Color.LightGray
			};
			_contentPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(0f, 5f)
			};
			Kenedia.Modules.Core.Controls.FlowPanel namePanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = _contentPanel,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				ControlPadding = new Vector2(5f, 0f)
			};
			_nameLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = namePanel,
				Text = strings.Name + ":",
				Width = 75,
				Height = 30,
				VerticalAlignment = VerticalAlignment.Middle
			};
			_nameBox = new Blish_HUD.Controls.TextBox
			{
				Parent = namePanel,
				Width = 300,
				Height = 30
			};
			_nameBox.TextChanged += delegate
			{
				if (!_syncingListMetadata)
				{
					_service.UpdateSelectedRoutineName(_nameBox.Text);
				}
			};
			_copyButton = new ButtonImage
			{
				Parent = namePanel,
				Texture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Copy, "Copy"),
				HoveredTexture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Copy_Hovered, "Copy_Hovered"),
				ClickedTexture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Copy_Active, "Copy_Active"),
				Width = 30,
				Height = 30,
				SetLocalizedTooltip = () => strings.CopyCharacterRoutineTooltip,
				ClickAction = delegate
				{
					_service.CopySelectedRoutine();
				}
			};
			_deleteButton = new ButtonImage
			{
				Parent = namePanel,
				BasicTooltipText = strings.DeleteList,
				Width = 30,
				Height = 30,
				Texture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Delete, "Delete"),
				HoveredTexture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Delete_Hovered, "Delete_Hovered"),
				ClickedTexture = (AsyncTexture2D)TexturesService.GetTextureFromRef(textures_common.Delete_Active, "Delete_Active"),
				ClickAction = delegate
				{
					ConfirmDeleteSelectedRoutine();
				}
			};
			Kenedia.Modules.Core.Controls.FlowPanel resetPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = _contentPanel,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				ControlPadding = new Vector2(5f, 0f)
			};
			_resetLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = resetPanel,
				Text = strings.AutoResetField,
				Width = 75,
				Height = 30,
				VerticalAlignment = VerticalAlignment.Middle
			};
			_resetDropdown = new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = resetPanel,
				Width = 220,
				Height = 30
			};
			_resetDropdown.Items.Add(strings.ResetNone);
			_resetDropdown.Items.Add(strings.ResetDaily);
			_resetDropdown.Items.Add(strings.ResetWeekly);
			_resetDropdown.ValueChangedAction = delegate(string selected)
			{
				if (!_syncingListMetadata)
				{
					ResetFrequency resetFrequency = ((selected == strings.ResetDaily) ? ResetFrequency.Daily : ((selected == strings.ResetWeekly) ? ResetFrequency.Weekly : ResetFrequency.None));
					ResetFrequency frequency = resetFrequency;
					_service.UpdateSelectedRoutineResetFrequency(frequency);
				}
			};
			_stepsPanel = new CharacterRoutineStepsPanel(_service, settings, characterModels)
			{
				Parent = _contentPanel
			};
			_service.State.SelectedRoutine.Changed += new EventHandler<StateVarChangedEventArgs<CharacterRoutineModel>>(SelectedRoutine_Changed);
			BindSelectedRoutine(_service.SelectedRoutine);
		}

		private async void ConfirmDeleteSelectedRoutine()
		{
			CharacterRoutineModel list = _service.SelectedRoutine;
			if (list != null && await new BaseDialog(strings.DeleteConfirmationTitle, string.Format(strings.ConfirmCharacterRoutineDelete, list.Name))
			{
				DesiredWidth = 380,
				AutoSize = true
			}.ShowDialog() == DialogResult.OK && _service.SelectedRoutine == list)
			{
				_service.DeleteSelectedRoutine();
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (_pendingInitialLayout && base.Parent != null)
			{
				_pendingInitialLayout = false;
				RecalculateLayout();
				_stepsPanel?.RecalculateLayout();
			}
		}

		protected override void DisposeControl()
		{
			_service.State.SelectedRoutine.Changed -= new EventHandler<StateVarChangedEventArgs<CharacterRoutineModel>>(SelectedRoutine_Changed);
			base.DisposeControl();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			base.OnResized(e);
			if (_deleteButton != null && _copyButton != null && _nameBox != null && _resetDropdown != null)
			{
				_deleteButton?.SetLocation(base.Width - 10 - _deleteButton.Width, base.Top);
				_copyButton?.SetLocation(_deleteButton.Left - _copyButton.Width - 5, base.Top);
				_nameBox?.SetSize(Math.Max(100, _copyButton.Left - _nameBox.Left - 5), _nameBox.Height);
				_resetDropdown?.SetSize(Math.Max(100, _deleteButton.Right - _nameBox.Left), _resetDropdown.Height);
			}
		}

		private void BindSelectedRoutine(CharacterRoutineModel list)
		{
			_boundList = list;
			bool hasList = _boundList != null;
			_placeholderLabel.Visible = !hasList;
			_contentPanel.Visible = hasList;
			_stepsPanel.BindSelectedRoutine(_boundList);
			if (hasList)
			{
				_syncingListMetadata = true;
				_nameBox.Text = _boundList.Name ?? string.Empty;
				Kenedia.Modules.Core.Controls.Dropdown resetDropdown = _resetDropdown;
				resetDropdown.SelectedItem = _boundList.ResetFrequency switch
				{
					ResetFrequency.Daily => strings.ResetDaily, 
					ResetFrequency.Weekly => strings.ResetWeekly, 
					_ => strings.ResetNone, 
				};
				_syncingListMetadata = false;
			}
		}

		private bool IsBoundList(CharacterRoutineModel list)
		{
			if (_boundList != null)
			{
				return _boundList == list;
			}
			return false;
		}

		private void SelectedRoutine_Changed(object sender, StateVarChangedEventArgs<CharacterRoutineModel> e)
		{
			if (e.OldValue != e.NewValue)
			{
				BindSelectedRoutine(e.NewValue);
			}
			else if (IsBoundList(e.NewValue))
			{
				_syncingListMetadata = true;
				_nameBox.Text = e.NewValue.Name ?? string.Empty;
				Kenedia.Modules.Core.Controls.Dropdown resetDropdown = _resetDropdown;
				resetDropdown.SelectedItem = e.NewValue.ResetFrequency switch
				{
					ResetFrequency.Daily => strings.ResetDaily, 
					ResetFrequency.Weekly => strings.ResetWeekly, 
					_ => strings.ResetNone, 
				};
				_syncingListMetadata = false;
			}
		}
	}
}
