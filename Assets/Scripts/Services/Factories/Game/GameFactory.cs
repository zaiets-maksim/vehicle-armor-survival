using Services.Factories;
using Zenject;

public class GameFactory : Factory, IGameFactory
{
    private const string HudPath = "Prefabs/UI/HUD";
    private const string CameraPath = "Prefabs/Camera";
    private const string PlayerPath = "Prefabs/Player";
    private const string InputControllerPath = "Prefabs/InputController";
    
    public CameraScript Camera { get; private set; }
    public Player Player { get; private set; }
    public InputController InputController { get; private set; }


    public GameFactory(IInstantiator instantiator) : base(instantiator)
    {
    }

    public CameraScript CreateCamera()
    {
        Camera = InstantiateOnActiveScene(CameraPath).GetComponent<CameraScript>();
        return Camera;
    }

    public Player CreatePlayer()
    {
        Player = InstantiateOnActiveScene(PlayerPath).GetComponent<Player>();
        return Player;
    }
    
    public InputController CreateInputController()
    {
        InputController = InstantiateOnActiveScene(InputControllerPath).GetComponent<InputController>();
        return InputController;
    }
}

public interface IGameFactory
{
    CameraScript CreateCamera();
    Player CreatePlayer();
    InputController CreateInputController();

    public CameraScript Camera { get; }
    public Player Player { get; }
    InputController InputController { get; }
}