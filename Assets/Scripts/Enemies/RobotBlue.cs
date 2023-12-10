using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RobotBlue : MonoBehaviour
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

    [SerializeField] Transform _firePoint;
    [SerializeField] GameObject _bulletPrefab;

    [SerializeField] GameObject _explosionParticle;

    private NavMeshAgent _navMeshAgent;
    private Vector3 _randomDestination;
    private float _idleTimer;
    private float _lastAttackTime;
    Animator _animator;

    bool _moveRandom = false;


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
        if (GameTracker.Instance.IsGameOver())
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        if (Vector3.Distance(transform.position, _player.position) <= _attackRadius && !_moveRandom)
        {
            if (Time.time - _lastAttackTime >= _attackDelay)
            {
                Attack();
                _lastAttackTime = Time.time;
            }

            _navMeshAgent.SetDestination(transform.position);

            _animator.SetBool("Running", false);
            _idleTimer = 0f;
        }
        else if (Vector3.Distance(transform.position, _player.position) <= _detectionRadius && !_moveRandom)
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

                var playerPos = _player.transform.position;
                playerPos.y = transform.position.y;
                transform.LookAt(playerPos);

                if (_idleTimer >= _idleTime)
                {
                    SetRandomDestination();
                    _animator.SetBool("Running", true);
                    _idleTimer = 0f;
                    _moveRandom = false;
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
        var secondsPerBeat = 60 / BeatManager.Instance.GetBPM();
        var fireDir = (_player.transform.GetChild(0).position - transform.position).normalized;

        var playerPos = _player.transform.GetChild(0).position;
        playerPos.y = transform.position.y;
        transform.LookAt(playerPos);

        yield return new WaitForSeconds(Time.time - nextBeatTime);

        var bullet1 = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        bullet1.GetComponent<Bullet>().SetBullet(1, 15, fireDir);

        yield return new WaitForSeconds(secondsPerBeat);

        var bullet2 = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        bullet2.GetComponent<Bullet>().SetBullet(1, 15, fireDir);

        yield return new WaitForSeconds(secondsPerBeat);

        var bullet3 = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        bullet3.GetComponent<Bullet>().SetBullet(1, 15, fireDir);

        _moveRandom = true;

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
        StartCoroutine(DestroyThis());
    }

    IEnumerator DestroyThis()
    {
        Instantiate(_explosionParticle, transform.position + new Vector3(0,1,0), Quaternion.identity);

        yield return new WaitForSeconds(0.1f);
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 1);
        Destroy(gameObject);
    }
}
