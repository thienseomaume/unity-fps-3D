using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourTree : MonoBehaviour
{
    protected BlackBoard blackBoard;
    protected Node root;
    public abstract void CreateTree();
    private void Awake()
    {

    }
    private void Start()
    {
        CreateTree();
    }
    private void Update()
    {
        NodeStatus status = root.Excute();
    }
}
