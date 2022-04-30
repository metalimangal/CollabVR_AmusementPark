using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
    public GameObject localXRRigObject;
    public GameObject MainAvatarGameObject;

    public GameObject AvatarHeadObject;
    public GameObject AvatarBodyObject;

    public GameObject[] AvatarModelPrefabs;

    public TMP_Text PlayerName;
    void Start()
    {
        if (photonView.IsMine)
        {
            localXRRigObject.SetActive(true);

            object avatarSelectionNumber;

            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CollabVRConstants.AVATAR_SELECTION_NUMBER, out avatarSelectionNumber))
            {
                Debug.Log("Selection num:" + (int)avatarSelectionNumber);
                photonView.RPC("InitializeSelectedAvatarModel", RpcTarget.AllBuffered, (int)avatarSelectionNumber);
            }

            SetLayerRecursively(AvatarHeadObject, 6);
            SetLayerRecursively(AvatarBodyObject, 7);

            MainAvatarGameObject.AddComponent<AudioListener>();
        }
        else
        {
            localXRRigObject.SetActive(false);
            SetLayerRecursively(AvatarHeadObject, 0);
            SetLayerRecursively(AvatarBodyObject, 0);
        }

        if(PlayerName != null)
        {
            PlayerName.text = photonView.Owner.NickName;
        }
    }

    
    void Update()
    {
        
    }
    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    [PunRPC]
    public void InitializeSelectedAvatarModel(int avatarSelectionNumber)
    {
        GameObject selectedAvatarGameobject = Instantiate(AvatarModelPrefabs[avatarSelectionNumber], localXRRigObject.transform);

        AvatarInputConverter avatarInputConverter = localXRRigObject.GetComponent<AvatarInputConverter>();
        AvatarHolder avatarHolder = selectedAvatarGameobject.GetComponent<AvatarHolder>();
        SetUpAvatarGameobject(avatarHolder.HeadTransform, avatarInputConverter.AvatarHead);
        SetUpAvatarGameobject(avatarHolder.BodyTransform, avatarInputConverter.AvatarBody);
        SetUpAvatarGameobject(avatarHolder.HandLeftTransform, avatarInputConverter.AvatarHand_Left);
        SetUpAvatarGameobject(avatarHolder.HandRightTransform, avatarInputConverter.AvatarHand_Right);
    }

    void SetUpAvatarGameobject(Transform avatarModelTransform, Transform mainAvatarTransform)
    {
        avatarModelTransform.SetParent(mainAvatarTransform);
        avatarModelTransform.localPosition = Vector3.zero;
        avatarModelTransform.localRotation = Quaternion.identity;
    }
}
