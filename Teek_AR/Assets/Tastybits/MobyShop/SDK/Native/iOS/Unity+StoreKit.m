#import <StoreKit/StoreKit.h>

@interface KSiOSInAppPurchase : NSObject
void sendMessageToUnity(const char *methodName, const char *msg);
- (void)buyProduct:(SKProduct *)product;
@end


void UnitySendMessage(const char* obj, const char* method, const char* msg);

// Extensions to NSDicitionality
@interface NSDictionary (BVJSONString2)
-(NSString*) UnityStore_dictToJsonStr:(BOOL) prettyPrint;
@end
@implementation NSDictionary (BVJSONString2)
-(NSString*) UnityStore_dictToJsonStr:(BOOL) prettyPrint {
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self
                                                       options:(NSJSONWritingOptions) (prettyPrint ? NSJSONWritingPrettyPrinted : 0)
                                                         error:&error];
    
    if( !jsonData ) {
        NSLog(@"UnityStore_dictToJsonStr: error: %@", error.localizedDescription);
        return @"{}";
    } else {
        return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }
}
@end



void UnityStoreKit_Callback( NSString* nscbid, NSDictionary* dict ) {
    NSString* strJson = [dict UnityStore_dictToJsonStr:TRUE];
    UnitySendMessage( [nscbid UTF8String], "CallDelegateFromNative", [strJson UTF8String] );
}


void UnityStoreKit_Callback2( NSString* nscbid, bool _ok, NSString* str_msg ) {
    NSDictionary* dict = [[NSDictionary alloc] initWithObjectsAndKeys:
                          ( _ok?@"true":@"false"),  @"ok",
                          str_msg,                  @"msg",
                          nil];
    NSString* strJson = [dict UnityStore_dictToJsonStr:TRUE];
    UnitySendMessage( [nscbid UTF8String], "CallDelegateFromNative", [strJson UTF8String] );
}



@interface UnityStoreKit : NSObject
void SendToStoreKitObject(const char *methodName, const char *msg);
- (void)buyProductImpl:(SKProduct*)product cb:(NSString*)cbid;
+ (UnityStoreKit*)sharedInstance;
@end


@interface UnityStoreKit () <SKProductsRequestDelegate, SKPaymentTransactionObserver>
    @property (nonatomic, retain ) NSString* cbInit;
    @property (nonatomic, retain ) NSString* cbBuyProduct;
    @property (nonatomic, retain ) SKProductsRequest* productsRequest;
    @property (nonatomic, assign ) BOOL hasReceivedProductCatalogue;
    @property (nonatomic, assign ) BOOL failedGettingProducts;
    @property (nonatomic, retain ) NSMutableDictionary* products;
    @property (nonatomic, retain ) NSMutableArray* invalidProducts;
    @property (nonatomic, assign ) BOOL failedToGetProductCatalogue;
    @property (nonatomic, retain ) NSString* goid;
    @property (nonatomic, retain ) NSString* cbRestoreProducts;
@end



void SendToStoreKitObject( const char* methodName, const char* msg ) {
    const char *gameObj = "MobiShop";
    if( !([UnityStoreKit sharedInstance].goid == nil || [[UnityStoreKit sharedInstance].goid isEqualToString:@""] ) ){
        gameObj = [[UnityStoreKit sharedInstance].goid UTF8String];
    }
    UnitySendMessage( gameObj, methodName, msg );
}


void SendToStoreKitObject2( const char* methodName, NSDictionary* dict  ){
    NSString* strJson = [dict UnityStore_dictToJsonStr:TRUE];
    SendToStoreKitObject( methodName, [strJson UTF8String] );
}


@implementation UnityStoreKit {
    SKProductsRequest *     _productsRequest;
    NSSet*                  _productIds;
    NSMutableDictionary*    _products;
    NSMutableArray*         _invalidProducts;
    NSString*               _cbInit;
    NSString*               _cbBuyProduct;
    NSString*               _goid;
    NSString*               _cbRestoreProducts;
}
@synthesize cbInit = _cbInit;
@synthesize cbBuyProduct = _cbBuyProduct;
@synthesize productsRequest = _productsRequest;
@synthesize products = _products;
@synthesize invalidProducts = _invalidProducts;
@synthesize goid = _goid;
@synthesize cbRestoreProducts = _cbRestoreProducts;
//@synthesize hasReceivedProductCatalogue = _hasReceivedProductCatalogue;

