using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class ConstantClass 
    {
        #region SCENE NAME
        public const string HomeSceneName = "HomeScene";
        public const string ProfileSceneName = "ProfileScene";
        public const string CouponSceneName = "CouponScene";
        public const string LoginSceneName = "LoginScene";
        public const string GameSceneName = "GameScene";
        public const string RedeemCodeSceneName = "RedeemCodeScene";
        public const string EventDetailSceneName = "EventDetailScene";
        public const string InventorySceneName = "InventoryScene";
        public const string TutorialSceneName = "TutorialScene";
        public const string StoreEventSceneName = "StoreEventScene";
        public const string BrandDetailSceneName = "BrandDetailScene";
        public const string MyEventScene = "MyEventScene";
        #endregion

        #region THIS CLASS NAME IS CLASS NAME IN MOBY SHOP
        public const string FireBallItemClassName = "Fireball";
        public const string IceBallItemClassName = "Iceball";
        public const string CoinItemClassName = "coins";
        #endregion

        #region USED THESE ID TO LOAD PLAYER INTEMS FROM SERVER
        public const string FireBallItemID = "fireball";
        public const string IceBallItemID = "iceball";
        public const string CoinItemID = "coinpack1";
        #endregion

        #region USED THESE TO STORE DATA IN PLAYERSPREFS
        public const string PP_UsernameKey = "Username"; //MUST ENCRYPT
        public const string PP_PasswordKey = "Password"; //MUST ENCRYPT
        public const string PP_UserIDKey = "UserID"; //MUST ENCRYPT
        public const string PP_EventIDKey = "EventID";
        public const string PP_OrganizerId = "OrganizerID";
        public const string PP_GameId = "GameID";
        public const string PP_OrganizerName = "OrganizerName";
        public const string PP_UserParticipationID = "UserParticipationID";
        public const string PP_CustomerId = "CustomerID";
        #endregion

        #region API LINKS
        //API LINKS
        public const string API_HOST_IP = "localhost";
        public const string API_Login = "http://" + API_HOST_IP + "/Teek/api/account/login";
        public const string API_Register = "http://" + API_HOST_IP + "/Teek/api/account/register";
        public const string API_RedeemPrizeCode = "http://" + API_HOST_IP + "/Teek/api/Prize/ReedemPrize";
        public const string API_GetAllEvents = "http://" + API_HOST_IP + "/Teek/api/event/GetEvents";
        public const string API_GetParticipateEvents = "http://" + API_HOST_IP + "/Teek/api/event/ParticipateEvents";
        public const string API_LoadStoreList = "http://" + API_HOST_IP + "/Teek/api/store/GetStoresByOrganizerId";
        public const string API_LoadEventListByOrganizer = "http://" + API_HOST_IP + "/Teek/api/event/GetEventByOrganizer";
        public const string API_LoadEventListByStore = "http://" + API_HOST_IP + "/Teek/api/event/GetEventByStore";
        public const string API_LoadOrganizer = "http://" + API_HOST_IP + "/Teek/api/organizer/getorganizers";
        public const string API_LoadMyBrand = "http://" + API_HOST_IP + "/Teek/api/organizer/GetOrganizersByUserId";
        public const string API_LoadPrizeList= "http://" + API_HOST_IP + "/Teek/api/prize/GetPrizes";
        public const string API_LoadCustomerInformation = "http://" + API_HOST_IP + "/Teek/api/Customer/GetCustomerInfo";
        public const string API_LoadPrizeCode = "http://" + API_HOST_IP + "/Teek/api/Prize/GetPrizeCode";
        public const string API_RedeemPrizeByTeek = "http://" + API_HOST_IP + "/Teek/api/Prize/RedeemPrizeByTeek";
        public const string API_RedeemPrizeByGem = "http://" + API_HOST_IP + "/Teek/api/Prize/RedeemPrizeByGem";
        public const string API_LoadUserProfile = "http://" + API_HOST_IP + "/Teek/api/PlayerInfo/GetPlayerInfoByUserId";
        public const string API_RegisterEvent = "http://" + API_HOST_IP + "/Teek/api/Event/RegisterEvent";
        public const string API_GetPublishedActivityByEventId = "http://" + API_HOST_IP + "/Teek/api/Activity/GetPublished";
        public const string API_UpdateBallItem = "http://" + API_HOST_IP + "/Teek/api/Customer/UpdateBallItem";
        public const string API_UpdateCoinItem = "http://" + API_HOST_IP + "/Teek/api/Customer/UpdateCoinAmount";
        public const string API_LoadCouponList = "http://" + API_HOST_IP + "/Teek/api/RedeemCoupon/GetCouponList";
        public const string API_RedeemCoupon = "http://" + API_HOST_IP + "/Teek/api/RedeemCoupon/RedeemCoupon";
        public const string API_LoadMyEvent = "http://" + API_HOST_IP + "/Teek/api/Event/GetEventByUserId";
        public const string API_LoadDropRate = "http://" + API_HOST_IP + "/Teek/api/Game/GetDropRate";

        public const string API_UserInfo = "http://" + API_HOST_IP + "/Teek/api/account/getuserinfo?userId="; //Player Information in PlayerInfo Scene

        public const string API_UpdateGemAmount = "http://" + API_HOST_IP + "/Teek/Api/Customer/UpdateGemAmount";
        #endregion

        #region OTHERS
        public const string Msg_TimeOut = "Connection Timeout!!!";
        public const string Msg_MessageTitle = "Message";
        public const string Msg_ErrorTitle = "Connection Error!";
        public const string Msg_ConfirmTitle = "Confirm";

        //TEST LOGIN INFO
        //USername: test123
        //Password: testtest
        //zaQ@123456
        #endregion
    }
}
