namespace WebApp.Shop.Neptons.Models
{
    public class DefaultTSComponent
    {
        public bool HasDefaultName { get; private set; } = false;
        public string defaultName;
        public string DefaultName
        {
            get => defaultName; set
            {
                HasDefaultName = !string.IsNullOrWhiteSpace(value);
                defaultName = value;
            }
        }
        public string Header { get; set; }
        public Type ModelType { get; set; }
    }
}
