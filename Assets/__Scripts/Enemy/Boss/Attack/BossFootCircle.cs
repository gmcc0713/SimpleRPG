using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFootCircle : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 0.1f; // ũ�� ���� �ӵ�
     [SerializeField]private float scaleMax = 6f; // ũ�� ���� �ӵ�
    
    [SerializeField]private BossRockLauncher rockLauncher;
    public void ResetData()
    {
        Debug.Log("reset");
        gameObject.SetActive(false);
       transform.localScale = Vector3.one;
    }
    public void Run()
    {
        gameObject.SetActive(true);
        StartCoroutine(CircleRun());
    }
    public IEnumerator CircleRun()
    {
        yield return new WaitForSeconds(1.7f);
        AudioManager.Instance.PlaySFX(11);
        Vector3 currentScale = transform.localScale;
        while (currentScale.x<= scaleMax) 
        {
            currentScale = transform.localScale;

            // x�� z ũ�⸸ �����ϵ��� ��
            float newScaleX = currentScale.x + scaleSpeed * Time.deltaTime;
            float newScaleZ = currentScale.z + scaleSpeed * Time.deltaTime;

            // ���ο� ũ�⸦ �����Ͽ� ����
            transform.localScale = new Vector3(newScaleX, currentScale.y, newScaleZ);
            yield return null;
        }
        rockLauncher.FootAttackEnd();
        ResetData();
    }
}
