using UnityEngine;

public class Combat : MonoBehaviour
{
    [Header("Attack Settings")]
    private int damage;
    private float critChance;
    public float critMultiplier = 1.5f;

    public float attackRadius = 0.5f;
    public float attackCooldown = 1.5f;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public Animator hitFX;

    [Header("Sound")]
    public AudioData attackSound;
    public AudioData hitSound;

    private AudioManager audioManager;

    public Player player;

    public bool CanAttack => Time.time >= nextAttackTime;
    private float nextAttackTime;

    private void Start() => audioManager = ServiceLocator.Get<AudioManager>();

    public void AttackAnimationFinished()
    {
        player.AnimationFinished();
    }

    public void SetStats(int damage, float critChance)
    {
        this.damage = damage;
        this.critChance = critChance;
    }

    public void Attack()
    {
        if (!CanAttack)
            return;

        audioManager.PlaySFX(attackSound);

        nextAttackTime = Time.time + attackCooldown;

        Collider2D enemy = Physics2D.OverlapCircle(attackPoint.position, attackRadius, enemyLayer);

        if (enemy != null)
        {
            hitFX.Play("HitFX");
            int realDamage = damage;
            if (Random.value < critChance)
            {
                realDamage = Mathf.RoundToInt(realDamage * critMultiplier);
            }   
            enemy.gameObject.GetComponent<Health>().ChangeHealth(-realDamage, transform.position);
            audioManager.PlaySFX(hitSound);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
