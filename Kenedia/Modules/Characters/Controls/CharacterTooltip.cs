using System;
using System.Collections.Generic;
using System.Linq;
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

		private readonly FlowPanel _contentPanel;

		private readonly Dummy _iconDummy;

		private readonly CharacterLabels _infoLabels;

		private readonly Func<Character_Model> _currentCharacter;

		private readonly TextureManager _textureManager;

		private readonly Data _data;

		private Rectangle _iconRectangle;

		private Rectangle _contentRectangle;

		private Point _textureOffset = new Point(25, 25);

		private Character_Model _character;

		private readonly List<Tag> _tags = new List<Tag>();

		private bool _updateCharacter;

		public Color BackgroundTint { get; set; } = Color.get_Honeydew() * 0.95f;


		public BitmapFont Font { get; set; } = GameService.Content.get_DefaultFont14();


		public BitmapFont NameFont { get; set; } = GameService.Content.get_DefaultFont18();


		public Character_Model Character
		{
			get
			{
				return _character;
			}
			set
			{
				_infoLabels.Character = value;
				Common.SetProperty(ref _character, value);
			}
		}

		public CharacterTooltip(Func<Character_Model> currentCharacter, TextureManager textureManager, Data data, Settings settings)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			_currentCharacter = currentCharacter;
			_textureManager = textureManager;
			_data = data;
			base.TextureRectangle = new Rectangle(60, 25, 250, 250);
			base.BackgroundImage = AsyncTexture2D.FromAssetId(156003);
			base.BorderColor = Color.get_Black();
			base.BorderWidth = new RectangleDimensions(2);
			base.BackgroundColor = Color.get_Black() * 0.6f;
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_AutoSizePadding(new Point(5, 5));
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(5f, 2f));
			((FlowPanel)flowPanel).set_OuterControlPadding(new Vector2(5f, 0f));
			((Container)flowPanel).set_AutoSizePadding(new Point(5, 5));
			_contentPanel = flowPanel;
			Dummy dummy = new Dummy();
			((Control)dummy).set_Parent((Container)(object)this);
			_iconDummy = dummy;
			_infoLabels = new CharacterLabels(_contentPanel)
			{
				Settings = settings,
				Data = data,
				TextureManager = textureManager,
				CurrentCharacter = _currentCharacter
			};
			_infoLabels.UpdateDataControlsVisibility(tooltip: true);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateContainer(gameTime);
			((Control)this).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
				.X, Control.get_Input().get_Mouse().get_Position()
				.Y + 35));
			_infoLabels.UpdateDataControlsVisibility(tooltip: true);
			_infoLabels.Update(gameTime);
			UpdateSize();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		public void UpdateSize()
		{
			_infoLabels.UpdateDataControlsVisibility(tooltip: true);
			IEnumerable<Control> visibleControls = _infoLabels.DataControls.Where((Control e) => e.get_Visible());
			int num = visibleControls.Count();
			int height = ((num > 0) ? visibleControls.Aggregate(0, (int result, Control ctrl) => result + ctrl.get_Height() + (int)((FlowPanel)_contentPanel).get_ControlPadding().Y) : 0);
			if (num > 0)
			{
				visibleControls.Max((Control ctrl) => (ctrl != _infoLabels.TagPanel) ? ctrl.get_Width() : 0);
			}
			((Control)_contentPanel).set_Height(height);
			((Control)_contentPanel).set_Width(((Control)this).get_Width());
			_infoLabels.TagPanel.FitWidestTag(((Control)this).get_Width() - 10);
		}

		protected override void OnShown(EventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			base.OnShown(e);
			((Control)this).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
				.X, Control.get_Input().get_Mouse().get_Position()
				.Y + 35));
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
		}
	}
}
