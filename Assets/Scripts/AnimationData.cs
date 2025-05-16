using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationData
{
    public static int IDLE = Animator.StringToHash("Idle");
    public static int WALK = Animator.StringToHash("Walk");
    public static int RUN = Animator.StringToHash("Run");
    public static int LEFT_CLICK = Animator.StringToHash("Left_Click");
    public static int R_PRESS = Animator.StringToHash("R_Press");

    public static int ITEM_LEFT_CLICK = Animator.StringToHash("Left_Click");
    public static int ITEM_RPRESS = Animator.StringToHash("R_Press");
    public static int ITEM_DEFAULT = Animator.StringToHash("DefaultState");

    public static int HUMANOID_IDLE = Animator.StringToHash("Rifle Idle");
    public static int HUMANOID_WALK = Animator.StringToHash("Rifle Walk");
    public static int HUMANOID_RUN = Animator.StringToHash("Rifle Run");
    public static int HUMANOID_START_AIM = Animator.StringToHash("Rifle Down To Aim");
    public static int HUMANOID_IDLE_AIM = Animator.StringToHash("Rifle Aiming Idle");
    public static int HUMANOID_WALK_AIM = Animator.StringToHash("Rifle Aiming Walking");
    public static int HUMANOID_RUN_AIM = Animator.StringToHash("Rifle Aiming Run");
    public static int HUMANOID_IDLE_FIRE = Animator.StringToHash("Rifle Firing Idle");
    public static int HUMANOID_WALK_FIRE = Animator.StringToHash("Rifle Firing Walk");

    public static int HUMANOID_AIM = Animator.StringToHash("Aiming");
    public static int HUMANOID_HOLDING = Animator.StringToHash("Holding");
}
