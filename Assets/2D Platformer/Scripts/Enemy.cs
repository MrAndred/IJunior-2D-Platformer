using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform[] _points;

    [SerializeField] private Transform _path;
    [SerializeField] private float _speed;

    private Transform _target;
    private int _currentPointIndex;
    private float _flipAngle;
    private float _defaultRotationY;

    private void OnValidate()
    {
        _animator = GetComponent<Animator>();

        _points = new Transform[_path.childCount];

        for (int i = 0; i < _path.childCount; i++)
        {
            _points[i] = _path.GetChild(i);
        }
    }

    private void Start()
    {
        _target = _points[0];
        _defaultRotationY = 0;
        _flipAngle = 180f;
    }

    private void Update()
    {
        MoveAuto();
    }

    private void MoveAuto()
    {
        Vector2 direction = (_target.position - transform.position).normalized;
        float horizontal = direction.x * _speed * Time.deltaTime;

        if (horizontal < _defaultRotationY)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, _flipAngle, transform.rotation.z);
        }
        else if (horizontal > _defaultRotationY)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, _defaultRotationY, transform.rotation.z);
        }

        if (Vector2.Distance(transform.position, _target.position) < 0.01f)
        {
            _currentPointIndex++;

            if (_currentPointIndex >= _points.Length)
            {
                _currentPointIndex = 0;
            }

            _target = _points[_currentPointIndex];
        }

        float horizontalAbs = Mathf.Abs(horizontal);
        _animator.SetFloat("Speed", horizontalAbs);
        transform.Translate(horizontalAbs, 0, 0);
    }
}
