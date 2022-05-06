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

		public Texture2D ToolSearchWindowEmblem { get; }

		public Texture2D CornerIconTexture { get; }

		public Texture2D HoverCornerIconTexture { get; }

		public Texture2D BankTexture { get; }

		public Texture2D CharacterInventoryTexture { get; }

		public Texture2D SharedInventoryTexture { get; }

		public Texture2D EquipmentTexture { get; }

		public Texture2D UnknownToolTexture { get; }

		public TextureService(ContentsManager contentsManager)
		{
			ReminderBackgroundTexture = contentsManager.GetTexture("logoutOverlay\\logoutDialogTextArea.png");
			Tool1Texture = contentsManager.GetTexture("logoutOverlay\\tool1_1998933.png");
			Tool2Texture = contentsManager.GetTexture("logoutOverlay\\tool2_1998934.png");
			Tool3Texture = contentsManager.GetTexture("logoutOverlay\\tool3_1998935.png");
			WindowBackgroundTexture = contentsManager.GetTexture("toolSearch\\textures\\windowsBackground_155985.png");
			ToolSearchWindowEmblem = contentsManager.GetTexture("toolSearch\\textures\\toolSearchWindowEmblem.png");
			CornerIconTexture = contentsManager.GetTexture("toolSearch\\textures\\cornerIcon.png");
			HoverCornerIconTexture = contentsManager.GetTexture("toolSearch\\textures\\cornerIcon_hover.png");
			BankTexture = contentsManager.GetTexture("toolSearch\\textures\\bank_156670.png");
			CharacterInventoryTexture = contentsManager.GetTexture("toolSearch\\textures\\inventory_157098.png");
			SharedInventoryTexture = contentsManager.GetTexture("toolSearch\\textures\\sharedInventory.png");
			EquipmentTexture = contentsManager.GetTexture("toolSearch\\textures\\equipment_156714.png");
			UnknownToolTexture = contentsManager.GetTexture("toolSearch\\textures\\unknownTool_66591.png");
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
			Texture2D toolSearchWindowEmblem = ToolSearchWindowEmblem;
			if (toolSearchWindowEmblem != null)
			{
				((GraphicsResource)toolSearchWindowEmblem).Dispose();
			}
			Texture2D cornerIconTexture = CornerIconTexture;
			if (cornerIconTexture != null)
			{
				((GraphicsResource)cornerIconTexture).Dispose();
			}
			Texture2D hoverCornerIconTexture = HoverCornerIconTexture;
			if (hoverCornerIconTexture != null)
			{
				((GraphicsResource)hoverCornerIconTexture).Dispose();
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
			Texture2D unknownToolTexture = UnknownToolTexture;
			if (unknownToolTexture != null)
			{
				((GraphicsResource)unknownToolTexture).Dispose();
			}
		}
	}
}
