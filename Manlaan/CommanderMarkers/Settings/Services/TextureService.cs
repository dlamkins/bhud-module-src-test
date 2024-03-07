using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.Settings.Services
{
	public class TextureService : IDisposable
	{
		public Texture2D SettingWindowBackground;

		public Texture2D _imgArrow;

		public Texture2D _imgCircle;

		public Texture2D _imgHeart;

		public Texture2D _imgSpiral;

		public Texture2D _imgSquare;

		public Texture2D _imgStar;

		public Texture2D _imgTriangle;

		public Texture2D _imgX;

		public Texture2D _imgArrowFade;

		public Texture2D _imgCircleFade;

		public Texture2D _imgHeartFade;

		public Texture2D _imgSpiralFade;

		public Texture2D _imgSquareFade;

		public Texture2D _imgStarFade;

		public Texture2D _imgTriangleFade;

		public Texture2D _imgXFade;

		public Texture2D _imgClear;

		public Texture2D _imgCheck;

		public Texture2D _blishHeart;

		public Texture2D _blishHeartSmall;

		public Texture2D IconEye;

		public Texture2D IconCopy;

		public Texture2D IconDelete;

		public Texture2D IconDeleteLarge;

		public Texture2D IconEdit;

		public Texture2D IconExport;

		public Texture2D IconGoBack;

		public Texture2D IconImport;

		public Texture2D IconSave;

		public Texture2D IconCorner;

		public TextureService(ContentsManager contentsManager)
		{
			SettingWindowBackground = contentsManager.GetTexture("window\\background.png");
			_imgArrow = contentsManager.GetTexture("arrow.png");
			_imgCircle = contentsManager.GetTexture("circle.png");
			_imgHeart = contentsManager.GetTexture("heart.png");
			_imgSpiral = contentsManager.GetTexture("spiral.png");
			_imgSquare = contentsManager.GetTexture("square.png");
			_imgStar = contentsManager.GetTexture("star.png");
			_imgTriangle = contentsManager.GetTexture("triangle.png");
			_imgX = contentsManager.GetTexture("x.png");
			_imgArrowFade = contentsManager.GetTexture("arrow_fade.png");
			_imgCircleFade = contentsManager.GetTexture("circle_fade.png");
			_imgHeartFade = contentsManager.GetTexture("heart_fade.png");
			_imgSpiralFade = contentsManager.GetTexture("spiral_fade.png");
			_imgSquareFade = contentsManager.GetTexture("square_fade.png");
			_imgStarFade = contentsManager.GetTexture("star_fade.png");
			_imgTriangleFade = contentsManager.GetTexture("triangle_fade.png");
			_imgXFade = contentsManager.GetTexture("x_fade.png");
			_imgClear = contentsManager.GetTexture("clear.png");
			_imgCheck = contentsManager.GetTexture("check.png");
			_blishHeart = contentsManager.GetTexture("mapmarker.png");
			_blishHeartSmall = contentsManager.GetTexture("mapmarker20.png");
			IconEye = contentsManager.GetTexture("eye.png");
			IconCopy = contentsManager.GetTexture("iconCopy.png");
			IconDelete = contentsManager.GetTexture("iconDelete.png");
			IconDeleteLarge = contentsManager.GetTexture("iconDelete48.png");
			IconEdit = contentsManager.GetTexture("iconEdit.png");
			IconExport = contentsManager.GetTexture("iconExport.png");
			IconGoBack = contentsManager.GetTexture("iconGoBack.png");
			IconImport = contentsManager.GetTexture("iconImport.png");
			IconSave = contentsManager.GetTexture("iconSave.png");
			IconCorner = contentsManager.GetTexture("cornerIcon.png");
		}

		public void Dispose()
		{
			Texture2D settingWindowBackground = SettingWindowBackground;
			if (settingWindowBackground != null)
			{
				((GraphicsResource)settingWindowBackground).Dispose();
			}
			Texture2D imgArrow = _imgArrow;
			if (imgArrow != null)
			{
				((GraphicsResource)imgArrow).Dispose();
			}
			Texture2D imgCircle = _imgCircle;
			if (imgCircle != null)
			{
				((GraphicsResource)imgCircle).Dispose();
			}
			Texture2D imgHeart = _imgHeart;
			if (imgHeart != null)
			{
				((GraphicsResource)imgHeart).Dispose();
			}
			Texture2D imgSpiral = _imgSpiral;
			if (imgSpiral != null)
			{
				((GraphicsResource)imgSpiral).Dispose();
			}
			Texture2D imgSquare = _imgSquare;
			if (imgSquare != null)
			{
				((GraphicsResource)imgSquare).Dispose();
			}
			Texture2D imgStar = _imgStar;
			if (imgStar != null)
			{
				((GraphicsResource)imgStar).Dispose();
			}
			Texture2D imgTriangle = _imgTriangle;
			if (imgTriangle != null)
			{
				((GraphicsResource)imgTriangle).Dispose();
			}
			Texture2D imgX = _imgX;
			if (imgX != null)
			{
				((GraphicsResource)imgX).Dispose();
			}
			Texture2D imgArrowFade = _imgArrowFade;
			if (imgArrowFade != null)
			{
				((GraphicsResource)imgArrowFade).Dispose();
			}
			Texture2D imgCircleFade = _imgCircleFade;
			if (imgCircleFade != null)
			{
				((GraphicsResource)imgCircleFade).Dispose();
			}
			Texture2D imgHeartFade = _imgHeartFade;
			if (imgHeartFade != null)
			{
				((GraphicsResource)imgHeartFade).Dispose();
			}
			Texture2D imgSpiralFade = _imgSpiralFade;
			if (imgSpiralFade != null)
			{
				((GraphicsResource)imgSpiralFade).Dispose();
			}
			Texture2D imgSquareFade = _imgSquareFade;
			if (imgSquareFade != null)
			{
				((GraphicsResource)imgSquareFade).Dispose();
			}
			Texture2D imgStarFade = _imgStarFade;
			if (imgStarFade != null)
			{
				((GraphicsResource)imgStarFade).Dispose();
			}
			Texture2D imgTriangleFade = _imgTriangleFade;
			if (imgTriangleFade != null)
			{
				((GraphicsResource)imgTriangleFade).Dispose();
			}
			Texture2D imgXFade = _imgXFade;
			if (imgXFade != null)
			{
				((GraphicsResource)imgXFade).Dispose();
			}
			Texture2D imgClear = _imgClear;
			if (imgClear != null)
			{
				((GraphicsResource)imgClear).Dispose();
			}
			Texture2D imgCheck = _imgCheck;
			if (imgCheck != null)
			{
				((GraphicsResource)imgCheck).Dispose();
			}
			Texture2D blishHeart = _blishHeart;
			if (blishHeart != null)
			{
				((GraphicsResource)blishHeart).Dispose();
			}
			Texture2D blishHeartSmall = _blishHeartSmall;
			if (blishHeartSmall != null)
			{
				((GraphicsResource)blishHeartSmall).Dispose();
			}
			Texture2D iconEye = IconEye;
			if (iconEye != null)
			{
				((GraphicsResource)iconEye).Dispose();
			}
			Texture2D iconCopy = IconCopy;
			if (iconCopy != null)
			{
				((GraphicsResource)iconCopy).Dispose();
			}
			Texture2D iconDelete = IconDelete;
			if (iconDelete != null)
			{
				((GraphicsResource)iconDelete).Dispose();
			}
			Texture2D iconDeleteLarge = IconDeleteLarge;
			if (iconDeleteLarge != null)
			{
				((GraphicsResource)iconDeleteLarge).Dispose();
			}
			Texture2D iconEdit = IconEdit;
			if (iconEdit != null)
			{
				((GraphicsResource)iconEdit).Dispose();
			}
			Texture2D iconExport = IconExport;
			if (iconExport != null)
			{
				((GraphicsResource)iconExport).Dispose();
			}
			Texture2D iconGoBack = IconGoBack;
			if (iconGoBack != null)
			{
				((GraphicsResource)iconGoBack).Dispose();
			}
			Texture2D iconImport = IconImport;
			if (iconImport != null)
			{
				((GraphicsResource)iconImport).Dispose();
			}
			Texture2D iconSave = IconSave;
			if (iconSave != null)
			{
				((GraphicsResource)iconSave).Dispose();
			}
			Texture2D iconCorner = IconCorner;
			if (iconCorner != null)
			{
				((GraphicsResource)iconCorner).Dispose();
			}
		}
	}
}
