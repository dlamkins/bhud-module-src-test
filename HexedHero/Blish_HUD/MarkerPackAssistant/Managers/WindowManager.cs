using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using HexedHero.Blish_HUD.MarkerPackAssistant.Objects;
using HexedHero.Blish_HUD.MarkerPackAssistant.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Managers
{
	public class WindowManager
	{
		private static Lazy<WindowManager> instance = new Lazy<WindowManager>(() => new WindowManager());

		private CornerIcon cornerIcon;

		private Texture2D iconTexture;

		private AsyncTexture2D emblemTexture;

		private AsyncTexture2D backgroundTexture;

		public static WindowManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Lazy<WindowManager>(() => new WindowManager());
				}
				return instance.Value;
			}
		}

		public StandardWindow MainWindow { get; private set; }

		public AssistanceView AssistanceView { get; private set; }

		private WindowManager()
		{
			Load();
		}

		private void Load()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			iconTexture = MarkerPackAssistant.Instance.Module.get_ContentsManager().GetTexture("102348_modified.png");
			emblemTexture = AsyncTexture2D.FromAssetId(102348);
			backgroundTexture = AsyncTexture2D.FromAssetId(155983);
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(iconTexture));
			((Control)val).set_BasicTooltipText("Marker Pack Assistant");
			val.set_Priority(89157212);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			cornerIcon = val;
			((Control)cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				MainWindow.ToggleWindow((IView)(object)AssistanceView);
			});
			StandardWindow val2 = new StandardWindow(Textures.get_TransparentPixel(), new Rectangle(5, 5, 330, 220), new Rectangle(30, 30, 300, 180));
			((WindowBase2)val2).set_Emblem(Textures.get_TransparentPixel());
			((WindowBase2)val2).set_Title("MPA");
			((Control)val2).set_Location(new Point(100, 100));
			((WindowBase2)val2).set_SavesPosition(true);
			((WindowBase2)val2).set_Id("MarkerPackAssistantMainWindow");
			((Control)val2).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			MainWindow = val2;
			if (backgroundTexture.get_HasSwapped())
			{
				injectBackground();
			}
			else
			{
				backgroundTexture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate
				{
					injectBackground();
				});
			}
			if (emblemTexture.get_HasSwapped())
			{
				injectEmblem();
			}
			else
			{
				emblemTexture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate
				{
					injectEmblem();
				});
			}
			AssistanceView = new AssistanceView();
			void injectBackground()
			{
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				Reflection.InjectNewBackground((WindowBase2)(object)MainWindow, AsyncTexture2D.op_Implicit(backgroundTexture), new Rectangle(-10, 30, 365, 340));
			}
			void injectEmblem()
			{
				((WindowBase2)MainWindow).set_Emblem(AsyncTexture2D.op_Implicit(emblemTexture));
			}
		}

		public void Unload()
		{
			CornerIcon obj = cornerIcon;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			StandardWindow mainWindow = MainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			((View<IPresenter>)(object)AssistanceView)?.DoUnload();
			AssistanceView = null;
			Texture2D obj2 = iconTexture;
			if (obj2 != null)
			{
				((GraphicsResource)obj2).Dispose();
			}
			instance = null;
		}
	}
}
