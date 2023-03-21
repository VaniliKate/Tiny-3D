using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;
    Animator Animator;
    public GameObject Target;
    public Vector3 targetPoint;
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
        controller = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
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
                Animator.SetBool("Run",true);
                //transform.LookAt(Target.transform.position);
                FaceTarget();
            }
            else{
                isChase = false;
                return;
            }
        }
        else
        {
            if(Vector3.Distance(transform.position, targetPoint) < DistanceToChase)
            {
                FaceTarget();
                Animator.SetBool("Run",true);
                transform.position = Vector3.MoveTowards(transform.position,Target.transform.position,Movespeed*Time.deltaTime);
            }
            else
            {
                isChase= false;
                transform.position = Vector3.MoveTowards(transform.position,StartPos,Movespeed*Time.deltaTime);
            }
          
            if(Vector3.Distance(transform.position, targetPoint) > DistanceToLose)
            {
                //enemy stops chsing
                isChase = false;
                targetPoint = StartPos;
                FaceTarget();
                transform.position = Vector3.MoveTowards(transform.position,StartPos,Movespeed*Time.deltaTime);
                if(Vector3.Distance(transform.position,targetPoint)<.1f)
                {
                    Animator.SetBool("Run",false);
                    Movespeed = 0;
                }
                else{
                    Movespeed = 5;
                    Animator.SetBool("Idle",false);
                }
            }
            else
            {
                isChase =true;
                targetPoint = player.transform.position;
                Animator.SetBool("Run",false);
            }

            if(Vector3.Distance(transform.position,targetPoint) < DistanceToStop )
            {
                //enemy stops and becomes idle
                isChase = false;
                Animator.SetBool("Run",false);
                Movespeed = 0;
            }
            else
            {
                //enemy continues following
                isChase = true;
                Animator.SetBool("Run",true);
                Movespeed = 5;
            }

            if(Vector3.Distance(transform.position,targetPoint) < DistanceToAttack)
            {
                //Enemy Attacks
                isChase = false;
                IsAttack = true;
                Animator.SetBool("Run",false);
                if(IsAttack)
                {
                    //attack player
                    Movespeed = 0;
                    Animator.SetLayerWeight(1,1);
                }
                else{
                    Movespeed = 5;
                }
            }
            else
            {
                //stops attacking player
                IsAttack = false;
                isChase = true;
                Animator.SetBool("Run",true);
            }
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
    }

    void FaceTarget()
    {
        // enemy rotation to face player
        Vector3 direction = (targetPoint - transform.position).normalized;
        Quaternion lookRotation =Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
