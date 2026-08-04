using UnityEngine;

[CreateAssetMenu(menuName ="Spells/HealSpell")]
public class HealSpellSO : SpellSO
{
    [Header("Heal Settings")]
    public int healAmount = 10;
    public GameObject healFXPrefab;

    public override void Cast(Player player, int spellPower)
    {
        GameObject newHealFX = Instantiate(healFXPrefab, player.transform.position + Vector3.down * 0.5f, Quaternion.identity, player.transform);
        Destroy(newHealFX, 2);

        float spellModifier = 1f + (spellPower / 20);
        int realHeal = Mathf.RoundToInt(healAmount * spellModifier);
        player.health.ChangeHealth(realHeal, player.transform.position);
    }
}
