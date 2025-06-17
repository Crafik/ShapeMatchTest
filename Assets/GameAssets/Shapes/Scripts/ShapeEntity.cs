using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShapeEntity : MonoBehaviour
{
    public ShapeEntityTemplate entity;

    private bool _isActive;

    public void Init(ShapeEntityTemplate template)
    {
        entity = template;

        Instantiate(GameManager.Instance.GemsList[(int)entity.gem], this.transform);
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GameManager.Instance.ColorsList[(int)entity.color];

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
        if (_isActive)
        {
            Destroy(this.gameObject);
        }
    }
}
