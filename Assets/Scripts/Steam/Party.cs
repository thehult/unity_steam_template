using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facepunch.Steamworks;

public class Party : MonoBehaviour {

    public int maxPartySize;
    public GameObject partyPlayerPrefab;
    public GameObject container;

	// Use this for initialization
	void Start () {
        Client.Instance.Lobby.OnLobbyMemberDataUpdated = (ulong uid) =>
        {
            UpdateParty();
        };
        Client.Instance.Lobby.OnLobbyCreated = (bool success) =>
        {
            Debug.Log("Created Party!");
            Client.Instance.Lobby.CurrentLobbyData.SetData("state", "inmenu");
            UpdateParty();
        };
        Client.Instance.Lobby.OnLobbyJoined = (bool success) =>
        {
            Debug.Log("Joined Party!");
            UpdateParty();
        };
        Client.Instance.Lobby.OnUserInvitedToLobby = (ulong lobbyId, ulong steamId) =>
        {
            Debug.Log("Invited player " + Client.Instance.Friends.GetName(steamId));
        };
        Client.Instance.Lobby.OnLobbyDataUpdated = () =>
        {
            Debug.Log("Lobby data updated");
            foreach(KeyValuePair<string, string> data in Client.Instance.Lobby.CurrentLobbyData.GetAllData())
            {
                Debug.Log(data.Key + ": " + data.Value);
            }
        };
        Client.Instance.Lobby.OnChatStringRecieved = (ulong uid, string msg) =>
        {
            Debug.Log("MESSAGE: " + msg);
        };
        UpdateParty();
	}
	

    void UpdateParty()
    {
        Debug.Log("Update Party!");
        if (!Client.Instance.Lobby.IsValid)
        {
            Debug.Log("derp!?");
            Client.Instance.Lobby.Create(Lobby.Type.Invisible, maxPartySize);
            return;
        }
        else
        {
            Debug.Log("Herp!?");
            ulong[] memberIds = Client.Instance.Lobby.GetMemberIDs();
            foreach (ulong memberId in memberIds)
            {
                SteamFriend friend = Client.Instance.Friends.Get(memberId);
                GameObject go = Instantiate(partyPlayerPrefab);
                go.transform.SetParent(container.transform, false);
                go.transform.SetSiblingIndex(go.transform.GetSiblingIndex() - 1);
                go.GetComponent<RawImage>().texture = Utils.ConvertSteamImage(friend.GetAvatar(Friends.AvatarSize.Small));
            }
        }
    }

}
