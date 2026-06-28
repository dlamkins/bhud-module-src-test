using System;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public sealed class TutorialWaypointMapOverlay : Control, IDisposable
	{
		public const float VicinityDistanceMeters = 30f;

		public const float VicinityDistanceMetersSquared = 900f;

		private const float RingPulseSpeed = 3f;

		private readonly TutorialHint _hint;

		private readonly AsyncTexture2D _mapIcon;

		private Map? _map;

		private bool _mapRequested;

		private double _animationTime;

		private bool _hiddenByProximity;

		private bool _notifiedCrossZone;

		public event EventHandler? HiddenByProximity;

		public TutorialWaypointMapOverlay(TutorialHint hint)
			: this()
		{
			_hint = hint;
			_mapIcon = Service.Textures.DatAsset(157122);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_ZIndex(2147483636);
			RequestWaypointMap();
		}

		public void CopyChatCodeToClipboard()
		{
			if (!string.IsNullOrWhiteSpace(_hint.ChatCode))
			{
				try
				{
					Clipboard.SetText(_hint.ChatCode);
					ScreenNotification.ShowNotification("Copied " + _hint.ChatCode + " to clipboard", (NotificationType)0, (Texture2D)null, 4);
				}
				catch (Exception ex)
				{
					Logger.GetLogger<Module>().Warn(ex, "Failed to copy tutorial waypoint chat code");
				}
			}
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).DoUpdate(gameTime);
			_animationTime += gameTime.get_ElapsedGameTime().TotalSeconds;
			((Control)this).set_Width(((Control)GameService.Graphics.get_SpriteScreen()).get_Width());
			((Control)this).set_Height(((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			((Control)this).set_Location(Point.get_Zero());
			((Control)this).set_Visible(true);
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			bool onWaypointMap = GameService.Gw2Mumble.get_CurrentMap().get_Id() == _hint.MapId;
			RequestWaypointMap();
			if (onWaypointMap)
			{
				if (IsWithinVicinity(GameService.Gw2Mumble.get_PlayerCharacter().get_Position(), _hint.ToVector3()))
				{
					if (!_hiddenByProximity)
					{
						_hiddenByProximity = true;
						this.HiddenByProximity?.Invoke(this, EventArgs.Empty);
					}
					return;
				}
				_hiddenByProximity = false;
			}
			MaybeNotifyCrossZone(onWaypointMap, GameService.Gw2Mumble.get_UI().get_IsMapOpen());
		}

		private bool ShouldRender()
		{
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return false;
			}
			if (_map == null && !_hint.TryGetContinent(out var _))
			{
				return false;
			}
			bool onWaypointMap = GameService.Gw2Mumble.get_CurrentMap().get_Id() == _hint.MapId;
			bool mapOpen = GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			if (!onWaypointMap && !mapOpen)
			{
				return false;
			}
			if (onWaypointMap && IsWithinVicinity(GameService.Gw2Mumble.get_PlayerCharacter().get_Position(), _hint.ToVector3()))
			{
				return false;
			}
			return true;
		}

		private static bool IsWithinVicinity(Vector3 player, Vector3 target)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			float num = player.X - target.X;
			float dy = player.Y - target.Y;
			return num * num + dy * dy <= 900f;
		}

		private void RequestWaypointMap()
		{
			if (!_mapRequested)
			{
				_mapRequested = true;
				MapDataCache.Request(_hint.MapId, delegate(Map? map)
				{
					_map = map;
				});
			}
		}

		private void MaybeNotifyCrossZone(bool onWaypointMap, bool mapOpen)
		{
			if (!(_notifiedCrossZone || onWaypointMap))
			{
				_notifiedCrossZone = true;
				string label = (string.IsNullOrWhiteSpace(_hint.Label) ? "the puzzle zone" : _hint.Label);
				ScreenNotification.ShowNotification(mapOpen ? ("Waypoint is in another zone\nPan the world map to " + label + ".") : ("Waypoint is in another zone\nopen the world map and pan to " + label + "."), (NotificationType)0, (Texture2D)null, 6);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			if (!ShouldRender())
			{
				return;
			}
			float pulse = (float)(Math.Sin(_animationTime * 3.0) * 0.5 + 0.5);
			if (CompassMapUtils.TryGetScreenPosition(_hint, _map, out var compass))
			{
				Color markerColor = (compass.IsCrossZone ? Color.get_Orange() : Color.get_LightGreen());
				TutorialMarkerDraw.DrawPulsingMarker(spriteBatch, _mapIcon, compass.Screen, pulse, markerColor);
				string label = _hint.Label;
				if (compass.IsCrossZone && !string.IsNullOrWhiteSpace(label))
				{
					label += " (other zone)";
				}
				if (!string.IsNullOrWhiteSpace(label))
				{
					Rectangle labelBounds = default(Rectangle);
					((Rectangle)(ref labelBounds))._002Ector((int)compass.Screen.X - 90, (int)compass.Screen.Y + 16, 180, 20);
					SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, label, GameService.Content.get_DefaultFont12(), labelBounds, Color.get_White(), false, (HorizontalAlignment)1, (VerticalAlignment)1);
				}
			}
		}

		public void Dispose()
		{
			((Control)this).DisposeControl();
		}

		protected override void DisposeControl()
		{
			((Control)this).set_Parent((Container)null);
			((Control)this).DisposeControl();
		}
	}
}
