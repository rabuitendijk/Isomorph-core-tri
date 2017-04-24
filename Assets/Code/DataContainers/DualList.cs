
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A combination of to simultainiously maintained lists
/// </summary>
public class DualList<T, U>
{

    List<T> list1 = new List<T>();
    List<U> list2 = new List<U>();
    public int Count { get { return list1.Count; } }

    /// <summary>
    /// Adds tubple to lists
    /// </summary>
    public void add(T v1, U v2)
    {
        list1.Add(v1);
        list2.Add(v2);
    }

    /// <summary>
    /// Clears lists
    /// </summary>
    public void clear()
    {
        list1.Clear();
        list2.Clear();
    }

    /// <summary>
    /// Removes tuple from lists
    /// </summary>
    public void remove(T v1, U v2)
    {
        int i = list1.IndexOf(v1);
        removeAt(i);
    }

    /// <summary>
    /// Removes tuple at index
    /// </summary>
    public void removeAt(int i)
    {
        list1.RemoveAt(i);
        list2.RemoveAt(i);
    }

    /// <summary>
    /// Get component from list 1
    /// </summary>
    public T get_1(int i)
    {
        return list1[i];
    }

    /// <summary>
    /// Get component from list 2
    /// </summary>
    public U get_2(int i)
    {
        return list2[i];
    }

    /// <summary>
    /// Get list 1
    /// </summary>
    public List<T> get_1()
    {
        return list1;
    }

    /// <summary>
    /// Get list 2
    /// </summary>
    public List<U> get_2()
    {
        return list2;
    }
}
