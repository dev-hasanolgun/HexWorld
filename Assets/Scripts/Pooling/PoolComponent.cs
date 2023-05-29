using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pooling
{
    public class PoolComponent : MonoBehaviour
    {
        private void Awake()
        {
            if (PoolInstance.Instance == null)
            {
                PoolInstance.Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PoolManagerTracker.ClearPoolDictionary();
            PoolInstance.CreateParent();
        }
    
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}