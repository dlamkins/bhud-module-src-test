using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Manlaan.Mounts.Controls
{
	internal class DrawRadial : Container
	{
		private readonly Helper _helper;

		private List<RadialMount> RadialMounts = new List<RadialMount>();

		private int radius = 0;

		private int mountIconSize = 0;

		private int _maxRadialDiameter = 0;

		private Point SpawnPoint = default(Point);

		private RadialMount SelectedMount => RadialMounts.SingleOrDefault((RadialMount m) => m.Selected);

		public DrawRadial(Helper helper)
			: this()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Visible(false);
			((Control)this).set_Padding(Thickness.Zero);
			_helper = helper;
		}

		protected override CaptureType CapturesInput()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return (CaptureType)4;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Expected O, but got Unknown
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Expected O, but got Unknown
			RadialMounts.Clear();
			List<Mount> mounts = Module._availableOrderedMounts;
			if (Module._settingMountRadialCenterMountBehavior.get_Value() != "None")
			{
				Mount mountToPutInCenter = null;
				string value = Module._settingMountRadialCenterMountBehavior.get_Value();
				string text = value;
				if (!(text == "Default"))
				{
					if (text == "LastUsed")
					{
						mountToPutInCenter = _helper.GetLastUsedMount();
					}
				}
				else
				{
					mountToPutInCenter = _helper.GetDefaultMount();
				}
				if (mountToPutInCenter != null)
				{
					mounts.Remove(mountToPutInCenter);
					Texture2D texture2 = _helper.GetImgFile(mountToPutInCenter.ImageFileName);
					int loc = radius;
					RadialMounts.Add(new RadialMount
					{
						Texture = (Texture)(object)texture2,
						Mount = mountToPutInCenter,
						ImageX = loc,
						ImageY = loc,
						Default = true
					});
				}
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
					Texture = (Texture)(object)texture,
					Mount = mount,
					ImageX = x,
					ImageY = y,
					AngleBegin = currentAngle,
					AngleEnd = angleEnd
				});
				currentAngle = angleEnd;
			}
			Point mousePos = Control.get_Input().get_Mouse().get_Position();
			Point diff = mousePos - SpawnPoint;
			double angle = Math.Atan2(diff.Y, diff.X);
			if (angle < 0.0)
			{
				angle += Math.PI * 2.0;
			}
			Vector2 val = new Vector2((float)diff.Y, (float)diff.X);
			float length = ((Vector2)(ref val)).Length();
			((Container)this).get_Children().Clear();
			foreach (RadialMount radialMount in RadialMounts)
			{
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)this);
				val2.set_Texture(AsyncTexture2D.op_Implicit((Texture2D)radialMount.Texture));
				((Control)val2).set_Size(new Point(mountIconSize, mountIconSize));
				((Control)val2).set_Location(new Point(radialMount.ImageX, radialMount.ImageY));
				((Control)val2).set_BasicTooltipText(radialMount.Mount.DisplayName);
				Image _btnMount = val2;
				if ((double)length < (double)mountIconSize * Math.Sqrt(2.0) / 2.0)
				{
					radialMount.Selected = radialMount.Default;
				}
				else
				{
					radialMount.Selected = radialMount.AngleBegin <= angle && radialMount.AngleEnd > angle;
				}
				((Control)_btnMount).set_Opacity(radialMount.Selected ? 1f : 0.5f);
				((Container)this).AddChild((Control)(object)_btnMount);
			}
		}

		public void TriggerSelectedMount()
		{
			SelectedMount?.Mount.DoHotKey();
		}

		internal void Start()
		{
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_Visible())
			{
				_maxRadialDiameter = Math.Min(((Control)GameService.Graphics.get_SpriteScreen()).get_Width(), ((Control)GameService.Graphics.get_SpriteScreen()).get_Height());
				mountIconSize = (int)((float)(_maxRadialDiameter / 4) * Module._settingMountRadialIconSizeModifier.get_Value());
				radius = (int)((float)(_maxRadialDiameter / 2 - mountIconSize / 2) * Module._settingMountRadialRadiusModifier.get_Value());
				((Control)this).set_Size(new Point(_maxRadialDiameter, _maxRadialDiameter));
				if (Module._settingMountRadialSpawnAtMouse.get_Value())
				{
					SpawnPoint = Control.get_Input().get_Mouse().get_Position();
				}
				else
				{
					Mouse.SetPosition(GameService.Graphics.get_WindowWidth() / 2, GameService.Graphics.get_WindowHeight() / 2);
					SpawnPoint = new Point(((Control)GameService.Graphics.get_SpriteScreen()).get_Width() / 2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Height() / 2);
				}
				((Control)this).set_Location(new Point(SpawnPoint.X - radius - mountIconSize / 2, SpawnPoint.Y - radius - mountIconSize / 2));
			}
			((Control)this).set_Visible(true);
		}

		internal void Stop()
		{
			TriggerSelectedMount();
			((Control)this).set_Visible(false);
		}
	}
}
