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
using Microsoft.Xna.Framework.Input;

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
			base.BackgroundColor = Color.Transparent * 0.2f;
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
			if (e.Key == Keys.Escape)
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
			base.DoUpdate(gameTime);
			if (!Control.Input.Keyboard.KeysDown.Contains(_settings.RadialKey.Value.PrimaryKey))
			{
				Hide();
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			_tooltip?.Hide();
			base.Paint(spriteBatch, bounds);
		}

		protected override ColorGradient GetSliceColors(int index, bool contains_mouse)
		{
			if (_settings.Radial_UseProfessionColor.Value && _displayedCharacters.Count > index)
			{
				return new ColorGradient(_displayedCharacters[index].Profession.GetProfessionColor() * (contains_mouse ? 0.8f : 0.5f));
			}
			return base.GetSliceColors(index, contains_mouse);
		}

		protected override void DrawSliceContent(SpriteBatch spriteBatch, bool contains_mouse, Vector2 center, float midAngle, float iconRadius, int sliceIndex)
		{
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
			Vector2 relativeCenter = center / base.DpiScale - texture.Bounds.Size.ToVector2() * scale / 2f;
			Color color = (_settings.Radial_UseProfessionIconsColor.Value ? character.Profession.GetProfessionColor() : Color.White);
			spriteBatch.Draw(texture, relativeCenter, texture.Bounds, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
			if (contains_mouse)
			{
				_selected = character;
				if (_settings.Radial_ShowAdvancedTooltip.Value)
				{
					base.BasicTooltipText = string.Empty;
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
			if (_tooltip != null)
			{
				_tooltip.Character = null;
				_tooltip.Hide();
			}
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
