using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Steam : Weapon
{
    private bool receivedFire = false;
    private bool receivedIce = false;

    public float addSize = 0.1f;

    public float grapTime = 0.3f;
    private float expiredExpandTime;

    public float expiredTime = 15f;
    private float _expiredtime;

    public bool isElectric = false;

    private Vector3 defaultScale;
    private Color defaultColor;
    private Material m_Material;

    // Game object map with next damage time
    private Dictionary<GameObject, float> StayEnemies = new Dictionary<GameObject, float>();
    public float dealDamageEvery = 1f;

    private void Awake()
    {
        defaultScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_Material = GetComponent<Renderer>().material;
        defaultColor = m_Material.color;
    }

    private void Start()
    {
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        handleElectric();
        if (Time.time > _expiredtime)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (receivedIce && receivedFire)
            {
                Expand();
            }
        }
    }

    private void Expand()
    {
        transform.localScale += new Vector3(addSize, addSize, 0);
        _expiredtime = Time.time + expiredTime;
        receivedFire = false;
        receivedIce = false;
        expiredExpandTime = Time.time + grapTime;
    }

    private void handleElectric()
    {
        if (isElectric)
        {
            m_Material.color = Color.blue;
        }
        else
        {
            m_Material.color = defaultColor;
        }
    }

    private void FixedUpdate()
    {
        if (Time.time <= _expiredtime)
        {
            //Debug.Log((int)(_expiredtime - Time.time));
        }

        // deal damage every deal damage interval
        for (int index = 0; index < StayEnemies.Count; index++)
        {
            var item = StayEnemies.ElementAt(index);
            GameObject go = item.Key;
            float nextDamageTime = item.Value;

            EnemyBaseClass enemy = go.GetComponent<EnemyBaseClass>();
            // if the enemy already dead remove from the list
            if (enemy.GetCurrentHP() <= 0)
            {
                StayEnemies.Remove(go);
            }

            // deal damage
            if (Time.time > nextDamageTime)
            {
                int damage = GetActualDamage();
                enemy.ModifyHealth(-damage);
                nextDamageTime = Time.time + dealDamageEvery;
                StayEnemies[go] = nextDamageTime;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Track which enemy we need to deal the damage
        if (collision.gameObject.tag == "Enemy")
        {
            StayEnemies.Add(collision.gameObject, Time.time);
        }

        if (collision.gameObject.name == "AttackCollider")
        {
            ElectricHand eh = collision.gameObject.GetComponent<ElectricHand>();
            if (eh.getElectricHand())
            {
                // touched by electric.
                Debug.Log("Electric touch");
                // do the exposion here



                gameObject.SetActive(false);

                isElectric = true;
                eh.setElectricHand(false);
            }
        }

        if (Time.time > expiredExpandTime)
        {
            return;
        }
        if (collision.gameObject.GetComponent<IceMissile>() != null)
        {
            receivedIce = true;
        }
        else if (collision.gameObject.GetComponent<FlameProjectile>() != null)
        {
            receivedFire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            StayEnemies.Remove(collision.gameObject);
        }
    }


    public void Restart()
    {
        transform.localScale = defaultScale;
        expiredExpandTime = Time.time + grapTime;
        _expiredtime = Time.time + expiredTime;
        isElectric = false;
    }
}
