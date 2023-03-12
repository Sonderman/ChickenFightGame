using UnityEngine;

namespace ScriptableObjects.Managers
{
    [CreateAssetMenu(fileName = "ClipsDataSo", menuName = "ScriptableObjects/ClipsDataSo")]
    public class ClipsDataSo : ScriptableObject
    {
        public AudioClip puchClip;
        public AudioClip playerDamageTakenClip;
        public AudioClip takingScoreClip;
        public AudioClip levelFailClip;
        public AudioClip levelCompleteClip;
    }
}
