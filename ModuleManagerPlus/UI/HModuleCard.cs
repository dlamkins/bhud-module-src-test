using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModuleManagerPlus.Data;
using ModuleManagerPlus.Services;
using ModuleManagerPlus.Utility;
using MonoGame.Extended.BitmapFonts;
using SemVer;

namespace ModuleManagerPlus.UI
{
	internal class HModuleCard : Container
	{
		private const int CONTROL_WIDTH = 693;

		private const int FOOTER_HEIGHT = 50;

		private const int TITLE_HEIGHT = 25;

		private const int BUFFER = 10;

		private readonly ModuleInstallService _installService;

		private ModuleInstallState _installState;

		private int _heroSize = 160;

		private Texture2D _backgroundMask;

		private Texture2D _avatarMask;

		private Effect _maskEffect;

		private AsyncTexture2D _heroTexture;

		private AsyncTexture2D _authorTexture;

		private Texture2D _downloadsTexture;

		private Texture2D _cornerTexture;

		private Texture2D _cornerGradientTexture;

		private bool _newModule;

		private bool _pendingUpdate;

		private static Color _newModuleColor = Color.FromNonPremultiplied(52, 179, 255, 255);

		private static Color _newUpdateColor = Color.FromNonPremultiplied(245, 177, 73, 255);

		private static readonly RasterizerState _scissorOn;

		private Rectangle _heroRegion = Rectangle.get_Empty();

		private Rectangle _footerRegion = Rectangle.get_Empty();

		private Rectangle _nameRegion = Rectangle.get_Empty();

		private Rectangle _descriptionRegion = Rectangle.get_Empty();

		private Rectangle _authorAvatarRegion = Rectangle.get_Empty();

		private Rectangle _authorNameRegion = Rectangle.get_Empty();

		private Rectangle _downloadsIconRegion = Rectangle.get_Empty();

		private Rectangle _downloadsValueRegion = Rectangle.get_Empty();

		private Rectangle _releaseNotesCornerRegion = Rectangle.get_Empty();

		private readonly BlueButton _ctrlMoreInfoBttn;

		private readonly BlueButton _ctrlReleaseNotesBttn;

		private readonly StandardButton _ctrlActionBttn;

		private readonly Dropdown _ctrlVersionDd;

		private string _downloadCount = "0";

		private static readonly BitmapFont _fontModuleName;

		private Stopwatch _hoverStart = Stopwatch.StartNew();

		public Module Module { get; set; }

		public Author Author { get; set; }

		public int HeroSize
		{
			get
			{
				return _heroSize;
			}
			set
			{
				((Control)this).SetProperty<int>(ref _heroSize, value, true, "HeroSize");
			}
		}

		private int MeasuredHeight => _heroSize + 20 + 50;

		public bool HasUpdate => _installService.GetInstallState(Module) == ModuleInstallState.UpdateAvailable;

		public HModuleCard(Module module, Author author, TextureLoader textureLoader, ModuleInstallService installService)
			: this()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Expected O, but got Unknown
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Expected O, but got Unknown
			Module = module;
			Author = author;
			_installService = installService;
			_installState = _installService.GetInstallState(module);
			_pendingUpdate = _installState == ModuleInstallState.UpdateAvailable;
			_maskEffect = ModuleManagerPlus.MaskEffect;
			_backgroundMask = textureLoader.LoadTextureFromRef("textures/blackcarousel-tile_default.png");
			_avatarMask = textureLoader.LoadTextureFromRef("textures/avatar_mask.png");
			_downloadsTexture = textureLoader.LoadTextureFromRef("textures/downloads.png");
			_cornerTexture = textureLoader.LoadTextureFromRef("textures/corner-grid.png");
			_cornerGradientTexture = textureLoader.LoadTextureFromRef("textures/bg.png");
			if (module.HeroUrl != null)
			{
				_heroTexture = textureLoader.LoadTextureFromWeb(module.HeroUrl);
			}
			if (author.AvatarUrl != null)
			{
				_authorTexture = textureLoader.LoadTextureFromWeb(author.AvatarUrl);
			}
			_downloadCount = FormatDownloads(module.TotalDownloads);
			BlueButton obj = new BlueButton
			{
				Text = "More Info"
			};
			((Control)obj).set_Width(96);
			((Control)obj).set_Parent((Container)(object)this);
			_ctrlMoreInfoBttn = obj;
			if (!Module.HasMoreInfo)
			{
				((Control)_ctrlMoreInfoBttn).set_Enabled(false);
				((Control)_ctrlMoreInfoBttn).set_BasicTooltipText("No additional info is available for this module.");
			}
			BlueButton obj2 = new BlueButton
			{
				Text = "Release Notes"
			};
			((Control)obj2).set_Width(128);
			((Control)obj2).set_Parent((Container)(object)this);
			_ctrlReleaseNotesBttn = obj2;
			StandardButton val = new StandardButton();
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Width(96);
			((Control)val).set_Parent((Container)(object)this);
			_ctrlActionBttn = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(96);
			((Control)val2).set_BasicTooltipText("Select Version");
			((Control)val2).set_Parent((Container)(object)this);
			_ctrlVersionDd = val2;
			PopulateVersionDropdown(showPrereleases: false);
			RefreshActionButton();
			((Control)_ctrlActionBttn).add_Click((EventHandler<MouseEventArgs>)_ctrlActionBttn_Click);
			_ctrlVersionDd.add_ValueChanged((EventHandler<ValueChangedEventArgs>)_ctrlVersionDd_ValueChanged);
			((Control)_ctrlMoreInfoBttn).add_Click((EventHandler<MouseEventArgs>)_ctrlMoreInfoBttn_Click);
			((Control)_ctrlReleaseNotesBttn).add_Click((EventHandler<MouseEventArgs>)_ctrlReleaseNotesBttn_Click);
			((Control)this).Invalidate();
		}

