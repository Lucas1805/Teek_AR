﻿using System;
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
        public const string StoreEventSceneName = "StoreEventScene";
        public const string BrandDetailSceneName = "BrandDetailScene";

        //THIS CLASS NAME IS CLASS NAME IN MOBY SHOP
        public const string FireBallItemClassName = "Fireball";
        public const string IceBallItemClassName = "Iceball";
        public const string CoinItemClassName = "coins";

        //USED THESE ID TO LOAD PLAYER INTEMS FROM SERVER
        public const string FireBallItemID = "fireball";
        public const string IceBallItemID = "iceball";
        public const string CoinItemID = "coinpack1";

        //USED THESE TO STORE DATA IN PLAYERSPREFS
        public const string PP_UsernameKey = "Username"; //MUST ENCRYPT
        public const string PP_PasswordKey = "Password"; //MUST ENCRYPT
        public const string PP_UserIDKey = "UserID"; //MUST ENCRYPT
        public const string PP_EventIDKey = "EventID";
        public const string PP_OrganizerId = "OrganizerID";
        public const string PP_UserParticipationID = "UserParticipationID";
        

        //API LINKS
        public const string API_HOST_IP = "192.168.1.66";

        public const string API_Login = "http://" + API_HOST_IP + "/Teek/api/account/login";
        public const string API_Register = "http://" + API_HOST_IP + "/Teek/api/account/register";
        public const string API_RedeemCode = "http://" + API_HOST_IP + "/Teek/api/";
        public const string API_GetAllEvents = "http://" + API_HOST_IP + "/Teek/api/event/GetEvents";
        public const string API_GetParticipateEvents = "http://" + API_HOST_IP + "/Teek/api/event/ParticipateEvents";
        public const string API_LoadStoreList = "http://" + API_HOST_IP + "/Teek/api/store/GetStoresByOrganizerId";
        public const string API_LoadEventListByOrganizer = "http://" + API_HOST_IP + "/Teek/api/event/GetEventByOrganizer";
        public const string API_LoadEventListByStore = "http://" + API_HOST_IP + "/Teek/api/event/GetEventByStore";

        public const string API_LoadCoins = "http://" + API_HOST_IP + "/Teek/api/";
        public const string API_LoadTeek = "http://" + API_HOST_IP + "/Teek/api/";
        public const string API_LoadFireball = "http://" + API_HOST_IP + "/Teek/api/";
        public const string API_BuyItem = "http://" + API_HOST_IP + "/Teek/api/";
        public const string API_UseFireball = "http://" + API_HOST_IP + "/Teek/api/";

        public const string API_UserInfo = "http://" + API_HOST_IP + "/Teek/api/account/getuserinfo?userId="; //Player Information in PlayerInfo Scene

        public const string Msg_TimeOut = "Connection Timeout!!!";
        //TEST LOGIN INFO
        //USername: test123
        //Password: testtest

    }
}
