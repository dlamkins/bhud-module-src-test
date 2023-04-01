using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Extended;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Stream_Out.Core.Services
{
	internal class KillProofService : ExportService
	{
		private const string KILLPROOF_ME_UNSTABLE_FRACTAL_ESSENCE = "unstable_fractal_essence.txt";

		private const string KILLPROOF_ME_LEGENDARY_DIVINATION = "legendary_divination.txt";

		private const string KILLPROOF_ME_LEGENDARY_INSIGHT = "legendary_insight.txt";

		private const string KILLPROOF_API_URL = "https://killproof.me/api/kp/";

		private DirectoriesManager DirectoriesManager => StreamOutModule.Instance?.DirectoriesManager;

		private ContentsManager ContentsManager => StreamOutModule.Instance?.ContentsManager;

		public KillProofService(SettingCollection settings)
			: base(settings)
		{
		}

		public override async Task Initialize()
		{
			string moduleDir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			await ContentsManager.Extract("legendary_divination.png", Path.Combine(moduleDir + "\\static", "legendary_divination.png"));
			await ContentsManager.Extract("legendary_insight.png", Path.Combine(moduleDir + "\\static", "legendary_insight.png"));
			await ContentsManager.Extract("unstable_fractal_essence.png", Path.Combine(moduleDir + "\\static", "unstable_fractal_essence.png"));
		}

		protected override async Task Update()
		{
			await UpdateKillProofs();
		}

		private unsafe async Task UpdateKillProofs()
		{
			Account account = StreamOutModule.Instance?.Account;
			if (account == null || string.IsNullOrEmpty(account.get_Name()))
			{
				return;
			}
			await TaskUtil.GetJsonResponse<object>(string.Format("{0}{1}?lang={2}", "https://killproof.me/api/kp/", account.get_Name(), GameService.Overlay.get_UserLocale().get_Value())).ContinueWith((Func<Task<(bool, object)>, Task>)async delegate(Task<(bool, dynamic)> task)
			{
				if (!task.IsFaulted && ((ValueTuple<bool, object>)task.Result).Item1)
				{
					IEnumerable<object> killProofs = ((dynamic)((ValueTuple<bool, object>)task.Result).Item2).killproofs;
					if (!killProofs.IsNullOrEmpty())
					{
						string killproofDir = DirectoriesManager.GetFullDirectoryPath("stream_out") + "/killproof.me";
						if (!Directory.Exists(killproofDir))
						{
							Directory.CreateDirectory(killproofDir);
						}
						int count = 0;
						AsyncTaskMethodBuilder asyncTaskMethodBuilder = default(AsyncTaskMethodBuilder);
						foreach (object killProof in killProofs)
						{
							switch ((int)((dynamic)killProof).id)
							{
							case 88485:
							{
								object awaiter = FileUtil.WriteAllTextAsync(killproofDir + "/legendary_divination.txt", ((dynamic)killProof).amount.ToString()).GetAwaiter();
								if (!(bool)((dynamic)awaiter).IsCompleted)
								{
									ICriticalNotifyCompletion awaiter2 = awaiter as ICriticalNotifyCompletion;
									if (awaiter2 == null)
									{
										INotifyCompletion awaiter3 = (INotifyCompletion)awaiter;
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref *(_003C_003CUpdateKillProofs_003Eb__11_0_003Ed*)/*Error near IL_041c: stateMachine*/);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref *(_003C_003CUpdateKillProofs_003Eb__11_0_003Ed*)/*Error near IL_042f: stateMachine*/);
									}
									/*Error near IL_0438: leave MoveNext - await not detected correctly*/;
								}
								((dynamic)awaiter).GetResult();
								count++;
								break;
							}
							case 77302:
							{
								object awaiter = FileUtil.WriteAllTextAsync(killproofDir + "/legendary_insight.txt", ((dynamic)killProof).amount.ToString()).GetAwaiter();
								if (!(bool)((dynamic)awaiter).IsCompleted)
								{
									ICriticalNotifyCompletion awaiter2 = awaiter as ICriticalNotifyCompletion;
									if (awaiter2 == null)
									{
										INotifyCompletion awaiter3 = (INotifyCompletion)awaiter;
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref *(_003C_003CUpdateKillProofs_003Eb__11_0_003Ed*)/*Error near IL_06d7: stateMachine*/);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref *(_003C_003CUpdateKillProofs_003Eb__11_0_003Ed*)/*Error near IL_06ea: stateMachine*/);
									}
									/*Error near IL_06f3: leave MoveNext - await not detected correctly*/;
								}
								((dynamic)awaiter).GetResult();
								count++;
								break;
							}
							case 94020:
							{
								object awaiter = FileUtil.WriteAllTextAsync(killproofDir + "/unstable_fractal_essence.txt", ((dynamic)killProof).amount.ToString()).GetAwaiter();
								if (!(bool)((dynamic)awaiter).IsCompleted)
								{
									ICriticalNotifyCompletion awaiter2 = awaiter as ICriticalNotifyCompletion;
									if (awaiter2 == null)
									{
										INotifyCompletion awaiter3 = (INotifyCompletion)awaiter;
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref *(_003C_003CUpdateKillProofs_003Eb__11_0_003Ed*)/*Error near IL_0992: stateMachine*/);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref *(_003C_003CUpdateKillProofs_003Eb__11_0_003Ed*)/*Error near IL_09a5: stateMachine*/);
									}
									/*Error near IL_09ae: leave MoveNext - await not detected correctly*/;
								}
								((dynamic)awaiter).GetResult();
								count++;
								break;
							}
							}
							if (count == 3)
							{
								break;
							}
						}
					}
				}
			});
		}

		public override async Task Clear()
		{
			string dir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			await FileUtil.DeleteAsync(Path.Combine(dir, "unstable_fractal_essence.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "legendary_divination.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "legendary_insight.txt"));
			await FileUtil.DeleteDirectoryAsync(Path.Combine(Path.Combine(dir, "static")));
		}

		public override void Dispose()
		{
		}
	}
}
