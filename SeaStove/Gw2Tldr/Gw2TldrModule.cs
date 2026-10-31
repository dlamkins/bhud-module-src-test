using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SeaStove.Gw2Tldr.Data;
using SeaStove.Gw2Tldr.UI;

namespace SeaStove.Gw2Tldr
{
	[Export(typeof(Module))]
	public sealed class Gw2TldrModule : Module
	{
		private readonly ModuleLifetime _lifetime = new ModuleLifetime();

		private SettingEntry<KeyBinding> _toggleSetting;

		private SettingEntry<int> _textSize;

		private SettingEntry<int> _fontStyle;

		private SettingEntry<string> _lastGuide;

		private KeyBinding _boundKey;

		private CornerIcon _icon;

		private AsyncTexture2D _logoTexture;

		private CompactGuideWindow _window;

		private SettingEntry<Point> _panelPosition;

		private SettingEntry<bool> _panelLocked;

		private SettingEntry<Point> _panelSize;

		private int _lastTextSize;

		private SettingEntry<int> _backgroundOpacity;

		private SettingEntry<bool> _colorCoding;

		private SettingEntry<bool> _autoHeight;

		private SettingEntry<string> _customGuides;

		private SettingEntry<bool> _autoSelect;

		private SettingEntry<bool> _fadeHeader;

		private SettingEntry<bool> _lightMode;

		private SettingEntry<bool> _showCopyButtons;

		private SettingEntry<bool> _colorText;

		private SettingEntry<bool> _hideUnmapped;

		private SettingEntry<bool> _showMapped;

		private SettingEntry<bool> _hideLoading;

		[ImportingConstructor]
		public Gw2TldrModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			_toggleSetting = settings.DefineSetting<KeyBinding>("ToggleGuides", new KeyBinding((ModifierKeys)5, (Keys)71), (Func<string>)(() => "Toggle guide window"), (Func<string>)(() => "Open or close GW2 TLDR. Defaults to Ctrl+Shift+G."));
			_textSize = settings.DefineSetting<int>("TextSize", 1, (Func<string>)(() => "Guide text size"), (Func<string>)(() => "0 = small, 1 = medium, 2 = large."));
			_fontStyle = settings.DefineSetting<int>("GuideFontStyle", 0, (Func<string>)(() => "Guide font style"), (Func<string>)(() => "0 = regular, 1 = bold, 2 = italic."));
			_backgroundOpacity = settings.DefineSetting<int>("BackgroundOpacity", 75, (Func<string>)(() => "Background opacity (%)"), (Func<string>)(() => "0 is transparent; 100 is opaque."));
			_showCopyButtons = settings.DefineSetting<bool>("ShowCopyButtons", true, (Func<string>)(() => "Show copy buttons"), (Func<string>)(() => "Show buttons for copying notes and the full guide."));
			_colorText = settings.DefineSetting<bool>("ColorText", true, (Func<string>)(() => "Color-code note text"), (Func<string>)(() => "Color note text by mechanic type."));
			_lightMode = settings.DefineSetting<bool>("LightMode", false, (Func<string>)(() => "Light mode"), (Func<string>)(() => "Use a light background with dark text."));
			_fadeHeader = settings.DefineSetting<bool>("FadeHeader", true, (Func<string>)(() => "Fade header buttons"), (Func<string>)(() => "Fade header controls until you hover over the header."));
			_hideUnmapped = settings.DefineSetting<bool>("HideUnmapped", true, (Func<string>)(() => "Hide on unmapped maps"), (Func<string>)(() => "Hide the guide when entering a map without an encounter guide."));
			_showMapped = settings.DefineSetting<bool>("ShowMapped", true, (Func<string>)(() => "Show on mapped maps"), (Func<string>)(() => "Show the guide when entering a map with an encounter guide."));
			_hideLoading = settings.DefineSetting<bool>("HideDuringLoading", false, (Func<string>)(() => "Hide during loading screens"), (Func<string>)(() => "Temporarily hide the guide during loading screens, cinematics, and character selection."));
			_autoSelect = settings.DefineSetting<bool>("AutoSelect", true, (Func<string>)(() => "Auto-select guides"), (Func<string>)(() => "Select the first guide for your map and keep the current guide when there is no match."));
			_autoHeight = settings.DefineSetting<bool>("AutoHeight", false, (Func<string>)(() => "Fit guide height to content"), (Func<string>)(() => "Automatically resize guide height to fit its notes."));
			_colorCoding = settings.DefineSetting<bool>("ColorCoding", true, (Func<string>)(() => "Color-coded icons"), (Func<string>)(() => "Disable to show all mechanic icons in neutral gray."));
			SettingCollection internalSettings = settings.AddSubCollection("GuideBrowser", false);
			_customGuides = internalSettings.DefineSetting<string>("CustomGuides", "{}", (Func<string>)null, (Func<string>)null);
			_lastGuide = internalSettings.DefineSetting<string>("LastGuide", "", (Func<string>)null, (Func<string>)null);
			_panelPosition = internalSettings.DefineSetting<Point>("PanelPosition", new Point(200, 150), (Func<string>)null, (Func<string>)null);
			_panelLocked = internalSettings.DefineSetting<bool>("PanelLocked", false, (Func<string>)null, (Func<string>)null);
			_panelSize = internalSettings.DefineSetting<Point>("PanelSize", new Point(390, 245), (Func<string>)null, (Func<string>)null);
		}

