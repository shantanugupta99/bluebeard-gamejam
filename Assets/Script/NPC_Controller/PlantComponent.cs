using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantComponent : MonoBehaviour
{
    public bool thirsty;

    private void Start()
    {
        thirsty = true;
    }

    public void ChangeStatus()
    {
        thirsty = false;
    }
}
