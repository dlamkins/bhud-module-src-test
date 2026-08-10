using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Neokain.GW2.AllianceManager.Controls.Connection;
using Neokain.GW2.AllianceManager.Controls.Shared;
using Neokain.GW2.AllianceManager.Services.Connection;

namespace Neokain.GW2.AllianceManager.Views
{
	internal sealed class ConnectionView : View
	{
		private const string LiveLabel = "Live (am.neokain.de)";

		private const string TestLabel = "Test (am-test.neokain.de)";

		private readonly Module _module;

		private FlowPanel _root;

		private bool _built;

		private bool _applyingBodyWidths;

		private const int ControlWidth = 210;

		private const int KeyInputWidth = 420;

		private static readonly Color WarnColor = new Color(235, 195, 100);

		private static readonly Color ErrorColor = new Color(225, 105, 95);

		private static readonly Color OkColor = new Color(130, 200, 130);

		public ConnectionView(Module module)
			: this()
		{
			_module = module ?? throw new ArgumentNullException("module");
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val).set_CanScroll(true);
			((Control)val).set_Padding(new Thickness(15f));
			val.set_ControlPadding(new Vector2(10f));
			_root = val;
			_built = true;
			((Container)_root).add_ContentResized((EventHandler<RegionChangedEventArgs>)OnRootContentResized);
			_module.ConnectionStatusChanged += OnConnectionStatusChanged;
			RebuildContent();
		}

