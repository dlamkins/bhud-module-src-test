using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Windows
{
	public class CategoryInputWindow : StandardWindow
	{
		private const int WINDOW_WIDTH = 350;

		private const int WINDOW_HEIGHT = 180;

		private const int PADDING = 10;

		private readonly SpamCategoryDto _existingCategory;

		private TextBox _nameTextBox;

		private TextBox _descriptionTextBox;

		private StandardButton _saveButton;

		private StandardButton _cancelButton;

		public event EventHandler<CategoryInputResult> Confirmed;

		public CategoryInputWindow(SpamCategoryDto existingCategory = null)
			: this(Textures.get_Pixel(), new Rectangle(0, 0, 350, 180), new Rectangle(10, 10, 330, 160))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			_existingCategory = existingCategory;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Visible(false);
			((WindowBase2)this).set_Title((existingCategory != null) ? "Edit Category" : "New Category");
			((WindowBase2)this).set_SavesPosition(false);
			((WindowBase2)this).set_Id("Neokain_GW2_AllianceManager_categoryInputWindow");
			((Control)this).set_BackgroundColor(Color.get_Black());
			((WindowBase2)this).set_CanCloseWithEscape(true);
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 350) / 2);
			((Control)this).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 180) / 2);
			CreateControls();
		}

		private void CreateControls()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Expected O, but got Unknown
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Expected O, but got Unknown
			int yOffset = 0;
			int labelWidth = 80;
			int controlHeight = 28;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Name:");
			((Control)val).set_Left(0);
			((Control)val).set_Top(yOffset + 5);
			((Control)val).set_Width(labelWidth);
			val.set_AutoSizeHeight(true);
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Left(labelWidth);
			((Control)val2).set_Top(yOffset);
			((Control)val2).set_Width(((Container)this).get_ContentRegion().Width - labelWidth);
			((Control)val2).set_Height(controlHeight);
			((TextInputBase)val2).set_PlaceholderText("Category name...");
			((TextInputBase)val2).set_Text(_existingCategory?.Name ?? string.Empty);
			_nameTextBox = val2;
			yOffset += controlHeight + 10;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Description:");
			((Control)val3).set_Left(0);
			((Control)val3).set_Top(yOffset + 5);
			((Control)val3).set_Width(labelWidth);
			val3.set_AutoSizeHeight(true);
			TextBox val4 = new TextBox();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Left(labelWidth);
			((Control)val4).set_Top(yOffset);
			((Control)val4).set_Width(((Container)this).get_ContentRegion().Width - labelWidth);
			((Control)val4).set_Height(controlHeight);
			((TextInputBase)val4).set_PlaceholderText("Optional description...");
			((TextInputBase)val4).set_Text(_existingCategory?.Description ?? string.Empty);
			_descriptionTextBox = val4;
			yOffset += controlHeight + 20;
			int buttonWidth = 100;
			int buttonSpacing = 10;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text((_existingCategory != null) ? "Save" : "Create");
			((Control)val5).set_Width(buttonWidth);
			((Control)val5).set_Left(((Container)this).get_ContentRegion().Width - buttonWidth * 2 - buttonSpacing);
			((Control)val5).set_Top(yOffset);
			_saveButton = val5;
			((Control)_saveButton).add_Click((EventHandler<MouseEventArgs>)SaveButton_Click);
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("Cancel");
			((Control)val6).set_Width(buttonWidth);
			((Control)val6).set_Left(((Container)this).get_ContentRegion().Width - buttonWidth);
			((Control)val6).set_Top(yOffset);
			_cancelButton = val6;
			((Control)_cancelButton).add_Click((EventHandler<MouseEventArgs>)CancelButton_Click);
		}

		private void SaveButton_Click(object sender, MouseEventArgs e)
		{
			string name = ((TextInputBase)_nameTextBox).get_Text()?.Trim();
			if (string.IsNullOrEmpty(name))
			{
				ScreenNotification.ShowNotification("Category name is required", (NotificationType)1, (Texture2D)null, 4);
				return;
			}
			CategoryInputResult result = new CategoryInputResult
			{
				CategoryId = _existingCategory?.Id,
				Name = name,
				Description = ((TextInputBase)_descriptionTextBox).get_Text()?.Trim(),
				DisplayOrder = (_existingCategory?.DisplayOrder ?? 0)
			};
			this.Confirmed?.Invoke(this, result);
			((Control)this).Hide();
		}

		private void CancelButton_Click(object sender, MouseEventArgs e)
		{
			((Control)this).Hide();
		}

		protected override void DisposeControl()
		{
			if (_saveButton != null)
			{
				((Control)_saveButton).remove_Click((EventHandler<MouseEventArgs>)SaveButton_Click);
			}
			if (_cancelButton != null)
			{
				((Control)_cancelButton).remove_Click((EventHandler<MouseEventArgs>)CancelButton_Click);
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
