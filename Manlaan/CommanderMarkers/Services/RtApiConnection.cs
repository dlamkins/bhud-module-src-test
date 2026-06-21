using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using Blish_HUD;
using Manlaan.CommanderMarkers.Presets.Model;
using Manlaan.CommanderMarkers.RtApi;
using Microsoft.Xna.Framework;

namespace Manlaan.CommanderMarkers.Services
{
	public sealed class RtApiConnection : IDisposable
	{
		private readonly object _sync = new object();

		private MemoryMappedFile? _mappedFile;

		private MemoryMappedViewAccessor? _accessor;

		private int _connectedProcessId;

		private RtApiConnectionState _state;

		public RtApiConnectionState State
		{
			get
			{
				lock (_sync)
				{
					return _state;
				}
			}
		}

		public bool IsActive => State == RtApiConnectionState.Active;

		public event EventHandler<RtApiConnectionState>? ConnectionStateChanged;

		public bool EnsureActive()
		{
			lock (_sync)
			{
				int processId = ResolveProcessId();
				if (processId <= 0)
				{
					DisconnectInternal(RtApiConnectionState.NotDetected);
					return false;
				}
				if (_mappedFile == null || _connectedProcessId != processId)
				{
					DisconnectInternal(_state);
					if (!TryConnect(processId))
					{
						DisconnectInternal(RtApiConnectionState.NotDetected);
						return false;
					}
				}
				if (!IsGameBuildActive())
				{
					DisconnectInternal(RtApiConnectionState.Inactive);
					return false;
				}
				SetState(RtApiConnectionState.Active);
				return true;
			}
		}

		public bool TryGetSquadMarkerPosition(int slotIndex, out Vector3 position)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			position = Vector3.get_Zero();
			if (slotIndex < 0 || slotIndex >= 8)
			{
				return false;
			}
			lock (_sync)
			{
				if (_accessor == null || !IsGameBuildActive())
				{
					return false;
				}
				int baseOffset = 40 + slotIndex * 12;
				float x = _accessor!.ReadSingle(baseOffset);
				float y = _accessor!.ReadSingle(baseOffset + 4);
				float z = _accessor!.ReadSingle(baseOffset + 8);
				if (!IsSquadMarkerPlaced(x, y, z))
				{
					return false;
				}
				position = RtApiCoordinates.ToGame(x, y, z);
				return true;
			}
		}

		public bool TryImportSquadMarker(int slotIndex, MarkerCoord marker)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (!EnsureActive() || !TryGetSquadMarkerPosition(slotIndex, out var position))
			{
				return false;
			}
			marker.icon = slotIndex + 1;
			marker.x = position.X;
			marker.y = position.Y;
			marker.z = position.Z;
			return true;
		}

		public void Dispose()
		{
			lock (_sync)
			{
				DisconnectInternal(RtApiConnectionState.NotDetected);
			}
		}

		private bool TryConnect(int processId)
		{
			try
			{
				_mappedFile = MemoryMappedFile.OpenExisting(RealTimeDataLayout.DataMapName(processId));
				_accessor = _mappedFile!.CreateViewAccessor(0L, 512L, MemoryMappedFileAccess.Read);
				_connectedProcessId = processId;
				return true;
			}
			catch (FileNotFoundException)
			{
				return false;
			}
			catch (UnauthorizedAccessException)
			{
				return false;
			}
		}

		private bool IsGameBuildActive()
		{
			if (_accessor == null)
			{
				return false;
			}
			return _accessor!.ReadUInt32(0L) != 0;
		}

		private void DisconnectInternal(RtApiConnectionState nextState)
		{
			_accessor?.Dispose();
			_accessor = null;
			_mappedFile?.Dispose();
			_mappedFile = null;
			_connectedProcessId = 0;
			SetState(nextState);
		}

		private void SetState(RtApiConnectionState nextState)
		{
			if (_state != nextState)
			{
				_state = nextState;
				this.ConnectionStateChanged?.Invoke(this, nextState);
			}
		}

		private static int ResolveProcessId()
		{
			try
			{
				Process gw2Process = GameService.GameIntegration.get_Gw2Instance().get_Gw2Process();
				if (gw2Process != null && !gw2Process.HasExited)
				{
					return gw2Process.Id;
				}
			}
			catch
			{
			}
			return FindProcessIdByName("Gw2-64") ?? FindProcessIdByName("Gw2").GetValueOrDefault();
		}

		private static int? FindProcessIdByName(string processName)
		{
			Process[] processesByName = Process.GetProcessesByName(processName);
			int num = 0;
			if (num < processesByName.Length)
			{
				Process process = processesByName[num];
				try
				{
					return process.Id;
				}
				finally
				{
					process.Dispose();
				}
			}
			return null;
		}

		private static bool IsSquadMarkerPlaced(float x, float y, float z)
		{
			if (float.IsInfinity(x) || float.IsInfinity(y) || float.IsInfinity(z))
			{
				return false;
			}
			if (!(Math.Abs(x) > 0.01f) && !(Math.Abs(y) > 0.01f))
			{
				return Math.Abs(z) > 0.01f;
			}
			return true;
		}
	}
}
