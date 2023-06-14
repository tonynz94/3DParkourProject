using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    [SerializeField] List<ParkourAction> parkourActions;

    bool inAction;

    EnviromentScanner enviroment;
    PlayerController playerController;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        enviroment = GetComponent<EnviromentScanner>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Space Button
         if(Input.GetButton("Jump") && !inAction)
        {
           var hitData = enviroment.ObstacleCheck();
            if(hitData.forwardHitFound)
            {
                foreach(var action in parkourActions)
                {
                    if(action.CheckIfPossible(hitData, transform))
                    {
                        StartCoroutine(DoParkourAction(action));
                        break;
                    }
                }
            }
        }
    }

    IEnumerator DoParkourAction(ParkourAction action)
    {
        inAction = true;
        playerController.SetControl(false);
        //crossFade를 이용해서 애니메이션을 변경하면 바로 변경되지 않는다. 
        //그래서 다음꺼 애니메이션으로 검사를 해야 아래 StepUp 애니메이션이 선택된다.
        animator.SetBool("mirrorAction", action.Mirror);
        animator.CrossFade(action.AnimName, 0.2f);

        //CrossFade는 이 프레임 끝나고 StepUp으로 이동하게 된다.(그래야 GetNextAnimatorStateInfo에서 StepUp를 검사할 수 있다.
        yield return null; 
        var animState = animator.GetNextAnimatorStateInfo(0);
        if (!animState.IsName(action.AnimName))
            Debug.LogError("animation is wrong");

        float timer = 0f;     
        while(timer <= animState.length)
        {
            timer += Time.deltaTime;

            //Rotate the player to obstcal
            if(action.RotateToObstacle)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation, playerController.RotationSpeed * Time.deltaTime);
            }

            if(action.EnableTargetMatching)
            {
                MatchTarget(action);
            }

            if(animator.IsInTransition(0) && timer > 0.5f)
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(action.PoseActionDelay);
    //---------------------------------
    //여기를 넘어온거는 애니메이션이 끝났다는것
        inAction = false;
        playerController.SetControl(true);
    }

    //한번만 실행해주면 된다.
    void MatchTarget(ParkourAction action)
    {
        if (animator.isMatchingTarget) return;

        //(매치할 위치, 로테이션, 매치할 부위, 적용할(x,y,z), 시작시간, 소요시간) 
        animator.MatchTarget(
            action.MatchPos, 
            transform.rotation, action.MatchBodyPart, 
            new MatchTargetWeightMask(action.MatchPosWeight, 0),
            action.MatchStartTime, 
            action.MatchTargetTime);
    }
}
