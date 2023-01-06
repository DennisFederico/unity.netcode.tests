using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld {

    public class HelloWorldPlayer : NetworkBehaviour {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn() {
            if (IsOwner) {
                Move();
            }
            Position.OnValueChanged += ChangePosition;
        }

        private void ChangePosition(Vector3 oldPlayerState, Vector3 newPlayerState) {
            transform.position = newPlayerState;
        }

        private NetworkVariable<Vector3>.OnValueChangedDelegate PositionChange() {
            throw new System.NotImplementedException();
        }

        public void Move() {
            if (NetworkManager.Singleton.IsServer) {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            } else {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        private void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default) {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane() {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        // void Update() {
        //     //TODO Any OnChange or listener instead of Update for every frame?
        //     transform.position = Position.Value;
        // }
    }
}
