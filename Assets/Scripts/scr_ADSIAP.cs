using System;
using UnityEngine;
using UnityEngine.Advertisements;
//using AppodealAds.Unity.Api;
using UnityEngine.Purchasing;

public class scr_ADSIAP : MonoBehaviour, IStoreListener
{
    #region MainData
    public int NoADS = 0;

    public int BlockAds = 3;
    //const string appKey = "1a770896baf8f6ef426903a41d89797bd7d3ce19dcf120e9";

    void Start()
    {
        //ADS inistalizaton 
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize("6105534c-b784-4eaf-b51c-013818ae9330");
        }
        else
        {
            Debug.Log("Platform is not suported");
        }
        //ADSInitialization(); (Appodeal)
        //IAP inistalizaton
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }
    #endregion

    #region ADS
    //Unity ADS
    public void ShowADS()
    {
        if(NoADS == 0)
        {
            if (Advertisement.IsReady())
            {
                if (BlockAds < 1)
                    Advertisement.Show();
                else
                    BlockAds--;
            }
        }
    }
    //Appodeal
    //private void ADSInitialization()
    //{
    //    Appodeal.initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO);
    //}
    //public void ADSShow()
    //{
    //    if (NoADS == 0)
    //        Appodeal.show(Appodeal.INTERSTITIAL);
    //}
    #endregion

    #region IAP
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    public static string NoAds = "no_ads";
    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(NoAds, ProductType.NonConsumable);

        //builder.Configure<IGooglePlayConfiguration>().SetPublicKey("");
        UnityPurchasing.Initialize(this, builder);
    }
    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }
    public void BuyNoADS()
    {
        BuyProductID(NoAds);
    }
    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, NoAds, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            NoADS = 1;
            GetComponent<scr_GameController>().Save();
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
    #endregion
}
