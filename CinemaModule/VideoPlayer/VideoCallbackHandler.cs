using System;
using System.Runtime.InteropServices;
using Blish_HUD;

namespace CinemaModule.VideoPlayer
{
	internal class VideoCallbackHandler
	{
		private static readonly Logger Logger = Logger.GetLogger<VideoCallbackHandler>();

		private readonly VideoBuffer _buffer;

		private volatile bool _frameDirty;

		private volatile bool _lockFailed;

		public bool IsFrameDirty => _frameDirty;

		public bool LockFailed => _lockFailed;

		public VideoCallbackHandler(VideoBuffer buffer)
		{
			_buffer = buffer;
		}

		public IntPtr LockCallback(IntPtr opaque, IntPtr planes)
		{
			IntPtr bufferPtr = _buffer.BufferPtr;
			if (bufferPtr == IntPtr.Zero)
			{
				_lockFailed = true;
				return IntPtr.Zero;
			}
			_lockFailed = false;
			Marshal.WriteIntPtr(planes, bufferPtr);
			return IntPtr.Zero;
		}

		public void DisplayCallback(IntPtr opaque, IntPtr picture)
		{
			if (!_lockFailed)
			{
				_frameDirty = true;
			}
		}

		public void ClearFrameDirty()
		{
			_frameDirty = false;
		}

		public void Reset()
		{
			_frameDirty = false;
			_lockFailed = false;
		}
	}
}