		private void OnConnectionStatusChanged(object sender, EventArgs e)
		{
			GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
			{
				RebuildContent();
			});
		}

		private void RebuildContent()
		{
			if (_built && _root != null)
			{
				((Container)_root).ClearChildren();
				ConnectionStatus status = _module.ConnectionStatus;
				ModuleConnectionState state = status?.State ?? ModuleConnectionState.NoKey;
				switch (state)
				{
				case ModuleConnectionState.NoKey:
					BuildNoKey(status);
					break;
				case ModuleConnectionState.BadKeyFormat:
					BuildBadKeyFormat(status);
					break;
				case ModuleConnectionState.Connecting:
				case ModuleConnectionState.FailedTransient:
					BuildConnectingOrTransient(status);
					break;
				case ModuleConnectionState.FailedTerminal:
					BuildFailedTerminal(status);
					break;
				case ModuleConnectionState.Connected:
					BuildConnected(status);
					break;
				default:
					BuildNoKey(status);
					break;
				}
				if (state != 0 && state != ModuleConnectionState.BadKeyFormat)
				{
					BuildFooter(status);
				}
			}
		}

		private void BuildNoKey(ConnectionStatus status)
		{
			Heading("Connect to Alliance Manager");
			Body("Set this up once to get started.");
			BuildInlineSetupControls(onboarding: true);
		}

		private void BuildBadKeyFormat(ConnectionStatus status)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			if (LooksLikeGw2ApiKey(_module.CustomApiKey?.get_Value() ?? string.Empty))
			{
				Heading("Use an Alliance Manager key", WarnColor);
				Body("That looks like a Guild Wars 2 API key. Alliance Manager doesn't use Guild Wars 2 keys — it has its own key that starts with \"am_\".");
				Body("Open the website, sign in, and create an Alliance Manager key, then paste it below.");
			}
			else
			{
				Heading("Check your API key", WarnColor);
				Body("That key doesn't look right — it may be incomplete. An Alliance Manager key starts with \"am_\".");
				Body("Re-copy it from the website and paste it again below.");
			}
			BuildInlineSetupControls(onboarding: true);
		}

		private void BuildConnectingOrTransient(ConnectionStatus status)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			bool connecting = status != null && status.State == ModuleConnectionState.Connecting;
			Heading(connecting ? "Connecting…" : "Connection lost", connecting ? null : new Color?(WarnColor));
			Body(connecting ? "Contacting the server and authenticating…" : "The server couldn't be reached. The module keeps retrying automatically.");
			KeyValue("Server", EndpointOrDash(status));
			if (!connecting)
			{
				string countdown = ConnectionOverlayPolicy.CountdownText(status, DateTime.UtcNow);
				if (countdown.Length > 0)
				{
					Body(countdown);
				}
			}
		}

		private void BuildFailedTerminal(ConnectionStatus status)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Heading("Key rejected", ErrorColor);
			Body("The server rejected this key. Live and Test use separate keys — check that the server below matches where you created it.");
			BuildInlineSetupControls(onboarding: true);
		}

		private void BuildConnected(ConnectionStatus status)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Heading("Connected", OkColor);
			KeyValue("Account", string.IsNullOrEmpty(status?.AccountName) ? "—" : status.AccountName);
			KeyValue("Server", EndpointOrDash(status));
		}

		private void BuildInlineSetupControls(bool onboarding)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			Caption(onboarding ? "1. Choose your server" : "Server");
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)_root);
			((Control)val).set_Width(210);
			val.get_Items().Add("Live (am.neokain.de)");
			val.get_Items().Add("Test (am-test.neokain.de)");
			val.set_SelectedItem((_module.EndpointSelectionSetting.get_Value() == EndpointSelection.Test) ? "Test (am-test.neokain.de)" : "Live (am.neokain.de)");
			val.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnEndpointDropdownChanged);
			if (onboarding)
			{
				Hint("Most people use Live.");
			}
			if (onboarding)
			{
				Caption("2. Get your API key");
			}
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)_root);
			val2.set_Text("Open website");
			((Control)val2).set_Width(210);
			((Control)val2).set_BasicTooltipText("Open the Alliance Manager website to sign in and create an API key.");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)OnOpenFrontendClicked);
			if (onboarding)
			{
				Hint("Sign in, then create an Alliance Manager key — not a Guild Wars 2 API key.");
			}
			Caption(onboarding ? "3. Paste your key" : "API key");
			MaskedKeyInput maskedKeyInput = new MaskedKeyInput(_module.CustomApiKey, (BitmapFont)(object)_module.FontService?.DejaVuSansDefault);
			((Control)maskedKeyInput).set_Parent((Container)(object)_root);
			((Container)maskedKeyInput).set_WidthSizingMode((SizingMode)0);
			((Control)maskedKeyInput).set_Width(420);
			if (onboarding)
			{
				Hint("The module connects as soon as the key is valid.");
			}
		}

		private static bool LooksLikeGw2ApiKey(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return false;
			}
			key = key.Trim();
			if (!key.StartsWith("am_", StringComparison.OrdinalIgnoreCase) && key.Contains("-"))
			{
				return key.Length >= 30;
			}
			return false;
		}

		private void OnEndpointDropdownChanged(object sender, ValueChangedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			EndpointSelection selection = ((((Dropdown)sender).get_SelectedItem() == "Test (am-test.neokain.de)") ? EndpointSelection.Test : EndpointSelection.Live);
			if (_module.EndpointSelectionSetting.get_Value() != selection)
			{
				_module.EndpointSelectionSetting.set_Value(selection);
			}
		}

		private void OnOpenFrontendClicked(object sender, MouseEventArgs e)
		{
			try
			{
				Process.Start(new ProcessStartInfo(EndpointResolver.ResolveFrontend(_module.EndpointSelectionSetting.get_Value()))
				{
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				Logger.GetLogger<ConnectionView>().Warn(ex, "Failed to open frontend URL");
			}
		}

		private void BuildFooter(ConnectionStatus status)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_root);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Height(1);
			((Control)val).set_BackgroundColor(new Color(80, 80, 80, 200));
			string reason = ReasonOrDash(status);
			string stamp = ((status != null) ? status.UpdatedUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : "—");
			KeyValue("Last status", reason);
			KeyValue("Updated", stamp);
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)_root);
			val2.set_Text("Reconnect");
			((Control)val2).set_Width(160);
			((Control)val2).set_BasicTooltipText("Cancel the current attempt and reconnect now.");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_module.RequestReconnect();
			});
		}

		private void Heading(string text, Color? color = null)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_root);
			val.set_Text(text);
			val.set_Font((BitmapFont)(((object)_module.FontService?.DejaVuSansHeader) ?? ((object)GameService.Content.get_DefaultFont16())));
			val.set_TextColor((Color)(((_003F?)color) ?? Color.get_White()));
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
		}

		private void Body(string text)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			AutoWrappingLabel autoWrappingLabel = new AutoWrappingLabel();
			((Control)autoWrappingLabel).set_Parent((Container)(object)_root);
			autoWrappingLabel.Text = text;
			autoWrappingLabel.Font = (BitmapFont)(((object)_module.FontService?.DejaVuSansDefault) ?? ((object)GameService.Content.get_DefaultFont14()));
			autoWrappingLabel.TextColor = Color.get_LightGray();
			AutoWrappingLabel lbl = autoWrappingLabel;
			ApplyBodyWidth(lbl);
		}

		private void Caption(string text)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_root);
			val.set_Text(text);
			val.set_Font((BitmapFont)(((object)_module.FontService?.DejaVuSansDefault) ?? ((object)GameService.Content.get_DefaultFont14())));
			val.set_TextColor(Color.get_White());
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
		}

		private void Hint(string text)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			AutoWrappingLabel autoWrappingLabel = new AutoWrappingLabel();
			((Control)autoWrappingLabel).set_Parent((Container)(object)_root);
			autoWrappingLabel.Text = text;
			autoWrappingLabel.Font = (BitmapFont)(((object)_module.FontService?.DejaVuSansDefault) ?? ((object)GameService.Content.get_DefaultFont14()));
			autoWrappingLabel.TextColor = new Color(150, 150, 150);
			AutoWrappingLabel lbl = autoWrappingLabel;
			ApplyBodyWidth(lbl);
		}

		private void KeyValue(string key, string value)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			AutoWrappingLabel autoWrappingLabel = new AutoWrappingLabel();
			((Control)autoWrappingLabel).set_Parent((Container)(object)_root);
			autoWrappingLabel.Text = key + ": " + value;
			autoWrappingLabel.Font = (BitmapFont)(((object)_module.FontService?.DejaVuSansDefault) ?? ((object)GameService.Content.get_DefaultFont14()));
			autoWrappingLabel.TextColor = Color.get_LightGray();
			AutoWrappingLabel lbl = autoWrappingLabel;
			ApplyBodyWidth(lbl);
		}

		private void ApplyBodyWidth(AutoWrappingLabel label)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel root = _root;
			int w = ((root != null) ? ((Container)root).get_ContentRegion().Width : 0);
			if (w > 0)
			{
				label.MaxWidth = w;
				((Control)label).set_Width(w);
			}
		}

		private void OnRootContentResized(object sender, RegionChangedEventArgs e)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (_root == null || _applyingBodyWidths)
			{
				return;
			}
			int w = ((Container)_root).get_ContentRegion().Width;
			if (w <= 0)
			{
				return;
			}
			_applyingBodyWidths = true;
			try
			{
				foreach (Control item in ((Container)_root).get_Children().ToList())
				{
					AutoWrappingLabel label = item as AutoWrappingLabel;
					if (label != null && (((Control)label).get_Width() != w || label.MaxWidth != w))
					{
						label.MaxWidth = w;
						((Control)label).set_Width(w);
					}
				}
			}
			finally
			{
				_applyingBodyWidths = false;
			}
		}

		private static string EndpointOrDash(ConnectionStatus status)
		{
			if (!string.IsNullOrEmpty(status?.Endpoint))
			{
				return status.Endpoint;
			}
			return "—";
		}

		private static string ReasonOrDash(ConnectionStatus status)
		{
			if (!string.IsNullOrEmpty(status?.Reason))
			{
				return status.Reason;
			}
			return "—";
		}

		protected override void Unload()
		{
			_module.ConnectionStatusChanged -= OnConnectionStatusChanged;
			if (_root != null)
			{
				((Container)_root).remove_ContentResized((EventHandler<RegionChangedEventArgs>)OnRootContentResized);
			}
			FlowPanel root = _root;
			if (root != null)
			{
				((Control)root).Dispose();
			}
			_root = null;
			_built = false;
			((View<IPresenter>)this).Unload();
		}
	}
}
