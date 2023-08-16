namespace MODELS
{
    
    public enum ProductStatus
    {
        available = 0,
        out_Of_Stock = 1,
        comming_soon = 2
    }

    public enum ProductType
    {
        Physical_Product,
        Service_Based,
        Digital
    }

    public enum ProductDeliveryMethod
    {
        Home_Delivery,
        Local_Pickup,
        Digital
    }

    public enum ProductSubscriptionType
    {
        One_Time_Purchase,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public enum OrderStatus{
        Pending,
        Approved,
        Denied,
        Canceled
    }
}