using DataLayer.Domin.Models.Interfaces;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataLayer.Domin.Models
{
    public class WebCityAndState : IDeleteBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CityAndStateId { get; set; }
        [JsonIgnore]
        public WebCityAndState CityAndStateItem { get; set; }
        [JsonIgnore]
        public ICollection<WebCityAndState> CityAndStates { get; set; }
        [JsonIgnore]
        public ICollection<ShopAddress> Addresses { get; set; }
        public bool IsDeleted { get; set; }
    }



}
