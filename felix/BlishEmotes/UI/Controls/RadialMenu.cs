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
using felix.BlishEmotes.Strings;

namespace felix.BlishEmotes.UI.Controls
{
	internal class RadialMenu : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<RadialMenu>();

		private Helper _helper;

		private ModuleSettings _settings;

		private Texture2D _lockedTexture;

		private List<RadialContainer<Emote>> _radialEmotes = new List<RadialContainer<Emote>>();

		private List<RadialContainer<Category>> _radialCategories = new List<RadialContainer<Category>>();

		private bool _isActionCamToggled;

		private double _startAngle = Math.PI * Math.Floor(270.0) / 180.0;

		private int _disabledRadius = 100;

		private int _categoryRadius;

		private int _radius;

		private int _iconSize;

		private int _maxRadialDiameter;

		private Label _noEmotesLabel;

		private Label _selectedEmoteLabel;

		private Label _synchronizeToggleActiveLabel;

		private Label _targetToggleActiveLabel;

		private Point RadialSpawnPoint;

		private float _debugLineThickness = 2f;

		public List<Emote> Emotes { private get; set; }

		public List<Category> Categories { private get; set; }

		public bool IsEmoteSynchronized { private get; set; }

		public bool IsEmoteTargeted { private get; set; }

		private RadialContainer<Emote> SelectedEmote => _radialEmotes.SingleOrDefault((RadialContainer<Emote> m) => m.Selected);

		private RadialContainer<Category> SelectedCategory => _radialCategories.SingleOrDefault((RadialContainer<Category> m) => m.Selected);

		public event EventHandler<Emote> EmoteSelected;

		public RadialMenu(ModuleSettings settings, Texture2D LockedTexture)
		{
			_helper = new Helper();
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
			_synchronizeToggleActiveLabel = new Label
			{
				Parent = this,
				Visible = false,
				Location = new Point(0, 0),
				Size = new Point(200, 30),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Middle,
				Font = GameService.Content.DefaultFont14,
				Text = Common.emote_synchronizeActive,
				BackgroundColor = Color.Black * 0.3f
			};
			_targetToggleActiveLabel = new Label
			{
				Parent = this,
				Visible = false,
				Location = new Point(0, 0),
				Size = new Point(200, 30),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Middle,
				Font = GameService.Content.DefaultFont14,
				Text = Common.emote_targetingActive,
				BackgroundColor = Color.Black * 0.3f
			};
		}

