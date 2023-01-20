// Decompiled with JetBrains decompiler
// Type: Androids.ReflectionUtility
// Assembly: Androids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8066CB7E-6A03-46DB-AA24-53C0F3BB55DD
// Assembly location: D:\SteamLibrary\steamapps\common\RimWorld\Mods\Androids\Assemblies\Androids.dll

using System;
using System.Reflection;

namespace Androids
{
  public static class ReflectionUtility
  {
    public static object CloneObjectShallowly(this object sourceObject)
    {
      if (sourceObject == null)
        return (object) null;
      Type type = sourceObject.GetType();
      if (type.IsAbstract)
        return (object) null;
      if (type.IsPrimitive || type.IsValueType || type.IsArray || type == typeof (string))
        return sourceObject;
      object instance = Activator.CreateInstance(type);
      if (instance == null)
        return (object) null;
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
