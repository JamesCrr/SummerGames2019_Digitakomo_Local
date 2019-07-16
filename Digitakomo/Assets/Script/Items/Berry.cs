using UnityEngine;

public enum BerrySize
{
    Small = 0,
    Medium = 1,
    Large = 2
}

public class Berry : BaseItem
{
    [Header("Chance")]
    public int SmallBerry = 50;
    public int MediumBerry = 35;
    public int LargeBerry = 15;

    [Header("Min Consume HP")]
    public int MinSmallBerry = 0;
    public int MinMediumBerry = 0;
    public int MinLargeBerry = 0;

    [Header("Max Consume HP")]
    public int MaxSmallBerry = 20;
    public int MaxMediumBerry = 50;
    public int MaxLargeBerry = 70;

    [Header("Result randomed field")]
    public BerrySize size;
    public int AddHp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character ch = collision.gameObject.GetComponent<Character>();
        if (ch != null)
        {
            ch.AddHP(AddHp);
            gameObject.SetActive(false);
        }
    }


    public override void Restart()
    {
        base.Restart();
        // random the size
        BerrySize size = RandomSize(SmallBerry, MediumBerry, LargeBerry);
        this.size = size;
        // random the consume
        int addHP = RandomConsume(size);
        this.AddHp = addHP;
    }

    private int RandomConsume(BerrySize size)
    {
        int randomed = 0;
        switch (size)
        {
            case BerrySize.Small: randomed = Random.Range(MinSmallBerry, MaxSmallBerry); break;
            case BerrySize.Medium: randomed = Random.Range(MinMediumBerry, MaxMediumBerry); break;
            case BerrySize.Large: randomed = Random.Range(MinLargeBerry, MaxLargeBerry); break;
        }
        return randomed;
    }

    /**
     * value is a [] that first array in enum 0
     * second array is enum 1
     * thrid array is enum 2
     * */
    private BerrySize RandomSize(params int[] values)
    {
        int MaxValue = 0;
        foreach (int value in values)
        {
            MaxValue += value;
        }
        int randomed = Random.Range(0, MaxValue - 1);
        BerrySize result = BerrySize.Small;
        int index = 0;
        foreach (BerrySize s in System.Enum.GetValues(typeof(BerrySize)))
        {
            if (randomed <= 0)
            {
                continue;
            }
            randomed -= values[index];
            if (randomed <= 0)
            {
                result = s;
            }
        }

        return result;
    }
}
