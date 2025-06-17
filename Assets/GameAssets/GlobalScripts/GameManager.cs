using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct ShapeEntityTemplate
{
    public Shapes shape;
    public Gems gem;
    public Colors color;
    public Mutators mutator;

    public ShapeEntityTemplate(Shapes sh, Gems g, Colors c, Mutators m = Mutators.None)
    {
        shape = sh;
        gem = g;
        color = c;
        mutator = m;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    private InputController _input;
    private UIManager _ui;

    [Header(" Links ")]
    [SerializeField] private List<GameObject> _spawnPoints;

    [Space(5)]
    [SerializeField] private TextMeshProUGUI _fieldCounter;
    [SerializeField] private TextMeshProUGUI _bagCounter;

    [Space(5)]
    [SerializeField] private GameObject _canvas;

    [Space(10)]
    [Header(" Public data ")]
    public List<GameObject> ShapesList;
    public List<GameObject> GemsList;
    public List<Color> ColorsList;

    [Space(5)]
    public Color inactiveColor;
    public Color activeColor;

    [Space(5)]
    public List<GameObject> scorePlaces;

    [Space(10)]
    [Header(" Variables ")]
    [SerializeField] private float _float;

    // Lists of shapes
    private List<ShapeEntityTemplate> _bag;
    private List<GameObject> _inGame;
    private List<GameObject> _inScore;

    private int _totalCount;
    [HideInInspector] public int scoreCount;

    private bool _isActive;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        _input = new InputController();
        _ui = new UIManager(_fieldCounter, _bagCounter, scorePlaces);

        _isActive = false;
        scoreCount = 0;

        _inScore = new List<GameObject>();
    }

    void Start()
    {
        // assembling the bag
        _bag = new List<ShapeEntityTemplate>();
        _inGame = new List<GameObject>();
        for (int i = 0; i < ShapesList.Count; ++i)
        {
            for (int k = 0; k < 12; ++k)
            {
                ShapeEntityTemplate sh = new ShapeEntityTemplate((Shapes)i, (Gems)Random.Range(0, GemsList.Count), (Colors)Random.Range(0, ColorsList.Count));
                _bag.Add(sh);
                _bag.Add(sh);
                _bag.Add(sh);
            }
        }
        _bag = ShuffleList(_bag);
        _totalCount = _bag.Count;
        _ui.RefreshCounters(_inGame.Count, _bag.Count);

        StartCoroutine(ShapesSpawning());
    }

    private IEnumerator ShapesSpawning()
    {
        _isActive = true;
        _input.EnableTouch(false);
        while (_inGame.Count < 84 & _inGame.Count < _totalCount) // kinda magic number
        {
            GameObject s = Instantiate(ShapesList[(int)_bag[0].shape], _spawnPoints[Random.Range(0, _spawnPoints.Count)].transform.position, Quaternion.identity);
            _inGame.Add(s);
            s.GetComponent<ShapeEntity>().Init(_bag[0]);
            yield return new WaitForSeconds(0.25f);
            _bag.RemoveAt(0);
            _ui.RefreshCounters(_inGame.Count, _bag.Count);
        }
        _input.EnableTouch(true);
        _isActive = false;
    }

    public void RefreshField()
    {
        if (!_isActive)
        {
            while (_inGame.Count > 0)
            {
                _bag.Add(_inGame[0].GetComponent<ShapeEntity>().entity);
                Destroy(_inGame[0]);
                _inGame.RemoveAt(0);
            }
            _bag = ShuffleList(_bag);
            _ui.RefreshCounters(_inGame.Count, _bag.Count);
            StartCoroutine(ShapesSpawning());
        }
    }

    private List<ShapeEntityTemplate> ShuffleList(List<ShapeEntityTemplate> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            ShapeEntityTemplate temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }

    public void ShapeClicked(GameObject shape)
    {
        _inGame.Remove(shape);
        scoreCount += 1;
        _ui.RefreshCounters(_inGame.Count, _bag.Count);
        shape.transform.SetParent(_canvas.transform);
    }

    public void AddToScore(GameObject shape)
    {
        int matchCounter = 0;
        List<GameObject> matches = new List<GameObject>();
        foreach (GameObject s in _inScore)
        {
            ShapeEntityTemplate sT = s.GetComponent<ShapeEntity>().entity;
            ShapeEntityTemplate shapeT = shape.GetComponent<ShapeEntity>().entity;
            if (sT.shape == shapeT.shape && sT.gem == shapeT.gem && sT.color == shapeT.color)
            {
                matchCounter += 1;
                matches.Add(s);
                if (matchCounter > 1) // 2 matches for 3 of the same
                {
                    matches.Add(shape);
                    _inScore.Add(shape);
                    foreach (GameObject match in matches)
                    {
                        _inScore.Remove(match);
                        Destroy(match);
                    }
                    scoreCount -= 3;
                    break;
                }
            }
        }
        _ui.AddToScoreBar(_inScore);
    }
}