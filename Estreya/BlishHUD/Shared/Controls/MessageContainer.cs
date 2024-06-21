using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Settings;
using Estreya.BlishHUD.Shared.UI.Views;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class MessageContainer : IDisposable
	{
		public enum MessageType
		{
			Info,
			Warning,
			Error,
			Fatal,
			Debug
		}

		private const string WINDOW_TITLE = "Estreya Messages";

		private readonly Gw2ApiManager _apiManager;

		private readonly TranslationService _translationService;

		private readonly IconService _iconService;

		private Window _window;

		private ContainerView _containerView;

		private FlowPanel _messagePanel;

		private ConcurrentBag<Guid> _openedGuids;

		private AsyncLock _lock = new AsyncLock();

		public MessageContainer(Gw2ApiManager apiManager, BaseModuleSettings settings, TranslationService translationService, IconService iconService, string title = "Estreya Messages")
		{
			_window = WindowUtil.CreateStandardWindow(settings, title, GetType(), Guid.Parse("89dc6f18-9c3b-4e1b-b16f-7db71682129a"), iconService);
			((Control)_window).set_Width(500);
			((Control)_window).set_Height(500);
			((Control)_window).Hide();
			_apiManager = apiManager;
			_translationService = translationService;
			_iconService = iconService;
			_openedGuids = new ConcurrentBag<Guid>();
		}

		private async Task CreateContainerView()
		{
			using (await _lock.LockAsync())
			{
				if (_containerView == null)
				{
					_containerView = new ContainerView(_apiManager, _iconService, _translationService);
					await _window.SetView((IView)(object)_containerView);
					MessageContainer messageContainer = this;
					FlowPanel val = new FlowPanel();
					val.set_FlowDirection((ControlFlowDirection)3);
					((Container)val).set_WidthSizingMode((SizingMode)2);
					((Container)val).set_HeightSizingMode((SizingMode)2);
					((Panel)val).set_CanScroll(true);
					val.set_ControlPadding(new Vector2(0f, 10f));
					messageContainer._messagePanel = val;
					_containerView.Add((Control)(object)_messagePanel);
					((Control)_messagePanel).RecalculateLayout();
					((Control)_messagePanel).Update(GameService.Overlay.get_CurrentGameTime());
				}
			}
		}

		private string GetTimestamp()
		{
			return DateTimeOffset.Now.ToString("T");
		}

		private Color GetMessageTypeColor(MessageType type)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			return (Color)(type switch
			{
				MessageType.Info => Color.get_Green(), 
				MessageType.Warning => Color.get_Yellow(), 
				MessageType.Error => Color.get_Red(), 
				MessageType.Fatal => Color.get_MediumVioletRed(), 
				MessageType.Debug => Color.get_LightBlue(), 
				_ => Color.get_White(), 
			});
		}

		private bool ShouldShowItself(MessageType type)
		{
			if ((uint)(type - 2) <= 1u)
			{
				return true;
			}
			return false;
		}

		public Task Add(Module module, MessageType messageType, string message)
		{
			return Add(module, Guid.NewGuid(), messageType, message);
		}

		public async Task Add(Module module, Guid identification, MessageType messageType, string message)
		{
			await CreateContainerView();
			using (await _lock.LockAsync())
			{
				Panel val = new Panel();
				((Control)val).set_Width(((Container)_messagePanel).get_ContentRegion().Width);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				((Control)val).set_Parent((Container)(object)_messagePanel);
				val.set_ShowBorder(true);
				Panel panel = val;
				Color messageTypeColor = GetMessageTypeColor(messageType);
				string timestamp = GetTimestamp();
				((Control)new FormattedLabelBuilder().SetWidth(((Container)panel).get_ContentRegion().Width).AutoSizeHeight().Wrap()
					.CreatePart("[", (Action<FormattedLabelPartBuilder>)delegate
					{
					})
					.CreatePart(timestamp ?? "", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
					{
						//IL_0001: Unknown result type (might be due to invalid IL or missing references)
						b.SetTextColor(Color.get_Gray());
					})
					.CreatePart(" - ", (Action<FormattedLabelPartBuilder>)delegate
					{
					})
					.CreatePart(module.get_Name() ?? "", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
					{
						//IL_0001: Unknown result type (might be due to invalid IL or missing references)
						b.SetTextColor(Color.get_DarkGray());
					})
					.CreatePart(" - ", (Action<FormattedLabelPartBuilder>)delegate
					{
					})
					.CreatePart($"{messageType}", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder b)
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						b.SetTextColor(messageTypeColor);
					})
					.CreatePart("] ", (Action<FormattedLabelPartBuilder>)delegate
					{
					})
					.CreatePart(message ?? "", (Action<FormattedLabelPartBuilder>)delegate
					{
					})
					.Build()).set_Parent((Container)(object)panel);
				if (!(_openedGuids?.ToArray().Contains(identification) ?? false) && !((Control)_window).get_Visible() && ShouldShowItself(messageType))
				{
					_openedGuids?.Add(identification);
					((Control)_window).Show();
				}
			}
		}

		public void Show()
		{
			Window window = _window;
			if (window != null)
			{
				((Control)window).Show();
			}
		}

		public void Dispose()
		{
			using (_lock.Lock())
			{
				Window window = _window;
				if (window != null)
				{
					((Control)window).Dispose();
				}
				((View<IPresenter>)(object)_containerView)?.DoUnload();
				FlowPanel messagePanel = _messagePanel;
				if (messagePanel != null)
				{
					((Control)messagePanel).Dispose();
				}
				_window = null;
				_containerView = null;
				_messagePanel = null;
				_openedGuids = null;
			}
		}
	}
}
