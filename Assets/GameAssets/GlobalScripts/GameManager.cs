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
    [SerializeField] private GameObject _canvas;

    [Space(10)]
    [Header(" Public data ")]
    public List<GameObject> ShapesList;
    public List<GameObject> GemsList;
    public List<Color> ColorsList;

    [Space(5)]
    public Color inactiveColor;
    public Color activeColor;

    public List<GameObject> scorePlaces { get => _ui.places; }

    // Lists of shapes
    private List<ShapeEntityTemplate> _bag;
    private List<GameObject> _inGame;
    private List<GameObject> _inScore;

    private int _totalCount;
    [HideInInspector] public int scoreCount;

    [HideInInspector] public bool isActive;
    [HideInInspector] public bool isGameStarted;
    private bool _isNotFirstTime;
    private int _scorepoints;
    private int _refreshTokens;

    private Coroutine _comboCoroutine;
    private int _comboCounter;


    void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        _input = new InputController();
        _ui = new UIManager(_canvas.transform);

        isActive = false;
        isGameStarted = false;
        _isNotFirstTime = false;
        scoreCount = 0;
        _refreshTokens = 3;
        _scorepoints = 0;

        _inScore = new List<GameObject>();
    }

    public void StartGame()
    {
        if (_isNotFirstTime)
        {
            while (_inGame.Count > 0)
            {
                Destroy(_inGame[0]);
                _inGame.RemoveAt(0);
            }
            while (_inScore.Count > 0)
            {
                Destroy(_inScore[0]);
                _inScore.RemoveAt(0);
            }
            _ui.RefreshScoreBar(_inScore);
            scoreCount = 0;
            _refreshTokens = 3;
            _ui.SetRefreshTokens(_refreshTokens);
            _ui.SetComboActive(false);
        }
        if (!isGameStarted)
        {
            _ui.PlayButtonAnim(false);
            _ui.PlayLabelAnim(false);
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

            StartShapeSpawning();
            isGameStarted = true;
            _isNotFirstTime = true;
        }
    }

    private void StartShapeSpawning()
    {
        if (_comboCoroutine != null)
        {
            StopCoroutine(_comboCoroutine);
        }
        StartCoroutine(ShapesSpawning());
    }

    private IEnumerator ShapesSpawning()
    {
        isActive = false;
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
        isActive = true;
        if (_comboCounter > 0)
        {
            StartCoroutine(ScoreComboCoroutine());
        }
    }

    public void RefreshField()
    {
        if (isActive && _refreshTokens > 0)
        {
            _refreshTokens -= 1;
            _ui.SetRefreshTokens(_refreshTokens);
            while (_inGame.Count > 0)
            {
                _bag.Add(_inGame[0].GetComponent<ShapeEntity>().entity);
                Destroy(_inGame[0]);
                _inGame.RemoveAt(0);
            }
            _bag = ShuffleList(_bag);
            _ui.RefreshCounters(_inGame.Count, _bag.Count);
            StartShapeSpawning();
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
        _totalCount -= 1;
        scoreCount += 1;
        _ui.RefreshCounters(_inGame.Count, _bag.Count);
        shape.transform.SetParent(_canvas.transform);

        if (_inGame.Count < 60 && _inGame.Count != _totalCount)
        {
            StartShapeSpawning();
        }
    }

    public void AddToScore(GameObject shape)
    {
        _inScore.Add(shape);
        FindMatches();
        _ui.RefreshScoreBar(_inScore);

        if (_inScore.Count == 7)
        {
            isActive = false;
            isGameStarted = false;
            _ui.PlayLabelAnim(true);
            _ui.SetLabelText("Defeat!");
            _ui.PlayButtonAnim(true);
        }
    }

    private void FindMatches()
    {
        // kinda bungled this one, but don't see much sense in breaking it up in pieces
        // gonna comment instead :)
        List<int> matches = new List<int>();

        // looking for matches with last added shape and counting hits
        ShapeEntityTemplate matchTo = _inScore[_inScore.Count - 1].GetComponent<ShapeEntity>().entity;
        for (int i = 0; i < _inScore.Count - 1; ++i)
        {
            ShapeEntityTemplate matchWith = _inScore[i].GetComponent<ShapeEntity>().entity;
            if (matchTo.shape == matchWith.shape)
            {
                if (matchTo.gem == matchWith.gem)
                {
                    if (matchTo.color == matchWith.color)
                    {
                        matches.Add(i);
                    }
                }
            }
        }
        matches.Add(_inScore.Count - 1); // adding last added to scorebar shape into matches

        // check if enough matches and scoring if so
        if (matches.Count > 2)
        {
            for (int i = matches.Count - 1; i > -1; --i)
            {
                Destroy(_inScore[matches[i]]);
                _inScore.RemoveAt(matches[i]);
                scoreCount -= 1;
            }

            if (_comboCounter < 6)
            {
                _comboCounter += 1;
            }
            _scorepoints += 300 + ((_comboCounter - 1) * 150);
            _ui.SetScorePoints(_scorepoints);
            if (_comboCoroutine != null)
            {
                StopCoroutine(_comboCoroutine);
            }
            _comboCoroutine = StartCoroutine(ScoreComboCoroutine());
        }

        // end game check
        if (_inScore.Count == 0 && _totalCount == 0)
        {
            isActive = false;
            isGameStarted = false;
            _ui.PlayLabelAnim(true);
            _ui.SetLabelText("Victory!");
            _ui.PlayButtonAnim(true);
        }
    }

    private IEnumerator ScoreComboCoroutine()
    {
        if (_comboCounter > 1)
        {
            _ui.SetComboActive(true);
            _ui.SetComboMeter(_comboCounter);
            float comboTimer = 3.5f;
            while (comboTimer > 0f)
            {
                _ui.SetComboProgress(comboTimer / 3.5f);
                comboTimer -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        _ui.SetComboActive(false);
        _comboCounter = 0;
    }
}