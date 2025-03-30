using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] GameObject destroyVFXPrefab;
    public void OnDestruct()
    {
        Instantiate(destroyVFXPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
