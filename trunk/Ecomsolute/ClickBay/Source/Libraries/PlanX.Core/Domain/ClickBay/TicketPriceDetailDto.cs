using Newtonsoft.Json;

namespace PlanX.Core.Domain.ClickBay
{
    public class TicketPriceDetailDto : BaseEntity
    {
        /*Code:    "PassengerType" : "ADT",
                  "Description" : "Giá vé người lớn",
                  "Code" : "NET",
                  "Quantity" : 1,
                  "Price" : 895000.0000,
                  "Total" : 895000.0000
        */

        [JsonProperty("PassengerType")]
        public string PassengerType { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [JsonProperty("Quantity")]
        public System.Int16 Quantity { get; set; }

        [JsonProperty("Total")]
        public decimal Total { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

    }
}
