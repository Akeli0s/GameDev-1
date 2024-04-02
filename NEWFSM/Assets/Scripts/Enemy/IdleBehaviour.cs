using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    private AgrroTrigger agrroTrigger;

    public EnemyMovement enemymovement;

    public Vector3 targetPos;
    public bool waiting;
    public float waitTimer;
    private Rigidbody2D RB;

    public float RandomMovementSpeed = 2;

    public float horizontal;

    public bool isFacingRight = true;

    private Transform Player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        agrroTrigger = animator.gameObject.GetComponentInChildren<AgrroTrigger>();

        enemymovement = animator.gameObject.GetComponent<EnemyMovement>();

        targetPos = GetRandomPointBetween(enemymovement.PA.position, enemymovement.PB.position);
        ;
        waiting = false;

        RB = animator.gameObject.GetComponent<Rigidbody2D>();

        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        horizontal = RB.velocity.x;
        if (agrroTrigger.isAggroed)
        {
            animator.SetBool("isChasing", true);
            if (Player.transform.position.x < animator.transform.position.x && isFacingRight)
            {
                Flip(animator);
            }
            else if (Player.transform.position.x > animator.transform.position.x && !isFacingRight)
            {
                Flip(animator);
            }
        }
        else
        {
            animator.SetBool("isChasing", false);
        }
        if (waiting)
        {
            RB.velocity = Vector2.zero;
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                waiting = false; // Reset waiting to false here
                // animator.Play("Idle");
                targetPos = GetRandomPointBetween(
                    enemymovement.PA.position,
                    enemymovement.PB.position
                );
            }
        }
        else
        {
            Vector3 direction = (targetPos - animator.transform.position).normalized;
            RB.velocity = direction * RandomMovementSpeed;
            if ((animator.transform.position - targetPos).sqrMagnitude < 0.01f)
            {
                // Start waiting

                waiting = true;
                // animator.Play("Stop");

                waitTimer = 3f;
            }
        }

        if (horizontal > 0 && isFacingRight == false)
        {
            Flip(animator);
        }
        else if (horizontal < 0 && isFacingRight == true)
        {
            Flip(animator);
        }
    }

    public void Flip(Animator animator)
    {
        animator.transform.Rotate(0f, 180f, 0f);
        isFacingRight = !isFacingRight;
    }

    public Vector3 GetRandomPointBetween(Vector3 pointA, Vector3 pointB)
    {
        float randomX = Random.Range(pointA.x, pointB.x);
        float randomY = Random.Range(pointA.y, pointB.y);
        return new Vector3(randomX, randomY, pointA.z); // Keeping the Z coordinate the same as point A
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    ) { }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
