using UnityEngine;

namespace EnumSelectionTool.Example
{
    public class EnumSelectionDemo : MonoBehaviour
    {
        public EnumSelection Demo1;
        public EnumSelection Demo2;
        public EnumSelection Demo3;

        // show only specified category enums
        [EnumSelectionOption(Category = "MyCategory")]
        public EnumSelection CategoryDemo;

        private void Start()
        {
            // get enum value with generics, type, non-generics
            Debug.Log(this.Demo1.GetEnum<SampleEnum1>()?.ToString());
            Debug.Log(this.Demo2.GetEnum(typeof(SampleEnum2))?.ToString());
            Debug.Log(this.Demo3.GetEnum()?.ToString());

            // check enum type safely
            if (this.Demo3.IsEnumClass<SampleEnum1>())
            {
                var value = this.Demo3.GetEnum<SampleEnum1>();
                Debug.LogFormat("Class:SampleEnum1, Value:{0}", value);
            }
            else if (this.Demo3.IsEnumClass<SampleEnum2>())
            {
                var value = this.Demo3.GetEnum<SampleEnum2>();
                Debug.LogFormat("Class:SampleEnum2, Value:{0}", value);
            }
        }
    }
}