using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Neokain.GW2.AllianceManager.Models;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Enums;
using Neokain.GW2.WebClient.Models.Spams;
using Neokain.GW2.WebClient.Models.Spams.SpamLines;

namespace Neokain.GW2.AllianceManager.Services.Spam
{
	public sealed class SpamOrchestrator : ISpamOrchestrator
	{
		private readonly SpamClient _spamClient;

		private readonly ISpamSender _sender;

		private readonly IGw2WebClient _webClient;

		private readonly int _messageDelay;

		private readonly Func<int?> _getCurrentMapId;

		private readonly Action<string> _notifyUser;

		private readonly SemaphoreSlim _inFlight = new SemaphoreSlim(1, 1);

		public bool IsSpamming { get; private set; }

		public event EventHandler SpamStateChanged;

		private void RaiseStateChanged()
		{
			this.SpamStateChanged?.Invoke(this, EventArgs.Empty);
		}

		private void Notify(string msg)
		{
			_notifyUser?.Invoke(msg);
		}

		public SpamOrchestrator(SpamClient spamClient, ISpamSender sender, IGw2WebClient webClient, int messageDelay, Func<int?> getCurrentMapId = null, Action<string> notifyUser = null)
		{
			_spamClient = spamClient ?? throw new ArgumentNullException("spamClient");
			_sender = sender ?? throw new ArgumentNullException("sender");
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
			_messageDelay = messageDelay;
			_getCurrentMapId = getCurrentMapId ?? ((Func<int?>)(() => null));
			_notifyUser = notifyUser;
		}

		public async Task<bool> UseAsync(SpamSource source, Guid spamId, SpamContext context, Func<string, Task<bool>> confirmOverride)
		{
			if (!_webClient.IsConnected || !_webClient.IsVerified)
			{
				Notify("Not connected — reconnect on the Connection tab.");
				return false;
			}
			if (!_inFlight.Wait(0))
			{
				Notify("A spam is already running.");
				return false;
			}
			try
			{
				IsSpamming = true;
				RaiseStateChanged();
				SpamDetailDto spam = await _spamClient.GetSpam(source, spamId);
				if (spam == null)
				{
					return false;
				}
				if (!SpamSender.HasSendableLines(spam, context))
				{
					Notify("This spam has no lines valid for the current context.");
					return false;
				}
				bool num = spam.SpamLines != null && spam.SpamLines.Any((SpamLineDto l) => l.Target == ChatType.Map);
				int? mapId = null;
				bool canUse;
				if (num)
				{
					mapId = _getCurrentMapId();
					canUse = await _spamClient.CanUseSpam(source, spamId, mapId);
				}
				else
				{
					canUse = SpamCooldown.CanUseNow(spam, DateTime.UtcNow);
				}
				if (!canUse)
				{
					string reason = SpamCooldown.DescribeRemaining(spam, DateTime.UtcNow);
					bool flag = confirmOverride == null;
					if (!flag)
					{
						flag = !(await confirmOverride(reason));
					}
					if (flag)
					{
						return false;
					}
				}
				await _spamClient.UseSpam(source, spamId, new SpamUsageRequestDto
				{
					MapId = mapId
				});
				await _sender.SendAsync(spam, context, _messageDelay, _webClient);
				return true;
			}
			finally
			{
				IsSpamming = false;
				RaiseStateChanged();
				_inFlight.Release();
			}
		}
	}
}