		protected override CaptureType CapturesInput()
		{
			return CaptureType.Mouse;
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			_selectedEmoteLabel.Text = "";
			if (Emotes.Count == 0)
			{
				_noEmotesLabel.Show();
				return;
			}
			_noEmotesLabel.Hide();
			if (IsEmoteSynchronized)
			{
				_synchronizeToggleActiveLabel.Show();
			}
			else
			{
				_synchronizeToggleActiveLabel.Hide();
			}
			if (IsEmoteTargeted)
			{
				_targetToggleActiveLabel.Show();
			}
			else
			{
				_targetToggleActiveLabel.Hide();
			}
			List<Emote> emotes = Emotes;
			int emotesInnerRadius = _disabledRadius;
			if (_settings.GlobalUseCategories.Value && Categories.Count > 0)
			{
				PaintEmoteCategories(spriteBatch, Categories);
				emotes = ((SelectedCategory != null) ? SelectedCategory.Value.Emotes : new List<Emote>());
				emotesInnerRadius = _categoryRadius;
			}
			PaintEmotes(spriteBatch, emotesInnerRadius, emotes);
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		private void PaintEmoteCategories(SpriteBatch spriteBatch, List<Category> categories)
		{
			if (Helper.IsDebugEnabled())
			{
				spriteBatch.DrawCircle(RadialSpawnPoint.ToVector2(), _disabledRadius, 50, Color.Red, _debugLineThickness);
			}
			List<RadialContainer<Category>> newList = CreateRadialContainerList(_categoryRadius, (Category category) => SelectedCategory?.Value == category, categories);
			_radialCategories.Clear();
			_radialCategories.AddRange(newList);
			DrawRadialContainerItems(spriteBatch, _disabledRadius, _categoryRadius, _radialCategories);
		}

		private void PaintEmotes(SpriteBatch spriteBatch, int innerRadius, List<Emote> emotes)
		{
			if (Helper.IsDebugEnabled())
			{
				spriteBatch.DrawCircle(RadialSpawnPoint.ToVector2(), innerRadius, 50, Color.Red, _debugLineThickness);
			}
			List<RadialContainer<Emote>> newList = CreateRadialContainerList(_radius, (Emote emote) => false, emotes);
			_radialEmotes.Clear();
			_radialEmotes.AddRange(newList);
			DrawRadialContainerItems(spriteBatch, innerRadius, int.MaxValue, _radialEmotes);
		}

		private void DrawRadialContainerItems<T>(SpriteBatch spriteBatch, int innerRadius, int outerRadius, List<RadialContainer<T>> radialContainerItems) where T : RadialBase
		{
			Point diff = Control.Input.Mouse.Position - RadialSpawnPoint;
			double angle;
			for (angle = Math.Atan2(diff.Y, diff.X); angle < _startAngle; angle += Math.PI * 2.0)
			{
			}
			float length = new Vector2(diff.Y, diff.X).Length();
			foreach (RadialContainer<T> item in radialContainerItems)
			{
				DrawDebugSectionSeparators(spriteBatch, innerRadius, Math.Min(_radius, outerRadius), item);
				if (length >= (float)innerRadius && length <= (float)outerRadius)
				{
					item.Selected = item.StartAngle <= angle && item.EndAngle > angle;
					if (item.Selected)
					{
						_selectedEmoteLabel.Text = item.Text;
					}
				}
				else if (length < (float)innerRadius)
				{
					item.Selected = false;
				}
				spriteBatch.DrawOnCtrl(this, item.Texture, new Rectangle(item.X, item.Y, _iconSize, _iconSize), null, item.Locked ? (Color.White * 0.25f) : (Color.White * (item.Selected ? 1f : _settings.RadialIconOpacity.Value)));
				if (item.Locked)
				{
					spriteBatch.DrawOnCtrl(this, _lockedTexture, new Rectangle(item.X, item.Y, _iconSize, _iconSize), null, Color.White * (item.Selected ? 1f : _settings.RadialIconOpacity.Value));
				}
			}
		}

		private List<RadialContainer<T>> CreateRadialContainerList<T>(int outerRadius, Func<T, bool> IsSelected, List<T> items) where T : RadialBase
		{
			double currentAngle = _startAngle;
			double sweepAngle = Math.PI * 2.0 / (double)items.Count;
			List<RadialContainer<T>> newList = new List<RadialContainer<T>>();
			foreach (T item in items)
			{
				double midAngle = currentAngle + sweepAngle / 2.0;
				double endAngle = currentAngle + sweepAngle;
				int offset = _iconSize / 2;
				int x = (int)Math.Round((double)_radius + (double)(outerRadius - offset) * Math.Cos(midAngle));
				int y = (int)Math.Round((double)_radius + (double)(outerRadius - offset) * Math.Sin(midAngle));
				newList.Add(new RadialContainer<T>
				{
					Value = item,
					StartAngle = currentAngle,
					EndAngle = endAngle,
					X = x,
					Y = y,
					Text = item.Label,
					Texture = item.Texture,
					Selected = IsSelected(item),
					Locked = item.Locked
				});
				currentAngle = endAngle;
			}
			return newList;
		}

		private void DrawDebugSectionSeparators<T>(SpriteBatch spriteBatch, int innerRadius, int outerRadius, RadialContainer<T> item) where T : RadialBase
		{
			if (Helper.IsDebugEnabled())
			{
				float xDebugInner = (float)Math.Round((double)innerRadius * Math.Cos(item.StartAngle)) + (float)RadialSpawnPoint.X;
				float yDebugInner = (float)Math.Round((double)innerRadius * Math.Sin(item.StartAngle)) + (float)RadialSpawnPoint.Y;
				float xDebugOuter = (float)Math.Round((double)outerRadius * Math.Cos(item.StartAngle)) + (float)RadialSpawnPoint.X;
				float yDebugOuter = (float)Math.Round((double)outerRadius * Math.Sin(item.StartAngle)) + (float)RadialSpawnPoint.Y;
				spriteBatch.DrawLine(new Vector2(xDebugInner, yDebugInner), new Vector2(xDebugOuter, yDebugOuter), Color.Red, _debugLineThickness);
			}
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
			_categoryRadius = _disabledRadius + (int)((double)(_radius - _disabledRadius) * 0.5);
			_disabledRadius = (int)((float)_radius * _settings.RadialInnerRadiusPercentage.Value);
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
			_synchronizeToggleActiveLabel.Location = new Point(_selectedEmoteLabel.Location.X, _selectedEmoteLabel.Location.Y + _selectedEmoteLabel.Size.Y);
			_targetToggleActiveLabel.Location = new Point(_synchronizeToggleActiveLabel.Location.X, _synchronizeToggleActiveLabel.Location.Y + _synchronizeToggleActiveLabel.Size.Y);
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
			RadialContainer<Emote> selected = SelectedEmote;
			if (selected != null)
			{
				Logger.Debug("Sending command for " + selected.Value.Id);
				this.EmoteSelected(this, selected.Value);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
		}
	}
}
