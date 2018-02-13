using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressionAggregator.Interfaces;

namespace ExpressionAggregator
{
    public class ExpressionAggregator<T> : IExpressionAggregator<T>
    {
        private readonly Func<Expression, Expression, BinaryExpression> _accumulator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accumulator"></param>
        public ExpressionAggregator(Func<Expression, Expression, BinaryExpression> accumulator = null)
        {
            _accumulator = accumulator ?? Expression.AndAlso;
        }

        /// <summary>
        /// Aggregates list of expressions and returns aggregated expression result
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> Aggregate(IEnumerable<Expression<Func<T, bool>>> list)
        {
            // common parameter of expressions
            var param = Expression.Parameter(typeof(T));

            // aggregate the expression list
            var rawAggregatedExp = list
                .Select(x => x.Body)
                .Select(exp => ParameterReplacer.Replace(param, exp))
                .Aggregate(_accumulator);
           
            // convert the result to lambda expression
            var aggregatedExp = Expression.Lambda<Func<T, bool>>(rawAggregatedExp, param);

            // return aggregated expression
            return aggregatedExp;
        }
        
        /// <summary>
        /// Aggregates list of expressions that are of type: <![CDATA[ Expression<Func<T, bool>> ]]>
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Func<T, bool> AggregateAndCompile(IEnumerable<Expression<Func<T, bool>>> list)
        {
            // convert expression to Func<T, bool>
            var result = Aggregate(list).Compile();

            // return the result
            return result;
        }
    }
}