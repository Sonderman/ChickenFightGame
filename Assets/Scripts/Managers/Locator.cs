using ScriptableObjects;
using ScriptableObjects.Enemy;
using ScriptableObjects.Managers;
using ScriptableObjects.Player;
using UnityEngine;

namespace Managers
{
    public class Locator : MonoBehaviour
    {
        public static Locator Instance { get; private set; }
        public PlayerScriptableObj playerSo;
        public EnemyScriptableObj enemySo;
        public GameManagerSo gameManagerSo;
        public GlobalSettings globalSettingsSo;
        public ClipsDataSo clipsDataSo;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
    }
}
