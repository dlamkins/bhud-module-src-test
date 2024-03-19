using System;
using System.Linq;
using System.Linq.Expressions;

namespace LiteDB
{
	public class EntityBuilder<T>
	{
		private readonly BsonMapper _mapper;

		private readonly EntityMapper _entity;

		private readonly ITypeNameBinder _typeNameBinder;

		internal EntityBuilder(BsonMapper mapper, ITypeNameBinder typeNameBinder)
		{
			_mapper = mapper;
			_typeNameBinder = typeNameBinder;
			_entity = mapper.GetEntityMapper(typeof(T));
		}

		public EntityBuilder<T> Ignore<K>(Expression<Func<T, K>> member)
		{
			return GetMember(member, delegate(MemberMapper p)
			{
				_entity.Members.Remove(p);
			});
		}

		public EntityBuilder<T> Field<K>(Expression<Func<T, K>> member, string field)
		{
			if (field.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("field");
			}
			return GetMember(member, delegate(MemberMapper p)
			{
				p.FieldName = field;
			});
		}

		public EntityBuilder<T> Id<K>(Expression<Func<T, K>> member, bool autoId = true)
		{
			return GetMember(member, delegate(MemberMapper p)
			{
				MemberMapper memberMapper = _entity.Members.FirstOrDefault((MemberMapper x) => x.FieldName == "_id");
				if (memberMapper != null)
				{
					memberMapper.FieldName = _mapper.ResolveFieldName(memberMapper.MemberName);
					memberMapper.AutoId = false;
				}
				p.FieldName = "_id";
				p.AutoId = autoId;
			});
		}

		public EntityBuilder<T> Ctor(Func<BsonDocument, T> createInstance)
		{
			_entity.CreateInstance = (BsonDocument v) => createInstance(v);
			return this;
		}

		public EntityBuilder<T> DbRef<K>(Expression<Func<T, K>> member, string collection = null)
		{
			return GetMember(member, delegate(MemberMapper p)
			{
				BsonMapper.RegisterDbRef(_mapper, p, _typeNameBinder, collection ?? _mapper.ResolveCollectionName(typeof(K)));
			});
		}

		private EntityBuilder<T> GetMember<TK, K>(Expression<Func<TK, K>> member, Action<MemberMapper> action)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			MemberMapper memb = _entity.GetMember(member);
			if (memb == null)
			{
				throw new ArgumentNullException("Member '" + member.GetPath() + "' not found in type '" + _entity.ForType.Name + "' (use IncludeFields in BsonMapper)");
			}
			action(memb);
			return this;
		}
	}
}
