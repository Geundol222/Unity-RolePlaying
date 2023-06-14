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
            Debug.Log("랜덤 맵 생성");
            yield return new WaitForSecondsRealtime(1f);

            progress = 0.2f;
            Debug.Log("랜덤 몬스터 생성");
            yield return new WaitForSecondsRealtime(1f);

            progress = 0.4f;
            Debug.Log("랜덤 아이템 생성");
            yield return new WaitForSecondsRealtime(1f);

            progress = 0.6f;
            Debug.Log("플레이어 배치");
            player.transform.position = playerPosition.position;
            player.transform.rotation = playerPosition.rotation;
            yield return new WaitForSecondsRealtime(1f);

            progress = 1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}