- (id)init {
    if (self = [super init]) {
        self.failedToGetProductCatalogue = NO;
        self.hasReceivedProductCatalogue = NO;
        self.goid = @"";
        self.cbRestoreProducts = nil;
    }
    return self;
}


- (void)dealloc {
    // Should never be called, but just here for clarity really.
}


- (void) initProductCatalogue:(NSSet*)productIds {
    self->_productIds = productIds;
    self->_products = [[NSMutableDictionary alloc] init];
    self->_invalidProducts = [[NSMutableArray alloc] init];
    self.hasReceivedProductCatalogue = false;
    [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
    
    SKProductsRequest* request = [[SKProductsRequest alloc] initWithProductIdentifiers:productIds];
    self.productsRequest = request;
    request.delegate = [UnityStoreKit sharedInstance];
    [[UnityStoreKit sharedInstance].productsRequest start];

}


- (SKProduct*) getProduct:(NSString*) productId{
    return [_products objectForKey:productId];
}


- (bool)hasProduct:(NSString*) productId{
    return [self getProduct :productId] != nil;
}


- (NSString*) priceAsStringFromProduct:(SKProduct*) product{
    NSNumberFormatter *formatter = [[NSNumberFormatter alloc] init];
    [formatter setFormatterBehavior:NSNumberFormatterBehavior10_4];
    [formatter setNumberStyle:NSNumberFormatterCurrencyStyle];
    [formatter setLocale:product.priceLocale];
    
    NSString* currencyString = [formatter internationalCurrencySymbol];
    
    NSString* posFormat = [formatter positiveFormat];
    posFormat = [posFormat stringByReplacingOccurrencesOfString:@"¤" withString:currencyString];
    [formatter setPositiveFormat:posFormat];
    
    NSString* negFormat = [formatter negativeFormat];
    negFormat = [negFormat stringByReplacingOccurrencesOfString:@"¤" withString:currencyString];
    [formatter setNegativeFormat:negFormat];
    
    //NSString* formattedString = [formatter stringFromNumber:self.price];
    
    NSString* str = [formatter stringFromNumber:product.price];
    //[formatter release];
    return str;
}

- (NSString*) priceAsStringFromProductIdentifier:(NSString*) productId{
    SKProduct * product = [self getProduct: productId];
    if( product == nil ) {
        return @"?";
    }
    return [self priceAsStringFromProduct :product];
    //return [_productPriceStrings objectForKey :productId];
}



#pragma mark - SKProductsRequestDelegate
/**/
- (void)productsRequest:(SKProductsRequest*)request didReceiveResponse:(SKProductsResponse*)response {
    
    NSLog( @"Loaded list of products..." );
    self.productsRequest = nil;
    
    for( SKProduct * p in response.products ) {
        NSLog(@"MobyShop:Found product: %@ %@ %0.2f",
              p.productIdentifier,
              p.localizedTitle,
              p.price.floatValue);
        [_products setObject:p forKey:p.productIdentifier];
    }
    
    // if you have given some product identifiers that are not valid it will be in this list.
    for( NSString* invalidProductId in response.invalidProductIdentifiers ) {
        NSLog(@"Error: Invalid product id: %@" , invalidProductId);
        [self.invalidProducts addObject:invalidProductId];
    }
    
    self.hasReceivedProductCatalogue = true;
    self.failedToGetProductCatalogue = false;
}


- (void)request:(SKRequest*)request didFailWithError:(NSError*)error {
    NSLog(@"Failed to load list of products - Error - %@", error.description);
    self.productsRequest = nil;
    self.failedToGetProductCatalogue = true;
    self.hasReceivedProductCatalogue = false;
    self.failedGettingProducts = true;
}


- (void)buyProductImpl:(SKProduct*)product cb:(NSString*)cbid {
    if( product == nil ) {
        NSLog(@"MobyShop: Error, Product is nil!");
        UnityStoreKit_Callback2( cbid, false, @"Product is null" );
        
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error"
                                                        message:[NSString stringWithFormat:@"Product is null"]
                                                       delegate:nil
                                              cancelButtonTitle:nil
                                              otherButtonTitles:@"Okay",nil];
        [alert show];
        
        return;
    }
    NSLog(@"MobyShop: buyProductImpl: %@ ; CB=%@", product.productIdentifier, cbid );
    SKPayment * sk_payment = [SKPayment paymentWithProduct:product];
    [[SKPaymentQueue defaultQueue] addPayment:sk_payment];
}


