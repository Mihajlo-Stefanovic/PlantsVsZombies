using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlienMoonwalker : Alien
{

    private void Awake() {
        base.Awake();

        SetAttackAnim = delegate (bool isAttacking)
        {
            // Debug.Log("attacking animation: " + isAttacking);
            if (isAttacking) 
            {
                animator.SetBool("Move", false);
                animator.SetBool("Attack", true);
            }
            else 
            {
                animator.SetBool("Attack", false);
                animator.SetBool("Move", true);
            }
        };

        PlayDeathAnim = delegate ()
        {
            // Debug.Log("death animation");
            animator.SetBool("Death", true);
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        _startVar = transform.position;
        gridManager = GridManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEverything();
        moonWalk();
    }
}
