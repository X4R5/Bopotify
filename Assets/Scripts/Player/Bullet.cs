using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _bulletSpeed;

    float _dmg, _range;
    float _traveledDistance;

    Vector3 _dir;

    private void Update()
    {
        transform.Translate(_dir * Time.deltaTime * _bulletSpeed);

        _traveledDistance += Time.deltaTime;

        if (_traveledDistance >= _range)
        {
            Destroy(gameObject);
        }
    }

    public void SetBullet(float dmg, float range, Vector3 dir)
    {
        _dmg = dmg;
        _range = range;
        _dir = dir.normalized;
        _traveledDistance = 0f;
    }
}
