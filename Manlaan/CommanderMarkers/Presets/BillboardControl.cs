using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Manlaan.CommanderMarkers.Presets.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Manlaan.CommanderMarkers.Presets
{
	public class BillboardControl : Control
	{
		private readonly List<BillBoardPreview> _entities = new List<BillBoardPreview>();

		private readonly MapData _mapData;

		private AsyncTexture2D _interactBackground = AsyncTexture2D.FromAssetId(156775);

		private Texture2D? _invertedTexture;

		private bool _inverted;

		public BillboardControl(MapData mapData)
			: this()
		{
			_mapData = mapData;
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public void AddEntity(BillBoardPreview entity)
		{
			_entities.Add(entity);
		}

		public void RemoveEntity(BillBoardPreview entity)
		{
			_entities.Remove(entity);
		}

		public void ClearEntities()
		{
			_entities.Clear();
		}

		private void InvertTexture(AsyncTexture2D texture)
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			if (texture.get_HasTexture())
			{
				Texture2D texture2 = texture.get_Texture();
				int width = texture2.get_Width();
				int height = texture2.get_Height();
				Color[] data = (Color[])(object)new Color[width * height];
				texture2.GetData<Color>(data);
				for (int i = 0; i < data.Length; i++)
				{
					data[i] = new Color(255 - ((Color)(ref data[i])).get_R(), 255 - ((Color)(ref data[i])).get_G(), 255 - ((Color)(ref data[i])).get_B(), (int)((Color)(ref data[i])).get_A());
				}
				GraphicsDeviceContext graphicsDeviceContext = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					Texture2D invertedTexture = new Texture2D(((GraphicsDeviceContext)(ref graphicsDeviceContext)).get_GraphicsDevice(), width, height);
					invertedTexture.SetData<Color>(data);
					_invertedTexture = invertedTexture;
				}
				finally
				{
					((GraphicsDeviceContext)(ref graphicsDeviceContext)).Dispose();
				}
			}
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).DoUpdate(gameTime);
			((Control)this).set_Size(((Control)((Control)this).get_Parent()).get_Size());
			if (!_inverted && _interactBackground.get_HasSwapped())
			{
				_inverted = true;
				InvertTexture(_interactBackground);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame() || _mapData.Current == null || (GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat() && !Service.Settings.AutoMarker_Allow_Combat_Placement.get_Value()) || (Service.Settings.AutoMarker_OnlyWhenCommander.get_Value() && !GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander() && !Service.LtMode.get_Value()) || GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				return;
			}
			GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
			((Rectangle)(ref bounds)).set_Location(((Control)this).get_Location());
			Rectangle _promptRectangle = default(Rectangle);
			Rectangle _textRectangle = default(Rectangle);
			foreach (BillBoardPreview entity in _entities)
			{
				entity.Draw();
				if (Service.Settings.AutoMarker_Billboard_Placement.get_Value() && entity.PlayerWithinTriggerDistance())
				{
					((Rectangle)(ref _promptRectangle))._002Ector(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 + 150, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2 + 120, 300, 150);
					((Rectangle)(ref _textRectangle))._002Ector(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 + 220, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2 + 110, 300, 150);
					AsyncTexture2D textureToUse = _interactBackground;
					string interactKey = Service.Settings._settingInteractKeyBinding.get_Value().GetBindingDisplayText();
					BitmapFont _bitmapFont = GameService.Content.get_DefaultFont18();
					spriteBatch.Draw(AsyncTexture2D.op_Implicit(textureToUse), _promptRectangle, Color.get_White());
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Press '" + interactKey + "' to place markers\n" + entity.GetMarkerText(), _bitmapFont, _textRectangle, Color.get_Black(), false, true, 2, (HorizontalAlignment)0, (VerticalAlignment)1);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Press '" + interactKey + "' to place markers\n" + entity.GetMarkerText(), _bitmapFont, _textRectangle, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				}
			}
		}

		protected override void DisposeControl()
		{
			Texture2D? invertedTexture = _invertedTexture;
			if (invertedTexture != null)
			{
				((GraphicsResource)invertedTexture).Dispose();
			}
			((Control)this).DisposeControl();
		}
	}
}
