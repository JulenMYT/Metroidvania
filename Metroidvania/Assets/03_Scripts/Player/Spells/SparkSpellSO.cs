using UnityEngine;

[CreateAssetMenu(menuName = "Spells/SparkSpell")]
public class SparkSpellSO : SpellSO
{
    [Header("Spark Settings")]
    public int damage = 3;
    public float radius = 5;
    public GameObject sparkFXPrefab;
    public LayerMask enemyLayer;

    public override void Cast(Player player, int spellPower)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, radius, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Health health = enemy.GetComponent<Health>();

            if (health != null)
            {
                float spellModifier = 1f + (spellPower / 20);
                int realDamage = Mathf.RoundToInt(damage * spellModifier);
                health.ChangeHealth(-realDamage, player.transform.position);
            }

            if (sparkFXPrefab != null)
            {
                GameObject newFX = Instantiate(sparkFXPrefab, enemy.transform.position, Quaternion.identity, enemy.transform);
                Destroy(newFX, 2);
            }
        }
    }
}
