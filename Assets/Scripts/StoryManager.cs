using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _allPages = new List<GameObject>();

    int _currentPage;
    void Start()
    {
        OpenStory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevPage();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void NextPage()
    {
        if(_currentPage == _allPages.Count - 1)
        {
            return;
        }
        else
        {
            _allPages[_currentPage].SetActive(false);
            _allPages[_currentPage + 1].SetActive(true);
            _currentPage++;
        }
    }

    void PrevPage()
    {
        if (_currentPage == 0)
        {
            return;
        }
        else
        {
            _allPages[_currentPage].SetActive(false);
            _allPages[_currentPage - 1].SetActive(true);
            _currentPage--;
        }
    }

    public void OpenStory()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        _currentPage = 0;

        foreach(var page in _allPages)
        {
            page.SetActive(false);
        }

        _allPages[0].SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("StartRunMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
