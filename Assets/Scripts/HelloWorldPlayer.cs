using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public NetworkVariable<Color> color;

        public NetworkVariable<int> prevTeam;

        public static List<int> teams;

        public Renderer ren;
        public bool firstTime = true;

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
                
            }
            firstTime = false;
        }

        public void Move(int team = 0)
        {
            
            SubmitPositionRequestServerRpc(team);
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(int team = 0, ServerRpcParams rpcParams = default)
        {
            Debug.Log("Valor de firstTime: "+firstTime);
            if(teams[team]==2 && team != 0){
                Debug.Log("Equipo lleno");
            }else {
                Position.Value = GetRandomPositionOnPlane(team);
                AsignTeamColor(team);
                if(teams[prevTeam.Value]>0 && !firstTime){

                    teams[prevTeam.Value]--;
                }
                prevTeam.Value = team;
                teams[team]++;
                Debug.Log("Jugadores en equipo: "+teams[team]);
            }
        }

        void AsignTeamColor(int team = 0){

            if(team == 0){
                color.Value = Color.white;    
            }else if(team == 1){
                color.Value = Color.blue;
            }else if(team == 2){
                color.Value = Color.red;
            }

            Debug.Log("He entrado en AsignTeamColor y el valor de team es "+team);
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

        void OnPositionChanged(Vector3 oldPos, Vector3 newPos){

            transform.position = Position.Value;
        }

        void OnColorChanged(Color oldColor, Color newColor){

            ren.material.color = color.Value;
        }

        void Awake() {

            ren = GetComponent<Renderer>();
            teams = new List<int>();
            for(int i = 0; i<3; i++){
                teams.Add(0);
            }
        }

        void Start(){

            Position.OnValueChanged += OnPositionChanged;
            color.OnValueChanged += OnColorChanged;
        }
    }
}