using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nekres.Mumble_Info_Module
{
	[Export(typeof(Module))]
	public class MumbleInfoModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(MumbleInfoModule));

		internal static MumbleInfoModule ModuleInstance;

		private SettingEntry<KeyBinding> _toggleInfoBinding;

		private SettingEntry<bool> _showCursorPosition;

		internal SettingEntry<bool> EnablePerformanceCounters;

		private DataPanel _dataPanel;

		private Label _cursorPos;

		private PerformanceCounter _ramCounter;

		private PerformanceCounter _cpuCounter;

		private DateTime _timeOutPc;

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal Map CurrentMap { get; private set; }

		internal Specialization CurrentSpec { get; private set; }

		internal float MemoryUsage { get; private set; }

		internal float CpuUsage { get; private set; }

		internal string CpuName { get; private set; }

		[ImportingConstructor]
		public MumbleInfoModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			_toggleInfoBinding = settings.DefineSetting<KeyBinding>("ToggleInfoBinding", new KeyBinding((Keys)187), (Func<string>)(() => "Toggle display"), (Func<string>)(() => "Toggles the display of data."));
			_showCursorPosition = settings.DefineSetting<bool>("ShowCursorPosition", false, (Func<string>)(() => "Show cursor position"), (Func<string>)(() => "Whether the cursor's current interface-relative position should be displayed.\nUse [Left Alt] to copy it."));
			EnablePerformanceCounters = settings.DefineSetting<bool>("PerfCountersEnabled", false, (Func<string>)(() => "Show performance counters"), (Func<string>)(() => "Whether performance counters such as RAM and CPU utilization of the Guild Wars 2 process should be displayed."));
		}

		protected override void Initialize()
		{
			_timeOutPc = DateTime.UtcNow;
			CpuName = string.Empty;
		}

		protected override async Task LoadAsync()
		{
			if (EnablePerformanceCounters.get_Value())
			{
				await LoadPerformanceCounters();
				await QueryManagementObjects();
			}
		}

		protected override void Update(GameTime gameTime)
		{
			UpdateCounter();
			UpdateCursorPos();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			_toggleInfoBinding.get_Value().set_Enabled(true);
			_toggleInfoBinding.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleInfoBindingActivated);
			_showCursorPosition.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowCursorPositionSettingChanged);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)OnSpecializationChanged);
			GameService.GameIntegration.get_Gw2Instance().add_Gw2Closed((EventHandler<EventArgs>)OnGw2Closed);
			GameService.GameIntegration.get_Gw2Instance().add_Gw2Started((EventHandler<EventArgs>)OnGw2Started);
			EnablePerformanceCounters.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnEnablePerformanceCountersSettingChanged);
			((Module)this).OnModuleLoaded(e);
		}

		private Task LoadPerformanceCounters()
		{
			return Task.Run(delegate
			{
				if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning())
				{
					_ramCounter = new PerformanceCounter
					{
						CategoryName = "Process",
						CounterName = "Working Set - Private",
						InstanceName = GameService.GameIntegration.get_Gw2Instance().get_Gw2Process().ProcessName,
						ReadOnly = true
					};
					_cpuCounter = new PerformanceCounter
					{
						CategoryName = "Process",
						CounterName = "% Processor Time",
						InstanceName = GameService.GameIntegration.get_Gw2Instance().get_Gw2Process().ProcessName,
						ReadOnly = true
					};
				}
			});
		}

		private Task QueryManagementObjects()
		{
			return Task.Run(delegate
			{
				using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
				foreach (ManagementObject item in managementObjectSearcher.Get())
				{
					string text = (string)item["Name"];
					if (text.Length >= CpuName.Length)
					{
						CpuName = text.Trim();
					}
				}
			});
		}

		private async void OnEnablePerformanceCountersSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_PreviousValue() != e.get_NewValue())
			{
				if (!e.get_NewValue())
				{
					_ramCounter?.Dispose();
					_cpuCounter?.Dispose();
				}
				else
				{
					await LoadPerformanceCounters();
					await QueryManagementObjects();
				}
			}
		}

		private void OnGw2Closed(object o, EventArgs e)
		{
			DataPanel dataPanel = _dataPanel;
			if (dataPanel != null)
			{
				((Control)dataPanel).Dispose();
			}
			Label cursorPos = _cursorPos;
			if (cursorPos != null)
			{
				((Control)cursorPos).Dispose();
			}
			_ramCounter?.Dispose();
			_cpuCounter?.Dispose();
		}

		private async void OnGw2Started(object o, EventArgs e)
		{
			await LoadPerformanceCounters();
		}

		private void UpdateCounter()
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && _ramCounter != null && _cpuCounter != null && !(DateTime.UtcNow < _timeOutPc))
			{
				_timeOutPc = DateTime.UtcNow.AddMilliseconds(1000.0);
				MemoryUsage = _ramCounter.NextValue() / 1024f / 1024f;
				CpuUsage = _cpuCounter.NextValue() / (float)Environment.ProcessorCount;
			}
		}

		private void UpdateCursorPos()
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && _cursorPos != null)
			{
				((Control)_cursorPos).set_Visible(!GameService.Input.get_Mouse().get_CameraDragging());
				_cursorPos.set_Text(PInvoke.IsLControlPressed() ? $"X: {GameService.Input.get_Mouse().get_Position().X - ((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2}, Y: {Math.Abs(GameService.Input.get_Mouse().get_Position().Y - ((Control)GameService.Graphics.get_SpriteScreen()).get_Height())}" : $"X: {GameService.Input.get_Mouse().get_Position().X}, Y: {GameService.Input.get_Mouse().get_Position().Y}");
				((Control)_cursorPos).set_Location(new Point(GameService.Input.get_Mouse().get_Position().X + 50, GameService.Input.get_Mouse().get_Position().Y));
			}
		}

		private void OnToggleInfoBindingActivated(object o, EventArgs e)
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
			{
				if (_dataPanel != null)
				{
					((Control)_dataPanel).Dispose();
					_dataPanel = null;
				}
				else
				{
					BuildDisplay();
				}
				if (_cursorPos != null)
				{
					((Control)_cursorPos).Dispose();
					_cursorPos = null;
				}
				else
				{
					BuildCursorPosTooltip();
				}
			}
		}

		private void OnShowCursorPositionSettingChanged(object o, ValueChangedEventArgs<bool> e)
		{
			if (!e.get_NewValue())
			{
				Label cursorPos = _cursorPos;
				if (cursorPos != null)
				{
					((Control)cursorPos).Dispose();
				}
			}
			else if (_dataPanel != null)
			{
				BuildCursorPosTooltip();
			}
		}

		private void BuildCursorPosTooltip()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			if (_showCursorPosition.get_Value())
			{
				Label cursorPos = _cursorPos;
				if (cursorPos != null)
				{
					((Control)cursorPos).Dispose();
				}
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
				((Control)val).set_Size(new Point(130, 20));
				val.set_StrokeText(true);
				val.set_ShowShadow(true);
				((Control)val).set_Location(new Point(GameService.Input.get_Mouse().get_Position().X, GameService.Input.get_Mouse().get_Position().Y));
				val.set_VerticalAlignment((VerticalAlignment)0);
				((Control)val).set_ZIndex(-9999);
				_cursorPos = val;
				GameService.Input.get_Keyboard().add_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
				GameService.Input.get_Keyboard().add_KeyReleased((EventHandler<KeyboardEventArgs>)OnKeyReleased);
				((Control)_cursorPos).add_Disposed((EventHandler<EventArgs>)delegate
				{
					GameService.Input.get_Keyboard().remove_KeyPressed((EventHandler<KeyboardEventArgs>)OnKeyPressed);
					GameService.Input.get_Keyboard().remove_KeyReleased((EventHandler<KeyboardEventArgs>)OnKeyReleased);
				});
			}
		}

		private void OnKeyPressed(object o, KeyboardEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Invalid comparison between Unknown and I4
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (_cursorPos != null && !GameService.Input.get_Mouse().get_CameraDragging() && (int)e.get_Key() == 164)
			{
				_cursorPos.set_TextColor(new Color(252, 252, 84));
			}
		}

		private void OnKeyReleased(object o, KeyboardEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Invalid comparison between Unknown and I4
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			if (_cursorPos != null && !GameService.Input.get_Mouse().get_CameraDragging() && (int)e.get_Key() == 164)
			{
				_cursorPos.set_TextColor(Color.get_White());
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(_cursorPos.get_Text());
				ScreenNotification.ShowNotification("Copied!", (NotificationType)0, (Texture2D)null, 4);
			}
		}

		private void BuildDisplay()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			DataPanel dataPanel = _dataPanel;
			if (dataPanel != null)
			{
				((Control)dataPanel).Dispose();
			}
			DataPanel dataPanel2 = new DataPanel();
			((Control)dataPanel2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)dataPanel2).set_Size(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height()));
			((Control)dataPanel2).set_Location(new Point(0, 0));
			((Control)dataPanel2).set_ZIndex(-9999);
			_dataPanel = dataPanel2;
			GetCurrentMap(GameService.Gw2Mumble.get_CurrentMap().get_Id());
			GetCurrentElite(GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization());
		}

		private void OnMapChanged(object o, ValueEventArgs<int> e)
		{
			GetCurrentMap(e.get_Value());
		}

		private void OnSpecializationChanged(object o, ValueEventArgs<int> e)
		{
			GetCurrentElite(e.get_Value());
		}

		private async void GetCurrentMap(int id)
		{
			await ((IBulkExpandableClient<Map, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(id, default(CancellationToken)).ContinueWith(delegate(Task<Map> response)
			{
				if (response.Exception == null && !response.IsFaulted && !response.IsCanceled)
				{
					Map result = response.Result;
					if (_dataPanel != null)
					{
						CurrentMap = result;
					}
				}
			});
		}

		private async void GetCurrentElite(int id)
		{
			await ((IBulkExpandableClient<Specialization, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).GetAsync(id, default(CancellationToken)).ContinueWith(delegate(Task<Specialization> response)
			{
				if (response.Exception == null && !response.IsFaulted && !response.IsCanceled)
				{
					Specialization result = response.Result;
					if (_dataPanel != null)
					{
						CurrentSpec = result;
					}
				}
			});
		}

		protected override void Unload()
		{
			DataPanel dataPanel = _dataPanel;
			if (dataPanel != null)
			{
				((Control)dataPanel).Dispose();
			}
			Label cursorPos = _cursorPos;
			if (cursorPos != null)
			{
				((Control)cursorPos).Dispose();
			}
			_ramCounter?.Dispose();
			_cpuCounter?.Dispose();
			_toggleInfoBinding.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleInfoBindingActivated);
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_SpecializationChanged((EventHandler<ValueEventArgs<int>>)OnSpecializationChanged);
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2Closed((EventHandler<EventArgs>)OnGw2Closed);
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2Started((EventHandler<EventArgs>)OnGw2Started);
			ModuleInstance = null;
		}
	}
}
