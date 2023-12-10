using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RobotRed : MonoBehaviour
{
    Transform _player;

    [SerializeField] float _maxHealth = 100f;
    float _currentHealth;

    [SerializeField] float _detectionRadius = 10f;
    [SerializeField] float _attackRadius = 2f;
    [SerializeField] float _idleTime = 5f;
    [SerializeField] float _walkRadius = 5f;
    [SerializeField] float _attackDamage = 10f;
    [SerializeField] float _attackDelay = 1f;

    [SerializeField] GameObject _attackTrigger;
    [SerializeField] GameObject _explosionParticle;


    NavMeshAgent _navMeshAgent;
    Vector3 _randomDestination;
    private float _idleTimer;
    float _lastAttackTime;
    Animator _animator;
    bool _attacking = false;

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
        _animator.SetBool("Running", true);
    }

    void Update()
    {
        if (GameTracker.Instance.IsGameOver()) return;

        if (_attacking)
        {
            _navMeshAgent.isStopped = true;
        }
        else
        {
            _navMeshAgent.isStopped = false;
        }

        if (Vector3.Distance(transform.position, _player.position) <= _attackRadius)
        {
            if (Time.time - _lastAttackTime >= _attackDelay)
            {
                var lookPos = _player.transform.position;
                lookPos.y = transform.position.y;
                transform.LookAt(lookPos);
                Attack();
                _lastAttackTime = Time.time;
            }

            _navMeshAgent.SetDestination(transform.position);

            _animator.SetBool("Running", false);
            _idleTimer = 0f;
        }
        else if (Vector3.Distance(transform.position, _player.position) <= _detectionRadius && !_navMeshAgent.isStopped)
        {
            _navMeshAgent.SetDestination(_player.position);
            _animator.SetBool("Running", true);
            _idleTimer = 0f;
        }
        else
        {

            if (_navMeshAgent.remainingDistance < 0.2f)
            {
                _idleTimer += Time.deltaTime;
                _animator.SetBool("Running", false);

                if (_idleTimer >= _idleTime)
                {
                    SetRandomDestination();
                    _animator.SetBool("Running", true);
                    _idleTimer = 0f;
                }
            }
            else
            {
                _animator.SetBool("Running", true);
            }
        }
    }

    void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        var nextBeatTime = BeatManager.Instance.GetNextBeatTime();

        yield return new WaitForSeconds(Time.time - nextBeatTime);

        _animator.SetTrigger("Attack");
        _attacking = true;

        yield return new WaitForSeconds(1f);

        _attackTrigger.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        _attackTrigger.SetActive(false);

        yield return new WaitForSeconds(1.7f);

        _attacking = false;

        yield return null;
    }

    void SetRandomDestination()
    {
        Vector2 randomDirection = Random.insideUnitCircle * _walkRadius;
        _randomDestination = new Vector3(transform.position.x + randomDirection.x, transform.position.y, transform.position.z + randomDirection.y);

        _navMeshAgent.SetDestination(_randomDestination);
    }

    private void OnTriggerEnter(Collider other)
    {
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
        LevelManager.Instance.EnemyKilled();
        Instantiate(_explosionParticle, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        Destroy(gameObject);
    }
}
