using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Input;
using Flurl.Http;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ChatMacros.Core.Services.Data;
using Nekres.ChatMacros.Core.Services.Macro;
using Nekres.ChatMacros.Core.UI;
using Nekres.ChatMacros.Properties;

namespace Nekres.ChatMacros.Core.Services
{
	internal class MacroService : IDisposable
	{
		private const char COMMAND_START = '{';

		private const char COMMAND_END = '}';

		private const char PARAM_CHAR = ' ';

		private Regex _commandRegex = new Regex($"\\{'{'}(?<command>[^\\{'}'}]+)\\{'}'}", RegexOptions.Compiled);

		private Regex _paramRegex = new Regex($"{' '}(?<param>[^{' '}]+)", RegexOptions.Compiled);

		private IReadOnlyList<ContinentFloorRegionMap> _regionMaps;

		private IReadOnlyList<ContinentFloorRegionMapSector> _mapSectors;

		private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

		private ManualResetEvent _lockReleased = new ManualResetEvent(initialState: false);

		private bool _lockAcquired;

		private ContextMenuStrip _quickAccessWindow;

		public readonly FileMacroObserver Observer;

		public IReadOnlyList<Map> AllMaps { get; private set; }

		public Map CurrentMap { get; private set; }

		public ContinentFloorRegionMapSector CurrentSector { get; private set; }

		public string CurrentMapName { get; private set; }

		public ContinentFloorRegionMapPoi ClosestWaypoint { get; private set; }

		public ContinentFloorRegionMapPoi ClosestPoi { get; private set; }

		public IReadOnlyList<BaseMacro> ActiveMacros { get; private set; }

		public event EventHandler<ValueEventArgs<IReadOnlyList<BaseMacro>>> ActiveMacrosChange;

		public MacroService()
		{
			ActiveMacros = new List<BaseMacro>();
			OnMapChanged(this, new ValueEventArgs<int>(GameService.Gw2Mumble.get_CurrentMap().get_Id()));
			UpdateMacros();
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			Observer = new FileMacroObserver();
		}

		private async void OnUserLocaleChanged(object sender, ValueEventArgs<CultureInfo> e)
		{
			AllMaps = await ChatMacros.Instance.Gw2Api.GetMaps();
		}

