using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFootCircle : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 0.1f; // 크기 증가 속도
     [SerializeField]private float scaleMax = 6f; // 크기 증가 속도
    
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

            // x와 z 크기만 증가하도록 함
            float newScaleX = currentScale.x + scaleSpeed * Time.deltaTime;
            float newScaleZ = currentScale.z + scaleSpeed * Time.deltaTime;

            // 새로운 크기를 적용하여 적용
            transform.localScale = new Vector3(newScaleX, currentScale.y, newScaleZ);
            yield return null;
        }
        rockLauncher.FootAttackEnd();
        ResetData();
    }
}
