using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUpgradeInstantDmg : MonoBehaviour
{
    float _dmg;

    private void OnEnable()
    {
        Invoke("DestroySelf", 0.1f);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public void SetDamage(float damage)
    {
        _dmg = damage;
    }

    public float GetDamage()
    {
        return _dmg;
    }
}