- (void)paymentQueueRestoreCompletedTransactionsFinished:(SKPaymentQueue *)queue {
    NSLog(@"MobyShop: paymentQueueRestoreCompletedTransactionsFinished");
    if( self.cbRestoreProducts!=nil ) {
        NSString* cb_tmp = [NSString stringWithFormat:@"%@", self.cbRestoreProducts ];
        self.cbRestoreProducts = nil;
        bool _ok = true;
        NSString* msg = @"Restore purchases done";
        UnityStoreKit_Callback( cb_tmp,
                              [[NSDictionary alloc] initWithObjectsAndKeys:
                               ( _ok?@"true":@"false"),  @"ok",
                               msg, @"msg",
                               nil] );
    }
}


- (void)paymentQueue:(SKPaymentQueue *)queue restoreCompletedTransactionsFailedWithError:(NSError *)error {
    if( self.cbRestoreProducts!=nil ) {
        NSString* cb_tmp = [NSString stringWithFormat:@"%@", self.cbRestoreProducts ];
        self.cbRestoreProducts = nil;
        bool _ok = false;
        NSString* msg = [NSString stringWithFormat:@"Error: %@",error];
        UnityStoreKit_Callback( cb_tmp,
                               [[NSDictionary alloc] initWithObjectsAndKeys:
                                ( _ok?@"true":@"false"),  @"ok",
                                msg, @"msg",
                                nil] );
    }
}


