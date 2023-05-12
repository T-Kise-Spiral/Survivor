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

        private DamageUIManager _damageUIManager;
        public DamageUIManager DamageUIManager => _damageUIManager;

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
            await OnInstantiateDamageUIManager();
        }

        private async UniTask OnInstantiatePlayer()
        {
            var request = Resources.LoadAsync<PlayerCharacter>(ConstStringManager.RESOURCES_PLAYER);
            await request;

            PlayerCharacter player = request.asset as PlayerCharacter;
            _playerCharacter = Instantiate(player, new Vector3(0, 0.5f, 0), Quaternion.identity, _instantiateParent.transform);

            // プレイヤーの子供にVirtualCameraがあるので、エディタ用のカメラは無効にする
            _cinemachineVirtualCamera.gameObject.SetActive(false);
        }

        private async UniTask OnInstantiateDamageUIManager()
        {
            var request = Resources.LoadAsync<DamageUIManager>(ConstStringManager.RESOURCES_DAMAGE_UI_MANAGER);
            await request;

            DamageUIManager damageUIManager = request.asset as DamageUIManager;
            _damageUIManager = Instantiate(damageUIManager, Vector3.zero, Quaternion.identity, _instantiateParent.transform);
        }


        private void OnDestroy()
        {
            I = null;
        }
    }
}