		private static string FormatDownloads(int? num)
		{
			if (!num.HasValue)
			{
				return "0";
			}
			double value = num.Value;
			if (value >= 1000000.0)
			{
				return (value / 1000000.0).ToString("0.#") + "m";
			}
			if (value >= 1000.0)
			{
				return (value / 1000.0).ToString("0.#") + "k";
			}
			return value.ToString();
		}

		private SemVer.Version GetInstalledVersion()
		{
			ModuleManager obj = _installService.FindInstalledModule(Module);
			if (obj == null)
			{
				return null;
			}
			return obj.get_Manifest().get_Version();
		}

		private Release GetSelectedRelease()
		{
			return Module.Releases.SingleOrDefault((Release r) => "v" + r.Version == _ctrlVersionDd.get_SelectedItem());
		}

		private void RefreshActionButton()
		{
			SemVer.Version installedVersion = GetInstalledVersion();
			Release selectedRelease = GetSelectedRelease();
			if (installedVersion == null)
			{
				_ctrlActionBttn.set_Text("Install");
				((Control)_ctrlActionBttn).set_Enabled(true);
				return;
			}
			if (selectedRelease == null)
			{
				_ctrlActionBttn.set_Text("Installed");
				((Control)_ctrlActionBttn).set_Enabled(false);
				return;
			}
			SemVer.Version selectedVersion = selectedRelease.TypedVersion;
			if (selectedVersion > installedVersion)
			{
				_ctrlActionBttn.set_Text("Update");
				((Control)_ctrlActionBttn).set_Enabled(true);
			}
			else if (selectedVersion < installedVersion)
			{
				_ctrlActionBttn.set_Text("Downgrade");
				((Control)_ctrlActionBttn).set_Enabled(true);
			}
			else
			{
				_ctrlActionBttn.set_Text("Installed");
				((Control)_ctrlActionBttn).set_Enabled(false);
			}
		}

