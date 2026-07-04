using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlimeIdle : StateMachineBehaviour
{
    public float speed = 2.5f;

    private Transform player;
    private Rigidbody2D rb;
    private float minX;
    private float maxX;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        EnemyAI AI = animator.GetComponent<EnemyAI>();
        minX = AI.minX;
        maxX = AI.maxX;
        animator.SetBool("MoveLeft", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool moveLeft = animator.GetBool("MoveLeft");

        if (rb.position.x <= minX)
            animator.SetBool("MoveLeft", false);

        if (rb.position.x >= maxX)
            animator.SetBool("MoveLeft", true);

        if (moveLeft)
        {
            Vector2 target = new Vector2(minX, rb.position.y);
            rb.position = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (!moveLeft)
        {
            Vector2 target = new Vector2(maxX, rb.position.y);
            rb.position = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

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
