using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
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


    private NavMeshAgent _navMeshAgent;
    private Vector3 _randomDestination;
    private float _idleTimer;
    private float _lastAttackTime;
    Animator _animator;

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

    protected virtual void Attack()
    {
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, _player.position) <= _attackRadius)
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
        else if (Vector3.Distance(transform.position, _player.position) <= _detectionRadius)
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

            PlayerUpgradeManager.Instance.AddXp(2);

            Destroy(other.gameObject);
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
        PlayerUpgradeManager.Instance.AddXp(10);
        Destroy(gameObject);
    }
}
