using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SyncAttackSpeed
{
    public static float GetNewMultiplier(AnimationClip shootingAnimation, float attackSpeed){
        return (1/(shootingAnimation.length * 1/(shootingAnimation.length * attackSpeed)));
    }

}
