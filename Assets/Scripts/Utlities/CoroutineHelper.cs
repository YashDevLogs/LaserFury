using System.Collections;
using UnityEngine;

// a Script which start a coroutine in a Non-Monobehaviour script.
public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper _instance;

    public static CoroutineHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("CoroutineHelper");
                _instance = obj.AddComponent<CoroutineHelper>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    public void StartHelperCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
