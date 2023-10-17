using System;
using System.Collections.Generic;
using System.Reflection;
using Blish_HUD;
using Blish_HUD.Gw2WebApi;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi.Middleware;
using Microsoft.Xna.Framework;

namespace BhModule.Community.ErrorSubmissionModule.WebHooks
{
	internal class WebApi
	{
		private static readonly Logger Logger = Logger.GetLogger<WebApi>();

		private const int HOOK_INTERVAL = 50;

		private FieldInfo _cachedAnonConnectionField;

		private FieldInfo _cachedPrivConnectionField;

		private FieldInfo _cachedApiManagersField;

		private FieldInfo _cachedManagedConnectionField;

		private bool _active;

		private ErrorSubmissionModule _errorSubmissionModule;

		private WebStatMiddleware _wsm;

		private double _lastCheck = -50.0;

		public WebApi(EtmConfig config, ErrorSubmissionModule errorSubmissionModule)
		{
			_errorSubmissionModule = errorSubmissionModule;
			_wsm = new WebStatMiddleware(config, errorSubmissionModule);
			try
			{
				_cachedAnonConnectionField = typeof(Gw2WebApiService).GetField("_anonymousConnection", BindingFlags.Instance | BindingFlags.NonPublic);
				_cachedPrivConnectionField = typeof(Gw2WebApiService).GetField("_privilegedConnection", BindingFlags.Instance | BindingFlags.NonPublic);
				_cachedApiManagersField = typeof(Gw2ApiManager).GetField("_apiManagers", BindingFlags.Static | BindingFlags.NonPublic);
				_cachedManagedConnectionField = typeof(Gw2ApiManager).GetField("_connection", BindingFlags.Instance | BindingFlags.NonPublic);
				HookBaseConnections();
				_active = true;
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to get field information for Web API hook.");
			}
		}

		private IEnumerable<ManagedConnection> GetBaseConnections()
		{
			object value = _cachedAnonConnectionField.GetValue(GameService.Gw2WebApi);
			ManagedConnection anonymousConnection = (ManagedConnection)((value is ManagedConnection) ? value : null);
			object value2 = _cachedPrivConnectionField.GetValue(GameService.Gw2WebApi);
			ManagedConnection privilegedConnection = (ManagedConnection)((value2 is ManagedConnection) ? value2 : null);
			yield return anonymousConnection;
			yield return privilegedConnection;
		}

		private void HookBaseConnections()
		{
			foreach (ManagedConnection mc in GetBaseConnections())
			{
				HookManagedConnection(mc);
			}
		}

		private void HookManagedConnection(ManagedConnection connection)
		{
			if (!connection.get_Connection().get_Middleware().Contains((IWebApiMiddleware)(object)_wsm))
			{
				connection.get_Connection().get_Middleware().Add((IWebApiMiddleware)(object)_wsm);
			}
		}

		private void UnhookManagedConnection(ManagedConnection connection)
		{
			connection.get_Connection().get_Middleware().Remove((IWebApiMiddleware)(object)_wsm);
		}

		private void HookAllModules()
		{
			try
			{
				List<Gw2ApiManager> allModuleApiManagers = _cachedApiManagersField.GetValue(null) as List<Gw2ApiManager>;
				if (allModuleApiManagers == null)
				{
					return;
				}
				foreach (Gw2ApiManager apiManager in allModuleApiManagers)
				{
					object value = _cachedManagedConnectionField.GetValue(apiManager);
					ManagedConnection connection = (ManagedConnection)((value is ManagedConnection) ? value : null);
					HookManagedConnection(connection);
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to hook a module's API manager.");
				_active = false;
			}
		}

		public void Update(GameTime gameTime)
		{
			if (_active && gameTime.get_TotalGameTime().TotalMilliseconds - _lastCheck > 50.0)
			{
				_lastCheck = gameTime.get_TotalGameTime().TotalMilliseconds;
				HookAllModules();
			}
		}

		public void UnloadHooks()
		{
			try
			{
				List<Gw2ApiManager> allModuleApiManagers = _cachedApiManagersField.GetValue(null) as List<Gw2ApiManager>;
				if (allModuleApiManagers != null)
				{
					foreach (Gw2ApiManager apiManager in allModuleApiManagers)
					{
						object value = _cachedManagedConnectionField.GetValue(apiManager);
						ManagedConnection connection = (ManagedConnection)((value is ManagedConnection) ? value : null);
						UnhookManagedConnection(connection);
					}
				}
				foreach (ManagedConnection mc in GetBaseConnections())
				{
					UnhookManagedConnection(mc);
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e, "Failed to unload all API middlewares.");
			}
			_active = false;
		}
	}
}
