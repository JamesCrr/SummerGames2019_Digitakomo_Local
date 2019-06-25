using UnityEngine;

public class Steam : MonoBehaviour
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
            Debug.Log((int)(_expiredtime - Time.time));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "AttackCollider")
        {
            ElectricHand eh = collision.gameObject.GetComponent<ElectricHand>();
            if (eh.getElectricHand())
            {
                Debug.Log("Electric touch");
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

    public void Restart()
    {
        transform.localScale = defaultScale;
        Debug.Log(transform.localScale);
        expiredExpandTime = Time.time + grapTime;
        _expiredtime = Time.time + expiredTime;
        isElectric = false;
    }
}
