using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{

    private TextMeshProUGUI _fieldCounter;
    private TextMeshProUGUI _bagCounter;

    private Animator _startButtonAnim;
    private Animator _mainLabelAnim;
    private TextMeshProUGUI _mainLabelText;

    public List<GameObject> places;

    private TextMeshProUGUI _currentScore;

    private List<GameObject> _refreshTokens;
    private Button _refreshButton;

    public UIManager(GameObject canvas)
    {
        places = new List<GameObject>();
        Transform scorePanel = canvas.transform.GetChild(0);
        for (int i = 0; i < 7; ++i)
        {
            places.Add(scorePanel.GetChild(i).gameObject);
        }
        _bagCounter = scorePanel.GetChild(scorePanel.childCount - 1).GetComponent<TextMeshProUGUI>();
        _fieldCounter = scorePanel.GetChild(scorePanel.childCount - 2).GetComponent<TextMeshProUGUI>();

        _currentScore = scorePanel.GetChild(7).GetChild(1).GetComponent<TextMeshProUGUI>();

        _refreshTokens = new List<GameObject>();
        Transform refreshPanel = scorePanel.GetChild(8);
        for (int i = 0; i < 3; ++i)
        {
            _refreshTokens.Add(refreshPanel.GetChild(i).gameObject);
        }

        _refreshButton = scorePanel.GetChild(9).gameObject.GetComponent<Button>();

        _startButtonAnim = canvas.transform.GetChild(1).GetComponent<Animator>();
        _mainLabelAnim = canvas.transform.GetChild(2).GetComponent<Animator>();
        _mainLabelText = canvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        PlayButtonAnim(true);
        PlayLabelAnim(true);
    }

    public void SetScorePoints(int score)
    {
        _currentScore.text = score.ToString("D7");
    }

    public void SetRefreshTokens(int count)
    {
        for (int i = 0; i < _refreshTokens.Count; ++i)
        {
            if (i < count)
            {
                _refreshTokens[i].SetActive(true);
            }
            else
            {
                _refreshTokens[i].SetActive(false);
            }
        }
        _refreshButton.interactable = (count != 0);
    }

    public void PlayButtonAnim(bool FadeIn)
    {
        if (FadeIn)
        {
            _startButtonAnim.Play("FadeIn");
        }
        else
        {
            _startButtonAnim.Play("FadeOut");
        }
    }

    public void PlayLabelAnim(bool FadeIn)
    {
        if (FadeIn)
        {
            _mainLabelAnim.Play("FadeIn");
        }
        else
        {
            _mainLabelAnim.Play("FadeOut");
        }
    }

    public void SetLabelText(string txt)
    {
        _mainLabelText.text = txt;
    }

    public void RefreshCounters(int field, int bag)
    {
        _fieldCounter.text = field.ToString();
        _bagCounter.text = bag.ToString();
    }

    public void RefreshScoreBar(List<GameObject> shapes)
    {
        int i = 0;
        foreach (GameObject shape in shapes)
        {
            shape.transform.position = places[i].transform.position;
            shape.transform.localScale = Vector3.one * 160f;
            places[i].SetActive(false);
            i += 1;
        }
        for (; i < places.Count; ++i)
        {
            places[i].SetActive(true);
        }
    }
}