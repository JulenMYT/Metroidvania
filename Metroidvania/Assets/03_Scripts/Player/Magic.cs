using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    [Header("References")]
    public Player player;
    public SpellUIManager spellUIManager;

    [Header("Spell State")]
    [SerializeField] private List<SpellSO> availableSpells = new List<SpellSO>();
    [SerializeField] private int currentIndex = 0;
    public SpellSO CurrentSpell => availableSpells.Count > 0? availableSpells[currentIndex] : null;
    private SpellSO lockedSpell;

    private Dictionary<SpellSO, float> spellCooldowns = new Dictionary<SpellSO, float>();

    private int spellPower;

    private void Start()
    {
        spellUIManager.ShowSpells(availableSpells);
        HighlightCurrentSpell();
    }

    public void LearnSpell(SpellSO spellSO)
    {
        if (!availableSpells.Contains(spellSO))
        {
            availableSpells.Add(spellSO);
        }

        currentIndex = Mathf.Clamp(currentIndex, 0, availableSpells.Count);

        spellUIManager.ShowSpells(availableSpells);

        if (!spellCooldowns.ContainsKey(spellSO))
        {
            spellCooldowns[spellSO] = 0;
        }

        if (availableSpells.Count > 0)
        {
            HighlightCurrentSpell();
        }
    }

    public void NextSpell()
    {
        if (availableSpells.Count == 0)
        {
            return;
        }

        currentIndex = (currentIndex + 1) % availableSpells.Count;
        HighlightCurrentSpell();
    }

    public void PreviousSpell()
    {
        if (availableSpells.Count == 0)
        {
            return;
        }

        currentIndex = (currentIndex - 1 + availableSpells.Count) % availableSpells.Count;
        HighlightCurrentSpell();
    }

    public void LockSpell()
    {
        lockedSpell = CurrentSpell;
    }

    private void HighlightCurrentSpell()
    {
        if (CurrentSpell != null)
        {
            spellUIManager.HighlightSpell(CurrentSpell);
        }
    }

    public void MagicAnimationFinished()
    {
        player.AnimationFinished();
        CastSpell();
    }

    public bool CanCast(SpellSO spellSO)
    {
        if (spellSO == null)
            return false;
        return Time.time >= spellCooldowns[spellSO];
    }

    private void CastSpell()
    {
        if (!CanCast(lockedSpell) || lockedSpell == null)
            return;

        lockedSpell.Cast(player, spellPower);

        spellCooldowns[lockedSpell] = Time.time + lockedSpell.cooldown;
        spellUIManager.TriggerCooldown(lockedSpell, lockedSpell.cooldown);
    }

    public void SetStats(int spellPower)
    {
        this.spellPower = spellPower;
    }
}
