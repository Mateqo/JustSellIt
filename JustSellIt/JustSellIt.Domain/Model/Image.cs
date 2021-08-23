namespace JustSellIt.Domain.Model
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool isMain { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
