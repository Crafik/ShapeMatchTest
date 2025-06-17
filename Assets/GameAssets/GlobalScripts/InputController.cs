using UnityEngine;
using UnityEngine.InputSystem;

public class InputController
{
    private Controls _controls;

    private int _shapesLayerMask;

    private bool _isEnabled;

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

        _isEnabled = false;
    }

    ~InputController()
    {
        _controls.Touch.PrimaryTouch.performed -= OnPrimaryTouch;

        _controls.Disable();
    }

    public void EnableTouch(bool enable)
    {
        _isEnabled = enable;
    }

    void OnPrimaryTouch(InputAction.CallbackContext ctx)
    {
        if (_isEnabled)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());

            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, _shapesLayerMask);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("PlayShape"))
                {
                    hit.collider.gameObject.GetComponent<ShapeEntity>().GetClicked();
                }
            }
        }
    }
}