		protected override Task LoadAsync()
		{
			GuideCatalog catalog = null;
			string error = null;
			try
			{
				catalog = GuideCatalog.Load(typeof(Gw2TldrModule).Assembly.GetManifestResourceStream("SeaStove.Gw2Tldr.guides.json"));
			}
			catch (Exception exception)
			{
				Logger.GetLogger<Gw2TldrModule>().Warn(exception, "Could not load bundled GW2 TLDR guides.");
				error = "GW2 TLDR could not load its bundled guides. Update or reinstall the module.\n\n" + exception.Message;
			}
			_lifetime.TryUse(delegate
			{
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_044b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0450: Unknown result type (might be due to invalid IL or missing references)
				//IL_0460: Expected O, but got Unknown
				try
				{
					_lastTextSize = _textSize.get_Value();
					_window = new CompactGuideWindow(catalog, error, _lastGuide.get_Value(), _textSize.get_Value(), _panelPosition.get_Value(), _panelSize.get_Value(), _panelLocked.get_Value(), delegate(string id)
					{
						_lastGuide.set_Value(id);
					}, delegate(int size)
					{
						_textSize.set_Value(size);
					}, delegate(Point position)
					{
						//IL_000b: Unknown result type (might be due to invalid IL or missing references)
						_panelPosition.set_Value(position);
					}, delegate(Point dimensions)
					{
						//IL_000b: Unknown result type (might be due to invalid IL or missing references)
						_panelSize.set_Value(dimensions);
					}, delegate(bool locked)
					{
						_panelLocked.set_Value(locked);
					}, () => _customGuides.get_Value(), delegate(string json)
					{
						_customGuides.set_Value(json);
					}, () => _autoSelect.get_Value(), delegate(bool enabled)
					{
						_autoSelect.set_Value(enabled);
					}, delegate(int opacity)
					{
						_backgroundOpacity.set_Value(opacity);
					}, delegate(bool enabled)
					{
						_colorCoding.set_Value(enabled);
					}, () => _fadeHeader.get_Value(), delegate(bool enabled)
					{
						_fadeHeader.set_Value(enabled);
					}, () => _lightMode.get_Value(), delegate(bool enabled)
					{
						_lightMode.set_Value(enabled);
					}, () => _showCopyButtons.get_Value(), delegate(bool enabled)
					{
						_showCopyButtons.set_Value(enabled);
					}, () => _colorText.get_Value(), delegate(bool enabled)
					{
						_colorText.set_Value(enabled);
					}, () => _hideUnmapped.get_Value(), delegate(bool enabled)
					{
						_hideUnmapped.set_Value(enabled);
					}, () => _showMapped.get_Value(), delegate(bool enabled)
					{
						_showMapped.set_Value(enabled);
					}, () => _fontStyle.get_Value(), delegate(int value)
					{
						_fontStyle.set_Value(value);
					}, () => _hideLoading.get_Value(), delegate(bool enabled)
					{
						_hideLoading.set_Value(enabled);
					}, () => _autoHeight.get_Value(), delegate(bool enabled)
					{
						_autoHeight.set_Value(enabled);
					});
					MechanicIcon.LoadTextures();
					_logoTexture = LoadLogo();
					Gw2TldrModule gw2TldrModule = this;
					CornerIcon val = new CornerIcon(_logoTexture, "GW2 TLDR");
					((Control)val).set_BasicTooltipText("GW2 TLDR — encounter guides");
					gw2TldrModule._icon = val;
					((Control)_icon).add_Click((EventHandler<MouseEventArgs>)ToggleWindow);
					((Control)_icon).add_RightMouseButtonReleased((EventHandler<MouseEventArgs>)OpenGuideSettings);
					BindHotkey();
				}
				catch
				{
					CleanupResources();
					throw;
				}
			});
			return Task.CompletedTask;
		}

