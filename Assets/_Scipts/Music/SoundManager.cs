using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Kitchen.Music
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioClipData audioClipData;
        private bool _listening;

        private void OnEnable()
        {
            _SubscribeEvent();
        }

        private void Start()
        {
            _SubscribeEvent();
        }

        private void _SubscribeEvent()
        {
            try
            {
                if (_listening) return;

                var deliveryManager = DeliveryManager.Singleton;
                deliveryManager.OnOrderSuccess += _OnOrderSuccess;
                deliveryManager.OnOrderFailed += _OnOrderFailed;
                CuttingCounter.OnAnyCut += _CuttingCounter_OnAnyCut;
                Player.Player.Singleton.OnPickUpSomeThing += _Player_On_PickUpSomeThing;
                BaseCounter.OnAnyObjPlaceOnCounter += _BaseCounter_OnAnyObjPlaceOnCounter;
                TrashCounter.OnAnyObjTrashed += _TrashCounter_OnAnyObjTrashed;
                Player.Player.Singleton.onMoving += _Player_OnMoving;
                _listening = true;
            }
            catch (NullReferenceException)
            {
                //由于不同对象的OnEnable调用顺序不同，可能会出现DeliveryManager还没有初始化的情况
                //因为OnEnable会在Awake之前调用，所以可能会出现DeliveryManager还没有初始化的情况
            }
        }

        private void OnDisable()
        {
            try
            {
                DeliveryManager.Singleton.OnOrderSuccess -= _OnOrderSuccess;
                DeliveryManager.Singleton.OnOrderFailed -= _OnOrderFailed;
                CuttingCounter.OnAnyCut -= _CuttingCounter_OnAnyCut;
                Player.Player.Singleton.OnPickUpSomeThing -= _Player_On_PickUpSomeThing;
                BaseCounter.OnAnyObjPlaceOnCounter -= _BaseCounter_OnAnyObjPlaceOnCounter;
                TrashCounter.OnAnyObjTrashed -= _TrashCounter_OnAnyObjTrashed;
                Player.Player.Singleton.onMoving -= _Player_OnMoving;

                _listening = false;
            }
            catch (NullReferenceException)
            {
            }
        }


        #region Play Sound

        private void _PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, volume);
        }

        private void _PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
        {
            var audioClip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
            _PlaySound(audioClip, position, volume);
        }

        #endregion


        private void _Player_OnMoving(Vector3 position)
        {
            //TODO 由于玩家每帧都在动 会导致生成的声音数量太多 听起来卡卡的
            _PlaySound(audioClipData.footStep, position);
        }

        private void _OnOrderSuccess(object sender, Vector3 position)
        {
            _PlaySound(audioClipData.deliverySuccess, position);
        }

        private void _OnOrderFailed(object sender, Vector3 position)
        {
            _PlaySound(audioClipData.deliveryFail, position);
        }


        private void _TrashCounter_OnAnyObjTrashed(Vector3 position)
        {
            _PlaySound(audioClipData.trash, position);
        }

        private void _BaseCounter_OnAnyObjPlaceOnCounter(object sender, Vector3 position)
        {
            _PlaySound(audioClipData.drop, position);
        }

        private void _Player_On_PickUpSomeThing(object sender, EventArgs e)
        {
            _PlaySound(audioClipData.pickUp, Player.Player.Singleton.transform.position);
        }

        private void _CuttingCounter_OnAnyCut(object sender, Vector3 position)
        {
            _PlaySound(audioClipData.chop, position);
        }
    }
}