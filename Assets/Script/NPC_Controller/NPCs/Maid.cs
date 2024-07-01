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
    private PlantComponent[] plants;
    private float dirtyFurnCount = 0;
    private float thirstyPlants;
    public Animator maid_anim;
    private List<HashSet<KeyValuePair<string, object>>> goals;
    private int currentGoalIndex;

    void Start()
    {
       
        maid_anim = GetComponent<Animator>();
        goals = new List<HashSet<KeyValuePair<string, object>>>();
        HashSet<KeyValuePair<string, object>> firstGoal = new HashSet<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("dirtyFurnitureCount", false)
        };
        HashSet<KeyValuePair<string, object>> secondGoal = new HashSet<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("plantThirsty", false)
        };
        goals.Add(firstGoal);
        goals.Add(secondGoal);
        currentGoalIndex = 0;
    }

    void Update()
    {

    }

    public HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData =  new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("hasDuster", (duster != null)));
        worldData.Add(new KeyValuePair<string, object>("hasWaterCan", true));
        worldData.Add(new KeyValuePair<string, object>("dirtyFurnitureCount", GetDirtyFurnitureCount()>0));
        worldData.Add(new KeyValuePair<string, object>("plantThirsty", GetThirstyPlantsCount() > 0));
        Debug.Log(DebugHashSet(worldData));
        return worldData;
    }

    public HashSet<KeyValuePair<string, object>> createGoalState()
    {
        if (currentGoalIndex < goals.Count)
        {
            HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> (goals[currentGoalIndex]);
            return goal;
        }

        return null;

        //goal.Add(new KeyValuePair<string, object>("dirtyFurnitureCount", false));
        //goal.Add(new KeyValuePair<string, object>("plantThirsty", false));
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
        if (GetDirtyFurnitureCount() == 0)
        {
            currentGoalIndex++;
        }
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
        //Vector3 lastpos = gameObject.transform.position;
        Vector3 targetpos = new Vector3(nextAction.target.transform.position.x, gameObject.transform.position.y, nextAction.target.transform.position.z);
        Vector3 movementDirection = (targetpos - transform.position).normalized;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetpos, step);
        maid_anim.SetBool("isMoving",true);
        maid_anim.SetFloat("Horizontal",movementDirection.x);
        maid_anim.SetFloat("Vertical",movementDirection.z);
        
       
        
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
        furniture = ((FurnitureComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(FurnitureComponent)))
            .Where(f => f.dirty)
            .ToArray();
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
    private float GetThirstyPlantsCount()
    {
        plants = ((PlantComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(PlantComponent)))
            .Where(f => f.thirsty)
            .ToArray();
        thirstyPlants = plants.Length;
        return thirstyPlants;
    }
}
