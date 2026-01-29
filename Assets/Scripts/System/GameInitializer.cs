using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private ItemDataBase itemDatabase;

    void Awake()
    {       
        ItemDataBase.Init(itemDatabase);        
        DontDestroyOnLoad(gameObject);
    }
}