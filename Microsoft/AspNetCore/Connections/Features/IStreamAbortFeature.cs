namespace Microsoft.AspNetCore.Connections.Features
{
	internal interface IStreamAbortFeature
	{
		void AbortRead(long errorCode, ConnectionAbortedException abortReason);

		void AbortWrite(long errorCode, ConnectionAbortedException abortReason);
	}
}
