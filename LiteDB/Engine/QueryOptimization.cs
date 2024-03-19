using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDB.Engine
{
	internal class QueryOptimization
	{
		private readonly Snapshot _snapshot;

		private readonly Query _query;

		private readonly Collation _collation;

		private readonly QueryPlan _queryPlan;

		private readonly List<BsonExpression> _terms = new List<BsonExpression>();

		public QueryOptimization(Snapshot snapshot, Query query, IEnumerable<BsonDocument> source, Collation collation)
		{
			if (query.Select == null)
			{
				throw new ArgumentNullException("Select");
			}
			_snapshot = snapshot;
			_query = query;
			_collation = collation;
			_queryPlan = new QueryPlan(snapshot.CollectionName)
			{
				Index = ((source != null) ? new IndexVirtual(source) : null),
				Select = new Select(_query.Select, _query.Select.UseSource),
				ForUpdate = query.ForUpdate,
				Limit = query.Limit,
				Offset = query.Offset
			};
		}

		public QueryPlan ProcessQuery()
		{
			SplitWherePredicateInTerms();
			OptimizeTerms();
			DefineQueryFields();
			DefineIndex();
			DefineOrderBy();
			DefineGroupBy();
			DefineIncludes();
			return _queryPlan;
		}

		private void SplitWherePredicateInTerms()
		{
			foreach (BsonExpression predicate2 in _query.Where)
			{
				add(predicate2);
			}
			void add(BsonExpression predicate)
			{
				if (predicate.UseSource)
				{
					throw new LiteException(0, "WHERE filter can not use `*` expression in `" + predicate.Source);
				}
				if (predicate.IsPredicate || predicate.Type == BsonExpressionType.Or)
				{
					_terms.Add(predicate);
				}
				else
				{
					if (predicate.Type != BsonExpressionType.And)
					{
						throw LiteException.InvalidExpressionTypePredicate(predicate);
					}
					BsonExpression left = predicate.Left;
					BsonExpression right = predicate.Right;
					add(left);
					add(right);
				}
			}
		}

		private void OptimizeTerms()
		{
			for (int i = 0; i < _terms.Count; i++)
			{
				BsonExpression term = _terms[i];
				BsonExpression left = term.Left;
				if (left != null && !left.IsScalar && term.IsANY && term.Type == BsonExpressionType.Equal)
				{
					BsonExpression right = term.Right;
					if (right != null && right.Type == BsonExpressionType.Path)
					{
						_terms[i] = BsonExpression.Create(term.Right.Source + " IN ARRAY(" + term.Left.Source + ")", term.Parameters);
					}
				}
			}
		}

		private void DefineQueryFields()
		{
			HashSet<string> fields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			fields.AddRange(_query.Select.Fields);
			fields.AddRange(_terms.SelectMany((BsonExpression x) => x.Fields));
			fields.AddRange(_query.Includes.SelectMany((BsonExpression x) => x.Fields));
			fields.AddRange(_query.GroupBy?.Fields);
			fields.AddRange(_query.Having?.Fields);
			fields.AddRange(_query.OrderBy?.Fields);
			if (fields.Contains("$"))
			{
				fields.Clear();
			}
			_queryPlan.Fields = fields;
		}

		private void DefineIndex()
		{
			BsonExpression selected = null;
			if (_queryPlan.Index == null)
			{
				IndexCost indexCost = ChooseIndex(_queryPlan.Fields);
				if (indexCost != null)
				{
					_queryPlan.Index = indexCost.Index;
					_queryPlan.IndexCost = indexCost.Cost;
					_queryPlan.IndexExpression = indexCost.IndexExpression;
				}
				else
				{
					CollectionIndex pk = _snapshot.CollectionPage.PK;
					_queryPlan.Index = new IndexAll("_id", 1);
					_queryPlan.IndexCost = _queryPlan.Index.GetCost(pk);
					_queryPlan.IndexExpression = "$._id";
				}
				selected = indexCost?.Expression;
			}
			else
			{
				Constants.ENSURE(_queryPlan.Index is IndexVirtual, "pre-defined index must be only for virtual collections");
				_queryPlan.IndexCost = 0u;
			}
			if (_queryPlan.Fields.Count == 1 && _queryPlan.IndexExpression == "$." + _queryPlan.Fields.First())
			{
				_queryPlan.IsIndexKeyOnly = true;
			}
			_queryPlan.Filters.AddRange(_terms.Where((BsonExpression x) => x != selected));
		}

		private IndexCost ChooseIndex(HashSet<string> fields)
		{
			CollectionIndex[] indexes = _snapshot.CollectionPage.GetCollectionIndexes().ToArray();
			string preferred = ((fields.Count == 1) ? ("$." + fields.First()) : null);
			IndexCost lowest = null;
			foreach (BsonExpression expr in _terms.Where((BsonExpression x) => x.IsPredicate))
			{
				Constants.ENSURE(expr.Left != null && expr.Right != null, "predicate expression must has left/right expressions");
				Tuple<CollectionIndex, BsonExpression> index2 = null;
				if (!expr.Left.IsScalar && expr.Right.IsScalar)
				{
					if (expr.IsANY)
					{
						index2 = (from x in indexes
							where x.Expression == expr.Left.Source && expr.Right.IsValue
							select Tuple.Create(x, expr.Right)).FirstOrDefault();
					}
				}
				else
				{
					index2 = (from x in indexes
						where x.Expression == expr.Left.Source && expr.Right.IsValue
						select Tuple.Create(x, expr.Right)).Union(from x in indexes
						where x.Expression == expr.Right.Source && expr.Left.IsValue
						select Tuple.Create(x, expr.Left)).FirstOrDefault();
				}
				if (index2 != null)
				{
					IndexCost current = new IndexCost(index2.Item1, expr, index2.Item2, _collation);
					if (lowest == null || current.Cost < lowest.Cost)
					{
						lowest = current;
					}
				}
			}
			if (lowest == null && (_query.OrderBy != null || _query.GroupBy != null || preferred != null))
			{
				CollectionIndex index = indexes.FirstOrDefault((CollectionIndex x) => x.Expression == _query.GroupBy?.Source) ?? indexes.FirstOrDefault((CollectionIndex x) => x.Expression == _query.OrderBy?.Source) ?? indexes.FirstOrDefault((CollectionIndex x) => x.Expression == preferred);
				if (index != null)
				{
					lowest = new IndexCost(index);
				}
			}
			return lowest;
		}

		private void DefineOrderBy()
		{
			if (_query.OrderBy != null)
			{
				OrderBy orderBy = new OrderBy(_query.OrderBy, _query.Order);
				if (orderBy.Expression.Source == _queryPlan.IndexExpression)
				{
					_queryPlan.Index.Order = orderBy.Order;
					orderBy = null;
				}
				_queryPlan.OrderBy = orderBy;
			}
		}

		private void DefineGroupBy()
		{
			if (_query.GroupBy != null)
			{
				if (_query.OrderBy != null)
				{
					throw new NotSupportedException("GROUP BY expression do not support ORDER BY");
				}
				if (_query.Includes.Count > 0)
				{
					throw new NotSupportedException("GROUP BY expression do not support INCLUDE");
				}
				GroupBy groupBy = new GroupBy(_query.GroupBy, _queryPlan.Select.Expression, _query.Having);
				OrderBy orderBy = null;
				if (!(groupBy.Expression.Source == _queryPlan.IndexExpression))
				{
					orderBy = new OrderBy(groupBy.Expression, 1);
				}
				_queryPlan.GroupBy = groupBy;
				_queryPlan.OrderBy = orderBy;
			}
		}

		private void DefineIncludes()
		{
			foreach (BsonExpression include in _query.Includes)
			{
				string field = include.Fields.Single();
				bool num = _queryPlan.Filters.Any((BsonExpression x) => x.Fields.Contains(field)) || (_queryPlan.OrderBy?.Expression.Fields.Contains(field) ?? false);
				if (num)
				{
					_queryPlan.IncludeBefore.Add(include);
				}
				if (!num || _queryPlan.OrderBy != null)
				{
					_queryPlan.IncludeAfter.Add(include);
				}
			}
		}
	}
}
