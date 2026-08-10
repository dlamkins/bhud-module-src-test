namespace Microsoft.AspNetCore.SignalR.Client
{
	internal interface IHubConnectionBuilder : ISignalRBuilder
	{
		HubConnection Build();
	}
}
