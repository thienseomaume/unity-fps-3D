using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackBoard 
{
    public Transform owner;
    public Transform target;
    public NavMeshAgent agent;
    public bool alreadyAim=false;
    public Vector3 lastTargetPos = Vector3.zero;
    public Dictionary<string, object> sharedVariables = new();
    public BlackBoard()
    {

    }
    public BlackBoard(Transform owner, Transform target, NavMeshAgent agent)
    {
        this.owner = owner;
        this.target = target;
        this.agent = agent;
    }
    public T Get<T>(string key)
    {
        if(sharedVariables.TryGetValue(key, out object value))
        {
            return (T)value;
        }
        throw new Exception(key + " varibale not found");
    }
    public void Set<T>(string key, T value)
    {
        sharedVariables[key] = value;
    }
}
