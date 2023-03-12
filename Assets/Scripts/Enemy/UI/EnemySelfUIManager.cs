using Managers;
using ScriptableObjects.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy.UI
{
    public class EnemySelfUIManager : MonoBehaviour
    {
        private Transform _camTransform;
        public GameObject healthBar;
        public Slider healthBarSlider;
        private int _id;
        private EnemyScriptableObj _enemySo;

        private void Start()
        {
            _enemySo = Locator.Instance.enemySo;
            _camTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
            _id = GetComponent<EnemyAIController>().ID;
            healthBarSlider.maxValue = _enemySo.maxHealth;
            healthBarSlider.value = _enemySo.Enemies[_id].Health;
            _enemySo.Enemies[_id].UIUpdateNeeded += UpdateUI;
        }

        private void UpdateUI()
        {
            if(!healthBar.activeInHierarchy) healthBar.SetActive(true);
            healthBarSlider.value = _enemySo.Enemies[_id].Health;
        }

        void LateUpdate()
        {
            healthBar.transform.LookAt(healthBar.transform.position + _camTransform.forward);
        }
    }
}
