using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;

    private EnemyConfig config;
    private Enemy enemy;
    private float lastAttackTime;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        config = enemy.Config;
    }

    public bool CanMeleeAttack() => Time.time > lastAttackTime + config.meleeCooldown;
    public bool CanRangedAttack() => Time.time > lastAttackTime + config.rangedCooldown;

    public void PerformMeleeAttack()
    {
        lastAttackTime = Time.time;

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, config.meleeRange, config.targetLayer);
        if (!hit)
            return;

        Health health = hit.GetComponentInChildren<Health>();
        if (health != null)
        {
            health.ChangeHealth(-config.meleeDamage, transform.position);
        }
    }

    public void PerformRangedAttack()
    {
        lastAttackTime = Time.time;

        Vector2 fireDirection = (enemy.CurrentTarget.position - attackPoint.position).normalized;
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject newProjectile = Instantiate(config.projectilePrefab, attackPoint.position, rotation);
        Projectile projectile  = newProjectile.GetComponent<Projectile>();
        projectile.Damage = config.rangedDamage;
        projectile.LifeTime = config.projectileLifetime;

        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = fireDirection * config.projectileSpeed;
    }
}
