using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionAggregator
{
    public class ExpressionList<T> : IList<Expression<Func<T, bool>>>
    {
        private readonly List<Expression<Func<T, bool>>> _list;
        private readonly bool _removeNullOperands;

        /// <summary>
        /// Initializes the custom expression list
        /// </summary>
        public ExpressionList(bool remoteNullOperands = true)
        {
            _list = new List<Expression<Func<T, bool>>>();
            _removeNullOperands = remoteNullOperands;
        }
        
        /// <summary>
        /// Override of add method, but does not add if operand is null
        /// </summary>
        /// <param name="exp"></param>
        public void AddIfNotNull(Expression<Func<T, bool>> exp)
        {
            // convert expression to binary expression
            if (_removeNullOperands && exp.Body is BinaryExpression binaryExpression && (IsNullExpression(binaryExpression.Left) || IsNullExpression(binaryExpression.Right)))
            {
                return;
            }

            _list.Add(exp);
        }
                
        /// <summary>
        /// Checks if expression is null or not
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static bool IsNullExpression(Expression exp)
        {
            // If types are different  for example int and int? there will be an extra conversion expression, we need to unwrap this
            if (exp is UnaryExpression uExp) exp = uExp.Operand;
        
            // If we are dealing with a captured variable, then teh constant will be the capture object and the value is stored as a member on this object
            else if (exp is MemberExpression mExp && mExp.Expression is ConstantExpression cExp)
            {
                var value = mExp.Member is PropertyInfo pInfo ? pInfo.GetValue(cExp.Value) :
                    mExp.Member is FieldInfo fInfo ? fInfo.GetValue(cExp.Value) :
                    throw new NotSupportedException();
        
                return value == null;
            }
            // If we use a simple constant, this is what will be called
            else if (exp is ConstantExpression constantExpression)
            {
                return constantExpression.Value == null;
            }
        
            return false;
        }

        #region ILIST_IMPLEMENTATION

        public IEnumerator<Expression<Func<T, bool>>> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Expression<Func<T, bool>> item)
        {
            AddIfNotNull(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(Expression<Func<T, bool>> item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(Expression<Func<T, bool>>[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(Expression<Func<T, bool>> item)
        {
            return _list.Remove(item);
        }

        public int Count => _list.Count;
        public bool IsReadOnly => false;

        public int IndexOf(Expression<Func<T, bool>> item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, Expression<Func<T, bool>> item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public Expression<Func<T, bool>> this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }
                
        #endregion
    }
}