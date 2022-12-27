#if NEWSGENERATOR

namespace DataLayer.Infrastructure.NGWebModels
{
    public class NGSheetTextLine
    {
        public int Index { get; set; }
        public string TextLine { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string JsonStyle { get; set; }
        public int Width { get; set; }
        public string CalssName { get; set; }
    }
}
#endif