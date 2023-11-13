using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private int _value;

    public int Value => _value;
}
