using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Player _player;

    private int _maxHealth;
    
    private void OnEnable()
    {
        _maxHealth = _player.Health;
        _player.OnTakeDamage += RefreshBar;
        _healthBar.color = _gradient.Evaluate(_healthBar.fillAmount);
    }

    private void RefreshBar(int damage)
    {
        float value = (float)_player.Health / _maxHealth;
        _healthBar.fillAmount = value;
        _healthBar.color = _gradient.Evaluate(value);
    }

    private void OnDisable()
    {
        _player.OnTakeDamage -= RefreshBar;
    }
}
