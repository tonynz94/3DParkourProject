using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/Custom Action/new vault action")]
public class VaultAction : ParkourAction
{
    public override bool CheckIfPossible(ObstacleHitData hitDate, Transform player)
    {
        if(!base.CheckIfPossible(hitDate, player))
            return false;

        var hitPoint = hitDate.forwardHit.transform.InverseTransformPoint(hitDate.forwardHit.point);
        
        if(hitPoint.x < 0 && hitPoint.z < 0 || hitPoint.z > 0 && hitPoint.x > 0)
        {
            // Mirror Animation
            Mirror = true;
            matchBodyPart = AvatarTarget.RightHand;
        }
        else
        {
            Mirror = true;
            matchBodyPart = AvatarTarget.LeftHand;
        }

        return true;
    }
}
