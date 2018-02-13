using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionAggregator.Interfaces
{
    public interface IExpressionAggregator<T>
    {
        Func<T, bool> AggregateAndCompile(IEnumerable<Expression<Func<T, bool>>> list);
        
        Expression<Func<T, bool>> Aggregate(IEnumerable<Expression<Func<T, bool>>> list);
    }
}