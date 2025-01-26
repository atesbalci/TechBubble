using DG.Tweening;
using TechBubble.Behaviors;
using TMPro;
using UnityEngine;
using Zenject;

namespace TechBubble.Views
{
    public class FeedbackView : MonoBehaviour
    {
        [SerializeField] private TMP_Text feedbackTemplate;

        [Inject]
        public void Initialize(IDeadlinesProvider deadlinesProvider)
        {
            feedbackTemplate.gameObject.SetActive(false);
            deadlinesProvider.OnDeadlineCreated += OnDeadlineCreated;
            deadlinesProvider.OnDeadlineReached += OnDeadlineReached;
            deadlinesProvider.OnDeadlineFailed += OnDeadlineFailed;
        }

        private void OnDeadlineFailed(DeadlineBehavior deadlineBehavior)
        {
            GiveFeedback($"Deadline missed!\n-{deadlineBehavior.Money.ToMoneyString()}", false);
        }

        private void OnDeadlineReached(DeadlineBehavior deadlineBehavior)
        {
            GiveFeedback("Deadline met!", true);

        }

        private void OnDeadlineCreated(DeadlineBehavior deadlineBehavior)
        {
            GiveFeedback($"Investment taken!\n+{deadlineBehavior.Money.ToMoneyString()}", true);
        }

        private void GiveFeedback(string text, bool positive)
        {
            var feedback = Instantiate(feedbackTemplate, transform, false);
            feedback.gameObject.SetActive(true);
            feedback.color = positive ? Color.green : Color.red;
            feedback.text = text;
            DOTween.Sequence()
                .Append(feedback.rectTransform.DOAnchorPosY(100f, 3f))
                .Join(feedback.DOFade(0f, 3f));
        }
    }
}