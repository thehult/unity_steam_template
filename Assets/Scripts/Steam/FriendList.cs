using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facepunch.Steamworks;

public class FriendList : MonoBehaviour {

    public GameObject container;
    public GameObject friendPrefab;

    void Start()
    {
        UpdateFriends();
    }

    public void UpdateFriends() {
        foreach (Transform child in container.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (SteamFriend friend in Client.Instance.Friends.AllFriends.Where(x => x.IsPlayingThisGame && !x.IsBlocked).OrderBy(x => x.Name))
        {
            GameObject go = InsertFriend(friend);
            go.GetComponent<UnityEngine.UI.Image>().color = UnityEngine.Color.yellow;
        }
        foreach (SteamFriend friend in Client.Instance.Friends.AllFriends.Where( x => !x.IsPlayingThisGame && (x.IsOnline || x.IsBusy || x.IsAway || x.IsPlaying || x.IsSnoozing)).OrderBy(x => x.Name))
        {
            InsertFriend(friend);
        }
        
    }

    GameObject InsertFriend(SteamFriend friend)
    {
        GameObject go = Instantiate(friendPrefab);
        go.transform.SetParent(container.transform, false);
        Facepunch.Steamworks.Image image = friend.GetAvatar(Friends.AvatarSize.Small);

        var texture = Utils.ConvertSteamImage(image);

        var rawImage = go.GetComponentInChildren<RawImage>();
        if (rawImage != null)
            rawImage.texture = texture;
        go.GetComponentInChildren<Text>().text = friend.Name;
        go.name = "Friend_" + friend.Name;
        go.GetComponentInChildren<Button>().onClick.AddListener(delegate { InviteFriend(friend.Id); });
        return go;
    }

    void InviteFriend(ulong uid)
    {

        Debug.Log("Sending invite to " + Client.Instance.Friends.GetName(uid));
        Client.Instance.Lobby.InviteUserToLobby(uid);
    }

    public void ShowFriendList()
    {
        UpdateFriends();
        gameObject.SetActive(true);
    }

    public void HideFriendList()
    {
        gameObject.SetActive(false);
    }

}
