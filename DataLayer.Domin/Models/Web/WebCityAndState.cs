using DataLayer.Domin.Models.BaseModels.Interfaces;
#if shop_project
using DataLayer.Domin.Models.WebShop;
#endif
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataLayer.Domin.Models.Web
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
#if shop_project
        [JsonIgnore]
        public ICollection<ShopAddress> Addresses { get; set; }
#endif
        public bool IsDeleted { get; set; }
    }
}
