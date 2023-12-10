using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance;

    [SerializeField] float _perfectAttackDmg, _goodAttackDmg;
    [SerializeField] float _perfectAttackRange, _goodAttackRange;
    [SerializeField] GameObject _perfectBulletPrefab, _goodBulletPrefab;

    [SerializeField] Transform _firePoint;


    private void Awake()
    {
        Instance = this;

        _perfectAttackDmg = PlayerPrefs.GetFloat("PerfectAttackDmg");
        _goodAttackDmg = PlayerPrefs.GetFloat("GoodAttackDmg");
        _perfectAttackRange = PlayerPrefs.GetFloat("PerfectAttackRange");
        _goodAttackRange = PlayerPrefs.GetFloat("GoodAttackRange");
    }
    private void Update()
    {
        AttackCheck();
    }

    private void AttackCheck()
    {
        if (!BeatManager.Instance.CanAction()) return;

        if (!Input.GetMouseButtonDown(0)) return;

        var result = BeatManager.Instance.GetBeatResult();

        switch (result)
        {
            case BeatResult.Miss:
                Debug.Log("Miss");
                break;
            default:
                Attack(result);
                break;

        }
    }

    private void Attack(BeatResult result)
    {
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Vector3 dir = Vector3.zero;

        if (groundPlane.Raycast(ray, out float hitDistance))
        {
            Vector3 targetPoint = ray.GetPoint(hitDistance);
            dir = targetPoint - transform.position;

        }

        switch (result)
        {
            case BeatResult.Perfect:
                GameObject perfectBullet = Instantiate(_perfectBulletPrefab, _firePoint.position, Quaternion.identity);
                perfectBullet.GetComponent<Bullet>().SetBullet(_perfectAttackDmg, _perfectAttackRange, dir);
                break;
            case BeatResult.Good:
                GameObject goodBullet = Instantiate(_goodBulletPrefab, _firePoint.position, Quaternion.identity);
                goodBullet.GetComponent<Bullet>().SetBullet(_goodAttackDmg, _goodAttackRange, dir);
                break;
        }
    }

    public void UpdateAttackValues(float perfectAttackIncreasePercent, float goodAttackIncreasePercent, float perfectAttackRangeIncreasePercent, float goodAttackRangeIncreasePercent)
    {
        _perfectAttackDmg *= (1 + perfectAttackIncreasePercent / 100f);

        _goodAttackDmg *= (1 + goodAttackIncreasePercent / 100f);

        _perfectAttackRange *= (1 + perfectAttackRangeIncreasePercent / 100f);

        _goodAttackRange *= (1 + goodAttackRangeIncreasePercent / 100f);
    }

}
