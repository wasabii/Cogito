﻿using System;
using System.Activities;
using System.Activities.Statements;
using System.Threading.Tasks;

namespace Cogito.Activities
{

    public static partial class Activities
    {

        public static AsyncActionActivity Invoke(Func<Task> func)
        {
            return new AsyncActionActivity(context => func());
        }
        public static AsyncActionActivity InvokeWithContext(Func<ActivityContext, Task> func)
        {
            return new AsyncActionActivity(func);
        }

        public static Sequence Then(this Activity activity, Func<Task> action)
        {
            return Then(activity, new AsyncActionActivity(contxt => action()));
        }

        public static Sequence Then(this Activity activity, Func<ActivityContext, Task> action)
        {
            return Then(activity, new AsyncActionActivity(action));
        }

        public static AsyncActionActivity<TResult> Then<TResult>(this Activity<TResult> activity, Func<TResult, Task> action)
        {
            return new AsyncActionActivity<TResult>((arg, context) => action(arg), activity);
        }

        public static AsyncActionActivity<TResult> Then<TResult>(this Activity<TResult> activity, Func<TResult, ActivityContext, Task> action)
        {
            return new AsyncActionActivity<TResult>(action, activity);
        }

    }

    /// <summary>
    /// Provides an <see cref="Activity"/> that executes the given asynchronous function.
    /// </summary>
    public class AsyncActionActivity :
        AsyncTaskCodeActivity
    {

        public static implicit operator ActivityAction(AsyncActionActivity activity)
        {
            return Activities.Delegate(() => activity);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public AsyncActionActivity()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        public AsyncActionActivity(Func<ActivityContext, Task> action)
            : this()
        {
            Action = action;
        }

        /// <summary>
        /// Gets or sets the action to be invoked.
        /// </summary>
        [RequiredArgument]
        public Func<ActivityContext, Task> Action { get; set; }

        protected override Task ExecuteAsync(AsyncCodeActivityContext context)
        {
            return Action(context);
        }

    }

}