using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nekres.Music_Mixer.Core.Services;
using Nekres.Music_Mixer.Core.Services.Data;
using Nekres.Music_Mixer.Core.Services.YtDlp;

namespace Nekres.Music_Mixer.Core.UI.Library
{
	public class BgmLibraryView : View
	{
		public class BgmEntry : View
		{
			private AudioSource _audioSource;

			public event EventHandler<EventArgs> OnDeleted;

			public BgmEntry(AudioSource audioSource)
				: this()
			{
				_audioSource = audioSource;
			}

			protected override void Build(Container buildPanel)
			{
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_013e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0148: Unknown result type (might be due to invalid IL or missing references)
				//IL_0153: Unknown result type (might be due to invalid IL or missing references)
				//IL_0158: Unknown result type (might be due to invalid IL or missing references)
				//IL_0164: Unknown result type (might be due to invalid IL or missing references)
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0174: Unknown result type (might be due to invalid IL or missing references)
				//IL_017b: Unknown result type (might be due to invalid IL or missing references)
				//IL_018f: Unknown result type (might be due to invalid IL or missing references)
				//IL_019b: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c5: Expected O, but got Unknown
				//IL_020a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0215: Unknown result type (might be due to invalid IL or missing references)
				//IL_023c: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0302: Unknown result type (might be due to invalid IL or missing references)
				//IL_030d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0315: Unknown result type (might be due to invalid IL or missing references)
				//IL_031c: Unknown result type (might be due to invalid IL or missing references)
				//IL_032d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0339: Unknown result type (might be due to invalid IL or missing references)
				//IL_0340: Unknown result type (might be due to invalid IL or missing references)
				//IL_0346: Unknown result type (might be due to invalid IL or missing references)
				//IL_0356: Unknown result type (might be due to invalid IL or missing references)
				//IL_0360: Unknown result type (might be due to invalid IL or missing references)
				//IL_0366: Unknown result type (might be due to invalid IL or missing references)
				//IL_0376: Unknown result type (might be due to invalid IL or missing references)
				//IL_0382: Expected O, but got Unknown
				//IL_03da: Unknown result type (might be due to invalid IL or missing references)
				//IL_03df: Unknown result type (might be due to invalid IL or missing references)
				//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_040f: Unknown result type (might be due to invalid IL or missing references)
				//IL_042c: Expected O, but got Unknown
				SlidePanel slidePanel2 = new SlidePanel();
				((Control)slidePanel2).set_Parent(buildPanel);
				((Control)slidePanel2).set_Width(buildPanel.get_ContentRegion().Width);
				((Control)slidePanel2).set_Height(buildPanel.get_ContentRegion().Height);
				((Control)slidePanel2).set_Left(buildPanel.get_ContentRegion().Width);
				SlidePanel slidePanel = slidePanel2;
				RoundedImage roundedImage = new RoundedImage(_audioSource.Thumbnail);
				((Control)roundedImage).set_Parent((Container)(object)slidePanel);
				((Control)roundedImage).set_Width(192);
				((Control)roundedImage).set_Height(108);
				RoundedImage thumbnail = roundedImage;
				((Control)thumbnail).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					if (string.IsNullOrWhiteSpace(_audioSource.PageUrl))
					{
						ScreenNotification.ShowNotification("Page Not Found.", (NotificationType)2, (Texture2D)null, 4);
						GameService.Content.PlaySoundEffectByName("error");
					}
					else
					{
						Process.Start(_audioSource.PageUrl);
						GameService.Content.PlaySoundEffectByName("open-skill-slot");
					}
				});
				string durationStr = _audioSource.Duration.ToShortForm();
				Point size = LabelUtil.GetLabelSize((FontSize)14, durationStr);
				FormattedLabel obj = new FormattedLabelBuilder().SetWidth(size.X + 4).SetHeight(size.Y + 4).SetHorizontalAlignment((HorizontalAlignment)1)
					.CreatePart(durationStr, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
					{
						o.SetFontSize((FontSize)14);
					})
					.Build();
				((Control)obj).set_Parent((Container)(object)slidePanel);
				((Control)obj).set_Bottom(((Control)thumbnail).get_Bottom());
				((Control)obj).set_Right(((Control)thumbnail).get_Right());
				((Control)obj).set_BackgroundColor(Color.get_Black() * 0.25f);
				Image val = new Image();
				((Control)val).set_Parent((Container)(object)slidePanel);
				((Control)val).set_Width(32);
				((Control)val).set_Height(32);
				((Control)val).set_Right(((Container)slidePanel).get_ContentRegion().Width - 4 - 13);
				((Control)val).set_Top(((Control)thumbnail).get_Top());
				val.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156012));
				((Control)val).set_BasicTooltipText("Remove from Playlist");
				Image delBttn = val;
				((Control)delBttn).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
				{
					delBttn.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156011));
				});
				((Control)delBttn).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
				{
					delBttn.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156012));
				});
				((Control)delBttn).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					delBttn.set_Texture(GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156012));
					slidePanel.SlideOut((Action)((Control)buildPanel).Dispose);
					GameService.Content.PlaySoundEffectByName("window-close");
					this.OnDeleted?.Invoke(this, EventArgs.Empty);
				});
				FormattedLabel obj2 = new FormattedLabelBuilder().SetWidth(((Container)slidePanel).get_ContentRegion().Width - ((Control)thumbnail).get_Right() - ((Control)delBttn).get_Width() - 4 - 13 - ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X * 3).SetHeight(((Control)thumbnail).get_Height()).SetVerticalAlignment((VerticalAlignment)0)
					.CreatePart(_audioSource.Title, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
					{
						o.SetFontSize((FontSize)20);
						o.MakeBold();
						if (string.IsNullOrWhiteSpace(_audioSource.PageUrl))
						{
							o.SetLink((Action)delegate
							{
								ScreenNotification.ShowNotification("Page Not Found.", (NotificationType)2, (Texture2D)null, 4);
							});
							GameService.Content.PlaySoundEffectByName("error");
						}
						else
						{
							o.SetLink((Action)delegate
							{
								Process.Start(_audioSource.PageUrl);
								GameService.Content.PlaySoundEffectByName("open-skill-slot");
							});
						}
					})
					.Wrap()
					.CreatePart("\n" + _audioSource.Uploader, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
					{
						o.SetFontSize((FontSize)16);
					})
					.Build();
				((Control)obj2).set_Parent((Container)(object)slidePanel);
				((Control)obj2).set_Top(((Control)thumbnail).get_Top());
				((Control)obj2).set_Left(((Control)thumbnail).get_Right() + ((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X);
				FlowPanel val2 = new FlowPanel();
				((Control)val2).set_Parent((Container)(object)slidePanel);
				((Control)val2).set_Width(140);
				((Control)val2).set_Height(32);
				((Control)val2).set_Right(((Container)slidePanel).get_ContentRegion().Width - 4);
				((Control)val2).set_Bottom(((Control)thumbnail).get_Bottom());
				val2.set_FlowDirection((ControlFlowDirection)2);
				val2.set_ControlPadding(new Vector2((float)((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0f));
				val2.set_OuterControlPadding(new Vector2((float)((DesignStandard)(ref Control.ControlStandard)).get_ControlOffset().X, 0f));
				FlowPanel cyclesPanel = val2;
				foreach (AudioSource.DayCycle cycle in Enum.GetValues(typeof(AudioSource.DayCycle)).Cast<AudioSource.DayCycle>().Skip(1)
					.Except(new AudioSource.DayCycle[2]
					{
						AudioSource.DayCycle.Dawn,
						AudioSource.DayCycle.Dusk
					}))
				{
					Checkbox val3 = new Checkbox();
					((Control)val3).set_Parent((Container)(object)cyclesPanel);
					((Control)val3).set_Width(100);
					((Control)val3).set_Height(32);
					val3.set_Text(cycle.ToString());
					val3.set_Checked(_audioSource.HasDayCycle(cycle));
					Checkbox cb = val3;
					cb.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object _, CheckChangedEvent e)
					{
						if ((_audioSource.DayCycles & ~cycle) == 0)
						{
							cb.GetPrivateField("_checked").SetValue(cb, !e.get_Checked());
							GameService.Content.PlaySoundEffectByName("error");
						}
						else
						{
							AudioSource.DayCycle dayCycles = _audioSource.DayCycles;
							if (e.get_Checked())
							{
								_audioSource.DayCycles |= cycle;
							}
							else
							{
								_audioSource.DayCycles &= ~cycle;
							}
							if (!MusicMixer.Instance.Data.Upsert(_audioSource))
							{
								_audioSource.DayCycles = dayCycles;
								cb.GetPrivateField("_checked").SetValue(cb, !e.get_Checked());
								ScreenNotification.ShowNotification("Something went wrong. Please try again.", (NotificationType)2, (Texture2D)null, 4);
								GameService.Content.PlaySoundEffectByName("error");
							}
							else
							{
								GameService.Content.PlaySoundEffectByName("color-change");
							}
						}
					});
				}
				slidePanel.SlideIn();
				((View<IPresenter>)this).Build(buildPanel);
			}
		}

		public class SlidePanel : Panel
		{
			private Tween _tween;

			public void SlideIn()
			{
				Tween tween = _tween;
				if (tween != null)
				{
					tween.Cancel();
				}
				_tween = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<SlidePanel>(this, (object)new
				{
					Left = 0,
					Opacity = 1f
				}, 0.3f, 0f, true).Ease((Func<float, float>)Ease.CubeOut);
			}

			public void SlideOut(Action onComplete = null)
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				Tween tween = _tween;
				if (tween != null)
				{
					tween.Cancel();
				}
				_tween = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<SlidePanel>(this, (object)new
				{
					Left = ((Control)this).get_Parent().get_ContentRegion().Width,
					Opacity = 0f
				}, 0.3f, 0f, true).Ease((Func<float, float>)Ease.CubeIn).OnComplete(onComplete);
			}

			protected override void DisposeControl()
			{
				SlideOut(base.DisposeControl);
			}

			public SlidePanel()
				: this()
			{
			}
		}

		public class RoundedImage : Control
		{
			private Effect _curvedBorder;

			private AsyncTexture2D _texture;

			private SpriteBatchParameters _defaultParams;

			private SpriteBatchParameters _curvedBorderParams;

			private float _radius = 0.215f;

			private Tween _tween;

			public RoundedImage(AsyncTexture2D texture)
				: this()
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Expected O, but got Unknown
				_defaultParams = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
				_curvedBorder = MusicMixer.Instance.ContentsManager.GetEffect<Effect>("effects\\curvedborder.mgfx");
				SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
				val.set_Effect(_curvedBorder);
				_curvedBorderParams = val;
				_texture = texture;
			}

			protected override void DisposeControl()
			{
				((GraphicsResource)_curvedBorder).Dispose();
				((Control)this).DisposeControl();
			}

			protected override void OnMouseEntered(MouseEventArgs e)
			{
				Tween tween = _tween;
				if (tween != null)
				{
					tween.Cancel();
				}
				_tween = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<RoundedImage>(this, (object)new
				{
					_radius = 0.315f
				}, 0.1f, 0f, true);
				((Control)this).OnMouseEntered(e);
			}

			protected override void OnMouseLeft(MouseEventArgs e)
			{
				Tween tween = _tween;
				if (tween != null)
				{
					tween.Cancel();
				}
				_tween = ((TweenerImpl)Control.get_Animation().get_Tweener()).Tween<RoundedImage>(this, (object)new
				{
					_radius = 0.215f
				}, 0.1f, 0f, true);
				((Control)this).OnMouseLeft(e);
			}

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				if (!_texture.get_HasTexture() || !_texture.get_HasSwapped())
				{
					LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, new Rectangle((bounds.Width - 32) / 2, (bounds.Height - 32) / 2, 32, 32));
					return;
				}
				_curvedBorder.get_Parameters().get_Item("Radius").SetValue(_radius);
				_curvedBorder.get_Parameters().get_Item("Opacity").SetValue(((Control)this).get_Opacity());
				spriteBatch.End();
				SpriteBatchExtensions.Begin(spriteBatch, _curvedBorderParams);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texture), new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()));
				spriteBatch.End();
				SpriteBatchExtensions.Begin(spriteBatch, _defaultParams);
			}
		}

		private Playlist _playlist;

		private KeyBinding _pasteShortcut;

		private FlowPanel _tracksPanel;

		private string _name;

		private bool _checkingLink;

		public BgmLibraryView(Playlist playlist, string playlistName)
			: this()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			_name = playlistName;
			_playlist = playlist;
			KeyBinding val = new KeyBinding((ModifierKeys)1, (Keys)86);
			val.set_Enabled(true);
			_pasteShortcut = val;
			_pasteShortcut.add_Activated((EventHandler<EventArgs>)OnPastePressed);
		}

		protected override void Unload()
		{
			_pasteShortcut.set_Enabled(false);
			_pasteShortcut.remove_Activated((EventHandler<EventArgs>)OnPastePressed);
			((View<IPresenter>)this).Unload();
		}

		private async void OnPastePressed(object sender, EventArgs e)
		{
			Container viewTarget = ((View<IPresenter>)this).get_ViewTarget();
			if (((viewTarget != null) ? ((Control)viewTarget).get_Parent() : null) != null && _tracksPanel != null && ((Control)((Control)((View<IPresenter>)this).get_ViewTarget()).get_Parent()).get_Visible())
			{
				Rectangle contentRegion = ((Container)_tracksPanel).get_ContentRegion();
				if (((Rectangle)(ref contentRegion)).Contains(((Control)_tracksPanel).get_RelativeMousePosition()))
				{
					await FindAdd();
				}
			}
		}

		private async Task FindAdd()
		{
			if (_checkingLink)
			{
				ScreenNotification.ShowNotification("Checking link… (Please wait.)", (NotificationType)1, (Texture2D)null, 4);
				GameService.Content.PlaySoundEffectByName("button-click");
				return;
			}
			_checkingLink = true;
			try
			{
				string url = await ClipboardUtil.get_WindowsClipboardService().GetTextAsync();
				if (!url.IsWebLink())
				{
					ScreenNotification.ShowNotification("Your clipboard does not contain a valid link.", (NotificationType)2, (Texture2D)null, 4);
					GameService.Content.PlaySoundEffectByName("error");
					return;
				}
				ScreenNotification.ShowNotification("Link pasted. Checking… (Please wait.)", (NotificationType)0, (Texture2D)null, 4);
				MetaData data = await MusicMixer.Instance.YtDlp.GetMetaData(url);
				if (data.IsError)
				{
					ScreenNotification.ShowNotification("Unsupported website.", (NotificationType)2, (Texture2D)null, 4);
					GameService.Content.PlaySoundEffectByName("error");
					return;
				}
				if (_playlist.Tracks.Any((AudioSource track) => string.Equals(track.ExternalId, data.Id)))
				{
					ScreenNotification.ShowNotification("This track is already in the playlist.", (NotificationType)2, (Texture2D)null, 4);
					GameService.Content.PlaySoundEffectByName("error");
					return;
				}
				if (MusicMixer.Instance.Data.GetTrackByMediaId(data.Id, out var source))
				{
					goto IL_02bf;
				}
				source = new AudioSource
				{
					ExternalId = data.Id,
					Uploader = data.Uploader,
					Title = data.Title,
					PageUrl = data.Url,
					Duration = data.Duration,
					Volume = 1f,
					DayCycles = (AudioSource.DayCycle.Day | AudioSource.DayCycle.Night)
				};
				if (MusicMixer.Instance.Data.Upsert(source))
				{
					goto IL_02bf;
				}
				ScreenNotification.ShowNotification("Something went wrong. Please try again.", (NotificationType)2, (Texture2D)null, 4);
				GameService.Content.PlaySoundEffectByName("error");
				goto end_IL_004d;
				IL_02bf:
				_playlist.Tracks.Add(source);
				MusicMixer.Instance.Data.Upsert(_playlist);
				AddBgmEntry(source, _tracksPanel);
				GameService.Content.PlaySoundEffectByName("select-skill");
				end_IL_004d:;
			}
			catch (Exception e)
			{
				ScreenNotification.ShowNotification("Something went wrong. Please try again.", (NotificationType)2, (Texture2D)null, 4);
				GameService.Content.PlaySoundEffectByName("error");
				MusicMixer.Logger.Info(e, e.Message);
			}
			finally
			{
				_checkingLink = false;
			}
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			((Control)val).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val).set_Height(40);
			val.set_Text(_name);
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)0));
			val.set_TextColor(Color.get_White());
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			val.set_VerticalAlignment((VerticalAlignment)1);
			Label title = val;
			Checkbox val2 = new Checkbox();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Width(100);
			((Control)val2).set_Top(20);
			((Control)val2).set_Left(4);
			val2.set_Text("Enabled");
			((Control)val2).set_BasicTooltipText("Enable or disable this playlist.");
			val2.set_Checked(_playlist.Enabled);
			val2.add_CheckedChanged((EventHandler<CheckChangedEvent>)async delegate(object _, CheckChangedEvent e)
			{
				_playlist.Enabled = e.get_Checked();
				if (!MusicMixer.Instance.Data.Upsert(_playlist))
				{
					ScreenNotification.ShowNotification("Something went wrong. Please try again.", (NotificationType)2, (Texture2D)null, 4);
					GameService.Content.PlaySoundEffectByName("error");
				}
				else
				{
					if (_playlist.ExternalId.Equals("Defeated"))
					{
						if (e.get_Checked())
						{
							await MusicMixer.Instance.Gw2State.SetupLockFiles(Gw2StateService.State.Defeated);
						}
						else
						{
							MusicMixer.Instance.Gw2State.RevertLockFiles(Gw2StateService.State.Defeated);
						}
					}
					if (e.get_Checked())
					{
						GameService.Content.PlaySoundEffectByName("color-change");
					}
				}
			});
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent(buildPanel);
			((Control)val3).set_Width(buildPanel.get_ContentRegion().Width);
			((Control)val3).set_Top(((Control)title).get_Bottom());
			((Control)val3).set_Height(buildPanel.get_ContentRegion().Height - ((Control)title).get_Bottom() - 32 - 7);
			((Panel)val3).set_ShowBorder(true);
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_ControlPadding(new Vector2(0f, 7f));
			val3.set_OuterControlPadding(new Vector2(4f, 7f));
			((Panel)val3).set_CanScroll(true);
			_tracksPanel = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Width(((Control)_tracksPanel).get_Width());
			((Control)val4).set_Height(32);
			((Control)val4).set_Top(((Control)_tracksPanel).get_Bottom() + 7);
			((Control)val4).set_Left(((Control)_tracksPanel).get_Left());
			val4.set_Text("Paste From Clipboard [" + _pasteShortcut.GetBindingDisplayText() + "]");
			((Control)val4).set_BasicTooltipText("Paste a video or audio link from your clipboard to add it to the playlist.\nRecommended platforms: SoundCloud, YouTube.");
			StandardButton addBttn = val4;
			foreach (AudioSource track in _playlist.Tracks)
			{
				AddBgmEntry(track, _tracksPanel);
			}
			((Control)addBttn).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await FindAdd();
			});
			((View<IPresenter>)this).Build(buildPanel);
		}

		private void AddBgmEntry(AudioSource source, FlowPanel parent)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(((Container)parent).get_ContentRegion().Width);
			((Control)val).set_Height(108);
			BgmEntry bgmEntry = new BgmEntry(source);
			bgmEntry.OnDeleted += delegate
			{
				_playlist.Tracks.Remove(source);
				if (!MusicMixer.Instance.Data.Upsert(_playlist))
				{
					ScreenNotification.ShowNotification("Something went wrong. Please try again.", (NotificationType)2, (Texture2D)null, 4);
					GameService.Content.PlaySoundEffectByName("error");
				}
			};
			val.Show((IView)(object)bgmEntry);
		}
	}
}
