using System;

namespace CacheManager
{
    public class InventoryElement
    {
        public string Supplier { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Ean { get; set; }
        public string Brand { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
    }
}
