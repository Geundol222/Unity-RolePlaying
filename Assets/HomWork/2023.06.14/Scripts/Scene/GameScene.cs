using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeWork0614
{
    public class GameScene : BaseScene
    {
        public GameObject player;
        public Transform playerPosition;

        protected override IEnumerator LoadingRoutine()
        {
            progress = 0f;
            Debug.Log("���� �� ����");
            yield return new WaitForSecondsRealtime(1f);

            progress = 0.2f;
            Debug.Log("���� ���� ����");
            yield return new WaitForSecondsRealtime(1f);

            progress = 0.4f;
            Debug.Log("���� ������ ����");
            yield return new WaitForSecondsRealtime(1f);

            progress = 0.6f;
            Debug.Log("�÷��̾� ��ġ");
            player.transform.position = playerPosition.position;
            player.transform.rotation = playerPosition.rotation;
            yield return new WaitForSecondsRealtime(1f);

            progress = 1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}