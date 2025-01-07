using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics;
using Blish_HUD.Input;
using DecorBlishhudModule.CustomControls;
using DecorBlishhudModule.CustomControls.CustomTab;
using DecorBlishhudModule.Homestead;
using DecorBlishhudModule.Model;
using DecorBlishhudModule.Sections;
using DecorBlishhudModule.Sections.LeftSideTasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DecorBlishhudModule
{
	public class LeftSideSection
	{
		private static readonly Logger Logger = Logger.GetLogger<DecorModule>();

		private static readonly Dictionary<string, Texture2D> _sharedTextureCache = new Dictionary<string, Texture2D>();

		private static Dictionary<string, List<Decoration>> _homesteadDecorationsCache;

		private static Dictionary<string, List<Decoration>> _guildHallDecorationsCache;

		private static Panel lastClickedIconPanel = null;

		private static bool isOperationRunning = false;

		private static Task<Dictionary<string, List<Decoration>>> FetchHomesteadDecorationsAsync()
		{
			if (_homesteadDecorationsCache == null)
			{
				return RefreshHomesteadDecorationsAsync();
			}
			return Task.FromResult(_homesteadDecorationsCache);
		}

		private static Task<Dictionary<string, List<Decoration>>> FetchGuildHallDecorationsAsync()
		{
			if (_guildHallDecorationsCache == null)
			{
				return RefreshGuildHallDecorationsAsync();
			}
			return Task.FromResult(_guildHallDecorationsCache);
		}

		private static async Task<Dictionary<string, List<Decoration>>> RefreshHomesteadDecorationsAsync()
		{
			_homesteadDecorationsCache = await HomesteadDecorationFetcher.FetchDecorationsAsync();
			return _homesteadDecorationsCache;
		}

		private static async Task<Dictionary<string, List<Decoration>>> RefreshGuildHallDecorationsAsync()
		{
			_guildHallDecorationsCache = await GuildHallDecorationFetcher.FetchDecorationsAsync();
			return _guildHallDecorationsCache;
		}

		public static async Task PopulateHomesteadIconsInFlowPanel(FlowPanel homesteadDecorationsFlowPanel, bool _isIconView)
		{
			Dictionary<string, List<Decoration>> obj = await FetchHomesteadDecorationsAsync();
			Dictionary<string, List<Decoration>> caseInsensitiveCategories = new Dictionary<string, List<Decoration>>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, List<Decoration>> kvp in obj)
			{
				caseInsensitiveCategories[kvp.Key] = kvp.Value;
			}
			await Task.WhenAll((from category in HomesteadCategories.GetCategories()
				where caseInsensitiveCategories.ContainsKey(category)
				select category).Select((Func<string, Task>)async delegate(string category)
			{
				int heightIncrementPerDecorationSet = 52;
				List<Decoration> decorations = caseInsensitiveCategories[category];
				int numDecorationSets = (int)Math.Ceiling((double)decorations.Count / 9.0);
				int calculatedHeight = 45 + numDecorationSets * heightIncrementPerDecorationSet;
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent((Container)(object)homesteadDecorationsFlowPanel);
				((Panel)val).set_Title(category);
				val.set_FlowDirection((ControlFlowDirection)0);
				((Control)val).set_Width(((Control)homesteadDecorationsFlowPanel).get_Width() - 20);
				((Control)val).set_Height(calculatedHeight);
				((Panel)val).set_CanCollapse(false);
				val.set_ControlPadding(new Vector2(4f, 4f));
				val.set_OuterControlPadding(new Vector2(6f, 4f));
				FlowPanel categoryFlowPanel = val;
				await Task.WhenAll(decorations.Select((Decoration decoration) => CreateDecorationIconsImagesAsync(decoration, categoryFlowPanel, _isIconView)));
			}));
			await OrderDecorations.OrderDecorationsAsync(homesteadDecorationsFlowPanel, _isIconView);
		}

		public static async Task PopulateGuildHallIconsInFlowPanel(FlowPanel decorationsFlowPanel, bool _isIconView)
		{
			await Task.WhenAll((await FetchGuildHallDecorationsAsync()).Where((KeyValuePair<string, List<Decoration>> entry) => entry.Value != null && entry.Value.Count > 0).Select((Func<KeyValuePair<string, List<Decoration>>, Task>)async delegate(KeyValuePair<string, List<Decoration>> entry)
			{
				string category = entry.Key;
				List<Decoration> value = entry.Value;
				int baseHeight = 45;
				int heightIncrementPerDecorationSet = 52;
				int numDecorationSets = (int)Math.Ceiling((double)value.Count / 9.0);
				int calculatedHeight = baseHeight + numDecorationSets * heightIncrementPerDecorationSet;
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent((Container)(object)decorationsFlowPanel);
				((Panel)val).set_Title(category);
				val.set_FlowDirection((ControlFlowDirection)0);
				((Control)val).set_Width(((Control)decorationsFlowPanel).get_Width() - 20);
				((Control)val).set_Height(calculatedHeight);
				((Panel)val).set_CanCollapse(false);
				val.set_ControlPadding(new Vector2(4f, 4f));
				val.set_OuterControlPadding(new Vector2(6f, 4f));
				FlowPanel categoryFlowPanel = val;
				await Task.WhenAll(value.Select((Decoration decoration) => CreateDecorationIconsImagesAsync(decoration, categoryFlowPanel, _isIconView)));
			}).ToList());
			await OrderDecorations.OrderDecorationsAsync(decorationsFlowPanel, _isIconView);
		}

		public static async Task PopulateHomesteadBigIconsInFlowPanel(FlowPanel homesteadDecorationsFlowPanel, bool _isIconView)
		{
			Dictionary<string, List<Decoration>> obj = await FetchHomesteadDecorationsAsync();
			Dictionary<string, List<Decoration>> caseInsensitiveCategories = new Dictionary<string, List<Decoration>>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, List<Decoration>> kvp in obj)
			{
				caseInsensitiveCategories[kvp.Key] = kvp.Value;
			}
			await Task.WhenAll((from category in HomesteadCategories.GetCategories()
				where caseInsensitiveCategories.ContainsKey(category)
				select category).Select((Func<string, Task>)async delegate(string category)
			{
				int heightIncrementPerDecorationSet = 312;
				List<Decoration> decorations = caseInsensitiveCategories[category];
				int numDecorationSets = (int)Math.Ceiling((double)decorations.Count / 4.0);
				int calculatedHeight = 45 + numDecorationSets * heightIncrementPerDecorationSet;
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent((Container)(object)homesteadDecorationsFlowPanel);
				((Panel)val).set_Title(category);
				val.set_FlowDirection((ControlFlowDirection)0);
				((Control)val).set_Width(((Control)homesteadDecorationsFlowPanel).get_Width() - 20);
				((Control)val).set_Height(calculatedHeight);
				((Panel)val).set_CanCollapse(true);
				val.set_ControlPadding(new Vector2(8f, 10f));
				val.set_OuterControlPadding(new Vector2(10f, 10f));
				FlowPanel categoryFlowPanel = val;
				await Task.WhenAll(decorations.Select((Decoration decoration) => CreateDecorationIconsImagesAsync(decoration, categoryFlowPanel, _isIconView)));
			}));
			await OrderDecorations.OrderDecorationsAsync(homesteadDecorationsFlowPanel, _isIconView);
		}

		public static async Task PopulateGuildHallBigIconsInFlowPanel(FlowPanel decorationsFlowPanel, bool _isIconView)
		{
			await Task.WhenAll((await FetchGuildHallDecorationsAsync()).Where((KeyValuePair<string, List<Decoration>> entry) => entry.Value != null && entry.Value.Count > 0).Select((Func<KeyValuePair<string, List<Decoration>>, Task>)async delegate(KeyValuePair<string, List<Decoration>> entry)
			{
				string category = entry.Key;
				List<Decoration> value = entry.Value;
				int baseHeight = 45;
				int heightIncrementPerDecorationSet = 312;
				int numDecorationSets = (int)Math.Ceiling((double)value.Count / 4.0);
				int calculatedHeight = baseHeight + numDecorationSets * heightIncrementPerDecorationSet;
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent((Container)(object)decorationsFlowPanel);
				((Panel)val).set_Title(category);
				val.set_FlowDirection((ControlFlowDirection)0);
				((Control)val).set_Width(((Control)decorationsFlowPanel).get_Width() - 20);
				((Control)val).set_Height(calculatedHeight);
				((Panel)val).set_CanCollapse(true);
				val.set_ControlPadding(new Vector2(8f, 10f));
				val.set_OuterControlPadding(new Vector2(10f, 10f));
				FlowPanel categoryFlowPanel = val;
				await Task.WhenAll(value.Select((Decoration decoration) => CreateDecorationIconsImagesAsync(decoration, categoryFlowPanel, _isIconView)));
			}).ToList());
			await OrderDecorations.OrderDecorationsAsync(decorationsFlowPanel, _isIconView);
		}

		public static async Task CreateDecorationIconsImagesAsync(Decoration decoration, FlowPanel categoryFlowPanel, bool _isIconView)
		{
			try
			{
				Texture2D iconTexture = await GetOrCreateTextureAsync(decoration.Name, decoration.IconUrl);
				if (iconTexture == null)
				{
					Logger.Warn("Icon texture for '" + decoration.Name + "' could not be loaded.");
					return;
				}
				if (_isIconView)
				{
					if (iconTexture == null)
					{
						return;
					}
					Panel val = new Panel();
					((Control)val).set_Parent((Container)(object)categoryFlowPanel);
					((Control)val).set_Size(new Point(49, 49));
					((Control)val).set_BackgroundColor(Color.get_Black());
					Panel borderPanel = val;
					Image val2 = new Image(AsyncTexture2D.op_Implicit(iconTexture));
					((Control)val2).set_Parent((Container)(object)borderPanel);
					((Control)val2).set_Size(new Point(45));
					((Control)val2).set_Location(new Point(2, 2));
					((Control)val2).set_BasicTooltipText(decoration.Name);
					Image decorationIconImage = val2;
					((Control)borderPanel).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
					{
						//IL_0013: Unknown result type (might be due to invalid IL or missing references)
						if (lastClickedIconPanel != borderPanel)
						{
							((Control)borderPanel).set_BackgroundColor(Color.get_LightGray());
							((Control)decorationIconImage).set_Opacity(0.75f);
						}
					});
					((Control)borderPanel).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
					{
						//IL_0013: Unknown result type (might be due to invalid IL or missing references)
						if (lastClickedIconPanel != borderPanel)
						{
							((Control)borderPanel).set_BackgroundColor(Color.get_Black());
							((Control)decorationIconImage).set_Opacity(1f);
						}
					});
					DecorModule decorModule;
					((Control)decorationIconImage).add_Click((EventHandler<MouseEventArgs>)async delegate
					{
						if (!isOperationRunning)
						{
							CustomTabbedWindow2 decorWindow2 = DecorModule.DecorModuleInstance.DecorWindow;
							bool loaded = DecorModule.DecorModuleInstance.Loaded;
							if (lastClickedIconPanel != null && ((Control)lastClickedIconPanel).get_BackgroundColor() == new Color(254, 254, 176))
							{
								((Control)lastClickedIconPanel).set_BackgroundColor(Color.get_Black());
								((Control)decorationIconImage).set_Opacity(1f);
							}
							((Control)borderPanel).set_BackgroundColor(new Color(254, 254, 176));
							((Control)decorationIconImage).set_Opacity(1f);
							lastClickedIconPanel = borderPanel;
							LoadingSpinner val12 = new LoadingSpinner();
							((Control)val12).set_Parent((Container)(object)decorWindow2);
							((Control)val12).set_Size(new Point(32, 32));
							((Control)val12).set_Location(new Point(727, 320));
							LoadingSpinner loaderSpinner = val12;
							Label val13 = new Label();
							((Control)val13).set_Parent((Container)(object)decorWindow2);
							val13.set_Text("Loading...");
							val13.set_Font(GameService.Content.get_DefaultFont16());
							((Control)val13).set_Location(new Point(762, 325));
							val13.set_HorizontalAlignment((HorizontalAlignment)1);
							val13.set_AutoSizeWidth(true);
							Label loadingLabel = val13;
							Label loadingLabel2 = new Label();
							if (!loaded)
							{
								Label val14 = new Label();
								((Control)val14).set_Parent((Container)(object)decorWindow2);
								val14.set_Text("The image may take longer as the full data is fetched.");
								val14.set_Font(GameService.Content.get_DefaultFont16());
								((Control)val14).set_Location(new Point(620, 350));
								val14.set_HorizontalAlignment((HorizontalAlignment)1);
								val14.set_AutoSizeWidth(true);
								loadingLabel2 = val14;
							}
							isOperationRunning = true;
							try
							{
								decorModule = DecorModule.DecorModuleInstance;
								await Task.Run(async delegate
								{
									try
									{
										await RightSideSection.UpdateDecorationImageAsync(decoration, (Container)(object)decorModule.DecorWindow, decorModule.DecorationImage);
									}
									catch (Exception ex2)
									{
										Console.WriteLine("Error occurred during decoration image update: " + ex2.Message);
									}
								});
							}
							finally
							{
								isOperationRunning = false;
								((Control)loaderSpinner).Dispose();
								((Control)loadingLabel).Dispose();
								((Control)loadingLabel2).Dispose();
							}
						}
					});
					return;
				}
				Texture2D imageTexture = await GetOrCreateTextureAsync(decoration.Name + "_Image", decoration.ImageUrl);
				if (imageTexture == null)
				{
					return;
				}
				BorderPanel borderPanel2 = new BorderPanel();
				((Control)borderPanel2).set_Parent((Container)(object)categoryFlowPanel);
				((Control)borderPanel2).set_Size(new Point(254, 300));
				((Control)borderPanel2).set_BackgroundColor(new Color(0, 0, 0, 36));
				((Control)borderPanel2).set_BasicTooltipText(decoration.Name);
				BorderPanel mainContainer = borderPanel2;
				Panel val3 = new Panel();
				((Control)val3).set_Parent((Container)(object)mainContainer);
				((Control)val3).set_Location(new Point(0, 0));
				((Control)val3).set_Size(new Point(256, 50));
				((Control)val3).set_BasicTooltipText(decoration.Name);
				val3.set_BackgroundTexture(AsyncTexture2D.op_Implicit(DecorModule.DecorModuleInstance.BlackTexture));
				Panel iconTextContainer = val3;
				Image val4 = new Image(AsyncTexture2D.op_Implicit(iconTexture));
				((Control)val4).set_Parent((Container)(object)iconTextContainer);
				((Control)val4).set_Location(new Point(3, 3));
				((Control)val4).set_Size(new Point(44, 44));
				((Control)val4).set_BasicTooltipText(decoration.Name);
				Image iconImage = val4;
				Label val5 = new Label();
				((Control)val5).set_Parent((Container)(object)iconTextContainer);
				((Control)val5).set_Size(new Point(190, 40));
				((Control)val5).set_Location(new Point(((Control)iconImage).get_Location().X + ((Control)iconImage).get_Size().X + 8, 5));
				val5.set_Text(decoration.Name);
				val5.set_Font((decoration.Name.ToString().Length > 30) ? GameService.Content.get_DefaultFont12() : GameService.Content.get_DefaultFont14());
				val5.set_HorizontalAlignment((HorizontalAlignment)0);
				val5.set_VerticalAlignment((VerticalAlignment)1);
				((Control)val5).set_BasicTooltipText(decoration.Name);
				int width2 = imageTexture.get_Width();
				int imageHeight = imageTexture.get_Height();
				float aspectRatio = (float)width2 / (float)imageHeight;
				int width = 245;
				int height = (int)(245f / aspectRatio);
				if (height > 245)
				{
					height = 245;
					width = (int)(245f * aspectRatio);
				}
				int xOffset = (((Control)mainContainer).get_Size().X - width) / 2;
				int yOffset = ((Control)iconTextContainer).get_Location().Y + ((Control)iconTextContainer).get_Size().Y;
				int centeredYOffset = (((Control)mainContainer).get_Size().Y - ((Control)iconTextContainer).get_Size().Y - height) / 2 + yOffset;
				Image val6 = new Image(AsyncTexture2D.op_Implicit(imageTexture));
				((Control)val6).set_Parent((Container)(object)mainContainer);
				((Control)val6).set_Location(new Point(xOffset + 2, centeredYOffset - 1));
				((Control)val6).set_Size(new Point(width - 3, height));
				((Control)val6).set_BasicTooltipText(decoration.Name);
				Image decorationImage = val6;
				((Control)mainContainer).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					//IL_001b: Unknown result type (might be due to invalid IL or missing references)
					if (lastClickedIconPanel != mainContainer)
					{
						((Control)mainContainer).set_BackgroundColor(new Color(101, 101, 84, 36));
					}
				});
				((Control)mainContainer).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					if (lastClickedIconPanel != mainContainer)
					{
						((Control)mainContainer).set_BackgroundColor(new Color(0, 0, 0, 36));
					}
				});
				CustomTabbedWindow2 decorWindow = DecorModule.DecorModuleInstance.DecorWindow;
				Texture2D borderedTexture = BorderCreator.CreateBorderedTexture(imageTexture);
				((Control)decorationImage).add_Click((EventHandler<MouseEventArgs>)async delegate
				{
					decorWindow = DecorModule.DecorModuleInstance.DecorWindow;
					Image val11 = new Image(AsyncTexture2D.op_Implicit(borderedTexture));
					((Control)val11).set_Parent((Container)(object)decorWindow);
					decorationImage = val11;
					await BigImageSection.UpdateDecorationImageAsync(decoration, (Container)(object)decorWindow, decorationImage);
				});
				Panel val7 = new Panel();
				((Control)val7).set_Parent((Container)(object)mainContainer);
				((Control)val7).set_Size(new Point(24, 24));
				((Control)val7).set_Location(new Point(((Control)mainContainer).get_Size().X - 24, 0));
				Panel copyPanelContainer = val7;
				Image val8 = new Image(AsyncTexture2D.op_Implicit(DecorModule.DecorModuleInstance?.CopyIcon));
				((Control)val8).set_Parent((Container)(object)copyPanelContainer);
				((Control)val8).set_Size(((Control)copyPanelContainer).get_Size());
				((Control)val8).set_BasicTooltipText("Copy Name");
				Panel val9 = new Panel();
				((Control)val9).set_Parent((Container)(object)mainContainer);
				((Control)val9).set_Location(new Point(90, 150));
				val9.set_Title("Copied !");
				((Control)val9).set_Width(80);
				((Control)val9).set_Height(45);
				val9.set_ShowBorder(true);
				((Control)val9).set_Opacity(0f);
				((Control)val9).set_Visible(false);
				Panel savePanel = val9;
				Panel val10 = new Panel();
				((Control)val10).set_Parent((Container)(object)copyPanelContainer);
				((Control)val10).set_Size(((Control)copyPanelContainer).get_Size());
				((Control)val10).set_Location(Point.get_Zero());
				((Control)val10).set_BackgroundColor(Color.get_White() * 0.3f);
				((Control)val10).set_Visible(false);
				Panel copyBrightnessOverlay = val10;
				((Control)val8).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					((Control)copyBrightnessOverlay).set_Visible(true);
				});
				((Control)val8).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					((Control)copyBrightnessOverlay).set_Visible(false);
				});
				((Control)copyBrightnessOverlay).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					if (!((Control)savePanel).get_Visible())
					{
						SaveTasks.CopyTextToClipboard(decoration.Name);
						SaveTasks.ShowSavedPanel(savePanel);
					}
				});
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to load decoration icon for '" + decoration.Name + "'. Error: " + ex.Message);
			}
		}

		private static async Task<Texture2D> GetOrCreateTextureAsync(string key, string iconUrl)
		{
			if (_sharedTextureCache.TryGetValue(key, out var existingTexture))
			{
				return existingTexture;
			}
			try
			{
				Texture2D newTexture = CreateIconTexture(await DecorModule.DecorModuleInstance.Client.GetByteArrayAsync(iconUrl));
				if (newTexture != null)
				{
					_sharedTextureCache[key] = newTexture;
				}
				return newTexture;
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to load texture for '" + key + "'. Error: " + ex.Message);
				return null;
			}
		}

		private static Texture2D CreateIconTexture(byte[] iconResponse)
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				using MemoryStream memoryStream = new MemoryStream(iconResponse);
				using Image originalImage = Image.FromStream(memoryStream);
				int maxDimension = 200;
				int newWidth = originalImage.Width;
				int newHeight = originalImage.Height;
				if (originalImage.Width > maxDimension || originalImage.Height > maxDimension)
				{
					float scale = Math.Min((float)maxDimension / (float)originalImage.Width, (float)maxDimension / (float)originalImage.Height);
					newWidth = (int)((float)originalImage.Width * scale);
					newHeight = (int)((float)originalImage.Height * scale);
				}
				using Bitmap resizedImage = new Bitmap(originalImage, newWidth, newHeight);
				using MemoryStream resizedStream = new MemoryStream();
				GraphicsDeviceContext graphicsContext = GameService.Graphics.LendGraphicsDeviceContext();
				try
				{
					resizedImage.Save(resizedStream, ImageFormat.Png);
					resizedStream.Seek(0L, SeekOrigin.Begin);
					return Texture2D.FromStream(((GraphicsDeviceContext)(ref graphicsContext)).get_GraphicsDevice(), (Stream)resizedStream);
				}
				finally
				{
					((GraphicsDeviceContext)(ref graphicsContext)).Dispose();
				}
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to create icon texture. Error: " + ex.Message);
				return null;
			}
		}

		private static void CleanupSharedTextureCache()
		{
			foreach (Texture2D value in _sharedTextureCache.Values)
			{
				((GraphicsResource)value).Dispose();
			}
			_sharedTextureCache.Clear();
			_homesteadDecorationsCache.Clear();
			_guildHallDecorationsCache.Clear();
			Logger.Info("Shared texture cache cleaned up.");
		}

		protected void Unload()
		{
			CleanupSharedTextureCache();
			Unload();
		}
	}
}
