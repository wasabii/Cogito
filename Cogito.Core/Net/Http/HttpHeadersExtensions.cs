﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Cogito.Net.Http
{

    /// <summary>
    /// Provides various extension methods for <see cref="HttpHeaders"/>.
    /// </summary>
    public static class HttpHeadersExtensions
    {

        /// <summary>
        /// Gets the headers with the specified <paramref name="name"/> or <c>null</c>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetValuesOrNull(this HttpHeaders self, string name)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            IEnumerable<string> values;
            return self.TryGetValues(name, out values) ? values : null;
        }

        /// <summary>
        /// Gets the headers with the specififed <paramref name="name"/> or an empty enumeration.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetValuesOrEmpty(this HttpHeaders self, string name)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return GetValuesOrNull(self, name) ?? Enumerable.Empty<string>();
        }

    }

}
