namespace ClubCanotajeAPI.Models.Dtos.Transbank.Response
{
    public class TransBankResponseGlobal
    {
        public bool Exito { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }


    public class CreateTransactionResponse
    {
        public string token { get; set; }
        public string url { get; set; }
    }

    public class ConfirmTransactionResponse
    {
        public string vci { get; set; }
        public decimal? amount { get; set; }
        public string status { get; set; }
        public string buy_order { get; set; }
        public string session_id { get; set; }
        public card_detail card_detail { get; set; }
        public string accounting_date { get; set; }
        public string transaction_date { get; set; }
        public string authorization_code { get; set; }
        public string payment_type_code { get; set; }
        public int? response_code { get; set; }
        public int installments_amount { get; set; }
        public int installments_number { get; set; }
        public int balance { get; set; }

    }

    public class card_detail
    {
        public string card_number { get; set; }
    }

}
