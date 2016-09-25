================================================================================================
  MobyShop - What is it?
================================================================================================

MobyShop is an API and template to quickly let you create a shop for in-app purchases in your mobile game.

MobyShop is a solution to get a shop into your game with as little friction as possible. It can be used in an existing project if you already have done some shop UI and want to integrate it with Google Play or the Appstore . Or if you don’t have a shop you can use the included Shop UI to make a fully functioning shop without any programming skills required.

We have provided you with a Shop Configuration Editor which makes it possible for you to manage your products in Unity as well as a “Editor Purchase Simulator” which makes it possible to test the purchases before you launch the game.


================================================================================================
  1. Getting Started
================================================================================================

MobyShop consists of 3 parts: 

MobyShop API, A Shop Configuration Editor & the MobyShop UI prefabs.


Prefabs: 
MobyShop is build in a way that you an use the UI Prefabs to create a shop with little friction and no programming experience. This can be achieved by dragging the a prefab into an existing project and configuring the products in the Shop Configurator (2. Integrating the ShopUI) 

Shop Configurator:
The Shop Configuration Editor can be started in the menu “Window/MobyShop/Shop Configuration”. It’s used to manage your products that you’re selling in the game. (See 4. The Shop Configuration for a detailed walkthrough on this.)

MobyShop API:
MobyShop features a simple API that will allow you to trigger a purchase and test it in the editor as well as integrate it with google play and apple’s iTunes connect etc. To buy a product simply call MobyShop.Store.BuyProduct and the flow envoled in buying a product is started.
(See 6. The MobyShop API Examples for examples on how to use the API )


================================================================================================
  2. Integrating MobyShop and The ShopUI Template.
================================================================================================

In order to get up you and running as quickly as possible we have provided you with a set of prefab of some template UI 
that you can modify to suit your own needs or as a base for some more advanced UI for your project. 

1) Drag and drop the Prefab named “MobyShop” located in Tastybits/MobyShop/Prefabs into the root hierarchy of your main scene. 

2) Drag and drop the prefab named “SimpleF2PShopUI” into a UI Canvas in the scene. This gives you a basic shop with the most essential purchases that you can modify or change in other ways.

3) Run the game and show the Shop UI somewhere by activating the gameobject. Try clicking a purchase.

We have created some basic product configuration for you that you can start out with. See. section 3. Changing a Product: if you with to change the configuration of a product. 



================================================================================================
  3. Changing a existing Product
================================================================================================

Configuration of an existing product for sale is done in the "Shop Configuration Editor". 

The following is an explanation of how to open up the edit a property of a product.

1) Open up the Shop Configuration (Window/MobyShop/Shop Configuration) or click a ShopItem in the shop UI and click “Open in Shop Config” in the Inspector.

2) Various things about a product can be changed like Product Name, Description, Price Tag, Product Type. 

See “4. The Shop Configuration” for more information about the each different type of property that you can change in the Shop Configuration Editor for each product. 

For now we can change the Product Type of the Price Tag of the Product. The “Price tag” is a text string which is used before the price is downloaded from the Appstore or Google Play Store and it allows you to set something to test in the editor.



================================================================================================
  4. The Shop Configuration Editor.
================================================================================================


Things that you want the user to buy is called product aká in-app purchases. 

The Shop Configuration editor is a tool to manage the products you want to sell in the shop. 

Products are the things you will let the user buy in the game -They can be coins, swords, remove ads, powerups like bombs etc. 

You can open the Shop Configuration by going to “Window/MobyShop/Shop Configuration”. Or, If you have the option to look at some scene UI can also select a ShopItem in the UI and click “Open Shop Config” in it’s inspector window.

-

Types of Products

There are two main types of products that most games sell and that is “Unlockable’s” and “Consumables”. 

Unlockable Products are things that the user can only buy once and things that the user usually will get back again if he uninstalls and installs the game again. This can be things like a “remove advertisement” purchase.

