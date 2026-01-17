using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using NpcFinder.Models;
using NpcFinder.Services;
using NpcFinder.Util;

namespace NpcFinder.Controls
{
	public class NpcFinderWindow : StandardWindow
	{
		private readonly WikiNpcService _wiki;

		private readonly Gw2MapIndexService _mapIndex;

		private readonly Gw2ApiService _gw2;

		private readonly NpcMerchantResolverService _merchantResolver;

		private readonly CancellationTokenSource _cts;

		private readonly Func<int> _currentContinentIdProvider;

		private readonly Action<NpcTarget> _setTarget;

		private Panel _contentRoot;

		private Panel _resultsViewport;

		private TextBox _searchBox;

		private StandardButton _searchBtn;

		private FlowPanel _resultsPanel;

		private Label _status;

		private StandardButton _clearBtn;

		private readonly Action _clearTarget;

		private readonly Action _clearCache;

		private StandardButton _clearCacheBtn;

		public NpcFinderWindow(AsyncTexture2D background, WikiNpcService wiki, Gw2MapIndexService mapIndex, Gw2ApiService gw2, NpcMerchantResolverService merchantResolver, CancellationTokenSource cts, Func<int> currentContinentIdProvider, Action<NpcTarget> setTarget, Action clearTarget, Action clearCache)
			: this(background, new Rectangle(5, 60, 600, 550), new Rectangle(40, 70, 520, 310))
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			_wiki = wiki;
			_mapIndex = mapIndex;
			_gw2 = gw2;
			_merchantResolver = merchantResolver;
			_cts = cts;
			_currentContinentIdProvider = currentContinentIdProvider;
			_setTarget = setTarget;
			_clearTarget = clearTarget;
			_clearCache = clearCache;
			((WindowBase2)this).set_Title("Abattele's NPC Finder");
			((WindowBase2)this).set_CanResize(false);
			((WindowBase2)this).set_SavesPosition(true);
			BuildUi();
		}

