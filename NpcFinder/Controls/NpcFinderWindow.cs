using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

		private static void MakeNonFocusable(object control)
		{
			if (control == null)
			{
				return;
			}
			try
			{
				Type t = control.GetType();
				BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				string[] array = new string[6] { "CanFocus", "CanReceiveFocus", "IsFocusable", "Focusable", "CanBeFocused", "CanTakeFocus" };
				foreach (string propName in array)
				{
					PropertyInfo p = t.GetProperty(propName, flags);
					if (p != null && p.CanWrite && p.PropertyType == typeof(bool))
					{
						p.SetValue(control, false, null);
						break;
					}
				}
				array = new string[3] { "CanFocus", "IsFocusable", "Focusable" };
				foreach (string fieldName in array)
				{
					FieldInfo f = t.GetField(fieldName, flags);
					if (f != null && f.FieldType == typeof(bool))
					{
						f.SetValue(control, false);
						break;
					}
				}
			}
			catch
			{
			}
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

		private async Task<List<(string label, string value)>> BuildMergedSuggestionsAsync(string text, CancellationToken ct)
		{
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
				List<(string, string)> merged = await BuildMergedSuggestionsAsync(q, ct);
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
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Expected O, but got Unknown
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Expected O, but got Unknown
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Expected O, but got Unknown
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Expected O, but got Unknown
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_0398: Expected O, but got Unknown
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Expected O, but got Unknown
			Rectangle cr = ((Container)this).get_ContentRegion();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(40, 40));
			((Control)val).set_Size(new Point(520, 400));
			((Control)val).set_ClipsBounds(false);
			_contentRoot = val;
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)_contentRoot);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Width(cr.Width - 30 - 110);
			((TextInputBase)val2).set_PlaceholderText("NPC name...");
			_searchBox = val2;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)_contentRoot);
			((Control)val3).set_Location(new Point(((Control)_searchBox).get_Location().X, ((Control)_searchBox).get_Location().Y + 34));
			((Control)val3).set_Size(new Point(((Control)_searchBox).get_Width(), 120));
			((Control)val3).set_ClipsBounds(true);
			((Panel)val3).set_CanScroll(true);
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_ControlPadding(new Vector2(0f, 2f));
			((Control)val3).set_Visible(false);
			_suggestPanel = val3;
			_suggestBtns = (StandardButton[])(object)new StandardButton[12];
			_suggestValues = new string[12];
			for (int i = 0; i < 12; i++)
			{
				int idx = i;
				StandardButton val4 = new StandardButton();
				((Control)val4).set_Parent((Container)(object)_suggestPanel);
				((Control)val4).set_Size(new Point(((Control)_suggestPanel).get_Width() - 18, 28));
				val4.set_Text("");
				((Control)val4).set_Visible(false);
				StandardButton b = val4;
				MakeNonFocusable(b);
				((Control)b).add_Click((EventHandler<MouseEventArgs>)async delegate
				{
					string val11 = _suggestValues[idx];
					if (!string.IsNullOrWhiteSpace(val11))
					{
						((TextInputBase)_searchBox).set_Text(val11);
						HideSuggestions();
						await DoSearchAsync();
					}
				});
				_suggestBtns[i] = b;
			}
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)_contentRoot);
			((Control)val5).set_Location(new Point(((Control)_searchBox).get_Right() + 10, ((Control)_searchBox).get_Top() - 2));
			((Control)val5).set_Size(new Point(110, 34));
			val5.set_Text("Search");
			_searchBtn = val5;
			((Control)_searchBtn).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await DoSearchAsync();
			});
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)_contentRoot);
			((Control)val6).set_Location(new Point(0, ((Control)_suggestPanel).get_Location().Y + ((Control)_suggestPanel).get_Height() + 8));
			val6.set_AutoSizeWidth(true);
			val6.set_Text("Awaiting NPC selection from the list below...");
			_status = val6;
			int bottomButtonsH = 34;
			int bottomY = cr.Height - 10 - bottomButtonsH;
			Panel val7 = new Panel();
			((Control)val7).set_Parent((Container)(object)_contentRoot);
			((Control)val7).set_Location(new Point(((Control)_searchBox).get_Left(), ((Control)_status).get_Bottom() + 8));
			((Control)val7).set_Size(new Point(cr.Width - 20, bottomY - (((Control)_status).get_Bottom() - 32)));
			((Control)val7).set_ClipsBounds(true);
			_resultsViewport = val7;
			FlowPanel val8 = new FlowPanel();
			((Control)val8).set_Parent((Container)(object)_resultsViewport);
			((Control)val8).set_Location(new Point(0, 0));
			((Control)val8).set_Size(((Control)_resultsViewport).get_Size());
			((Panel)val8).set_CanScroll(true);
			val8.set_FlowDirection((ControlFlowDirection)3);
			val8.set_ControlPadding(new Vector2(0f, 4f));
			_resultsPanel = val8;
			StandardButton val9 = new StandardButton();
			((Control)val9).set_Parent((Container)(object)_contentRoot);
			((Control)val9).set_Location(new Point(((Control)_searchBox).get_Left(), 355));
			((Control)val9).set_Size(new Point(220, 34));
			val9.set_Text("Remove marker / Stop search");
			_clearBtn = val9;
			((Control)_clearBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				StopSearch();
				_clearTarget?.Invoke();
				((Container)_resultsPanel).ClearChildren();
				_status.set_Text("Marker removed (and search stopped if it was running).");
			});
			StandardButton val10 = new StandardButton();
			((Control)val10).set_Parent((Container)(object)_contentRoot);
			((Control)val10).set_Location(new Point(cr.Width - 10 - 160, 355));
			((Control)val10).set_Size(new Point(160, 34));
			val10.set_Text("Delete cache");
			_clearCacheBtn = val10;
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
			MakeNonFocusable(_contentRoot);
			MakeNonFocusable(_resultsViewport);
			MakeNonFocusable(_resultsPanel);
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
					_status.set_Text("You are on " + ContinentNames.Name(cur) + "; target is on " + ContinentNames.Name(target.TargetContinentId) + ".");
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
			List<int> allMapIds = await _mapIndex.GetAllKnownMapIdsAsync(ct).ConfigureAwait(continueOnCapturedContext: false);
			if (allMapIds == null || allMapIds.Count == 0)
			{
				return null;
			}
			int curCont = ((_currentContinentIdProvider != null) ? _currentContinentIdProvider() : 0);
			if (curCont != 0)
			{
				for (int j = 0; j < allMapIds.Count; j++)
				{
					ct.ThrowIfCancellationRequested();
					if (j % 20 == 0)
					{
						await Task.Yield();
					}
					Gw2MapInfo mi3 = await _gw2.GetMapInfoAsync(allMapIds[j], ct).ConfigureAwait(continueOnCapturedContext: false);
					if (mi3 != null && mi3.ContinentId == curCont && Contains(mi3))
					{
						_continentPointMemo[key] = mi3;
						return mi3;
					}
				}
			}
			for (int j = 0; j < allMapIds.Count; j++)
			{
				ct.ThrowIfCancellationRequested();
				if (j % 20 == 0)
				{
					await Task.Yield();
				}
				Gw2MapInfo mi2 = await _gw2.GetMapInfoAsync(allMapIds[j], ct).ConfigureAwait(continueOnCapturedContext: false);
				if (mi2 != null && Contains(mi2))
				{
					_continentPointMemo[key] = mi2;
					return mi2;
				}
			}
			return null;
			bool Contains(Gw2MapInfo mi)
			{
				double minX = Math.Min(mi.ContinentRect.X1, mi.ContinentRect.X2);
				double maxX = Math.Max(mi.ContinentRect.X1, mi.ContinentRect.X2);
				double minY = Math.Min(mi.ContinentRect.Y1, mi.ContinentRect.Y2);
				double maxY = Math.Max(mi.ContinentRect.Y1, mi.ContinentRect.Y2);
				if ((double)cx >= minX && (double)cx <= maxX && (double)cy >= minY)
				{
					return (double)cy <= maxY;
				}
				return false;
			}
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
						_status.set_Text("You are on " + ContinentNames.Name(curCont) + ". Target is on " + ContinentNames.Name(npcTarget.TargetContinentId) + ". Teleport there, then open map.");
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

		private async Task DoSearchAsync()
		{
			HideSuggestions();
			CancelSuggest();
			string q = (((TextInputBase)_searchBox).get_Text() ?? "").Trim();
			if (q.Length == 0)
			{
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
			if (fp == null)
			{
				return;
			}
			try
			{
				((Control)fp).Invalidate();
				BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				string[] array = new string[4] { "RecalculateLayout", "ReflowChildren", "UpdateLayout", "InvalidateLayout" };
				foreach (string name in array)
				{
					MethodInfo i = ((object)fp).GetType().GetMethod(name, flags);
					if (i != null && i.GetParameters().Length == 0)
					{
						i.Invoke(fp, null);
						break;
					}
				}
			}
			catch
			{
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
