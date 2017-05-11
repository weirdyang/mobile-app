namespace LH.Forcas.Domain.UserData
{
    interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}