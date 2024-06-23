using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoapPlanner
{
    public Queue<GoapAction> plan(GameObject agent, HashSet<GoapAction> availableActions,
        HashSet<KeyValuePair<string, object>> worldState, HashSet<KeyValuePair<string, object>> goal)
    {
        foreach (GoapAction a in availableActions) // reset each action to calculate a fresh plan
        {
            a.doReset();
        }

        HashSet<GoapAction> usableActions = new HashSet<GoapAction>();
        foreach (GoapAction a in availableActions)
        {
            if (a.checkProceduralPrecondition(agent))
            {
                usableActions.Add(a);
            }
        }
        List<Node> leaves = new List<Node>(); //build tree and record the nodes that provide the solution
        Node start = new Node (null, 0, worldState, null);
        bool success = buildGraph(start, leaves, usableActions, goal);
        if (!success)
        {
            Debug.Log("No plan found. Available actions: " + string.Join(", ", usableActions));
            return null;
        }
        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else
            {
                if (leaf.runningCost < cheapest.runningCost)
                    cheapest = leaf;
            }
        }
        List<GoapAction> result = new List<GoapAction> ();
        Node n = cheapest;
        while (n != null) {
            if (n.action != null) {
                result.Insert(0, n.action); // insert the action in the front
            }
            n = n.parent;
        }
        // we now have this action list in correct order

        Queue<GoapAction> queue = new Queue<GoapAction> ();
        foreach (GoapAction a in result) {
            queue.Enqueue(a);
        }

        // wuhu we have a plan!
        return queue;
    }
    private class Node {
        public Node parent; //root node
        public float runningCost; //cost of action
        public HashSet<KeyValuePair<string,object>> state;
        public GoapAction action;

        public Node(Node parent, float runningCost, HashSet<KeyValuePair<string,object>> state, GoapAction action) {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = state;
            this.action = action;
        }
    }
    private bool buildGraph (Node parent, List<Node> leaves, HashSet<GoapAction> usableActions, HashSet<KeyValuePair<string, object>> goal)
    {
        bool foundOne = false;

        // go through each action available at this node and see if we can use it here
        foreach (GoapAction action in usableActions) {

            // if the parent state has the conditions for this action's preconditions, we can use it here
            if ( inState(action.Preconditions, parent.state) ) {

                // apply the action's effects to the parent state
                HashSet<KeyValuePair<string,object>> currentState = populateState (parent.state, action.Effects);
                //Debug.Log(GoapAgent.prettyPrint(currentState));
                Node node = new Node(parent, parent.runningCost+action.cost, currentState, action);

                if (inState(goal, currentState)) {
                    // we found a solution!
                    leaves.Add(node);
                    foundOne = true;
                } else {
                    // not at a solution yet, so test all the remaining actions and branch out the tree
                    HashSet<GoapAction> subset = actionSubset(usableActions, action);
                    bool found = buildGraph(node, leaves, subset, goal);
                    if (found)
                        foundOne = true;
                }
            }
        }

        return foundOne;
    }
    private bool inState(HashSet<KeyValuePair<string,object>> test, HashSet<KeyValuePair<string,object>> state) {
        bool allMatch = true;
        foreach (KeyValuePair<string,object> t in test) {
            bool match = false;
            foreach (KeyValuePair<string,object> s in state) {
                if (s.Equals(t)) {
                    match = true;
                    break;
                }
            }
            if (!match)
                allMatch = false;
        }
        return allMatch;
    } // check that all items in 'test' are in 'state'. if even one doesn't match it returns false
    private HashSet<KeyValuePair<string,object>> populateState(HashSet<KeyValuePair<string,object>> currentState, HashSet<KeyValuePair<string,object>> stateChange) {
        HashSet<KeyValuePair<string,object>> state = new HashSet<KeyValuePair<string,object>> ();
        // copy the KVPs over as new objects
        foreach (KeyValuePair<string,object> s in currentState) {
            state.Add(new KeyValuePair<string, object>(s.Key,s.Value));
        }

        foreach (KeyValuePair<string,object> change in stateChange) {
            // if the key exists in the current state, update the Value
            bool exists = false;

            foreach (KeyValuePair<string,object> s in state) {
                if (s.Equals(change)) {
                    exists = true;
                    break;
                }
            }

            if (exists) {
                state.RemoveWhere( (KeyValuePair<string,object> kvp) => { return kvp.Key.Equals (change.Key); } );
                KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key,change.Value);
                state.Add(updated);
            }
            // if it does not exist in the current state, add it
            else {
                state.Add(new KeyValuePair<string, object>(change.Key,change.Value));
            }
        }
        return state;
    }
    private HashSet<GoapAction> actionSubset(HashSet<GoapAction> actions, GoapAction removeMe) {
        HashSet<GoapAction> subset = new HashSet<GoapAction> ();
        foreach (GoapAction a in actions) {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }

}