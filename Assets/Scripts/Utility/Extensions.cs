using System;
using System.Collections.Generic;
using Unity.Collections;

public static class Extensions
{
    public static NativeList<T> Shuffle<T>(this NativeList<T> source, int seed) where T : unmanaged
    {
        var random = new Unity.Mathematics.Random((uint) seed);
        var n = source.Length;
        
        while (n > 1)
        {
            n--;
            var k = random.NextInt(n + 1);
            (source[k], source[n]) = (source[n], source[k]);
        }

        return source;
    }
    
    public static bool Contains<T>(this NativeList<T> source, T item) where T : unmanaged, IEquatable<T>
    {
        for (int i = 0; i < source.Length; i++)
        {
            if (source[i].Equals(item)) return true;
        }
        return false;
    }
    
    public static NativeList<T> Reverse<T>(this NativeList<T> source) where T : unmanaged
    {
        var i = 0;
        var j = source.Length - 1;

        while (i < j)
        {
            (source[i], source[j]) = (source[j], source[i]);
            i++;
            j--;
        }

        return source;
    }
    
    public static List<T> ToList<T>(this NativeList<T> source) where T : unmanaged
    {
        var list = new List<T>();

        for (int i = 0; i < source.Length; i++)
        {
            list.Add(source[i]);
        }

        return list;
    }
}
