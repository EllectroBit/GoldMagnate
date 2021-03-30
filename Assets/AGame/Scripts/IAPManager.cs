using System;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public DonateManager donateManager;

    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products
    private string ForgeBoost = "forge_boost";
    private string GoldPriceBoost = "gold_price_boost";
    private string NoAds = "no_ads";

    private string Money10k = "money_10k";
    private string Money1m = "money_1m";
    private string Money10m = "money_10m";
    private string Money1b = "money_1b";
    private string Money100b = "money_100b";

    private string SpecialFuel1 = "special_fuel_1";
    private string SpecialFuel10 = "special_fuel_10";
    private string SpecialFuel100 = "special_fuel_100";

    private string GetIngots = "get_ingots";


    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(ForgeBoost, ProductType.NonConsumable);
        builder.AddProduct(GoldPriceBoost, ProductType.NonConsumable);
        builder.AddProduct(NoAds, ProductType.NonConsumable);

        builder.AddProduct(Money10k, ProductType.Consumable);
        builder.AddProduct(Money1m, ProductType.Consumable);
        builder.AddProduct(Money10m, ProductType.Consumable);
        builder.AddProduct(Money1b, ProductType.Consumable);
        builder.AddProduct(Money100b, ProductType.Consumable);

        builder.AddProduct(SpecialFuel1, ProductType.Consumable);
        builder.AddProduct(SpecialFuel10, ProductType.Consumable);
        builder.AddProduct(SpecialFuel100, ProductType.Consumable);

        builder.AddProduct(GetIngots, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods

    public void BuyForgeBoost()
    {
        BuyProductID(ForgeBoost);
    }
    public void BuyGoldPriceBoost()
    {
        BuyProductID(GoldPriceBoost);
    }
    public void BuyNoAds()
    {
        BuyProductID(NoAds);
    }
    public void BuyMoney10k()
    {
        BuyProductID(Money10k);
    }
    public void BuyMoney1m()
    {
        BuyProductID(Money1m);
    }
    public void BuyMoney10m()
    {
        BuyProductID(Money10m);
    }
    public void BuyMoney1b()
    {
        BuyProductID(Money1b);
    }
    public void BuyMoney100b()
    {
        BuyProductID(Money100b);
    }
    public void BuySpecialFuel1()
    {
        BuyProductID(SpecialFuel1);
    }
    public void BuySpecialFuel10()
    {
        BuyProductID(SpecialFuel10);
    }
    public void BuySpecialFuel100()
    {
        BuyProductID(SpecialFuel100);
    }
    public void BuyGetIngots()
    {
        BuyProductID(GetIngots);
    }


    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, ForgeBoost, StringComparison.Ordinal))
        {
            donateManager.IsForgeBoosted = true;
            donateManager._Load();
        }
        else if(String.Equals(args.purchasedProduct.definition.id, GoldPriceBoost, StringComparison.Ordinal))
        {
            donateManager.IsGoldPriceBoosted = true;
            donateManager._Load();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, NoAds, StringComparison.Ordinal))
        {
            donateManager.IsNoAds = true;
            donateManager._Load();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Money10k, StringComparison.Ordinal))
        {
            donateManager.AddMoney(10000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Money1m, StringComparison.Ordinal))
        {
            donateManager.AddMoney(1000000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Money10m, StringComparison.Ordinal))
        {
            donateManager.AddMoney(10000000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Money1b, StringComparison.Ordinal))
        {
            donateManager.AddMoney(1000000000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Money100b, StringComparison.Ordinal))
        {
            donateManager.AddMoney(10000000000);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, SpecialFuel1, StringComparison.Ordinal))
        {
            donateManager.AddSpecialFuel(1);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, SpecialFuel10, StringComparison.Ordinal))
        {
            donateManager.AddSpecialFuel(10);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, SpecialFuel100, StringComparison.Ordinal))
        {
            donateManager.AddSpecialFuel(100);
        }
        else if(String.Equals(args.purchasedProduct.definition.id, GetIngots, StringComparison.Ordinal))
        {
            donateManager.OnGetIngotsBTClicked(true);
        }
        else
        {
            Debug.Log("Purchase Failed");
        }
        return PurchaseProcessingResult.Complete;
    }










    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}