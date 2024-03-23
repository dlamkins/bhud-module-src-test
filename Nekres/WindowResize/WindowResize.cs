using System;
using System.ComponentModel.Composition;
using System.Drawing;
using Blish_HUD;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Nekres.WindowResize.Core;
using Nekres.WindowResize.Core.Services;

namespace Nekres.WindowResize
{
	[Export(typeof(Module))]
	public class WindowResize : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger<WindowResize>();

		internal WindowService Window;

		internal SettingEntry<WindowUtil.WindowSize> WindowSize;

		internal SettingEntry<bool> Borderless;

		internal static WindowResize Instance { get; private set; }

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public WindowResize([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SettingCollection styleCol = settings.AddSubCollection("window_style", true, (Func<string>)(() => "Window Appearance"));
			WindowSize = styleCol.DefineSetting<WindowUtil.WindowSize>("size", WindowUtil.WindowSize._1920x1080, (Func<string>)(() => "Size"), (Func<string>)null);
		}

		protected override void Initialize()
		{
			Window = new WindowService();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			WindowSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<WindowUtil.WindowSize>>)OnWindowSizeChanged);
			((Module)this).OnModuleLoaded(e);
		}

		private void OnWindowSizeChanged(object sender, ValueChangedEventArgs<WindowUtil.WindowSize> e)
		{
			if (Window.IsWindowedMode)
			{
				Size size = WindowUtil.ParseSize(WindowSize.get_Value());
				WindowUtil.ResizeAndCenterWindow(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle(), size.Width, size.Height);
			}
		}

		private void OnBorderlessChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (Window.IsWindowedMode)
			{
				if (e.get_NewValue())
				{
					WindowUtil.RemoveBorder(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle());
				}
				else
				{
					WindowUtil.AddBorder(GameService.GameIntegration.get_Gw2Instance().get_Gw2WindowHandle());
				}
			}
		}

		protected override void Update(GameTime gameTime)
		{
			Window.Update(gameTime);
		}

		protected override void Unload()
		{
			WindowSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<WindowUtil.WindowSize>>)OnWindowSizeChanged);
			Instance = null;
		}
	}
}
