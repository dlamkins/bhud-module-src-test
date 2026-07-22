using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterTooltip : FramedContainer
	{
		private readonly AsyncTexture2D _iconFrame = AsyncTexture2D.FromAssetId(1414041);

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _contentPanel;

		private readonly Dummy _iconDummy;

		private readonly CharacterLabels _infoLabels;

		private readonly Func<Character_Model> _currentCharacter;

		private readonly TextureManager _textureManager;

		private readonly Data _data;

		private Point _textureOffset = new Point(25, 25);

		private readonly List<Tag> _tags = new List<Tag>();

		public Color BackgroundTint { get; set; } = Color.Honeydew * 0.95f;


		public BitmapFont Font { get; set; } = GameService.Content.DefaultFont14;


		public BitmapFont NameFont { get; set; } = GameService.Content.DefaultFont18;


		public Character_Model Character
		{
			[CompilerGenerated]
			get
			{
				return _003CCharacter_003Ek__BackingField;
			}
			set
			{
				_infoLabels.Character = value;
				Character_Model temp = _003CCharacter_003Ek__BackingField;
				if (Common.SetProperty(ref _003CCharacter_003Ek__BackingField, value))
				{
					if (temp != null)
					{
						temp.Updated -= new EventHandler(Character_Updated);
					}
					if (_003CCharacter_003Ek__BackingField != null)
					{
						_003CCharacter_003Ek__BackingField.Updated += new EventHandler(Character_Updated);
					}
				}
			}
		}

		public CharacterTooltip(Func<Character_Model> currentCharacter, TextureManager textureManager, Data data, Settings settings)
		{
			_currentCharacter = currentCharacter;
			_textureManager = textureManager;
			_data = data;
			base.TextureRectangle = new Rectangle(60, 25, 250, 250);
			base.BackgroundImage = AsyncTexture2D.FromAssetId(156003);
			base.BorderColor = Color.Black;
			base.BorderWidth = new RectangleDimensions(2);
			base.BackgroundColor = Color.Black * 0.6f;
			HeightSizingMode = SizingMode.AutoSize;
			base.AutoSizePadding = new Point(5, 5);
			_contentPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(5f, 2f),
				OuterControlPadding = new Vector2(5f, 0f),
				AutoSizePadding = new Point(5, 5)
			};
			_iconDummy = new Dummy
			{
				Parent = this
			};
			_infoLabels = new CharacterLabels(_contentPanel)
			{
				Settings = settings,
				Data = data,
				TextureManager = textureManager,
				CurrentCharacter = _currentCharacter
			};
			_infoLabels.UpdateDataControlsVisibility(tooltip: true);
		}

		private void Character_Updated(object sender, EventArgs e)
		{
			_infoLabels.UpdateDataControlsVisibility(tooltip: true);
			_infoLabels.Update();
			UpdateSize();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			base.Location = new Point(Control.Input.Mouse.Position.X, Control.Input.Mouse.Position.Y + 35);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		public void UpdateSize()
		{
			_infoLabels.UpdateDataControlsVisibility(tooltip: true);
			IEnumerable<Control> visibleControls = _infoLabels.DataControls.Where((Control e) => e.Visible);
			int num = visibleControls.Count();
			int height = ((num > 0) ? visibleControls.Aggregate(0, (int result, Control ctrl) => result + ctrl.Height + (int)_contentPanel.ControlPadding.Y) : 0);
			if (num > 0)
			{
				visibleControls.Max((Control ctrl) => (ctrl != _infoLabels.TagPanel) ? ctrl.Width : 0);
			}
			_contentPanel.Height = height;
			_contentPanel.Width = base.Width;
			_infoLabels.TagPanel.FitWidestTag(base.Width - 10);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			base.Location = new Point(Control.Input.Mouse.Position.X, Control.Input.Mouse.Position.Y + 35);
			Character_Updated(this, null);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			if (Character != null)
			{
				Character.Updated -= new EventHandler(Character_Updated);
			}
		}
	}
}
