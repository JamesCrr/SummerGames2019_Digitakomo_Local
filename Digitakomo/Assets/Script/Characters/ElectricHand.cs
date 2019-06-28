using UnityEngine;

public class ElectricHand : Weapon
{
    private Character character;
    private bool isElectricHand = false;

    public float MinElectricDamage = 10f;
    public float MaxElectricDamage = 15f;

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
        character.electricAttack = active;
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

    public override float GetActualDamage()
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
