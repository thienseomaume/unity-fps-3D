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
}
