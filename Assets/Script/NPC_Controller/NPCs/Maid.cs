using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Maid : MonoBehaviour,IGoap
{
    // Start is called before the first frame update
    public float moveSpeed = 1;
    public DusterComponent duster;
    public List<FurnitureComponent> dirtyFurniture = new List<FurnitureComponent>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public HashSet<KeyValuePair<string,object>> getWorldState () {
        FurnitureComponent[] listFurniture =
            (FurnitureComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(FurnitureComponent));
        foreach (FurnitureComponent furn in listFurniture)
        {
            if (furn.status == "dirty")
            {
                dirtyFurniture.Add(furn);
            }
        }
        HashSet<KeyValuePair<string,object>> worldData = new HashSet<KeyValuePair<string,object>> ();

        worldData.Add(new KeyValuePair<string, object>("dirtyFurnitureCount",(dirtyFurniture.Count)) );
        worldData.Add(new KeyValuePair<string, object>("hasDuster",(duster!=null)));

        return worldData;
    }

    public HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("dirtyFurnitureCount", 0));
        return goal;
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
        Debug.Log ("<color=red>Plan Aborted</color> "+GoapAgent.prettyPrint(aborter));
    }

    public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        
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
}
