using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New parkour action")]
public class ParkourAction : ScriptableObject
{
    [SerializeField] string animName;

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;

    [SerializeField] bool rotateToObstacle;

    public Quaternion TargetRotation { get; set; }

    public bool CheckIfPossible(ObstacleHitData hitDate, Transform player)
    {
        float height = hitDate.heightHit.point.y - player.position.y;
        if (height < minHeight || height > maxHeight)
            return false;

        if(rotateToObstacle)
        {
            TargetRotation = Quaternion.LookRotation(-hitDate.forwardHit.normal);
        }

        return true;
    }

    public string AnimName => animName;
    public bool RotateToObstacle => rotateToObstacle;
}
