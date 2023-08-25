using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace felix.BlishEmotes.UI.Controls
{
	internal class RadialMenu : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<RadialMenu>();

		private Helper _helper;

		private ModuleSettings _settings;

		private Texture2D _lockedTexture;

		private List<RadialEmote> _radialEmotes = new List<RadialEmote>();

		private bool _isActionCamToggled;

		private int _innerRadius = 100;

		private int _radius;

		private int _iconSize;

		private int _maxRadialDiameter;

		private Label _noEmotesLabel;

		private Label _selectedEmoteLabel;

		private Point RadialSpawnPoint;

		private float _debugLineThickness = 2f;

		public List<Emote> Emotes { private get; set; }

		private RadialEmote SelectedEmote => _radialEmotes.SingleOrDefault((RadialEmote m) => m.Selected);

		public RadialMenu(Helper helper, ModuleSettings settings, Texture2D LockedTexture)
		{
			_helper = helper;
			_settings = settings;
			Emotes = new List<Emote>();
			_lockedTexture = LockedTexture;
			base.Visible = false;
			base.Padding = Blish_HUD.Controls.Thickness.Zero;
			base.Shown += async delegate(object sender, EventArgs e)
			{
				await HandleShown(sender, e);
			};
			base.Hidden += async delegate(object sender, EventArgs e)
			{
				await HandleHidden(sender, e);
			};
			_noEmotesLabel = new Label
			{
				Parent = this,
				Location = new Point(0, 0),
				Size = new Point(800, 500),
				Font = GameService.Content.DefaultFont32,
				Text = "No Emotes found!",
				TextColor = Color.Red
			};
			_selectedEmoteLabel = new Label
			{
				Parent = this,
				Location = new Point(0, 0),
				Size = new Point(200, 50),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Middle,
				Font = GameService.Content.DefaultFont32,
				Text = "",
				BackgroundColor = Color.Black * 0.5f
			};
		}

		protected override CaptureType CapturesInput()
		{
			return CaptureType.Mouse;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			_radialEmotes.Clear();
			_selectedEmoteLabel.Text = "";
			if (Emotes.Count == 0)
			{
				_noEmotesLabel.Show();
				return;
			}
			_noEmotesLabel.Hide();
			double startAngle = Math.PI * Math.Floor(270.0) / 180.0;
			if (Helper.IsDebugEnabled())
			{
				spriteBatch.DrawCircle(RadialSpawnPoint.ToVector2(), _innerRadius, 50, Color.Red, _debugLineThickness);
			}
			double currentAngle = startAngle;
			double sweepAngle = Math.PI * 2.0 / (double)Emotes.Count;
			foreach (Emote emote in Emotes)
			{
				double midAngle = currentAngle + sweepAngle / 2.0;
				double endAngle = currentAngle + sweepAngle;
				int x = (int)Math.Round((double)_radius + (double)_radius * Math.Cos(midAngle));
				int y = (int)Math.Round((double)_radius + (double)_radius * Math.Sin(midAngle));
				if (Helper.IsDebugEnabled())
				{
					float xDebugInner = (float)Math.Round((double)_innerRadius * Math.Cos(currentAngle)) + (float)RadialSpawnPoint.X;
					float yDebugInner = (float)Math.Round((double)_innerRadius * Math.Sin(currentAngle)) + (float)RadialSpawnPoint.Y;
					int debugRadiusOuter = 250;
					float xDebugOuter = (float)Math.Round((double)(2 * debugRadiusOuter) * Math.Cos(currentAngle)) + (float)RadialSpawnPoint.X;
					float yDebugOuter = (float)Math.Round((double)(2 * debugRadiusOuter) * Math.Sin(currentAngle)) + (float)RadialSpawnPoint.Y;
					spriteBatch.DrawLine(new Vector2(xDebugInner, yDebugInner), new Vector2(xDebugOuter, yDebugOuter), Color.Red, _debugLineThickness);
				}
				_radialEmotes.Add(new RadialEmote
				{
					Emote = emote,
					StartAngle = currentAngle,
					EndAngle = endAngle,
					X = x,
					Y = y,
					Text = _helper.EmotesResourceManager.GetString(emote.Id),
					Texture = emote.Texture
				});
				currentAngle = endAngle;
			}
			Point diff = Control.Input.Mouse.Position - RadialSpawnPoint;
			double angle;
			for (angle = Math.Atan2(diff.Y, diff.X); angle < startAngle; angle += Math.PI * 2.0)
			{
			}
			float length = new Vector2(diff.Y, diff.X).Length();
			foreach (RadialEmote radialEmote in _radialEmotes)
			{
				if (length >= (float)_innerRadius)
				{
					radialEmote.Selected = radialEmote.StartAngle <= angle && radialEmote.EndAngle > angle;
					if (radialEmote.Selected)
					{
						_selectedEmoteLabel.Text = radialEmote.Text;
					}
				}
				spriteBatch.DrawOnCtrl(this, radialEmote.Texture, new Rectangle(radialEmote.X, radialEmote.Y, _iconSize, _iconSize), null, radialEmote.Emote.Locked ? (Color.White * 0.25f) : (Color.White * (radialEmote.Selected ? 1f : _settings.RadialIconOpacity.Value)));
				if (radialEmote.Emote.Locked)
				{
					spriteBatch.DrawOnCtrl(this, _lockedTexture, new Rectangle(radialEmote.X, radialEmote.Y, _iconSize, _iconSize), null, Color.White * (radialEmote.Selected ? 1f : _settings.RadialIconOpacity.Value));
				}
			}
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		private async Task HandleShown(object sender, EventArgs e)
		{
			Logger.Debug("HandleShown entered");
			if (!GameService.Input.Mouse.CursorIsVisible && !_settings.RadialToggleActionCameraKeyBind.IsNull)
			{
				_isActionCamToggled = true;
				await _helper.TriggerKeybind(_settings.RadialToggleActionCameraKeyBind);
				Logger.Debug("HandleShown turned off action cam");
			}
			_maxRadialDiameter = Math.Min(GameService.Graphics.SpriteScreen.Width, GameService.Graphics.SpriteScreen.Height);
			_iconSize = (int)((float)(_maxRadialDiameter / 8) * _settings.RadialIconSizeModifier.Value);
			_radius = (int)(((double)_maxRadialDiameter * 0.75 - (double)(_iconSize / 2)) * (double)_settings.RadialRadiusModifier.Value);
			_innerRadius = (int)((float)_radius * _settings.RadialInnerRadiusPercentage.Value);
			base.Size = new Point(_maxRadialDiameter, _maxRadialDiameter);
			if (_settings.RadialSpawnAtCursor.Value)
			{
				RadialSpawnPoint = Control.Input.Mouse.Position;
			}
			else
			{
				Mouse.SetPosition(GameService.Graphics.WindowWidth / 2, GameService.Graphics.WindowHeight / 2, sendToSystem: true);
				RadialSpawnPoint = new Point(GameService.Graphics.SpriteScreen.Width / 2, GameService.Graphics.SpriteScreen.Height / 2);
			}
			base.Location = new Point(RadialSpawnPoint.X - _radius - _iconSize / 2, RadialSpawnPoint.Y - _radius - _iconSize / 2);
			_selectedEmoteLabel.Location = new Point(RadialSpawnPoint.X - base.Location.X - _selectedEmoteLabel.Size.X / 2, RadialSpawnPoint.Y - base.Location.Y - _selectedEmoteLabel.Size.Y / 2 - 20);
		}

		private async Task HandleHidden(object sender, EventArgs e)
		{
			Logger.Debug("HandleHidden entered");
			if (_isActionCamToggled)
			{
				await _helper.TriggerKeybind(_settings.RadialToggleActionCameraKeyBind);
				_isActionCamToggled = false;
				Logger.Debug("HandleHidden turned back on action cam");
			}
			RadialEmote selected = SelectedEmote;
			if (selected != null)
			{
				Logger.Debug("Sending command for " + selected.Emote.Id);
				_helper.SendEmoteCommand(selected.Emote);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			Logger.Debug("UPDATE CONTAINER");
		}
	}
}
