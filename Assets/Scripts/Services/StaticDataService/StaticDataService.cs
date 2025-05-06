using System.Collections.Generic;
using System.Linq;
using Connect4.Scripts.StaticData;
using StaticData;
using StaticData.Configs;
using StaticData.Levels;
using UnityEngine;

namespace Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private const string GameConfigPath = "StaticData/GameConfig";
        private const string BalanceConfigPath = "StaticData/Balance";
        private const string WindowsStaticDataPath = "StaticData/WindowsStaticData";
        private const string KitchenItemStaticDataPath = "StaticData/KitchenItemStaticData";
        private const string HallItemStaticDataPath = "StaticData/HallItemStaticData";
        private const string CharacterStaticDataPath = "StaticData/CharacterStaticData";
        private const string DishStaticDataPath = "StaticData/DishStaticData";
        private const string LevelStaticDataPath = "StaticData/LevelStaticData";
        private const string PopUpWindowsStaticDataPath = "StaticData/PopUpWindowsStaticData";
        
        private GameStaticData _gameStaticData;
        private BalanceStaticData _balanceStaticData;
        private LevelStaticData _levelStaticData;
        
        private Dictionary<WindowTypeId, WindowConfig> _windowConfigs;
        // private Dictionary<PopUpWindowTypeId, PopUpWindowConfig> _popUpWindowConfigs;

        public void LoadData()
        {
            _gameStaticData = Resources
                .Load<GameStaticData>(GameConfigPath);

            _balanceStaticData = Resources
                .Load<BalanceStaticData>(BalanceConfigPath);
            
            _windowConfigs = Resources
                .Load<WindowStaticData>(WindowsStaticDataPath)
                .Configs.ToDictionary(x => x.WindowTypeId, x => x);
            
            _levelStaticData = Resources
                .Load<LevelStaticData>(LevelStaticDataPath);
            
            // _popUpWindowConfigs = Resources
            //     .Load<PopUpWindowStaticData>(PopUpWindowsStaticDataPath)
            //     .Configs.ToDictionary(x => x.PopUpWindowTypeId, x => x);
        }

        public LevelStaticData LevelConfig() => 
            _levelStaticData;

        public BalanceStaticData Balance() => 
            _balanceStaticData;

        public WindowConfig ForWindow(WindowTypeId typeId) => 
            _windowConfigs[typeId];

        public GameStaticData GameConfig() => _gameStaticData;


        // public PopUpWindowConfig ForPopUpWindow(PopUpWindowTypeId popUpWindowTypeId) => 
        //     _popUpWindowConfigs[popUpWindowTypeId];
    }
}