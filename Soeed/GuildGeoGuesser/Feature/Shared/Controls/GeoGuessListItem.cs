using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class GeoGuessListItem : DetailsButton
	{
		private Puzzle _model;

		private readonly int ICON_SIZE = 140;

		private readonly int SCALE_FACTOR = 4;

		private int ICON_SCALED;

		private Point ICON_OFFSET;

		private Rectangle DrawBounds;

		private Rectangle SourceBounds;

		private AsyncTexture2D _Icon = new AsyncTexture2D();

		private bool _isImageLoaded;

		private string _textureKey = string.Empty;

		private EventHandler<ValueChangedEventArgs<Texture2D>>? _textureSwappedHandler;

		private AsyncTexture2D _Checkmark = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156950);

		private AsyncTexture2D _HeartIcon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156127);

		private bool _hasAlreadyGuessed;

		private bool _hasUpvoted;

		private string _userName = string.Empty;

		private readonly TimeSinceCreated _timer;

		private string _cameraModeString = string.Empty;

		private bool _isFirstPersonCamera;

		public Puzzle Model => _model;

		public GeoGuessListItem(Puzzle model)
			: this()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			TimeSinceCreated timeSinceCreated = new TimeSinceCreated(model.CreatedAt);
			((Control)timeSinceCreated).set_Parent((Container)(object)this);
			_timer = timeSinceCreated;
			_model = model;
			((Control)this).add_Click((EventHandler<MouseEventArgs>)GeoGuessListItem_Click);
			Build();
		}

		private void GeoGuessListItem_Click(object sender, MouseEventArgs e)
		{
			Service.GeoGuessWindow.State.SelectModelForDetails(_model);
		}

		protected void Build()
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			ICON_SCALED = ICON_SIZE * SCALE_FACTOR;
			((Control)this).set_Width(400);
			((Control)this).set_Height(150);
			((DetailsButton)this).set_Text(DescriptionString(_model));
			DrawBounds = new Rectangle(5, 5, ICON_SIZE, ICON_SIZE);
			SourceBounds = new Rectangle(0, 0, ICON_SIZE, ICON_SIZE);
			Account? account = Service.UserManager.Account;
			string user = ((account != null) ? account!.get_Name() : null);
			_userName = user ?? string.Empty;
			if (!string.IsNullOrEmpty(_userName))
			{
				if (!_model.IsAuthor(_userName) && _model.UserHasGuessed(_userName))
				{
					try
					{
						_hasAlreadyGuessed = true;
						string score = _model.Guesses.Find((GuessUser g) => g.AccountName == _userName).PuzzleGuess.Location.Score(_model.Location);
						((Control)this).set_BasicTooltipText("You have already guessed\n\nYou scored: " + score);
						((DetailsButton)this).set_Text(DescriptionString(_model) + "\nYour score: " + score);
					}
					catch (Exception)
					{
					}
				}
				if (_model.IsAuthor(_userName))
				{
					_hasAlreadyGuessed = true;
				}
				_hasUpvoted = _model.HasUserUpvoted(_userName);
			}
			_cameraModeString = _model.Location.GetCameraModeString();
			_isFirstPersonCamera = _model.Location.IsFirstPersonCamera();
		}

		protected string DescriptionString(Puzzle model)
		{
			return model.Title + "\nCreated by: " + model.AccountName + "\n" + $"Total Guesses: {model.ActualGuessCount}\n" + $"Upvotes: {model.Upvotes}";
		}

		private void LoadImage()
		{
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			if (_isImageLoaded)
			{
				return;
			}
			_textureKey = _model.Image;
			AsyncTexture2D texture = _model.GetImageTexture();
			ICON_SCALED = Math.Min(Math.Min(texture.get_Height(), texture.get_Width()), ICON_SCALED);
			_Icon = texture;
			ICON_OFFSET.X = (texture.get_Width() - ICON_SCALED) / 2;
			ICON_OFFSET.Y = (texture.get_Height() - ICON_SCALED) / 2;
			SourceBounds = new Rectangle(ICON_OFFSET.X, ICON_OFFSET.Y, ICON_SCALED, ICON_SCALED);
			if (!texture.get_HasSwapped())
			{
				_textureSwappedHandler = delegate(object s, ValueChangedEventArgs<Texture2D> e)
				{
					//IL_0094: Unknown result type (might be due to invalid IL or missing references)
					//IL_0099: Unknown result type (might be due to invalid IL or missing references)
					ICON_SCALED = Math.Min(Math.Min(e.get_NewValue().get_Height(), e.get_NewValue().get_Width()), ICON_SIZE * SCALE_FACTOR);
					ICON_OFFSET.X = (e.get_NewValue().get_Width() - ICON_SCALED) / 2;
					ICON_OFFSET.Y = (e.get_NewValue().get_Height() - ICON_SCALED) / 2;
					SourceBounds = new Rectangle(ICON_OFFSET.X, ICON_OFFSET.Y, ICON_SCALED, ICON_SCALED);
					_Icon = AsyncTexture2D.op_Implicit(e.get_NewValue());
				};
				texture.add_TextureSwapped(_textureSwappedHandler);
			}
			_isImageLoaded = true;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			((DetailsButton)this).PaintBeforeChildren(spriteBatch, bounds);
			if (!_isImageLoaded)
			{
				LoadImage();
			}
			if (_Icon != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_Icon), DrawBounds, (Rectangle?)SourceBounds);
			}
			if (!_isFirstPersonCamera)
			{
				string cameraMode = _cameraModeString;
				Color cameraColor = Color.get_Green();
				Rectangle cameraRect = default(Rectangle);
				((Rectangle)(ref cameraRect))._002Ector(5, bounds.Height - 25, 65, 20);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), cameraRect, cameraColor * 0.7f);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, cameraMode, Control.get_Content().get_DefaultFont12(), cameraRect, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			if (_hasAlreadyGuessed)
			{
				if (_hasUpvoted)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_HeartIcon), new Rectangle(bounds.Width - 32, bounds.Height - 32, 32, 32));
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_Checkmark), new Rectangle(bounds.Width - 32, bounds.Height - 32, 32, 32));
				}
			}
		}

		public void Dispose()
		{
			((Control)this).remove_Click((EventHandler<MouseEventArgs>)GeoGuessListItem_Click);
			if (_isImageLoaded && !string.IsNullOrEmpty(_textureKey) && Service.Textures != null)
			{
				Service.Textures.ReleaseTexture(_textureKey);
			}
			if (_Icon != null && _textureSwappedHandler != null)
			{
				_Icon.remove_TextureSwapped(_textureSwappedHandler);
				_textureSwappedHandler = null;
			}
			_Icon = null;
			((Control)this).Dispose();
		}
	}
}
