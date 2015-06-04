﻿using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Cogito.Components.Server
{

    /// <summary>
    /// Receives messages from the remote <see cref="AppDomainTraceListener"/>.
    /// </summary>
    public class AppDomainTraceReceiver :
        MarshalByRefObject
    {

        /// <summary>
        /// Attaches to the remote diagnostics pipeline and forwards messages to the current <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="domain"></param>
        public static void ListenTo(AppDomain domain)
        {
            Contract.Requires<ArgumentNullException>(domain != null);

            var receiver = new AppDomainTraceReceiver();
            var listener = (AppDomainTraceListener)domain.CreateInstanceFromAndUnwrap(
                    typeof(AppDomainTraceListener).Assembly.Location,
                    typeof(AppDomainTraceListener).FullName,
                    true,
                    BindingFlags.Default,
                    null,
                    null,
                    null,
                    null);

            // forward trace events from listener to receiver
            listener.ForwardTo(receiver);
        }

        /// <summary>
        /// Invoked on the local <see cref="AppDomain"/> to write a message.
        /// </summary>
        /// <param name="message"></param>
        internal void Write(string message)
        {
            Contract.Requires<ArgumentNullException>(message != null);

            Trace.Write(message);
        }

        /// <summary>
        /// Invoked on the local <see cref="AppDomain"/> to write a message.
        /// </summary>
        /// <param name="message"></param>
        internal void WriteLine(string message)
        {
            Contract.Requires<ArgumentNullException>(message != null);

            Trace.WriteLine(message);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

    }

}