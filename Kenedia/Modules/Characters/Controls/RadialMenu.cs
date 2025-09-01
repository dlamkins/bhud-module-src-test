using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Characters.Controls
{
	public class RadialMenu : Kenedia.Modules.Core.Controls.RadialMenu
	{
		private readonly Data _data;

		private readonly AsyncTexture2D _dummy = AsyncTexture2D.FromAssetId(1128572);

		private readonly CharacterTooltip _tooltip;

		private readonly Settings _settings;

		private readonly ObservableCollection<Character_Model> _characters;

		private readonly Func<Character_Model> _currentCharacter;

		private List<Character_Model> _displayedCharacters;

		private int _iconSize;

		private readonly List<RadialMenuSection> _sections = new List<RadialMenuSection>();

		private Character_Model? _selected;

		private Vector2 _center;

		private GraphicsDevice _graphicsDevice;

		private BasicEffect _effect;

		public RadialMenu(Settings settings, ObservableCollection<Character_Model> characters, Blish_HUD.Controls.Container parent, Func<Character_Model> currentCharacter, Data data, TextureManager textureManager)
		{
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			_settings = settings;
			_settings.Radial_SliceHighlight.PropertyChanged += Radial_Colors_PropertyChanged;
			_settings.Radial_SliceBackground.PropertyChanged += Radial_Colors_PropertyChanged;
			_characters = characters;
			_currentCharacter = currentCharacter;
			_data = data;
			base.Parent = parent;
			_tooltip = new CharacterTooltip(currentCharacter, textureManager, data, settings)
			{
				Parent = GameService.Graphics.SpriteScreen,
				ZIndex = 1073741824,
				Size = new Point(300, 50),
				Visible = false
			};
			base.BackgroundColor = Color.get_Transparent() * 0.2f;
			foreach (Character_Model character in _characters)
			{
				character.Updated += new EventHandler(Character_Updated);
			}
			base.Parent.Resized += Parent_Resized;
			Control.Input.Keyboard.KeyPressed += Keyboard_KeyPressed;
			base.SliceBackground = _settings.Radial_SliceBackground.Value;
			base.SliceHighlight = _settings.Radial_SliceHighlight.Value;
		}

		private void Radial_Colors_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.SliceBackground = _settings.Radial_SliceBackground.Value;
			base.SliceHighlight = _settings.Radial_SliceHighlight.Value;
		}

		public void SetDisplayedCharacters()
		{
			_displayedCharacters = _characters?.Where((Character_Model e) => e.ShowOnRadial).ToList() ?? new List<Character_Model>();
			base.Slices = _displayedCharacters.Count;
		}

		public bool HasDisplayedCharacters()
		{
			return _displayedCharacters.Count > 0;
		}

		private void Keyboard_KeyPressed(object sender, KeyboardEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			if ((int)e.Key == 27)
			{
				Hide();
			}
		}

		private void Parent_Resized(object sender, ResizedEventArgs e)
		{
			RecalculateLayout();
		}

		private void Character_Updated(object sender, EventArgs e)
		{
			RecalculateLayout();
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			_ = _graphicsDevice;
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			base.OnMouseMoved(e);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			base.DoUpdate(gameTime);
			if (!Control.Input.Keyboard.KeysDown.Contains(_settings.RadialKey.Value.PrimaryKey))
			{
				Hide();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			_tooltip?.Hide();
			base.Paint(spriteBatch, bounds);
		}

		protected override ColorGradient GetSliceColors(int index, bool contains_mouse)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (_settings.Radial_UseProfessionColor.Value && _displayedCharacters.Count > index)
			{
				return new ColorGradient(_displayedCharacters[index].Profession.GetProfessionColor() * (contains_mouse ? 0.8f : 0.5f));
			}
			return base.GetSliceColors(index, contains_mouse);
		}

		protected override void DrawSliceContent(SpriteBatch spriteBatch, bool contains_mouse, Vector2 center, float midAngle, float iconRadius, int sliceIndex)
		{
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			if (_displayedCharacters.Count <= sliceIndex)
			{
				return;
			}
			Character_Model character = _displayedCharacters[sliceIndex];
			AsyncTexture2D texture = (_settings.Radial_UseProfessionIcons.Value ? (character.SpecializationIcon ?? character.ProfessionIcon ?? _dummy) : (character.Icon ?? character.ProfessionIcon));
			if (_settings.Radial_UseProfessionIconsColor.Value)
			{
				texture = character.SpecializationIcon ?? texture;
			}
			int totalSlices = _displayedCharacters.Count;
			float sliceAngle = (float)Math.PI * 2f / (float)totalSlices;
			float scale = 2f * iconRadius * (float)Math.Sin(sliceAngle / 2f) * 0.75f / (float)texture.Width;
			_ = new Vector2((float)Math.Cos(midAngle), (float)Math.Sin(midAngle)) * iconRadius;
			Vector2 val = center / base.DpiScale;
			Rectangle bounds = texture.Bounds;
			Point size = ((Rectangle)(ref bounds)).get_Size();
			Vector2 relativeCenter = val - ((Point)(ref size)).ToVector2() * scale / 2f;
			Color color = (_settings.Radial_UseProfessionIconsColor.Value ? character.Profession.GetProfessionColor() : Color.get_White());
			spriteBatch.Draw((Texture2D)texture, relativeCenter, (Rectangle?)texture.Bounds, color, 0f, Vector2.get_Zero(), scale, (SpriteEffects)0, 1f);
			if (contains_mouse)
			{
				if (_settings.Radial_ShowAdvancedTooltip.Value)
				{
					base.BasicTooltipText = string.Empty;
					_selected = character;
					_tooltip.Character = character;
					_tooltip.Show();
				}
				else
				{
					base.BasicTooltipText = character.Name;
				}
			}
		}

		protected override async void OnSliceClick(int i)
		{
			if (_selected == null)
			{
				return;
			}
			Hide();
			if (_displayedCharacters.Count > i)
			{
				Character_Model character = _displayedCharacters[i];
				if (await ExtendedInputService.WaitForNoKeyPressed())
				{
					character.Swap();
				}
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			RecalculateLayout();
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			_tooltip.Character = null;
			_tooltip.Hide();
		}

		protected override void DisposeControl()
		{
			if (base.Parent != null)
			{
				base.Parent.Resized -= Parent_Resized;
			}
			foreach (Character_Model character in _characters)
			{
				character.Updated -= new EventHandler(Character_Updated);
			}
			_tooltip?.Dispose();
			base.DisposeControl();
		}
	}
}
