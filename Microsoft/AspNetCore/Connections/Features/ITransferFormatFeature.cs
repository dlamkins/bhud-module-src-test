namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface ITransferFormatFeature
	{
		TransferFormat SupportedFormats { get; }

		TransferFormat ActiveFormat { get; set; }
	}
}
