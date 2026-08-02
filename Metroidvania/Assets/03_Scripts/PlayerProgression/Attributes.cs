[System.Serializable]
public class Attributes
{
    public int power = 10;
    public int vittality = 10;
    public int focus = 10;
    public int agility = 10;

    //Create a copy for preview purposes
    public Attributes Clone()
    {
        return new Attributes
        {
            power = this.power,
            vittality = this.vittality,
            focus = this.focus,
            agility = this.agility
        };
    }

    public int Get(AttributeType type)
    {
        return type switch
        {
            AttributeType.Power => power,
            AttributeType.Vitality => vittality,
            AttributeType.Focus => focus,
            AttributeType.Agility => agility,
            _ => 0
        };
    }

    public void Set(AttributeType type, int value)
    {
        switch (type)
        {
            case AttributeType.Power:
                power = value;
                break;
            case AttributeType.Vitality:
                vittality = value;
                break;
            case AttributeType.Focus:
                focus = value;
                break;
            case AttributeType.Agility:
                agility = value;
                break;
        }
    }
}

public enum AttributeType
{
    Power,
    Vitality,
    Focus,
    Agility
}
