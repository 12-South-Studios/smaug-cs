﻿using Realm.Library.Common.Objects;
using System.Threading;
using System.Threading.Tasks;

namespace Realm.Library.Common.Extensions

{
    /// <summary>
    ///
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Does any task clean-up
        /// </summary>
        /// <param name="task"></param>
        /// <param name="tokenSource"></param>
        public static void Cancel(this Task task, CancellationTokenSource tokenSource)
        {
            if (task.IsNotNull())
            {
                if (task.Status != TaskStatus.Canceled || task.Status != TaskStatus.RanToCompletion)
                    tokenSource.Cancel();

                task.Dispose();
            }

            if (tokenSource.IsNotNull())
                tokenSource.Dispose();
        }
    }
}