		private void OnOpenQuickAccessActivated(object sender, EventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			if (Gw2Util.IsInGame())
			{
				if (_quickAccessWindow == null)
				{
					ContextMenuStrip val = new ContextMenuStrip();
					((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
					((Control)val).set_Visible(false);
					((Container)val).set_WidthSizingMode((SizingMode)1);
					_quickAccessWindow = val;
				}
				AddMacrosToQuickAccess(ActiveMacros.ToList());
				GameService.Content.PlaySoundEffectByName("numeric-spinner");
				if (((Control)_quickAccessWindow).get_Visible())
				{
					((Control)_quickAccessWindow).Hide();
					return;
				}
				((Control)_quickAccessWindow).set_Left(((Control)GameService.Graphics.get_SpriteScreen()).get_RelativeMousePosition().X - ((Control)_quickAccessWindow).get_Width() / 2);
				((Control)_quickAccessWindow).set_Top(((Control)GameService.Graphics.get_SpriteScreen()).get_RelativeMousePosition().Y - (int)Math.Round(0.25f * (float)((Control)_quickAccessWindow).get_Height()));
				((Control)_quickAccessWindow).Show();
			}
		}

		private void AddMacrosToQuickAccess(IReadOnlyList<BaseMacro> macros)
		{
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			if (macros.IsNullOrEmpty())
			{
				return;
			}
			foreach (Control item in ((Container)_quickAccessWindow).get_Children().ToList())
			{
				if (item != null)
				{
					item.Dispose();
				}
			}
			((Container)_quickAccessWindow).ClearChildren();
			((Control)_quickAccessWindow).set_Width(1);
			foreach (BaseMacro macro in SortMacros(ActiveMacros.ToList()))
			{
				ContextMenuStripItem<BaseMacro> contextMenuStripItem = new ContextMenuStripItem<BaseMacro>(macro);
				((Control)contextMenuStripItem).set_Parent((Container)(object)_quickAccessWindow);
				((ContextMenuStripItem)contextMenuStripItem).set_Text(AssetUtil.Truncate(macro.Title, 300, GameService.Content.get_DefaultFont14()));
				((Control)contextMenuStripItem).set_BasicTooltipText(macro.Title);
				contextMenuStripItem.FontColor = macro.GetDisplayColor();
				ContextMenuStripItem<BaseMacro> menuItem = contextMenuStripItem;
				((Control)menuItem).add_Click((EventHandler<MouseEventArgs>)async delegate
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					((Control)_quickAccessWindow).Hide();
					await Trigger(macro);
				});
				((Container)_quickAccessWindow).AddChild((Control)(object)menuItem);
			}
		}

		public IEnumerable<BaseMacro> SortMacros(List<BaseMacro> toSort)
		{
			return toSort.OrderBy(delegate(BaseMacro x)
			{
				ChatMacro chatMacro2 = x as ChatMacro;
				return (chatMacro2 != null && !chatMacro2.Lines.IsNullOrEmpty()) ? ChatMacros.Instance.LibraryConfig.get_Value().IndexChannelHistory(chatMacro2.Lines[0].Channel) : int.MaxValue;
			}).ThenBy(delegate(BaseMacro x)
			{
				ChatMacro chatMacro = x as ChatMacro;
				return (chatMacro == null) ? new ChatChannel?(ChatChannel.Current) : chatMacro.Lines?.FirstOrDefault()?.Channel;
			}).ThenBy((BaseMacro x) => x.Title.ToLowerInvariant());
		}

		public void UpdateMacros()
		{
			ToggleMacros(enabled: false);
			ContextMenuStrip quickAccessWindow = _quickAccessWindow;
			if (quickAccessWindow != null)
			{
				((Control)quickAccessWindow).Hide();
			}
			ActiveMacros = ChatMacros.Instance.Data.GetActiveMacros();
			this.ActiveMacrosChange?.Invoke(this, new ValueEventArgs<IReadOnlyList<BaseMacro>>((IReadOnlyList<BaseMacro>)ActiveMacros.ToList()));
			ToggleMacros(enabled: true);
		}

		public void ToggleMacros(bool enabled)
		{
			LockUtil.Acquire(_rwLock, _lockReleased, ref _lockAcquired);
			if (enabled)
			{
				ChatMacros.Instance.ControlsConfig.get_Value().OpenQuickAccess.add_Activated((EventHandler<EventArgs>)OnOpenQuickAccessActivated);
			}
			else
			{
				ChatMacros.Instance.ControlsConfig.get_Value().OpenQuickAccess.remove_Activated((EventHandler<EventArgs>)OnOpenQuickAccessActivated);
			}
			try
			{
				foreach (BaseMacro macro in ActiveMacros.ToList())
				{
					macro.Toggle(enabled);
					if (enabled)
					{
						macro.Triggered += OnMacroTriggered;
					}
					else
					{
						macro.Triggered -= OnMacroTriggered;
					}
				}
			}
			finally
			{
				LockUtil.Release(_rwLock, _lockReleased, ref _lockAcquired);
			}
		}

		private async void OnMacroTriggered(object sender, EventArgs e)
		{
			await Trigger((BaseMacro)sender);
		}

		public async Task Trigger(BaseMacro macro)
		{
			if (macro != null && Gw2Util.IsInGame())
			{
				ChatMacro chatMacro = macro as ChatMacro;
				if (chatMacro != null)
				{
					await Fire(chatMacro);
				}
			}
		}

		public async Task Fire(ChatMacro macro)
		{
			ToggleMacros(enabled: false);
			ChatLine firstLine = macro.Lines.FirstOrDefault();
			if (firstLine != null)
			{
				ChatMacros.Instance.LibraryConfig.get_Value().UpdateChannelHistory(firstLine.Channel);
			}
			bool isSquadbroadcastCleared = false;
			bool isChatCleared = false;
			foreach (ChatLine line in macro.Lines.ToList())
			{
				await Task.Delay(1);
				string message = await ReplaceCommands(line.ToChatMessage());
				if (string.IsNullOrEmpty(message))
				{
					message = " ";
				}
				if (line.Channel == ChatChannel.Squad && line.SquadBroadcast && GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander())
				{
					if (ChatMacros.Instance.ControlsConfig.get_Value().SquadBroadcastMessage == null || ChatMacros.Instance.ControlsConfig.get_Value().SquadBroadcastMessage.GetBindingDisplayText().Equals(string.Empty))
					{
						ScreenNotification.ShowNotification(string.Format(Resources._0__is_not_assigned_a_key_, Resources.Squad_Broadcast_Message), (NotificationType)1, (Texture2D)null, 4);
						break;
					}
					if (!isSquadbroadcastCleared)
					{
						isSquadbroadcastCleared = await ChatUtil.Clear(ChatMacros.Instance.ControlsConfig.get_Value().SquadBroadcastMessage);
					}
					await ChatUtil.Send(message, ChatMacros.Instance.ControlsConfig.get_Value().SquadBroadcastMessage, ChatMacros.Logger);
					await Task.Delay(100);
					continue;
				}
				if (ChatMacros.Instance.ControlsConfig.get_Value().ChatMessage == null || ChatMacros.Instance.ControlsConfig.get_Value().ChatMessage.GetBindingDisplayText().Equals(string.Empty))
				{
					ScreenNotification.ShowNotification(string.Format(Resources._0__is_not_assigned_a_key_, Resources.Chat_Message), (NotificationType)1, (Texture2D)null, 4);
					break;
				}
				if (!isChatCleared)
				{
					isChatCleared = await ChatUtil.Clear(ChatMacros.Instance.ControlsConfig.get_Value().ChatMessage);
				}
				if (line.Channel == ChatChannel.Whisper)
				{
					if (string.IsNullOrWhiteSpace(line.WhisperTo))
					{
						ScreenNotification.ShowNotification(Resources.Unable_to_whisper__No_recipient_specified_, (NotificationType)1, (Texture2D)null, 4);
						break;
					}
					await ChatUtil.SendWhisper(line.WhisperTo, message, ChatMacros.Instance.ControlsConfig.get_Value().ChatMessage, ChatMacros.Logger);
				}
				else
				{
					await ChatUtil.Send(message, ChatMacros.Instance.ControlsConfig.get_Value().ChatMessage, ChatMacros.Logger);
				}
			}
			await Task.Delay(200);
			ToggleMacros(enabled: true);
		}

		public void Update(GameTime gameTime)
		{
			GetClosestPoints();
		}

		private async void OnMapChanged(object sender, ValueEventArgs<int> e)
		{
			UpdateMacros();
			if (!ChatMacros.Instance.Gw2Api.IsApiAvailable())
			{
				return;
			}
			if (AllMaps.IsNullOrEmpty())
			{
				AllMaps = await ChatMacros.Instance.Gw2Api.GetMaps();
			}
			Map currentMap = AllMaps.FirstOrDefault((Map map) => map.get_Id() == e.get_Value());
			if (currentMap != null)
			{
				CurrentMap = currentMap;
				if (CurrentMap != null)
				{
					_regionMaps = await ChatMacros.Instance.Gw2Api.GetRegionMap(CurrentMap);
					_mapSectors = await ChatMacros.Instance.Gw2Api.GetMapSectors(CurrentMap);
				}
			}
		}

		private void GetClosestPoints()
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Invalid comparison between Unknown and I4
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Invalid comparison between Unknown and I4
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			if (CurrentMap == null)
			{
				return;
			}
			List<ContinentFloorRegionMapPoi> pois = _regionMaps?.Where((ContinentFloorRegionMap x) => x != null).SelectMany((ContinentFloorRegionMap x) => x.get_PointsOfInterest().Values.Distinct()).ToList();
			if (!pois.IsNullOrEmpty())
			{
				Coordinates3 continentPosition = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition().ToContinentCoords(CoordsUnit.METERS, CurrentMap.get_MapRect(), CurrentMap.get_ContinentRect());
				double closestPoiDistance = double.MaxValue;
				double closestWaypointDistance = double.MaxValue;
				ContinentFloorRegionMapPoi closestPoi = null;
				ContinentFloorRegionMapPoi closestWaypoint = null;
				foreach (ContinentFloorRegionMapPoi poi in pois)
				{
					double x2 = ((Coordinates3)(ref continentPosition)).get_X();
					Coordinates2 coord = poi.get_Coord();
					double x3 = Math.Abs(x2 - ((Coordinates2)(ref coord)).get_X());
					double z = ((Coordinates3)(ref continentPosition)).get_Z();
					coord = poi.get_Coord();
					double distanceZ = Math.Abs(z - ((Coordinates2)(ref coord)).get_Y());
					double distance = Math.Sqrt(Math.Pow(x3, 2.0) + Math.Pow(distanceZ, 2.0));
					PoiType value = poi.get_Type().get_Value();
					if ((int)value != 1)
					{
						if ((int)value == 2 && distance < closestWaypointDistance)
						{
							closestWaypointDistance = distance;
							closestWaypoint = poi;
						}
					}
					else if (distance < closestPoiDistance)
					{
						closestPoiDistance = distance;
						closestPoi = poi;
					}
				}
				ClosestWaypoint = closestWaypoint;
				ClosestPoi = closestPoi;
			}
			else
			{
				ClosestWaypoint = null;
				ClosestPoi = null;
			}
			IReadOnlyList<ContinentFloorRegionMapSector> mapSectors = _mapSectors;
			CurrentMapName = ((mapSectors != null && mapSectors.Count == 1) ? _mapSectors[0].get_Name() : CurrentMap.get_Name());
			Coordinates2 playerLocation = GameService.Gw2Mumble.get_RawClient().get_AvatarPosition().ToContinentCoords(CoordsUnit.METERS, CurrentMap.get_MapRect(), CurrentMap.get_ContinentRect())
				.SwapYz()
				.ToPlane();
			CurrentSector = _mapSectors?.FirstOrDefault((ContinentFloorRegionMapSector sector) => playerLocation.Inside(sector.get_Bounds()));
		}

