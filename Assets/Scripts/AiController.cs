using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    Animator Animator;
    public GameObject Target;
    Vector3 targetPoint;
    public bool isChase = false;
    [Space]
    public bool IsAttack = false;
    public float Movespeed = 5;
    Vector3 CurrentPos;
    public float DistanceToChase = 5f;
    public float DistanceToLose = 10f;

    public float DistanceToAttack = 3f;
    public float DistanceToStop = 2f;
    Vector3 StartPos;

    public PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //assign enemy target
        targetPoint = player.transform.position;

        //find distance between enemy and play
        float Dist = Vector3.Distance(transform.position,Target.transform.position);
        Debug.Log(Dist);
        if(!isChase)
        {
            //compare distance to player to concider chasing
            if(Dist < DistanceToChase)
            {
                //begins to chase player
                isChase = true;
                //transform.LookAt(Target.transform.position);
                FaceTarget();
            }
            else
            {
                //enemy returns to original pos
                transform.position = Vector3.MoveTowards(transform.position,StartPos,Movespeed*Time.deltaTime);
            }
           
        }
        else
        {
            if(Vector3.Distance(transform.position, targetPoint)<DistanceToChase)
            {
                FaceTarget();
                transform.position = Vector3.MoveTowards(transform.position,Target.transform.position,Movespeed*Time.deltaTime);
            }
          
            if(Dist > DistanceToLose)
            {
                //enemy stops chsing
                isChase = false;
                targetPoint = StartPos;
                FaceTarget();
                transform.position = Vector3.MoveTowards(transform.position,StartPos,Movespeed*Time.deltaTime);
            }
            else
            {
                targetPoint = player.transform.position;
            }

            if(Dist < DistanceToStop)
            {
                //enemy stops and becomes idle
                isChase = false;

            }
            else
            {
                //enemy continues following
                isChase = true;
            }

            if(Dist < DistanceToAttack)
            {
                //Enemy Attacks
                IsAttack = true;
                if(IsAttack)
                {
                    //attack player
                    //Animator.SetLayerWeight(1,1);

                }

            }
            else
            {
                //stops attacking player
                IsAttack = false;

            }
        }
    }

    void FaceTarget()
    {
        // enemy rotation to face player
        Vector3 direction = (targetPoint - transform.position).normalized;
        Quaternion lookRotation =Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
