using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public CardBehaviour Card;
    public Target CurrentTarget = Target.Off;
    public bool HaveCard = false;
    public GameObject CurrentCardPlace;
    public GameBehaviour GameBehaviourScript;

    public void ChangeTarget(int targetNum)
    {
        Target target = Target.Off;
        if (targetNum == 0)
        {
            target = Target.Hand;
        }
        else if (targetNum == 1)
        {
            target = Target.Defend;
        }
        else if (targetNum == 2)
        {
            target = Target.Attack;
        }
        else if (targetNum == 3)
        {
            target = Target.Off;
        }
        else if (targetNum == 4)
        {
            target = Target.Other;
        }

        CurrentTarget = target;
    }
}

public enum Target
{
    Hand,
    Defend,
    Attack,
    Off,
    Other
}

