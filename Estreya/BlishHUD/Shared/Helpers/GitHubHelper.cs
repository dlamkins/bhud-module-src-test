using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Security;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Settings;
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
		private static readonly Logger Logger = Logger.GetLogger<GitHubHelper>();

		private readonly BaseModuleSettings _baseModuleSettings;

		private readonly string _clientId;

		private readonly GitHubClient _github;

		private readonly IconService _iconService;

		private readonly string _moduleName;

		private readonly string _owner;

		private readonly PasswordManager _passwordManager;

		private readonly string _repository;

		private readonly TranslationService _translationService;

		private GitHubCreateIssueView _issueView;

		private StandardWindow _window;

		public GitHubHelper(string owner, string repository, string clientId, string moduleName, PasswordManager passwordManager, IconService iconService, TranslationService translationService, BaseModuleSettings baseModuleSettings)
		{
			_owner = owner;
			_repository = repository;
			_clientId = clientId;
			_moduleName = moduleName;
			_passwordManager = passwordManager;
			_iconService = iconService;
			_translationService = translationService;
			_baseModuleSettings = baseModuleSettings;
			_github = new GitHubClient(new ProductHeaderValue(moduleName.Dehumanize()));
			CreateWindow();
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
			if (needNewToken)
			{
				OauthDeviceFlowRequest request = new OauthDeviceFlowRequest(_clientId);
				OauthDeviceFlowResponse deviceFlowResponse = await _github.Oauth.InitiateDeviceFlow(request);
				Process.Start(deviceFlowResponse.VerificationUri);
				if (await new ConfirmDialog("GitHub Login", "Enter the code \"" + deviceFlowResponse.UserCode + "\" in the opened GitHub browser window.", _iconService).ShowDialog() != DialogResult.OK)
				{
					throw new Exception("Login cancelled");
				}
				OauthToken token = await _github.Oauth.CreateAccessTokenForDeviceFlow(_clientId, deviceFlowResponse);
				if (_passwordManager != null)
				{
					await _passwordManager.Save("github", Encoding.UTF8.GetBytes(token.AccessToken), silent: true);
				}
				_github.Credentials = new Credentials(token.AccessToken);
			}
		}

		private void CreateWindow()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			StandardWindow window = _window;
			if (window != null)
			{
				((Control)window).Dispose();
			}
			StandardWindow standardWindow = new StandardWindow(_baseModuleSettings, _iconService.GetIcon("155985.png"), new Rectangle(40, 26, 913, 691), new Rectangle(70, 71, 839, 605));
			((Control)standardWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			standardWindow.Title = "Create Issue";
			standardWindow.Emblem = AsyncTexture2D.op_Implicit(_iconService.GetIcon("156022.png"));
			standardWindow.SavesPosition = true;
			standardWindow.Id = "GitHubHelper_ec5d3b09-b304-44c9-b70b-a4713ba8ffbf";
			_window = standardWindow;
		}

		public void OpenIssueWindow(string title = null, string message = null)
		{
			UnloadIssueView();
			_issueView = new GitHubCreateIssueView(_moduleName, _iconService, _translationService, title, message);
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
	}
}
