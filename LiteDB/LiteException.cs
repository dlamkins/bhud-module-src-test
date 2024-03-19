using System;
using System.Reflection;
using System.Text;
using LiteDB.Engine;

namespace LiteDB
{
	public class LiteException : Exception
	{
		public const int FILE_NOT_FOUND = 101;

		public const int DATABASE_SHUTDOWN = 102;

		public const int INVALID_DATABASE = 103;

		public const int FILE_SIZE_EXCEEDED = 105;

		public const int COLLECTION_LIMIT_EXCEEDED = 106;

		public const int INDEX_DROP_ID = 108;

		public const int INDEX_DUPLICATE_KEY = 110;

		public const int INVALID_INDEX_KEY = 111;

		public const int INDEX_NOT_FOUND = 112;

		public const int INVALID_DBREF = 113;

		public const int LOCK_TIMEOUT = 120;

		public const int INVALID_COMMAND = 121;

		public const int ALREADY_EXISTS_COLLECTION_NAME = 122;

		public const int ALREADY_OPEN_DATAFILE = 124;

		public const int INVALID_TRANSACTION_STATE = 126;

		public const int INDEX_NAME_LIMIT_EXCEEDED = 128;

		public const int INVALID_INDEX_NAME = 129;

		public const int INVALID_COLLECTION_NAME = 130;

		public const int TEMP_ENGINE_ALREADY_DEFINED = 131;

		public const int INVALID_EXPRESSION_TYPE = 132;

		public const int COLLECTION_NOT_FOUND = 133;

		public const int COLLECTION_ALREADY_EXIST = 134;

		public const int INDEX_ALREADY_EXIST = 135;

		public const int INVALID_UPDATE_FIELD = 136;

		public const int INVALID_FORMAT = 200;

		public const int DOCUMENT_MAX_DEPTH = 201;

		public const int INVALID_CTOR = 202;

		public const int UNEXPECTED_TOKEN = 203;

		public const int INVALID_DATA_TYPE = 204;

		public const int PROPERTY_NOT_MAPPED = 206;

		public const int INVALID_TYPED_NAME = 207;

		public const int PROPERTY_READ_WRITE = 209;

		public const int INITIALSIZE_CRYPTO_NOT_SUPPORTED = 210;

		public const int INVALID_INITIALSIZE = 211;

		public const int INVALID_NULL_CHAR_STRING = 212;

		public const int INVALID_FREE_SPACE_PAGE = 213;

		public const int DATA_TYPE_NOT_ASSIGNABLE = 214;

		public const int AVOID_USE_OF_PROCESS = 215;

		public const int NOT_ENCRYPTED = 216;

		public const int INVALID_PASSWORD = 217;

		public int ErrorCode { get; private set; }

		public long Position { get; private set; }

		public LiteException(int code, string message)
			: base(message)
		{
			ErrorCode = code;
		}

		internal LiteException(int code, string message, params object[] args)
			: base(string.Format(message, args))
		{
			ErrorCode = code;
		}

		internal LiteException(int code, Exception inner, string message, params object[] args)
			: base(string.Format(message, args), inner)
		{
			ErrorCode = code;
		}

		internal static LiteException FileNotFound(object fileId)
		{
			return new LiteException(101, "File '{0}' not found.", fileId);
		}

		internal static LiteException DatabaseShutdown()
		{
			return new LiteException(102, "Database is in shutdown process.");
		}

		internal static LiteException InvalidDatabase()
		{
			return new LiteException(103, "File is not a valid LiteDB database format or contains a invalid password.");
		}

		internal static LiteException FileSizeExceeded(long limit)
		{
			return new LiteException(105, "Database size exceeds limit of {0}.", FileHelper.FormatFileSize(limit));
		}

		internal static LiteException CollectionLimitExceeded(int limit)
		{
			return new LiteException(106, "This database exceeded the maximum limit of collection names size: {0} bytes", limit);
		}

