using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidBehaviourTree : BehaviourTree
{
    public override void CreateTree()
    {
        blackBoard.target = PlayerInformation.Instance()?.GetTransform();
        blackBoard.owner = transform;
        blackBoard.agent = GetComponent<NavMeshAgent>();
        root = new Sequence(blackBoard,
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
                        new InvertResult(new Selector(
                            new DetectTargetBySelf(),
                            new DetectTargetInGroup()
                            )),
                        new Sequence(
                            new ReceiveCommand("attack"),
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
                            new ReceiveCommand("move"),
                            new Move()
                            ),
                        new Sequence(
                            new ReceiveCommand("search"),
                            new Searching()
                            )
                        )
            );
        root.Init();
    }
    private void Start()
    {
        
    }
}
