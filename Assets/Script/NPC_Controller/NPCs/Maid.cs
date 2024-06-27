using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
public class Maid : MonoBehaviour, IGoap
{
    public GameObject duster;
    public float moveSpeed;
    private FurnitureComponent[] furniture;
    private float dirtyFurnCount = 0;

    void Start()
    {
        furniture = ((FurnitureComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(FurnitureComponent)))
            .Where(f => f.dirty)
            .ToArray();

    }

    void Update()
    {

    }

    public HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData =  new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("hasDuster", (duster != null)));
        worldData.Add(new KeyValuePair<string, object>("dirtyFurnitureCount", GetDirtyFurnitureCount()==0));
        Debug.Log(DebugHashSet(worldData));
        return worldData;
    }

    public HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		
        goal.Add(new KeyValuePair<string, object>("dirtyFurnitureCount", (GetDirtyFurnitureCount() == 0 )));
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
        float distance = Vector3.Distance(gameObject.transform.position, nextAction.target.transform.position);
		
        if (distance<=2f) {
            // we are at the target location, we are done
            nextAction.setInRange(true);
            return true;
        } else
            return false;
    }

    private float GetDirtyFurnitureCount()
    {
        dirtyFurnCount = furniture.Length;
        return dirtyFurnCount;
    }
    string DebugHashSet(HashSet<KeyValuePair<string, object>> hashSet)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("HashSet contents:");
        foreach (var kvp in hashSet)
        {
            sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
        }

        return sb.ToString();
    }
}