		public async Task<string> ReplaceCommands(string text)
		{
			MatchCollection matches = _commandRegex.Matches(text);
			string result = text;
			foreach (Match match in matches)
			{
				string command = match.Groups["command"].Value;
				string replacement = await Resolve(command);
				if (string.IsNullOrWhiteSpace(replacement))
				{
					return string.Empty;
				}
				result = result.Replace($"{'{'}{command}{'}'}", replacement);
			}
			return result;
		}

		private async Task<string> Resolve(string fullCommand)
		{
			MatchCollection matchCollection = _paramRegex.Matches(fullCommand);
			List<string> args = new List<string>();
			foreach (Match item in matchCollection)
			{
				string arg = item.Groups["param"].Value;
				args.Add(arg);
			}
			int paramsStart = fullCommand.IndexOf(' ');
			string command = ((paramsStart < 0) ? fullCommand : fullCommand.Substring(0, paramsStart));
			return await Exec(command, args);
		}

		private async Task<string> Exec(string command, IReadOnlyList<string> args)
		{
			if (command == null)
			{
				goto IL_02fc;
			}
			switch (command.Length)
			{
			case 5:
				break;
			case 4:
				goto IL_006c;
			case 3:
				goto IL_008a;
			case 6:
				goto IL_00b4;
			case 2:
				goto IL_012d;
			default:
				goto IL_02fc;
			}
			char c = command[0];
			string result;
			if (c != 'b')
			{
				if (c != 't' || !(command == "today"))
				{
					goto IL_02fc;
				}
				result = DateTime.Now.ToString("dddd, d.M.yyyy", CultureInfo.CurrentUICulture);
			}
			else
			{
				if (!(command == "blish"))
				{
					goto IL_02fc;
				}
				result = GetVersion();
			}
			goto IL_0302;
			IL_008a:
			c = command[0];
			if (c != 'm')
			{
				if (c != 'p')
				{
					if (c != 't' || !(command == "txt"))
					{
						goto IL_02fc;
					}
					result = ReadTextFile(args);
				}
				else
				{
					if (!(command == "poi"))
					{
						goto IL_02fc;
					}
					ContinentFloorRegionMapPoi closestPoi = ClosestPoi;
					result = ((closestPoi != null) ? closestPoi.get_ChatLink() : null) ?? string.Empty;
				}
			}
			else
			{
				if (!(command == "map"))
				{
					goto IL_02fc;
				}
				result = CurrentMapName ?? string.Empty;
			}
			goto IL_0302;
			IL_012d:
			if (!(command == "wp"))
			{
				goto IL_02fc;
			}
			ContinentFloorRegionMapPoi closestWaypoint = ClosestWaypoint;
			result = ((closestWaypoint != null) ? closestWaypoint.get_ChatLink() : null) ?? string.Empty;
			goto IL_0302;
			IL_006c:
			c = command[0];
			if (c != 'j')
			{
				if (c != 't' || !(command == "time"))
				{
					goto IL_02fc;
				}
				result = DateTime.Now.ToString("HH:mm", CultureInfo.CurrentUICulture);
			}
			else
			{
				if (!(command == "json"))
				{
					goto IL_02fc;
				}
				result = await GetJson(args);
			}
			goto IL_0302;
			IL_02fc:
			result = string.Empty;
			goto IL_0302;
			IL_0302:
			return result;
			IL_00b4:
			c = command[0];
			if (c != 'r')
			{
				if (c != 's' || !(command == "sector"))
				{
					goto IL_02fc;
				}
				ContinentFloorRegionMapSector currentSector = CurrentSector;
				result = ((currentSector != null) ? currentSector.get_Name() : null) ?? string.Empty;
			}
			else
			{
				if (!(command == "random"))
				{
					goto IL_02fc;
				}
				result = GetRandom(args).ToString();
			}
			goto IL_0302;
		}