		private static AsyncTexture2D LoadLogo()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			using Stream stream = typeof(Gw2TldrModule).Assembly.GetManifestResourceStream("SeaStove.Gw2Tldr.logo.png");
			GraphicsDeviceContext context = GameService.Graphics.LendGraphicsDeviceContext();
			try
			{
				Texture2D original = Texture2D.FromStream(((GraphicsDeviceContext)(ref context)).get_GraphicsDevice(), stream);
				try
				{
					Color[] pixels = (Color[])(object)new Color[original.get_Width() * original.get_Height()];
					original.GetData<Color>(pixels);
					for (int i = 0; i < pixels.Length; i++)
					{
						Color pixel = pixels[i];
						byte premultiplied = (byte)((byte)(255 - (((Color)(ref pixel)).get_R() * 299 + ((Color)(ref pixel)).get_G() * 587 + ((Color)(ref pixel)).get_B() * 114) / 1000) * ((Color)(ref pixel)).get_A() / 255);
						pixels[i] = new Color(premultiplied, premultiplied, premultiplied, ((Color)(ref pixel)).get_A());
					}
					Texture2D val = new Texture2D(((GraphicsDeviceContext)(ref context)).get_GraphicsDevice(), original.get_Width(), original.get_Height());
					val.SetData<Color>(pixels);
					return new AsyncTexture2D(val);
				}
				finally
				{
					((IDisposable)original)?.Dispose();
				}
			}
			finally
			{
				((GraphicsDeviceContext)(ref context)).Dispose();
			}
		}

		private void ToggleWindow(object sender, EventArgs args)
		{
			_lifetime.TryUse(delegate
			{
				_window?.Toggle();
			});
		}

		private void OpenGuideSettings(object sender, MouseEventArgs args)
		{
			_lifetime.TryUse(delegate
			{
				_window?.OpenSettings();
			});
		}

		private void BindHotkey()
		{
			if (_boundKey != _toggleSetting.get_Value())
			{
				if (_boundKey != null)
				{
					_boundKey.remove_Activated((EventHandler<EventArgs>)ToggleWindow);
					_boundKey.set_Enabled(false);
				}
				_boundKey = _toggleSetting.get_Value();
				if (_boundKey != null)
				{
					_boundKey.set_IgnoreWhenInTextField(true);
					_boundKey.add_Activated((EventHandler<EventArgs>)ToggleWindow);
					_boundKey.set_Enabled(true);
				}
			}
		}

		protected override void Update(GameTime gameTime)
		{
			_lifetime.TryUse(delegate
			{
				if (_window != null)
				{
					BindHotkey();
					_window.Update();
					_window.SetAppearance(_backgroundOpacity.get_Value(), _colorCoding.get_Value(), _lightMode.get_Value(), _showCopyButtons.get_Value(), _colorText.get_Value());
					if (_lastTextSize != _textSize.get_Value())
					{
						_lastTextSize = _textSize.get_Value();
						_window.SetTextSize(_lastTextSize);
					}
				}
			});
		}

		protected override void Unload()
		{
			_lifetime.Unload(CleanupResources);
		}

		private void CleanupResources()
		{
			if (_boundKey != null)
			{
				_boundKey.remove_Activated((EventHandler<EventArgs>)ToggleWindow);
				_boundKey.set_Enabled(false);
				_boundKey = null;
			}
			if (_icon != null)
			{
				((Control)_icon).remove_Click((EventHandler<MouseEventArgs>)ToggleWindow);
				((Control)_icon).remove_RightMouseButtonReleased((EventHandler<MouseEventArgs>)OpenGuideSettings);
			}
			CornerIcon icon = _icon;
			if (icon != null)
			{
				((Control)icon).Dispose();
			}
			_icon = null;
			AsyncTexture2D logoTexture = _logoTexture;
			if (logoTexture != null)
			{
				logoTexture.Dispose();
			}
			_logoTexture = null;
			_window?.Dispose();
			_window = null;
			MechanicIcon.DisposeTextures();
		}
	}
}
