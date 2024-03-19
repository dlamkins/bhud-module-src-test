using System.Text;

namespace LiteDB.Engine
{
	internal class CollectionIndex
	{
		public uint FreeIndexPageList;

		public byte Slot { get; }

		public byte IndexType { get; }

		public string Name { get; }

		public string Expression { get; }

		public BsonExpression BsonExpr { get; }

		public bool Unique { get; }

		public PageAddress Head { get; set; }

		public PageAddress Tail { get; set; }

		public byte MaxLevel { get; set; } = 1;


		public bool IsEmpty => string.IsNullOrEmpty(Name);

		public CollectionIndex(byte slot, byte indexType, string name, string expr, bool unique)
		{
			Slot = slot;
			IndexType = indexType;
			Name = name;
			Expression = expr;
			Unique = unique;
			FreeIndexPageList = uint.MaxValue;
			BsonExpr = BsonExpression.Create(expr);
		}

		public CollectionIndex(BufferReader reader)
		{
			Slot = reader.ReadByte();
			IndexType = reader.ReadByte();
			Name = reader.ReadCString();
			Expression = reader.ReadCString();
			Unique = reader.ReadBoolean();
			Head = reader.ReadPageAddress();
			Tail = reader.ReadPageAddress();
			MaxLevel = reader.ReadByte();
			FreeIndexPageList = reader.ReadUInt32();
			BsonExpr = BsonExpression.Create(Expression);
		}

		public void UpdateBuffer(BufferWriter writer)
		{
			writer.Write(Slot);
			writer.Write(IndexType);
			writer.WriteCString(Name);
			writer.WriteCString(Expression);
			writer.Write(Unique);
			writer.Write(Head);
			writer.Write(Tail);
			writer.Write(MaxLevel);
			writer.Write(FreeIndexPageList);
		}

		public static int GetLength(CollectionIndex index)
		{
			return GetLength(index.Name, index.Expression);
		}

		public static int GetLength(string name, string expr)
		{
			return 2 + Encoding.UTF8.GetByteCount(name) + 1 + Encoding.UTF8.GetByteCount(expr) + 1 + 1 + 5 + 5 + 1 + 4;
		}
	}
}
