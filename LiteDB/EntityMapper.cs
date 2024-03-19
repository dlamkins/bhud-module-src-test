using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LiteDB
{
	public class EntityMapper
	{
		public Type ForType { get; }

		public List<MemberMapper> Members { get; } = new List<MemberMapper>();


		public MemberMapper Id => Members.SingleOrDefault((MemberMapper x) => x.FieldName == "_id");

		public CreateObject CreateInstance { get; set; }

		public EntityMapper(Type forType)
		{
			ForType = forType;
		}

		public MemberMapper GetMember(Expression expr)
		{
			return Members.FirstOrDefault((MemberMapper x) => x.MemberName == expr.GetPath());
		}
	}
}
