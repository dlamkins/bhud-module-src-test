using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Flurl;
using Flurl.Http;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;

namespace HomeDesigner
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		private CornerIcon cornerIcon;

		private DesignerWindow designerWindow;

		private GraphicsDevice gd;

		private BlueprintRenderer _blueprintRenderer;

		private RendererControl _rendererControl;

		private SettingEntry<int> renderDistance;

		private SettingEntry<int> gizmoSize;

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			gd = GameService.Graphics.get_GraphicsDeviceManager().get_GraphicsDevice();
			_blueprintRenderer = new BlueprintRenderer(gd, ContentsManager);
			_rendererControl = new RendererControl(_blueprintRenderer);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			renderDistance = settings.DefineSetting<int>("Render Distance", 1000, (Func<string>)(() => "Render Distance"), (Func<string>)(() => "Sets the distance for visible Blueprints"));
			SettingComplianceExtensions.SetRange(renderDistance, 0, 1000);
			renderDistance.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate
			{
				_blueprintRenderer.renderDistance = renderDistance.get_Value();
			});
			gizmoSize = settings.DefineSetting<int>("Gizmo Size", 5, (Func<string>)(() => "Gizmo Size"), (Func<string>)(() => "Sets the size of your editing tools"));
			SettingComplianceExtensions.SetRange(gizmoSize, 1, 10);
			gizmoSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)delegate
			{
				_blueprintRenderer.gizmoSize = gizmoSize.get_Value();
			});
		}

		protected override void Initialize()
		{
		}

		protected override async Task LoadAsync()
		{
			Module module = this;
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("CornerIcon.png")));
			val.set_Priority(61747774);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Visible(false);
			module.cornerIcon = val;
			BlueprintRenderer blueprintRenderer = _blueprintRenderer;
			blueprintRenderer.decoCategories = await LoadDecoCategories();
			blueprintRenderer = _blueprintRenderer;
			blueprintRenderer.decorationLut = await "https://bhm.blishhud.com/gw2stacks_blish/item_storage/decorationLUT.json".WithHeader("User-Agent", "Blish-HUD").GetJsonAsync<DecorationLUT>(default(CancellationToken), (HttpCompletionOption)0);
			if (Directory.EnumerateFiles(DirectoriesManager.GetFullDirectoryPath("HomeDesigner"), "*", SearchOption.AllDirectories).ToList().Count < _blueprintRenderer.decorationLut.decorations.Count)
			{
				foreach (KeyValuePair<int, Decoration> deco2 in _blueprintRenderer.decorationLut.decorations)
				{
					AsyncTexture2D texture2 = AsyncTexture2D.FromAssetId(deco2.Value.icon);
					if (texture2 == null)
					{
						texture2 = AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("Icons/placeholder.png"));
					}
					_blueprintRenderer.decoIconDict[deco2.Key] = texture2;
				}
				saveDecoIcons();
			}
			else
			{
				foreach (KeyValuePair<int, Decoration> deco in _blueprintRenderer.decorationLut.decorations)
				{
					Texture2D texture = loadDecoIcon(deco.Key);
					_blueprintRenderer.decoIconDict[deco.Key] = AsyncTexture2D.op_Implicit(texture);
				}
			}
			await Task.Delay(75);
			((Control)cornerIcon).set_Visible(true);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
			{
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				foreach (KeyValuePair<int, Decoration> current in _blueprintRenderer.decorationLut.decorations)
				{
					_blueprintRenderer.LoadModel(current.Value.id.ToString(), $"models/{current.Value.id}.obj", Vector3.get_Zero());
				}
				designerWindow = new DesignerWindow(ContentsManager, _rendererControl, _blueprintRenderer);
				initializeDesignerTool();
				((Control)cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					((WindowBase2)designerWindow).ToggleWindow();
				});
			});
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			if (_rendererControl != null && designerWindow != null)
			{
				designerWindow.designerView.RefreshSelectedList();
			}
		}

		protected override void Unload()
		{
			designerWindow?.unload();
			DesignerWindow obj = designerWindow;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			_rendererControl.unload();
			RendererControl rendererControl = _rendererControl;
			if (rendererControl != null)
			{
				((Control)rendererControl).Dispose();
			}
			_blueprintRenderer?.Dispose();
			CornerIcon obj2 = cornerIcon;
			if (obj2 != null)
			{
				((Control)obj2).Dispose();
			}
		}

		private Task saveDecoIcons()
		{
			string folder = DirectoriesManager.GetFullDirectoryPath("HomeDesigner");
			return Task.Run(delegate
			{
				foreach (KeyValuePair<int, AsyncTexture2D> current in _blueprintRenderer.decoIconDict)
				{
					string path = Path.Combine(folder, current.Key + ".png");
					try
					{
						if (!File.Exists(path))
						{
							using FileStream fileStream = File.Create(path);
							current.Value.get_Texture().SaveAsPng((Stream)fileStream, current.Value.get_Width(), current.Value.get_Height());
						}
					}
					catch (Exception)
					{
					}
				}
			});
		}

		private Texture2D loadDecoIcon(int decoKey)
		{
			string fullDirectoryPath = DirectoriesManager.GetFullDirectoryPath("HomeDesigner");
			Directory.CreateDirectory(fullDirectoryPath);
			string filePath = Path.Combine(fullDirectoryPath, $"{decoKey}.png");
			if (File.Exists(filePath))
			{
				using (FileStream stream = File.OpenRead(filePath))
				{
					return Texture2D.FromStream(gd, (Stream)stream);
				}
			}
			return ContentsManager.GetTexture("Icons/placeholder.png");
		}

		private async Task<Dictionary<int, string>> LoadDecoCategories()
		{
			JArray obj = await "https://api.guildwars2.com/v2/homestead/decorations/categories".SetQueryParam("ids", string.Join(",", await "https://api.guildwars2.com/v2/homestead/decorations/categories".GetJsonAsync<List<int>>(default(CancellationToken), (HttpCompletionOption)0))).GetJsonAsync<JArray>(default(CancellationToken), (HttpCompletionOption)0);
			Dictionary<int, string> dict = new Dictionary<int, string>();
			foreach (JToken item in obj)
			{
				int id = item.Value<int>("id");
				string name = (dict[id] = item.Value<string>("name"));
			}
			return dict;
		}

		private void initializeDesignerTool()
		{
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			_blueprintRenderer.LoadGizmoModel("translate_X", "gizmos/Gizmo_Translate_X.obj");
			_blueprintRenderer.LoadGizmoModel("translate_Y", "gizmos/Gizmo_Translate_Y.obj");
			_blueprintRenderer.LoadGizmoModel("translate_Z", "gizmos/Gizmo_Translate_Z.obj");
			_blueprintRenderer.LoadGizmoModel("rotate_X", "gizmos/Gizmo_Rotate_X.obj");
			_blueprintRenderer.LoadGizmoModel("rotate_Y", "gizmos/Gizmo_Rotate_Y.obj");
			_blueprintRenderer.LoadGizmoModel("rotate_Z", "gizmos/Gizmo_Rotate_Z.obj");
			_blueprintRenderer.LoadGizmoModel("scale_X", "gizmos/Gizmo_Scale_X.obj");
			_blueprintRenderer.LoadGizmoModel("scale_Y", "gizmos/Gizmo_Scale_Y.obj");
			_blueprintRenderer.LoadGizmoModel("scale_Z", "gizmos/Gizmo_Scale_Z.obj");
			_rendererControl.AddTranslateGizmos(new BlueprintObject
			{
				ModelKey = "translate_Z",
				Position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				Rotation = new Vector3(0f, 0f, 0f),
				Scale = 0.05f
			});
			_rendererControl.AddTranslateGizmos(new BlueprintObject
			{
				ModelKey = "translate_Y",
				Position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				Rotation = new Vector3(0f, 0f, 0f),
				Scale = 0.05f
			});
			_rendererControl.AddTranslateGizmos(new BlueprintObject
			{
				ModelKey = "translate_X",
				Position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				Rotation = new Vector3(0f, 0f, 0f),
				Scale = 0.05f
			});
			_rendererControl.AddRotateGizmos(new BlueprintObject
			{
				ModelKey = "rotate_Y",
				Position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				Rotation = new Vector3(0f, 0f, 0f),
				Scale = 0.05f
			});
			_rendererControl.AddRotateGizmos(new BlueprintObject
			{
				ModelKey = "rotate_Z",
				Position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				Rotation = new Vector3(0f, 0f, 0f),
				Scale = 0.05f
			});
			_rendererControl.AddRotateGizmos(new BlueprintObject
			{
				ModelKey = "rotate_X",
				Position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				Rotation = new Vector3(0f, 0f, 0f),
				Scale = 0.05f
			});
			_rendererControl.AddScaleGizmos(new BlueprintObject
			{
				ModelKey = "scale_Z",
				Position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				Rotation = new Vector3(0f, 0f, 0f),
				Scale = 0.05f
			});
			_rendererControl.AddScaleGizmos(new BlueprintObject
			{
				ModelKey = "scale_Y",
				Position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				Rotation = new Vector3(0f, 0f, 0f),
				Scale = 0.05f
			});
			_rendererControl.AddScaleGizmos(new BlueprintObject
			{
				ModelKey = "scale_X",
				Position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				Rotation = new Vector3(0f, 0f, 0f),
				Scale = 0.05f
			});
			_rendererControl.updateWorld();
			_rendererControl.updateGizmos();
		}
	}
}
