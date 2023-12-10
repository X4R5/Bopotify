﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossGolem : MonoBehaviour
{
    Transform _player;

    [SerializeField] float _maxHealth = 300f;
    float _currentHealth;

    [SerializeField] float _detectionRadius = 10f;
    [SerializeField] float _attackRadius = 4f;
    [SerializeField] float _idleTime = 5f;
    [SerializeField] float _walkRadius = 5f;
    [SerializeField] float _attackDamage = 10f;
    [SerializeField] float _attackDelay = 1f;

    [SerializeField] float _jumpAttackRadius = 3f;
    [SerializeField] float _punchRadius = 4f;

    [SerializeField] GameObject _punchTrigger;
    [SerializeField] GameObject _jumpAttackTrigger;

    [SerializeField] GameObject _explosionParticle;


    private NavMeshAgent _navMeshAgent;
    private Vector3 _randomDestination;
    private float _idleTimer;
    private float _lastAttackTime;
    Animator _animator;
    bool _isDied = false;



    private float _valChangeDelay = 2f;


    private void Awake()
    {
        _currentHealth = _maxHealth;
        _animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomDestination();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator.SetBool("Walking", true);
    }

    void Update()
    {
        if (GameTracker.Instance.IsGameOver()) return;

        if (_isDied)
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        if (_navMeshAgent.isStopped)
        {
            _animator.SetBool("Walking", false);
            var lookPos = _player.transform.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
            return;
        }

        if (Vector3.Distance(transform.position, _player.position) <= _attackRadius)
        {
            if (Time.time - _lastAttackTime >= _attackDelay)
            {
                var randomAttack = Random.Range(0f, 10f);
                if (randomAttack < 5f)
                {
                    JumpAttack();
                }
                else
                {
                    Punch();
                }
            }

                _navMeshAgent.SetDestination(transform.position);

            _animator.SetBool("Walking", false);
            _idleTimer = 0f;
        }
        else if (Vector3.Distance(transform.position, _player.position) <= _detectionRadius)
        {
            _navMeshAgent.SetDestination(_player.position);
            _animator.SetBool("Walking", true);
            _idleTimer = 0f;

            if (Vector3.Distance(transform.position, _player.position) <= _jumpAttackRadius && Time.time > _valChangeDelay)
            {
                var val = Random.Range(0, 10);
                _valChangeDelay = Time.time + 2f;
                if (val <= 2)
                {
                    JumpAttack();
                }
            }
        }
        else
        {

            if (_navMeshAgent.remainingDistance < 0.2f)
            {
                _idleTimer += Time.deltaTime;
                _animator.SetBool("Walking", false);

                if (_idleTimer >= _idleTime)
                {
                    SetRandomDestination();
                    _animator.SetBool("Walking", true);
                    _idleTimer = 0f;
                }
            }
            else
            {
                _animator.SetBool("Walking", true);
            }
        }
    }

    private void Punch()
    {
        _attackDelay = 4f;
        _lastAttackTime = Time.time;
        StartCoroutine(PunchCoroutine());
    }

    IEnumerator PunchCoroutine()
    {
        _navMeshAgent.isStopped = true;
        _animator.SetTrigger("Punch");

        yield return new WaitForSeconds(1.3f);
        if (_isDied) yield break;
        _punchTrigger.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        _punchTrigger.SetActive(false);

        yield return new WaitForSeconds(2.3f);
        _navMeshAgent.isStopped = false;

    }

    private void JumpAttack()
    {
        _attackDelay = 6f;
        _lastAttackTime = Time.time;
        StartCoroutine(JumpAttackCoroutine());
    }

    IEnumerator JumpAttackCoroutine()
    {
        _animator.SetTrigger("JumpAttack");

        yield return new WaitForSeconds(1f);
        _navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(0.6f);
        if (_isDied) yield break;
        _jumpAttackTrigger.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        _jumpAttackTrigger.SetActive(false);

        yield return new WaitForSeconds(3.3f);
        _navMeshAgent.isStopped = false;
    }

    void SetRandomDestination()
    {
        Vector2 randomDirection = Random.insideUnitCircle * _walkRadius;
        _randomDestination = new Vector3(transform.position.x + randomDirection.x, transform.position.y, transform.position.z + randomDirection.y);

        _navMeshAgent.SetDestination(_randomDestination);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDied) return;

        if (other.CompareTag("Bullet"))
        {
            var bulletDamage = other.GetComponent<Bullet>().GetDamage();

            TakeDamage(bulletDamage);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("DashInstantDmg"))
        {
            TakeDamage(other.GetComponent<DashUpgradeInstantDmg>().GetDamage());
        }

    }

    private void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log("Current Health: " + _currentHealth);
        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        _animator.SetTrigger("Die");
        LevelManager.Instance.EnemyKilled();
        Instantiate(_explosionParticle, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        _isDied = true;
    }
}
