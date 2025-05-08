using System;
using Services.WindowService;
using StaticData.Configs;

public class GameCurator : IGameCurator
{
    public event Action OnStartGame;
    public event Action<GameResult> OnEndGame;
    
    private readonly IWindowService _windowService;

    public GameCurator(IWindowService windowService)
    {
        _windowService = windowService;
    }
    
    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void EndGame(GameResult result)
    {
        OnEndGame?.Invoke(result);
        
        switch (result)
        {
            case GameResult.Lose:
                _windowService.Open(WindowTypeId.Lose);
                break;
            case GameResult.Win:
                _windowService.Open(WindowTypeId.Win);
                break;
        }
    }
}

public enum GameResult
{
    Unknown,
    Win,
    Lose
}

public interface IGameCurator
{
    public event Action OnStartGame;
    public event Action<GameResult> OnEndGame;

    void StartGame();
    void EndGame(GameResult result);
}
