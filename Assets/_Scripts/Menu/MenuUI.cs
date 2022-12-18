using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _noNameText;
    [SerializeField] private UnitPlayer _unitPlayer;

    public TMP_InputField enterName;

    public void StartGame()
    {
        if (enterName.text.Length > 0 && enterName.text.Length <= 12)
        {
            _unitPlayer.unitName = enterName.text;
            SceneManager.LoadScene(1);
        }
        else
        {
            _noNameText.SetActive(true);
            StartCoroutine(TimerForText());
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#else  
        Application.Quit();
#endif
    }

    IEnumerator TimerForText()
    {
        yield return new WaitForSeconds(2f);
        _noNameText.SetActive(false);
    }
}
