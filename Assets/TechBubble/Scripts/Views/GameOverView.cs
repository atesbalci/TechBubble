using TechBubble.Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace TechBubble.Views
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private TMP_Text infoText;

        [Inject]
        public void Initialize(IGameState gameState)
        {
            gameObject.SetActive(false);
            gameState.OnGameOver += () => Show(gameState.InvestmentsSurvived);
            restartButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        }

        private void Show(int investmentRounds)
        {
            infoText.text = $"You survived {investmentRounds} investment rounds";
            gameObject.SetActive(true);
        }
    }
}