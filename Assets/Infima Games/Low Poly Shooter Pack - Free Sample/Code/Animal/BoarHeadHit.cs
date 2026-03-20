using UnityEngine;

public class BoarHeadHit : MonoBehaviour
{
    AnimalAI animal;

    void Start()
    {
        animal = GetComponentInParent<AnimalAI>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (animal != null)
            {
                animal.OnHitPlayer(collision);
            }
        }
    }
}