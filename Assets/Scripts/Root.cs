using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private FightController fightController;
    [SerializeField] private ConfigManager configManager;

    private static Root instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        fightController.Run();
    }

    public static FightController FightController => instance.fightController;
    public static ConfigManager ConfigManager => instance.configManager;
}