		private void BuildUi()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Expected O, but got Unknown
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Expected O, but got Unknown
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(40, 40));
			((Control)val).set_Size(new Point(520, 340));
			((Control)val).set_ClipsBounds(false);
			_contentRoot = val;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)_contentRoot);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(360);
			((TextInputBase)val2).set_PlaceholderText("NPC name...");
			_searchBox = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_contentRoot);
			((Control)val3).set_Location(new Point(370, -2));
			((Control)val3).set_Size(new Point(110, 34));
			val3.set_Text("Search");
			_searchBtn = val3;
			((Control)_searchBtn).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await DoSearchAsync();
			});
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)_contentRoot);
			((Control)val4).set_Location(new Point(0, 40));
			val4.set_AutoSizeWidth(true);
			val4.set_Text("Awaiting NPC selection from the list below...");
			_status = val4;
			Panel val5 = new Panel();
			((Control)val5).set_Parent((Container)(object)_contentRoot);
			((Control)val5).set_Location(new Point(0, 70));
			((Control)val5).set_Size(new Point(520, 230));
			((Control)val5).set_ClipsBounds(true);
			_resultsViewport = val5;
			FlowPanel val6 = new FlowPanel();
			((Control)val6).set_Parent((Container)(object)_resultsViewport);
			((Control)val6).set_Location(new Point(0, 0));
			((Control)val6).set_Size(((Control)_resultsViewport).get_Size());
			((Panel)val6).set_CanScroll(true);
			val6.set_FlowDirection((ControlFlowDirection)3);
			val6.set_ControlPadding(new Vector2(0f, 4f));
			_resultsPanel = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)_contentRoot);
			((Control)val7).set_Location(new Point(0, 306));
			((Control)val7).set_Size(new Point(220, 34));
			val7.set_Text("Remove current marker");
			_clearBtn = val7;
			((Control)_clearBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_clearTarget?.Invoke();
				_status.set_Text("Marker removed.");
				((Container)_resultsPanel).ClearChildren();
			});
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)_contentRoot);
			((Control)val8).set_Location(new Point(230, 306));
			((Control)val8).set_Size(new Point(140, 34));
			val8.set_Text("Delete cache");
			_clearCacheBtn = val8;
			((Control)_clearCacheBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					_clearCache?.Invoke();
					_status.set_Text("Cache deleted.");
					((Container)_resultsPanel).ClearChildren();
				}
				catch (Exception ex)
				{
					_status.set_Text("Cache delete failed: " + ex.Message);
				}
			});
		}

		private async Task DoSearchAsync()
		{
			string q = (((TextInputBase)_searchBox).get_Text() ?? "").Trim();
			if (q.Length == 0)
			{
				return;
			}
			((Container)_resultsPanel).ClearChildren();
			_status.set_Text("Searching wiki...");
			try
			{
				WikiLookupResult res = await _wiki.ResolveByNpcNameAsync(q, _cts.Token);
				if (res == null)
				{
					_status.set_Text("No results.");
				}
				else if (res.CandidateTitles != null && res.CandidateTitles.Count > 1 && (res.Hits == null || res.Hits.Count == 0))
				{
					_status.set_Text("Multiple pages found. Pick one:");
					foreach (string t in res.CandidateTitles)
					{
						AddTitleChoice(t);
					}
				}
				else if (res.Hits != null && res.Hits.Count > 0)
				{
					_status.set_Text($"Found {res.Hits.Count} hit(s). Resolving maps...");
					foreach (NpcCandidateHit h in res.Hits)
					{
						await AddResolvedHitAsync(h);
					}
					_status.set_Text("Done. Click a hit to set marker.");
				}
				else
				{
					_status.set_Text("Found page, but no coordinates parsed.");
				}
			}
			catch (OperationCanceledException)
			{
				_status.set_Text("Cancelled.");
			}
			catch (Exception ex)
			{
				_status.set_Text("Error: " + ex.Message);
			}
		}

		private void AddTitleChoice(string title)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)_resultsPanel);
			((Control)val).set_Size(new Point(((Control)_resultsViewport).get_Width() - 25, 34));
			val.set_Text(title);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				((Container)_resultsPanel).ClearChildren();
				_status.set_Text("Loading page...");
				try
				{
					WikiLookupResult res = await _wiki.ResolveByTitleAsync(title, _cts.Token);
					if (res == null || res.Hits == null || res.Hits.Count == 0)
					{
						if (_merchantResolver == null)
						{
							_status.set_Text("Merchant resolver not initialized.");
						}
						else
						{
							_status.set_Text("No direct coordinates. Resolving via GW2 API...");
							List<NpcResolvedHit> resolved = await _merchantResolver.ResolveMerchantAsync(title, _cts.Token);
							if (resolved == null || resolved.Count == 0)
							{
								_status.set_Text("No anchor found via GW2 API.");
							}
							else
							{
								_status.set_Text("Pick an anchor location:");
								foreach (NpcResolvedHit r in resolved)
								{
									AddResolvedRow(r);
								}
							}
						}
					}
					else
					{
						_status.set_Text($"Found {res.Hits.Count} hit(s). Resolving maps...");
						foreach (NpcCandidateHit h in res.Hits)
						{
							await AddResolvedHitAsync(h);
						}
						_status.set_Text("Done. Click a hit to set marker.");
					}
				}
				catch (Exception ex)
				{
					_status.set_Text("Error: " + ex.Message);
				}
			});
		}

		private void AddResolvedRow(NpcResolvedHit r)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)_resultsPanel);
			((Control)val).set_Size(new Point(((Control)_resultsViewport).get_Width() - 25, 34));
			val.set_Text(r.MapName + " | " + r.Source);
			((Control)val).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				_status.set_Text("Setting marker...");
				Gw2MapInfo mapInfo = await _gw2.GetMapInfoAsync(r.MapId, _cts.Token);
				NpcTarget target = new NpcTarget
				{
					WikiTitle = r.Title,
					DisplayName = r.Title,
					MapId = r.MapId,
					MapName = r.MapName,
					TargetContinentId = r.ContinentId,
					TargetContinentX = r.ContinentX,
					TargetContinentY = r.ContinentY,
					MapInfo = mapInfo
				};
				Logger.GetLogger<NpcFinderWindow>().Warn($"[UI] Row clicked -> setting target: map={target.MapName} cont={target.TargetContinentId}");
				MumbleReader.DumpUiOnce();
				_setTarget?.Invoke(target);
				int cur = ((_currentContinentIdProvider != null) ? _currentContinentIdProvider() : 0);
				if (cur != 0 && target.TargetContinentId != 0 && cur != target.TargetContinentId)
				{
					_status.set_Text("You are on " + ContinentNames.Name(cur) + "; target is on " + ContinentNames.Name(target.TargetContinentId) + ".");
				}
				else
				{
					_status.set_Text("Marker set (" + r.Source + "). Open world map.");
				}
			});
		}

		private async Task AddResolvedHitAsync(NpcCandidateHit h)
		{
			int? mapId = h.MapId;
			if (!mapId.HasValue && !string.IsNullOrWhiteSpace(h.MapName))
			{
				mapId = await _mapIndex.ResolveMapIdByNameAsync(h.MapName, _cts.Token);
			}
			Gw2MapInfo mapInfo = null;
			double cx = 0.0;
			double cy = 0.0;
			if (mapId.HasValue)
			{
				mapInfo = await _gw2.GetMapInfoAsync(mapId.Value, _cts.Token);
				if (mapInfo != null)
				{
					(cx, cy) = CoordConverter.MapToContinent(h.X, h.Y, mapInfo.MapRect, mapInfo.ContinentRect);
				}
			}
			int curCont = ((_currentContinentIdProvider != null) ? _currentContinentIdProvider() : 0);
			int hitCont = ((mapInfo != null) ? mapInfo.ContinentId : 0);
			string mapLabel = ((mapInfo != null) ? mapInfo.Name : (h.MapName ?? "(unknown map)"));
			string contLabel = ((hitCont != 0) ? ContinentNames.Name(hitCont) : "Unknown continent");
			string warn = ((curCont != 0 && hitCont != 0 && curCont != hitCont) ? " !!! different continent" : "");
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)_resultsPanel);
			((Control)val).set_Size(new Point(((Control)_resultsViewport).get_Width() - 25, 34));
			val.set_Text($"{mapLabel} | [{h.X},{h.Y}] | {contLabel}{warn}");
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (mapInfo == null)
				{
					_status.set_Text("Can't place marker: map unresolved.");
				}
				else
				{
					NpcTarget npcTarget = new NpcTarget
					{
						WikiTitle = h.Title,
						DisplayName = h.Title,
						MapId = mapInfo.Id,
						MapName = mapInfo.Name,
						TargetMapX = h.X,
						TargetMapY = h.Y,
						TargetContinentId = mapInfo.ContinentId,
						TargetContinentX = cx,
						TargetContinentY = cy,
						MapInfo = mapInfo
					};
					if (curCont != 0 && npcTarget.TargetContinentId != 0 && curCont != npcTarget.TargetContinentId)
					{
						_status.set_Text("You are on " + ContinentNames.Name(curCont) + ". Target is on " + ContinentNames.Name(npcTarget.TargetContinentId) + ". Teleport there, then open map.");
					}
					else
					{
						_status.set_Text($"Marker set: {npcTarget.MapName} [{npcTarget.TargetMapX},{npcTarget.TargetMapY}]");
					}
					_setTarget?.Invoke(npcTarget);
				}
			});
		}
	}
}
