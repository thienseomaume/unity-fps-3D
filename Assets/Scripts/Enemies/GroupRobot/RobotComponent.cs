using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Robot))]
public class RobotComponent : MonoBehaviour
{
    Robot robot;
    private void Awake()
    {
        robot = robot.GetComponent<Robot>();
    }
}
