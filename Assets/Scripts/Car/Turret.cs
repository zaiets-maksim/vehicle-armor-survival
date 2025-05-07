using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Turret : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private SwipeInputController _swipeInput;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _reloadTime;
    [SerializeField] private Transform _firePoint;
    [FormerlySerializedAs("_angle")] [Range(0, 180)] [SerializeField] private int _angleLimit = 120;
    [SerializeField] private bool _useSmoothing;
    [SerializeField] private float _rotationSpeed = 400f;
    [SerializeField] private Camera _camera;
    
    private float _targetAngle;

    private void Start()
    {
        StartCoroutine(ReloadAndShot());
        _swipeInput.OnSwipeUpdate += AimTurretAtTouch;
    }

    private void AimTurretAtTouch(Vector2 touchPosition)
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = _camera.ScreenPointToRay(touchPosition);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 worldTouchPosition = ray.GetPoint(enter);
            Vector3 directionToTarget = worldTouchPosition - transform.position;
            directionToTarget.y = 0;

            _targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
            _targetAngle = Mathf.Clamp(_targetAngle, -_angleLimit, _angleLimit);
        }

        if (!_useSmoothing)
        {
            transform.rotation = Quaternion.Euler(270f, 0, _targetAngle);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(270f, 0, _targetAngle),
                _rotationSpeed * Time.deltaTime);
        }
    }


    private IEnumerator ReloadAndShot()
    {
        yield return new WaitForSeconds(_reloadTime);
        Shot();
    }

    private void Shot()
    {
        var bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        StartCoroutine(bullet.Move(_car.CurrentVelocity));
        bullet.AlightByHeight(1.1f);
        StartCoroutine(ReloadAndShot());
    }
}