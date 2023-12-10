using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    [SerializeField] int _maxHealth = 10;
    int _currentHealth;

    [SerializeField] float _moveSpeed;
    [SerializeField] float _perfectDashSpeed, _goodDashSpeed;
    [SerializeField] float _dashDuration;
    [SerializeField] float _rotationSpeed;
    [SerializeField] GameObject _trail;

    float _damageDecreasePercent;
    bool _isDashing;

    Animator _animator;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        Move();
        DashCheck();
        LookAtMouse();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackTrigger"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        _animator.SetTrigger("Hit");
        _currentHealth--;
    }

    void DashCheck()
    {
        if (!BeatManager.Instance.CanAction()) return;

        if (Input.GetKeyDown(KeyCode.Space) && !_isDashing)
        {
            var result = BeatManager.Instance.GetBeatResult();

            switch (result)
            {
                case BeatResult.Miss:
                    break;
                default:
                    StartCoroutine(Dash(result));
                    break;
            }
        }
    }

    void Move()
    {
        if (_isDashing) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        SetAnimatorBools(horizontal, vertical);

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 movement = inputDir * _moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    void LookAtMouse()
    {
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float hitDistance))
        {
            Vector3 targetPoint = ray.GetPoint(hitDistance);
            Vector3 lookDir = targetPoint - transform.position;
            lookDir.y = 0f;

            if (lookDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }
    }



    void SetAnimatorBools(float horizontal, float vertical)
    {
        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        if (isMoving)
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            forward.y = 0f;
            right.y = 0f;

            Vector3 inputDir = (forward * vertical + right * horizontal).normalized;

            bool isMovingForward = Vector3.Dot(transform.forward, inputDir) > 0.5f;
            bool isMovingBackward = Vector3.Dot(transform.forward, inputDir) < -0.5f;
            bool isMovingRight = Vector3.Dot(transform.right, inputDir) > 0.5f;
            bool isMovingLeft = Vector3.Dot(transform.right, inputDir) < -0.5f;


            _animator.SetBool("Forward", isMovingForward);
            _animator.SetBool("Back", isMovingBackward);
            _animator.SetBool("Right", isMovingRight);
            _animator.SetBool("Left", isMovingLeft);
        }
        else
        {
            _animator.SetBool("Forward", false);
            _animator.SetBool("Back", false);
            _animator.SetBool("Right", false);
            _animator.SetBool("Left", false);
        }
    }







    IEnumerator Dash(BeatResult r)
    {
        _isDashing = true;
        _trail.SetActive(true);

        CheckDashEffectOnStart();

        var dashSpeed = r == BeatResult.Perfect ? _perfectDashSpeed : _goodDashSpeed;

        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        Vector3 dashDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * inputDir;

        float startTime = Time.time;

        while (Time.time < startTime + _dashDuration)
        {
            Vector3 dashMovement = dashDirection * dashSpeed * Time.deltaTime;

            if (!CheckCollision(dashMovement))
            {
                transform.Translate(dashMovement, Space.World);
            }

            yield return null;
        }

        _isDashing = false;
        _trail.SetActive(false);

        CheckDashEffectOnEnd();
    }

    private void CheckDashEffectOnStart()
    {
        var allUpgrades = PlayerUpgradeManager.Instance.GetUpgrades();

        foreach (var upgrade in allUpgrades)
        {
            if (upgrade._dashUpgrade._explosionPrefabOnStart != null)
            {
                var upg = Instantiate(upgrade._dashUpgrade._explosionPrefabOnStart, transform.position, Quaternion.identity);
                upg.GetComponent<DashUpgradeInstantDmg>().SetDamage(upgrade._dashUpgrade._explosionPrefabOnStartDamage);
                return;
            }
        }
    }

    private void CheckDashEffectOnEnd()
    {
        var allUpgrades = PlayerUpgradeManager.Instance.GetUpgrades();

        foreach (var upgrade in allUpgrades)
        {
            if (upgrade._dashUpgrade._explosionPrefabOnEnd != null)
            {
                var upg = Instantiate(upgrade._dashUpgrade._explosionPrefabOnEnd, transform.position, Quaternion.identity);
                upg.GetComponent<DashUpgradeInstantDmg>().SetDamage(upgrade._dashUpgrade._explosionPrefabOnEndDamage);
                return;
            }
        }
    }

    bool CheckCollision(Vector3 movement)
    {
        Vector3 startPos = transform.position + new Vector3(0, 0.1f, 0);

        RaycastHit hit;
        if (Physics.Raycast(startPos, movement.normalized, out hit, movement.magnitude))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateStats(float moveSpeedIncreasePercent, float damageDecreaseIncreasePercent , float perfectDashSpeedIncreasePercent, float goodDashSpeedIncreasePercent)
    {
        _moveSpeed *= (1 + moveSpeedIncreasePercent / 100f);

        _perfectDashSpeed *= (1 + perfectDashSpeedIncreasePercent / 100f);

        _goodDashSpeed *= (1 + goodDashSpeedIncreasePercent / 100f);

        _damageDecreasePercent *= (1 + damageDecreaseIncreasePercent / 100f);
    }

}
