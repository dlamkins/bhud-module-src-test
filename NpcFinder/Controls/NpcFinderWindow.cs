using System;
using System.Collections.Generic;
using System.Linq;
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
		private static readonly bool DEBUG_LOGS;

		private const int MAX_MAP_RESULTS = 120;

		private Checkbox _mapModeCheckbox;

		private bool _mapMode;

		private CancellationToken _activeSearchToken;

		private FlowPanel _suggestPanel;

		private int _suggestReqId;

		private readonly SemaphoreSlim _suggestGate = new SemaphoreSlim(1, 1);

		private CancellationTokenSource _suggestCts;

		private const int MaxSuggestButtons = 12;

		private StandardButton[] _suggestBtns;

		private string[] _suggestValues;

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

		private Label _changeLog;

		private StandardButton _clearBtn;

		private readonly Action _clearTarget;

		private readonly Action _clearCache;

		private StandardButton _clearCacheBtn;

		private readonly SemaphoreSlim _searchGate = new SemaphoreSlim(1, 1);

		private CancellationTokenSource _searchCts;

		private readonly Dictionary<long, Gw2MapInfo> _continentPointMemo = new Dictionary<long, Gw2MapInfo>();

		public NpcFinderWindow(AsyncTexture2D background, WikiNpcService wiki, Gw2MapIndexService mapIndex, Gw2ApiService gw2, NpcMerchantResolverService merchantResolver, CancellationTokenSource cts, Func<int> currentContinentIdProvider, Action<NpcTarget> setTarget, Action clearTarget, Action clearCache)
			: this(background, new Rectangle(5, 60, 580, 590), new Rectangle(40, 70, 480, 350))
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
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

		private static string Norm(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				return "";
			}
			s = s.Trim().ToLowerInvariant();
			return s.Replace("_", " ");
		}

		private static int ScoreTitle(string title, string q)
		{
			string t = Norm(title);
			string qq = Norm(q);
			if (t == qq)
			{
				return 1000;
			}
			if (t.StartsWith(qq))
			{
				return 900;
			}
			if (t.Contains(" " + qq))
			{
				return 780;
			}
			if (t.Contains(qq))
			{
				return 650;
			}
			return 0;
		}

		private void RenderSuggestions(List<(string label, string value)> merged)
		{
			if (_suggestBtns == null || _suggestValues == null)
			{
				return;
			}
			int j = Math.Min(12, merged?.Count ?? 0);
			for (int i = 0; i < 12; i++)
			{
				StandardButton b = _suggestBtns[i];
				if (b != null)
				{
					if (i < j)
					{
						b.set_Text(merged[i].label);
						_suggestValues[i] = merged[i].value;
						((Control)b).set_Visible(true);
					}
					else
					{
						((Control)b).set_Visible(false);
						_suggestValues[i] = null;
					}
				}
			}
			((Control)_suggestPanel).set_Visible(j > 0);
			ForceFlowPanelLayout(_suggestPanel);
		}

		private void HideSuggestions()
		{
			if (_suggestPanel == null)
			{
				return;
			}
			if (_suggestBtns != null)
			{
				for (int i = 0; i < _suggestBtns.Length; i++)
				{
					if (_suggestBtns[i] != null)
					{
						((Control)_suggestBtns[i]).set_Visible(false);
					}
					if (_suggestValues != null && i < _suggestValues.Length)
					{
						_suggestValues[i] = null;
					}
				}
			}
			((Control)_suggestPanel).set_Visible(false);
			ForceFlowPanelLayout(_suggestPanel);
		}

		private void CancelSuggest()
		{
			try
			{
				_suggestCts?.Cancel();
			}
			catch
			{
			}
			try
			{
				_suggestCts?.Dispose();
			}
			catch
			{
			}
			_suggestCts = null;
		}

		private CancellationToken BeginSuggestToken()
		{
			CancelSuggest();
			_suggestCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);
			return _suggestCts.Token;
		}

		private async Task<List<(string label, string value)>> BuildMergedSuggestionsAsync(string text, bool mapMode, CancellationToken ct)
		{
			if (mapMode)
			{
				return (from m in ((await _mapIndex.SuggestMapNamesAsync(text, 12, ct).ConfigureAwait(continueOnCapturedContext: false)) ?? new List<string>()).Distinct(StringComparer.OrdinalIgnoreCase).Take(12)
					select (m, m)).ToList();
			}
			Task<List<string>> mapTask = _mapIndex.SuggestMapNamesAsync(text, 6, ct);
			Task<List<string>> sugTask = _wiki.SuggestTitlesAsync(text, 10, ct);
			Task<List<string>> searchTask = Task.FromResult(new List<string>());
			if ((text?.Trim().Length ?? 0) >= 3)
			{
				searchTask = _wiki.SearchTitlesAsync(text, 20, ct);
			}
			await Task.WhenAll<List<string>>(mapTask, sugTask, searchTask).ConfigureAwait(continueOnCapturedContext: false);
			List<string> obj = mapTask.Result ?? new List<string>();
			List<string> wikiSug = sugTask.Result ?? new List<string>();
			List<string> wikiFind = searchTask.Result ?? new List<string>();
			HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<(string label, string value, int score)> merged = new List<(string, string, int)>();
			foreach (string i in obj)
			{
				if (!string.IsNullOrWhiteSpace(i) && seen.Add(i))
				{
					merged.Add(("Map: " + i, i, 400));
				}
			}
			addWikiList(wikiSug);
			addWikiList(wikiFind);
			return (from x in merged.OrderByDescending(((string label, string value, int score) x) => x.score).ThenBy(((string label, string value, int score) x) => x.value, StringComparer.OrdinalIgnoreCase).Take(12)
				select (x.label, x.value)).ToList();
			void addWikiList(IEnumerable<string> titles)
			{
				foreach (string t in titles)
				{
					if (!string.IsNullOrWhiteSpace(t) && seen.Add(t))
					{
						merged.Add((t, t, ScoreTitle(t, text)));
					}
				}
			}
		}

		private async Task UpdateSuggestionsAsync()
		{
			int myId = Interlocked.Increment(ref _suggestReqId);
			string q = (((TextInputBase)_searchBox).get_Text() ?? "").Trim();
			if (q.Length < 2)
			{
				HideSuggestions();
				return;
			}
			await _suggestGate.WaitAsync();
			try
			{
				if (myId != _suggestReqId)
				{
					return;
				}
				CancellationToken ct = BeginSuggestToken();
				await Task.Delay(250, ct);
				ct.ThrowIfCancellationRequested();
				if (myId != _suggestReqId)
				{
					return;
				}
				q = (((TextInputBase)_searchBox).get_Text() ?? "").Trim();
				if (q.Length < 2)
				{
					HideSuggestions();
					return;
				}
				List<(string, string)> merged = await BuildMergedSuggestionsAsync(q, _mapMode, ct);
				ct.ThrowIfCancellationRequested();
				if (myId == _suggestReqId)
				{
					if (merged == null || merged.Count == 0)
					{
						HideSuggestions();
					}
					else
					{
						RenderSuggestions(merged);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch
			{
				HideSuggestions();
			}
			finally
			{
				_suggestGate.Release();
			}
		}

		private void BuildUi()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected O, but got Unknown
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Expected O, but got Unknown
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Expected O, but got Unknown
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Expected O, but got Unknown
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Expected O, but got Unknown
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Expected O, but got Unknown
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Expected O, but got Unknown
			//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Expected O, but got Unknown
			//IL_0407: Unknown result type (might be due to invalid IL or missing references)
			//IL_040c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Unknown result type (might be due to invalid IL or missing references)
			//IL_0459: Expected O, but got Unknown
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_0482: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Expected O, but got Unknown
			Rectangle cr = ((Container)this).get_ContentRegion();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(40, 40));
			((Control)val).set_Size(new Point(520, 450));
			((Control)val).set_ClipsBounds(false);
			_contentRoot = val;
			Checkbox val2 = new Checkbox();
			((Control)val2).set_Parent((Container)(object)_contentRoot);
			((Control)val2).set_Location(new Point(350, 50));
			((Control)val2).set_Size(new Point(260, 20));
			val2.set_Text("Map mode ?");
			val2.set_Checked(false);
			_mapModeCheckbox = val2;
			_mapModeCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				_mapMode = _mapModeCheckbox.get_Checked();
				((Container)_resultsPanel).ClearChildren();
				HideSuggestions();
				((TextInputBase)_searchBox).set_Text("");
				((TextInputBase)_searchBox).set_PlaceholderText(_mapMode ? "Map name..." : "NPC/WP/POI name...");
				_status.set_Text(_mapMode ? "Map mode: type a map name, then Search to list NPCs." : "Awaiting NPC selection from the list below...");
			});
			TextBox val3 = new TextBox();
			((Control)val3).set_Parent((Container)(object)_contentRoot);
			((Control)val3).set_Location(new Point(0, 0));
			((Control)val3).set_Width(cr.Width - 30 - 110);
			((TextInputBase)val3).set_PlaceholderText("NPC/WP/POI name...");
			_searchBox = val3;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)_contentRoot);
			((Control)val4).set_Location(new Point(((Control)_searchBox).get_Location().X, ((Control)_searchBox).get_Location().Y + 34));
			((Control)val4).set_Size(new Point(((Control)_searchBox).get_Width(), 120));
			((Control)val4).set_ClipsBounds(true);
			((Panel)val4).set_CanScroll(true);
			val4.set_FlowDirection((ControlFlowDirection)3);
			val4.set_ControlPadding(new Vector2(0f, 2f));
			((Control)val4).set_Visible(false);
			_suggestPanel = val4;
			_suggestBtns = (StandardButton[])(object)new StandardButton[12];
			_suggestValues = new string[12];
			for (int i = 0; i < 12; i++)
			{
				int idx = i;
				StandardButton val5 = new StandardButton();
				((Control)val5).set_Parent((Container)(object)_suggestPanel);
				((Control)val5).set_Size(new Point(((Control)_suggestPanel).get_Width() - 18, 28));
				val5.set_Text("");
				((Control)val5).set_Visible(false);
				StandardButton b = val5;
				((Control)b).add_Click((EventHandler<MouseEventArgs>)async delegate
				{
					string val13 = _suggestValues[idx];
					if (!string.IsNullOrWhiteSpace(val13))
					{
						((TextInputBase)_searchBox).set_Text(val13);
						HideSuggestions();
						await DoSearchAsync();
					}
				});
				_suggestBtns[i] = b;
			}
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)_contentRoot);
			((Control)val6).set_Location(new Point(((Control)_searchBox).get_Right() + 10, ((Control)_searchBox).get_Top() - 2));
			((Control)val6).set_Size(new Point(110, 34));
			val6.set_Text("Search");
			_searchBtn = val6;
			((Control)_searchBtn).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await DoSearchAsync();
			});
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)_contentRoot);
			((Control)val7).set_Location(new Point(0, ((Control)_suggestPanel).get_Location().Y + ((Control)_suggestPanel).get_Height() + 8));
			val7.set_AutoSizeWidth(true);
			val7.set_Text("Awaiting NPC selection from the list below...");
			_status = val7;
			int bottomButtonsH = 34;
			int bottomY = cr.Height - 10 - bottomButtonsH;
			Panel val8 = new Panel();
			((Control)val8).set_Parent((Container)(object)_contentRoot);
			((Control)val8).set_Location(new Point(((Control)_searchBox).get_Left(), ((Control)_status).get_Bottom() + 8));
			((Control)val8).set_Size(new Point(cr.Width - 20, bottomY - (((Control)_status).get_Bottom() - 32)));
			((Control)val8).set_ClipsBounds(true);
			_resultsViewport = val8;
			FlowPanel val9 = new FlowPanel();
			((Control)val9).set_Parent((Container)(object)_resultsViewport);
			((Control)val9).set_Location(new Point(0, 0));
			((Control)val9).set_Size(((Control)_resultsViewport).get_Size());
			((Panel)val9).set_CanScroll(true);
			val9.set_FlowDirection((ControlFlowDirection)3);
			val9.set_ControlPadding(new Vector2(0f, 4f));
			_resultsPanel = val9;
			StandardButton val10 = new StandardButton();
			((Control)val10).set_Parent((Container)(object)_contentRoot);
			((Control)val10).set_Location(new Point(((Control)_searchBox).get_Left(), 355));
			((Control)val10).set_Size(new Point(220, 34));
			val10.set_Text("Remove marker / Stop search");
			_clearBtn = val10;
			((Control)_clearBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				StopSearch();
				_clearTarget?.Invoke();
				((Container)_resultsPanel).ClearChildren();
				_status.set_Text("Marker removed (and search stopped if it was running).");
			});
			StandardButton val11 = new StandardButton();
			((Control)val11).set_Parent((Container)(object)_contentRoot);
			((Control)val11).set_Location(new Point(cr.Width - 10 - 160, 355));
			((Control)val11).set_Size(new Point(160, 34));
			val11.set_Text("Delete cache");
			_clearCacheBtn = val11;
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
			Label val12 = new Label();
			((Control)val12).set_Parent((Container)(object)_contentRoot);
			((Control)val12).set_Location(new Point(50, 410));
			val12.set_AutoSizeWidth(true);
			val12.set_Text("For more info check the changelog (right click the addon)");
			_changeLog = val12;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				UpdateSuggestionsAsync();
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
				Gw2MapInfo mapInfo = await _gw2.GetMapInfoAsync(r.MapId, _activeSearchToken);
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
				if (DEBUG_LOGS)
				{
					Logger.GetLogger<NpcFinderWindow>().Warn($"[UI] Row clicked -> setting target: map={target.MapName} cont={target.TargetContinentId}");
				}
				MumbleReader.DumpUiOnce();
				_setTarget?.Invoke(target);
				int cur = ((_currentContinentIdProvider != null) ? _currentContinentIdProvider() : 0);
				if (cur != 0 && target.TargetContinentId != 0 && cur != target.TargetContinentId)
				{
					_status.set_Text("You are on " + ContinentNames.Name(cur) + ". Target is on " + ContinentNames.Name(target.TargetContinentId) + ". Teleport continents.");
				}
				else
				{
					_status.set_Text("Marker set (" + r.Source + "). Open world map.");
				}
			});
		}

		private async Task<Gw2MapInfo> ResolveMapInfoByContinentPointAsync(int cx, int cy, CancellationToken ct)
		{
			long key = ((long)cx << 32) ^ (uint)cy;
			if (_continentPointMemo.TryGetValue(key, out var memo) && memo != null)
			{
				return memo;
			}
			int curCont = ((_currentContinentIdProvider != null) ? _currentContinentIdProvider() : 0);
			int? mapId = await _mapIndex.FindMapIdByContinentPointAsync(cx, cy, curCont, ct).ConfigureAwait(continueOnCapturedContext: false);
			if (!mapId.HasValue)
			{
				return null;
			}
			Gw2MapInfo mi = await _gw2.GetMapInfoAsync(mapId.Value, ct).ConfigureAwait(continueOnCapturedContext: false);
			if (mi != null)
			{
				_continentPointMemo[key] = mi;
			}
			return mi;
		}

		private async Task AddResolvedHitAsync(NpcCandidateHit h, CancellationToken ct)
		{
			int curCont = ((_currentContinentIdProvider != null) ? _currentContinentIdProvider() : 0);
			await Task.Yield();
			ct.ThrowIfCancellationRequested();
			int? mapId = h.MapId;
			if (!mapId.HasValue && !string.IsNullOrWhiteSpace(h.MapName))
			{
				mapId = await _mapIndex.ResolveMapIdByNameAsync(h.MapName, ct);
			}
			Gw2MapInfo mapInfo = null;
			if (mapId.HasValue)
			{
				mapInfo = await _gw2.GetMapInfoAsync(mapId.Value, ct);
			}
			double cx = 0.0;
			double cy = 0.0;
			if (mapInfo != null)
			{
				if (LooksLikeMapCoordsFor(mapInfo))
				{
					(cx, cy) = CoordConverter.MapToContinent(h.X, h.Y, mapInfo.MapRect, mapInfo.ContinentRect);
				}
				else
				{
					cx = h.X;
					cy = h.Y;
				}
			}
			else if (h.X > 20000 || h.Y > 20000)
			{
				mapInfo = await ResolveMapInfoByContinentPointAsync(h.X, h.Y, ct);
				if (mapInfo != null)
				{
					new int?(mapInfo.Id);
					cx = h.X;
					cy = h.Y;
				}
			}
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
					_setTarget?.Invoke(npcTarget);
					if (curCont != 0 && npcTarget.TargetContinentId != 0 && curCont != npcTarget.TargetContinentId)
					{
						_status.set_Text("You are on " + ContinentNames.Name(curCont) + ". Target is on " + ContinentNames.Name(npcTarget.TargetContinentId) + ". Teleport continents.");
					}
					else
					{
						_status.set_Text("Marker set. Open world map.");
					}
				}
			});
			bool LooksLikeMapCoordsFor(Gw2MapInfo mi)
			{
				if (mi == null)
				{
					return false;
				}
				double minX = Math.Min(mi.MapRect.X1, mi.MapRect.X2);
				double maxX = Math.Max(mi.MapRect.X1, mi.MapRect.X2);
				double minY = Math.Min(mi.MapRect.Y1, mi.MapRect.Y2);
				double maxY = Math.Max(mi.MapRect.Y1, mi.MapRect.Y2);
				if ((double)h.X >= minX && (double)h.X <= maxX && (double)h.Y >= minY)
				{
					return (double)h.Y <= maxY;
				}
				return false;
			}
		}

		private async Task DoMapSearchAsync(string mapName)
		{
			await _searchGate.WaitAsync();
			try
			{
				CancellationToken ct = BeginNewSearchToken();
				((Control)_searchBtn).set_Enabled(false);
				((Control)_searchBox).set_Enabled(false);
				((Container)_resultsPanel).ClearChildren();
				_status.set_Text("Searching NPCs on that map...");
				int? mapId = await _mapIndex.ResolveMapIdByNameAsync(mapName, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (!mapId.HasValue || mapId.Value <= 0)
				{
					_status.set_Text("Map not recognized. Pick one from suggestions.");
					return;
				}
				List<string> titles = await _wiki.SearchNpcTitlesByMapAsync(mapName, 120, ct).ConfigureAwait(continueOnCapturedContext: false);
				if (titles == null || titles.Count == 0)
				{
					_status.set_Text("No NPC pages found for that map (wiki search came back empty).");
					return;
				}
				_status.set_Text($"Found {titles.Count} NPCs. Click one to resolve location.");
				foreach (string t in titles)
				{
					AddTitleChoice(t);
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
			finally
			{
				((Control)_searchBtn).set_Enabled(true);
				((Control)_searchBox).set_Enabled(true);
				_searchGate.Release();
			}
		}

		private async Task DoSearchAsync()
		{
			HideSuggestions();
			CancelSuggest();
			string q = (((TextInputBase)_searchBox).get_Text() ?? "").Trim();
			if (q.Length == 0)
			{
				return;
			}
			if (_mapMode)
			{
				await DoMapSearchAsync(q);
				return;
			}
			await _searchGate.WaitAsync();
			try
			{
				CancellationToken ct = BeginNewSearchToken();
				((Control)_searchBtn).set_Enabled(false);
				((Control)_searchBox).set_Enabled(false);
				((Container)_resultsPanel).ClearChildren();
				_status.set_Text("Searching wiki...");
				WikiLookupResult res = await _wiki.ResolveByNpcNameAsync(q, ct);
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
					HashSet<int> shownMapIds = new HashSet<int>();
					foreach (NpcCandidateHit h in res.Hits)
					{
						int? mid = await AddResolvedHitAsync_ReturnMapId(h, ct);
						if (mid.HasValue)
						{
							shownMapIds.Add(mid.Value);
						}
					}
					if (_merchantResolver != null)
					{
						_status.set_Text("Also checking other locations (anchors)...");
						List<NpcResolvedHit> anchors2 = await _merchantResolver.ResolveMerchantAsync(res.Title ?? q, ct);
						if (anchors2 != null)
						{
							foreach (NpcResolvedHit a2 in anchors2)
							{
								if (a2 != null && !shownMapIds.Contains(a2.MapId))
								{
									AddResolvedRow(a2);
									shownMapIds.Add(a2.MapId);
								}
							}
						}
					}
					_status.set_Text("Done. Click a hit to set marker.");
				}
				else if (_merchantResolver != null)
				{
					_status.set_Text("No direct coordinates. Resolving via GW2 API (anchors)...");
					List<NpcResolvedHit> anchors = await _merchantResolver.ResolveMerchantAsync(res.Title ?? q, ct);
					if (anchors != null && anchors.Count > 0)
					{
						_status.set_Text("Pick an anchor location:");
						foreach (NpcResolvedHit a in anchors)
						{
							AddResolvedRow(a);
						}
					}
					else
					{
						_status.set_Text("Found page, but no coordinates parsed (and no anchor found).");
					}
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
			finally
			{
				((Control)_searchBtn).set_Enabled(true);
				((Control)_searchBox).set_Enabled(true);
				_searchGate.Release();
			}
		}

		private static void ForceFlowPanelLayout(FlowPanel fp)
		{
			if (fp != null)
			{
				try
				{
					((Control)fp).Invalidate();
					((Control)fp).RecalculateLayout();
				}
				catch
				{
				}
			}
		}

		private CancellationToken BeginNewSearchToken()
		{
			try
			{
				_searchCts?.Cancel();
			}
			catch
			{
			}
			try
			{
				_searchCts?.Dispose();
			}
			catch
			{
			}
			_searchCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);
			_activeSearchToken = _searchCts.Token;
			return _activeSearchToken;
		}

		private void StopSearch()
		{
			try
			{
				_searchCts?.Cancel();
			}
			catch
			{
			}
			((Control)_searchBtn).set_Enabled(true);
			((Control)_searchBox).set_Enabled(true);
			_status.set_Text("Search cancelled.");
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
				await _searchGate.WaitAsync();
				try
				{
					CancellationToken ct = BeginNewSearchToken();
					((Control)_searchBtn).set_Enabled(false);
					((Control)_searchBox).set_Enabled(false);
					((Container)_resultsPanel).ClearChildren();
					_status.set_Text("Loading page...");
					WikiLookupResult res = await _wiki.ResolveByTitleAsync(title, ct);
					if (res == null || res.Hits == null || res.Hits.Count == 0)
					{
						if (_merchantResolver == null)
						{
							_status.set_Text("Merchant resolver not initialized.");
						}
						else
						{
							_status.set_Text("No direct coordinates. Resolving via GW2 API...");
							List<NpcResolvedHit> resolved = await _merchantResolver.ResolveMerchantAsync(title, ct);
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
						HashSet<int> shownMapIds = new HashSet<int>();
						foreach (NpcCandidateHit h in res.Hits)
						{
							int? mid = await AddResolvedHitAsync_ReturnMapId(h, ct);
							if (mid.HasValue)
							{
								shownMapIds.Add(mid.Value);
							}
						}
						if (_merchantResolver != null)
						{
							_status.set_Text("Also checking other locations (anchors)...");
							List<NpcResolvedHit> anchors = await _merchantResolver.ResolveMerchantAsync(title, ct);
							if (anchors != null)
							{
								foreach (NpcResolvedHit a in anchors)
								{
									if (a != null && !shownMapIds.Contains(a.MapId))
									{
										AddResolvedRow(a);
										shownMapIds.Add(a.MapId);
									}
								}
							}
						}
						_status.set_Text("Done. Click a hit to set marker.");
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
				finally
				{
					((Control)_searchBtn).set_Enabled(true);
					((Control)_searchBox).set_Enabled(true);
					_searchGate.Release();
				}
			});
		}

		private async Task<int?> AddResolvedHitAsync_ReturnMapId(NpcCandidateHit h, CancellationToken ct)
		{
			int? mapId = h.MapId;
			bool looksLikeContinentCoords = h.X > 30000 || h.Y > 30000;
			if (!mapId.HasValue && !string.IsNullOrWhiteSpace(h.MapName))
			{
				mapId = await _mapIndex.ResolveMapIdByNameAsync(h.MapName, ct);
			}
			Gw2MapInfo mapInfo = null;
			if (mapId.HasValue)
			{
				mapInfo = await _gw2.GetMapInfoAsync(mapId.Value, ct);
			}
			if (mapInfo == null && looksLikeContinentCoords)
			{
				mapInfo = await ResolveMapInfoByContinentPointAsync(h.X, h.Y, ct);
				if (mapInfo != null)
				{
					mapId = mapInfo.Id;
				}
			}
			await AddResolvedHitAsync(h, ct);
			Gw2MapInfo gw2MapInfo = mapInfo;
			return (gw2MapInfo != null) ? new int?(gw2MapInfo.Id) : mapId;
		}
	}
}
