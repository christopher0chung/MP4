using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelObjectInteraction : MonoBehaviour {

    public Dictionary<Thing, GameObject> RegisteredInteractableObject;

    public Thing p0_InteractableInterested { get; private set; }
    public Thing p1_InteractableInterested { get; private set; }

    public Thing p0_InteractableGrabbed { get; private set; }
    public Thing p1_InteractableGrabbed { get; private set; }

    //Array so that it accepts output of spherecast
    public Thing[] p0_Interactable_ObjsOfConcern { get; private set; }
    public Thing[] p1_Interactable_ObjsOfConcern { get; private set; }

    public void SetInteractableInterested (ServiceLocator.ID id, Thing interactable)
    {
        if (id == ServiceLocator.ID.p0)
            p0_InteractableInterested = interactable;
        else
            p1_InteractableInterested = interactable;
    }

    public void SetOOCs (ServiceLocator.ID id, Thing[] OOCs)
    {
        if (id == ServiceLocator.ID.p0)
            p0_Interactable_ObjsOfConcern = OOCs;
        else
            p1_Interactable_ObjsOfConcern = OOCs;
    }

    public void SetGrabbed (ServiceLocator.ID id, Thing grabbedThing)
    {
        if (id == ServiceLocator.ID.p0)
        {
            p0_InteractableGrabbed = grabbedThing;
            //Debug.Log("Hello! This is the model. The grabbed thing is " + grabbedThing.ToString());
        }
        else
            p1_InteractableGrabbed = grabbedThing;
    }

    public void SetDrop (ServiceLocator.ID id)
    {
        if (id == ServiceLocator.ID.p0)
            p0_InteractableGrabbed = null;
        else
            p1_InteractableGrabbed = null;
    }

    public Vector3 WhereIsThing(Thing thing)
    {
        Debug.Assert(RegisteredInteractableObject.ContainsKey(thing), "Asking for location of unregistered item");

        GameObject thingGO;
        RegisteredInteractableObject.TryGetValue(thing, out thingGO);

        return thingGO.transform.position;
    }

    public GameObject BodyOfThing(Thing thing)
    {
        Debug.Assert(RegisteredInteractableObject.ContainsKey(thing), "Asking for gameobject of unregistered item");

        GameObject thingGO;
        RegisteredInteractableObject.TryGetValue(thing, out thingGO);

        return thingGO;
    }

    public int CountOfTypeInOOC (ServiceLocator.ID id, ServiceLocator.Interactives type)
    {
        if (id == ServiceLocator.ID.p0)
        {
            if (p0_Interactable_ObjsOfConcern.Length == 0)
                return 0;
            else
            {
                int counter = 0;
                for (int i = 0; i < p0_Interactable_ObjsOfConcern.Length; i++)
                {
                    if (p0_Interactable_ObjsOfConcern[i].type == type)
                        counter++;
                }
                return counter;
            }
        }
        else
        {
            if (p1_Interactable_ObjsOfConcern.Length == 0)
                return 0;
            else
            {
                int counter = 0;
                for (int i = 0; i < p1_Interactable_ObjsOfConcern.Length; i++)
                {
                    if (p1_Interactable_ObjsOfConcern[i].type == type)
                        counter++;
                }
                return counter;
            }
        }
    }

    public Thing ClosestThingOfTypeInOOC (ServiceLocator.ID id, ServiceLocator.Interactives type)
    {
        Thing[] examinedOOC;
        Thing examinedObjectOfInterest;

        if (id == ServiceLocator.ID.p0)
        {
            examinedOOC = p0_Interactable_ObjsOfConcern;
            examinedObjectOfInterest = p0_InteractableInterested;
        }
        else
        {
            examinedOOC = p1_Interactable_ObjsOfConcern;
            examinedObjectOfInterest = p1_InteractableInterested;
        }

        // if there's nothing, closest is nothing
        if (examinedOOC.Length == 0)
            return null;
        // if there's something in the list, but none of the right type, closest is nothing
        else if (CountOfTypeInOOC(id, type) == 0)
            return null;
        // if there's one of type, then the closest is only one
        else if (CountOfTypeInOOC(id, type) == 1)
        {
            for (int i = 0; i < examinedOOC.Length; i++)
            {
                if (examinedOOC[i].type == type)
                    return examinedOOC[i];
            }
        }
        // if there's more than one of type, then the closest must be found in order to be returned
        else
        {
            List<Thing> allOfTypeInOOC = new List<Thing>();

            for (int i = 0; i < examinedOOC.Length; i++)
            {
                if (examinedOOC[i].type == type)
                    allOfTypeInOOC.Add(examinedOOC[i]);
            }

            float shortestDist = 9999999;
            Thing currentClosestThing = null;
            for (int i = 0; i < allOfTypeInOOC.Count; i++)
            {
                Vector3 pos1 = WhereIsThing(examinedObjectOfInterest);
                Vector3 pos2 = WhereIsThing(allOfTypeInOOC[i]);
                if (Vector3.Distance(pos1, pos2) < shortestDist)
                    currentClosestThing = allOfTypeInOOC[i];
            }

            return currentClosestThing;
        }
        return null;
    }

    //public void Update()
    //{
    //    if (p0_InteractableInterested != null)
    //        Debug.Log("Interactable is a " + p0_InteractableInterested.type + ". The number of OOCs is " + p0_Interactable_ObjsOfConcern.Length + ".");
    //}
}
