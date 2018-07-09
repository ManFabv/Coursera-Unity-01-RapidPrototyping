using UnityEngine;

public class LoaderManager : MonoBehaviour
{
    private LoaderManager() { }

    private static LoaderManager _instance;
    private static GameObject _template;
    private static GameObject loader;

    private void Start()
    {
        LoaderManager.SetInstance();
    }

    private static void SetInstance()
    {
        if (_template == null)
        {
            _template = Resources.Load<GameObject>(GameplayConstants.LOADER_NAME);
        }

        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<LoaderManager>();

            if (_instance != null)
            {
                LoaderManager[] loaders = GameObject.FindObjectsOfType<LoaderManager>();

                if (loaders != null && loaders.Length > 1)
                {
                    for (int i = 0; i < loaders.Length; i++)
                    {
                        if (loaders[i] != _instance)
                            GameObject.Destroy(loaders[i].gameObject);
                    }
                }
            }

            else
            {
                GameObject loader_aux = new GameObject(GameplayConstants.LOADER_NAME);

                _instance = loader_aux.AddComponent<LoaderManager>();
            }

            if (_instance != null)
                GameObject.DontDestroyOnLoad(_instance);
        }
    }

    public static LoaderManager Instance
    {
        get
        {
            SetInstance();

            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

	public void StartLoader()
    {
        if(_template != null && Instance != null && loader == null)
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();

            if(canvas != null)
            {
                loader = GameObject.Instantiate<GameObject>(_template);

                loader.transform.SetParent(canvas.transform);

                loader.transform.SetAsLastSibling();

                RectTransform rectLoader = loader.GetComponent<RectTransform>();

                if(rectLoader != null)
                {
                    rectLoader.anchoredPosition = new Vector2(0, 0);
                    rectLoader.localScale = new Vector3(1, 1, 1);

                    rectLoader.offsetMin = new Vector2(-10, -10);
                    rectLoader.offsetMax = new Vector2(10, 10);
                }

                Animator animLoader = loader.GetComponent<Animator>();

                if (animLoader != null)
                    animLoader.SetTrigger("LoaderStart");
            }
        }
    }

    public void DestroyLoader()
    {
        if (loader != null && Instance != null)
            GameObject.Destroy(loader);
    }
}
