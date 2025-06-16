using UnityEngine;
using UnityEngine.InputSystem;

public class InputController
{
    private Controls _controls;

    private int _shapesLayerMask;

    public InputController()
    {
        _shapesLayerMask = 1 << LayerMask.NameToLayer("Shapes");
        if (_shapesLayerMask == -1)
        {
            // no time to throw exception here
        }
        
        _controls = new Controls();
        _controls.Enable();

        _controls.Touch.PrimaryTouch.performed += OnPrimaryTouch;
    }

    ~InputController()
    {
        _controls.Touch.PrimaryTouch.performed -= OnPrimaryTouch;

        _controls.Disable();
    }

    void OnPrimaryTouch(InputAction.CallbackContext ctx)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        Debug.Log("OnPrimaryTouch");

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, _shapesLayerMask);
        if (hit.collider != null)
        {
            Debug.Log("If hit != null");
            if (hit.collider.CompareTag("PlayShape"))
            {
                Debug.Log("If hit is PlayShape");
                hit.collider.gameObject.GetComponent<ShapeEntity>().GetClicked();
            }
        }
    }
}