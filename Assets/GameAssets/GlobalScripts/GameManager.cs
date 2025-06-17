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

    private int _totalCount;
    private int _fieldCount;
    private int _scoreCount;

    private bool _isAssembling;


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
        _ui = new UIManager(_fieldCounter, _bagCounter);

        _isAssembling = false;
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
        _fieldCount = 0;
        _ui.RefreshCounters(_fieldCount, _bag.Count);

        StartCoroutine(ShapesSpawning());
    }

    private IEnumerator ShapesSpawning()
    {
        _isAssembling = true;
        _input.EnableTouch(false);
        _fieldCount = 0;
        while (_fieldCount < 84 & _fieldCount < _totalCount) // kinda magic number
        {
            GameObject s = Instantiate(ShapesList[(int)_bag[0].shape], _spawnPoints[Random.Range(0, _spawnPoints.Count)].transform.position, Quaternion.identity);
            _inGame.Add(s);
            s.GetComponent<ShapeEntity>().Init(_bag[0]);
            yield return new WaitForSeconds(0.25f);
            _fieldCount += 1;
            _bag.RemoveAt(0);
            _ui.RefreshCounters(_fieldCount, _bag.Count);
        }
        _input.EnableTouch(true);
        _isAssembling = false;
    }

    public void RefreshField()
    {
        if (!_isAssembling)
        {
            while (_inGame.Count > 0)
            {
                _bag.Add(_inGame[0].GetComponent<ShapeEntity>().entity);
                Destroy(_inGame[0]);
                _inGame.RemoveAt(0);
            }
            _bag = ShuffleList(_bag);
            _fieldCount = 0;
            _ui.RefreshCounters(_fieldCount, _bag.Count);
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
}