		internal static LiteException IndexNameLimitExceeded(int limit)
		{
			return new LiteException(128, "This collection exceeded the maximum limit of indexes names/expression size: {0} bytes", limit);
		}

		internal static LiteException InvalidIndexName(string name, string collection, string reason)
		{
			return new LiteException(129, "Invalid index name '{0}' on collection '{1}': {2}", name, collection, reason);
		}

		internal static LiteException InvalidCollectionName(string name, string reason)
		{
			return new LiteException(130, "Invalid collection name '{0}': {1}", name, reason);
		}

		internal static LiteException IndexDropId()
		{
			return new LiteException(108, "Primary key index '_id' can't be dropped.");
		}

		internal static LiteException TempEngineAlreadyDefined()
		{
			return new LiteException(131, "Temporary engine already defined or auto created.");
		}

		internal static LiteException CollectionNotFound(string key)
		{
			return new LiteException(133, "Collection not found: '{0}'", key);
		}

		internal static LiteException InvalidExpressionType(BsonExpression expr, BsonExpressionType type)
		{
			return new LiteException(132, "Expression '{0}' must be a {1} type.", expr.Source, type);
		}

		internal static LiteException InvalidExpressionTypePredicate(BsonExpression expr)
		{
			return new LiteException(132, "Expression '{0}' are not supported as predicate expression.", expr.Source);
		}

		internal static LiteException CollectionAlreadyExist(string key)
		{
			return new LiteException(134, "Collection already exist: '{0}'", key);
		}

		internal static LiteException IndexAlreadyExist(string name)
		{
			return new LiteException(135, "Index name '{0}' already exist with a differnt expression. Try drop index first.", name);
		}

		internal static LiteException InvalidUpdateField(string field)
		{
			return new LiteException(136, "'{0}' can't be modified in UPDATE command.", field);
		}

		internal static LiteException IndexDuplicateKey(string field, BsonValue key)
		{
			return new LiteException(110, "Cannot insert duplicate key in unique index '{0}'. The duplicate value is '{1}'.", field, key);
		}

		internal static LiteException InvalidIndexKey(string text)
		{
			return new LiteException(111, text);
		}

		internal static LiteException IndexNotFound(string name)
		{
			return new LiteException(112, "Index not found '{0}'.", name);
		}

		internal static LiteException LockTimeout(string mode, TimeSpan ts)
		{
			return new LiteException(120, "Database lock timeout when entering in {0} mode after {1}", mode, ts.ToString());
		}

		internal static LiteException LockTimeout(string mode, string collection, TimeSpan ts)
		{
			return new LiteException(120, "Collection '{0}' lock timeout when entering in {1} mode after {2}", collection, mode, ts.ToString());
		}

		internal static LiteException InvalidCommand(string command)
		{
			return new LiteException(121, "Command '{0}' is not a valid shell command.", command);
		}

		internal static LiteException AlreadyExistsCollectionName(string newName)
		{
			return new LiteException(122, "New collection name '{0}' already exists.", newName);
		}

		internal static LiteException AlreadyOpenDatafile(string filename)
		{
			return new LiteException(124, "Your datafile '{0}' is open in another process.", filename);
		}

		internal static LiteException InvalidDbRef(string path)
		{
			return new LiteException(113, "Invalid value for DbRef in path '{0}'. Value must be document like {{ $ref: \"?\", $id: ? }}", path);
		}

		internal static LiteException AlreadyExistsTransaction()
		{
			return new LiteException(126, "The current thread already contains an open transaction. Use the Commit/Rollback method to release the previous transaction.");
		}

		internal static LiteException CollectionLockerNotFound(string collection)
		{
			return new LiteException(126, "Collection locker '{0}' was not found inside dictionary.", collection);
		}

		internal static LiteException InvalidFormat(string field)
		{
			return new LiteException(200, "Invalid format: {0}", field);
		}

