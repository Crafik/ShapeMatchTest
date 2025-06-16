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

        _isActive = false;

        // here be mutators
    }

    public void ChangeState(bool activate)
    {
        _isActive = activate;
        if (_isActive)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.activeColor;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.inactiveColor;
        }
    }

    public void GetClicked()
    {
        Debug.Log(shape + " - " + gem + " - " + color + " got clicked!");
        Destroy(this.gameObject);
    }
}
