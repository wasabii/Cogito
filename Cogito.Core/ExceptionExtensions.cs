﻿using System;
using System.Collections.Generic;

namespace Cogito
{

    /// <summary>
    /// Various extension methods for <see cref="Exception"/> instances.
    /// </summary>
    public static class ExceptionExtensions
    {

        /// <summary>
        /// Traces the exception to the default trace source as an error.
        /// </summary>
        /// <param name="self"></param>
        public static void Trace(this Exception self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            System.Diagnostics.Trace.TraceError("{0:HH:mm:ss.fff} {1} {2}", DateTime.Now, self.GetType().FullName, self);
        }

        /// <summary>
        /// Unpacks any InnerExceptions hidden by <see cref="AggregateException"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static IEnumerable<Exception> Expand(this Exception e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            var ae = e as AggregateException;
            if (ae != null)
                foreach (var aei in ae.InnerExceptions)
                    foreach (var aee in Expand(aei))
                        yield return aee;
            else
                yield return e;
        }

    }

}