		internal static LiteException DocumentMaxDepth(int depth, Type type)
		{
			return new LiteException(201, "Document has more than {0} nested documents in '{1}'. Check for circular references (use DbRef).", depth, (type == null) ? "-" : type.Name);
		}

		internal static LiteException InvalidCtor(Type type, Exception inner)
		{
			return new LiteException(202, inner, "Failed to create instance for type '{0}' from assembly '{1}'. Checks if the class has a public constructor with no parameters.", type.FullName, type.AssemblyQualifiedName);
		}

		internal static LiteException UnexpectedToken(Token token, string expected = null)
		{
			long position = (token?.Position - (token?.Value?.Length).GetValueOrDefault()).GetValueOrDefault();
			string str = ((token != null && token.Type == TokenType.EOF) ? "[EOF]" : (token?.Value ?? ""));
			string exp = ((expected == null) ? "" : (" Expected `" + expected + "`."));
			return new LiteException(203, $"Unexpected token `{str}` in position {position}.{exp}")
			{
				Position = position
			};
		}

		internal static LiteException UnexpectedToken(string message, Token token)
		{
			long position = (token?.Position - (token?.Value?.Length).GetValueOrDefault()).GetValueOrDefault();
			return new LiteException(203, message)
			{
				Position = position
			};
		}

		internal static LiteException InvalidDataType(string field, BsonValue value)
		{
			return new LiteException(204, "Invalid BSON data type '{0}' on field '{1}'.", value.Type, field);
		}

		internal static LiteException PropertyReadWrite(PropertyInfo prop)
		{
			return new LiteException(209, "'{0}' property must have public getter and setter.", prop.Name);
		}

		internal static LiteException PropertyNotMapped(string name)
		{
			return new LiteException(206, "Property '{0}' was not mapped into BsonDocument.", name);
		}

		internal static LiteException InvalidTypedName(string type)
		{
			return new LiteException(207, "Type '{0}' not found in current domain (_type format is 'Type.FullName, AssemblyName').", type);
		}

		internal static LiteException InitialSizeCryptoNotSupported()
		{
			return new LiteException(210, "Initial Size option is not supported for encrypted datafiles.");
		}

		internal static LiteException InvalidInitialSize()
		{
			return new LiteException(211, "Initial Size must be a multiple of page size ({0} bytes).", 8192);
		}

		internal static LiteException InvalidNullCharInString()
		{
			return new LiteException(212, "Invalid null character (\\0) was found in the string");
		}

		internal static LiteException InvalidPageType(PageType pageType, BasePage page)
		{
			StringBuilder sb = new StringBuilder($"Invalid {pageType} on {page.PageID}. ");
			sb.Append($"Full zero: {page.Buffer.All(0)}. ");
			sb.Append($"Page Type: {page.PageType}. ");
			sb.Append($"Prev/Next: {page.PrevPageID}/{page.NextPageID}. ");
			sb.Append($"UniqueID: {page.Buffer.UniqueID}. ");
			sb.Append($"ShareCounter: {page.Buffer.ShareCounter}. ");
			return new LiteException(0, sb.ToString());
		}

		internal static LiteException InvalidFreeSpacePage(uint pageID, int freeBytes, int length)
		{
			return new LiteException(213, $"An operation that would corrupt page {pageID} was prevented. The operation required {length} free bytes, but the page had only {freeBytes} available.");
		}

		internal static LiteException DataTypeNotAssignable(string type1, string type2)
		{
			return new LiteException(214, "Data type " + type1 + " is not assignable from data type " + type2);
		}

		internal static LiteException FileNotEncrypted()
		{
			return new LiteException(216, "File is not encrypted.");
		}

		internal static LiteException InvalidPassword()
		{
			return new LiteException(217, "Invalid password.");
		}

		internal static LiteException AvoidUseOfProcess()
		{
			return new LiteException(215, "LiteDB do not accept System.Diagnostics.Process class in deserialize mapper");
		}
	}
}
