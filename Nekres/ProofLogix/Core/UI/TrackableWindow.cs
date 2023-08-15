using System;
using System.Collections.Concurrent;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.ProofLogix.Core.UI
{
	internal class TrackableWindow : StandardWindow
	{
		private static ConcurrentDictionary<string, TrackableWindow> _windows;

		private readonly string _trackId;

		public static bool TryGetById(string id, out TrackableWindow wnd)
		{
			ValidateDictionary();
			return _windows.TryGetValue(id, out wnd);
		}

		public static void Unset()
		{
			if (_windows == null)
			{
				return;
			}
			foreach (TrackableWindow value in _windows.Values)
			{
				if (value != null)
				{
					((Control)value).Hide();
				}
			}
			_windows.Clear();
			_windows = null;
		}

		private static void ValidateDictionary()
		{
			if (_windows == null)
			{
				_windows = new ConcurrentDictionary<string, TrackableWindow>();
			}
		}

		public TrackableWindow(string id, AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			ValidateDictionary();
			_trackId = id ?? string.Empty;
			_windows.TryAdd(_trackId, this);
		}

		public TrackableWindow(string id, Texture2D background, Rectangle windowRegion, Rectangle contentRegion)
			: this(background, windowRegion, contentRegion)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			ValidateDictionary();
			_trackId = id ?? string.Empty;
			_windows.TryAdd(_trackId, this);
		}

		public TrackableWindow(string id, AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(background, windowRegion, contentRegion, windowSize)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			ValidateDictionary();
			_trackId = id ?? string.Empty;
			_windows.TryAdd(_trackId, this);
		}

		public TrackableWindow(string id, Texture2D background, Rectangle windowRegion, Rectangle contentRegion, Point windowSize)
			: this(background, windowRegion, contentRegion, windowSize)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			ValidateDictionary();
			_trackId = id ?? string.Empty;
			_windows.TryAdd(_trackId, this);
		}

		protected override void OnHidden(EventArgs e)
		{
			((Control)this).Dispose();
			((Control)this).OnHidden(e);
		}

		protected override void DisposeControl()
		{
			_windows?.TryRemove(_trackId ?? string.Empty, out var _);
			((WindowBase2)this).DisposeControl();
		}
	}
}
