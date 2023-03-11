using ScriptableObjects;
using ScriptableObjects.Enemy;
using ScriptableObjects.Player;
using UnityEngine;

public class Locator : MonoBehaviour
{
    public static Locator Instance { get; private set; }
    public PlayerScriptableObj playerSo;
    public EnemyScriptableObj enemySo;
    public GameManagerSo gameManagerSo;
    public GlobalSettings globalSettingsSo;
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
