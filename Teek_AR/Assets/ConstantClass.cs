using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class ConstantClass
    {
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
        public const string PP_UsernameKey = "Username"; //MUST ENCRYPT
        public const string PP_PasswordKey = "Password"; //MUST ENCRYPT
        public const string PP_UserIDKey = "UserID"; //MUST ENCRYPT
        public const string PP_EventIDKey = "EventID";
        public const string PP_UserParticipationID = "UserParticipationID";
        

        //API LINKS
        public const string API_HOST_IP = "172.16.20.32";

        public const string API_Login = "http://" + API_HOST_IP + "/Teek/api/account/login";
        public const string API_Register = "http://" + API_HOST_IP + "/Teek/api/account/register";
        public const string API_RedeemCode = "http://" + API_HOST_IP + "/Teek/api/";
        public const string API_GetAllEvents = "http://" + API_HOST_IP + "/Teek/api/event/GetEvents";

        public const string API_LoadCoins = "http://" + API_HOST_IP + "/Teek/api/";
        public const string API_LoadFireball = "http://" + API_HOST_IP + "/Teek/api/";
        public const string API_BuyItem = "http://" + API_HOST_IP + "/Teek/api/";
        public const string API_UseFireball = "http://" + API_HOST_IP + "/Teek/api/";

        public const string API_UserInfo = "http://" + API_HOST_IP + "/Teek/api/account/getuserinfo?userId="; //Player Information in PlayerInfo Scene


        //TEST LOGIN INFO
        //USername: test123
        //Password: testtest

    }
}
