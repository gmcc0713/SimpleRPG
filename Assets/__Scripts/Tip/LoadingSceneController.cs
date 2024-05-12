using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] Image []LoadingImage;
    [SerializeField] TextMeshProUGUI title;

    void Start()
    {
        StartCoroutine(LoadingSceneProgress());
    }


    IEnumerator LoadingSceneProgress()
    {
        switch (LoadingSceneManager.Instance.m_NextSceneName)
        {
            case "Village":
                LoadingImage[0].gameObject.SetActive(true);
                LoadingImage[1].gameObject.SetActive(false);
                title.text = "������ ����";
                break;
            case "FirstDungeonMap":
                LoadingImage[1].gameObject.SetActive(true);
                LoadingImage[0].gameObject.SetActive(false);
                title.text = "�ذ���� ����";
                
                break;
        }

        //�񵿱� ���(���� �ҷ����� ���� �ٸ��۾��� ������)
        AsyncOperation op =   SceneManager.LoadSceneAsync(LoadingSceneManager.Instance.m_NextSceneName);
        //���� �񵿱�� �ҷ����϶� ���� �ε��� ������ �ڵ����� �ҷ��� ������ �̵��Ұ��� ����(�ε��ð��� �ʹ� ª������ ���)
        op.allowSceneActivation = false;    

        float timer = 0;
        //bar�� �������� �ϱ����� op�� ������ �ʾ����� ��� ����
        while (!op.isDone)   
        {
            yield return null;
            if (op.progress < 0.9f)     //���� �ε��� 90�� ���� �ε� �ٸ� ä���
            {
                loadingBar.value = op.progress;
            }
            else                        //������ 10�۴� 1�ʵ��� ä��� ���� �ҷ��´�
            {
                timer += Time.unscaledDeltaTime;
                loadingBar.value = Mathf.Lerp(0.9f, 1f, timer);
                if(loadingBar.value >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield return null;
                }
            }

        }
    }
}
