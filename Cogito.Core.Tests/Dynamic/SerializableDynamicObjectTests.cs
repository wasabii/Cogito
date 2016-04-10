﻿using System.Runtime.Serialization;
using System.Xml.Linq;
using Cogito.Dynamic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace Cogito.Core.Tests.Dynamic
{

    [TestClass]
    public class SerializableDynamicObjectTests
    {

        [TestMethod]
        public void Test_SerializeToXml()
        {
            dynamic o = new SerializableDynamicObject();
            o.Property1 = "foo";
            o.Property2 = "bar";
            var s = new DataContractSerializer(typeof(SerializableDynamicObject));
            var m = new XDocument();
            using (var w = m.CreateWriter())
                s.WriteObject(w, o);
        }

        [TestMethod]
        public void Test_SerializeToJson()
        {
            dynamic o = new SerializableDynamicObject();
            o.Property1 = "foo";
            o.Property2 = "bar";
            var s = JsonConvert.SerializeObject(o);
        }

        [TestMethod]
        public void Test_Nested_SerializeToJson()
        {
            dynamic o = new SerializableDynamicObject();
            o.Property1 = "foo";
            o.Property2 = "bar";
            o.Nested1 = new SerializableDynamicObject();
            o.Nested1.Property1 = "foo";
            o.Nested1.Property2 = "foo";
            var s = JsonConvert.SerializeObject(o);
        }

        [TestMethod]
        public void Test_Nested_SerializeToXml()
        {
            dynamic o = new SerializableDynamicObject();
            o.Property1 = "foo";
            o.Property2 = "bar";
            o.Nested1 = new SerializableDynamicObject();
            o.Nested1.Property1 = "foo";
            o.Nested1.Property2 = "foo";
            var s = new DataContractSerializer(typeof(SerializableDynamicObject));
            var m = new XDocument();
            using (var w = m.CreateWriter())
                s.WriteObject(w, o);
        }

        [TestMethod]
        public void Test_DeserializeFromJson()
        {
            dynamic o = new SerializableDynamicObject();
            o.Property1 = "foo";
            o.Property2 = "bar";
            var s = JsonConvert.SerializeObject(o);
            o = JsonConvert.DeserializeObject<SerializableDynamicObject>(s);
            Assert.IsNotNull(o);
            Assert.AreEqual("foo", o.Property1);
            Assert.AreEqual("bar", o.Property2);
        }

        [TestMethod]
        public void Test_DeserializeFromXml()
        {
            dynamic o = new SerializableDynamicObject();
            o.Property1 = "foo";
            o.Property2 = "bar";
            var s = new DataContractSerializer(typeof(SerializableDynamicObject));
            var m = new XDocument();
            using (var w = m.CreateWriter())
                s.WriteObject(w, o);
            o = s.ReadObject(m.CreateReader());
            Assert.IsNotNull(o);
            Assert.AreEqual("foo", o.Property1);
            Assert.AreEqual("bar", o.Property2);
        }

        [TestMethod]
        public void Test_Nested_DeserializeFromJson()
        {
            dynamic o = new SerializableDynamicObject();
            o.Property1 = "foo";
            o.Property2 = "bar";
            o.Nested1 = new SerializableDynamicObject();
            o.Nested1.Property1 = "1foo";
            o.Nested1.Property2 = "1bar";
            var s = JsonConvert.SerializeObject(o);
            o = JsonConvert.DeserializeObject<SerializableDynamicObject>(s);
            Assert.IsNotNull(o);
            Assert.AreEqual("foo", o.Property1);
            Assert.AreEqual("bar", o.Property2);
            Assert.AreEqual("1foo", o.Nested1.Property1);
            Assert.AreEqual("1bar", o.Nested1.Property2);
        }

        [TestMethod]
        public void Test_Nested_DeserializeFromXml()
        {
            dynamic o = new SerializableDynamicObject();
            o.Property1 = "foo";
            o.Property2 = "bar";
            o.Nested1 = new SerializableDynamicObject();
            o.Nested1.Property1 = "1foo";
            o.Nested1.Property2 = "1bar";
            var s = new DataContractSerializer(typeof(SerializableDynamicObject));
            var m = new XDocument();
            using (var w = m.CreateWriter())
                s.WriteObject(w, o);
            o = s.ReadObject(m.CreateReader());
            Assert.IsNotNull(o);
            Assert.AreEqual("foo", o.Property1);
            Assert.AreEqual("bar", o.Property2);
            Assert.AreEqual("1foo", o.Nested1.Property1);
            Assert.AreEqual("1bar", o.Nested1.Property2);
        }

    }

}
