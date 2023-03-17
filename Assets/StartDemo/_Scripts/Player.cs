using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MulPlayerGame
{
    public class Player : NetworkBehaviour
    {
        private StanderInput input;

        private NetworkVariable<int> health = new NetworkVariable<int>(0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public float moveSpeed = 3f;

        private void Update()
        {
            if (IsOwner)
            {
                var move2D = input.Player.move2D.ReadValue<Vector2>();
                if (move2D != Vector2.zero)
                {
                    move2D = move2D * moveSpeed * Time.deltaTime;
                    transform.position += new Vector3(move2D.x, 0, move2D.y);
                }


                if (Keyboard.current.upArrowKey.wasPressedThisFrame)
                {
                    health.Value++;
                }

                if (Keyboard.current.tKey.wasPressedThisFrame)
                {
                    TestServerRpc();
                }

                if (Keyboard.current.rKey.wasPressedThisFrame)
                {
                    TestClientRpc(new ClientRpcParams
                    {
                        Send = new ClientRpcSendParams
                        {
                            //发送给谁 指定执行操作的目标客户端
                            TargetClientIds = new ulong[] { OwnerClientId }
                        }
                    });
                }
            }
        }


        [ServerRpc]
        private void TestServerRpc()
        {
            Debug.Log($"client-id:{OwnerClientId}->:HealthChangeServerRpc");
        }

        [ClientRpc]
        private void TestClientRpc(ClientRpcParams clientRpcParams)
        {
            Debug.Log($"TestClientRpc");
        }

        private void Awake()
        {
            input = new StanderInput();
            health.OnValueChanged += (oldValue, newValue) =>
            {
                Debug.Log($"client-id:{OwnerClientId} health:{newValue}");
            };
        }

        private void OnEnable()
        {
            input.Player.Enable();
        }

        private void OnDisable()
        {
            input.Player.Disable();
        }
    }
}