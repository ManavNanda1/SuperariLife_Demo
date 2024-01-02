namespace SuperariLife.Model.CommonPagination
{
    public class CommonPaginationModel
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public string? StrSearch { get; set; }
        public long? UserId { get; set; }
        public bool? AllUser { get; set; }
        public long? CustomerId { get; set; }
    }
    public class CommonDeleteModel
    {
        public long? Id { get; set; }
        public string? Name { get; set; }   
    }

    public class CommonUserModel
    {
        public long? Id { get; set; }
        public long AdminRoleId { get; set; }
        public long? CompanyId { get; set; }
        public long? UpdateById { get; set; }
    }

    
}
