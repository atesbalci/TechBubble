using System;

namespace TechBubble.Models
{
    public interface IGameState
    {
        event Action OnGameOver;
        long Money { get; }
        bool IsGameOver { get; }
        int InvestmentsSurvived { get; }
    }
}