using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask targetLayer;

    public float LifeTime { get; set; } = 5;
    public int Damage { get; set; } = 1;

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy (gameObject);
        }

        if (((1 << collision.gameObject.layer) & targetLayer) == 0) 
        {
            return;
        }

        Health health = collision.GetComponentInChildren<Health>();

        health.ChangeHealth(-Damage, transform.position);
        Destroy (gameObject);
    }
}