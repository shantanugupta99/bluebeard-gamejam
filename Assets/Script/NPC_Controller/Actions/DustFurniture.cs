using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class DustFurniture : GoapAction
{
    private bool dusted = false;
    private FurnitureComponent targetFurniture; //furniture to dust
    private FurnitureComponent[] furniture;
    private float startTime = 0;
    public float dustDuration = 1f;

    public DustFurniture()
    {
        addPrecondition("dirtyFurnitureCount", true);
        addPrecondition("hasDuster",true);
        addEffect("dirtyFurnitureCount",false);
    }

    public override void reset()
    {
        dusted = false;
        targetFurniture = null;
        startTime = 0;
    }

    public new void doReset()
    {
        reset();
    }
    public override bool isDone()
    {
        //doReset();
        return dusted;
    }
	
    public override bool requiresInRange()
    {
        return true; // yes we need to be near a piece of dirty furniture
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        furniture = ((FurnitureComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(FurnitureComponent)))
            .Where(f => f.dirty)
            .ToArray();
        FurnitureComponent closest = null;
        float closestDist = 0;
        foreach (FurnitureComponent furn in furniture)
        {
            if (furn.dirty)
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
                    }
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
        targetFurniture.ChangeStatus();
        dusted = true;
        return true;
    }

}
