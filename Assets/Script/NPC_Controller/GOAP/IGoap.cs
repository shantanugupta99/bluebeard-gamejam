using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoap
{
   HashSet<KeyValuePair<string, object>> getWorldState(); //supply the states that are needed for actions to run
   HashSet<KeyValuePair<string, object>> createGoalState(); //give the planner a goal so it can figure out the sequence of actions needed
   void planFailed (HashSet<KeyValuePair<string,object>> failedGoal); //plan not found; try another goal
   
   void planFound(HashSet<KeyValuePair<string,object>> goal, Queue<GoapAction> actions); //plan was found for current goal; actions required are returned

   void actionsFinished(); //actions complete; goal reached

   void planAborted(GoapAction aborter); //an action caused plan to abort. said action is returned
   
   public bool moveAgent(GoapAction nextAction); //called during Update. move agent towards target object. return true if at target and action can perform otherwise false

}
