# Glip Wallet Unity SDK

Unity SDK and demo for using Glip Wallet in Android/iOS/Desktop apps

## Setup

Import `glipwallet.unitypackage` into your project. Download latest version from releases section.

v1 - https://github.com/glip-gg/glip-wallet-unity/releases/download/v1/glipwallet.unitypackage

Alternatively, You can also just add [GlipWallet.cs]('https://github.com/glip-gg/glip-wallet-unity/blob/main/Assets/GlipWallet/GlipWallet.cs') in your project


 ### Initialization
First make sure that you have created a clientId already.

```cs
GlipWallet.init("client_id", chainId, "redirect_scheme")
```

Deeplink handling will also need to be setup for the redirect scheme that is passed in init.

### Enable deeplinks for different platforms

Follow the guide [here]('https://docs.unity3d.com/Manual/deep-linking.html') to setup deeplink handling for different platforms.

Choose deeplink scheme according to your app's naming convention. An example for Android is provided in this demo project in `Assets/Plugins/Android/AndroidManifest.xml`

 This unique scheme allows your app to handle wallet interactions uniquely if multiple apps have Glip Wallet SDK integrated.
 
### Methods
`GlipWallet.Login(this);` 

`GlipWallet.Logout(this);`

`GlipWallet.ShowWalletUI();`

`GlipWallet.SignPersonalMessage("Test message", this);`

`GlipWallet.SignTransaction(dummyTx, this);`

`GlipWallet.SendTransaction(dummyTx, this);`

### Demo
This project contains a working demo for all wallet interactions. A prebuilt Android apk is also available to test [here](https://github.com/glip-gg/glip-wallet-unity/releases/download/v1/glipwallet-unity-android-demo.apk)


