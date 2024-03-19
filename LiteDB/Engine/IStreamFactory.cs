using System.IO;

namespace LiteDB.Engine
{
	internal interface IStreamFactory
	{
		string Name { get; }

		bool CloseOnDispose { get; }

		Stream GetStream(bool canWrite, bool sequencial);

		long GetLength();

		bool Exists();

		void Delete();

		bool IsLocked();
	}
}
