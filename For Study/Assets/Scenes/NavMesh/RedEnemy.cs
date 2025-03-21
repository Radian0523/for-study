using UnityEngine;
using UnityEngine.AI;

public class RedEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform player;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
    }

    void Update()
    {
        agent.destination = player.position;

    }
}
