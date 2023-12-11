using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelManager.Instance.IsGameOver())
            {
                GameTracker.Instance.LoadRandomScene();
                BeatManager.Instance.StopMusic();
            }
                
        }
    }
}
