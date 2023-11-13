using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private Coin _coin;
    [SerializeField] private Transform[] _coinPositions;

    private void OnValidate()
    {
        _coinPositions = GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        for (int i = 0; i < _coinPositions.Length; i++)
        {
            Coin coin = Instantiate(_coin, _coinPositions[i].position, Quaternion.identity);
        }
    }
}
