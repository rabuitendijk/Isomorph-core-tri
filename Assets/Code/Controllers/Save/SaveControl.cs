using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveControl : Controller
{
    public static SaveControl main;
    public static ulong isoObject_id;

    protected SaveControl()
    {
        isoObject_id = 0;
        main = this;
    }

    public abstract void delayedConstruction();

    protected abstract void destructor();
    protected abstract void save(string filename);

    public void destroy()
    {
        destructor();
    }
}
