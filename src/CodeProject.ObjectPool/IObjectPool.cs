﻿/*
 * Generic Object Pool Implementation
 *
 * Implemented by Ofir Makmal, 28/1/2013
 *
 * My Blog: Blogs.microsoft.co.il/blogs/OfirMakmal
 * Email:   Ofir.Makmal@gmail.com
 *
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using CodeProject.ObjectPool.Core;

namespace CodeProject.ObjectPool
{
    /// <summary>
    ///   Describes all methods available on Object Pools.
    /// </summary>
    /// <typeparam name="T">The type of the objects stored in the pool.</typeparam>
    public interface IObjectPool<T>
        where T : PooledObject
    {
        /// <summary>
        ///   Gets the async Factory method that will be used for creating new objects with
        ///   async/await pattern.
        /// </summary>
        Func<CancellationToken, bool, Task<T>> AsyncFactoryMethod { get; }

        /// <summary>
        ///   Gets or sets the Diagnostics class for the current Object Pool, whose goal is to record
        ///   data about how the pool operates. By default, however, an object pool records anything,
        ///   in order to be most efficient; in any case, you can enable it through the <see
        ///   cref="ObjectPoolDiagnostics.Enabled"/> property.
        /// </summary>
        ObjectPoolDiagnostics Diagnostics { get; set; }

        /// <summary>
        ///   Gets the Factory method that will be used for creating new objects.
        /// </summary>
        Func<T> FactoryMethod { get; }

        /// <summary>
        ///   Gets or sets the maximum number of objects that could be available at the same time in
        ///   the pool.
        /// </summary>
        int MaximumPoolSize { get; set; }

        /// <summary>
        ///   Gets the count of the objects currently in the pool.
        /// </summary>
        int ObjectsInPoolCount { get; }

        /// <summary>
        ///   Clears the pool and destroys each object stored inside it.
        /// </summary>
        void Clear();

        /// <summary>
        ///   Gets a monitored object from the pool.
        /// </summary>
        /// <returns>A monitored object from the pool.</returns>
        /// <exception cref="InvalidOperationException">
        ///   If a custom async factory method has been specified, this exception is thrown in order
        ///   not to perform a sync-over-async operation, which might lead to deadlocks.
        /// </exception>
        T GetObject();

        /// <summary>
        ///   Gets a monitored object from the pool.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="continueOnCapturedContext">
        ///   Whether async calls should continue on a captured synchronization context.
        /// </param>
        /// <returns>A monitored object from the pool.</returns>
        Task<T> GetObjectAsync(
            CancellationToken cancellationToken = default,
            bool continueOnCapturedContext = default);
    }
}
