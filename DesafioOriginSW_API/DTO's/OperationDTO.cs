namespace DesafioOriginSW_API.DTO_s
{
    public class OperationDTO
    {
        public Int32 id_operation { get; set; }

        public Int32 id_bank_card { get; set; }

        public Int32 id_operation_type { get; set; }

        public DateTime date { get; set; }

        public Double? amount { get; set; }
    }
}
