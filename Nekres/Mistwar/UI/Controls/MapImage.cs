using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Input;
using Glide;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.UI.Controls
{
	internal class MapImage : Container
	{
		private IEnumerable<WvwObjectiveEntity> _wvwObjectives;

		private ContinentFloorRegionMap _map;

		protected AsyncTexture2D _texture;

		private SpriteEffects _spriteEffects;

		private Rectangle? _sourceRectangle;

		private Color _tint = Color.get_White();

		private Effect _grayscaleEffect;

		private SpriteBatchParameters _grayscaleSpriteBatchParams;

		private Texture2D _playerArrow;

		private Dictionary<int, Rectangle> _wayPointBounds;

		private Texture2D _warnTriangle;

		public IEnumerable<WvwObjectiveEntity> WvwObjectives
		{
			get
			{
				return _wvwObjectives;
			}
			set
			{
				((Control)this).SetProperty<IEnumerable<WvwObjectiveEntity>>(ref _wvwObjectives, value, false, "WvwObjectives");
			}
		}

		public ContinentFloorRegionMap Map
		{
			get
			{
				return _map;
			}
			set
			{
				if (((Control)this).SetProperty<ContinentFloorRegionMap>(ref _map, value, false, "Map"))
				{
					_wayPointBounds?.Clear();
				}
			}
		}

		public AsyncTexture2D Texture
		{
			get
			{
				return _texture;
			}
			private init
			{
				((Control)this).SetProperty<AsyncTexture2D>(ref _texture, value, false, "Texture");
			}
		}

		public SpriteEffects SpriteEffects
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _spriteEffects;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<SpriteEffects>(ref _spriteEffects, value, false, "SpriteEffects");
			}
		}

		public Rectangle SourceRectangle
		{
			get
			{
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				return (Rectangle)(((_003F?)_sourceRectangle) ?? _texture.get_Texture().get_Bounds());
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Rectangle?>(ref _sourceRectangle, (Rectangle?)value, false, "SourceRectangle");
			}
		}

		public Color Tint
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _tint;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _tint, value, false, "Tint");
			}
		}

		public float TextureOpacity { get; private set; }

		public float ScaleRatio { get; private set; } = MathHelper.Clamp(MistwarModule.ModuleInstance.ScaleRatioSetting.get_Value() / 100f, 0f, 1f);


		public MapImage()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Expected O, but got Unknown
			_wayPointBounds = new Dictionary<int, Rectangle>();
			_playerArrow = MistwarModule.ModuleInstance.ContentsManager.GetTexture("156081.png");
			_warnTriangle = GameService.Content.GetTexture("common/1444522");
			((Control)this)._spriteBatchParameters = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			_grayscaleEffect = MistwarModule.ModuleInstance.ContentsManager.GetEffect<Effect>("effects\\grayscale.mgfx");
			SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val.set_Effect(_grayscaleEffect);
			_grayscaleSpriteBatchParams = val;
			Texture = new AsyncTexture2D();
			Texture.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
			MistwarModule.ModuleInstance.ScaleRatioSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnScaleRatioChanged);
		}

		public void Toggle(bool forceHide = false, bool silent = false, float tDuration = 0.1f)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			silent = silent || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch();
			if (forceHide || !GameUtil.IsAvailable() || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() || ((Control)this)._visible)
			{
				((Control)this)._visible = false;
				if (silent)
				{
					((Control)this).set_Visible(false);
					return;
				}
				GameService.Content.PlaySoundEffectByName("window-close");
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MapImage>(this, (object)new
				{
					Opacity = 0f
				}, tDuration, 0f, true).OnComplete((Action)((Control)this).Hide);
				return;
			}
			((Control)this)._visible = true;
			((Control)this).set_Visible(true);
			if (!silent)
			{
				GameService.Content.PlaySoundEffectByName("page-open-" + RandomUtil.GetRandom(1, 3));
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MapImage>(this, (object)new
				{
					Opacity = 1f
				}, 0.35f, 0f, true);
			}
		}

		internal void SetOpacity(float opacity)
		{
			TextureOpacity = opacity;
			_grayscaleEffect.get_Parameters().get_Item("Opacity").SetValue(opacity);
		}

		public void SetColorIntensity(float colorIntensity)
		{
			_grayscaleEffect.get_Parameters().get_Item("Intensity").SetValue(MathHelper.Clamp(colorIntensity, 0f, 1f));
		}

		private void OnScaleRatioChanged(object o, ValueChangedEventArgs<float> e)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			ScaleRatio = MathHelper.Clamp(e.get_NewValue() / 100f, 0f, 1f);
			if (_texture.get_HasTexture())
			{
				Rectangle bounds = _texture.get_Texture().get_Bounds();
				((Control)this).set_Size(PointExtensions.ResizeKeepAspect(((Rectangle)(ref bounds)).get_Size(), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width()), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height()), false));
				((Control)this).set_Location(new Point(((Control)((Control)this).get_Parent()).get_Size().X / 2 - ((Control)this).get_Size().X / 2, ((Control)((Control)this).get_Parent()).get_Size().Y / 2 - ((Control)this).get_Size().Y / 2));
			}
		}

		protected override async void OnClick(MouseEventArgs e)
		{
			foreach (KeyValuePair<int, Rectangle> bound in _wayPointBounds.ToList())
			{
				Rectangle value = bound.Value;
				if (!((Rectangle)(ref value)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					continue;
				}
				ContinentFloorRegionMapPoi wp = Map.get_PointsOfInterest().Values.FirstOrDefault((ContinentFloorRegionMapPoi x) => x.get_Id() == bound.Key);
				if (wp != null)
				{
					GameService.Content.PlaySoundEffectByName("button-click");
					if (PInvoke.IsLControlPressed())
					{
						ChatUtil.Send(wp.get_ChatLink(), MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value());
					}
					else if (PInvoke.IsLShiftPressed())
					{
						ChatUtil.Insert(wp.get_ChatLink(), MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value());
					}
					else if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(wp.get_ChatLink()))
					{
						ScreenNotification.ShowNotification("Waypoint copied to clipboard!", (NotificationType)0, (Texture2D)null, 4);
					}
				}
				break;
			}
			_003C_003En__0(e);
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			List<KeyValuePair<int, Rectangle>> wps = _wayPointBounds.ToList();
			foreach (KeyValuePair<int, Rectangle> bound in wps)
			{
				Rectangle value = bound.Value;
				if (!((Rectangle)(ref value)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					continue;
				}
				ContinentFloorRegionMapPoi wp = Map.get_PointsOfInterest().Values.FirstOrDefault((ContinentFloorRegionMapPoi x) => x.get_Id() == bound.Key);
				if (wp == null || wp.get_Name() == null)
				{
					break;
				}
				string wpName = wp.get_Name();
				if (wp.get_Name().StartsWith(" "))
				{
					WvwObjectiveEntity obj = WvwObjectives.FirstOrDefault((WvwObjectiveEntity x) => x.WayPoints.Any((ContinentFloorRegionMapPoi y) => y.get_Id() == wp.get_Id()));
					if (obj == null)
					{
						break;
					}
					wpName = MistwarModule.ModuleInstance.WvwService.GetWorldName(obj.Owner) + wpName;
				}
				((Control)this).set_BasicTooltipText(wpName);
			}
			if (wps.All(delegate(KeyValuePair<int, Rectangle> x)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Rectangle value2 = x.Value;
				return !((Rectangle)(ref value2)).Contains(((Control)this).get_RelativeMousePosition());
			}))
			{
				((Control)this).set_BasicTooltipText(string.Empty);
			}
			((Control)this).OnMouseMoved(e);
		}

		protected override void DisposeControl()
		{
			if (_texture != null)
			{
				_texture.remove_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)OnTextureSwapped);
				_texture.Dispose();
			}
			Effect grayscaleEffect = _grayscaleEffect;
			if (grayscaleEffect != null)
			{
				((GraphicsResource)grayscaleEffect).Dispose();
			}
			Texture2D playerArrow = _playerArrow;
			if (playerArrow != null)
			{
				((GraphicsResource)playerArrow).Dispose();
			}
			MistwarModule.ModuleInstance.ScaleRatioSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<float>>)OnScaleRatioChanged);
			((Container)this).DisposeControl();
		}

		private void OnTextureSwapped(object o, ValueChangedEventArgs<Texture2D> e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			if (e.get_NewValue() != null)
			{
				SourceRectangle = e.get_NewValue().get_Bounds();
				Rectangle bounds = e.get_NewValue().get_Bounds();
				((Control)this).set_Size(PointExtensions.ResizeKeepAspect(((Rectangle)(ref bounds)).get_Size(), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Width()), (int)(ScaleRatio * (float)((Control)GameService.Graphics.get_SpriteScreen()).get_Height()), false));
				((Control)this).set_Location(new Point(((Control)((Control)this).get_Parent()).get_Size().X / 2 - ((Control)this).get_Size().X / 2, ((Control)((Control)this).get_Parent()).get_Size().Y / 2 - ((Control)this).get_Size().Y / 2));
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Invalid comparison between Unknown and I4
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_0443: Unknown result type (might be due to invalid IL or missing references)
			//IL_0448: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0459: Unknown result type (might be due to invalid IL or missing references)
			//IL_045e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0502: Unknown result type (might be due to invalid IL or missing references)
			//IL_0507: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_051c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_0537: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0634: Unknown result type (might be due to invalid IL or missing references)
			//IL_063e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0645: Unknown result type (might be due to invalid IL or missing references)
			//IL_064f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0656: Unknown result type (might be due to invalid IL or missing references)
			//IL_065d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0668: Unknown result type (might be due to invalid IL or missing references)
			//IL_0672: Unknown result type (might be due to invalid IL or missing references)
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06db: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0700: Unknown result type (might be due to invalid IL or missing references)
			//IL_0704: Unknown result type (might be due to invalid IL or missing references)
			//IL_070b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0716: Unknown result type (might be due to invalid IL or missing references)
			//IL_072c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0734: Unknown result type (might be due to invalid IL or missing references)
			//IL_075c: Unknown result type (might be due to invalid IL or missing references)
			//IL_075e: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0818: Unknown result type (might be due to invalid IL or missing references)
			//IL_081a: Unknown result type (might be due to invalid IL or missing references)
			if (!GameUtil.IsAvailable() || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() || !((Control)this).get_Visible() || !_texture.get_HasTexture() || WvwObjectives == null)
			{
				return;
			}
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _grayscaleSpriteBatchParams);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texture), bounds, (Rectangle?)SourceRectangle, _tint, 0f, Vector2.get_Zero(), _spriteEffects);
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this)._spriteBatchParameters);
			float widthRatio = (float)bounds.Width / (float)SourceRectangle.Width;
			float heightRatio = (float)bounds.Height / (float)SourceRectangle.Height;
			if (MistwarModule.ModuleInstance.DrawSectorsSetting.get_Value())
			{
				foreach (WvwObjectiveEntity item in WvwObjectives.OrderBy((WvwObjectiveEntity x) => x.Owner == MistwarModule.ModuleInstance.WvwService.CurrentTeam))
				{
					Color teamColor = item.TeamColor.GetColorBlindType(MistwarModule.ModuleInstance.ColorTypeSetting.get_Value(), (int)(TextureOpacity * 255f));
					Vector2[] sectorBounds = item.Bounds.Select((Func<Point, Vector2>)delegate(Point p)
					{
						//IL_0020: Unknown result type (might be due to invalid IL or missing references)
						//IL_002b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0030: Unknown result type (might be due to invalid IL or missing references)
						//IL_0035: Unknown result type (might be due to invalid IL or missing references)
						//IL_0036: Unknown result type (might be due to invalid IL or missing references)
						//IL_003d: Unknown result type (might be due to invalid IL or missing references)
						//IL_0044: Unknown result type (might be due to invalid IL or missing references)
						Point val4 = PointExtensions.ToBounds(new Point((int)(widthRatio * (float)p.X), (int)(heightRatio * (float)p.Y)), ((Control)this).get_AbsoluteBounds());
						return new Vector2((float)val4.X, (float)val4.Y);
					}).ToArray();
					ShapeExtensions.DrawPolygon(spriteBatch, new Vector2(0f, 0f), (IReadOnlyList<Vector2>)sectorBounds, teamColor, 4f, 0f);
				}
			}
			Rectangle dest = default(Rectangle);
			Coordinates2 val;
			Rectangle wpDest = default(Rectangle);
			foreach (WvwObjectiveEntity objectiveEntity in WvwObjectives)
			{
				if (objectiveEntity.Icon != null)
				{
					if (!MistwarModule.ModuleInstance.DrawRuinMapSetting.get_Value() && (int)objectiveEntity.Type == 6)
					{
						continue;
					}
					int width = (int)(ScaleRatio * (float)objectiveEntity.Icon.get_Width());
					int height = (int)(ScaleRatio * (float)objectiveEntity.Icon.get_Height());
					((Rectangle)(ref dest))._002Ector((int)(widthRatio * (float)objectiveEntity.Center.X), (int)(heightRatio * (float)objectiveEntity.Center.Y), width, height);
					spriteBatch.DrawWvwObjectiveOnCtrl((Control)(object)this, objectiveEntity, dest, 1f, 0.75f, MistwarModule.ModuleInstance.DrawObjectiveNamesSetting.get_Value());
				}
				foreach (ContinentFloorRegionMapPoi wp in objectiveEntity.WayPoints)
				{
					if (GameUtil.IsEmergencyWayPoint(wp))
					{
						if (!MistwarModule.ModuleInstance.DrawEmergencyWayPointsSetting.get_Value() || objectiveEntity.Owner != MistwarModule.ModuleInstance.WvwService.CurrentTeam)
						{
							continue;
						}
						if (!objectiveEntity.HasEmergencyWaypoint())
						{
							_wayPointBounds.Remove(wp.get_Id());
							continue;
						}
					}
					else if (!objectiveEntity.HasRegularWaypoint())
					{
						_wayPointBounds.Remove(wp.get_Id());
						continue;
					}
					double num = widthRatio;
					val = wp.get_Coord();
					int num2 = (int)(num * ((Coordinates2)(ref val)).get_X()) - (int)(widthRatio * (ScaleRatio * 64f) / 2f);
					double num3 = heightRatio;
					val = wp.get_Coord();
					((Rectangle)(ref wpDest))._002Ector(num2, (int)(num3 * ((Coordinates2)(ref val)).get_Y()) - (int)(heightRatio * (ScaleRatio * 64f) / 2f), (int)(ScaleRatio * 64f), (int)(ScaleRatio * 64f));
					Texture2D tex = objectiveEntity.GetWayPointIcon(((Rectangle)(ref wpDest)).Contains(((Control)this).get_RelativeMousePosition()));
					if (!_wayPointBounds.ContainsKey(wp.get_Id()))
					{
						_wayPointBounds.Add(wp.get_Id(), wpDest);
					}
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tex, wpDest);
				}
			}
			if (Map != null)
			{
				Vector3 v = GameService.Gw2Mumble.get_PlayerCamera().get_Position() * 39.37008f;
				Rectangle val2 = Map.get_ContinentRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double x2 = ((Coordinates2)(ref val)).get_X();
				double num4 = v.X;
				val2 = Map.get_MapRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double num5 = num4 - ((Coordinates2)(ref val)).get_X();
				val2 = Map.get_MapRect();
				double num6 = num5 / ((Rectangle)(ref val2)).get_Width();
				val2 = Map.get_ContinentRect();
				float num7 = (float)(x2 + num6 * ((Rectangle)(ref val2)).get_Width());
				val2 = Map.get_ContinentRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double y = ((Coordinates2)(ref val)).get_Y();
				double num8 = v.Y;
				val2 = Map.get_MapRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double num9 = num8 - ((Coordinates2)(ref val)).get_Y();
				val2 = Map.get_MapRect();
				double num10 = num9 / ((Rectangle)(ref val2)).get_Height();
				val2 = Map.get_ContinentRect();
				Vector2 val3 = new Vector2(num7, (float)(y - num10 * ((Rectangle)(ref val2)).get_Height()));
				Vector2 mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter().ToXnaVector2();
				Vector2 pos = Vector2.Transform(val3 - mapCenter, Matrix.CreateRotationZ(0f));
				Coordinates2 value = new Coordinates2((double)pos.X, (double)pos.Y);
				val2 = Map.get_ContinentRect();
				Point fit = MapUtil.Refit(value, ((Rectangle)(ref val2)).get_TopLeft());
				Rectangle tDest = default(Rectangle);
				((Rectangle)(ref tDest))._002Ector((int)(widthRatio * (float)fit.X), (int)(heightRatio * (float)fit.Y), (int)(ScaleRatio * (float)_playerArrow.get_Width()), (int)(ScaleRatio * (float)_playerArrow.get_Height()));
				double rot = Math.Atan2(GameService.Gw2Mumble.get_PlayerCamera().get_Forward().X, GameService.Gw2Mumble.get_PlayerCamera().get_Forward().Y) * 3.5999999046325684 / Math.PI;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _playerArrow, new Rectangle(tDest.X + tDest.Width / 4, tDest.Y + tDest.Height / 4, tDest.Width, tDest.Height), (Rectangle?)_playerArrow.get_Bounds(), Color.get_White(), (float)rot, new Vector2((float)_playerArrow.get_Width() / 2f, (float)_playerArrow.get_Height() / 2f), (SpriteEffects)0);
			}
			if (MistwarModule.ModuleInstance.WvwService.IsLoading)
			{
				Rectangle spinnerBnds = default(Rectangle);
				((Rectangle)(ref spinnerBnds))._002Ector(bounds.Width / 2, bounds.Height - 100, 70, 70);
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, spinnerBnds);
				Size2 size = Control.get_Content().get_DefaultFont32().MeasureString(MistwarModule.ModuleInstance.WvwService.LoadingMessage);
				Rectangle dest2 = default(Rectangle);
				((Rectangle)(ref dest2))._002Ector((int)((float)(spinnerBnds.X + spinnerBnds.Width / 2) - size.Width / 2f), ((Rectangle)(ref spinnerBnds)).get_Bottom(), (int)size.Width, (int)size.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, MistwarModule.ModuleInstance.WvwService.LoadingMessage, Control.get_Content().get_DefaultFont16(), dest2, Color.get_White(), false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			TimeSpan lastChange = DateTime.UtcNow.Subtract(MistwarModule.ModuleInstance.WvwService.LastChange);
			if (lastChange.TotalSeconds > 120.0)
			{
				Rectangle warnBounds = default(Rectangle);
				((Rectangle)(ref warnBounds))._002Ector(bounds.Width - 300, bounds.Height - 32, 300, 32);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _warnTriangle, new Rectangle(((Rectangle)(ref warnBounds)).get_Left() - 32, ((Rectangle)(ref warnBounds)).get_Top(), 32, 32));
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, $"Last Change: {lastChange.Hours} hours {lastChange.Minutes} minutes ago", Control.get_Content().get_DefaultFont14(), warnBounds, Color.get_White(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}
	}
}
