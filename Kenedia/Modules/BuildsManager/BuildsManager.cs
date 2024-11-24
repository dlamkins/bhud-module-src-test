using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.Controls.Tabs;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager
{
	[Export(typeof(Module))]
	public class BuildsManager : BaseModule<BuildsManager, MainWindow, Settings, Paths>
	{
		public static int MainThread = Thread.CurrentThread.ManagedThreadId;

		private double _tick;

		private CancellationTokenSource _cancellationTokenSource;

		private bool _templatesLoaded;

		public static Data Data { get; set; }

		public bool TemplatesLoaded
		{
			get
			{
				return _templatesLoaded;
			}
			private set
			{
				Common.SetProperty(ref _templatesLoaded, value);
			}
		}

		public Template? SelectedTemplate => TemplatePresenter.Template;

		public GW2API GW2API { get; private set; }

		public TemplateTags TemplateTags { get; private set; }

		public TemplatePresenter TemplatePresenter { get; private set; }

		public TemplateCollection Templates { get; private set; }

		public Kenedia.Modules.Core.Controls.CornerIcon CornerIcon { get; private set; }

		public CornerIconContainer CornerContainer { get; private set; }

		public Kenedia.Modules.Core.Controls.LoadingSpinner LoadingSpinner { get; private set; }

		public NotificationBadge NotificationBadge { get; private set; }

		[ImportingConstructor]
		public BuildsManager([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
			HasGUI = true;
			AutoLoadGUI = false;
			base.CoreServices.GameStateDetectionService.Enabled = false;
		}

		protected override ServiceCollection DefineServices(ServiceCollection services)
		{
			base.DefineServices(services);
			services.AddSingleton(this);
			services.AddSingleton<TemplateCollection>();
			services.AddSingleton<TemplatePresenter>();
			services.AddSingleton<TemplateTags>();
			services.AddSingleton<TagGroups>();
			services.AddSingleton<Data>();
			services.AddSingleton<GW2API>();
			services.AddSingleton<Settings>();
			services.AddScoped<MainWindow>();
			services.AddScoped<MainWindowPresenter>();
			services.AddScoped<SelectionPanel>();
			services.AddScoped<AboutTab>();
			services.AddScoped<BuildTab>();
			services.AddScoped<GearTab>();
			services.AddScoped<QuickFiltersPanel>();
			services.AddSingleton<Func<Kenedia.Modules.Core.Controls.CornerIcon>>(() => CornerIcon);
			services.AddSingleton<Func<Kenedia.Modules.Core.Controls.LoadingSpinner>>(() => LoadingSpinner);
			services.AddSingleton<Func<NotificationBadge>>(() => NotificationBadge);
			services.AddSingleton<Func<CornerIconContainer>>(() => CornerContainer);
			services.AddTransient<TemplateFactory>();
			services.AddTransient<TemplateConverter>();
			return services;
		}

		protected override void AssignServiceInstaces(IServiceProvider serviceProvider)
		{
			base.AssignServiceInstaces(serviceProvider);
			Data = ServiceProviderServiceExtensions.GetRequiredService<Data>(base.ServiceProvider);
			Templates = ServiceProviderServiceExtensions.GetRequiredService<TemplateCollection>(base.ServiceProvider);
			TemplatePresenter = ServiceProviderServiceExtensions.GetRequiredService<TemplatePresenter>(base.ServiceProvider);
			TemplateTags = ServiceProviderServiceExtensions.GetRequiredService<TemplateTags>(base.ServiceProvider);
			GW2API = ServiceProviderServiceExtensions.GetRequiredService<GW2API>(base.ServiceProvider);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			base.Settings.ShowCornerIcon.SettingChanged += ShowCornerIcon_SettingChanged;
		}

		protected override void Initialize()
		{
			base.Initialize();
			CreateCornerIcons();
			BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Info("Starting " + base.Name + " v." + (object)base.Version.BaseVersion());
			LoadGUI();
		}

		public override IView GetSettingsView()
		{
			return new BlishSettingsView(delegate
			{
				if (!base.MainWindow.IsVisible())
				{
					base.MainWindow.Show();
				}
				base.MainWindow.SelectedTab = base.MainWindow.SettingsViewTab;
			});
		}

		protected override async void OnLocaleChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			if (GW2API != null)
			{
				Locale newValue = e.NewValue;
				if (newValue != Locale.Korean && newValue != Locale.Chinese && !Data.LoadedLocales.Contains(e.NewValue))
				{
					await Data.Load(e.NewValue);
				}
				base.OnLocaleChanged(sender, e);
			}
		}

		protected override async Task LoadAsync()
		{
			LoadingSpinner?.Show();
			await base.LoadAsync();
			await ServiceProviderServiceExtensions.GetService<TagGroups>(base.ServiceProvider)!.Load();
			await TemplateTags.Load();
			await Templates.Load();
			await Data.Load();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			base.OnModuleLoaded(e);
			if (!base.Settings.ShowCornerIcon.Value)
			{
				DeleteCornerIcons();
			}
			base.Settings.ToggleWindowKey.Value.Enabled = true;
			base.Settings.ToggleWindowKey.Value.Activated += OnToggleWindowKey;
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _tick > 500.0)
			{
				_tick = gameTime.get_TotalGameTime().TotalMilliseconds;
				Data data = Data;
				if (data != null && !data.IsLoaded)
				{
					Task.Run((Func<Task<bool>>)Data.Load);
				}
			}
		}

		protected override async void ReloadKey_Activated(object sender, EventArgs e)
		{
			BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("ReloadKey_Activated: " + base.Name);
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			base.ReloadKey_Activated(sender, e);
		}

		protected override void LoadGUI()
		{
			base.LoadGUI();
			BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Info("Building UI for " + base.Name);
			IServiceScope scope = ServiceProviderServiceExtensions.CreateScope(base.ServiceProvider);
			base.MainWindow = ServiceProviderServiceExtensions.GetRequiredService<MainWindow>(scope.ServiceProvider);
		}

		protected override void UnloadGUI()
		{
			base.UnloadGUI();
			base.MainWindow?.Dispose();
		}

		protected override void Unload()
		{
			DeleteCornerIcons();
			Templates.Clear();
			Data.Dispose();
			base.Unload();
		}

		private void ShowCornerIcon_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			if (e.NewValue)
			{
				CreateCornerIcons();
			}
			else
			{
				DeleteCornerIcons();
			}
		}

		private async Task LoadTemplates()
		{
		}

		private void CreateCornerIcons()
		{
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			DeleteCornerIcons();
			if (CornerIcon == null)
			{
				Kenedia.Modules.Core.Controls.CornerIcon obj = new Kenedia.Modules.Core.Controls.CornerIcon
				{
					Icon = AsyncTexture2D.FromAssetId(156720),
					HoverIcon = AsyncTexture2D.FromAssetId(156721),
					SetLocalizedTooltip = () => string.Format(strings_common.ToggleItem, base.Name ?? ""),
					Parent = GameService.Graphics.SpriteScreen,
					Priority = 51294257,
					Visible = (base.Settings?.ShowCornerIcon?.Value).GetValueOrDefault(),
					ClickAction = delegate
					{
						if (!Data.IsLoaded)
						{
							Task.Run(() => Data.Load(force: true));
						}
						else
						{
							base.MainWindow?.ToggleWindow();
						}
					}
				};
				Kenedia.Modules.Core.Controls.CornerIcon cornerIcon = obj;
				CornerIcon = obj;
			}
			if (CornerContainer == null)
			{
				CornerIconContainer obj2 = new CornerIconContainer
				{
					Parent = GameService.Graphics.SpriteScreen,
					WidthSizingMode = SizingMode.AutoSize,
					HeightSizingMode = SizingMode.AutoSize,
					Anchor = CornerIcon,
					AnchorPosition = AnchoredContainer.AnchorPos.Bottom,
					RelativePosition = new RectangleDimensions(0, -CornerIcon.Height / 2),
					CaptureInput = CaptureType.Filter
				};
				CornerIconContainer cornerIconContainer = obj2;
				CornerContainer = obj2;
			}
			if (NotificationBadge == null)
			{
				NotificationBadge obj3 = new NotificationBadge
				{
					Location = new Point(CornerIcon.Width - 15, 0),
					Parent = CornerContainer,
					Size = new Point(20),
					Opacity = 0.6f,
					HoveredOpacity = 1f,
					CaptureInput = CaptureType.Filter,
					Anchor = CornerIcon,
					Visible = false
				};
				NotificationBadge notificationBadge = obj3;
				NotificationBadge = obj3;
			}
			if (LoadingSpinner == null)
			{
				Kenedia.Modules.Core.Controls.LoadingSpinner obj4 = new Kenedia.Modules.Core.Controls.LoadingSpinner
				{
					Location = new Point(0, NotificationBadge.Bottom),
					Parent = CornerContainer,
					Size = CornerIcon.Size,
					BasicTooltipText = strings_common.FetchingApiData,
					Visible = false,
					CaptureInput = null
				};
				Kenedia.Modules.Core.Controls.LoadingSpinner loadingSpinner = obj4;
				LoadingSpinner = obj4;
			}
		}

		private void DeleteCornerIcons()
		{
			CornerIcon?.Dispose();
			CornerIcon = null;
			CornerContainer?.Dispose();
			CornerContainer = null;
		}

		private void OnToggleWindowKey(object sender, EventArgs e)
		{
			if (!(Control.ActiveControl is Blish_HUD.Controls.TextBox))
			{
				base.MainWindow?.ToggleWindow();
			}
		}
	}
}
