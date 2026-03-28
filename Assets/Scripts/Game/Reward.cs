using UnityEngine;

public class Reward : MonoBehaviour
{
    [SerializeField] private string riderTag = "Rider";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(riderTag)) return;

        GameManager.Instance.AddReward();
        Destroy(gameObject);
    }
}