- (void)paymentQueue:(SKPaymentQueue*)queue updatedTransactions:(NSArray*)transactions
{
    for( SKPaymentTransaction * transaction in transactions ) {
        NSString* nsTransactionId = transaction.transactionIdentifier;
        NSString* msg = @"";
        NSString* szerr = nil;
        bool _ok = true;
        
        switch( transaction.transactionState ) {
            /*
             SKPaymentTransactionStatePurchasing
            */
            case SKPaymentTransactionStatePurchasing:
                NSLog(@"MobyShop: Starting Purchasing | SKPaymentTransactionStatePurchasing.");
                
                msg = @"Started Purchasing";
                SendToStoreKitObject2( "iOSNativeCallback_TransactionPurchasing",
                                      [[NSDictionary alloc] initWithObjectsAndKeys:
                                       ( _ok?@"true":@"false"),  @"ok",
                                       msg, @"msg",
                                       [NSString stringWithFormat:@"%@",transaction.payment.productIdentifier], @"productId",
                                       [NSString stringWithFormat:@"%@",transaction.transactionIdentifier], @"transactionId",
                                       nil] );


                break;
                
            /*
             SKPaymentTransactionStatePurchased
            */
            case SKPaymentTransactionStatePurchased:
                NSLog(@"MobyShop: Complete Transaction | SKPaymentTransactionStatePurchased.");
                
                msg = @"Purchase Done";
                SendToStoreKitObject2( "iOSNativeCallback_TransactionCompleted",
                                      [[NSDictionary alloc] initWithObjectsAndKeys:
                                       ( _ok?@"true":@"false"),  @"ok",
                                       msg, @"msg",
                                       [NSString stringWithFormat:@"%@",transaction.payment.productIdentifier], @"productId",
                                       [NSString stringWithFormat:@"%@",transaction.transactionIdentifier], @"transactionId",
                                       nil] );

                [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
                break;
            
            /*
              The transaction failed. Check the error property to determine what happened
             */
            case SKPaymentTransactionStateFailed:
                NSLog(@"MobyShop: Transaction Failed | SKPaymentTransactionStateFailed.");
                
                szerr = @"Unknown error";
                if( transaction.error.code == SKErrorUnknown ) {
                    szerr = @"Unknown error.";
                } else if( transaction.error.code == SKErrorClientInvalid ) {
                    szerr = @"Invalid client.";
                } else if( transaction.error.code == SKErrorPaymentCancelled ) {
                    szerr = @"Payment cancelled.";
                } else if( transaction.error.code == SKErrorPaymentInvalid ) {
                    szerr = @"Payment is invalid.";
                } else if( transaction.error.code == SKErrorPaymentNotAllowed ) {
                    szerr = @"Payment not allowed.";
                }
                
                // Payment is not allowed.
                SendToStoreKitObject2( "iOSNativeCallback_TransactionFailed",
                                      [[NSDictionary alloc] initWithObjectsAndKeys:
                                       ( _ok?@"true":@"false"),  @"ok",
                                       szerr, @"msg",
                                       [NSString stringWithFormat:@"%@",transaction.payment.productIdentifier], @"productId",
                                       [NSString stringWithFormat:@"%@",transaction.transactionIdentifier], @"transactionId",
                                       nil] );

                
                [[SKPaymentQueue defaultQueue] finishTransaction: transaction];

                break;
                
            /*
              This transaction restores content previously purchased by the user. Read the originalTransaction property
              to obtain information about the original purchase.
             */
            case SKPaymentTransactionStateRestored:
                NSLog(@"MobyShop: Transaction restored | SKPaymentTransactionStateRestored.");
                
                msg = @"Purchase restored";
                SendToStoreKitObject2( "iOSNativeCallback_TransactionRestored",
                                      [[NSDictionary alloc] initWithObjectsAndKeys:
                                       ( _ok?@"true":@"false"),  @"ok",
                                       msg, @"msg",
                                       [NSString stringWithFormat:@"%@",transaction.payment.productIdentifier], @"productId",
                                       [NSString stringWithFormat:@"%@",transaction.transactionIdentifier], @"transactionId",
                                       nil] );
                
                [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
                
                
                break;

            /*
              The transaction is in the queue, but its final status is pending external action such as Ask to Buy. Update
              your UI to show the deferred state, and wait for another callback that indicates the final status.
             */
            case SKPaymentTransactionStateDeferred:
                NSLog(@"MobyShop: Transaction Deferred, Waiting for User | SKPaymentTransactionStateDeferred.");
                msg=@"";
                SendToStoreKitObject2( "iOSNativeCallback_TransactionDeferred",
                                      [[NSDictionary alloc] initWithObjectsAndKeys:
                                       ( _ok?@"true":@"false"),  @"ok",
                                       msg, @"msg",
                                       [NSString stringWithFormat:@"%@",transaction.payment.productIdentifier], @"productId",
                                       [NSString stringWithFormat:@"%@",transaction.transactionIdentifier], @"transactionId",
                                       nil] );
                break;
                
            default:
                break;
        }
    };
}




+ (UnityStoreKit*)sharedInstance {
    static UnityStoreKit* sharedInstance=nil;
    static dispatch_once_t onceSharedInstance;
    dispatch_once( &onceSharedInstance, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}



@end




void Billing_Init( const char* strStoreObjectId, const char* commaSepProductIdList, const char* cbid ) {
    NSLog(@"MobyShop: Init: %s", cbid );
    NSString* _productIds = [NSString stringWithFormat:@"%s",commaSepProductIdList];
    NSString* nscbid = [NSString stringWithFormat:@"%s", cbid];
    NSSet* productIdentifiers = [NSSet setWithArray:[_productIds componentsSeparatedByString:@","]];

    NSLog( @"MobyShop: Loading Product Identifiers: %s", commaSepProductIdList );
    [UnityStoreKit sharedInstance].goid = [NSString stringWithFormat:@"%s", strStoreObjectId];
    [[UnityStoreKit sharedInstance] initProductCatalogue:productIdentifiers];
    
    // return callback.
    UnityStoreKit_Callback2( nscbid, true, @"" );
}


void Billing_BuyProduct( const char * szproductId, const char* szcb ) {
    NSLog(@"MobyShop: BuyProduct PId=%s CB=%s", szproductId, szcb );
    UnityStoreKit* _this = [UnityStoreKit sharedInstance];
    NSString* nsProductId = [NSString stringWithFormat:@"%s", szproductId];
    NSString* nscbid = [NSString stringWithFormat:@"%s",szcb];
    SKProduct* skp = [_this getProduct:nsProductId];
    if( skp == nil ) {
        NSLog( @"MobyShop: Error product is null! (%s)", szproductId );
        
        UnityStoreKit_Callback2( nscbid, false, @"Product is null" );
        
        BOOL invalid = false;
        for( NSString* invalidProductId in _this.invalidProducts ) {
            NSLog(@"Error: Invalid product id: %@" , invalidProductId);
            if( [invalidProductId isEqualToString:nsProductId] ) {
                invalid=true;
                break;
            }
        }
        NSString* errmsg = [NSString stringWithFormat:@"Could not find product: %s in iTuenes - it's not a valid product id.\n1. Check, that the product exists in iTunes Connect\n2. Check, you're signed into iTunes with a Test account if testing the app.\n3. Check that the app is signed with the right app identifier.", szproductId];
        if( !invalid ) {
            errmsg = [NSString stringWithFormat:@"Product could not be found in iTunes."];
        }

        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error"
                                                        message:errmsg
                                                       delegate:nil
                                              cancelButtonTitle:nil
                                              otherButtonTitles:@"Okay",nil];
        [alert show];
        
        return;
    }
    [_this buyProductImpl:skp cb:nscbid];
}


bool Billing_HasProduct( const char* productId ){
    NSLog( [NSString stringWithFormat:@"MobyShop: HasProduct %s", productId] );
    NSString* nsProductId = [NSString stringWithFormat:@"%s", productId];
    return [[UnityStoreKit sharedInstance] hasProduct:nsProductId];
}


char* UnityStoreKit_MakeStringCopy( const char* string ) {
    if (string == NULL)
        return NULL;
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}


const char* Billing_GetPriceTag( const char* productId ) {
    NSString* nsProductId = [NSString stringWithFormat:@"%s", productId];
    NSString* nsPrice = [[UnityStoreKit sharedInstance] priceAsStringFromProductIdentifier:nsProductId];
    return UnityStoreKit_MakeStringCopy([nsPrice UTF8String]); // Note: There is a memory leak here.
}


bool Billing_HasReceivedProductCatalogue() {
    bool hasProducts = [UnityStoreKit sharedInstance].hasReceivedProductCatalogue;
    return hasProducts;
}


bool Billing_CanMakePayments() {
    bool supportsPayment = [SKPaymentQueue canMakePayments];
    bool hasFailed = [UnityStoreKit sharedInstance].hasReceivedProductCatalogue;
    return supportsPayment && !hasFailed;
}


void Billing_RestorePurchases( const char* cbid ) {
    if( [UnityStoreKit sharedInstance].cbRestoreProducts!=nil ) {
        bool _ok = false;
        NSString* msg = @"Aleady Restoring Purchases.";
        UnityStoreKit_Callback( [NSString stringWithFormat:@"%s", cbid ],
                               [[NSDictionary alloc] initWithObjectsAndKeys:
                                ( _ok?@"true":@"false"),  @"ok",
                                msg, @"msg",
                                nil] );
        return;
    }
    [UnityStoreKit sharedInstance].cbRestoreProducts = [NSString stringWithFormat:@"%s",cbid];
    [[SKPaymentQueue defaultQueue] restoreCompletedTransactions];
}


void Billing_ShowError( const char* errmsg ) {
    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Billing Error"
                                                    message:[NSString stringWithFormat:@"%s",errmsg]
                                                   delegate:nil
                                          cancelButtonTitle:nil
                                          otherButtonTitles:@"Okay",nil];
    [alert show];
}




