using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Maid : MonoBehaviour, IGoap
{
    public GameObject duster;
    public float moveSpeed;
    private FurnitureComponent[] furniture;
    private float dirtyFurnCount = 0;

    void Start()
    {

    }

    void Update()
    {

    }

    public HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData =  new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("hasDuster", (duster != null)));
        worldData.Add(new KeyValuePair<string, object>("dirtyFurnitureCount", GetDirtyFurnitureCount()));
        return worldData;
    }

    public HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		
        goal.Add(new KeyValuePair<string, object>("dirtyFurnitureCount", 0 ));
        return goal;
    }
    public void planFailed (HashSet<KeyValuePair<string, object>> failedGoal)
    {
        
    }

    public void planFound (HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
        // Yay we found a plan for our goal
        Debug.Log ("<color=green>Plan found</color> "+GoapAgent.prettyPrint(actions));
    }
    public void actionsFinished ()
    {
        // Everything is done, we completed our actions for this gool. Hooray!
        Debug.Log ("<color=blue>Actions completed</color>");
    }

    public void planAborted (GoapAction aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.
        //Debug.Log ("<color=red>Plan Aborted</color> "+GoapAgent.prettyPrint(aborter));
    }
    public bool moveAgent(GoapAction nextAction) {
        // move towards the NextAction's target
        float step = moveSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.target.transform.position, step);
		
        if (gameObject.transform.position.Equals(nextAction.target.transform.position) ) {
            // we are at the target location, we are done
            nextAction.setInRange(true);
            return true;
        } else
            return false;
    }

    private float GetDirtyFurnitureCount()
    {
        furniture = ((FurnitureComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(FurnitureComponent)))
            .Where(f => f.dirty)
            .ToArray();
        dirtyFurnCount = furniture.Length;
        return dirtyFurnCount;
    }
    
}
