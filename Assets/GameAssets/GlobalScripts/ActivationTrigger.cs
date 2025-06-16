using UnityEngine;

public class ActivationTrigger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayShape"))
        {
            collision.GetComponent<ShapeEntity>().ChangeState(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayShape"))
        {
            collision.GetComponent<ShapeEntity>().ChangeState(false);
        }
    }

}