		private string ReadTextFile(IReadOnlyList<string> args)
		{
			if (args.Count == 0)
			{
				return string.Empty;
			}
			if (!FileUtil.TryReadAllLines(args[0] ?? string.Empty, out var lines, ChatMacros.Logger, ChatMacros.Instance.BasePaths.ToArray()))
			{
				return string.Empty;
			}
			int line = RandomUtil.GetRandom(0, lines.Count - 1);
			if (args.Count == 2 && int.TryParse(args[1], out var lineArg))
			{
				line = ((lineArg <= lines.Count && lineArg > 0) ? (lineArg - 1) : line);
			}
			return lines[line];
		}

		private string GetVersion()
		{
			string version = typeof(BlishHud).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
			if (!string.IsNullOrEmpty(version))
			{
				return "Blish HUD v" + version.Split('+').First();
			}
			return string.Empty;
		}

		private int GetRandom(IReadOnlyList<string> args)
		{
			int max = int.MaxValue;
			int min = 0;
			if (args.Count > 0)
			{
				if (args.Count == 1)
				{
					int.TryParse(args[0], out max);
				}
				else if (args.Count == 2)
				{
					int.TryParse(args[0], out min);
					int.TryParse(args[1], out max);
				}
			}
			return RandomUtil.GetRandom(min, max);
		}

