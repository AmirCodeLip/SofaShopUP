namespace DataLayer.Domin.Models.BaseModels.Implements
{
    public class BaseUniversal<TId> : BaseLocal<TId>
    {
        public string Culture { get; set; }
    }
}
