using UnityEngine;

namespace EnumSelectionTool.Example
{
    public class EnumSelectionDemo : MonoBehaviour
    {
        public EnumSelection Demo1;
        public EnumSelection Demo2;

        private void Start()
        {
            Debug.Log(this.Demo1.GetEnum<SampleEnum1>()?.ToString());
            Debug.Log(this.Demo2.GetEnum(typeof(SampleEnum2))?.ToString());
        }
    }

}