Consumable products are things like “coin packs”, powers, in game items like bombs|swords|shields etc. These are things that should be spend in the game while you’re playing a session. 



Product Name & Product Description.
Product Name is the string that the player will see in the game. Sometimes you want to visualise this or sometimes you don’t it’s optional but its a good idea to set it to something meaning full.

The product Description is a small text string that can be used to describe something about a the product in the shop. A couple of nifty keywords can be placed in the text and they will get replaced when running the game. These keywords are %product% the product name and %amount% - the amount of items bought in the purchase. ( See pt. 5 for more info about amount ).


Price : 
The price of a product is downloaded from iTunes Connect or Google Play when the game is launched however when you’re testing it’s a good idea to specify a price tag that you thing might be working for your product thus this is mainly meant for testing in the editor. Also, after releasing your game you might want to change the price, so consider this as a “initial testing price tag”.

For more about price see "Billing Type".


Product Amount & Product Class :
The Product amount is used to describe how many items you get when you buy the product. This can be used when you want to make the user receive 150 coins for buying the product once. 
The Product Class can be used to group the value of more than one consumable into the same type. For instance when the user buys a product called “Coin pack 1” which gives them 150 coins you want to group it into a Class called “coins” in order for the other purchase called “Coin Pack 2” to add to the same value.  


Product Id :
The Product id is the most essential thing in the products. The Product Id is the name specified in iTunes Connect / Appstore and Google Publish / Google Play to identify the product the id is also when when displaying shop items to initiate a purchase or when you use the API.


Billing id :
Under normal circumstances you can ignore specifying a billing id, as the product id will be used when you game is communicating with google and apple. 

However, sometimes you might want to specify a different id only for billing purposes for a specific Product. This can be done by clicking the Store buttons and togglein g “override for storename”. That will enable you to specify a billing id for a specific platform. Like Google play: If you have a product with a billing id called “remove_ads_googleplay” on google play and “remove_ads_ios” for iTunesConnect/Apple Store you’re able to specifically specify it using this features. 


Billing type

The Billing Type can be set to RealLife Currency or Ingame currencts aká virtual currency.

Per default a product will be Reallife Billing type which means that the when the user makes a purchase they're changed real money. 

-

However, Some free-2-play games uses virtual currency as a payment and you will have to buy the virtual currency in order to buy other stuff. This is 
good for games where you want to let the user be able to play and accumulate value as well as skip and pay directly. 

For instance if you're making it possible to buy a sword and you want to let the user to pay with ingame coins then select "Ingame Currency" as billing 
and set a price.

Note that when you select ingame currency you need to specify a currency class id next to the price label. The id needs to be the same as 
an existing Product class id which represents the currency in which you want to pay. 

This allows you to create more advanced setup's of diffirent types of virtual curreny, gold, silver and cobber coins or diamons and gold.

If you're doing a standard setup just input coins next to the price of the item wihch you want to be payed with virtual currency.



================================================================================================
  5. Adding new products
================================================================================================

You can modify the products in the shop by opening the Shop Configuration editor.

1) Click “Window/MobyShop/Shop Configuration” to bring it up. ( Or select a ShopItem in the UI and click “Open in Shop Config” on it’s inspector )

2) In the Shop Editor Click. “New Product”. This will create a new product.

3) Set the name of the product and the product id. If you already setup a product id in iTunes connect and in Google play, specify the same id set.



================================================================================================
  6. The MobyShop API Examples
================================================================================================

In general you need to make very little code in order to get MobyShop up and running if you use the prefabs. You might still want to 
add some code to trigger some stuff in your game logic for instance. If you have a purchase for removing the ads in the game
you might want to check if the "remove_ads" purchase has been bought. Or, You might want to display the total number of coins bought. 
Or, you might want to add a value to the number of coins if the player pick's up a new coin.


6.1 Buying a product

