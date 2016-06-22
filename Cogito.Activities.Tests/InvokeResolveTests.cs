﻿using System;
using System.Activities;
using System.Activities.Expressions;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cogito.Activities.Tests
{

    /// <summary>
    /// Just ensures that various generic method resolutions function.
    /// </summary>
    [TestClass]
    public class InvokeResolveTests
    {

        [TestMethod]
        public void Test_resolve_func_activity()
        {
            var i = Expressions.Invoke(() => true);
            Assert.IsTrue(i is FuncActivity<bool>);
        }

        [TestMethod]
        public void Test_resolve_func_activity_with_arg1()
        {
            var i = Expressions.Invoke(j => true, new Literal<int>());
            Assert.IsTrue(i is FuncActivity<int, bool>);
        }

        [TestMethod]
        public void Test_resolve_func_activity_with_delegate_arg1()
        {
            var i = Expressions.Invoke(j => true, new DelegateInArgument<int>());
            Assert.IsTrue(i is FuncActivity<int, bool>);
        }

        [TestMethod]
        public void Test_resolve_action_activity()
        {
            var i = Expressions.Invoke(() => Console.WriteLine());
            Assert.IsTrue(i is ActionActivity);
        }

        [TestMethod]
        public void Test_resolve_action_activity_with_arg1()
        {
            var i = Expressions.Invoke(j => Console.WriteLine(), new Literal<int>());
            Assert.IsTrue(i is ActionActivity<int>);
        }

        [TestMethod]
        public void Test_resolve_action_activity_with_delegate_arg1()
        {
            var i = Expressions.Invoke(j => Console.WriteLine(), new DelegateInArgument<int>());
            Assert.IsTrue(i is ActionActivity<int>);
        }

        [TestMethod]
        public void Test_resolve_async_func_activity()
        {
            var i = Expressions.InvokeAsync(() => Task.FromResult(true));
            Assert.IsTrue(i is AsyncFuncActivity<bool>);
        }

        [TestMethod]
        public void Test_resolve_async_func_activity_with_arg1()
        {
            var i = Expressions.InvokeAsync(j => Task.FromResult(true), new Literal<int>());
            Assert.IsTrue(i is AsyncFuncActivity<int, bool>);
        }

        [TestMethod]
        public void Test_resolve_async_func_activity_with_delegate_arg1()
        {
            var i = Expressions.InvokeAsync(j => Task.FromResult(true), new DelegateInArgument<int>());
            Assert.IsTrue(i is AsyncFuncActivity<int, bool>);
        }

        [TestMethod]
        public void Test_resolve_async_action_activity()
        {
            var i = Expressions.InvokeAsync(() => Task.Run(() => { }));
            Assert.IsTrue(i is AsyncActionActivity);
        }

        [TestMethod]
        public void Test_resolve_async_action_activity_with_arg1()
        {
            var i = Expressions.InvokeAsync(j => Task.Run(() => { }), new Literal<int>());
            Assert.IsTrue(i is AsyncActionActivity<int>);
        }

        [TestMethod]
        public void Test_resolve_async_action_activity_with_delegate_arg1()
        {
            var i = Expressions.InvokeAsync(j => Task.Run(() => { }), new DelegateInArgument<int>());
            Assert.IsTrue(i is AsyncActionActivity<int>);
        }

    }

}
