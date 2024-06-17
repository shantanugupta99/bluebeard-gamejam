using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureComponent : MonoBehaviour
{
    public string status;
    // Start is called before the first frame update
    void Start()
    {
        status = "dirty";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeStatus()
    {
        status = "clean";
    }
    
    
}
