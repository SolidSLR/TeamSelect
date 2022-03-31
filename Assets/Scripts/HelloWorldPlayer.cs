using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public NetworkVariable<Color> color;

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            /*if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }*/

            SubmitPositionRequestServerRpc();
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(int team = 0, ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane(team);

            AsignTeamColor(team);
            // Usar esta forma en lugar de actualizar en el update cuando hayas metido los eventos
            //transform.position = Position.Value;
        }

        void AsignTeamColor(int team){

            if(team == 0){

            }
        }

        static Vector3 GetRandomPositionOnPlane(int team = 0)
        {
            if(team == 0){
                return new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-3f, 3f));
            }else if(team == 1){
                return new Vector3(Random.Range(-3f, -1f), 1f, Random.Range(-3f, 3f));
            }else{
                return new Vector3(Random.Range(1f, 3f), 1f, Random.Range(-3f, 3f));
            }
        }

        void Update()
        {
            transform.position = Position.Value;
        }
    }
}