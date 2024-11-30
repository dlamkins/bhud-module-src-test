using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Input;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.Core.UI.Controls
{
	internal class MapImage : Container
	{
		private IEnumerable<WvwObjectiveEntity> _wvwObjectives;

		private ContinentFloorRegionMap _map;

		private AsyncTexture2D _texture;

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

		public MapImage()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			_wayPointBounds = new Dictionary<int, Rectangle>();
			_playerArrow = MistwarModule.ModuleInstance.ContentsManager.GetTexture("156081.png");
			_warnTriangle = GameService.Content.GetTexture("common/1444522");
			((Control)this)._spriteBatchParameters = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			_grayscaleEffect = MistwarModule.ModuleInstance.ContentsManager.GetEffect<Effect>("effects\\grayscale.mgfx");
			SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val.set_Effect(_grayscaleEffect);
			_grayscaleSpriteBatchParams = val;
			Texture = new AsyncTexture2D();
			SetOpacity(1f);
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

		protected override async void OnClick(MouseEventArgs e)
		{
			_003C_003En__0(e);
			if (Map == null)
			{
				return;
			}
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
						await ChatUtil.Send(wp.get_ChatLink(), MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value());
					}
					else if (PInvoke.IsLShiftPressed())
					{
						await ChatUtil.Insert(wp.get_ChatLink(), MistwarModule.ModuleInstance.ChatMessageKeySetting.get_Value());
					}
					else if (await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(wp.get_ChatLink()))
					{
						ScreenNotification.ShowNotification("Waypoint copied to clipboard!", (NotificationType)0, (Texture2D)null, 4);
					}
				}
				break;
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseMoved(e);
			if (Map == null)
			{
				return;
			}
			List<KeyValuePair<int, Rectangle>> wps = _wayPointBounds.ToList();
			foreach (KeyValuePair<int, Rectangle> bound in wps)
			{
				Rectangle value = bound.Value;
				if (!((Rectangle)(ref value)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					continue;
				}
				ContinentFloorRegionMapPoi wp = Map.get_PointsOfInterest().Values.FirstOrDefault((ContinentFloorRegionMapPoi x) => x.get_Id() == bound.Key);
				ContinentFloorRegionMapPoi obj2 = wp;
				if (((obj2 != null) ? obj2.get_Name() : null) == null)
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
					wpName = MistwarModule.ModuleInstance.WvW.GetWorldName(obj.Owner) + wpName;
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
		}

		protected override void DisposeControl()
		{
			AsyncTexture2D texture = _texture;
			if (texture != null)
			{
				texture.Dispose();
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
			((Container)this).DisposeControl();
		}

		private static Point ComputeAspectRatioSize(Point parentSize, Point childSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			double num = (double)parentSize.X / (double)parentSize.Y;
			double childRatio = (double)childSize.X / (double)childSize.Y;
			double scaleFactor = Math.Min((num > childRatio) ? ((double)parentSize.Y / (double)childSize.Y) : ((double)parentSize.X / (double)childSize.X), 1.0);
			int num2 = (int)Math.Round((double)childSize.X * scaleFactor);
			int newHeight = (int)Math.Round((double)childSize.Y * scaleFactor);
			return new Point(num2, newHeight);
		}

		private static Point ComputeSize(Point parentSize, Point childSize)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			double scaleFactorWidth = (double)parentSize.X / (double)childSize.X;
			double scaleFactorHeight = (double)parentSize.Y / (double)childSize.Y;
			int num = (int)Math.Round((double)childSize.X * scaleFactorWidth);
			int newHeight = (int)Math.Round((double)childSize.Y * scaleFactorHeight);
			return new Point(num, newHeight);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Invalid comparison between Unknown and I4
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_0506: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_0555: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_056c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0570: Unknown result type (might be due to invalid IL or missing references)
			//IL_0575: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_0595: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05af: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05df: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0601: Unknown result type (might be due to invalid IL or missing references)
			//IL_0603: Unknown result type (might be due to invalid IL or missing references)
			//IL_060b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0613: Unknown result type (might be due to invalid IL or missing references)
			//IL_061e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0623: Unknown result type (might be due to invalid IL or missing references)
			//IL_0627: Unknown result type (might be due to invalid IL or missing references)
			//IL_063b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_064b: Unknown result type (might be due to invalid IL or missing references)
			//IL_064f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0654: Unknown result type (might be due to invalid IL or missing references)
			//IL_0659: Unknown result type (might be due to invalid IL or missing references)
			//IL_065d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0674: Unknown result type (might be due to invalid IL or missing references)
			//IL_068b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0692: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0703: Unknown result type (might be due to invalid IL or missing references)
			//IL_070d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0714: Unknown result type (might be due to invalid IL or missing references)
			//IL_071b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0726: Unknown result type (might be due to invalid IL or missing references)
			//IL_0730: Unknown result type (might be due to invalid IL or missing references)
			//IL_075c: Unknown result type (might be due to invalid IL or missing references)
			//IL_078a: Unknown result type (might be due to invalid IL or missing references)
			//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_07af: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07db: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_080b: Unknown result type (might be due to invalid IL or missing references)
			//IL_080d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0852: Unknown result type (might be due to invalid IL or missing references)
			//IL_0888: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
			if (!_texture.get_HasTexture() || WvwObjectives == null || Map == null)
			{
				return;
			}
			SetOpacity(((Control)((Control)this).get_Parent()).get_Opacity());
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _grayscaleSpriteBatchParams);
			Point size3 = ((Rectangle)(ref bounds)).get_Size();
			Rectangle bounds2 = _texture.get_Bounds();
			Point bgSize = ComputeAspectRatioSize(size3, ((Rectangle)(ref bounds2)).get_Size());
			Point bgOffset = default(Point);
			((Point)(ref bgOffset))._002Ector((bounds.Width - bgSize.X) / 2, (bounds.Height - bgSize.Y) / 2);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_texture), new Rectangle(bgOffset, bgSize), (Rectangle?)_texture.get_Bounds(), _tint, 0f, Vector2.get_Zero(), _spriteEffects);
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, ((Control)this)._spriteBatchParameters);
			float widthRatio = (float)bounds.Width / (float)_texture.get_Bounds().Width;
			float heightRatio = (float)bounds.Height / (float)_texture.get_Bounds().Height;
			float ratio = ((widthRatio > heightRatio) ? heightRatio : widthRatio);
			if (MistwarModule.ModuleInstance.DrawSectorsSetting.get_Value())
			{
				foreach (WvwObjectiveEntity item in WvwObjectives.OrderBy((WvwObjectiveEntity x) => x.Owner == MistwarModule.ModuleInstance.WvW.CurrentTeam))
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
						Point val4 = PointExtensions.ToBounds(new Point((int)(ratio * (float)p.X), (int)(ratio * (float)p.Y)), ((Control)this).get_AbsoluteBounds());
						return new Vector2((float)val4.X, (float)val4.Y);
					}).ToArray();
					ShapeExtensions.DrawPolygon(spriteBatch, new Vector2((float)bgOffset.X, (float)bgOffset.Y), (IReadOnlyList<Vector2>)sectorBounds, teamColor, 4f, 0f);
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
					int num = bgOffset.X + (int)(ratio * (float)objectiveEntity.Center.X);
					int num2 = bgOffset.Y + (int)(ratio * (float)objectiveEntity.Center.Y);
					float num3 = ratio;
					bounds2 = objectiveEntity.Icon.get_Bounds();
					int num4 = (int)(num3 * (float)((Rectangle)(ref bounds2)).get_Size().X) * 2;
					float num5 = ratio;
					bounds2 = objectiveEntity.Icon.get_Bounds();
					((Rectangle)(ref dest))._002Ector(num, num2, num4, (int)(num5 * (float)((Rectangle)(ref bounds2)).get_Size().Y) * 2);
					spriteBatch.DrawWvwObjectiveOnCtrl((Control)(object)this, objectiveEntity, dest, 1f, 0.75f, MistwarModule.ModuleInstance.DrawObjectiveNamesSetting.get_Value());
				}
				foreach (ContinentFloorRegionMapPoi wp in objectiveEntity.WayPoints)
				{
					if (GameUtil.IsEmergencyWayPoint(wp))
					{
						if (!MistwarModule.ModuleInstance.DrawEmergencyWayPointsSetting.get_Value() || objectiveEntity.Owner != MistwarModule.ModuleInstance.WvW.CurrentTeam)
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
					int x2 = bgOffset.X;
					double num6 = ratio;
					val = wp.get_Coord();
					int num7 = x2 + (int)(num6 * ((Coordinates2)(ref val)).get_X()) - (int)(ratio * 64f) / 4;
					int y = bgOffset.Y;
					double num8 = ratio;
					val = wp.get_Coord();
					((Rectangle)(ref wpDest))._002Ector(num7, y + (int)(num8 * ((Coordinates2)(ref val)).get_Y()) - (int)(ratio * 64f) / 4, (int)(ratio * 64f) * 2, (int)(ratio * 64f) * 2);
					Texture2D tex = MistwarModule.ModuleInstance.Resources.GetWayPointTexture(((Rectangle)(ref wpDest)).Contains(((Control)this).get_RelativeMousePosition()), objectiveEntity.Owner == MistwarModule.ModuleInstance.WvW.CurrentTeam);
					if (!_wayPointBounds.ContainsKey(wp.get_Id()))
					{
						_wayPointBounds.Add(wp.get_Id(), wpDest);
					}
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tex, wpDest);
				}
			}
			if (Map != null)
			{
				Vector3 v = GameService.Gw2Mumble.get_PlayerCharacter().get_Position() * 39.37008f;
				Rectangle val2 = Map.get_ContinentRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double x3 = ((Coordinates2)(ref val)).get_X();
				double num9 = v.X;
				val2 = Map.get_MapRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double num10 = num9 - ((Coordinates2)(ref val)).get_X();
				val2 = Map.get_MapRect();
				double num11 = num10 / ((Rectangle)(ref val2)).get_Width();
				val2 = Map.get_ContinentRect();
				float num12 = (float)(x3 + num11 * ((Rectangle)(ref val2)).get_Width());
				val2 = Map.get_ContinentRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double y2 = ((Coordinates2)(ref val)).get_Y();
				double num13 = v.Y;
				val2 = Map.get_MapRect();
				val = ((Rectangle)(ref val2)).get_TopLeft();
				double num14 = num13 - ((Coordinates2)(ref val)).get_Y();
				val2 = Map.get_MapRect();
				double num15 = num14 / ((Rectangle)(ref val2)).get_Height();
				val2 = Map.get_ContinentRect();
				Vector2 val3 = new Vector2(num12, (float)(y2 - num15 * ((Rectangle)(ref val2)).get_Height()));
				Vector2 mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter().ToXnaVector2();
				Vector2 pos = Vector2.Transform(val3 - mapCenter, Matrix.CreateRotationZ(0f));
				Coordinates2 value = new Coordinates2((double)pos.X, (double)pos.Y);
				val2 = Map.get_ContinentRect();
				Point fit = MapUtil.Refit(value, ((Rectangle)(ref val2)).get_TopLeft());
				Point size4 = ((Rectangle)(ref bounds)).get_Size();
				bounds2 = _playerArrow.get_Bounds();
				Point size2 = ComputeAspectRatioSize(size4, ((Rectangle)(ref bounds2)).get_Size());
				Rectangle tDest = default(Rectangle);
				((Rectangle)(ref tDest))._002Ector(bgOffset.X + (int)(ratio * (float)fit.X), bgOffset.Y + (int)(ratio * (float)fit.Y), size2.X, size2.Y);
				double rot = Math.Atan2(GameService.Gw2Mumble.get_PlayerCharacter().get_Forward().X, GameService.Gw2Mumble.get_PlayerCharacter().get_Forward().Y) * 3.5999999046325684 / Math.PI;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _playerArrow, new Rectangle(tDest.X + tDest.Width / 4, tDest.Y + tDest.Height / 4, tDest.Width, tDest.Height), (Rectangle?)_playerArrow.get_Bounds(), Color.get_White(), (float)rot, new Vector2((float)_playerArrow.get_Width() / 2f, (float)_playerArrow.get_Height() / 2f), (SpriteEffects)0);
			}
			if (MistwarModule.ModuleInstance.WvW.IsRefreshing)
			{
				Rectangle spinnerBnds = default(Rectangle);
				((Rectangle)(ref spinnerBnds))._002Ector(0, 0, 70, 70);
				LoadingSpinnerUtil.DrawLoadingSpinner((Control)(object)this, spriteBatch, spinnerBnds);
				Size2 size = Control.get_Content().get_DefaultFont32().MeasureString(MistwarModule.ModuleInstance.WvW.RefreshMessage);
				Rectangle dest2 = default(Rectangle);
				((Rectangle)(ref dest2))._002Ector((int)((float)(spinnerBnds.X + spinnerBnds.Width / 2) - size.Width / 2f), ((Rectangle)(ref spinnerBnds)).get_Bottom(), (int)size.Width, (int)size.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, MistwarModule.ModuleInstance.WvW.RefreshMessage, Control.get_Content().get_DefaultFont16(), dest2, Color.get_White(), false, true, 1, (HorizontalAlignment)1, (VerticalAlignment)1);
			}
			TimeSpan lastChange = DateTime.UtcNow.Subtract(MistwarModule.ModuleInstance.WvW.LastChange);
			if (lastChange.TotalSeconds > 120.0)
			{
				Rectangle warnBounds = default(Rectangle);
				((Rectangle)(ref warnBounds))._002Ector(bounds.Width - 300, 0, 300, 32);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _warnTriangle, new Rectangle(((Rectangle)(ref warnBounds)).get_Left() - 32, ((Rectangle)(ref warnBounds)).get_Top(), 32, 32));
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, $"Last Change: {lastChange.Hours} hours {lastChange.Minutes} minutes ago.", Control.get_Content().get_DefaultFont14(), warnBounds, Color.get_White(), false, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}
	}
}
