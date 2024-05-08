// Decompiled with JetBrains decompiler
// Type: Androids.ReflectionUtility
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 60A64EA7-F267-4623-A880-9FF7EC14F1A0
// Assembly location: E:\CACHE\Androids-1.3hsk.dll

using System;
using System.Reflection;

namespace Androids
{
    public static class ReflectionUtility
    {
        public static object CloneObjectShallowly(this object sourceObject)
        {
            if (sourceObject == null)
                return null;
            Type type = sourceObject.GetType();
            if (type.IsAbstract)
                return null;
            if (type.IsPrimitive || type.IsValueType || type.IsArray || type == typeof(string))
                return sourceObject;
            object instance = Activator.CreateInstance(type);
            if (instance == null)
                return null;
            foreach (FieldInfo field in type.GetFields())
            {
                if (!field.IsLiteral)
                {
                    object obj = field.GetValue(sourceObject);
                    field.SetValue(instance, obj);
                }
            }
            return instance;
        }
    }
}
