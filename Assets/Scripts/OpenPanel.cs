using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    public GameObject FirstPanel;
    public GameObject SecondPanel;

    public void OpenThePanel()
    {
        FirstPanel.SetActive(false);
        SecondPanel.SetActive(true);
        SoundManager.instance.PlaySFX("btnSound");

    }

}
