using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShapeEntity : MonoBehaviour
{

    [HideInInspector] public Shapes shape { get; private set; }
    [HideInInspector] public Gems gem { get; private set; }
    [HideInInspector] public Colors color { get; private set; }
    [HideInInspector] public Mutators mutator { get; private set; }

    private GameObject _shapeObj;

    private bool _isActive;

    public void Init(Shapes sh, Gems g, Colors c, Mutators m = Mutators.None)
    {
        shape = sh;
        gem = g;
        color = c;
        mutator = m;

        Instantiate(GameManager.Instance.GemsList[(int)gem], this.transform);
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GameManager.Instance.ColorsList[(int)color];

        // here be mutators
    }

    void Update()
    {
        
    }
}
