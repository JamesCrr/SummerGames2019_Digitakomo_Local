using UnityEngine;

public class ElectricHand : Weapon
{
    private Character character;
    private bool isElectricHand = false;

    public int MinElectricDamage = 10;
    public int MaxElectricDamage = 15;

    private void Start()
    {
        character = GetComponentInParent<Character>();
        isElectricHand = character.electricAttack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "AttackCollider")
        {
            Debug.Log("EMPOWER !!!!");
            setElectricHand(true);
        }
    }

    public void setElectricHand(bool active)
    {
        isElectricHand = active;
        character.SetElectricAttack(active);
        if (active)
        {
            at = AttackType.Electric;
        }
        else
        {
            at = AttackType.Normal;
        }
    }

    public bool getElectricHand()
    {
        return isElectricHand;
    }

    public override int GetActualDamage()
    {
        if (isElectricHand)
        {
            setElectricHand(false);
            return Random.Range(MinElectricDamage, MaxElectricDamage);
        }
        else
        {
            return base.GetActualDamage();
        }
    }
}