		private void _ctrlVersionDd_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			RefreshActionButton();
		}

		public void PopulateVersionDropdown(bool showPrereleases)
		{
			string previousSelection = _ctrlVersionDd.get_SelectedItem();
			_ctrlVersionDd.get_Items().Clear();
			SemVer.Version installedVersion = GetInstalledVersion();
			foreach (Release release in Module.Releases.OrderByDescending((Release r) => r.TypedVersion))
			{
				if (showPrereleases || !release.IsPrerelease || (!(installedVersion == null) && !(release.TypedVersion != installedVersion)))
				{
					_ctrlVersionDd.get_Items().Add("v" + release.Version);
				}
			}
			if (previousSelection != null && _ctrlVersionDd.get_Items().Contains(previousSelection))
			{
				_ctrlVersionDd.set_SelectedItem(previousSelection);
			}
			else if (_ctrlVersionDd.get_Items().Count > 0)
			{
				_ctrlVersionDd.set_SelectedItem(_ctrlVersionDd.get_Items().First());
			}
			RefreshActionButton();
		}

		public async Task<bool> PerformUpdate(IProgress<string> progress = null)
		{
			Release latestStable = (from r in Module.Releases
				where !r.IsPrerelease
				orderby r.TypedVersion descending
				select r).FirstOrDefault();
			if (latestStable == null)
			{
				return false;
			}
			((Control)_ctrlActionBttn).set_Enabled(false);
			_ctrlActionBttn.set_Text("Updating...");
			(bool, string) obj = await _installService.UpdateModule(Module, latestStable, progress);
			bool success = obj.Item1;
			string error = obj.Item2;
			_installState = _installService.GetInstallState(Module);
			_pendingUpdate = _installState == ModuleInstallState.UpdateAvailable;
			RefreshActionButton();
			if (!success)
			{
				((Control)_ctrlActionBttn).set_BasicTooltipText(error);
			}
			return success;
		}

		private async void _ctrlActionBttn_Click(object sender, MouseEventArgs e)
		{
			Release selectedRelease = GetSelectedRelease();
			if (selectedRelease != null)
			{
				Progress<string> progress = new Progress<string>(delegate(string msg)
				{
					((Control)_ctrlActionBttn).set_BasicTooltipText(msg);
				});
				string previousText = _ctrlActionBttn.get_Text();
				((Control)_ctrlActionBttn).set_Enabled(false);
				bool success;
				string error;
				if (GetInstalledVersion() == null)
				{
					_ctrlActionBttn.set_Text("Installing...");
					(success, error) = await _installService.InstallModule(Module, selectedRelease, progress);
				}
				else
				{
					_ctrlActionBttn.set_Text((previousText == "Downgrade") ? "Downgrading..." : "Updating...");
					(success, error) = await _installService.UpdateModule(Module, selectedRelease, progress);
				}
				_installState = _installService.GetInstallState(Module);
				_pendingUpdate = _installState == ModuleInstallState.UpdateAvailable;
				RefreshActionButton();
				if (!success)
				{
					_ctrlActionBttn.set_Text(previousText);
					((Control)_ctrlActionBttn).set_Enabled(true);
					((Control)_ctrlActionBttn).set_BasicTooltipText(error);
				}
			}
		}

		private void _ctrlMoreInfoBttn_Click(object sender, MouseEventArgs e)
		{
			try
			{
				Process.Start("https://blishhud.com/modules/?module=" + Module.Namespace);
			}
			catch (Exception)
			{
			}
		}

		private void _ctrlReleaseNotesBttn_Click(object sender, MouseEventArgs e)
		{
			try
			{
				Process.Start("https://blishhud.com/modules/?module=" + Module.Namespace + "#releases");
			}
			catch (Exception)
			{
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			if (_ctrlActionBttn != null && _ctrlVersionDd != null && _ctrlMoreInfoBttn != null)
			{
				((Control)this).set_Size(new Point(693, MeasuredHeight));
				_heroRegion = new Rectangle(10, 10, _heroSize, _heroSize);
				_footerRegion = new Rectangle(0, ((Control)this).get_Height() - 50, 693, 50);
				_nameRegion = new Rectangle(((Rectangle)(ref _heroRegion)).get_Right() + 10, 10, 693 - _heroSize - 20, 25);
				_descriptionRegion = new Rectangle(((Rectangle)(ref _heroRegion)).get_Right() + 10, ((Rectangle)(ref _nameRegion)).get_Bottom() + 10, 693 - _heroSize - 25, MeasuredHeight - 25);
				_authorAvatarRegion = new Rectangle(10, ((Rectangle)(ref _footerRegion)).get_Top() + _footerRegion.Height / 2 - 16, 32, 32);
				_authorNameRegion = new Rectangle(((Rectangle)(ref _authorAvatarRegion)).get_Right() + 10, ((Rectangle)(ref _footerRegion)).get_Top(), _footerRegion.Width - ((Rectangle)(ref _authorAvatarRegion)).get_Right() - 10, _footerRegion.Height);
				((Control)_ctrlActionBttn).set_Location(new Point(((Control)this).get_Width() - ((Control)_ctrlActionBttn).get_Width() - 10, ((Rectangle)(ref _footerRegion)).get_Top() + _footerRegion.Height / 2 - ((Control)_ctrlActionBttn).get_Height() / 2));
				((Control)_ctrlVersionDd).set_Location(new Point(((Control)_ctrlActionBttn).get_Left() - ((Control)_ctrlVersionDd).get_Width() - 5, ((Rectangle)(ref _footerRegion)).get_Top() + _footerRegion.Height / 2 - ((Control)_ctrlVersionDd).get_Height() / 2));
				((Control)_ctrlMoreInfoBttn).set_Location(new Point(((Rectangle)(ref _heroRegion)).get_Right() + 10, ((Control)_ctrlActionBttn).get_Top()));
				((Control)_ctrlReleaseNotesBttn).set_Location(new Point(((Control)_ctrlMoreInfoBttn).get_Right() + 5, ((Control)_ctrlActionBttn).get_Top()));
				_downloadsIconRegion = new Rectangle(((Control)_ctrlReleaseNotesBttn).get_Right() + 5, ((Rectangle)(ref _footerRegion)).get_Top() + _footerRegion.Height / 2 - 12, 24, 24);
				_downloadsValueRegion = new Rectangle(((Rectangle)(ref _downloadsIconRegion)).get_Right(), ((Rectangle)(ref _footerRegion)).get_Top(), 40, _footerRegion.Height);
				_releaseNotesCornerRegion = RectangleExtension.OffsetBy(_cornerGradientTexture.get_Bounds(), ((Control)this).get_Width() - _cornerGradientTexture.get_Width(), 0);
				((Control)this).RecalculateLayout();
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			Point localMousePos = ((Control)this).get_RelativeMousePosition();
			if (((Rectangle)(ref _downloadsValueRegion)).Contains(localMousePos) || ((Rectangle)(ref _downloadsIconRegion)).Contains(localMousePos))
			{
				((Control)this).set_BasicTooltipText($"Total Downloads: {Module.TotalDownloads:N0}");
			}
			else
			{
				((Control)this).set_BasicTooltipText(string.Empty);
			}
			((Control)this).OnMouseMoved(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			_hoverStart = Stopwatch.StartNew();
			((Control)this).OnMouseEntered(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			_hoverStart = Stopwatch.StartNew();
			((Control)this).OnMouseLeft(e);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).get_MouseOver();
			if (((Control)this).get_MouseOver())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * (0.15f + MathHelper.Clamp((float)_hoverStart.ElapsedMilliseconds / 2000f, 0f, 0.1f)));
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _footerRegion, Color.get_Black() * (0.2f + MathHelper.Clamp((float)_hoverStart.ElapsedMilliseconds / 2000f, 0f, 0.1f)));
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_Black() * 0.15f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _footerRegion, Color.get_Black() * 0.2f);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Module.Name, _fontModuleName, _nameRegion, Color.get_White(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			spriteBatch.DrawStringOnCtrl((Control)(object)this, Module.Description, GameService.Content.get_DefaultFont18(), _descriptionRegion, Color.get_LightGray(), wrap: true, stroke: false, 1, (HorizontalAlignment)0, (VerticalAlignment)0, _descriptionRegion);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Author.Name, GameService.Content.get_DefaultFont18(), _authorNameRegion, Color.get_LightGray(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			if (_pendingUpdate)
			{
				Color.FromNonPremultiplied(113, 163, 216, 255);
				Color.FromNonPremultiplied(79, 157, 254, 255);
				Color cornerYellow = Color.FromNonPremultiplied(245, 177, 73, 255);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _cornerGradientTexture, _releaseNotesCornerRegion);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "  New Update!", GameService.Content.get_DefaultFont14(), _releaseNotesCornerRegion, cornerYellow, false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			else if (_newModule)
			{
				Color.FromNonPremultiplied(113, 163, 216, 255);
				Color.FromNonPremultiplied(79, 157, 254, 255);
				Color cornerBlue = Color.FromNonPremultiplied(52, 179, 255, 255);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _cornerGradientTexture, _releaseNotesCornerRegion);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "  New Module!", GameService.Content.get_DefaultFont14(), _releaseNotesCornerRegion, cornerBlue, false, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			Color statsClr = Color.FromNonPremultiplied(200, 193, 175, 255);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _downloadsTexture, _downloadsIconRegion, statsClr);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _downloadCount, GameService.Content.get_DefaultFont14(), _downloadsValueRegion, statsClr, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			spriteBatch.End();
			spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.LinearWrap, (DepthStencilState)null, _scissorOn, _maskEffect, (Matrix?)GameService.Graphics.get_UIScaleTransform());
			if (_heroTexture != null && _heroTexture.get_HasTexture())
			{
				_maskEffect.get_Parameters().get_Item("Mask").SetValue((Texture)(object)_backgroundMask);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_heroTexture), _heroRegion, Color.get_White());
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, _heroRegion);
			}
			if (_authorTexture != null && _authorTexture.get_HasTexture())
			{
				_maskEffect.get_Parameters().get_Item("Mask").SetValue((Texture)(object)_avatarMask);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_authorTexture), _authorAvatarRegion, Color.get_White());
			}
			else
			{
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, _authorAvatarRegion);
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			if (_pendingUpdate)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _cornerTexture, _cornerTexture.get_Bounds(), _newUpdateColor);
			}
			else if (_newModule)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _cornerTexture, _cornerTexture.get_Bounds(), _newModuleColor);
			}
		}

		static HModuleCard()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			RasterizerState val = new RasterizerState();
			val.set_CullMode((CullMode)0);
			val.set_ScissorTestEnable(true);
			_scissorOn = val;
			_fontModuleName = GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0);
		}
	}
}
