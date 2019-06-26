using UnityEngine;

public class GuyTestScript : MonoBehaviour
{
    public FireCharacter fc;

    private float nextTick;

    void Start()
    {
        nextTick = Time.time + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTick)
        {
            fc.TakeDamage(51);
            nextTick = Time.time + 0.1f;
        }
    }
}
