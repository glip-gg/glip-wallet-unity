using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Web;

public class GlipWallet : MonoBehaviour {

    private static string redirectScheme;

    private static string clientId;
    private static int chainId;

    private static string PREF_WALLET_CONNECTED = "glip_wallet_connected";
    private static string PREF_USER_INFO = "glip_wallet_user_info";
    private static string PREF_WALLET_ADDRESS = "glip_wallet_user_address";

    private static string BASE_URL = "https://glip-gg.github.io/Glip-wallet-android/";
    private static string WALLET_HOST_URL = "https://glip.gg/wallet-host/";

    private static WalletConnectedListener walletConnectedListener;
    private static WalletLogoutListener walletLogoutListener;
    private static WalletSignTransactionListener walletSignTransactionListener;
    private static WalletSignMessageListener walletSignMessageListener;
    private static WalletSendTransactionListener walletSendTransactionListener;

   public interface WalletConnectedListener {
        void OnWalletConnected(string walletId, string userInfo);
        void OnWalletConnectCancelled();
    }

    public interface WalletLogoutListener {
        void OnWalletLogout();
    }

    public interface WalletSignTransactionListener {
        void OnTransactionSigned(string signedTransaction);
        void OnSignTransactionCancelled();
    }

    public interface WalletSendTransactionListener {
        void OnTransactionSent(string tx);
        void OnSendTransactionCancelled();
    }

    public interface WalletSignMessageListener {
        void OnMessageSigned(string signedMessage);
        void OnSignMessageCancelled();
    }


    public static void Init(string clientId, int chainId, string redirectScheme) {
        GlipWallet.clientId = clientId;
        GlipWallet.chainId = chainId;
        GlipWallet.redirectScheme = redirectScheme;
        Application.deepLinkActivated += onDeepLinkActivated;
    }

    public static void Login(WalletConnectedListener listener) {
        walletConnectedListener = listener;
        string url =
            BASE_URL + "?action=login&chain="+chainId+"&network=cyan&clientId="+clientId+"&provider=google&redirect_scheme="+redirectScheme;
        Debug.Log("glipwallet: Login url: " + url);
        Application.OpenURL(url);
    }

    public static bool IsConnected() {
        return PlayerPrefs.HasKey(PREF_WALLET_CONNECTED) && PlayerPrefs.GetInt(PREF_WALLET_CONNECTED) == 1;
    }

    public static string GetWalletAddress() {
        return PlayerPrefs.GetString(PREF_WALLET_ADDRESS);
    }

    public static string GetUserInfo() {
        return PlayerPrefs.GetString(PREF_USER_INFO);
    }

    public static void Logout(WalletLogoutListener listener) {
        walletLogoutListener = listener;
        string url =
            BASE_URL + "?action=logout&provider=google&redirect_scheme="+redirectScheme;
        Debug.Log("glipwallet: Login url: " + url);
        Application.OpenURL(url);
    }

    public static void ShowWalletUI() {
        Debug.Log("glipwallet: Showing wallet UI");
        Application.OpenURL(WALLET_HOST_URL);
    }

    public static void SignMessage(string message, WalletSignMessageListener listener) {
        walletSignMessageListener = listener;
        string encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        string url =
            BASE_URL + "?action=signMessage&message="+encodedMessage+"&chain="+chainId+"&redirect_scheme="+redirectScheme;
        Debug.Log("glipwallet: Signing message url: " + url);
        Application.OpenURL(url);
    }

    public static void SignPersonalMessage(string message, WalletSignMessageListener listener) {
        walletSignMessageListener = listener;
        string encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        string url =
            BASE_URL + "?action=signPersonalMessage&message="+encodedMessage+"&chain="+chainId+"&redirect_scheme="+redirectScheme;
        Debug.Log("glipwallet: Signing personal message url: " + url);
        Application.OpenURL(url);
    }

     public static void SignTransaction(string tx, WalletSignTransactionListener listener) {
        walletSignTransactionListener = listener;
        string encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(tx));
        string url =
            BASE_URL + "?action=signTx&txData="+encodedMessage+"&chain="+chainId+"&redirect_scheme="+redirectScheme;
        Debug.Log("glipwallet: Signing tx url: " + url);
        Application.OpenURL(url);
    }

     public static void SendTransaction(string tx, WalletSendTransactionListener listener) {
        walletSendTransactionListener = listener;
        string encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(tx));
        string url =
            BASE_URL + "?action=sendTx&txData="+encodedMessage+"&chain="+chainId+"&redirect_scheme="+redirectScheme;
        Debug.Log("glipwallet: Send tx url: " + url);
        Application.OpenURL(url);
    }

    private static void InternalLogout() {
        Debug.Log("glipwallet: Logging out");
        PlayerPrefs.DeleteKey(PREF_WALLET_ADDRESS);
        PlayerPrefs.DeleteKey(PREF_WALLET_CONNECTED);
        PlayerPrefs.DeleteKey(PREF_USER_INFO);
        PlayerPrefs.Save();
    }

private static void onDeepLinkActivated(string url)
    {
        Debug.Log("glipwallet: Deeplink url: " + url);

        Uri uri = new Uri(url);

        string host = uri.Host;
        string scheme = uri.Scheme;

        Debug.Log("glipwallet: Host: " + host);

        if (scheme != redirectScheme) {
            Debug.Log("glipwallet: Scheme is not redirect scheme: " + scheme);
            return;
        }

        if (host == "loggedout") {
            InternalLogout();
            walletLogoutListener.OnWalletLogout();
            return;
        }

         if (host == "walletconnected") {
                var query = HttpUtility.ParseQueryString(uri.Query);
                string walletId = query.Get("walletId");
                string userInfo = query.Get("userInfo");

                Debug.Log("glipwallet: walletId: " + walletId);
                Debug.Log("glipwallet: userInfo: " + userInfo);

                if (walletId != null && userInfo != null) {
                    PlayerPrefs.SetString(PREF_WALLET_ADDRESS, walletId);
                    PlayerPrefs.SetInt(PREF_WALLET_CONNECTED, 1);
                    PlayerPrefs.SetString(PREF_USER_INFO, userInfo);
                    PlayerPrefs.Save();

                    walletConnectedListener.OnWalletConnected(walletId, userInfo);
                }
         }

        if (host == "signpersonalmessage") {
                var query = HttpUtility.ParseQueryString(uri.Query);
                string data = query.Get("data");

                Debug.Log("glipwallet: data: " + data);

                if (data != null) {
                    walletSignMessageListener.OnMessageSigned(data);
                }
         }

         if (host == "signmessage") {
                var query = HttpUtility.ParseQueryString(uri.Query);
                string data = query.Get("data");

                Debug.Log("glipwallet: data: " + data);

                if (data != null) {
                    walletSignMessageListener.OnMessageSigned(data);
                }
         }

         if (host == "signtx") {
                var query = HttpUtility.ParseQueryString(uri.Query);
                string data = query.Get("data");

                Debug.Log("glipwallet: data: " + data);

                if (data != null) {
                    walletSignTransactionListener.OnTransactionSigned(data);
                }
         }

         if (host == "sendtx") {
                var query = HttpUtility.ParseQueryString(uri.Query);
                string data = query.Get("data");

                Debug.Log("glipwallet: data: " + data);

                if (data != null) {
                    walletSendTransactionListener.OnTransactionSent(data);
                }
         }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    
}
