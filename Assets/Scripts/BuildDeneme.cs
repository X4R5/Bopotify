using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDeneme : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1f;
        }
    }
}
