namespace SurfsUp.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public DateTime DateOfDelivery { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public Board? Board { get; set; }
        // public ICollection<Board> Boards { get; set; }
        public decimal Total { get; set; }

        /*    
        public void Add(Board board)
        {
            if (!Boards.Contains(board))
            {
                Boards.Add(board);
                Total += board.Price;
            }
        }

        public void Remove(Board board)
        {
            if (!Boards.Contains(board))
            {
                Boards.Remove(board);
                Total -= board.Price;
            }
        }

        public void Commit(DateTime dateOfDelivery)
        {
            DateOfDelivery = dateOfDelivery;
            DateOfSubmission = DateTime.Now;
            foreach (Board board in Boards)
            {
                board.State = BoardState.Rented;
            }
        }
        */

    }
}
