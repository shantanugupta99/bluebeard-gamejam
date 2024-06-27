using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoapAction : MonoBehaviour
{
    private HashSet<KeyValuePair<string, object>> preconditions;
    private HashSet<KeyValuePair<string, object>> effects;

    private bool inRange = false;
    public float cost = 1f;
    public GameObject target;
    
    public GoapAction()
    {
        preconditions = new HashSet<KeyValuePair<string, object>> ();
        effects = new HashSet<KeyValuePair<string, object>> ();
    }
    public void doReset() {
        inRange = false;
        target = null;
        reset ();
    }
    public abstract void reset(); //reset any variables before planning happens again

    public abstract bool isDone(); //is the action done?

    public abstract bool checkProceduralPrecondition(GameObject agent); //a procedural check to see whether the action can be performed or not (not all actions require this)

    public abstract bool performAction(GameObject agent); //run action; if action can execute successfully then return true. if false then goal can no longer be reached due to some condition; clear out action queue
    
    public abstract bool requiresInRange (); //if action is not required to be within range of a game object then moveTo will not run
    
    public bool isInRange () {
        return inRange;
    } //the moveTo state will set this. gets reset each time an action is performed
    
    public void setInRange(bool inRange) {
        this.inRange = inRange;
    }
    
    public void addPrecondition(string key, object value) {
        preconditions.Add (new KeyValuePair<string, object>(key, value) );
    }


    public void removePrecondition(string key) {
        KeyValuePair<string, object> remove = default(KeyValuePair<string,object>);
        foreach (KeyValuePair<string, object> kvp in preconditions) {
            if (kvp.Key.Equals (key)) 
                remove = kvp;
        }
        if ( !default(KeyValuePair<string,object>).Equals(remove) )
            preconditions.Remove (remove);
    }


    public void addEffect(string key, object value) {
        effects.Add (new KeyValuePair<string, object>(key, value) );
    }


    public void removeEffect(string key) {
        KeyValuePair<string, object> remove = default(KeyValuePair<string,object>);
        foreach (KeyValuePair<string, object> kvp in effects) {
            if (kvp.Key.Equals (key)) 
                remove = kvp;
        }
        if ( !default(KeyValuePair<string,object>).Equals(remove) )
            effects.Remove (remove);
    }

	
    public HashSet<KeyValuePair<string, object>> Preconditions {
        get {
            return preconditions;
        }
    }

    public HashSet<KeyValuePair<string, object>> Effects {
        get {
            return effects;
        }
    }



}
