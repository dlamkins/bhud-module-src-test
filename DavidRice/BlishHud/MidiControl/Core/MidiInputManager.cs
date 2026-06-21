using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Blish_HUD;
using NAudio.Midi;

namespace DavidRice.BlishHud.MidiControl.Core
{
	public sealed class MidiInputManager : IDisposable
	{
		private static readonly Logger Logger = Logger.GetLogger<MidiInputManager>();

		private readonly ConcurrentQueue<MidiNoteEvent> _noteQueue;

		private MidiIn? _midiIn;

		private string? _activeDeviceName;

		private bool _disposed;

		private bool _retryingConnection;

		private DateTime _lastConnectionCheck = DateTime.MinValue;

		private static readonly TimeSpan ConnectionCheckInterval = TimeSpan.FromSeconds(10.0);

		public string? ActiveDeviceName => _activeDeviceName;

		public bool IsDeviceOpen => _midiIn != null;

		public bool IsRetryingConnection => _retryingConnection;

		public static IReadOnlyList<string> AvailableDevices
		{
			get
			{
				List<string> devices = new List<string>();
				for (int i = 0; i < MidiIn.NumberOfDevices; i++)
				{
					devices.Add(MidiIn.DeviceInfo(i).ProductName);
				}
				return devices;
			}
		}

		public MidiInputManager(ConcurrentQueue<MidiNoteEvent> noteQueue)
		{
			_noteQueue = noteQueue ?? throw new ArgumentNullException("noteQueue");
		}

		public static int? GetDeviceIndex(string deviceName)
		{
			for (int i = 0; i < MidiIn.NumberOfDevices; i++)
			{
				if (MidiIn.DeviceInfo(i).ProductName == deviceName)
				{
					return i;
				}
			}
			return null;
		}

		public bool Open(string deviceName)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("MidiInputManager");
			}
			Close();
			int? index = GetDeviceIndex(deviceName);
			if (!index.HasValue)
			{
				Logger.Warn("MIDI device '" + deviceName + "' not found.");
				return false;
			}
			try
			{
				_midiIn = new MidiIn(index.Value);
				_midiIn!.MessageReceived += OnMessageReceived;
				_midiIn!.ErrorReceived += OnErrorReceived;
				_midiIn!.Start();
				_activeDeviceName = deviceName;
				_retryingConnection = false;
				Logger.Info("Opened MIDI device: " + deviceName);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to open MIDI device '" + deviceName + "'.", new object[1] { ex });
				_midiIn?.Dispose();
				_midiIn = null;
				return false;
			}
		}

		public void Close()
		{
			if (_midiIn == null)
			{
				return;
			}
			try
			{
				_midiIn!.MessageReceived -= OnMessageReceived;
				_midiIn!.ErrorReceived -= OnErrorReceived;
				_midiIn!.Stop();
				_midiIn!.Dispose();
			}
			catch (Exception ex)
			{
				Logger.Warn("Exception while closing MIDI device.", new object[1] { ex });
			}
			finally
			{
				_midiIn = null;
				_activeDeviceName = null;
			}
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				_retryingConnection = false;
				Close();
			}
		}

		public void CheckConnection(string targetDeviceName)
		{
			if (_disposed || string.IsNullOrEmpty(targetDeviceName))
			{
				return;
			}
			DateTime now = DateTime.UtcNow;
			if (now - _lastConnectionCheck < ConnectionCheckInterval)
			{
				return;
			}
			_lastConnectionCheck = now;
			IReadOnlyList<string> available = AvailableDevices;
			switch (ConnectionEvaluator.Evaluate(targetDeviceName, available, IsDeviceOpen))
			{
			case ConnectionEvaluator.Action.Close:
				Logger.Warn("MIDI device '" + targetDeviceName + "' disappeared. Will attempt to reconnect.");
				_retryingConnection = true;
				Close();
				break;
			case ConnectionEvaluator.Action.Reopen:
				if (Open(targetDeviceName))
				{
					Logger.Info("MIDI device '" + targetDeviceName + "' reconnected.");
				}
				else
				{
					Logger.Warn("MIDI device '" + targetDeviceName + "' reappeared but failed to open.");
				}
				break;
			}
		}

		private void OnMessageReceived(object? sender, MidiInMessageEventArgs e)
		{
			MidiNoteEvent? noteEvent = MidiEventConverter.TryConvertToMidiNoteEvent(e.MidiEvent);
			if (noteEvent.HasValue)
			{
				_noteQueue.Enqueue(noteEvent.Value);
			}
		}

		private void OnErrorReceived(object? sender, MidiInMessageEventArgs e)
		{
			Logger.Warn($"MIDI error: raw=0x{e.RawMessage:X8} timestamp={e.Timestamp}");
		}
	}
}
