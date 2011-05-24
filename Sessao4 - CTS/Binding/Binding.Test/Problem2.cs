using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Binding.Test
{
    [TestClass]
    public class Problem2
    {
        class A
        {
            [BindingAttribute(Name="AnInteger",Required=false)]
            public int AnInteger { get; set; }
            [BindingAttribute(Name = "AString", Required = false)]
            public string AString { get; set; }

            [BindingAttribute(Name = "AnIntegerV2", Required = false)]
            public int AStringCustoName { get; set; }

            public int AnFieldInteger = -1;
            public List<string> AnList;
        }

        [TestMethod]
        public void can_bind_property_to_A_using_attribute()
        {
            var binder = new Binder<A>();
            var pairs = new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("AnInteger","2"),
            new KeyValuePair<string,string>("AString","abc")
            };

            var a = binder.BindTo(pairs);

            Assert.AreEqual(2, a.AnInteger);
            Assert.AreEqual("abc", a.AString);
        }

        [TestMethod]
        public void can_bind_property_to_A_using_different_name_from_attribute()
        {
            var binder = new Binder<A>();
            var pairs = new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("AnIntegerV2","4")
            };

            var a = binder.BindTo(pairs);

            Assert.AreEqual(4, a.AnInteger);
        }

        [TestMethod]
        public void must_bind_required_property_to_A()
        {

        }

        [TestMethod]
        public void cannot_bind_to_A_inexistent_attribute_definition()
        {
            
        }

        [TestMethod]
        public void cannot_define_same_name_on_different_property_to_B()
        {
            
        }
    }
}
