
using System.Collections.Generic;
using UnityEngine;

public class DualList<T, U>
{

    List<T> list1 = new List<T>();
    List<U> list2 = new List<U>();
    public int Count { get { return list1.Count; } }

    public void add(T v1, U v2)
    {
        list1.Add(v1);
        list2.Add(v2);
    }

    public void clear()
    {
        list1.Clear();
        list2.Clear();
    }

    public void remove(T v1, U v2)
    {
        int i = list1.IndexOf(v1);
        removeAt(i);
    }

    public void removeAt(int i)
    {
        list1.RemoveAt(i);
        list2.RemoveAt(i);
    }

    public T get_1(int i)
    {
        return list1[i];
    }

    public U get_2(int i)
    {
        return list2[i];
    }

    public List<T> get_1()
    {
        return list1;
    }

    public List<U> get_2()
    {
        return list2;
    }
}
