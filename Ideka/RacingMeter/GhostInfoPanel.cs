using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class GhostInfoPanel : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<GhostInfoPanel>();

		private const int Spacing = 10;

		private FullRace? _fullRace;

		private FullGhost? _fullGhost;

		private readonly FlowPanel _panel;

		private readonly Label _ghostTimeLabel;

		private readonly Label _ghostRacerLabel;

		private readonly Label _ghostUploadedLabel;

		private readonly Label _ghostLoadingLabel;

		private readonly StandardButton _uploadGhostButton;

		private readonly StandardButton _raceGhostButton;

		private CancellationTokenSource? _downloadGhost;

		private CancellationTokenSource? _uploadGhost;

		public FullRace? FullRace
		{
			get
			{
				return _fullRace;
			}
			set
			{
				_fullRace = value;
				if (FullGhost?.Ghost.RaceId != _fullRace?.Meta.Id)
				{
					FullGhost = null;
				}
			}
		}

		public FullGhost? FullGhost
		{
			get
			{
				return _fullGhost;
			}
			set
			{
				_fullGhost = value;
				((Control)_raceGhostButton).set_Enabled(FullGhost?.Ghost != null);
				StandardButton uploadGhostButton = _uploadGhostButton;
				FullRace? fullRace = FullRace;
				((Control)uploadGhostButton).set_Enabled(fullRace != null && !fullRace!.IsLocal && (FullGhost?.IsLocal ?? false) && !TaskUtils.IsRunning(_uploadGhost));
				_ghostLoadingLabel.set_Text((string)null);
				string time = ((FullGhost?.IsLocal ?? false) ? FullGhost!.Ghost.Time.Formatted() : FullGhost?.Meta.Time.Formatted());
				_ghostTimeLabel.set_Text(StringExtensions.Format(Strings.LabelGhostTime, time ?? ""));
				string racer = ((FullGhost?.IsLocal ?? false) ? Strings.GhostLocal : FullGhost?.Meta.AccountName);
				_ghostRacerLabel.set_Text(StringExtensions.Format(Strings.LabelGhostRacer, racer ?? ""));
				((Control)_ghostRacerLabel).set_BasicTooltipText((FullGhost?.IsLocal ?? false) ? Strings.GhostLocalRacerTooltip : racer);
				string uploadedRelative = ((FullGhost?.IsLocal ?? false) ? Strings.GhostLocal : ((FullGhost == null) ? null : FullGhost!.Meta.Upload.ToRelativeDateUtc()));
				string uploaded = ((FullGhost?.IsLocal ?? false) ? Strings.GhostLocal : $"{FullGhost?.Meta.Upload.ToLocalTime()}");
				_ghostUploadedLabel.set_Text(StringExtensions.Format(Strings.LabelGhostUploaded, uploadedRelative ?? ""));
				((Control)_ghostUploadedLabel).set_BasicTooltipText((FullGhost?.IsLocal ?? false) ? Strings.GhostLocalUploadedTooltip : uploaded);
				if (FullGhost != null && !FullGhost!.IsLocal && FullGhost!.Ghost == null)
				{
					DownloadGhost();
				}
			}
		}

		public event Action<FullGhost>? GhostChanged;

		public event Action<FullRace, FullGhost>? GhostRequested;

		public GhostInfoPanel()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Expected O, but got Unknown
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Expected O, but got Unknown
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(Vector2.get_UnitY() * 10f / 2f);
			val.set_OuterControlPadding(Vector2.get_One() * 10f / 2f);
			((Panel)val).set_Title(Strings.Ghost);
			((Panel)val).set_ShowTint(true);
			_panel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_panel);
			val2.set_Font(Control.get_Content().get_DefaultFont32());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			_ghostTimeLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_panel);
			val3.set_Font(Control.get_Content().get_DefaultFont16());
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			_ghostRacerLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)_panel);
			val4.set_Font(Control.get_Content().get_DefaultFont16());
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			_ghostUploadedLabel = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)_panel);
			val5.set_Font(Control.get_Content().get_DefaultFont16());
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			_ghostLoadingLabel = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.UploadGhost);
			_uploadGhostButton = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text(Strings.LoadGhost);
			_raceGhostButton = val7;
			UpdateLayout();
			((Control)_uploadGhostButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				string text = FullRace?.Meta.Id;
				if (text != null)
				{
					FullGhost fullGhost2 = FullGhost;
					if (fullGhost2 != null && fullGhost2.IsLocal)
					{
						UploadGhost(text, fullGhost2.Ghost);
					}
				}
			});
			((Control)_raceGhostButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				FullRace fullRace = FullRace;
				if (fullRace != null)
				{
					FullGhost fullGhost = FullGhost;
					if (fullGhost != null && fullRace.Meta.Id == fullGhost.Ghost.RaceId)
					{
						this.GhostRequested?.Invoke(fullRace, fullGhost);
					}
				}
			});
			FullGhost = null;
		}

		private void DownloadGhost()
		{
			TaskUtils.Cancel(ref _downloadGhost);
			FullGhost target = FullGhost;
			if (target == null)
			{
				return;
			}
			string id = target.Meta?.Id;
			if (id == null)
			{
				return;
			}
			CancellationToken ct = TaskUtils.New(out _downloadGhost);
			((Func<Task>)async delegate
			{
				_ghostLoadingLabel.set_Text(Strings.Loading);
				FullGhost fullGhost = target;
				fullGhost.Ghost = await RacingModule.Server.GetGhost(id, ct);
				if (FullGhost == target)
				{
					this.GhostChanged?.Invoke(target);
				}
			})().Done(Logger, Strings.ErrorGhostLoad, _downloadGhost).ContinueWith(delegate(Task<TaskUtils.TaskState> task)
			{
				if (FullGhost == target)
				{
					_ghostLoadingLabel.set_Text(task.Result.Canceled ? Strings.Loading : (task.Result.Faulted ? Strings.ErrorGhostLoad : null));
				}
			});
		}

		private void UploadGhost(string raceId, Ghost ghost)
		{
			string raceId2 = raceId;
			Ghost ghost2 = ghost;
			TaskUtils.Cancel(ref _uploadGhost);
			((Control)_uploadGhostButton).set_Enabled(false);
			CancellationToken ct = TaskUtils.New(out _uploadGhost);
			((Func<Task>)async delegate
			{
				UserData user = RacingModule.Server.User;
				if (user == null)
				{
					ScreenNotification.ShowNotification(Strings.NotifyLogInRequired, (NotificationType)2, (Texture2D)null, 4);
				}
				else
				{
					if (RacingModule.Server.Leaderboards.Boards.TryGetValue(raceId2, out var board))
					{
						MetaGhost current = board.Places.Values.FirstOrDefault((MetaGhost p) => p.UserId == user.AccountId);
						if (current != null && current.Time <= ghost2.Time)
						{
							ScreenNotification.ShowNotification(Strings.NotifyGhostAlreadyBetter, (NotificationType)2, (Texture2D)null, 4);
							return;
						}
					}
					await RacingModule.Server.UploadGhost(raceId2, ghost2, ct);
				}
			})().Done(Logger, Strings.ErrorGhostUpload, _uploadGhost).ContinueWith(delegate
			{
				((Control)_uploadGhostButton).set_Enabled(FullGhost?.IsLocal ?? false);
			});
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			if (_panel != null)
			{
				((Control)_raceGhostButton).set_Bottom(((Container)this).get_ContentRegion().Height);
				((Control)_raceGhostButton).set_Right(((Control)_panel).get_Left() + ((Control)_panel).get_Width() / 2);
				((Control)_uploadGhostButton).set_Bottom(((Container)this).get_ContentRegion().Height);
				((Control)_uploadGhostButton).set_Left(((Control)_panel).get_Left() + ((Control)_panel).get_Width() / 2);
				((Control)_panel).set_Location(Point.get_Zero());
				((Control)_panel).set_Width(((Container)this).get_ContentRegion().Width);
				((Control)_panel).set_Height(((Container)this).get_ContentRegion().Height - ((Control)_raceGhostButton).get_Height() - 10);
			}
		}
	}
}
