using UnityEngine;

public class AidKit : MonoBehaviour
{
    [SerializeField] private float _health = 50f;

    public float Health => _health;
}
