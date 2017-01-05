using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
//Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
public class MenuPlayer : MonoBehaviour
{
    public static MenuPlayer current;
    static Color[] Colors = new Color[] { Color.white, Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    public Button colorButton;
    public Color playerColor = Color.white;
    public string playerName;

    InputField playerNameInput;
    
    void Start()
    {
        MenuPlayer.current = this;

        playerNameInput = GameObject.Find("PlayerName").GetComponent<InputField>();

        colorButton.interactable = true;        
        colorButton.onClick.RemoveAllListeners();
        colorButton.onClick.AddListener(OnColorClicked);
    }

    void Update()
    {
        colorButton.GetComponent<Image>().color = playerColor;
        playerName = playerNameInput.text;
    }
    
    public void OnColorClicked()
    {
        int idx = System.Array.IndexOf(Colors, playerColor);
        if (idx < 0) idx = 0;
        idx = (idx + 1) % Colors.Length;

        playerColor = Colors[idx];
    }
    
}
