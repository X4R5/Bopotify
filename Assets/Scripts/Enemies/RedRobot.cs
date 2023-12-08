using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RedRobot : EnemyAI
{
    Animator _rAnimator;
    NavMeshAgent _agent;
    [SerializeField] GameObject _attackTrigger;

    private void Awake()
    {
        _rAnimator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void Attack()
    {
        base.Attack();
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        var nextBeatTime = BeatManager.Instance.GetNextBeatTime();

        while (Time.time < nextBeatTime)
        {
            _agent.isStopped = true;
        }

        _rAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(1f);

        _attackTrigger.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        _attackTrigger.SetActive(false);

        yield return null;
    }
}
