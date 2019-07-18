using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public float Energy = 50f;

    public float ExpiredTime = 15f;
    public float WarnningTime = 5f;
    private float createdTime;
    private Color defaultColor;

    private Animator animate;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Restart();
        animate = GetComponent<Animator>();
        defaultColor = GetComponent<SpriteRenderer>().color;
    }

    protected virtual void FixedUpdate()
    {
        if (Time.time > createdTime + ExpiredTime - WarnningTime)
        {
            animate.SetBool("Blink", true);
        }
        if (Time.time > createdTime + ExpiredTime)
        {
            GetComponent<SpriteRenderer>().color = defaultColor;
            animate.SetBool("Blink", false);
            gameObject.SetActive(false);
        }
    }

    public virtual void Restart()
    {
        createdTime = Time.time;
        GetComponent<SpriteRenderer>().color = defaultColor;
        animate.SetBool("Blink", false);
    }
}
