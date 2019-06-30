using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private FightController fightController;
    [SerializeField] private ConfigManager configManager;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerInput playerInput;

    private static Root instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        cameraController.Init();
        uiManager.Init();
        fightController.Run();
    }

    public static FightController FightController => instance.fightController;
    public static ConfigManager ConfigManager => instance.configManager;
    public static CameraController CameraController => instance.cameraController;
    public static UIManager UIManager => instance.uiManager;

    public static PlayerInput PlayerInput => instance.playerInput;
}
