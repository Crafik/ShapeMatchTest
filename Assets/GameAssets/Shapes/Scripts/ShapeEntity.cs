using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShapeEntity : MonoBehaviour
{
    public ShapeEntityTemplate entity;

    private Collider2D _collider;
    private Rigidbody2D _rigidBody;

    private bool _isActive;

    void Start()
    {
        _collider = this.GetComponent<Collider2D>();
        _rigidBody = this.GetComponent<Rigidbody2D>();
    }

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
        if (GameManager.Instance.isActive)
        {
            if (_isActive)
            {
                if (GameManager.Instance.scoreCount < 7)
                {
                    _collider.enabled = false;
                    _isActive = false;

                    Vector3 pos = GameManager.Instance.scorePlaces[GameManager.Instance.scoreCount].transform.position;

                    GameManager.Instance.ShapeClicked(this.gameObject);
                    StartCoroutine(FlyingCoroutine(pos));
                }
            }
        }
    }

    private IEnumerator FlyingCoroutine(Vector3 targetPos)
    {
        float lerpCounter = 0f;

        Vector3 flyingDir = (Vector3)((Vector2)targetPos - _rigidBody.position);
        flyingDir.Normalize();

        while (_rigidBody.position.y < targetPos.y)
        {
            float currVecSpeed = Mathf.Lerp(-8f, 20f, lerpCounter);
            float currRotSpeed = Mathf.Lerp(0f, 1020f, lerpCounter);
            if (lerpCounter < 1f)
            {
                lerpCounter += Time.fixedDeltaTime;
            }

            _rigidBody.velocity = flyingDir * currVecSpeed;
            _rigidBody.MoveRotation(_rigidBody.rotation + currRotSpeed * Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }
        // Destroy(this.gameObject);

        this.transform.rotation = Quaternion.identity;
        _rigidBody.simulated = false;

        GameManager.Instance.AddToScore(this.gameObject);
    }
}
