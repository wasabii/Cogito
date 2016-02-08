﻿using System;
using System.Diagnostics.Contracts;
using System.Fabric;
using System.Fabric.Health;
using System.Net;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

using Owin;

namespace Cogito.Fabric.Http
{

    /// <summary>
    /// Provides extension methods to the <see cref="IAppBuilder"/> instances.
    /// </summary>
    public static class AppBuilderExtensions
    {

        const string DEFAULT_HEALTH_PATH = "/health";

        /// <summary>
        /// Adds a middleware to your web application to service Service Fabric health checks for the specified service from the given path.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="service"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IAppBuilder UseFabricHealth(this IAppBuilder app, StatelessService service, string path = DEFAULT_HEALTH_PATH)
        {
            Contract.Requires<ArgumentNullException>(app != null);
            Contract.Requires<ArgumentNullException>(service != null);
            Contract.Requires<ArgumentNullException>(path != null);

            return UseFabricHealth(app, path, service.ServiceInitializationParameters.PartitionId, service.ServiceInitializationParameters.InstanceId);
        }

        /// <summary>
        /// Adds a middleware to your web application to service Service Fabric health checks for the specified service from the given path.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="service"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IAppBuilder UseFabricHealth(this IAppBuilder app, StatefulService service, string path = DEFAULT_HEALTH_PATH)
        {
            Contract.Requires<ArgumentNullException>(app != null);
            Contract.Requires<ArgumentNullException>(service != null);
            Contract.Requires<ArgumentNullException>(path != null);

            return UseFabricHealth(app, path, service.ServiceInitializationParameters.PartitionId, service.ServiceInitializationParameters.ReplicaId);
        }

        /// <summary>
        /// Adds a middleware to your web application to service Service Fabric health checks for the specified replica from the given path.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="path"></param>
        /// <param name="partitionId"></param>
        /// <param name="replicaOrInstanceId"></param>
        /// <returns></returns>
        static IAppBuilder UseFabricHealth(this IAppBuilder app, string path, Guid partitionId, long replicaOrInstanceId)
        {
            Contract.Requires<ArgumentNullException>(app != null);
            Contract.Requires<ArgumentNullException>(path != null);

            return UseFabricHealth(app, path, async () =>
            {
                using (var fabric = new FabricClient())
                    return await fabric.HealthManager.GetReplicaHealthAsync(partitionId, replicaOrInstanceId);
            });
        }

        /// <summary>
        /// Adds a middleware to your web application to service Service Fabric health checks for the specified <see cref="HealthState"/> function.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="path"></param>
        /// <param name="health"></param>
        /// <returns></returns>
        static IAppBuilder UseFabricHealth(this IAppBuilder app, string path, Func<Task<EntityHealth>> health)
        {
            Contract.Requires<ArgumentNullException>(app != null);
            Contract.Requires<ArgumentNullException>(path != null);
            Contract.Requires<ArgumentNullException>(health != null);

            // attach service to context
            app.Use(async (context, func) =>
            {
                // check for requests to health path
                if (context.Request.Path.Value == path)
                {
                    var h = await health();
                    if (h == null)
                        throw new NullReferenceException("EntityHealth was null");

                    // check for various health errors
                    switch (h.AggregatedHealthState)
                    {
                        case HealthState.Error:
                            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                            break;
                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                            break;
                    }

                    // to serialize object
                    var s = new JsonSerializer();
                    s.Converters.Add(new StringEnumConverter());

                    // write out health report
                    context.Response.ContentType = "application/json";
                    context.Response.Write(JObject.FromObject(h, s).ToString(Formatting.Indented));

                    return;
                }

                await func();
            });

            return app;
        }

    }

}
