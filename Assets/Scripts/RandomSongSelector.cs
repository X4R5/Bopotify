using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSongSelector : MonoBehaviour
{
    public static RandomSongSelector Instance;

    List<SongSO> _allSongs = new List<SongSO>();
    List<SongSO> _songsToPick = new List<SongSO>();

    [SerializeField] AudioClip audioClip;
    [SerializeField] float bpm;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        var songSOs = Resources.LoadAll<SongSO>("SongSO");
        _allSongs.AddRange(songSOs);
        _songsToPick.AddRange(songSOs);
        _songsToPick.Shuffle();
    }

    public SongSO GetRandomSongSO()
    {
        if (audioClip != null)
        {
            SongSO newSO = ScriptableObject.CreateInstance<SongSO>();
            newSO._audioClip = audioClip;
            newSO._bpm = bpm;
            newSO._songName = "d";
            return newSO;
        }

        var so = _songsToPick[0];
        _songsToPick.RemoveAt(0);
        return so;
    }
}
