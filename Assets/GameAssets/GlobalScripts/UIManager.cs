using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager
{

    private TextMeshProUGUI _fieldCounter;
    private TextMeshProUGUI _bagCounter;

    private List<GameObject> _places;

    public UIManager(TextMeshProUGUI field, TextMeshProUGUI bag, List<GameObject> places)
    {
        _fieldCounter = field;
        _bagCounter = bag;
        _places = places;
    }

    public void RefreshCounters(int field, int bag)
    {
        _fieldCounter.text = field.ToString();
        _bagCounter.text = bag.ToString();
    }

    public void RefreshScorePlaces(int from)
    {
        // here be code
    }

    public void AddToScoreBar(List<GameObject> shapes)
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