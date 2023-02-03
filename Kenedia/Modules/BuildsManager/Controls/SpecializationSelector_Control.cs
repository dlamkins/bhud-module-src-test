using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Extensions;
using Kenedia.Modules.BuildsManager.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SpecializationSelector_Control : Control
	{
		public Specialization_Control Specialization_Control;

		public int Index;

		public bool Elite;

		private API.Specialization _Specialization;

		public API.Specialization Specialization
		{
			get
			{
				return _Specialization;
			}
			set
			{
				if (value != null)
				{
					_Specialization = value;
				}
			}
		}

		public Template Template => BuildsManager.s_moduleInstance.Selected_Template;

		public SpecializationSelector_Control()
			: this()
		{
			BuildsManager.s_moduleInstance.Selected_Template_Changed += ClosePopUp;
			Control.get_Input().get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)ClosePopUp);
		}

		private void ClosePopUp(object sender, EventArgs e)
		{
			if (!((Control)this).get_MouseOver())
			{
				((Control)this).Hide();
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnClick(e);
			int i = 0;
			int size = 64;
			if (Template.Build.Profession != null)
			{
				Rectangle rect = default(Rectangle);
				foreach (API.Specialization spec in Template.Build.Profession.Specializations)
				{
					if (spec.Elite && !Elite)
					{
						continue;
					}
					((Rectangle)(ref rect))._002Ector(20 + i * size, (((Control)this).get_Height() - size) / 2, size, size);
					if (((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition()))
					{
						SpecLine sp = Template.Build.SpecLines.Find((SpecLine x) => x.Specialization != null && x.Specialization.Id == spec.Id);
						if (sp != null && sp != Template.Build.SpecLines[Index])
						{
							Template.Build.SpecLines[Index].Specialization = sp.Specialization;
							Template.Build.SpecLines[Index].Traits = sp.Traits;
							Template.SetChanged();
							sp.Specialization = null;
							sp.Traits = new List<API.Trait>();
						}
						else if (Template.Build.SpecLines[Index] != null)
						{
							foreach (SpecLine specLine in Template.Build.SpecLines)
							{
								if (spec != Specialization && specLine.Specialization == spec)
								{
									specLine.Specialization = null;
									specLine.Traits = new List<API.Trait>();
								}
							}
							if (Template.Build.SpecLines[Index].Specialization != spec)
							{
								Template.Build.SpecLines[Index].Specialization = spec;
								Template.SetChanged();
							}
						}
						((Control)this).Hide();
						return;
					}
					i++;
				}
			}
			((Control)this).Hide();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), Textures.get_Pixel(), bounds.Add(((Control)this).get_Location()), (Rectangle?)bounds, new Color(0, 0, 0, 205), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			if (Template.Build.Profession == null)
			{
				return;
			}
			string text = string.Empty;
			int i = 0;
			int size = 64;
			Rectangle rect = default(Rectangle);
			foreach (API.Specialization spec in Template.Build.Profession.Specializations)
			{
				if (!spec.Elite || Elite)
				{
					((Rectangle)(ref rect))._002Ector(20 + i * size, (((Control)this).get_Height() - size) / 2, size, size);
					if (((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition()))
					{
						text = spec.Name;
					}
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), AsyncTexture2D.op_Implicit(spec.Icon._AsyncTexture), rect.Add(((Control)this).get_Location()), (Rectangle?)spec.Icon._AsyncTexture.get_Texture().get_Bounds(), (Specialization == spec || ((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition())) ? Color.get_White() : Color.get_Gray(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
					i++;
				}
			}
			if (text != string.Empty)
			{
				((Control)this).set_BasicTooltipText(text);
			}
			else
			{
				((Control)this).set_BasicTooltipText((string)null);
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			BuildsManager.s_moduleInstance.Selected_Template_Changed -= ClosePopUp;
			Control.get_Input().get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)ClosePopUp);
		}
	}
}
