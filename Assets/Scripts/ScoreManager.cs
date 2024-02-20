using UnityEngine;

public class ScoreManager : MonoBehaviour {
    [SerializeField] private TextMesh _scoreText;
    private int _score = 0;
    public void OnChangeState(Component component, object data) {
        if (data is not Zombie.State state) return;
        if (state != Zombie.State.Dead) return;

        _score++;
        _scoreText.text = _score.ToString();
    }
}
