﻿using System;
using System.Threading;

namespace Cogito.Threading
{

    /// <summary>
    /// Describes a work item dispatched to a <see cref="SynchronizationContext"/>.
    /// </summary>
    struct SynchronizationContextWorkItem
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public SynchronizationContextWorkItem(SendOrPostCallback callback, object state)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            Callback = callback;
            State = state;
        }

        /// <summary>
        /// Gets the callback to be invoked.
        /// </summary>
        public SendOrPostCallback Callback { get; set; }

        /// <summary>
        /// Gets the state to be passed to the callback.
        /// </summary>
        public object State { get; set; }

    }

}
