using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureComponent : MonoBehaviour
{
    public bool dirty;
    void Start()
    {
        dirty = true;
    }

    public void ChangeStatus()
    {
        dirty = false;
    }
    
}
