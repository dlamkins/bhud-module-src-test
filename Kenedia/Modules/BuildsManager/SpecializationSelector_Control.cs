using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager
{
	public class SpecializationSelector_Control : Control
	{
		public Specialization_Control Specialization_Control;

		public int Index;

		public bool Elite;

		private Template _Template;

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

		public Template Template => BuildsManager.ModuleInstance.Selected_Template;

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
							Template.Build.SpecLines[Index].Control.UpdateLayout();
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
							Template.Build.SpecLines[Index].Specialization = spec;
							Template.SetChanged();
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
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), Textures.get_Pixel(), bounds.Add(((Control)this).get_Location()), (Rectangle?)bounds, new Color(0, 0, 0, 205), 0f, Vector2.get_Zero(), (SpriteEffects)0);
			if (Template.Build.Profession == null)
			{
				return;
			}
			int i = 0;
			int size = 64;
			Rectangle rect = default(Rectangle);
			foreach (API.Specialization spec in Template.Build.Profession.Specializations)
			{
				if (!spec.Elite || Elite)
				{
					((Rectangle)(ref rect))._002Ector(20 + i * size, (((Control)this).get_Height() - size) / 2, size, size);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)((Control)this).get_Parent(), spec.Icon.Texture, rect.Add(((Control)this).get_Location()), (Rectangle?)spec.Icon.Texture.get_Bounds(), (Specialization == spec || ((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition())) ? Color.get_White() : Color.get_Gray(), 0f, Vector2.get_Zero(), (SpriteEffects)0);
					i++;
				}
			}
		}

		public void SetTemplate()
		{
			_ = BuildsManager.ModuleInstance.Selected_Template;
		}

		public SpecializationSelector_Control()
			: this()
		{
		}
	}
}
