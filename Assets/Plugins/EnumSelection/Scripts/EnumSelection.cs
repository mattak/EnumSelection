using System;
using UnityEngine;

namespace EnumSelectionTool
{
    [Serializable]
    public class EnumSelection
    {
        public string AssemblyName;
        public string ClassName;
        public string ClassValue;

        public EnumSelection()
        {
        }

        public EnumSelection(Type type)
        {
            this.AssemblyName = type.Assembly.GetName().Name;
            this.ClassName = type.FullName;
        }

        public EnumSelection(Enum enumValue)
        {
            this.AssemblyName = enumValue.GetType().Assembly.GetName().Name;
            this.ClassName = enumValue.GetType().FullName;
            this.ClassValue = enumValue.ToString();
        }

        public TEnum? GetEnum<TEnum>()
            where TEnum : struct
        {
            return Parse(typeof(TEnum)) as TEnum?;
        }

        public Enum GetEnum(Type type)
        {
            return Parse(type) as Enum;
        }

        public Enum GetEnum()
        {
            return this.GetEnum(this.GetClass());
        }

        public Type GetClass()
        {
            try
            {
                return Type.GetType(string.Format("{0}, {1}", this.ClassName, this.AssemblyName));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool IsEnumClass(Type type)
        {
            return this.GetClass() == type;
        }

        public bool IsEnumClass<TEnum>()
        {
            return this.GetClass() == typeof(TEnum);
        }

        public bool CanParse()
        {
            var type = this.GetClass();
            if (type == null) return false;

            try
            {
                Enum.Parse(type, this.ClassValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private object Parse(Type type)
        {
            try
            {
                return Enum.Parse(type, this.ClassValue);
            }
            catch (Exception)
            {
            }

            LogCannotParse(type);
            return null;
        }

        private void LogCannotParse(Type type)
        {
            Debug.LogWarningFormat(
                "Cannot parse enum: {0}, {1} as {2}",
                this.ClassName,
                this.ClassValue,
                type != null ? type.FullName : ""
            );
        }
    }
}