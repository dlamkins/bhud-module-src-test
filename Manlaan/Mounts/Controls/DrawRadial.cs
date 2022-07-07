using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Intern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.Mounts.Controls
{
	internal class DrawRadial : Control
	{
		private static readonly Logger Logger = Logger.GetLogger<DrawRadial>();

		private readonly Helper _helper;

		private readonly TextureCache _textureCache;

		private List<RadialMount> RadialMounts = new List<RadialMount>();

		private int radius;

		private int mountIconSize;

		private int _maxRadialDiameter;

		private Point SpawnPoint;

		private RadialMount SelectedMount => RadialMounts.SingleOrDefault((RadialMount m) => m.Selected);

		public override int ZIndex
		{
			get
			{
				return ((Control)this).get_ZIndex();
			}
			set
			{
				((Control)this).set_ZIndex(value);
			}
		}

		public bool IsActionCamToggledOnMount { get; private set; }

		public DrawRadial(Helper helper, TextureCache textureCache)
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Visible(false);
			((Control)this).set_Padding(Thickness.Zero);
			_helper = helper;
			_textureCache = textureCache;
			((Control)this).add_Shown((EventHandler<EventArgs>)async delegate(object sender, EventArgs e)
			{
				await HandleShown(sender, e);
			});
			((Control)this).add_Hidden((EventHandler<EventArgs>)async delegate(object sender, EventArgs e)
			{
				await HandleHidden(sender, e);
			});
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)4;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			RadialMounts.Clear();
			List<Mount> mounts = Module._availableOrderedMounts;
			Mount mountToPutInCenter = _helper.GetCenterMount();
			if (mountToPutInCenter != null && mountToPutInCenter.IsAvailable)
			{
				if (Module._settingMountRadialRemoveCenterMount.get_Value())
				{
					mounts.Remove(mountToPutInCenter);
				}
				Texture2D texture2 = _textureCache.GetImgFile(mountToPutInCenter.ImageFileName);
				int loc = radius;
				RadialMounts.Add(new RadialMount
				{
					Texture = texture2,
					Mount = mountToPutInCenter,
					ImageX = loc,
					ImageY = loc,
					Default = true
				});
			}
			double startAngle = Math.PI * Math.Floor(Module._settingMountRadialStartAngle.get_Value() * 360f) / 180.0;
			double currentAngle = startAngle;
			double partAngleStep = Math.PI * 2.0 / (double)mounts.Count();
			foreach (Mount mount in mounts)
			{
				double angleMid = currentAngle + partAngleStep / 2.0;
				double angleEnd = currentAngle + partAngleStep;
				Texture2D texture = _textureCache.GetImgFile(mount.ImageFileName);
				int x = (int)Math.Round((double)radius + (double)radius * Math.Cos(angleMid));
				int y = (int)Math.Round((double)radius + (double)radius * Math.Sin(angleMid));
				RadialMounts.Add(new RadialMount
				{
					Texture = texture,
					Mount = mount,
					ImageX = x,
					ImageY = y,
					AngleBegin = currentAngle,
					AngleEnd = angleEnd
				});
				currentAngle = angleEnd;
			}
			Point diff = Control.get_Input().get_Mouse().get_Position() - SpawnPoint;
			double angle;
			for (angle = Math.Atan2(diff.Y, diff.X); angle < startAngle; angle += Math.PI * 2.0)
			{
			}
			Vector2 val = new Vector2((float)diff.Y, (float)diff.X);
			float length = ((Vector2)(ref val)).Length();
			foreach (RadialMount radialMount in RadialMounts)
			{
				if ((double)length < (double)mountIconSize * Math.Sqrt(2.0) / 2.0)
				{
					radialMount.Selected = radialMount.Default;
				}
				else
				{
					radialMount.Selected = radialMount.AngleBegin <= angle && radialMount.AngleEnd > angle;
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, radialMount.Texture, new Rectangle(radialMount.ImageX, radialMount.ImageY, mountIconSize, mountIconSize), (Rectangle?)null, Color.get_White() * (radialMount.Selected ? 1f : Module._settingMountRadialIconOpacity.get_Value()));
			}
		}

		private void DrawDbg(SpriteBatch spriteBatch, int position, string s)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, s, GameService.Content.get_DefaultFont32(), new Rectangle(new Point(0, position), new Point(400, 400)), Color.get_Red(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}

		public async Task TriggerSelectedMountAsync()
		{
			await (SelectedMount?.Mount.DoMountAction() ?? Task.CompletedTask);
		}

		private async Task HandleShown(object sender, EventArgs e)
		{
			Logger.Debug("HandleShown entered");
			bool isCursorVisible = GameService.Input.get_Mouse().get_CursorIsVisible();
			if (!isCursorVisible)
			{
				IsActionCamToggledOnMount = true;
				await _helper.TriggerKeybind(Module._settingMountRadialToggleActionCameraKeyBinding);
				Logger.Debug("HandleShown turned off action cam");
			}
			_maxRadialDiameter = Math.Min(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
			mountIconSize = (int)((float)(_maxRadialDiameter / 4) * Module._settingMountRadialIconSizeModifier.get_Value());
			radius = (int)((float)(_maxRadialDiameter / 2 - mountIconSize / 2) * Module._settingMountRadialRadiusModifier.get_Value());
			((Control)this).set_Size(new Point(_maxRadialDiameter, _maxRadialDiameter));
			if (Module._settingMountRadialSpawnAtMouse.get_Value() && isCursorVisible)
			{
				SpawnPoint = Control.get_Input().get_Mouse().get_Position();
			}
			else
			{
				Mouse.SetPosition(GameService.Graphics.get_WindowWidth() / 2, GameService.Graphics.get_WindowHeight() / 2, true);
				SpawnPoint = new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2);
			}
			((Control)this).set_Location(new Point(SpawnPoint.X - radius - mountIconSize / 2, SpawnPoint.Y - radius - mountIconSize / 2));
		}

		private async Task HandleHidden(object sender, EventArgs e)
		{
			Logger.Debug("HandleHidden entered");
			if (IsActionCamToggledOnMount)
			{
				await _helper.TriggerKeybind(Module._settingMountRadialToggleActionCameraKeyBinding);
				IsActionCamToggledOnMount = false;
				Logger.Debug("HandleHidden turned back on action cam");
			}
			await TriggerSelectedMountAsync();
		}
	}
}
