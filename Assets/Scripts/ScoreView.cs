using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.ResourcePickUp += DisplayAmount;
    }

    private void OnDisable()
    {
        _player.ResourcePickUp -= DisplayAmount;
    }

    private void DisplayAmount()
    {
        int amount = _player.Colectable;
        _scoreText.text = amount.ToString();
    }
}
