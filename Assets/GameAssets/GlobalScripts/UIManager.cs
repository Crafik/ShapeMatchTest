using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager
{

    private TextMeshProUGUI _fieldCounter;
    private TextMeshProUGUI _bagCounter;
    private GameObject _startButton;
    private GameObject _mainLabel;

    private Animator _buttonAnim;
    private Animator _labelAnim;
    private TextMeshProUGUI _labelText;

    private List<GameObject> _places;

    public UIManager(TextMeshProUGUI field, TextMeshProUGUI bag, GameObject startbutton, GameObject mainLabel, List<GameObject> places)
    {
        _fieldCounter = field;
        _bagCounter = bag;
        _startButton = startbutton;
        _buttonAnim = _startButton.GetComponent<Animator>();
        _mainLabel = mainLabel;
        _labelAnim = _mainLabel.GetComponent<Animator>();
        _labelText = _mainLabel.GetComponent<TextMeshProUGUI>();
        _places = places;

        PlayButtonAnim(true);
        PlayLabelAnim(true);
    }

    public void PlayButtonAnim(bool FadeIn)
    {
        if (FadeIn)
        {
            _buttonAnim.Play("FadeIn");
        }
        else
        {
            _buttonAnim.Play("FadeOut");
        }
    }

    public void PlayLabelAnim(bool FadeIn)
    {
        if (FadeIn)
        {
            _labelAnim.Play("FadeIn");
        }
        else
        {
            _labelAnim.Play("FadeOut");
        }
    }

    public void SetLabelText(string txt)
    {
        _labelText.text = txt;
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
            shape.transform.position = _places[i].transform.position;
            shape.transform.localScale = Vector3.one * 160f;
            _places[i].SetActive(false);
            i += 1;
        }
        for (; i < _places.Count; ++i)
        {
            _places[i].SetActive(true);
        }
    }
}