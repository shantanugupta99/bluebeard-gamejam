using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DustFurniture : GoapAction
{
    private bool dusted = false;
    private FurnitureComponent targetFurniture; //which furniture to dust
    private float startTime = 0;
    public float dustDuration = 2;
    private float dirtyFurnC = 3;

    public void DustFurnitureAction()
    {
        addPrecondition("hasDuster",true);
        addPrecondition("dirtyFurnitureCount",(dirtyFurnC>0)); //if count>0
        addEffect("dirtyFurnitureCount",(dirtyFurnC=dirtyFurnC-1)); // subtract 1 from count
    }

    public override void reset()
    {
        dusted = false;
        targetFurniture = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return dusted;
    }

    public override bool requiresInRange()
    {
        return true; //yes we need to be within range of dirty furniture
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        FurnitureComponent[] furniture =
            (FurnitureComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(FurnitureComponent));
        FurnitureComponent closest = null;
        float closestDist = 0;
        foreach (FurnitureComponent furn in furniture) // find closest piece of furniture
        {
            if (furn.status == "dirty")
            {
                if (closest == null)
                {
                    closest = furn;
                    closestDist = (furn.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (furn.gameObject.transform.position - agent.transform.position).magnitude;
                    if (dist < closestDist)
                    {
                        closest = furn;
                        closestDist = dist;
                    } // we found a closer one
                }
            }
            
        }

        if (closest == null)
        {
            return false;
        }

        targetFurniture = closest;
        target = targetFurniture.gameObject;
        return closest != null;
    }

    public override bool performAction(GameObject agent)
    {
        if (startTime == 0)
            startTime = Time.time;
        if (Time.time - startTime > dustDuration) // finished dusting
        {
            dusted = true;
            FurnitureComponent frn = agent.GetComponent<FurnitureComponent>();
            frn.ChangeStatus();
        }

        return true;
    }
}
