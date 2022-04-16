using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;

namespace Nekres.Stream_Out.Core.Services
{
	internal class KillProofService : IExportService, IDisposable
	{
		private const string KILLPROOF_ME_UNSTABLE_FRACTAL_ESSENCE = "unstable_fractal_essence.txt";

		private const string KILLPROOF_ME_LEGENDARY_DIVINATION = "legendary_divination.txt";

		private const string KILLPROOF_ME_LEGENDARY_INSIGHT = "legendary_insight.txt";

		private const string KILLPROOF_API_URL = "https://killproof.me/api/kp/";

		private Logger Logger => StreamOutModule.Logger;

		private DirectoriesManager DirectoriesManager => StreamOutModule.ModuleInstance?.DirectoriesManager;

		private ContentsManager ContentsManager => StreamOutModule.ModuleInstance?.ContentsManager;

		private string AccountName => StreamOutModule.ModuleInstance?.AccountName.get_Value();

		public async Task Initialize()
		{
			string moduleDir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			ContentsManager.ExtractIcons("legendary_divination.png", Path.Combine(moduleDir + "\\static", "legendary_divination.png"));
			ContentsManager.ExtractIcons("legendary_insight.png", Path.Combine(moduleDir + "\\static", "legendary_insight.png"));
			ContentsManager.ExtractIcons("unstable_fractal_essence.png", Path.Combine(moduleDir + "\\static", "unstable_fractal_essence.png"));
		}

		public Task ResetDaily()
		{
			return Task.CompletedTask;
		}

		public async Task Update()
		{
			await UpdateKillProofs();
		}

		private unsafe async Task UpdateKillProofs()
		{
			await TaskUtil.GetJsonResponse<object>(string.Format("{0}{1}?lang={2}", "https://killproof.me/api/kp/", AccountName, GameService.Overlay.get_UserLocale().get_Value())).ContinueWith((Func<Task<(bool, object)>, Task>)async delegate(Task<(bool, dynamic)> task)
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
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref *(_003C_003CUpdateKillProofs_003Eb__16_0_003Ed*)/*Error near IL_041c: stateMachine*/);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref *(_003C_003CUpdateKillProofs_003Eb__16_0_003Ed*)/*Error near IL_042f: stateMachine*/);
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
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref *(_003C_003CUpdateKillProofs_003Eb__16_0_003Ed*)/*Error near IL_06d7: stateMachine*/);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref *(_003C_003CUpdateKillProofs_003Eb__16_0_003Ed*)/*Error near IL_06ea: stateMachine*/);
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
										asyncTaskMethodBuilder.AwaitOnCompleted(ref awaiter3, ref *(_003C_003CUpdateKillProofs_003Eb__16_0_003Ed*)/*Error near IL_0992: stateMachine*/);
									}
									else
									{
										asyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter2, ref *(_003C_003CUpdateKillProofs_003Eb__16_0_003Ed*)/*Error near IL_09a5: stateMachine*/);
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

		public void Dispose()
		{
		}
	}
}
