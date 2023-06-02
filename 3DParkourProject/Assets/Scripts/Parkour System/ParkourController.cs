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
        //crossFade�� �̿��ؼ� �ִϸ��̼��� �����ϸ� �ٷ� ������� �ʴ´�. 
        //�׷��� ������ �ִϸ��̼����� �˻縦 �ؾ� �Ʒ� StepUp �ִϸ��̼��� ���õȴ�.
        animator.CrossFade(action.AnimName, 0.2f);

        //CrossFade�� �� ������ ������ StepUp���� �̵��ϰ� �ȴ�.(�׷��� GetNextAnimatorStateInfo���� StepUp�� �˻��� �� �ִ�.
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

            yield return null;
        }

    //---------------------------------
    //���⸦ �Ѿ�°Ŵ� �ִϸ��̼��� �����ٴ°�
        inAction = false;
        playerController.SetControl(true);
    }
}
