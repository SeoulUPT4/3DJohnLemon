using UnityEngine;

public class FollowStart : MonoBehaviour
{
    [SerializeField] FollowEnemy[] followEnemys;

    private void OnTriggerEnter(Collider other)
    {
     if(other.CompareTag("Player"))
        {
            for (int i = 0; i < followEnemys.Length; i++)
            {
                followEnemys[i].StartFollow();
            }

            Destroy(this);
        }
    }
}
