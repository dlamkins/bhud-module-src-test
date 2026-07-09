using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rp.spark.UI
{
	internal class WindowBuilder
	{
		private sealed class EmblemBinding : IDisposable
		{
			private readonly WindowBase2 _window;

			private readonly AsyncTexture2D _emblem;

			private EmblemBinding(WindowBase2 window, AsyncTexture2D emblem)
			{
				_window = window;
				_emblem = emblem;
			}

			public static EmblemBinding Attach(WindowBase2 window, AsyncTexture2D emblem)
			{
				EmblemBinding binding = new EmblemBinding(window, emblem);
				window.set_Emblem(emblem.get_Texture());
				if (!emblem.get_HasSwapped())
				{
					emblem.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)binding.HandleTextureSwapped);
				}
				return binding;
			}

			public void Dispose()
			{
				_emblem.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)HandleTextureSwapped);
			}

			private void HandleTextureSwapped(object sender, ValueChangedEventArgs<Texture2D> e)
			{
				_window.set_Emblem(e.get_NewValue());
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<WindowBuilder>();

		private const int WindowBackgroundAssetId = 155985;

		private const int WindowEmblemAssetId = 3307061;

		private const int TabbedWindowContentX = 96;

		private const int TabbedWindowContentY = 22;

		private const int TabbedWindowContentWidth = 783;

		private const int TabbedWindowContentBottom = 676;

		private static readonly Rectangle StandardWindowBounds = new Rectangle(40, 26, 913, 691);

		private static readonly Rectangle TabbedWindowContentBounds = new Rectangle(96, 22, 783, 654);

		private readonly Dictionary<int, AsyncTexture2D> _assetIcons = new Dictionary<int, AsyncTexture2D>();

		private readonly Dictionary<WindowBase2, EmblemBinding> _emblemBindings = new Dictionary<WindowBase2, EmblemBinding>();

		private AsyncTexture2D _windowBackground;

		private AsyncTexture2D _windowEmblem;

		public TabbedWindow2 MakeTabbedWindow(string subtitle, string id)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected O, but got Unknown
			TabbedWindow2 val = new TabbedWindow2(GetWindowBackground(), StandardWindowBounds, TabbedWindowContentBounds);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("SPARK");
			((WindowBase2)val).set_Subtitle(subtitle);
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id(id);
			TabbedWindow2 window = val;
			AttachEmblem((WindowBase2)(object)window);
			return window;
		}

		public StandardWindow MakeWindow(string subtitle, string id, Rectangle contentBounds)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			StandardWindow val = new StandardWindow(GetWindowBackground(), StandardWindowBounds, contentBounds);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("SPARK");
			((WindowBase2)val).set_Subtitle(subtitle);
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id(id);
			StandardWindow window = val;
			AttachEmblem((WindowBase2)(object)window);
			return window;
		}

		public StandardWindow MakeWindow(string subtitle, string id, Rectangle windowBounds, Rectangle contentBounds)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			StandardWindow val = new StandardWindow(GetWindowBackground(), windowBounds, contentBounds);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("SPARK");
			((WindowBase2)val).set_Subtitle(subtitle);
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id(id);
			StandardWindow window = val;
			AttachEmblem((WindowBase2)(object)window);
			return window;
		}

		public SparkCompactWindow MakeCompactWindow(string title, Point size)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new SparkCompactWindow(title, GetWindowBackground(), StandardWindowBounds, size);
		}

		public AsyncTexture2D IconFromAsset(int assetId)
		{
			if (assetId <= 0)
			{
				return null;
			}
			if (_assetIcons.TryGetValue(assetId, out var icon))
			{
				return icon;
			}
			icon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(assetId);
			_assetIcons[assetId] = icon;
			return icon;
		}

		public void DisposeWindow(WindowBase2 window)
		{
			if (window != null)
			{
				try
				{
					ReleaseEmblem(window);
					((Control)window).Dispose();
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "SPARK failed to dispose a window.");
				}
			}
		}

		public void Clear()
		{
			foreach (EmblemBinding value in _emblemBindings.Values)
			{
				value.Dispose();
			}
			_emblemBindings.Clear();
			_assetIcons.Clear();
			_windowBackground = null;
			_windowEmblem = null;
		}

		private AsyncTexture2D GetWindowBackground()
		{
			return _windowBackground ?? (_windowBackground = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985));
		}

		private AsyncTexture2D GetWindowEmblem()
		{
			return _windowEmblem ?? (_windowEmblem = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(3307061));
		}

		private void AttachEmblem(WindowBase2 window)
		{
			AsyncTexture2D emblem = GetWindowEmblem();
			if (window != null && emblem != null)
			{
				ReleaseEmblem(window);
				_emblemBindings[window] = EmblemBinding.Attach(window, emblem);
			}
		}

		private void ReleaseEmblem(WindowBase2 window)
		{
			if (window != null && _emblemBindings.TryGetValue(window, out var binding))
			{
				binding.Dispose();
				_emblemBindings.Remove(window);
			}
		}
	}
}
