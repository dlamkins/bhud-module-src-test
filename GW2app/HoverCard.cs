using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GW2app
{
	internal static class HoverCard
	{
		private enum AnimPhase
		{
			None,
			Entering,
			Exiting
		}

		private static readonly Logger Logger = Logger.GetLogger(typeof(HoverCard));

		private const int HoverDelayMs = 200;

		private const int WindowHorizontalGap = 5;

		private const int LoadingPlaceholder = 64;

		private const int AnimDurationMs = 180;

		private const float AnimStartScale = 0.95f;

		private static Action<string, int> _openCallback;

		private static Action _closeCallback;

		private static Func<float> _uiScaleGetter;

		private static Func<float> _captureScaleGetter;

		private static Func<string, int, Texture2D> _cachedTextureGetter;

		private static Func<bool> _isConnectedGetter;

		private static string _hoveredListId;

		private static int _hoveredIndex;

		private static bool _hovered;

		private static DateTime _hoverStartedAt;

		private static Control _hoveredTarget;

		private static int _anchorEntryCenterY;

		private static Control _hoveredWindow;

		private static string _openListId;

		private static int _openIndex;

		private static bool _hasOpenSub;

		private static Panel _panel;

		private static Image _image;

		private static LoadingSpinner _spinner;

		private static Texture2D _currentTex;

		private static Point _logicalSize;

		private static AnimPhase _animPhase;

		private static bool _animPending;

		private static DateTime _animStartedAt;

		private static Texture2D _pendingTex;

		private static string _pendingListId;

		private static int _pendingIndex;

		private static float SafeUiScale
		{
			get
			{
				if (_uiScaleGetter == null)
				{
					return 1f;
				}
				return _uiScaleGetter();
			}
		}

		public static void Init(Action<string, int> openCallback, Action closeCallback, Func<float> uiScaleGetter, Func<float> captureScaleGetter, Func<string, int, Texture2D> cachedTextureGetter, Func<bool> isConnectedGetter)
		{
			_openCallback = openCallback;
			_closeCallback = closeCallback;
			_uiScaleGetter = uiScaleGetter;
			_captureScaleGetter = captureScaleGetter;
			_cachedTextureGetter = cachedTextureGetter;
			_isConnectedGetter = isConnectedGetter;
		}

		public static void Attach(Control hoverTarget, string listId, int index)
		{
			hoverTarget.add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				if (_hovered && (!object.Equals(_hoveredListId, listId) || _hoveredIndex != index))
				{
					HidePanel(animated: false);
				}
				_hoveredListId = listId;
				_hoveredIndex = index;
				_hoveredTarget = hoverTarget;
				Rectangle absoluteBounds = hoverTarget.get_AbsoluteBounds();
				_anchorEntryCenterY = absoluteBounds.Y + absoluteBounds.Height / 2;
				_hoveredWindow = FindOwningWindow(hoverTarget);
				_hovered = true;
				_hoverStartedAt = DateTime.UtcNow;
			});
			hoverTarget.add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				if (_hovered && object.Equals(_hoveredListId, listId) && _hoveredIndex == index)
				{
					_hovered = false;
					_hoveredListId = null;
					_hoveredTarget = null;
					_hoveredWindow = null;
					HidePanel();
					if (_hasOpenSub)
					{
						_hasOpenSub = false;
						_openListId = null;
						_currentTex = null;
						try
						{
							_closeCallback?.Invoke();
						}
						catch (Exception ex)
						{
							Logger.Warn(ex, "close_hover send failed.");
						}
					}
				}
			});
		}

		public static void Tick()
		{
			if ((_hovered || _hasOpenSub) && HoverTargetGone())
			{
				Teardown();
				return;
			}
			if (_pendingTex != null)
			{
				if (_hasOpenSub && object.Equals(_pendingListId, _openListId) && _pendingIndex == _openIndex)
				{
					ShowTexture(_pendingTex);
				}
				_pendingTex = null;
				_pendingListId = null;
			}
			if (_hovered && (!_hasOpenSub || !object.Equals(_openListId, _hoveredListId) || _openIndex != _hoveredIndex) && (DateTime.UtcNow - _hoverStartedAt).TotalMilliseconds >= 200.0 && (_isConnectedGetter == null || _isConnectedGetter()))
			{
				OpenSubscription(_hoveredListId, _hoveredIndex);
			}
			if (_panel != null && ((Control)_panel).get_Visible())
			{
				ApplyAnimFrame();
				UpdatePosition();
			}
		}

		public static void SetImage(string listId, int index, Texture2D tex)
		{
			if (_hasOpenSub && object.Equals(_openListId, listId) && _openIndex == index)
			{
				if (_currentTex == null)
				{
					ShowTexture(tex);
					return;
				}
				_pendingTex = tex;
				_pendingListId = listId;
				_pendingIndex = index;
			}
		}

		public static void Teardown()
		{
			_hasOpenSub = false;
			_openListId = null;
			_currentTex = null;
			_animPending = false;
			_pendingTex = null;
			_pendingListId = null;
			_hovered = false;
			_hoveredTarget = null;
			_hoveredWindow = null;
			HidePanel(animated: false);
		}

		private static bool HoverTargetGone()
		{
			if (_hoveredWindow == null)
			{
				return true;
			}
			try
			{
				return _hoveredWindow.get_Parent() == null || !_hoveredWindow.get_Visible();
			}
			catch
			{
				return true;
			}
		}

		public static void RefreshScale()
		{
			if (_panel != null && ((Control)_panel).get_Visible() && _currentTex != null)
			{
				ApplyImageSize(_currentTex);
				ApplyAnimFrame();
			}
		}

		public static void Dispose()
		{
			HidePanel();
			if (_panel != null)
			{
				try
				{
					((Control)_panel).Dispose();
				}
				catch
				{
				}
				_panel = null;
			}
			_image = null;
			_spinner = null;
			_currentTex = null;
			_hovered = false;
			_hoveredTarget = null;
			_hoveredWindow = null;
			_hasOpenSub = false;
			_animPhase = AnimPhase.None;
			_animPending = false;
			_pendingTex = null;
			_pendingListId = null;
		}

		private static void OpenSubscription(string listId, int index)
		{
			_openListId = listId;
			_openIndex = index;
			_hasOpenSub = true;
			_currentTex = null;
			_animPending = true;
			_animPhase = AnimPhase.None;
			_pendingTex = null;
			_pendingListId = null;
			try
			{
				_openCallback?.Invoke(listId, index);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "open_hover send failed.");
			}
			Texture2D cached = _cachedTextureGetter?.Invoke(listId, index);
			if (cached != null)
			{
				ShowTexture(cached);
			}
			else
			{
				ShowSpinner();
			}
		}

		private static void EnsurePanel()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			if (_panel == null)
			{
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)val).set_ZIndex(1073741823);
				_panel = val;
			}
		}

		private static void ShowSpinner()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			EnsurePanel();
			if (_image != null)
			{
				try
				{
					((Control)_image).Dispose();
				}
				catch
				{
				}
				_image = null;
			}
			int side = (int)Math.Round(64f * SafeUiScale);
			_logicalSize = new Point(side, side);
			if (_spinner == null)
			{
				LoadingSpinner val = new LoadingSpinner();
				((Control)val).set_Parent((Container)(object)_panel);
				((Control)val).set_Location(Point.get_Zero());
				_spinner = val;
			}
			_currentTex = null;
			((Control)_panel).set_Visible(true);
			ApplyAnimFrame();
			UpdatePosition();
		}

		private static void ShowTexture(Texture2D tex)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			if (tex == null)
			{
				ShowSpinner();
				return;
			}
			EnsurePanel();
			if (_spinner != null)
			{
				try
				{
					((Control)_spinner).Dispose();
				}
				catch
				{
				}
				_spinner = null;
			}
			if (_image == null)
			{
				Image val = new Image(AsyncTexture2D.op_Implicit(tex));
				((Control)val).set_Parent((Container)(object)_panel);
				((Control)val).set_Location(Point.get_Zero());
				_image = val;
			}
			else
			{
				_image.set_Texture(AsyncTexture2D.op_Implicit(tex));
			}
			_currentTex = tex;
			ApplyImageSize(tex);
			if (_animPending)
			{
				_animPending = false;
				_animPhase = AnimPhase.Entering;
				_animStartedAt = DateTime.UtcNow;
			}
			((Control)_panel).set_Visible(true);
			ApplyAnimFrame();
			UpdatePosition();
		}

		private static void ApplyImageSize(Texture2D tex)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			float scale = ((_captureScaleGetter != null) ? _captureScaleGetter() : SafeUiScale);
			int w = (int)Math.Round((float)tex.get_Width() * scale);
			int h = (int)Math.Round((float)tex.get_Height() * scale);
			Point screen = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
			if (w > screen.X)
			{
				w = screen.X;
			}
			if (h > screen.Y)
			{
				h = screen.Y;
			}
			_logicalSize = new Point(w, h);
		}

		private static void ApplyAnimFrame()
		{
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			int w;
			int h;
			float opacity;
			if (_animPhase == AnimPhase.None)
			{
				w = _logicalSize.X;
				h = _logicalSize.Y;
				opacity = 1f;
			}
			else
			{
				double elapsed = (DateTime.UtcNow - _animStartedAt).TotalMilliseconds;
				float t = (float)Math.Min(1.0, elapsed / 180.0);
				float eased = 1f - (float)Math.Pow(1.0 - (double)t, 3.0);
				bool entering = _animPhase == AnimPhase.Entering;
				float progress = (entering ? eased : (1f - eased));
				float scale = 0.95f + 0.050000012f * progress;
				opacity = progress;
				w = (int)Math.Round((float)_logicalSize.X * scale);
				h = (int)Math.Round((float)_logicalSize.Y * scale);
				if (t >= 1f)
				{
					if (!entering)
					{
						_animPhase = AnimPhase.None;
						if (_panel != null)
						{
							((Control)_panel).set_Visible(false);
						}
						if (_image != null)
						{
							try
							{
								((Control)_image).Dispose();
							}
							catch
							{
							}
							_image = null;
						}
						if (_spinner != null)
						{
							try
							{
								((Control)_spinner).Dispose();
							}
							catch
							{
							}
							_spinner = null;
						}
						_currentTex = null;
						return;
					}
					_animPhase = AnimPhase.None;
					opacity = 1f;
					w = _logicalSize.X;
					h = _logicalSize.Y;
				}
			}
			((Control)_panel).set_Size(new Point(w, h));
			((Control)_panel).set_Opacity(opacity);
			if (_image != null)
			{
				((Control)_image).set_Size(new Point(w, h));
			}
			if (_spinner != null)
			{
				((Control)_spinner).set_Size(new Point(w, h));
			}
		}

		private static void HidePanel(bool animated = true)
		{
			_animPending = false;
			_pendingTex = null;
			_pendingListId = null;
			if (_panel == null || !((Control)_panel).get_Visible())
			{
				_animPhase = AnimPhase.None;
			}
			else if (!animated || _logicalSize.X == 0 || _logicalSize.Y == 0)
			{
				((Control)_panel).set_Visible(false);
				_animPhase = AnimPhase.None;
				if (_image != null)
				{
					try
					{
						((Control)_image).Dispose();
					}
					catch
					{
					}
					_image = null;
				}
				if (_spinner != null)
				{
					try
					{
						((Control)_spinner).Dispose();
					}
					catch
					{
					}
					_spinner = null;
				}
				_currentTex = null;
			}
			else if (_animPhase != AnimPhase.Exiting)
			{
				_animPhase = AnimPhase.Exiting;
				_animStartedAt = DateTime.UtcNow;
			}
		}

		private static void UpdatePosition()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			if (_panel != null && _hoveredTarget != null)
			{
				Point screen = ((Control)GameService.Graphics.get_SpriteScreen()).get_Size();
				int w = ((Control)_panel).get_Size().X;
				int h = ((Control)_panel).get_Size().Y;
				int y = _anchorEntryCenterY - h / 2;
				Rectangle windowBounds = (Rectangle)((_hoveredWindow != null) ? _hoveredWindow.get_AbsoluteBounds() : new Rectangle(0, _anchorEntryCenterY, 0, 0));
				int num = screen.X - ((Rectangle)(ref windowBounds)).get_Right();
				int leftSpace = ((Rectangle)(ref windowBounds)).get_Left();
				int x = ((num >= leftSpace) ? (((Rectangle)(ref windowBounds)).get_Right() + 5) : (((Rectangle)(ref windowBounds)).get_Left() - 5 - w));
				x = Math.Max(0, Math.Min(screen.X - w, x));
				y = Math.Max(0, Math.Min(screen.Y - h, y));
				((Control)_panel).set_Location(new Point(x, y));
			}
		}

		private static Control FindOwningWindow(Control c)
		{
			Screen screen = GameService.Graphics.get_SpriteScreen();
			Control cur = c;
			while (cur != null && cur.get_Parent() != null && cur.get_Parent() != screen)
			{
				cur = (Control)(object)cur.get_Parent();
			}
			return cur;
		}
	}
}
