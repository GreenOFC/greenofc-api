namespace _24hplusdotnetcore.ModelDtos.LeadPtf
{

    public interface IUpdateLeadPtfPersonal
    {
        LeadPtfPersonalDto Personal { get; set; }
    }

    public interface ICreateLeadPtf
    {
        string ProductLine { get; set; }
        LeadPtfPersonalDto Personal { get; set; }
    }

    public interface ISubmitLeadPtf
    {
        bool IsSubmit { get; set; }
    }

    public interface IUpdateLeadPtf
    {

    }
}
