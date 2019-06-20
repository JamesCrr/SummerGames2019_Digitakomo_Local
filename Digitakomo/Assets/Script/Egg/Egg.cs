using UnityEngine;

public class Egg : MonoBehaviour
{
    public static Egg Instance = null;
    public float MaxHP = 5000;
    private float HP;

    // Awake
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);

        // BEING ATTACK !!!
    }

    public void ReduceHealth(float damage)
    {
        HP -= damage;
    }
}
