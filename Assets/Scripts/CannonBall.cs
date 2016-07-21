using UnityEngine;

public class CannonBall : MonoBehaviour {
    void OnTriggerEnter(Collider col)
    {
        Explode(col.gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        Explode(col.gameObject);
    }

    void Explode(GameObject collider)
    {
        Explodable explodable = collider.GetComponentInParent<Explodable>();

        if (explodable != null)
        {
            explodable.Explode();
        }

        gameObject.GetComponent<Explodable>().Explode();
        Destroy(gameObject);
    }
}
