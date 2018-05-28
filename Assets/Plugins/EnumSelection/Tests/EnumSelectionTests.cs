using NUnit.Framework;

namespace EnumSelectionTool
{
    public class EnumSelectionTests
    {
        [Test]
        public void GetTypeTest()
        {
            var selection1 = new EnumSelection()
            {
                AssemblyName = typeof(EnumTest1).Assembly.GetName().Name,
                ClassName = typeof(EnumTest1).FullName,
                ClassValue = "Value2"
            };
            Assert.AreEqual(EnumTest1.Value2, selection1.GetEnum<EnumTest1>());
            Assert.AreEqual(EnumTest1.Value2, selection1.GetEnum(typeof(EnumTest1)));
            Assert.AreEqual(EnumTest1.Value2, selection1.GetEnum());

            var selectionEmpty = new EnumSelection();
            Assert.IsNull(selectionEmpty.GetEnum<EnumTest1>());
            Assert.IsNull(selectionEmpty.GetEnum(typeof(EnumTest1)));
            Assert.IsNull(selectionEmpty.GetEnum());

            var selectionInvalid1 = new EnumSelection()
            {
                AssemblyName = "Invalid",
                ClassName = "Invalid",
                ClassValue = "Invalid",
            };
            var selectionInvalid2 = new EnumSelection()
            {
                AssemblyName = typeof(EnumTest1).Assembly.GetName().Name,
                ClassName = typeof(EnumTest1).FullName,
                ClassValue = "Invalid",
            };
            Assert.IsNull(selectionInvalid1.GetEnum<EnumTest1>());
            Assert.IsNull(selectionInvalid1.GetEnum(typeof(EnumTest1)));
            Assert.IsNull(selectionInvalid1.GetEnum());
            Assert.IsNull(selectionInvalid2.GetEnum<EnumTest1>());
            Assert.IsNull(selectionInvalid2.GetEnum(typeof(EnumTest1)));
            Assert.IsNull(selectionInvalid2.GetEnum());

            var selectionConstructor1 = new EnumSelection(typeof(EnumTest1));
            var selectionConstructor2 = new EnumSelection(EnumTest1.Value1);
            Assert.AreEqual(typeof(EnumTest1).Assembly.GetName().Name, selectionConstructor1.AssemblyName);
            Assert.AreEqual("EnumSelectionTool.EnumTest1", selectionConstructor1.ClassName);
            Assert.IsNull(selectionConstructor1.ClassValue);

            Assert.AreEqual(typeof(EnumTest1).Assembly.GetName().Name, selectionConstructor2.AssemblyName);
            Assert.AreEqual("EnumSelectionTool.EnumTest1", selectionConstructor2.ClassName);
            Assert.AreEqual("Value1", selectionConstructor2.ClassValue);
        }

        [Test]
        public void IsEnumClassTest()
        {
            Assert.IsTrue(new EnumSelection(typeof(EnumTest1)).IsEnumClass<EnumTest1>());
            Assert.IsTrue(new EnumSelection(typeof(EnumTest1)).IsEnumClass(typeof(EnumTest1)));
            Assert.IsFalse(new EnumSelection(typeof(EnumTest1)).IsEnumClass<EnumTest2>());
            Assert.IsFalse(new EnumSelection(typeof(EnumTest1)).IsEnumClass(typeof(EnumTest2)));

            Assert.IsTrue(new EnumSelection(EnumTest1.Value1).IsEnumClass<EnumTest1>());
            Assert.IsTrue(new EnumSelection(EnumTest1.Value1).IsEnumClass(typeof(EnumTest1)));
            Assert.IsFalse(new EnumSelection(EnumTest1.Value1).IsEnumClass<EnumTest2>());
            Assert.IsFalse(new EnumSelection(EnumTest1.Value1).IsEnumClass(typeof(EnumTest2)));
        }

        [Test]
        public void CanParse()
        {
            Assert.IsTrue(new EnumSelection(EnumTest1.Value1).CanParse());
            Assert.IsFalse(new EnumSelection(typeof(EnumTest1)).CanParse());
            Assert.IsFalse(new EnumSelection().CanParse());
            Assert.IsFalse(new EnumSelection()
            {
                AssemblyName = typeof(EnumTest1).Assembly.GetName().Name,
                ClassName = typeof(EnumTest1).FullName,
                ClassValue = "Invalid",
            }.CanParse());
        }
    }

    public enum EnumTest1
    {
        Value1,
        Value2,
    }

    public enum EnumTest2
    {
    }
}