		private async Task<string> GetJson(IReadOnlyList<string> args)
		{
			if (args.Count < 2)
			{
				return string.Empty;
			}
			string url = args[1];
			if (!url.IsWebLink())
			{
				return string.Empty;
			}
			string json = await HttpUtil.TryAsync(() => GeneratedExtensions.GetStringAsync(url, default(CancellationToken), (HttpCompletionOption)0));
			string path = args[0];
			return JsonPropertyUtil.GetPropertyFromJson(json, path);
		}

		public bool TryImportFromFile(string filePath, out IReadOnlyList<ChatLine> messages)
		{
			List<ChatLine> msgs = (List<ChatLine>)(messages = new List<ChatLine>());
			if (!FileUtil.TryReadAllLines(filePath, out var lines, ChatMacros.Logger, ChatMacros.Instance.BasePaths.ToArray()))
			{
				return false;
			}
			msgs.AddRange(lines.Select(ChatLine.Parse));
			return true;
		}

		public void Dispose()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			ToggleMacros(enabled: false);
			Observer.Dispose();
			if (_lockAcquired)
			{
				_lockReleased.WaitOne(500);
			}
			_lockReleased.Dispose();
			ContextMenuStrip quickAccessWindow = _quickAccessWindow;
			if (quickAccessWindow != null)
			{
				((Control)quickAccessWindow).Dispose();
			}
			try
			{
				_rwLock.Dispose();
			}
			catch (Exception ex)
			{
				ChatMacros.Logger.Debug(ex, ex.Message);
			}
		}
	}
}
