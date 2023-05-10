using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using Cinemachine;

namespace Suv
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] GameObject _instantiateParent;
        [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;

        public static StageManager I;

        private PlayerCharacter _playerCharacter;
        public PlayerCharacter PlayerCharacter => _playerCharacter;

        private void Awake()
        {
            if (I == null)
            {
                I = this;
            }

            OnInitialize().Forget();
        }

        private async UniTask OnInitialize()
        {
            // プレイヤー生成
            await OnInstantiatePlayer();
        }

        private async UniTask OnInstantiatePlayer()
        {
            var request = Resources.LoadAsync<PlayerCharacter>(ConstStringManager.RESOURCES_PLAYER);
            await request;

            PlayerCharacter player = request.asset as PlayerCharacter;
            _playerCharacter = Instantiate(player, new Vector3(0, 0.5f, 0), Quaternion.identity, _instantiateParent.transform);
            _cinemachineVirtualCamera.Follow = player.transform;
        }
    }
}