namespace SYT.VendingMachineSystem.VendingMachines
{
    public class PagedVendingMachineResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? isActive { get; set; }
        public int tenantId { get; set; }
    }
}