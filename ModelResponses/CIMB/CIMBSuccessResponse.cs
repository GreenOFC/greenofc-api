namespace _24hplusdotnetcore.ModelResponses.CIMB
{
    public class CIMBSuccessResponse<TData> : CIMBBaseResponse
    {
        public TData Data { get; set; }
    }
}
