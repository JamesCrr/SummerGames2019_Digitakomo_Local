using UnityEngine;
using UnityEngine.UI;

public class HUDEnemyHealth : MonoBehaviour
{
    public Text healthText;
    private EnemyBaseClass enemy;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyBaseClass>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 healthPosition = Camera.main.WorldToScreenPoint(transform.position);
        healthText.transform.position = healthPosition;

        healthText.text = Mathf.Ceil(enemy.GetCurrentHP()).ToString();
    }
}
