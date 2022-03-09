namespace Domain.Dto.Response.Management
{
    using System;

    public class TableInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsReserved { get; set; }
    }
}
