using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PannelManager : MonoBehaviour
{
    [SerializeField] private Dropdown packagechoose;
    [SerializeField] private Dropdown sleeperchoose;
    [SerializeField] private InputField host;
    [SerializeField] private InputField namebox;
    [SerializeField] private Button join;
    private void OnEnable()
    {
        packagechoose.options.Clear();
        for(int i = 0; i < SceneDataManager.instance.dataManagers.Length; i++)
        {
            var manager = SceneDataManager.instance.dataManagers[i];
            packagechoose.options.Add(new Dropdown.OptionData(manager.name));
        }
        packagechoose.onValueChanged.AddListener(PackageChange);
        namebox.text = PlayerPrefs.GetString("nickname", "");
        join.onClick.AddListener(Join);
        PackageChange(0);
    }
    private void PackageChange(int index)
    {
        var manager = SceneDataManager.instance.dataManagers[index];
        sleeperchoose.options.Clear();
        for(int i = 0; i < manager.sleepers.materials.Length; i++)
        {
            var sleeper = manager.sleepers.materials[i];
            sleeperchoose.options.Add(new Dropdown.OptionData(sleeper.mainSprite));
        }
        sleeperchoose.value = 0;
        host.text = manager.info.ipport;
    }
    private void Join()
    {
        SceneDataManager.instance.selectScene = SceneDataManager.instance.dataManagers[packagechoose.value];
        SceneDataManager.instance.sleeperMid = sleeperchoose.value;
        SceneDataManager.instance.ipport = host.text;
        SceneDataManager.instance.nickname = namebox.text;
        SceneManager.LoadSceneAsync("GameScene");
    }
}
