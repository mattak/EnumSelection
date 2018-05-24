using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace EnumSelectionTool
{
    [CustomPropertyDrawer(typeof(EnumSelectionOption))]
    public class EnumSelectionOptionDrawer : EnumSelectionDrawer
    {
        protected override void CheckInitialize()
        {
            var option = (EnumSelectionOption) attribute;

            if (this.EnumTypes == null)
            {
                var typeNames =
                        AppDomain.CurrentDomain.GetAssemblies().SelectMany(it => it.GetTypes())
                            .Where(it => it.IsEnum)
                            .Where(it => it.GetCustomAttribute(typeof(EnumSelectionEnable)) != null)
                            .Select(it => new
                            {
                                Type = it,
                                Name = it.FullName,
                                Assembly = it.Assembly.GetName().Name,
                                Category = (it.GetCustomAttribute(typeof(EnumSelectionEnable)) as EnumSelectionEnable).Category
                            })
                            .Where(it => option == null || option.Category == it.Category)
                    ;
                this.EnumTypes = typeNames.Select(it => it.Type).ToList();
                this.EnumNames = typeNames.Select(it => it.Name).ToList();
                this.EnumAssemblyNames = typeNames.Select(it => it.Assembly).ToList();
            }
        }
    }
}