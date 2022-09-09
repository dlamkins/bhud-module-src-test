using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Blish_HUD;
using Gw2Sharp.Models;

namespace Nekres.Mumble_Info.Core.Services
{
	internal class MockService : IDisposable
	{
		private int _sizeLink;

		private int _sizeContext;

		private int _sizeDiscarded;

		private int _memFileLen;

		private string _filePath;

		private DateTime _lastAutoSave;

		private int _autoSaveMins;

		public MockService()
		{
			_autoSaveMins = 30;
			_lastAutoSave = DateTime.UtcNow.Subtract(new TimeSpan(_autoSaveMins, 0, 0));
			_sizeLink = Marshal.SizeOf(typeof(Link));
			_sizeContext = Marshal.SizeOf(typeof(Context));
			_sizeDiscarded = 256 - _sizeContext + 4096;
			_memFileLen = _sizeLink + _sizeContext + _sizeDiscarded;
			_filePath = Path.Combine(MumbleInfoModule.Instance.DirectoriesManager.GetFullDirectoryPath("mumble_info"), "mockme.mumble");
		}

		public void Update()
		{
			if (!(DateTime.UtcNow.Subtract(_lastAutoSave).TotalMinutes < (double)_autoSaveMins) && GameService.Gw2Mumble.get_IsAvailable())
			{
				_lastAutoSave = DateTime.UtcNow;
				if (GetData(out var data))
				{
					SaveFile(data);
				}
			}
		}

		public bool GetData(out byte[] data)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected I4, but got Unknown
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Expected I4, but got Unknown
			data = null;
			Link link2 = default(Link);
			link2.uiVersion = 1u;
			link2.uiTick = (ulong)GameService.Gw2Mumble.get_RawClient().get_Tick();
			link2.fAvatarPosition = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition().ToVector3()
				.ToArray();
			link2.fAvatarFront = GameService.Gw2Mumble.get_RawClient().get_AvatarFront().ToVector3()
				.ToArray();
			link2.fAvatarTop = Vector3.Zero.ToArray();
			link2.name = GameService.Gw2Mumble.get_RawClient().get_CharacterName();
			link2.fCameraPosition = GameService.Gw2Mumble.get_RawClient().get_CameraPosition().ToVector3()
				.ToArray();
			link2.fCameraFront = GameService.Gw2Mumble.get_RawClient().get_CameraFront().ToVector3()
				.ToArray();
			link2.fCameraTop = Vector3.Zero.ToArray();
			link2.identity = GameService.Gw2Mumble.get_RawClient().get_RawIdentity();
			link2.context_len = 48u;
			Link link = link2;
			Context context = default(Context);
			context.serverAddress = GameService.Gw2Mumble.get_RawClient().get_ServerAddress().ToCharArray()
				.Select(Convert.ToByte)
				.ToArray();
			context.mapId = (uint)GameService.Gw2Mumble.get_RawClient().get_MapId();
			context.mapType = (uint)(int)GameService.Gw2Mumble.get_RawClient().get_MapType();
			context.shardId = GameService.Gw2Mumble.get_RawClient().get_ShardId();
			context.instance = GameService.Gw2Mumble.get_RawClient().get_Instance();
			context.buildId = (uint)GameService.Gw2Mumble.get_RawClient().get_BuildId();
			context.uiState = BitmaskUtil.GetBitmask(GameService.Gw2Mumble.get_RawClient().get_IsMapOpen(), GameService.Gw2Mumble.get_RawClient().get_IsCompassTopRight(), GameService.Gw2Mumble.get_RawClient().get_IsCompassRotationEnabled(), GameService.Gw2Mumble.get_RawClient().get_DoesGameHaveFocus(), GameService.Gw2Mumble.get_RawClient().get_IsCompetitiveMode(), GameService.Gw2Mumble.get_RawClient().get_DoesAnyInputHaveFocus(), GameService.Gw2Mumble.get_RawClient().get_IsInCombat());
			Size compass = GameService.Gw2Mumble.get_RawClient().get_Compass();
			context.compassWidth = (ushort)((Size)(ref compass)).get_Width();
			compass = GameService.Gw2Mumble.get_RawClient().get_Compass();
			context.compassHeight = (ushort)((Size)(ref compass)).get_Height();
			context.compassRotation = (float)GameService.Gw2Mumble.get_RawClient().get_CompassRotation();
			Coordinates2 val = GameService.Gw2Mumble.get_RawClient().get_PlayerLocationMap();
			context.playerX = (float)((Coordinates2)(ref val)).get_X();
			val = GameService.Gw2Mumble.get_RawClient().get_PlayerLocationMap();
			context.playerY = (float)((Coordinates2)(ref val)).get_Y();
			val = GameService.Gw2Mumble.get_RawClient().get_MapCenter();
			context.mapCenterX = (float)((Coordinates2)(ref val)).get_X();
			val = GameService.Gw2Mumble.get_RawClient().get_MapCenter();
			context.mapCenterY = (float)((Coordinates2)(ref val)).get_Y();
			context.mapScale = (float)GameService.Gw2Mumble.get_RawClient().get_MapScale();
			context.processId = GameService.Gw2Mumble.get_RawClient().get_ProcessId();
			context.mountIndex = (byte)(int)GameService.Gw2Mumble.get_RawClient().get_Mount();
			Context ctx = context;
			Padding padd = default(Padding);
			if (!link.GetBytes(out var linkbuf) || !ctx.GetBytes(out var ctxbuf) || !padd.GetBytes(out var padbuf))
			{
				return false;
			}
			data = linkbuf.Concat(ctxbuf).Concat(padbuf).ToArray();
			return true;
		}

		public void SaveFile(byte[] data)
		{
			try
			{
				File.WriteAllBytes(_filePath, data);
			}
			catch (SystemException e)
			{
				MumbleInfoModule.Logger.Error((Exception)e, e.Message);
			}
		}

		public void Dispose()
		{
		}
	}
}
