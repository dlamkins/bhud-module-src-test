using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.State;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public class DynamicEventsSettingsView : BaseSettingsView
	{
		private readonly DynamicEventState _dynamicEventState;

		private readonly ModuleSettings _moduleSettings;

		private readonly IFlurlClient _flurlClient;

		private StandardWindow _manageEventsWindow;

		private Texture2D _dynamicEventsInWorldImage;

		public DynamicEventsSettingsView(DynamicEventState dynamicEventState, ModuleSettings moduleSettings, IFlurlClient flurlClient, Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, SettingEventState settingEventState, BitmapFont font = null)
			: base(apiManager, iconState, translationState, settingEventState, font)
		{
			_dynamicEventState = dynamicEventState;
			_moduleSettings = moduleSettings;
			_flurlClient = flurlClient;
		}

		protected override void BuildView(FlowPanel parent)
		{
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.ShowDynamicEventsOnMap);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.ShowDynamicEventInWorld, async (bool oldVal, bool newVal) => !newVal || await new ConfirmDialog("Activate \"" + ((SettingEntry)_moduleSettings.ShowDynamicEventInWorld).get_DisplayName() + "\"?", "You are in the process of activating \"" + ((SettingEntry)_moduleSettings.ShowDynamicEventInWorld).get_DisplayName() + "\".\nThis setting will add event boundaries inside your view (only when applicable events are on your map).\n\nDo you want to continue?", base.IconState).ShowDialog() == DialogResult.OK);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.ShowDynamicEventsInWorldOnlyWhenInside);
			RenderBoolSetting((Panel)(object)parent, _moduleSettings.IgnoreZAxisOnDynamicEventsInWorld);
			RenderIntSetting((Panel)(object)parent, _moduleSettings.DynamicEventsRenderDistance);
			RenderButton((Panel)(object)parent, base.TranslationState.GetTranslation("dynamicEventsSettingsView-manageEvents-btn", "Manage Events"), delegate
			{
				if (_manageEventsWindow == null)
				{
					_manageEventsWindow = WindowUtil.CreateStandardWindow("Manage Events", ((object)this).GetType(), Guid.Parse("7dc52c82-67ae-4cfb-9fe3-a16a8b30892c"), base.IconState);
				}
				if (((WindowBase2)_manageEventsWindow).get_CurrentView() != null)
				{
					(((WindowBase2)_manageEventsWindow).get_CurrentView() as ManageDynamicEventsSettingsView).EventChanged -= ManageView_EventChanged;
				}
				ManageDynamicEventsSettingsView manageDynamicEventsSettingsView = new ManageDynamicEventsSettingsView(_dynamicEventState, () => _moduleSettings.DisabledDynamicEventIds.get_Value(), base.APIManager, base.IconState, base.TranslationState);
				manageDynamicEventsSettingsView.EventChanged += ManageView_EventChanged;
				_manageEventsWindow.Show((IView)(object)manageDynamicEventsSettingsView);
			});
			if (_dynamicEventsInWorldImage != null)
			{
				RenderEmptyLine((Panel)(object)parent, 100);
				RenderLabel((Panel)(object)parent, "Image of dynamic events inside the game world:");
				((Control)new Image(AsyncTexture2D.op_Implicit(_dynamicEventsInWorldImage))).set_Parent((Container)(object)parent);
			}
		}

		private void ManageView_EventChanged(object sender, ManageEventsView.EventChangedArgs e)
		{
			_moduleSettings.DisabledDynamicEventIds.set_Value(e.NewState ? new List<string>(from s in _moduleSettings.DisabledDynamicEventIds.get_Value()
				where s != e.EventSettingKey
				select s) : new List<string>(_moduleSettings.DisabledDynamicEventIds.get_Value()) { e.EventSettingKey });
		}

		protected override async Task<bool> InternalLoad(IProgress<string> progress)
		{
			await TryLoadingDynamicEventsInWorldImage();
			return true;
		}

		private async Task TryLoadingDynamicEventsInWorldImage()
		{
			try
			{
				Bitmap bitmap = ImageUtil.ResizeImage(Image.FromStream(await _flurlClient.Request("https://files.estreya.de/blish-hud/event-table/images/dynamic-events-in-world.png").GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0)), 500, 400);
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					bitmap.Save(memoryStream, ImageFormat.Png);
					await Task.Run(delegate
					{
						//IL_0005: Unknown result type (might be due to invalid IL or missing references)
						//IL_000a: Unknown result type (might be due to invalid IL or missing references)
						GraphicsDeviceContext val = GameService.Graphics.LendGraphicsDeviceContext();
						try
						{
							_dynamicEventsInWorldImage = Texture2D.FromStream(((GraphicsDeviceContext)(ref val)).get_GraphicsDevice(), (Stream)memoryStream);
						}
						finally
						{
							((GraphicsDeviceContext)(ref val)).Dispose();
						}
					});
				}
				finally
				{
					if (memoryStream != null)
					{
						((IDisposable)memoryStream).Dispose();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		protected override void Unload()
		{
			base.Unload();
			StandardWindow manageEventsWindow = _manageEventsWindow;
			if (((manageEventsWindow != null) ? ((WindowBase2)manageEventsWindow).get_CurrentView() : null) != null)
			{
				(((WindowBase2)_manageEventsWindow).get_CurrentView() as ManageDynamicEventsSettingsView).EventChanged -= ManageView_EventChanged;
			}
			StandardWindow manageEventsWindow2 = _manageEventsWindow;
			if (manageEventsWindow2 != null)
			{
				((Control)manageEventsWindow2).Dispose();
			}
			_manageEventsWindow = null;
			Texture2D dynamicEventsInWorldImage = _dynamicEventsInWorldImage;
			if (dynamicEventsInWorldImage != null)
			{
				((GraphicsResource)dynamicEventsInWorldImage).Dispose();
			}
			_dynamicEventsInWorldImage = null;
		}
	}
}
