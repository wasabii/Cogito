﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace Cogito.Core.Linq
{

    /// <summary>
    /// Implementation of <see cref="IQueryable"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Query<T> :
        IQueryable<T>
    {

        readonly QueryProvider provider;
        readonly Expression expression;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="expression"></param>
        public Query(QueryProvider provider, Expression expression)
        {
            Contract.Requires<ArgumentNullException>(provider != null);
            Contract.Requires<ArgumentNullException>(expression != null);
            Contract.Requires<ArgumentOutOfRangeException>(typeof(IQueryable<T>).IsAssignableFrom(expression.Type));

            this.provider = provider;
            this.expression = expression;
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public IQueryProvider Provider
        {
            get { return provider; }
        }

        public Expression Expression
        {
            get { return expression; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)provider.Execute(expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)provider.Execute(expression)).GetEnumerator();
        }

    }

}