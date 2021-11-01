using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Timers;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.Random_Generator_Module
{
	[Export(typeof(Module))]
	public class RandomGeneratorModule : Module
	{
		internal static RandomGeneratorModule ModuleInstance;

		private List<Texture2D> _dieTextures = new List<Texture2D>();

		private Panel Die;

		private SettingEntry<int> DieSides;

		private SettingEntry<bool> ToggleShowDieSetting;

		private SettingEntry<bool> ToggleSendToChatSetting;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public RandomGeneratorModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			ToggleShowDieSetting = settings.DefineSetting<bool>("ShowDie", true, "Show die", "Whether a die should be displayed to the right of your skill bar.", (SettingTypeRendererDelegate)null);
			ToggleSendToChatSetting = settings.DefineSetting<bool>("SendToChat", false, "Send to chat", "Whether results should be displayed and emphasised in chat.\nWarning: Can trigger supression of similar messages if results are generated too frequently.", (SettingTypeRendererDelegate)null);
			SettingCollection selfManagedSettings = settings.AddSubCollection("ManagedSettings", false, false);
			DieSides = selfManagedSettings.DefineSetting<int>("DieSides", 6, "Die Sides", "Indicates the amount of sides the die has.", (SettingTypeRendererDelegate)null);
		}

		protected override void Initialize()
		{
			LoadTextures();
			CreateDie();
		}

		private void LoadTextures()
		{
			for (int i = 0; i < 7; i++)
			{
				_dieTextures.Add(ContentsManager.GetTexture($"dice/side{i}.png"));
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			ToggleShowDieSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowDieSettingChanged);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.add_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (Die != null)
			{
				((Control)Die).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 480, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)Die).get_Height() - 25));
			}
		}

		protected override void Unload()
		{
			ToggleShowDieSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnShowDieSettingChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			GameService.GameIntegration.remove_IsInGameChanged((EventHandler<ValueEventArgs<bool>>)OnIsInGameChanged);
			Panel die = Die;
			if (die != null)
			{
				((Control)die).Dispose();
			}
			_dieTextures.Clear();
			_dieTextures = null;
			ModuleInstance = null;
		}

		private bool IsUiAvailable()
		{
			return GameService.Gw2Mumble.get_IsAvailable() && GameService.GameIntegration.get_IsInGame() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen();
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			ToggleControls(!e.get_Value(), 0.45f);
		}

		private void OnIsInGameChanged(object o, ValueEventArgs<bool> e)
		{
			ToggleControls(e.get_Value(), 0.1f);
		}

		private void OnShowDieSettingChanged(object o, ValueChangedEventArgs<bool> e)
		{
			ToggleControls(e.get_NewValue(), 0.1f);
		}

		private void ToggleControls(bool enabled, float tDuration)
		{
			if (enabled)
			{
				CreateDie();
			}
			else
			{
				if (Die == null)
				{
					return;
				}
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(Die, (object)new
				{
					Opacity = 0f
				}, tDuration, 0f, true).OnComplete((Action)delegate
				{
					Panel die = Die;
					if (die != null)
					{
						((Control)die).Dispose();
					}
				});
			}
		}

		private void CreateDie()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Expected O, but got Unknown
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			Panel die = Die;
			if (die != null)
			{
				((Control)die).Dispose();
			}
			if (!ToggleShowDieSetting.get_Value() || !IsUiAvailable())
			{
				return;
			}
			DieSides.set_Value((DieSides.get_Value() > 100 || DieSides.get_Value() < 2) ? 6 : DieSides.get_Value());
			bool rolling = false;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_Size(new Point(64, 64));
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Opacity(0f);
			Die = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)Die);
			val2.set_Texture(AsyncTexture2D.op_Implicit(_dieTextures[0]));
			((Control)val2).set_Size(new Point(64, 64));
			((Control)val2).set_Location(new Point(0, 0));
			Image dieImage = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)Die);
			((Control)val3).set_Size(((Control)Die).get_Size());
			((Control)val3).set_Location(new Point(0, 0));
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			val3.set_VerticalAlignment((VerticalAlignment)1);
			val3.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)22, (FontStyle)0));
			val3.set_ShowShadow(true);
			val3.set_TextColor(Color.get_Black());
			val3.set_ShadowColor(Color.get_Black());
			val3.set_StrokeText(false);
			val3.set_Text("");
			Label dieLabel = val3;
			ApplyDieValue(reset: false);
			bool dieSettingsOpen = false;
			((Control)Die).add_RightMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Expected O, but got Unknown
				//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_011b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0126: Unknown result type (might be due to invalid IL or missing references)
				//IL_012f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0137: Unknown result type (might be due to invalid IL or missing references)
				//IL_014e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0156: Unknown result type (might be due to invalid IL or missing references)
				//IL_0167: Expected O, but got Unknown
				//IL_0167: Unknown result type (might be due to invalid IL or missing references)
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0179: Unknown result type (might be due to invalid IL or missing references)
				//IL_017e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0189: Unknown result type (might be due to invalid IL or missing references)
				//IL_0190: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ca: Expected O, but got Unknown
				if (!(rolling || dieSettingsOpen))
				{
					dieSettingsOpen = true;
					Panel val4 = new Panel();
					((Control)val4).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
					((Control)val4).set_Size(new Point(200, 120));
					((Control)val4).set_Location(new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2 - 100, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2 - 60));
					((Control)val4).set_Opacity(0f);
					val4.set_BackgroundTexture(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("controls/window/502049")));
					val4.set_ShowBorder(true);
					val4.set_Title("Die Sides");
					Panel sidesTotalPanel = val4;
					CounterBox val5 = new CounterBox();
					((Control)val5).set_Parent((Container)(object)sidesTotalPanel);
					((Control)val5).set_Size(new Point(100, 100));
					val5.set_ValueWidth(60);
					((Control)val5).set_Location(new Point(((Container)sidesTotalPanel).get_ContentRegion().Width / 2 - 50, ((Control)sidesTotalPanel).get_Height() / 2 - 50));
					val5.set_MaxValue(100);
					val5.set_MinValue(2);
					val5.set_Value(DieSides.get_Value());
					val5.set_Numerator(1);
					val5.set_Suffix(" sides");
					CounterBox counter = val5;
					StandardButton val6 = new StandardButton();
					((Control)val6).set_Parent((Container)(object)sidesTotalPanel);
					((Control)val6).set_Size(new Point(50, 30));
					((Control)val6).set_Location(new Point(((Container)sidesTotalPanel).get_ContentRegion().Width / 2 - 25, ((Container)sidesTotalPanel).get_ContentRegion().Height - 35));
					val6.set_Text("Apply");
					StandardButton val7 = val6;
					((Control)val7).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
					{
						DieSides.set_Value(counter.get_Value());
						dieSettingsOpen = false;
						ApplyDieValue(reset: true);
						((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(sidesTotalPanel, (object)new
						{
							Opacity = 0f
						}, 0.2f, 0f, true).OnComplete((Action)delegate
						{
							((Control)sidesTotalPanel).Dispose();
						});
					});
					((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(sidesTotalPanel, (object)new
					{
						Opacity = 1f
					}, 0.2f, 0f, true);
				}
			});
			((Control)Die).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(Die, (object)new
				{
					Opacity = 1f
				}, 0.45f, 0f, true);
			});
			((Control)Die).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(Die, (object)new
				{
					Opacity = 0.4f
				}, 0.45f, 0f, true);
			});
			((Control)Die).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				if (!(rolling || dieSettingsOpen))
				{
					rolling = true;
					Stopwatch duration = new Stopwatch();
					BackgroundWorker worker = new BackgroundWorker();
					Timer interval = new Timer(70.0);
					interval.Elapsed += delegate
					{
						if (!worker.IsBusy)
						{
							worker.RunWorkerAsync();
						}
					};
					worker.DoWork += delegate
					{
						int num = ApplyDieValue(reset: false);
						if (duration.Elapsed > TimeSpan.FromMilliseconds(1200.0))
						{
							interval?.Stop();
							interval?.Dispose();
							duration?.Stop();
							duration = null;
							if (ToggleSendToChatSetting.get_Value() && !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
							{
								GameService.GameIntegration.get_Chat().Send($"/me rolls {num} on a {DieSides.get_Value()} sided die.");
							}
							ScreenNotification.ShowNotification(string.Format("{0} rolls {1} on a {2} sided die.", GameService.Gw2Mumble.get_IsAvailable() ? GameService.Gw2Mumble.get_PlayerCharacter().get_Name() : "You", num, DieSides.get_Value()), (NotificationType)0, (Texture2D)null, 4);
							rolling = false;
							worker.Dispose();
						}
					};
					interval.Start();
					duration.Start();
				}
			});
			((TweenerImpl)GameService.Animation.get_Tweener()).Tween<Panel>(Die, (object)new
			{
				Opacity = 0.4f
			}, 0.35f, 0f, true);
			int ApplyDieValue(bool reset)
			{
				int value = (reset ? DieSides.get_Value() : RandomUtil.GetRandom(1, DieSides.get_Value()));
				if (value < 7)
				{
					dieLabel.set_Text("");
					dieImage.set_Texture(AsyncTexture2D.op_Implicit(_dieTextures[value]));
				}
				else
				{
					dieImage.set_Texture(AsyncTexture2D.op_Implicit(_dieTextures[0]));
					dieLabel.set_Text($"{value}");
				}
				return value;
			}
		}
	}
}
