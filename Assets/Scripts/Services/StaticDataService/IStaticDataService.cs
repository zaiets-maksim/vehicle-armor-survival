using Connect4.Scripts.StaticData;
using StaticData;
using StaticData.Configs;
using StaticData.Levels;

namespace Services.StaticDataService
{
    public interface IStaticDataService
    {
        void LoadData();
        // GameStaticData GameConfig();
        WindowConfig ForWindow(WindowTypeId typeId);
        LevelStaticData LevelConfig();

        BalanceStaticData Balance();
        GameStaticData GameConfig();
    }
}
