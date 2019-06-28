using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public float Energy = 50f;

    public float ExpiredTime = 15f;
    public float WarnningTime = 5f;
    private float createdTime;

    private Animator animate;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Restart();
        animate = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        if (Time.time > createdTime + ExpiredTime - WarnningTime)
        {
            animate.SetBool("Blink", true);
        }
        if (Time.time > createdTime + ExpiredTime)
        {
            animate.SetBool("Blink", false);
            gameObject.SetActive(false);
        }
    }

    public virtual void Restart()
    {
        createdTime = Time.time;
    }
}
