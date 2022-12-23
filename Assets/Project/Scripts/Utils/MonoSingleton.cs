using UnityEngine;

    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static volatile T instance;

        public static T Instance
        {
            get
            {
                if ((bool) (Object) MonoSingleton<T>.instance)
                    return MonoSingleton<T>.instance;
                MonoSingleton<T>.instance = Object.FindObjectOfType<T>();
                if (!(bool) (Object) MonoSingleton<T>.instance)
                    MonoSingleton<T>.instance = new GameObject(nameof (T)).AddComponent<T>();
                return MonoSingleton<T>.instance;
            }
        }
    }