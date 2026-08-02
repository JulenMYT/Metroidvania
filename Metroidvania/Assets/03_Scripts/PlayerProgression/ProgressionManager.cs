using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text pointsText;
    
    public AttributesSlot[] attributesSlots;

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

        previewAttributes.Set(type, currentPreview + amount);
        availablePoints -= amount;

        RefreshUI();
    }

    public void ConfirmChanges()
    {
        baseAttributes = previewAttributes.Clone();
        startingPoints = availablePoints;
        RefreshUI();
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
    }
}
