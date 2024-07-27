using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Flurl.Http;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public class InteractiveMapControl : Control
	{
		private const string USER_AGENT = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";

		private const int TILE_SIZE = 256;

		private readonly string iconUrl;

		private readonly (double X, double Y) mapCoords;

		private readonly (int X, int Y) intMapCoords;

		private readonly List<List<double>> path;

		private readonly List<List<double>> bounds;

		private readonly int maxzoom;

		private readonly double floorId;

		private readonly double continentId;

		private readonly Waypoints waypoints;

		private readonly TileWhitelist tileWhiteList;

		private readonly ((double X, double Y) StartCoordinate, (double X, double Y) EndCoordinate) continentDimensions;

		private readonly string localTiles;

		private readonly (float X, float Y) mapBounds;

		private (int X, int Y) startCoordinate;

		private AsyncTexture2D icon;

		private AsyncTexture2D[,] tiles;

		private static AsyncTexture2D dot;

		private static AsyncTexture2D swirl;

		private static AsyncTexture2D flagStart;

		private static AsyncTexture2D flagEnd;

		public InteractiveMapControl(string iconUrl, string localTiles, string inputCoords, string path, string bounds)
			: this()
		{
			((Control)this).set_ClipsBounds(true);
			this.iconUrl = iconUrl;
			this.localTiles = localTiles;
			List<List<double>> coords = ConvertStringToNestedArray(inputCoords);
			this.path = ConvertStringToNestedArray(path);
			this.bounds = ConvertStringToNestedArray(bounds);
			List<List<double>> mastercoords = this.bounds;
			if (coords.Count > 0)
			{
				mastercoords = coords;
			}
			else if (this.path.Count > 0)
			{
				mastercoords = this.path;
			}
			floorId = 1.0;
			if (mastercoords[0].Count > 2)
			{
				floorId = mastercoords[0][2];
			}
			continentId = 1.0;
			if (mastercoords[0].Count > 3)
			{
				continentId = mastercoords[0][3];
			}
			(double, double) centroid = GetCentroid(mastercoords.Select((List<double> x) => (x[0], x[1])).ToList());
			GetSize(mastercoords.Select((List<double> x) => (x[0], x[1])).ToList());
			maxzoom = 8;
			continentDimensions = ((0.0, 0.0), (131072.0, 131072.0));
			if (continentId == 2.0)
			{
				maxzoom = 6;
				continentDimensions = ((0.0, 0.0), (16384.0, 16384.0));
			}
			mapBounds = ConvertCoordinates(continentDimensions.EndCoordinate, maxzoom);
			(Waypoints Waypoints, TileWhitelist TileWhitelist) tuple = InitializeWaypointsAndTiles();
			Waypoints waypoints = tuple.Waypoints;
			TileWhitelist tileWhiteList = tuple.TileWhitelist;
			this.waypoints = waypoints;
			this.tileWhiteList = tileWhiteList;
			(float, float) tuple2 = ConvertCoordinates(centroid, maxzoom);
			mapCoords = (tuple2.Item1, tuple2.Item2);
			intMapCoords = ((int)Math.Floor(mapCoords.X), (int)Math.Floor(mapCoords.Y));
			InitializeTextures();
		}

		static InteractiveMapControl()
		{
			InitializeStaticTextures();
		}

		protected override void DisposeControl()
		{
			AsyncTexture2D obj = icon;
			if (obj != null)
			{
				obj.Dispose();
			}
			for (int i = 0; i < tiles.GetLength(0); i++)
			{
				for (int j = 0; j < tiles.GetLength(1); j++)
				{
					AsyncTexture2D obj2 = tiles[i, j];
					if (obj2 != null)
					{
						obj2.Dispose();
					}
				}
			}
			((Control)this).DisposeControl();
		}

		private static void InitializeStaticTextures()
		{
			dot = InitializeTexture("https://wiki.guildwars2.com/images/2/23/Widget_map_dot.png");
			swirl = InitializeTexture("https://wiki.guildwars2.com/images/8/8d/Widget_map_yellow_swirl.png");
			flagStart = InitializeTexture("https://wiki.guildwars2.com/images/f/f0/Event_flag_green.png");
			flagEnd = InitializeTexture("https://wiki.guildwars2.com/images/8/8d/Event_flag_red.png");
		}

		private static AsyncTexture2D InitializeTexture(string url)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			AsyncTexture2D texture = new AsyncTexture2D(Textures.get_TransparentPixel());
			Stream imageStream;
			Task.Run(async delegate
			{
				try
				{
					imageStream = await url.WithHeader("user-agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36").GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0);
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice device)
					{
						texture.SwapTexture(TextureUtil.FromStreamPremultiplied(device, imageStream));
						imageStream.Close();
					});
				}
				catch (Exception ex)
				{
					FlurlHttpException httpException = ex.InnerException as FlurlHttpException;
					if (httpException == null)
					{
						throw;
					}
					if (!httpException.Message.Contains("404 (Not Found)"))
					{
						throw;
					}
				}
			});
			return texture;
		}

		private void InitializeTextures()
		{
			if (!string.IsNullOrEmpty(iconUrl))
			{
				icon = InitializeTexture(iconUrl);
			}
			InitializeTiles();
		}

		private void InitializeTiles()
		{
			string[,] tileUrls = GetTileUrls();
			tiles = new AsyncTexture2D[tileUrls.GetLength(0), tileUrls.GetLength(1)];
			for (int k = 0; k < tileUrls.GetLength(0); k++)
			{
				for (int l = 0; l < tileUrls.GetLength(1); l++)
				{
					tiles[k, l] = InitializeTexture(tileUrls[k, l]);
				}
			}
			int firstValidTileIndexX = -1;
			int lastValidTileIndexX = -1;
			int firstValidTileIndexY = -1;
			int lastValidTileIndexY = -1;
			for (int j = 0; j < tiles.GetLength(0); j++)
			{
				for (int m = 0; m < tiles.GetLength(1); m++)
				{
					if (tiles[j, m] != null && firstValidTileIndexX == -1 && firstValidTileIndexY == -1)
					{
						firstValidTileIndexX = j;
						firstValidTileIndexY = m;
					}
					if (firstValidTileIndexX != -1 && firstValidTileIndexY != -1 && j > firstValidTileIndexX && m > firstValidTileIndexY && j < tiles.GetLength(0) - 1 && m < tiles.GetLength(1) - 1 && (tiles[j + 1, m] == null || tiles[j, m + 1] == null))
					{
						lastValidTileIndexX = j;
						lastValidTileIndexY = m;
					}
				}
			}
			if (lastValidTileIndexX == -1)
			{
				lastValidTileIndexX = tiles.GetLength(0);
			}
			if (lastValidTileIndexY == -1)
			{
				lastValidTileIndexY = tiles.GetLength(1);
			}
			AsyncTexture2D[,] newTiles = new AsyncTexture2D[lastValidTileIndexX - firstValidTileIndexX, lastValidTileIndexY - firstValidTileIndexY];
			int i = firstValidTileIndexX;
			int newI = 0;
			while (i < lastValidTileIndexX)
			{
				int n = firstValidTileIndexY;
				int newJ = 0;
				while (n < lastValidTileIndexY)
				{
					newTiles[newI, newJ] = tiles[i, n];
					n++;
					newJ++;
				}
				i++;
				newI++;
			}
			startCoordinate.X += firstValidTileIndexX;
			startCoordinate.Y += firstValidTileIndexY;
			tiles = newTiles;
		}

		private string[,] GetTileUrls()
		{
			string[,] tileUrls = new string[3, 3];
			if (bounds.Count > 0)
			{
				IEnumerable<(int, int)> source = from x in bounds
					select ConvertCoordinates((x[0], x[1]), maxzoom) into x
					select ((int)Math.Floor(x.X), (int)Math.Floor(x.Y));
				int lowerBoundX3 = source.Min(((int, int) x) => x.Item1) - 1;
				int upperBoundX3 = source.Max(((int, int) x) => x.Item1) + 1;
				int lowerBoundY3 = source.Min(((int, int) x) => x.Item2) - 1;
				int upperBoundY3 = source.Max(((int, int) x) => x.Item2) + 1;
				startCoordinate.X = lowerBoundX3;
				startCoordinate.Y = lowerBoundY3;
				tileUrls = new string[upperBoundX3 - lowerBoundX3, upperBoundY3 - lowerBoundY3];
				for (int k = 0; k < upperBoundX3 - lowerBoundX3; k++)
				{
					for (int n = 0; n < upperBoundY3 - lowerBoundY3; n++)
					{
						tileUrls[k, n] = GetTileUrl((lowerBoundX3 + k, lowerBoundY3 + n, maxzoom), floorId, continentId, tileWhiteList);
					}
				}
			}
			else if (path.Count > 0)
			{
				IEnumerable<(int, int)> source2 = from x in path
					select ConvertCoordinates((x[0], x[1]), maxzoom) into x
					select ((int)Math.Floor(x.X), (int)Math.Floor(x.Y));
				int lowerBoundX2 = source2.Min(((int, int) x) => x.Item1) - 1;
				int upperBoundX2 = source2.Max(((int, int) x) => x.Item1) + 1;
				int lowerBoundY2 = source2.Min(((int, int) x) => x.Item2) - 1;
				int upperBoundY2 = source2.Max(((int, int) x) => x.Item2) + 1;
				startCoordinate.X = lowerBoundX2;
				startCoordinate.Y = lowerBoundY2;
				tileUrls = new string[upperBoundX2 - lowerBoundX2, upperBoundY2 - lowerBoundY2];
				for (int j = 0; j < upperBoundX2 - lowerBoundX2; j++)
				{
					for (int m = 0; m < upperBoundY2 - lowerBoundY2; m++)
					{
						tileUrls[j, m] = GetTileUrl((lowerBoundX2 + j, lowerBoundY2 + m, maxzoom), floorId, continentId, tileWhiteList);
					}
				}
			}
			else
			{
				int lowerBoundX = intMapCoords.X - 2;
				int upperBoundX = intMapCoords.X + 2;
				int lowerBoundY = intMapCoords.Y - 2;
				int upperBoundY = intMapCoords.Y + 2;
				startCoordinate.X = lowerBoundX;
				startCoordinate.Y = lowerBoundY;
				tileUrls = new string[upperBoundX - lowerBoundX, upperBoundY - lowerBoundY];
				for (int i = 0; i < upperBoundX - lowerBoundX; i++)
				{
					for (int l = 0; l < upperBoundY - lowerBoundY; l++)
					{
						tileUrls[i, l] = GetTileUrl((lowerBoundX + i, lowerBoundY + l, maxzoom), floorId, continentId, tileWhiteList);
					}
				}
			}
			return tileUrls;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_044f: Unknown result type (might be due to invalid IL or missing references)
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_053a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0546: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05be: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05df: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06de: Expected O, but got Unknown
			//IL_0705: Unknown result type (might be due to invalid IL or missing references)
			//IL_070c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0775: Unknown result type (might be due to invalid IL or missing references)
			//IL_077a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0789: Unknown result type (might be due to invalid IL or missing references)
			//IL_0795: Unknown result type (might be due to invalid IL or missing references)
			//IL_079b: Unknown result type (might be due to invalid IL or missing references)
			//IL_079f: Unknown result type (might be due to invalid IL or missing references)
			int tileWidth = ((Control)this).get_Width() / tiles.GetLength(0);
			int tileHeight = ((Control)this).get_Height() / tiles.GetLength(1);
			float scaleX = (float)tileWidth / 256f;
			float scaleY = (float)tileHeight / 256f;
			for (int j = 0; j < tiles.GetLength(0); j++)
			{
				for (int l = 0; l < tiles.GetLength(1); l++)
				{
					if (tiles[j, l] != null)
					{
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(tiles[j, l]), new Rectangle(j * tileWidth, l * tileHeight, tileWidth, tileHeight), Color.get_White());
					}
				}
			}
			float iconX = (float)((double)((intMapCoords.X - startCoordinate.X) * 256) + 256.0 * (mapCoords.X % 1.0));
			float iconY = (float)((double)((intMapCoords.Y - startCoordinate.Y) * 256) + 256.0 * (mapCoords.Y % 1.0));
			iconX = Scale(iconX, scaleX);
			iconY = Scale(iconY, scaleY);
			if (path.Count > 0)
			{
				(float, float) startImageCoordinates = ConvertCoordinates((path[0][0], path[0][1]), maxzoom);
				int startImageX = (int)((float)(((int)Math.Floor(startImageCoordinates.Item1) - startCoordinate.X) * 256) + 256f * (startImageCoordinates.Item1 % 1f));
				int startImageY = (int)((float)(((int)Math.Floor(startImageCoordinates.Item2) - startCoordinate.Y) * 256) + 256f * (startImageCoordinates.Item2 % 1f));
				(float, float) endImageCoordinates = ConvertCoordinates((path[path.Count - 1][0], path[path.Count - 1][1]), maxzoom);
				int endImageX = (int)((float)(((int)Math.Floor(endImageCoordinates.Item1) - startCoordinate.X) * 256) + 256f * (endImageCoordinates.Item1 % 1f));
				int endImageY = (int)((float)(((int)Math.Floor(endImageCoordinates.Item2) - startCoordinate.Y) * 256) + 256f * (endImageCoordinates.Item2 % 1f));
				Vector2[] points2 = ((IEnumerable<List<double>>)path).Select((Func<List<double>, Vector2>)delegate(List<double> x)
				{
					//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
					(float, float) tuple2 = ConvertCoordinates((x[0], x[1]), maxzoom);
					int num3 = (int)Math.Floor(tuple2.Item1);
					int num4 = (int)Math.Floor(tuple2.Item2);
					tuple2.Item1 = (int)((float)((num3 - startCoordinate.X) * 256) + 256f * (tuple2.Item1 % 1f));
					tuple2.Item2 = (int)((float)((num4 - startCoordinate.Y) * 256) + 256f * (tuple2.Item2 % 1f));
					return new Vector2(tuple2.Item1, tuple2.Item2);
				}).ToArray();
				Vector2 startPosition = default(Vector2);
				Vector2 nextPosition = default(Vector2);
				for (int k = 0; k < points2.Length - 1; k++)
				{
					((Vector2)(ref startPosition))._002Ector(Scale(points2[k].X, scaleX), Scale(points2[k].Y, scaleY));
					((Vector2)(ref nextPosition))._002Ector(Scale(points2[k + 1].X, scaleX), Scale(points2[k + 1].Y, scaleY));
					ShapeExtensions.DrawLine(spriteBatch, ToBounds(startPosition), ToBounds(nextPosition), Color.get_Yellow(), 10f, 0f);
				}
				float flagStartX = Scale(startImageX, scaleX) - Scale(flagStart.get_Texture().get_Width(), scaleX) / 2f;
				float flagStartY = Scale(startImageY, scaleY) - Scale(flagStart.get_Texture().get_Height(), scaleY) / 2f;
				float flagEndX = Scale(endImageX, scaleX) - Scale(flagEnd.get_Texture().get_Width(), scaleX) / 2f;
				float flagEndY = Scale(endImageY, scaleY) - Scale(flagEnd.get_Texture().get_Height(), scaleY) / 2f;
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(flagStart), ToBounds(new Vector2(flagStartX, flagStartY)), (Rectangle?)null, Color.get_White(), 0f, default(Vector2), new Vector2(scaleX, scaleY), (SpriteEffects)0, 0f);
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(flagEnd), ToBounds(new Vector2(flagEndX, flagEndY)), (Rectangle?)null, Color.get_White(), 0f, default(Vector2), new Vector2(scaleX, scaleY), (SpriteEffects)0, 0f);
			}
			if (!string.IsNullOrEmpty(iconUrl))
			{
				float pointX = iconX - Scale(icon.get_Texture().get_Width(), scaleX) / 2f;
				float pointY = iconY - Scale(icon.get_Texture().get_Height(), scaleY) / 2f;
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(icon), ToBounds(new Vector2(pointX, pointY)), (Rectangle?)null, Color.get_White(), 0f, default(Vector2), new Vector2(scaleX, scaleY), (SpriteEffects)0, 0f);
			}
			else
			{
				float dotX = iconX - Scale(dot.get_Texture().get_Width(), scaleX) / 2f;
				float dotY = iconY - Scale(dot.get_Texture().get_Height(), scaleY) / 2f;
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(dot), ToBounds(new Vector2(dotX, dotY)), (Rectangle?)null, Color.get_White(), 0f, default(Vector2), new Vector2(scaleX, scaleY), (SpriteEffects)0, 0f);
			}
			if (this.bounds.Count > 0)
			{
				Vector2[] points = ((IEnumerable<List<double>>)this.bounds).Select((Func<List<double>, Vector2>)delegate(List<double> x)
				{
					//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
					(float, float) tuple = ConvertCoordinates((x[0], x[1]), maxzoom);
					int num = (int)Math.Floor(tuple.Item1);
					int num2 = (int)Math.Floor(tuple.Item2);
					tuple.Item1 = (float)((num - startCoordinate.X) * 256) + 256f * (tuple.Item1 % 1f);
					tuple.Item2 = (float)((num2 - startCoordinate.Y) * 256) + 256f * (tuple.Item2 % 1f);
					return new Vector2(tuple.Item1, tuple.Item2);
				}).ToArray();
				Vector2 startPosition2 = default(Vector2);
				Vector2 nextPosition2 = default(Vector2);
				for (int i = 0; i < points.Length; i++)
				{
					int nextIndex = i + 1;
					if (i == points.Length - 1)
					{
						nextIndex = 0;
					}
					((Vector2)(ref startPosition2))._002Ector(Scale(points[i].X, scaleX), Scale(points[i].Y, scaleY));
					((Vector2)(ref nextPosition2))._002Ector(Scale(points[nextIndex].X, scaleX), Scale(points[nextIndex].Y, scaleY));
					ShapeExtensions.DrawLine(spriteBatch, ToBounds(startPosition2), ToBounds(nextPosition2), Color.get_Yellow(), 4f, 0f);
				}
				Polygon polygon = new Polygon((IEnumerable<Vector2>)points);
				ShapeExtensions.DrawPolygon(spriteBatch, points.OrderBy((Vector2 x) => x.Y).First(), polygon, Color.get_Yellow(), 4f, 0f);
			}
			else
			{
				float swirlX = iconX - Scale(swirl.get_Texture().get_Width(), scaleX) / 2f;
				float swirlY = iconY - Scale(swirl.get_Texture().get_Height(), scaleY) / 2f;
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(swirl), ToBounds(new Vector2(swirlX, swirlY)), (Rectangle?)null, Color.get_White(), 0f, default(Vector2), new Vector2(scaleX, scaleY), (SpriteEffects)0, 0f);
			}
		}

		private Vector2 ToBounds(Vector2 vector)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(vector.X + (float)((Control)this).get_AbsoluteBounds().X, vector.Y + (float)((Control)this).get_AbsoluteBounds().Y);
		}

		private float Scale(float input, float scale)
		{
			return input * scale;
		}

		private List<List<double>> ConvertStringToNestedArray(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return Array.Empty<List<double>>().ToList();
			}
			List<List<double>> result = new List<List<double>>();
			if (!input.Contains('['))
			{
				string[] array = input.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
				List<double> innerList2 = new List<double>();
				string[] array2 = array;
				foreach (string item in array2)
				{
					innerList2.Add(double.Parse(item, CultureInfo.InvariantCulture));
				}
				result.Add(innerList2);
			}
			else
			{
				string[] array2 = input.Replace("[", string.Empty).Split(new string[1] { "]" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string obj in array2)
				{
					List<double> innerList = new List<double>();
					string[] array3 = obj.Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string innerItem in array3)
					{
						innerList.Add(double.Parse(innerItem, CultureInfo.InvariantCulture));
					}
					result.Add(innerList);
				}
			}
			return result;
		}

		private string GenerateUrl(string fileName)
		{
			(string, string) parts = GenerateUrlComponents(fileName);
			return "https://wiki.guildwars2.com/images/" + parts.Item1 + "/" + parts.Item2 + "/" + fileName;
		}

		private (string FirstPart, string SecondPart) GenerateUrlComponents(string fileName)
		{
			string hex = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(fileName)), 0, 1);
			return (hex[0].ToString().ToLower(), hex.ToLower());
		}

		private static (double X, double Y) GetCentroid(List<(double X, double Y)> poly)
		{
			return poly.Aggregate(((double X, double Y) x, (double X, double Y) y) => (x.X + y.X / (double)poly.Count, x.Y + y.Y / (double)poly.Count));
		}

		private static (double X, double Y) GetSize(List<(double X, double Y)> polygon)
		{
			List<double> xValues = polygon.Select(((double X, double Y) x) => x.X).ToList();
			List<double> yValues = polygon.Select(((double X, double Y) y) => y.Y).ToList();
			double[] minimumArray = new double[2]
			{
				xValues.Min(),
				yValues.Min()
			};
			double[] maximumArray = new double[2]
			{
				xValues.Max(),
				yValues.Max()
			};
			return (maximumArray[0] - minimumArray[0], maximumArray[1] - minimumArray[1]);
		}

		private string GetTileUrl((int X, int Y, int Z) coordinates, double floorId, double continentId, TileWhitelist tileWhitelist)
		{
			if (floorId == 1.0)
			{
				TileWhitelist.Continent continent = tileWhitelist.Tyria;
				if (continentId == 2.0)
				{
					continent = tileWhitelist.Mists;
				}
				if (!continent.Floors.First((TileWhitelist.Floor x) => x.Id == coordinates.Z).Coordinates.Contains("X" + coordinates.X + "_Y" + coordinates.Y))
				{
					return "https://wiki.guildwars2.com/images/c/cb/World_map_tile_under_construction.png";
				}
				string file = $"World_map_tile_C{continentId}_F{floorId}_Z{coordinates.Z}_X{coordinates.X}_Y{coordinates.Y}.jpg";
				return GenerateUrl(file);
			}
			int xBodge = -1;
			int yBodge = -1;
			int zBodge = -1;
			if (continentId == 1.0)
			{
				xBodge = coordinates.X - (int)(128.0 / Math.Pow(2.0, 8 - coordinates.Z));
				yBodge = coordinates.Y - (int)(64.0 / Math.Pow(2.0, 8 - coordinates.Z));
				zBodge = coordinates.Z - 1;
			}
			else
			{
				xBodge = coordinates.X;
				yBodge = coordinates.Y;
				zBodge = coordinates.Z;
			}
			if (xBodge >= 0 && yBodge >= 0)
			{
				return $"https://tiles.guildwars2.com/{continentId}/{floorId}/{zBodge}/{xBodge}/{yBodge}.jpg";
			}
			return "https://wiki.guildwars2.com/images/c/cb/World_map_tile_under_construction.png";
		}

		private (float X, float Y) ConvertCoordinates((double X, double Y) gw2Coordinates, int zoom)
		{
			double scale = Math.Pow(2.0, zoom);
			double num = gw2Coordinates.X / scale;
			double y = gw2Coordinates.Y / scale;
			return ((float)num, (float)y);
		}

		private (Waypoints Waypoints, TileWhitelist TileWhitelist) InitializeWaypointsAndTiles()
		{
			string[] array = new WebClient
			{
				Headers = { { "user-agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36" } }
			}.DownloadString("https://wiki.guildwars2.com/index.php?title=Widget:Interactive_map_data_builder/infobox-map-output.js&action=raw").Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
			Waypoints waypoints = JsonSerializer.Deserialize<Waypoints>(array[0].Replace("var wiki_waypoints = ", string.Empty));
			TileWhitelist tiles = JsonSerializer.Deserialize<TileWhitelist>(array[1].Replace("var wiki_tile_whitelist = ", string.Empty), new JsonSerializerOptions
			{
				Converters = { (JsonConverter)new TileWhitelist.ContinentConverter() }
			});
			return (waypoints, tiles);
		}
	}
}
