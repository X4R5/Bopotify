using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashExplosionParticle : MonoBehaviour
{
    [SerializeField] Vector3 _speed;

    void Update()
    {
        transform.Rotate(_speed * Time.deltaTime);
    }
}
