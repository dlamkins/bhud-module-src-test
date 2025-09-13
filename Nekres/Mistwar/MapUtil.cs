using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Nekres.Mistwar
{
	internal static class MapUtil
	{
		private const int MAX_ZOOM = 7;

		private const int TILE_SIZE = 256;

		private const int PADDING = 0;

		public static async Task BuildMap(Map map, string filePath, bool removeBackground = false, IProgress<string> progress = null)
		{
			if (map == null)
			{
				return;
			}
			Rectangle area = map.get_ContinentRect();
			List<Point> tileArea = GetAreaTileList(area);
			Coordinates2 topLeftPx = ((Rectangle)(ref area)).get_TopLeft();
			Coordinates2 rightBottomPx = ((Rectangle)(ref area)).get_BottomRight();
			Point pxDelta = default(Point);
			((Point)(ref pxDelta))._002Ector((int)(((Coordinates2)(ref rightBottomPx)).get_X() - ((Coordinates2)(ref topLeftPx)).get_X()), (int)(((Coordinates2)(ref rightBottomPx)).get_Y() - ((Coordinates2)(ref topLeftPx)).get_Y()));
			Bitmap bmpDestination = new Bitmap(pxDelta.X, pxDelta.Y);
			using (Graphics gfx = Graphics.FromImage(bmpDestination))
			{
				gfx.CompositingMode = CompositingMode.SourceOver;
				foreach (Point p in tileArea)
				{
					progress?.Report($"Downloading {map.get_Name().Trim()} ({map.get_Id()})... {(float)tileArea.IndexOf(p) / (float)(tileArea.Count - 1) * 100f:N0}%");
					using Bitmap tile = await GetTileImage(0, map.get_ContinentId(), map.get_DefaultFloor(), p.X, p.Y, 7);
					if (tile != null)
					{
						long x = (long)((double)(p.X * tile.Width) - ((Coordinates2)(ref topLeftPx)).get_X() + 0.0);
						long y = (long)((double)(p.Y * tile.Height) - ((Coordinates2)(ref topLeftPx)).get_Y() + 0.0);
						gfx.DrawImage(tile, x, y, tile.Width, tile.Height);
					}
				}
				gfx.Flush();
				if (removeBackground)
				{
					ContinentFloorRegionMap mapExp = await MistwarModule.ModuleInstance.Resources.GetMapExpanded(map, map.get_DefaultFloor());
					if (mapExp != null && mapExp.get_Sectors().Count > 0)
					{
						GraphicsPath polygonPath = new GraphicsPath();
						polygonPath.FillMode = FillMode.Alternate;
						foreach (ContinentFloorRegionMapSector value in mapExp.get_Sectors().Values)
						{
							Point[] bbox = (from coord in value.get_Bounds()
								select Refit(coord, topLeftPx)).ToArray();
							polygonPath.AddPolygon(bbox);
						}
						Region region = new Region();
						region.MakeInfinite();
						region.Exclude(polygonPath);
						gfx.CompositingMode = CompositingMode.SourceCopy;
						gfx.FillRegion(Brushes.Transparent, region);
					}
				}
			}
			bmpDestination.Save(filePath, ImageFormat.Png);
			bmpDestination.Dispose();
		}

		public static Point Refit(Coordinates2 value, Coordinates2 destTopLeft, int padding = 0)
		{
			Coordinates2 node = default(Coordinates2);
			((Coordinates2)(ref node))._002Ector(((Coordinates2)(ref value)).get_X() / 256.0, ((Coordinates2)(ref value)).get_Y() / 256.0);
			int x = (int)(((Coordinates2)(ref node)).get_X() * 256.0 - ((Coordinates2)(ref destTopLeft)).get_X() + (double)padding);
			int y = (int)(((Coordinates2)(ref node)).get_Y() * 256.0 - ((Coordinates2)(ref destTopLeft)).get_Y() + (double)padding);
			return new Point(x, y);
		}

		private static Point FromPixelToTileXy(Coordinates2 p, int zoom = 7)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return new Point((int)(((Coordinates2)(ref p)).get_X() / 256.0), (int)(((Coordinates2)(ref p)).get_Y() / 256.0));
		}

		private static List<Point> GetAreaTileList(Rectangle rect)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			Point topLeft = FromPixelToTileXy(((Rectangle)(ref rect)).get_TopLeft());
			Point val = FromPixelToTileXy(((Rectangle)(ref rect)).get_BottomRight());
			int x = Math.Max(0, topLeft.X);
			int toX = val.X;
			int y2 = Math.Max(0, topLeft.Y);
			int toY = val.Y;
			List<Point> list = new List<Point>((toX - x + 1) * (toY - y2 + 1));
			for (; x <= toX; x++)
			{
				for (int y = y2; y <= toY; y++)
				{
					list.Add(new Point(x, y));
				}
			}
			return list;
		}

		private static async Task<Bitmap> GetTileImage(int dnsAlias, int continentId, int floor, int x, int y, int zoom)
		{
			if (zoom < 0 || zoom > 7)
			{
				throw new ArgumentOutOfRangeException("zoom");
			}
			string dns = ((dnsAlias >= 1 && dnsAlias <= 4) ? dnsAlias.ToString() : "1");
			string url = $"https://tiles{dns}.guildwars2.com/{continentId}/{floor}/{zoom}/{x}/{y}.jpg";
			try
			{
				HttpClient client = new HttpClient();
				try
				{
					using Stream responseStream = await client.GetStreamAsync(url);
					using MemoryStream ms = new MemoryStream();
					await responseStream.CopyToAsync(ms);
					ms.Position = 0L;
					return new Bitmap(ms);
				}
				finally
				{
					((IDisposable)client)?.Dispose();
				}
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
