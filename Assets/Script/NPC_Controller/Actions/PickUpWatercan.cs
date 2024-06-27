using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWatercan : GoapAction
{
    private bool pickedUp = false;
    private WatercanComponent targetCan; //furniture to dust
    private float startTime = 0;
    public float pick = 0.5f;

    public PickUpWatercan()
    {
        addPrecondition("hasWaterCan",false);
        addEffect("hasWaterCan",true);
    }
    public override void reset()
    {
        pickedUp = false;
        targetCan = null;
        startTime = 0;
    }

    public void doReset()
    {
        reset();
    }
    public override bool isDone()
    {
        return pickedUp;
    }
	
    public override bool requiresInRange()
    {
        return true; // yes we need to be near the can
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        targetCan = (WatercanComponent)UnityEngine.GameObject.FindObjectOfType(typeof(WatercanComponent));
        target = targetCan.gameObject;
        return target != null;
    }
    public override bool performAction(GameObject agent)
    {
        pickedUp = true;
        return true;
    }
}
