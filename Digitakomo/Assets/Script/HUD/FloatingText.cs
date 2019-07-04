using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Animator animate;
    public Text HPText;
    private float createdTime;
    private float destroyTime;
    private Vector3 createdPosition;
    // Start is called before the first frame update
    void Start()
    {
        AnimatorClipInfo[] clips = animate.GetCurrentAnimatorClipInfo(0);
        HPText = animate.GetComponent<Text>();

        destroyTime = clips[0].clip.length;
    }

    private void FixedUpdate()
    {
        if (Time.time >= createdTime + destroyTime - 0.5f)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        Vector3 healthPosition = Camera.main.WorldToScreenPoint(createdPosition);
        Debug.Log(healthPosition);
        transform.position = healthPosition;
    }

    public void SetText(string HPText, Vector3 position)
    {
        createdPosition = position;
        this.HPText.text = HPText;
        createdTime = Time.time;

    }
}
