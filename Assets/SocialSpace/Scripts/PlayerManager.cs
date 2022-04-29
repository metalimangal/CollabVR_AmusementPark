using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Collections;

namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// Player manager.
    /// Handles fire Input and Beams.
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion

        #region Private Fields

        private string[] userColor = { "Red",  "Blue", "Green", "White", "Black" };

        #endregion


        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;

                int characterColor = (int)Random.Range(0.0f, userColor.Length);

                set_skinned_mat("LeftHand Controller", 0, Resources.Load("Materials/" + userColor[characterColor], typeof(Material)) as Material);
                set_skinned_mat("RightHand Controller", 0, Resources.Load("Materials/" + userColor[characterColor], typeof(Material)) as Material);
                //transform.GetChild(1).GetComponent<OVRManager>().enabled = true;
            }

            else
            {
                transform.name = "other player";
            }

            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            ProcessInputs();
        }

        #endregion

        #region Custom

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        void ProcessInputs()
        {

        }

        void set_skinned_mat(string obj_name, int Mat_Nr, Material Mat)
        {
            GameObject obj = GameObject.Find(obj_name);

            SkinnedMeshRenderer renderer = obj.GetComponentInChildren<SkinnedMeshRenderer>();

            Material[] mats = renderer.materials;

            mats[Mat_Nr] = Mat;

            renderer.materials = mats;
        }

        #endregion
    }
}