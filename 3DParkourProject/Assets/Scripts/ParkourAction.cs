using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New parkour action")]
public class ParkourAction : ScriptableObject
{
    [SerializeField] string animName;

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;

    [Header("Tartget Matching")]
    [SerializeField] bool rotateToObstacle;
    [SerializeField] bool enableTargetMatching = true;
    [SerializeField] AvatarTarget matchBodyPart;
    [SerializeField] float matchStartTime;
    [SerializeField] float matchTargetTime;

    public Quaternion TargetRotation { get; set; }
    public Vector3 MatchPos { get; set; }

    public bool CheckIfPossible(ObstacleHitData hitDate, Transform player)
    {
        float height = hitDate.heightHit.point.y - player.position.y;
        if (height < minHeight || height > maxHeight)
            return false;

        if(rotateToObstacle)
        {
            TargetRotation = Quaternion.LookRotation(-hitDate.forwardHit.normal);
        }

        if(enableTargetMatching)
        {
            MatchPos = hitDate.heightHit.point;
        }

        return true;
    }

    public string AnimName => animName;
    public bool RotateToObstacle => rotateToObstacle;

    public bool EnableTargetMatching => enableTargetMatching;
    public AvatarTarget MatchBodyPart => matchBodyPart;
    public float MatchStartTime => matchStartTime;
    public float MatchTargetTime => matchTargetTime;
}
