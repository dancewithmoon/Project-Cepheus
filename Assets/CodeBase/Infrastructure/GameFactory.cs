using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;

        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }

        public GameObject CreateHero(GameObject initialPoint)
        {
            return _assets.Instantiate(AssetPath.HeroPath, initialPoint.transform.position);
        }

        public GameObject CreateHud()
        {
            return _assets.Instantiate(AssetPath.HudPath);
        }
    }
}