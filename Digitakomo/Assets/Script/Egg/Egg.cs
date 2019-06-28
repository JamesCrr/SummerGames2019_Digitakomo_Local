using UnityEngine;

public class Egg : MonoBehaviour, IDamagable
{
    // Instance
    public static Egg Instance = null;
    // Stats
    public float MaxHP = 5000;
    private float HP;
    // Other Components
    EggCenterPoint centerPoint = null;

    // 
    public int WarnPercentage = 30;

    // Awake
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        // Set the stats
        HP = MaxHP;

        // Get Components
        centerPoint = GetComponent<EggCenterPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MaxHP * WarnPercentage / 100 >= HP)
        {
            Debug.Log("Warning EGG HP LESS THAN " + WarnPercentage + " Percent");
        }

        if (HP <= 0)
        {
            SceneController.LoadEndScene(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);

        // BEING ATTACK !!!
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public float GetCurrentHP()
    {
        return HP;
    }

    #region Other Components
    public float GetStartingRadius()
    {
        return centerPoint.GetMaxSizeRadius();
    }
    #endregion

}
