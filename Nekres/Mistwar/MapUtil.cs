using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Nekres.Mistwar
{
	internal static class MapUtil
	{
		private static AsyncCache<(Map, int), ContinentFloorRegionMap> _mapExpandedCache = new AsyncCache<(Map, int), ContinentFloorRegionMap>(((Map, int) p) => RequestMapExpanded(p.Item1, p.Item2));

		private static AsyncCache<int, Map> _mapCache = new AsyncCache<int, Map>(RequestMap);

		public static async Task<Map> GetMap(int mapId)
		{
			return await _mapCache.GetItem(mapId);
		}

		public static async Task<ContinentFloorRegionMap> GetMapExpanded(Map map, int floorId)
		{
			return await _mapExpandedCache.GetItem((map, floorId));
		}

		public static async Task BuildMap(Map map, string filePath, bool removeBackground = false, IProgress<string> progress = null)
		{
			if (map == null)
			{
				return;
			}
			Rectangle area = map.get_ContinentRect();
			int zoom = 6;
			int padding = 0;
			List<Point> tileArea = GetAreaTileList(area);
			Coordinates2 topLeftPx = ((Rectangle)(ref area)).get_TopLeft();
			Coordinates2 rightBottomPx = ((Rectangle)(ref area)).get_BottomRight();
			Point pxDelta = default(Point);
			((Point)(ref pxDelta))._002Ector((int)(((Coordinates2)(ref rightBottomPx)).get_X() - ((Coordinates2)(ref topLeftPx)).get_X()), (int)(((Coordinates2)(ref rightBottomPx)).get_Y() - ((Coordinates2)(ref topLeftPx)).get_Y()));
			Bitmap bmpDestination = new Bitmap(pxDelta.X + padding * 2, pxDelta.Y + padding * 2);
			using (Graphics gfx = Graphics.FromImage(bmpDestination))
			{
				gfx.CompositingMode = CompositingMode.SourceOver;
				foreach (Point p in tileArea)
				{
					progress?.Report($"Downloading {map.get_Name().Trim()} ({map.get_Id()})... {(float)tileArea.IndexOf(p) / (float)(tileArea.Count - 1) * 100f:N0}%");
					Bitmap tile = await GetTileImage(0, map.get_ContinentId(), map.get_DefaultFloor(), p.X, p.Y, zoom);
					if (tile != null)
					{
						using (tile)
						{
							long x = (long)((double)(p.X * tile.Width) - ((Coordinates2)(ref topLeftPx)).get_X() + (double)padding);
							long y = (long)((double)(p.Y * tile.Height) - ((Coordinates2)(ref topLeftPx)).get_Y() + (double)padding);
							gfx.DrawImage(tile, x, y, tile.Width, tile.Height);
						}
					}
				}
				gfx.Flush();
				if (removeBackground)
				{
					ContinentFloorRegionMap obj = await GetMapExpanded(map, map.get_DefaultFloor());
					GraphicsPath polygonPath = new GraphicsPath
					{
						FillMode = FillMode.Alternate
					};
					foreach (ContinentFloorRegionMapSector value in obj.get_Sectors().Values)
					{
						Point[] bbox = (from coord in value.get_Bounds()
							select Refit(coord, topLeftPx, padding)).ToArray();
						polygonPath.AddPolygon(bbox);
					}
					Region region = new Region();
					region.MakeInfinite();
					region.Exclude(polygonPath);
					gfx.CompositingMode = CompositingMode.SourceCopy;
					gfx.FillRegion(Brushes.Transparent, region);
				}
			}
			bmpDestination.Save(filePath, ImageFormat.Png);
			bmpDestination.Dispose();
		}

		public static Point Refit(Coordinates2 value, Coordinates2 destTopLeft, int padding = 0, int tileSize = 256)
		{
			Coordinates2 node = default(Coordinates2);
			((Coordinates2)(ref node))._002Ector(((Coordinates2)(ref value)).get_X() / (double)tileSize, ((Coordinates2)(ref value)).get_Y() / (double)tileSize);
			int x = (int)(((Coordinates2)(ref node)).get_X() * (double)tileSize - ((Coordinates2)(ref destTopLeft)).get_X() + (double)padding);
			int y = (int)(((Coordinates2)(ref node)).get_Y() * (double)tileSize - ((Coordinates2)(ref destTopLeft)).get_Y() + (double)padding);
			return new Point(x, y);
		}

		public static Point Refit(Point value, Coordinates2 destTopLeft, int padding = 0, int tileSize = 256)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Point val = new Point(value.X / tileSize, value.Y / tileSize);
			int x = (int)((double)(val.X * tileSize) - ((Coordinates2)(ref destTopLeft)).get_X() + (double)padding);
			int y = (int)((double)(val.Y * tileSize) - ((Coordinates2)(ref destTopLeft)).get_Y() + (double)padding);
			return new Point(x, y);
		}

		private static async Task<Map> RequestMap(int mapId)
		{
			try
			{
				return await ((IBulkExpandableClient<Map, int>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
					.get_Maps()).GetAsync(mapId, default(CancellationToken));
			}
			catch (Exception ex) when (ex is BadRequestException || ex is NotFoundException)
			{
				return null;
			}
			catch (UnexpectedStatusException)
			{
				MistwarModule.Logger.Warn(CommonStrings.WebApiDown);
				return null;
			}
		}

		private static async Task<ContinentFloorRegionMap> RequestMapExpanded(Map map, int floorId)
		{
			try
			{
				return await ((IBulkExpandableClient<ContinentFloorRegionMap, int>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
					.get_Continents()
					.get_Item(map.get_ContinentId())
					.get_Floors()
					.get_Item(floorId)
					.get_Regions()
					.get_Item(map.get_RegionId())
					.get_Maps()).GetAsync(map.get_Id(), default(CancellationToken));
			}
			catch (Exception ex) when (ex is BadRequestException || ex is NotFoundException)
			{
				return null;
			}
			catch (UnexpectedStatusException)
			{
				MistwarModule.Logger.Warn(CommonStrings.WebApiDown);
				return null;
			}
		}

		private static Point FromPixelToTileXY(Coordinates2 p, int zoom = 8)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			int tileSize = zoom * 32;
			return new Point((int)(((Coordinates2)(ref p)).get_X() / (double)tileSize), (int)(((Coordinates2)(ref p)).get_Y() / (double)tileSize));
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
			Point topLeft = FromPixelToTileXY(((Rectangle)(ref rect)).get_TopLeft());
			Point val = FromPixelToTileXY(((Rectangle)(ref rect)).get_BottomRight());
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

		private static async Task<Bitmap> GetTileImage(int dnsAlias, int continentId, int floor, int x, int y, int zoom = 6)
		{
			if (zoom < 0 || zoom > 7)
			{
				return null;
			}
			string dns = ((dnsAlias > 0 && dnsAlias < 5) ? dnsAlias.ToString() : string.Empty);
			Stream responseStream = (await WebRequest.Create($"https://tiles{dns}.guildwars2.com/{continentId}/{floor}/{zoom}/{x}/{y}.jpg").GetResponseAsync()).GetResponseStream();
			if (responseStream == null)
			{
				return null;
			}
			return new Bitmap(responseStream);
		}
	}
}
