using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _healthBar;

    private int _maxHealth;
    private IDamageble _damageble;

    public void Init(int maxHealth, IDamageble damageble)
    {
        _damageble = damageble;
        _maxHealth = maxHealth;
        _damageble.OnTakeDamage += RefreshBar;
        _healthBar.color = _gradient.Evaluate(_healthBar.fillAmount);
    }

    private void RefreshBar(int damage)
    {
        float value = (float)_damageble.Health / _maxHealth;
        _healthBar.fillAmount = value;
        _healthBar.color = _gradient.Evaluate(value);
    }

    private void OnDisable()
    {
        _damageble.OnTakeDamage -= RefreshBar;
    }
}
