using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.Buttons
{
    public class TapToStartButton : MonoBehaviour, IPointerClickHandler
    {
        private IGameCurator _gameCurator;

        [Inject]
        public void Constructor(IGameCurator gameCurator)
        {
            _gameCurator = gameCurator;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _gameCurator.StartGame();
            gameObject.SetActive(false);
        }
    }
}