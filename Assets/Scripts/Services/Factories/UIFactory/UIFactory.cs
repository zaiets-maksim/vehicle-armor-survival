using Services.StaticDataService;
using StaticData.Configs;
using UnityEngine;
using Zenject;

namespace Services.Factories.UIFactory
{
    public class UIFactory : Factory, IUIFactory
    {
        private const string UiRootPath = "Prefabs/UI/UiRoot";
        private const string HudPath = "Prefabs/UI/Hud";
        private const string PopUpMarketPath = "Prefabs/UI/PopUpMarket/PopUpMarket";
        private const string KitchenItemElementPath = "Prefabs/UI/PopUpMarket/KitchenItemElement";
        private const string HallItemElementPath = "Prefabs/UI/PopUpMarket/HallItemElement";
        private const string UpgradeElementPath = "Prefabs/UI/PopUpMarket/UpgradeElement";
        private const string StuffElementPath = "Prefabs/UI/PopUpMarket/StuffElement";

        private readonly IStaticDataService _staticData;
        
        private Transform _uiRoot;

        public UIFactory(IInstantiator instantiator, IStaticDataService staticDataService) : base(instantiator)
        {
            _instantiator = instantiator;
            _staticData = staticDataService;
        }

        public void CreateUiRoot() => _uiRoot = InstantiateOnActiveScene(UiRootPath).transform;

        public void CreateHud() => InstantiateOnActiveScene(HudPath);

        public RectTransform CreateWindow(WindowTypeId windowTypeId)
        {
            WindowConfig config = _staticData.ForWindow(windowTypeId);
            var window = InstantiatePrefab(config.Prefab, _uiRoot).GetComponent<RectTransform>();
            
            return window;
        }
    }
}