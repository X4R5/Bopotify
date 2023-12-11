using UnityEngine;

[CreateAssetMenu(fileName = "SongSO", menuName = "SongSO", order = 1)]
public class SongSO : ScriptableObject
{
    public AudioClip _audioClip;
    public string _songName;
    public float _bpm;
}
