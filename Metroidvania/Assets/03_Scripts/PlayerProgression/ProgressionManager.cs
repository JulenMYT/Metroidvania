using System;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;

public class ProgressionManager : MenuPanel
{
    public event Action OnStatsChanged;

    [Header("UI References")]
    public TMP_Text pointsText;
    
    public AttributesSlot[] attributesSlots;
    public StatSlot[] statSlots;

    [Header("Settings")]
    public int availablePoints = 5;
    private int startingPoints; //Tracks the staring value in case we cancel and need to reset

    public Attributes baseAttributes;   //The "Real" stats
    public Attributes previewAttributes; //The "Draft" stats

    private void Start()
    {
        //Initialize
        startingPoints = availablePoints;
        previewAttributes = baseAttributes.Clone();

        //Setup our slots
        foreach (AttributesSlot slot in attributesSlots)
        {
            slot.Setup(this);
        }
        RefreshUI();
    }

    public int GetAttributeValue(AttributeType type, bool isPreview)
    {
        return isPreview ? previewAttributes.Get(type) : baseAttributes.Get(type);
    }

    public void ModifyAttribute(AttributeType type, int amount)
    {
        int currentPreview = previewAttributes.Get(type);

        if (amount > 0 && availablePoints <= 0)
            return;

        if (amount < 0 && currentPreview <= 0)
            return;

        if (amount > 0 && currentPreview >= 20)
            return;

        previewAttributes.Set(type, currentPreview + amount);
        availablePoints -= amount;

        RefreshUI();
    }

    public void ConfirmChanges()
    {
        baseAttributes = previewAttributes.Clone();
        startingPoints = availablePoints;
        RefreshUI();
        OnStatsChanged?.Invoke();
    }

    public void CancelChanges()
    {
        previewAttributes = baseAttributes.Clone();
        availablePoints = startingPoints;
        RefreshUI();
    }

    private void RefreshUI()
    {
        pointsText.text = availablePoints.ToString();
        foreach (AttributesSlot slot in attributesSlots)
        {
            slot.Refresh();
        }
        RefreshStatsMenu();
    }

    public void RefreshStatsMenu()
    {
        statSlots[0].Refresh(Stats.MaxHealth(baseAttributes), Stats.MaxHealth(previewAttributes));
        statSlots[1].Refresh(Stats.AttackDamage(baseAttributes), Stats.AttackDamage(previewAttributes));
        statSlots[2].Refresh(Stats.SpellPower(baseAttributes), Stats.SpellPower(previewAttributes));
        statSlots[3].Refresh(Stats.CritChance(baseAttributes), Stats.CritChance(previewAttributes));
    }

    public override void Open()
    {
        RefreshStatsMenu();
    }

    public override void Close()
    {
        CancelChanges();
    }
}
