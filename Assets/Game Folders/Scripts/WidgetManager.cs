using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WidgetManager : MonoBehaviour
{
    [SerializeField] private GameObject notification;
    [SerializeField] private TMP_Text label_notification;

    private void Start()
    {
        GameManager.Instance.OnNotificationUpdate += SetupNotification;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnNotificationUpdate -= SetupNotification;
    }

    public void SetupNotification(string info)
    {
        notification.SetActive(true);
        label_notification.text = info;

        StartCoroutine(HideNotification());
    }

    IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(2f);
        notification.GetComponent<Animator>().Play("hide");
        yield return new WaitForSeconds(0.5f);
        notification.SetActive(false);
    }
}
