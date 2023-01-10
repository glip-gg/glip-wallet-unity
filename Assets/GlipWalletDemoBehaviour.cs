using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlipWallet;

public class GlipWalletDemoBehaviour : MonoBehaviour, 
    WalletConnectedListener, WalletLogoutListener,
    WalletSignMessageListener, WalletSendTransactionListener,
    WalletSignTransactionListener
{

    [SerializeField] 
    private Text _message;

    [SerializeField] 
    private Text _connected;

    private string dummyTx = "{\"from\": \"0xB4A4d9FeFC5208e616772Ffc821cEF3E8f1d3ff5\", \"to\": \"0xB4A4d9FeFC5208e616772Ffc821cEF3E8f1d3ff5\"}";

    void Start() {
        Debug.Log("glipwallet: Initialising Glip wallet demo");
        GlipWallet.Init("63020e1ef81e3742a278846a", 137, "glipwalletunitydemo");
        setConnectedInfo();
    }

    void Update(){}

    public void OnLoginClicked() {
        Debug.Log("glipwallet: Login clicked");
        _message.text = "Logging in...";
        GlipWallet.Login(this);
    }

    public void OnLogoutClicked() {
        Debug.Log("glipwallet: Logout clicked");
        _message.text = "Logging out...";
        GlipWallet.Logout(this);
    }

     public void OnShowWalletClicked() {
        Debug.Log("glipwallet: Show wallet clicked");
        _message.text = "Showing wallet...";
        GlipWallet.ShowWalletUI();
    }

    public void OnSignMessageClicked() {
        Debug.Log("glipwallet: Signing dummy message");
        _message.text = "Signing dummy message...";
        GlipWallet.SignPersonalMessage("This is a test message from Glip Wallet Unity demo. Please sign this message", this);
    }

    public void OnSignTransactionClicked() {
        Debug.Log("glipwallet: Signing dummy tx");
        _message.text = "Signing dummy tx...";
        GlipWallet.SignTransaction(dummyTx, this);
    }

    public void OnSendTransactionClicked() {
        Debug.Log("glipwallet: Sending dummy message");
        _message.text = "Sending dummy tx...";
        GlipWallet.SendTransaction(dummyTx, this);
    }


    public void OnWalletConnected(string walletId, string userInfo) {
        Debug.Log("glipwallet: Wallet connected");
        _message.text = "Wallet connected";
        setConnectedInfo();
    }

    public void OnWalletConnectCancelled() {
        _message.text = "Error connecting wallet";
    }

    public void OnWalletLogout() {
        _message.text = "Logged out";
        setConnectedInfo();
    }

    public void OnMessageSigned(string signedMessage) {
        _message.text = "Signed message: " + signedMessage;
    }

    public void OnSignMessageCancelled() {
        _message.text = "Error signing message";
    }

    public void OnTransactionSigned(string signedTransaction) {
        _message.text = "Signed tx: " + signedTransaction;

    }
    public void OnSignTransactionCancelled() {
        _message.text = "Error signing tx";
    }

    public void OnTransactionSent(string tx) {
        _message.text = "Sent tx: " + tx;
    }

    public void OnSendTransactionCancelled() {
        _message.text = "Error sending tx";

    }

    private void setConnectedInfo() {
        _connected.text = "is connected: " + GlipWallet.IsConnected() + "\naddress: " + GlipWallet.GetWalletAddress();
    }

}
