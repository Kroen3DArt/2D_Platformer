using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AidHeart : MonoBehaviour
{
    public int Aid { get; private set; }

    private void Start()
    {
        Aid = 20;
    }

    public void Destroy() => Destroy(this.gameObject);
}
