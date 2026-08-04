using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [Header("References to Player Components")]
    public ProgressionManager progressionManager;
    public Health health;
    public Combat combat;
    public Magic magic;

    private Attributes BaseAttributes => progressionManager.baseAttributes;

    private void Start()
    {
        ApplyAllStats();
    }

    private void OnEnable() => progressionManager.OnStatsChanged += ApplyAllStats;
    private void OnDisable() => progressionManager.OnStatsChanged -= ApplyAllStats;

    private void ApplyAllStats()
    {
        ApplyHealthStats();
        ApplyCombatStats();
    }

    private void ApplyHealthStats()
    {
        health.ChangeMaxHealth(Stats.MaxHealth(BaseAttributes));
    }

    private void ApplyCombatStats()
    {
        combat.SetStats(Stats.AttackDamage(BaseAttributes), Stats.CritChance(BaseAttributes));
    }

    private void ApplyMagicStats()
    {
        magic.SetStats(Stats.SpellPower(BaseAttributes));
    }
}
