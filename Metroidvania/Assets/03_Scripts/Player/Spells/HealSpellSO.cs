using UnityEngine;

[CreateAssetMenu(menuName ="Spells/HealSpell")]
public class HealSpellSO : SpellSO
{
    [Header("Heal Settings")]
    public int healAmount = 10;
    public GameObject healFXPrefab;

    public override void Cast(Player player)
    {
        GameObject newHealFX = Instantiate(healFXPrefab, player.transform.position + Vector3.down * 0.5f, Quaternion.identity);
        Destroy(newHealFX, 2);
        player.health.ChangeHealth(healAmount);
    }
}
