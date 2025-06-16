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

    [Header(" Links ")]
    [SerializeField] private GameObject _spawnField;
    [SerializeField] private List<GameObject> _spawnPoints;

    [Space(10)]
    [Header(" Lists ")]
    public List<GameObject> ShapesList;
    public List<GameObject> GemsList;
    public List<Color> ColorsList;

    [Space(10)]
    [Header(" Variables ")]
    [SerializeField] private float _float;

    // Lists of shapes
    private List<ShapeEntityTemplate> _bag;
    private List<GameObject> _inGame;

    // Spawning area
    private float _areaWidthHalf;
    private float _areaHeightHalf;
    private Vector2 _areaCenter;


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
    }

    void Start()
    {
        _areaCenter = _spawnField.transform.position;
        _areaWidthHalf = _spawnField.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        _areaHeightHalf = _spawnField.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

        // assembling the bag
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

        StartCoroutine(ShapesSpawning());
    }

    private IEnumerator ShapesSpawning()
    {
        int counter = 0;
        while (counter < 84) // kinda magic number
        {
            // Vector3 spawnPos = new Vector3(_areaCenter.x + Random.Range(-_areaWidthHalf, _areaWidthHalf), _areaCenter.y + Random.Range(-_areaHeightHalf, _areaHeightHalf));
            var s = Instantiate(ShapesList[(int)_bag[0].shape], _spawnPoints[Random.Range(0, _spawnPoints.Count)].transform.position, Quaternion.identity);
            s.GetComponent<ShapeEntity>().Init(_bag[0].shape, _bag[0].gem, _bag[0].color);
            yield return new WaitForSeconds(0.25f);
            counter += 1;
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