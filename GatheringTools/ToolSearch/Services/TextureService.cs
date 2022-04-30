using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace GatheringTools.ToolSearch.Services
{
	public class TextureService : IDisposable
	{
		public Texture2D ReminderBackgroundTexture { get; }

		public Texture2D Tool1Texture { get; }

		public Texture2D Tool2Texture { get; }

		public Texture2D Tool3Texture { get; }

		public Texture2D WindowBackgroundTexture { get; }

		public Texture2D SickleTexture { get; }

		public Texture2D BankTexture { get; }

		public Texture2D CharacterInventoryTexture { get; }

		public Texture2D SharedInventoryTexture { get; }

		public Texture2D EquipmentTexture { get; }

		public TextureService(ContentsManager contentsManager)
		{
			ReminderBackgroundTexture = contentsManager.GetTexture("logoutOverlay\\logoutDialogTextArea.png");
			Tool1Texture = contentsManager.GetTexture("logoutOverlay\\tool1_1998933.png");
			Tool2Texture = contentsManager.GetTexture("logoutOverlay\\tool2_1998934.png");
			Tool3Texture = contentsManager.GetTexture("logoutOverlay\\tool3_1998935.png");
			WindowBackgroundTexture = contentsManager.GetTexture("toolSearch\\windowsBackground_155985.png");
			SickleTexture = contentsManager.GetTexture("toolSearch\\sickle.png");
			BankTexture = contentsManager.GetTexture("toolSearch\\bank_156670.png");
			CharacterInventoryTexture = contentsManager.GetTexture("toolSearch\\inventory_157098.png");
			SharedInventoryTexture = contentsManager.GetTexture("toolSearch\\sharedInventory.png");
			EquipmentTexture = contentsManager.GetTexture("toolSearch\\equipment_156714.png");
		}

		public void Dispose()
		{
			Texture2D reminderBackgroundTexture = ReminderBackgroundTexture;
			if (reminderBackgroundTexture != null)
			{
				((GraphicsResource)reminderBackgroundTexture).Dispose();
			}
			Texture2D tool1Texture = Tool1Texture;
			if (tool1Texture != null)
			{
				((GraphicsResource)tool1Texture).Dispose();
			}
			Texture2D tool2Texture = Tool2Texture;
			if (tool2Texture != null)
			{
				((GraphicsResource)tool2Texture).Dispose();
			}
			Texture2D tool3Texture = Tool3Texture;
			if (tool3Texture != null)
			{
				((GraphicsResource)tool3Texture).Dispose();
			}
			Texture2D windowBackgroundTexture = WindowBackgroundTexture;
			if (windowBackgroundTexture != null)
			{
				((GraphicsResource)windowBackgroundTexture).Dispose();
			}
			Texture2D sickleTexture = SickleTexture;
			if (sickleTexture != null)
			{
				((GraphicsResource)sickleTexture).Dispose();
			}
			Texture2D bankTexture = BankTexture;
			if (bankTexture != null)
			{
				((GraphicsResource)bankTexture).Dispose();
			}
			Texture2D characterInventoryTexture = CharacterInventoryTexture;
			if (characterInventoryTexture != null)
			{
				((GraphicsResource)characterInventoryTexture).Dispose();
			}
			Texture2D sharedInventoryTexture = SharedInventoryTexture;
			if (sharedInventoryTexture != null)
			{
				((GraphicsResource)sharedInventoryTexture).Dispose();
			}
			Texture2D equipmentTexture = EquipmentTexture;
			if (equipmentTexture != null)
			{
				((GraphicsResource)equipmentTexture).Dispose();
			}
		}
	}
}
