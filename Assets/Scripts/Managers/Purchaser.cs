//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Purchasing;
//
//// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, one of the existing Survival Shooter scripts.
//namespace CompleteProject
//{
//
//    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
//    public class Purchaser : MonoBehaviour, IStoreListener
//    {
//        private static IStoreController m_StoreController;                                                                  // Reference to the Purchasing system.
//        private static IExtensionProvider m_StoreExtensionProvider;                                                         // Reference to store-specific Purchasing subsystems.
//
//        // Product identifiers for all products capable of being purchased: "convenience" general identifiers for use with Purchasing, and their store-specific identifier counterparts 
//        // for use with and outside of Unity Purchasing. Define store-specific identifiers also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)
//
//        private static string kProductIDGold099 = "gold099";    // General handle for the consumable product.
//        private static string kProductIDGold199 = "gold199";    // General handle for the non-consumable product.
//        private static string kProductIDGold499 = "gold499";    // General handle for the subscription product.
//
//        private static string kProductNameAppleGold099 = "com.TeamWaesome.RumRunner.services.purchasing.gold099";             // Apple App Store identifier for the consumable product.
//        private static string kProductNameAppleGold199 = "com.TeamWaesome.RumRunner.services.purchasing.gold199";      // Apple App Store identifier for the non-consumable product.
//        private static string kProductNameAppleGold499 = "com.TeamWaesome.RumRunner.services.purchasing.gold499";       // Apple App Store identifier for the subscription product.
//
//        private static string kProductNameGooglePlayGold099 = "com.TeamWaesome.RumRunner.services.purchasing.gold099";        // Google Play Store identifier for the consumable product.
//        private static string kProductNameGooglePlayGold199 = "com.TeamWaesome.RumRunner.services.purchasing.gold199";     // Google Play Store identifier for the non-consumable product.
//        private static string kProductNameGooglePlayGold499 = "com.TeamWaesome.RumRunner.services.purchasing.gold499";  // Google Play Store identifier for the subscription product.
//
//        private GameManager _gameManager = null;
//
//        void Start()
//        {
//            // If we haven't set up the Unity Purchasing reference
//            if (m_StoreController == null)
//            {
//                // Begin to configure our connection to Purchasing
//                InitializePurchasing();
//            }
//            _gameManager = GameManager.instance;
//        }
//
//        public void InitializePurchasing()
//        {
//            // If we have already connected to Purchasing ...
//            if (IsInitialized())
//            {
//                // ... we are done here.
//                return;
//            }
//            // Create a builder, first passing in a suite of Unity provided stores.
//            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
//
//            // Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
//            builder.AddProduct(kProductIDGold099, ProductType.Consumable, new IDs() { { kProductNameAppleGold099, AppleAppStore.Name }, { kProductNameGooglePlayGold099, GooglePlay.Name }, }); 
//            builder.AddProduct(kProductIDGold199, ProductType.NonConsumable, new IDs() { { kProductNameAppleGold199, AppleAppStore.Name }, { kProductNameGooglePlayGold199, GooglePlay.Name }, }); 
//            builder.AddProduct(kProductIDGold499, ProductType.Subscription, new IDs() { { kProductNameAppleGold499, AppleAppStore.Name }, { kProductNameGooglePlayGold499, GooglePlay.Name }, }); // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
//            UnityPurchasing.Initialize(this, builder);
//        }
//
//
//        private bool IsInitialized()
//        {
//            // Only say we are initialized if both the Purchasing references are set.
//            return m_StoreController != null && m_StoreExtensionProvider != null;
//        }
//
//
//        public void BuyGold099()
//        {
//            // Buy the consumable product using its general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
//            BuyProductID(kProductIDGold099);
//        }
//
//
//        public void BuyGold199()
//        {
//            // Buy the non-consumable product using its general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
//            BuyProductID(kProductIDGold199);
//        }
//
//
//        public void BuyGold499()
//        {
//            // Buy the subscription product using its the general identifier. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
//            BuyProductID(kProductIDGold499);
//        }
//
//
//        void BuyProductID(string productId)
//        {
//            // If the stores throw an unexpected exception, use try..catch to protect my logic here.
//            try
//            {
//                // If Purchasing has been initialized ...
//                if (IsInitialized())
//                {
//                    // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
//                    Product product = m_StoreController.products.WithID(productId);
//
//                    // If the look up found a product for this device's store and that product is ready to be sold ... 
//                    if (product != null && product.availableToPurchase)
//                    {
//                        Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
//                        m_StoreController.InitiatePurchase(product);
//                    }
//                    // Otherwise ...
//                    else
//                    {
//                        // ... report the product look-up failure situation  
//                        Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//                    }
//                }
//                // Otherwise ...
//                else
//                {
//                    // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
//                    Debug.Log("BuyProductID FAIL. Not initialized.");
//                }
//            }
//            // Complete the unexpected exception handling ...
//            catch (Exception e)
//            {
//                // ... by reporting any unexpected exception for later diagnosis.
//                Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
//            }
//        }
//
//
//        // Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
//        public void RestorePurchases()
//        {
//            // If Purchasing has not yet been set up ...
//            if (!IsInitialized())
//            {
//                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
//                Debug.Log("RestorePurchases FAIL. Not initialized.");
//                return;
//            }
//
//            // If we are running on an Apple device ... 
//            if (Application.platform == RuntimePlatform.IPhonePlayer ||
//                Application.platform == RuntimePlatform.OSXPlayer)
//            {
//                // ... begin restoring purchases
//                Debug.Log("RestorePurchases started ...");
//
//                // Fetch the Apple store-specific subsystem.
//                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
//                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
//                apple.RestoreTransactions((result) => {
//                    // The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
//                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//                });
//            }
//            // Otherwise ...
//            else
//            {
//                // We are not running on an Apple device. No work is necessary to restore purchases.
//                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//            }
//        }
//
//
//        //  
//        // --- IStoreListener
//        //
//
//        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//        {
//            // Purchasing has succeeded initializing. Collect our Purchasing references.
//            Debug.Log("OnInitialized: PASS");
//
//            // Overall Purchasing system, configured with products for this application.
//            m_StoreController = controller;
//            // Store specific subsystem, for accessing device-specific store features.
//            m_StoreExtensionProvider = extensions;
//        }
//
//
//        public void OnInitializeFailed(InitializationFailureReason error)
//        {
//            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
//            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//        }
//
//
//        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
//        {
//            if (_gameManager == null) return PurchaseProcessingResult.Complete;
//            // A consumable product has been purchased by this user.
//            if (String.Equals(args.purchasedProduct.definition.id, kProductIDGold099, StringComparison.Ordinal))
//            {
//                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 1000 coins to the player's coin count.
//                foreach (UpgradeStruct us in _gameManager._allUpgrades)
//                {
//                    if (us.MoneyCost == 99)
//                    {
//                        _gameManager.AddCoins(us.CoinCost);
//                        break;
//                    }
//                }
//            }
//
//            else if (String.Equals(args.purchasedProduct.definition.id, kProductIDGold199, StringComparison.Ordinal))
//            {
//                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//                foreach (UpgradeStruct us in _gameManager._allUpgrades)
//                {
//                    if (us.MoneyCost == 199)
//                    {
//                        _gameManager.AddCoins(us.CoinCost);
//                        break;
//                    }
//                }
//            }
//            else if (String.Equals(args.purchasedProduct.definition.id, kProductIDGold499, StringComparison.Ordinal))
//            {
//                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//                foreach (UpgradeStruct us in _gameManager._allUpgrades)
//                {
//                    if (us.MoneyCost == 499)
//                    {
//                        _gameManager.AddCoins(us.CoinCost);
//                        break;
//                    }
//                }
//            }// Or ... an unknown product has been purchased by this user. Fill in additional products here.
//            else
//            {
//                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
//            }// Return a flag indicating wither this product has completely been received, or if the application needs to be reminded of this purchase at next app launch. Is useful when saving purchased products to the cloud, and when that save is delayed.
//            return PurchaseProcessingResult.Complete;
//        }
//
//
//        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//        {
//            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
//            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//        }
//    }
//}