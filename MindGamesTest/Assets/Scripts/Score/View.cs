using TMPro;
using UnityEngine;

namespace Score
{
    [AddComponentMenu("Scripts/Score/Score.View")]
    internal class View : MonoBehaviour
    {
        [SerializeField]
        private Model model;

        [SerializeField]
        private TMP_Text scoreLabel;

        private void Awake()
        {
            model.OnScoreChanged += OnScoreChanged;
        }

        private void OnScoreChanged(int value)
        {
            scoreLabel.text = "Score: " + value.ToString();
        }
    }
}
