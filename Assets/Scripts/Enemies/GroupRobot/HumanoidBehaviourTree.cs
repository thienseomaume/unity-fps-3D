using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidBehaviourTree : BehaviourTree
{
    public Transform target;
    public override void CreateTree()
    {
        blackBoard = new BlackBoard();
        if (target != null)
        {
            blackBoard.target = target;
        }
        else
        {
            blackBoard.target = PlayerInformation.Instance()?.GetTransform();
        }
        blackBoard.owner = transform;
        blackBoard.agent = GetComponent<NavMeshAgent>();
        root = new Selector(
                    new Sequence(
                        new LeaderOrSingle(),
                        new Selector(
                            new Sequence(
                                new Selector(
                                    new DetectTargetBySelf(),
                                    new DetectTargetInGroup()
                                    ),
                                new Selector(
                                    new Sequence(
                                        new NotReadyAim(),
                                        new Aim()
                                        ),
                                    new Sequence(
                                        new RotateToTarget(),
                                        new Attack()
                                        )
                                    )
                                ),
                            new Sequence(
                                new HasLastPosition(),
                                new MoveToLastTargetPos(),
                                new Searching()
                                ),
                            new Patrolling()
                            )
                        ),
                    new Selector(
                        new Sequence(
                            new ReceiveCommand(GroupCommand.ATTACK),
                            new Selector(
                                    new Sequence(
                                        new NotReadyAim(),
                                        new Aim()
                                        ),
                                    new Sequence(
                                        new RotateToTarget(),
                                        new Attack()
                                        )
                                    )
                            ),
                        new Sequence(
                            new Selector(
                                new ReceiveCommand(GroupCommand.PATROL),
                                new ReceiveCommand(GroupCommand.MOVE_TO_TARGET)
                                ),
                                new Move()
                            ),
                        new Sequence(
                            new ReceiveCommand(GroupCommand.SEARCH),
                            new Searching()
                            )
                        )
            );
        root.Init(blackBoard);
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
