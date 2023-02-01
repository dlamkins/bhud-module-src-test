using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Security;
using Estreya.BlishHUD.Shared.State;
using Estreya.BlishHUD.Shared.Threading.Events;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Humanizer;
using Microsoft.Xna.Framework;
using Octokit;

namespace Estreya.BlishHUD.Shared.Helpers
{
	public class GitHubHelper : IDisposable
	{
		private static Logger Logger = Logger.GetLogger<GitHubHelper>();

		private readonly string _owner;

		private readonly string _repository;

		private readonly string _clientId;

		private readonly string _moduleName;

		private readonly PasswordManager _passwordManager;

		private readonly IconState _iconState;

		private readonly TranslationState _translationState;

		private StandardWindow _window;

		private readonly GitHubClient _github;

		private GitHubCreateIssueView _issueView;

		public GitHubHelper(string owner, string repository, string clientId, string moduleName, PasswordManager passwordManager, IconState iconState, TranslationState translationState)
		{
			_owner = owner;
			_repository = repository;
			_clientId = clientId;
			_moduleName = moduleName;
			_passwordManager = passwordManager;
			_iconState = iconState;
			_translationState = translationState;
			_github = new GitHubClient(new ProductHeaderValue(moduleName.Dehumanize()));
			CreateWindow();
		}

		private async Task Login()
		{
			Credentials credentials = _github.Credentials;
			bool needNewToken = credentials == null || credentials.AuthenticationType != AuthenticationType.Oauth || string.IsNullOrWhiteSpace(_github.Credentials?.Password);
			if (needNewToken && _passwordManager != null)
			{
				byte[] githubTokenData = await _passwordManager.Retrive("github", silent: true);
				if (githubTokenData != null)
				{
					string githubToken = Encoding.UTF8.GetString(githubTokenData);
					_github.Credentials = new Credentials(githubToken);
					needNewToken = false;
				}
			}
			if (!needNewToken)
			{
				try
				{
					await _github.User.Current();
				}
				catch (AuthorizationException)
				{
					needNewToken = true;
				}
			}
			if (!needNewToken)
			{
				return;
			}
			OauthDeviceFlowRequest request = new OauthDeviceFlowRequest(_clientId);
			OauthDeviceFlowResponse deviceFlowResponse = await _github.Oauth.InitiateDeviceFlow(request);
			ScreenNotification screenNotification = new ScreenNotification("GITHUB: Enter the code " + deviceFlowResponse.UserCode, ScreenNotification.NotificationType.Info, null, TimeSpan.FromMinutes(15.0).Seconds);
			((Control)screenNotification).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)screenNotification).set_Opacity(1f);
			((Control)screenNotification).set_Visible(true);
			ScreenNotification notification = screenNotification;
			Process.Start(deviceFlowResponse.VerificationUri);
			try
			{
				OauthToken token = await _github.Oauth.CreateAccessTokenForDeviceFlow(_clientId, deviceFlowResponse);
				if (_passwordManager != null)
				{
					await _passwordManager.Save("github", Encoding.UTF8.GetBytes(token.AccessToken), silent: true);
				}
				_github.Credentials = new Credentials(token.AccessToken);
			}
			finally
			{
				((Control)notification).Dispose();
			}
		}

		private void CreateWindow()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			StandardWindow window = _window;
			if (window != null)
			{
				((Control)window).Dispose();
			}
			StandardWindow val = new StandardWindow(_iconState.GetIcon("155985.png"), new Rectangle(40, 26, 913, 691), new Rectangle(70, 71, 839, 605));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Create Issue");
			((WindowBase2)val).set_Emblem(AsyncTexture2D.op_Implicit(_iconState.GetIcon("156022.png")));
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("GitHubHelper_ec5d3b09-b304-44c9-b70b-a4713ba8ffbf");
			_window = val;
		}

		public void OpenIssueWindow(string title = null, string message = null)
		{
			UnloadIssueView();
			_issueView = new GitHubCreateIssueView(_moduleName, _iconState, _translationState, GameService.Content.get_DefaultFont18(), title, message);
			_issueView.CreateClicked += new AsyncEventHandler<(string, string, string, bool)>(IssueView_CreateClicked);
			_issueView.CancelClicked += IssueView_CancelClicked;
			_window.Show((IView)(object)_issueView);
		}

		private async Task IssueView_CreateClicked(object sender, (string Title, string Message, string DiscordName, bool IncludeSystemInformation) e)
		{
			await CreateIssue(e.Title, e.Message, e.DiscordName, e.IncludeSystemInformation);
			((Control)_window).Hide();
		}

		private void IssueView_CancelClicked(object sender, EventArgs e)
		{
			((Control)_window).Hide();
		}

		private void UnloadIssueView()
		{
			if (_issueView != null)
			{
				_issueView.CreateClicked -= new AsyncEventHandler<(string, string, string, bool)>(IssueView_CreateClicked);
				_issueView.CancelClicked -= IssueView_CancelClicked;
				((View<IPresenter>)(object)_issueView).DoUnload();
			}
		}

		private async Task CreateIssue(string title, string message, string discordName = null, bool includeSystemInformation = false)
		{
			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentNullException("title", "Title is required.");
			}
			if (string.IsNullOrWhiteSpace(message))
			{
				throw new ArgumentNullException("message", "Message is required.");
			}
			if (!string.IsNullOrWhiteSpace(discordName) && !DiscordUtil.IsValidUsername(discordName))
			{
				throw new ArgumentException("The username \"" + discordName + "\" is not valid.");
			}
			string issueMessage = "**Message**:\n" + message + "\n\n**Discord**: " + (discordName ?? string.Empty) + "\n\n**This Issue was created automatically by the module " + _moduleName + "**";
			await Login();
			NewIssue newIssue = new NewIssue(title)
			{
				Body = issueMessage
			};
			newIssue.Labels.Add("Module: " + _moduleName);
			Issue issue = await _github.Issue.Create(_owner, _repository, newIssue);
			if (includeSystemInformation)
			{
				string dxDiagInformation = await WindowsUtil.GetDxDiagInformation();
				if (!string.IsNullOrWhiteSpace(dxDiagInformation))
				{
					await _github.Issue.Comment.Create(_owner, _repository, issue.Number, dxDiagInformation);
				}
				else
				{
					Logger.Warn("Could not fetch dx diag information.");
				}
			}
			Process.Start(issue.HtmlUrl);
		}

		public void Dispose()
		{
			StandardWindow window = _window;
			if (window != null)
			{
				((Control)window).Hide();
			}
			UnloadIssueView();
			StandardWindow window2 = _window;
			if (window2 != null)
			{
				((Control)window2).Dispose();
			}
		}
	}
}
