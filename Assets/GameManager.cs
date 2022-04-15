using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {


        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        #endregion


        #region Monobehaviour Callbacks

        void Update()
        {
            
        }

        #endregion


        #region Private Methods

        private PhotonView pv;
        private PlayerProperty pp;

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            // PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("SocialSpace");
        }

        private void Start()
        {
            pv = GetComponent<PhotonView>();
            pp = GameObject.Find("PlayerProperty").GetComponent<PlayerProperty>();
            
            if (vrPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);

                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    if (pp.type == 0 && pv.IsMine)
                    {
                        GameObject player = PhotonNetwork.Instantiate(avatar.name, xrRig.transform.position, avatar.transform.rotation, 0);
                        // player.transform.parent = xrRig.transform.GetChild(0);

                        // GameObject player = PhotonNetwork.Instantiate(vrPrefab.name, vrPrefab.transform.position + new Vector3(0.0f, 0.0f, -5.0f), vrPrefab.transform.rotation, 0);
                        // player.transform.name = "XR Origin";
                        // GameObject player = Instantiate(vrPrefab);
                        // GameObject tmp = PhotonNetwork.Instantiate(this.avatarGrab.name, this.avatarGrab.transform.position, Quaternion.identity, 0);
                        // tmp.transform.name = PhotonNetwork.NickName;
                        // player.transform.parent = tmp.transform;
                    }
                }
            }
        }

        #endregion


        #region Public Methods

        [Tooltip("The prefab to use for representing the player")]
        public GameObject vrPrefab;
        public GameObject pcPrefab;
        
        public GameObject xrRig;
        public GameObject avatar;

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion
    }
}