using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class ConstantClass
    {
        public string a = "";
        public const string HomeSceneName = "HomeScene";
        public const string PlayerInfoSceneName = "PlayerInfoScene";
        public const string LoginSceneName = "LoginScene";
        public const string GameSceneName = "GameScene";
        public const string RedeemCodeSceneName = "RedeemCodeScene";
        public const string EventDetailSceneName = "EventDetailScene";
        public const string InventorySceneName = "InventoryScene";
        public const string TutorialSceneName = "TutorialScene";

        //THIS CLASS NAME IS CLASS NAME IN MOBY SHOP
        public const string FireBallItemClassName = "Fireball";
        public const string CoinItemClassName = "coins";

        //USED THESE ID TO LOAD PLAYER INTEMS FROM SERVER
        public const string FireBallItemID = "fireball";
        public const string CoinItemID = "coinpack1";

        //USED THESE TO STORE DATA IN PLAYERSPREFS
        public const string PP_UsernameKey = "Username";
        public const string PP_PasswordKey = "Password";
        public const string PP_UserIDKey = "PlayerID";

        //API LINKS
        public const string API_HOST_IP = "192.168.2.123";

        public const string API_Login = "http://" + API_HOST_IP + "/Teek/api/account/login";
        public const string API_Register = "http://" + API_HOST_IP + "/Teek/api/account/register";
        public static string API_RedeemCode = "http://" + API_HOST_IP + "/Teek/api/";

        public static string API_UserInfo = "http://" + API_HOST_IP + "/Teek/api/account/getuserinfo?userId="; //Player Information in PlayerInfo Scene


        //TEST LOGIN INFO
        //USername: test123
        //Password: testtest

    }
}
