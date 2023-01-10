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

        // onDeepLinkActivated("glipwalletunitydemo://walletConnected?walletId=0xB4A4d9FeFC5208e616772Ffc821cEF3E8f1d3ff5&userInfo=%7B%22email%22:%22namandwivedi14@gmail.com%22,%22glipAccessToken%22:%22eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ3YWxsZXRJZCI6IjB4QjRBNGQ5RmVGQzUyMDhlNjE2NzcyRmZjODIxY0VGM0U4ZjFkM2ZmNSIsImF1dGgiOnRydWUsImNvbXBhbnlJZCI6IjYyZmQwZTFiNWY2NTM1MzZlOWM2NTdhOCIsImlhdCI6MTY3MzAzNDQ2NiwiZXhwIjoxNzA0NTcwNDY2fQ.tj4rZj5c4kv_nS5qljrM_3iJLfJZsSlSGNfLrlqNXqk%22,%22googleIdToken%22:%22eyJhbGciOiJSUzI1NiIsImtpZCI6IjhlMGFjZjg5MWUwOTAwOTFlZjFhNWU3ZTY0YmFiMjgwZmQxNDQ3ZmEiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIzNzMxOTY0NDY1MDAtb2p0M2tvMWdoaXM5cHJpdGZoaG9nb2hsb3R1dDJodjYuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiIzNzMxOTY0NDY1MDAtb2p0M2tvMWdoaXM5cHJpdGZoaG9nb2hsb3R1dDJodjYuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMTU5MTE2ODI5MzAxMTMwNzgzNTUiLCJlbWFpbCI6Im5hbWFuZHdpdmVkaTE0QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhdF9oYXNoIjoiTzE5X2ZhVGRHejB6ZklHdWc0Z3ZPQSIsIm5hbWUiOiJOYW1hbiBEd2l2ZWRpIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FFZEZUcDVxRk9paWJDaGE4RVBEb2ZKOU1IMEhpV0MxSzNSdGg3SElGaGNZbGc9czk2LWMiLCJnaXZlbl9uYW1lIjoiTmFtYW4iLCJmYW1pbHlfbmFtZSI6IkR3aXZlZGkiLCJsb2NhbGUiOiJlbiIsImlhdCI6MTY3MzAzNDQ2NCwiZXhwIjoxNjczMDM4MDY0fQ.kZ9Hw_EtCJw7C8tnuyzrmUJHFHMDhMC13gCrX9Ep6pH8YCKIUQfUTwoqHUcc4gZMIApphz1KpVELS3sUokMw-GR01P4pangoZ1pp4IUE9W6KqKBrCj-XsVlETOW-jYMsjeHWOjhfSIzAfd66i0NwI459iV5KkuRfGmiZ4w9AROwz9QhvObhyFUYYDLUKHZpFhP1Eg-VTHrAKb2py_X_QtkGXOJ-weXv2kQkrP3f2e27sjXmqbRJJQuX_02ZCMUrDh3oV5iXnHdR4cSXtvHT8JdsG8qNYHcw7qHODnxtBU8WKQvOq4BOsTwmya33VAv3lj7NXT3R_8LDpZTZ7TwCjxA%22,%22publicAddress%22:%220xB4A4d9FeFC5208e616772Ffc821cEF3E8f1d3ff5%22%7D");
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

        onDeepLinkActivated("glipwalletunitydemo://loggedOut");
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

        onDeepLinkActivated("glipwalletunitydemo://signMessage?data=0x0");
    }

    public static void SignPersonalMessage(string message, WalletSignMessageListener listener) {
        walletSignMessageListener = listener;
        string encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        string url =
            BASE_URL + "?action=signPersonalMessage&message="+encodedMessage+"&chain="+chainId+"&redirect_scheme="+redirectScheme;
        Debug.Log("glipwallet: Signing personal message url: " + url);
        Application.OpenURL(url);

        onDeepLinkActivated("glipwalletunitydemo://signPersonalMessage?data=0x0");
    }

     public static void SignTransaction(string tx, WalletSignTransactionListener listener) {
        walletSignTransactionListener = listener;
        string encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(tx));
        string url =
            BASE_URL + "?action=signTx&txData="+encodedMessage+"&chain="+chainId+"&redirect_scheme="+redirectScheme;
        Debug.Log("glipwallet: Signing tx url: " + url);
        Application.OpenURL(url);

        onDeepLinkActivated("glipwalletunitydemo://signTx?data=0x0");
    }

     public static void SendTransaction(string tx, WalletSendTransactionListener listener) {
        walletSendTransactionListener = listener;
        string encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(tx));
        string url =
            BASE_URL + "?action=sendTx&txData="+encodedMessage+"&chain="+chainId+"&redirect_scheme="+redirectScheme;
        Debug.Log("glipwallet: Send tx url: " + url);
        Application.OpenURL(url);

        onDeepLinkActivated("glipwalletunitydemo://sendTx?data=0x0");
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
