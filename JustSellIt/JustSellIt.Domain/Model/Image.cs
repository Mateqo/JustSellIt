namespace JustSellIt.Domain.Model
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public int Position { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
