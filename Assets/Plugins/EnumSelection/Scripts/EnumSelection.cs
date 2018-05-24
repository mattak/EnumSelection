using System;
using UnityEngine;

namespace EnumSelectionTool
{
    [Serializable]
    public class EnumSelection
    {
        public string ClassName;
        public string ClassValue;

        public EnumSelection()
        {
        }

        public EnumSelection(Type type)
        {
            this.ClassName = type.FullName;
        }

        public EnumSelection(Enum enumValue)
        {
            this.ClassName = enumValue.GetType().FullName;
            this.ClassValue = enumValue.ToString();
        }

        public TEnum? GetEnum<TEnum>()
            where TEnum : struct
        {
            var type = typeof(TEnum);

            if (this.ClassName == type.FullName)
            {
                try
                {
                    return Enum.Parse(type, this.ClassValue) as TEnum?;
                }
                catch (Exception)
                {
                }
            }

            LogCannotParse(type);
            return null;
        }

        public Enum GetEnum(Type type)
        {
            if (this.ClassName == type.FullName)
            {
                try
                {
                    return Enum.Parse(type, this.ClassValue) as Enum;
                }
                catch (Exception)
                {
                }
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
                type.FullName
            );
        }
    }
}