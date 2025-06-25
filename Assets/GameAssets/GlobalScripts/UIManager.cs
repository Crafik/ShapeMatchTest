using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
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

    private GameObject _comboPanel;
    private Image _comboProgressBar;
    private TextMeshProUGUI _comboMeter;

    public UIManager(Transform canvas)
    {
        places = new List<GameObject>();
        Transform scorePanel = canvas.GetChild(0);
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

        _startButtonAnim = canvas.GetChild(1).GetComponent<Animator>();
        _mainLabelAnim = canvas.GetChild(2).GetComponent<Animator>();
        _mainLabelText = canvas.GetChild(2).GetComponent<TextMeshProUGUI>();

        _comboPanel = canvas.GetChild(3).gameObject;
        _comboProgressBar = _comboPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _comboMeter = _comboPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _comboPanel.SetActive(false);

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

    public void SetComboActive(bool active)
    {
        _comboPanel.SetActive(active);
    }

    public void SetComboProgress(float amount)
    {
        if (_comboPanel.activeSelf)
        {
            _comboProgressBar.fillAmount = amount;
        }
        else
        {
            Debug.LogError("Combo is inactive. SetComboProgress() ignored.");
        }
    }

    public void SetComboMeter(int meter)
    {
        if (_comboPanel.activeSelf)
        {
            if (meter < 7)
            {
                _comboMeter.text = "x" + meter.ToString();
            }
            else
            {
                Debug.LogError("Combo exceeds it's limits. Setting combo meter at \'N\'");
                _comboMeter.text = "xN";
            }
        }
        else
        {
            Debug.LogError("Combo is inactive. SetComboMeter() ignored.");
        }
    }
}