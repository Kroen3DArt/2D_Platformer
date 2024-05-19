using UnityEngine;

public class Traps : MonoBehaviour
{
    public int Damage { get; private set; }

    private void Start()
    {
        Damage = 5;
    }
}
