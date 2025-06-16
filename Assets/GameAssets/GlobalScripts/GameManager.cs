using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct ShapeEntityTemplate
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

    [Header(" Links ")]
    [SerializeField] private List<GameObject> _spawnPoints;

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
    }

    void Start()
    {
        // assembling the bag
        // to be redone
        _bag = new List<ShapeEntityTemplate>();
        for (int i = 0; i < ShapesList.Count; ++i)
        {
            for (int j = 0; j < GemsList.Count; ++j)
            {
                for (int k = 0; k < ColorsList.Count; ++k)
                {
                    _bag.Add(new ShapeEntityTemplate((Shapes)i, (Gems)j, (Colors)k));
                    _bag.Add(new ShapeEntityTemplate((Shapes)i, (Gems)j, (Colors)k));
                    _bag.Add(new ShapeEntityTemplate((Shapes)i, (Gems)j, (Colors)k));
                }
            }
        }
        _bag = ShuffleList(_bag);
        _totalCount = _bag.Count;

        StartCoroutine(ShapesSpawning());
    }

    private IEnumerator ShapesSpawning()
    {
        _fieldCount = 0;
        while (_fieldCount < 84) // kinda magic number
        {
            // Vector3 spawnPos = new Vector3(_areaCenter.x + Random.Range(-_areaWidthHalf, _areaWidthHalf), _areaCenter.y + Random.Range(-_areaHeightHalf, _areaHeightHalf));
            var s = Instantiate(ShapesList[(int)_bag[0].shape], _spawnPoints[Random.Range(0, _spawnPoints.Count)].transform.position, Quaternion.identity);
            s.GetComponent<ShapeEntity>().Init(_bag[0].shape, _bag[0].gem, _bag[0].color);
            yield return new WaitForSeconds(0.25f);
            _fieldCount += 1;
            _bag.RemoveAt(0);
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