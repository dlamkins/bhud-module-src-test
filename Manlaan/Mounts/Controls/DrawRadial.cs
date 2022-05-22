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

		public DrawRadial(Helper helper)
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Visible(false);
			((Control)this).set_Padding(Thickness.Zero);
			_helper = helper;
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
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			RadialMounts.Clear();
			List<Mount> mounts = Module._availableOrderedMounts;
			Mount mountToPutInCenter = _helper.GetCenterMount();
			if (mountToPutInCenter != null && mountToPutInCenter.IsAvailable)
			{
				if (Module._settingMountRadialRemoveCenterMount.get_Value())
				{
					mounts.Remove(mountToPutInCenter);
				}
				Texture2D texture2 = _helper.GetImgFile(mountToPutInCenter.ImageFileName);
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
			double currentAngle = 0.0;
			double partAngleStep = Math.PI * 2.0 / (double)mounts.Count();
			foreach (Mount mount in mounts)
			{
				double angleMid = currentAngle + partAngleStep / 2.0;
				double angleEnd = currentAngle + partAngleStep;
				Texture2D texture = _helper.GetImgFile(mount.ImageFileName);
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
			double angle = Math.Atan2(diff.Y, diff.X);
			if (angle < 0.0)
			{
				angle += Math.PI * 2.0;
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
