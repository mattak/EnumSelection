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
                ClassName = "EnumSelectionTool.EnumTest1",
                ClassValue = "Value2"
            };
            Assert.AreEqual(EnumTest1.Value2, selection1.GetEnum<EnumTest1>());
            Assert.AreEqual(EnumTest1.Value2, selection1.GetEnum(typeof(EnumTest1)));

            var selectionEmpty = new EnumSelection();
            Assert.IsNull(selectionEmpty.GetEnum<EnumTest1>());
            Assert.IsNull(selectionEmpty.GetEnum(typeof(EnumTest1)));

            var selectionInvalid1 = new EnumSelection()
            {
                ClassName = "Invalid",
                ClassValue = "Invalid",
            };
            var selectionInvalid2 = new EnumSelection()
            {
                ClassName = "EnumSelectionTool.EnumTest1",
                ClassValue = "Invalid",
            };
            Assert.IsNull(selectionInvalid1.GetEnum<EnumTest1>());
            Assert.IsNull(selectionInvalid1.GetEnum(typeof(EnumTest1)));
            Assert.IsNull(selectionInvalid2.GetEnum<EnumTest1>());
            Assert.IsNull(selectionInvalid2.GetEnum(typeof(EnumTest1)));

            var selectionConstructor1 = new EnumSelection(typeof(EnumTest1));
            var selectionConstructor2 = new EnumSelection(EnumTest1.Value1);
            Assert.AreEqual("EnumSelectionTool.EnumTest1", selectionConstructor1.ClassName);
            Assert.IsNull(selectionConstructor1.ClassValue);
            Assert.AreEqual("EnumSelectionTool.EnumTest1", selectionConstructor2.ClassName);
            Assert.AreEqual("Value1", selectionConstructor2.ClassValue);
        }
    }

    public enum EnumTest1
    {
        Value1,
        Value2,
    }
}