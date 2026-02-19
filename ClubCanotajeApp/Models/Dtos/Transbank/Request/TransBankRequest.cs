namespace ClubCanotajeAPI.Models.Dtos.Transbank.Request
{

    public class CreateTransaction
    {
        public string buy_order { get; set; }
        public string session_id { get; set; }
        public decimal amount { get; set; }
        public string return_url { get; set; }
    }

    public class ConfirmTransaction
    {
        public string Token { get; set; }
    }
}
