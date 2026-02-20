using System;
using System.Runtime.InteropServices;
using Blish_HUD;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.VideoPlayer
{
	internal class VideoBuffer : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<VideoBuffer>();

		private readonly object _lock = new object();

		private byte[] _rawBuffer;

		private byte[] _textureBuffer;

		private GCHandle _bufferHandle;

		private bool _isAllocated;

		private volatile IntPtr _bufferPtr;

		public IntPtr BufferPtr => _bufferPtr;

		public void Allocate(int size)
		{
			if (size <= 0)
			{
				Logger.Warn($"Allocate called with invalid size: {size}");
				return;
			}
			lock (_lock)
			{
				byte[] newBuffer = new byte[size];
				GCHandle newHandle = GCHandle.Alloc(newBuffer, GCHandleType.Pinned);
				IntPtr newPtr = newHandle.AddrOfPinnedObject();
				if (_bufferHandle.IsAllocated)
				{
					_bufferHandle.Free();
				}
				_rawBuffer = newBuffer;
				_bufferHandle = newHandle;
				_bufferPtr = newPtr;
				_textureBuffer = null;
				_isAllocated = true;
				Logger.Debug($"Buffer allocated: {size} bytes, ptr={newPtr}");
			}
		}

		public void Free()
		{
			lock (_lock)
			{
				_bufferPtr = IntPtr.Zero;
				if (_bufferHandle.IsAllocated)
				{
					_bufferHandle.Free();
				}
				_rawBuffer = null;
				_textureBuffer = null;
				_isAllocated = false;
			}
		}

		public void CopyToTextureWithPitch(Texture2D texture, int sourceWidth, int sourceHeight, uint pitch)
		{
			if (texture == null || ((GraphicsResource)texture).get_IsDisposed())
			{
				return;
			}
			int textureRowBytes;
			int copyHeight;
			int copyRowBytes;
			byte[] localRawBuffer;
			byte[] localTextureBuffer;
			bool needsPitchConversion;
			lock (_lock)
			{
				if (_rawBuffer == null || !_isAllocated)
				{
					return;
				}
				int width = texture.get_Width();
				int textureHeight = texture.get_Height();
				textureRowBytes = width * 4;
				int expectedSize = textureRowBytes * textureHeight;
				int num = Math.Min(width, sourceWidth);
				copyHeight = Math.Min(textureHeight, sourceHeight);
				copyRowBytes = num * 4;
				if (_textureBuffer == null || _textureBuffer.Length != expectedSize)
				{
					_textureBuffer = new byte[expectedSize];
				}
				localRawBuffer = _rawBuffer;
				localTextureBuffer = _textureBuffer;
				needsPitchConversion = pitch != (uint)textureRowBytes;
			}
			if (needsPitchConversion)
			{
				int num2 = copyHeight * (int)pitch;
				int maxDstOffset = copyHeight * textureRowBytes;
				if (num2 <= localRawBuffer.Length && maxDstOffset <= localTextureBuffer.Length)
				{
					for (int row = 0; row < copyHeight; row++)
					{
						Buffer.BlockCopy(localRawBuffer, row * (int)pitch, localTextureBuffer, row * textureRowBytes, copyRowBytes);
					}
				}
			}
			else
			{
				int totalBytes = copyHeight * textureRowBytes;
				if (totalBytes <= localRawBuffer.Length && totalBytes <= localTextureBuffer.Length)
				{
					Buffer.BlockCopy(localRawBuffer, 0, localTextureBuffer, 0, totalBytes);
				}
			}
			try
			{
				texture.SetData<byte>(localTextureBuffer);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		public void Dispose()
		{
			Free();
		}
	}
}
