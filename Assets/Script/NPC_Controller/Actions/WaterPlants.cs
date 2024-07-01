using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class WaterPlants : GoapAction
{
    private bool watered = false;
    private PlantComponent targetPlant; //furniture to dust
    private PlantComponent[] plants;
    private float startTime = 0;
    private FurnitureComponent[] furniture;
    private float dirtyFurnCount;
    public float waterDuration = 1f;

    public WaterPlants()
    {
        addPrecondition("plantThirsty", true);
        addPrecondition("hasWaterCan", true);
        addPrecondition("dirtyFurnitureCount", false);
        addEffect("plantThirsty",false);
    }

    public override void reset()
    {
        watered = false;
        targetPlant = null;
        startTime = 0;
    }

    public new void doReset()
    {
        reset();
    }
    public override bool isDone()
    {
        return watered;
    }
	
    public override bool requiresInRange()
    {
        return true; // yes we need to be near a plant
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        plants = ((PlantComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(PlantComponent)))
            .Where(f => f.thirsty)
            .ToArray();
        PlantComponent closest = null;
        float closestDist = 0;
        foreach (PlantComponent plant in plants)
        {
            if (plant.thirsty)
            {
                if (closest == null)
                {
                    closest = plant;
                    closestDist = (plant.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (plant.gameObject.transform.position - agent.transform.position).magnitude;
                    if (dist < closestDist)
                    {
                        closest = plant;
                        closestDist = dist;
                    }
                }
            }
        }

        if (closest == null)
        {
            return false;
        }

        targetPlant = closest;
        target = targetPlant.gameObject;
        return closest != null;

    }

    public override bool performAction(GameObject agent)
    {
        targetPlant.ChangeStatus();
        watered = true;
        return true;
    }
}