Buying a product usually takes place when clicking the UI and there is Components with functionality available in the ShopUI / ShopItem class that will 
help you to do that. However, sometimes you just want to call BuyProduct some place outside the shop views or call it manually in an extension to the shop
view.

Store.BuyProduct( "coinpack1",  (bool ok, string errmsg )=> {
   if( ok ) Debug.Log("Product bought");
   else Debug.LogError("Product was ignored");
} );


6.2 Checking if a Product has been bought. 

Let's say you want to check if "remove_ads" has been bought where you show the Ads , so that you can ignore showing the Ads.


if( Store.HasProductBeenBought("remove_ads") == false ) { 
   ShowAds();
} else {
   Debug.Log("Ignored showing ads since the user has bought the remove_ads purchase");
}


6.3 Displaying how many coins the user have.

this.text = "coins: " + Store.GetProductClassAmount( "coins" );



6.4 The player is picking up a coin in the game.

When you're selling coins in the game you might want to make it possible for the player to pickup the coins ingame as well
In order to increment the coin in the shop you can call a function to increment the number of "coins" held by the player.

void OnPlayerTouchedCoin(){
	Store.IncrementProductClassAmount( "coins", 1 );
}



6.5 Restore Purchases

On iOS you might want to make a restore purchases function.

Store.RestorePurchases( ( bool ok ) =>  {
	Debug.Log("RestorePurchases");
}, (string productIdRestored)=>{
	Debug.Log("Just restored product: " + productIdRestored );
} );



6.6 Hooking up to the bought products.

If you want some code to react when a product is bought you will be able to do this by adding a delegate listning on the Store.OnProductBought. 
The delegate will get invoken whenever a product is bought by the user or when the user has restored previous purchases.

In order to subscribe to the event :
Store.OnProductBought += this.OnProductBought;

And implement a method like this:
void OnProductBought( MobyShop.BoughtOrRestored state, MobyShop.ProductInfo product, int amount ) {
	Debug.Log("I want to do something specific when a product is bought");
}



6.7 Retriving the ProductInfo of product. 

If you want to retrive and inspect the same inforamtion setup in the Shop Configuration Editor you can retrieve a ProductInfo 
instance. 

This is done by calling "Store.GetProduct" with the Product Id that is specified in the Shop Configuration Editor.



Shoudl you have any other questions regarding the API please dont hesitate to write us at the support. ( tastybits8+support@gmail.com )


================================================================================================
  7. Releasing your App | Setting up Google Play and App store.
================================================================================================

When you’re releasing your app you must setup the purchases in iTunes Connect for iOS apps as well as google play for Android apps released on Google play.

We have created an online tutorial for setting up the stores individually.

App Store
http://bit.ly/MobyShopTutorialOnSettingUpitunesConnect

Google Play
http://bit.ly/MobyShopTutorialOnSettingUpGooglePlay


If you have any issues setting up and finalising the stores please don’t hesitate to write us and we will help you out. ( tastybits8+settingupstores@gmail.com )


================================================================================================
  8. A word on Google Play & Android
================================================================================================

License Key:
There is a couple of things that is important to mind when it comes to getting up and running on Android and Google play.

You need to paste the Google Play License Key into the "Shop Configuration Editor" in the bottom of the view. 

The App License Key is how your game will tell Google know which app purchases and app configuration the game belongs to.

AndroidManifest.xml
When you're publishing the game for Android / Google Play you need to ensure that the billing permissions is flagged in the Android Manifest. We have provided an Android Manifest that you 
can use , but if you are using other Android plugins in the project you may need to merge the AndroidManifest contents or just copy in the permissions for billing.




================================================
  Support
================================================

For support and feature requests please contact us at tastybits8@gmail.com



================================================
  Whats next | Feature requsts
================================================

The upcoming version will feature support for Amazon Store and Windows Store integration. If you want to be a beta tester of these SDK’s please contact support.  tastybits8+beta@gmail.com

We’re also accepting feature requests 
