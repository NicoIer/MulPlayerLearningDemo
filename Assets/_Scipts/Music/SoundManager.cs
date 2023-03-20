using System;
using Nico.DesignPattern;
using Unity.VisualScripting;
using UnityEngine;

namespace Kitchen.Music
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField] AudioClipData audioClipData;

        private void OnEnable()
        {
            var deliveryManager = DeliveryManager.Instance;
            deliveryManager.OnOrderSuccess += _OnOrderSuccess;
            deliveryManager.OnOrderFailed += _OnOrderFailed;
            CuttingCounter.OnAnyCut += _CuttingCounter_OnAnyCut;
            Player.Player.Instance.OnPickUpSomeThing += _Player_On_PickUpSomeThing;
            Player.Player.Instance.onMoving += _Player_OnMoving;
            BaseCounter.OnAnyObjPlaceOnCounter += _BaseCounter_OnAnyObjPlaceOnCounter;
            TrashCounter.OnAnyObjTrashed += _TrashCounter_OnAnyObjTrashed;


        }

        private void OnDisable()
        {

                DeliveryManager.Instance.OnOrderSuccess -= _OnOrderSuccess;
                DeliveryManager.Instance.OnOrderFailed -= _OnOrderFailed;
                CuttingCounter.OnAnyCut -= _CuttingCounter_OnAnyCut;
                Player.Player.Instance.OnPickUpSomeThing -= _Player_On_PickUpSomeThing;
                Player.Player.Instance.onMoving -= _Player_OnMoving;
                BaseCounter.OnAnyObjPlaceOnCounter -= _BaseCounter_OnAnyObjPlaceOnCounter;
                TrashCounter.OnAnyObjTrashed -= _TrashCounter_OnAnyObjTrashed;


        }


        #region Play Sound

        private void _PlaySound(AudioClip audioClip, Vector3 position)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, _volume);
        }

        private void _PlaySound(AudioClip[] audioClips, Vector3 position)
        {
            var audioClip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
            _PlaySound(audioClip, position);
        }

        #endregion

        #region Events

        private void _Player_OnMoving(Vector3 position)
        {
            //TODO 由于玩家每帧都在动 会导致生成的声音数量太多 听起来卡卡的
            //设置一个interval
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
            _PlaySound(audioClipData.pickUp, Player.Player.Instance.transform.position);
        }

        private void _CuttingCounter_OnAnyCut(object sender, Vector3 position)
        {
            _PlaySound(audioClipData.chop, position);
        }

        #endregion

        private float _volume = 1f;

        private void ChangeVolume(float volume)
        {
            _volume = Mathf.Clamp(volume, 0, 1);
        }
    }
}