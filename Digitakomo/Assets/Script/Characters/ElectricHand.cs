using UnityEngine;

public class ElectricHand : Weapon
{
    private Character character;
    private bool isElectricHand = false;

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
}
