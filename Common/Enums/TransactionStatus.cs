namespace _24hplusdotnetcore.Common.Enums
{
    public enum TransactionStatus
    {
        INIT,
        PENDING,
        SUCCEEDED,
        FAILED,
        CANCELED,
        EXPIRED,
        REFUNDED,
        PAYED,
        CANCELED_SUCCEEDED
    }
    public enum BillType
    {
        MC_CHECK_SIM
    }
    public enum BillStatus
    {
        INIT,
        SENT_OTP,
        RESENT_OTP,
        VERIFIED_OTP
    }

    public enum TransactionResponseCode
    {
        SUCCEEDED = 105003
    }

    public enum TransactionType
    {
        DEPOSIT,
        COMMISSION,
        WITHDRAW,
        REFUND
    }
}
