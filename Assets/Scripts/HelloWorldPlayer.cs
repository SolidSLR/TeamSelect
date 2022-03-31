using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public NetworkVariable<Color> color;

        public static List<GameObject> leftTeam;

        public static List<GameObject> rightTeam;

        public Renderer ren;

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
            }
        }

        public void Move(int team = 0)
        {

            if(leftTeam.Count >= 2){
                team = 0;
                Debug.Log("El equipo azul está lleno, sowwy :(");
            }

            if(rightTeam.Count >= 2){
                team = 0;
                Debug.Log("El equipo rojo está lleno, sowwy :(");
            }

            SubmitPositionRequestServerRpc(team);
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(int team = 0, ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane(team);

            // Usar esta forma en lugar de actualizar en el update cuando hayas metido los eventos
            //transform.position = Position.Value;
            AsignTeamColor(team);

            if(team == 1){
                leftTeam.Add(gameObject);
            }

            if(team == 2){
                rightTeam.Add(gameObject);
            }

            Debug.Log("Miembros de equipo azul: "+leftTeam.Count);
            Debug.Log("Miembros de equipo rojo: "+rightTeam.Count);
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

            leftTeam = new List<GameObject>();

            rightTeam = new List<GameObject>();

        }

        void Start(){

            Position.OnValueChanged += OnPositionChanged;
            color.OnValueChanged += OnColorChanged;
        }

        void Update()
        {
            //transform.position = Position.Value;
            //ren.material.color = color.Value;
        }
    }
}