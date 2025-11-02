namespace _24hplusdotnetcore.ModelDtos.PtfOmnis
{
    public class PtfOmniFetchServiceResponse
    {
        public PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCifResponse>> FetchServiceCif { get; set; }
        public PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceCbsResponse>> FetchServiceCbs { get; set; }
        public PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceLosResponse>> FetchServiceLos { get; set; }
        public PtfOmniResponseModel<PtfOmniResponseDataModel<PtfOmniFetchServiceBlackListResponse>> FetchServiceBlackList { get; set; }

        public bool IsValid => true;
    }
}
