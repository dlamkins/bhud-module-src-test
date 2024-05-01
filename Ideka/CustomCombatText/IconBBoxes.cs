using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.CustomCombatText
{
	internal class IconBBoxes : IDisposable
	{
		private readonly Dictionary<int, Rectangle?> _iconBBoxes = new Dictionary<int, Rectangle?>();

		private readonly Queue<int> _textureQueue = new Queue<int>();

		private CancellationTokenSource? _cts;

		private readonly object _lock = new object();

		public IconBBoxes()
		{
			CTextModule.Settings.MinIconMargin.OnChanged(delegate
			{
				_iconBBoxes.Clear();
			});
			CTextModule.Settings.AutocropTolerance.OnChanged(delegate
			{
				_iconBBoxes.Clear();
			});
		}

		public Rectangle? GetIconBBox(int? assetId)
		{
			if (!assetId.HasValue)
			{
				return null;
			}
			if (_iconBBoxes.TryGetValue(assetId.Value, out var bbox))
			{
				return bbox;
			}
			lock (_lock)
			{
				_textureQueue.Enqueue(assetId.Value);
			}
			if (_cts?.IsCancellationRequested ?? true)
			{
				_cts = new CancellationTokenSource();
				ProcessQueue(_cts!.Token);
			}
			return null;
		}

		public async Task<Rectangle> GetIconBBoxAsync(int assetId, CancellationToken ct)
		{
			Rectangle? bbox;
			while (true)
			{
				bbox = GetIconBBox(assetId);
				if (bbox.HasValue)
				{
					break;
				}
				await Task.Delay(100, ct);
			}
			return bbox.Value;
		}

		private async Task ProcessQueue(CancellationToken ct)
		{
			try
			{
				AsyncTexture2D asyncTexture = default(AsyncTexture2D);
				while (true)
				{
					int? num = getNext();
					if (!num.HasValue)
					{
						break;
					}
					int next = num.GetValueOrDefault();
					if (_iconBBoxes.ContainsKey(next))
					{
						continue;
					}
					if (!AsyncTexture2D.TryFromAssetId(next, ref asyncTexture))
					{
						_iconBBoxes[next] = null;
						continue;
					}
					await Task.WhenAny(((Func<Task>)async delegate
					{
						while (!asyncTexture.get_HasSwapped())
						{
							await Task.Delay(TimeSpan.FromMilliseconds(100.0));
							ct.ThrowIfCancellationRequested();
						}
					})(), Task.Delay(TimeSpan.FromSeconds(10.0), ct));
					Texture2D texture = asyncTexture.get_Texture();
					if (texture != null)
					{
						if (texture.get_Width() < 40 || texture.get_Height() < 40)
						{
							_iconBBoxes[next] = null;
							break;
						}
						using DirectBitmap im = new DirectBitmap(BitmapUtils.FromTexture(texture));
						Rectangle bbox = BitmapUtils.GetBoundingBox(im, im.GetPixel(0, 0), CTextModule.Settings.AutocropTolerance.Value);
						int margin = (int)((double)(CTextModule.Settings.MinIconMargin.Value * Math.Max(bbox.Width, bbox.Height)) / 64.0);
						int left = Math.Max(bbox.X - margin, 0);
						int top = Math.Max(bbox.Y - margin, 0);
						bbox = new Rectangle(left, top, Math.Min(bbox.Width + margin * 2, texture.get_Width() - left - 1), Math.Min(bbox.Height + margin * 2, texture.get_Height() - top - 1));
						_iconBBoxes[next] = bbox;
					}
				}
			}
			finally
			{
				_cts?.Dispose();
				_cts = null;
			}
			int? getNext()
			{
				lock (_lock)
				{
					return (!_textureQueue.Any()) ? null : new int?(_textureQueue.Dequeue());
				}
			}
		}

		public void Dispose()
		{
			_cts?.Dispose();
			_cts = null;
		}
	}
}
