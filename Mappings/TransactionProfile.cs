using _24hplusdotnetcore.ModelDtos.eWalletTransaction;
using _24hplusdotnetcore.ModelDtos.Transaction;
using _24hplusdotnetcore.Models.eWalletTransaction;
using AutoMapper;

namespace _24hplusdotnetcore.Mappings
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionModel, CreateTransactionDto>().ReverseMap();
            CreateMap<PaymeCreateOrderRequest, TransactionModel>().ReverseMap();
            CreateMap<TransactionIpnModel, IpnPaymeRequest>().ReverseMap();
            CreateMap<TransactionModel, TransactionResponse>()
                .ForMember(dest => dest.PaymeResponse, opt => opt.MapFrom(src => src.PaymeResponse));

            CreateMap<PayMeOrderData, PayMeDataResponse>().ReverseMap();
            CreateMap<BillStepHistories, BillStepHistoriesResponse>().ReverseMap();

            CreateMap<TransactionLogModel, eWalletTransactionLogDto>().ReverseMap();
            CreateMap<TransactionModel, PaymeRefundDto>()
                 .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.MobileNetworkFee))
                 .ReverseMap();

            CreateMap<TransactionModel, TransactionModel>().ReverseMap();
        }
    }

}
