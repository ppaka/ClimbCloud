using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    public BossRoomEnterController enterController;
    public Transform allObj;
    public Another boss;

    private GameObject _playerCam;
    private PlayerController[] _playerControllers;

    private bool _lockToScreenAllPlayers;

    private void Start()
    {
        allObj.gameObject.SetActive(false);
        _playerCam = GameObject.Find("CM vcam1");
    }

    private void Update()
    {
        if (_lockToScreenAllPlayers)
        {
            foreach (var player in _playerControllers)
            {
                var viewPos = Camera.main.WorldToViewportPoint(player.transform.position); // 오브젝트의 좌표를 뷰포트 좌표로...
                viewPos.x = Mathf.Clamp01(viewPos.x); // 위에서 받아온 좌표의 x값을 가공
                viewPos.y = Mathf.Clamp01(viewPos.y); // 위에서 받아온 좌표의 y값을 가공

                var worldPos = Camera.main.ViewportToWorldPoint(viewPos); // 뷰포트 좌표를 월드 좌표로...
                player.transform.position = worldPos; // 오브젝트에 월드좌표를 적용
            }
        }
    }

    public void ActiveBoss()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(2);
        boss.gameObject.SetActive(true);
        boss.virtualCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        DialogueManager.Instance.FadeIn();
        DialogueManager.Instance.Open();
        yield return new WaitForSeconds(0.2f);
        yield return new DOTweenCYInstruction.WaitForCompletion(DialogueManager.Instance.SetText("..."));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new DOTweenCYInstruction.WaitForCompletion(DialogueManager.Instance.SetText(".........."));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        DialogueManager.Instance.FadeOut();
        yield return new DOTweenCYInstruction.WaitForCompletion(DialogueManager.Instance.Close());
        DialogueManager.Instance.dialogText.text = "";
        _playerCam.SetActive(false);
        boss.virtualCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        _playerControllers = FindObjectsOfType<PlayerController>();
        foreach (var player in _playerControllers)
        {
            player.canMove = true;
            player.hpCanvasObject.SetActive(true);
        }
        _lockToScreenAllPlayers = true;
        boss.StartAttack();
    }
}
