using UnityEngine;

public class Egg : MonoBehaviour
{
    // Instance
    public static Egg Instance = null;
    // Stats
    public float MaxHP = 5000;
    private float HP;
    // Other Components
    EggCenterPoint centerPoint = null;


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

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    #region Other Components
    public float GetStartingRadius()
    {
        return centerPoint.GetMaxSizeRadius();
    }
    #endregion

}
