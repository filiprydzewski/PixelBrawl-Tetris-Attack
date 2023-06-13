using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStunning : MonoBehaviour
{
    public int stunDuration = 3;
    public bool isStunned = false;

    public Animator animator;

    private Coroutine regen;

    public void stun()
    {
        if (!isStunned)
        {
            Debug.Log("We stunned");
            isStunned = true;
            animator.SetBool("IsStunned", true);
            StartCoroutine(waitAndUnstun());
        }
    }

    IEnumerator waitAndUnstun()
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
        animator.SetBool("IsStunned", false);
    }

}
