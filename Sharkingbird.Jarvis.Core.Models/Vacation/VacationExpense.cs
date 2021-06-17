namespace Sharkingbird.Jarvis.Core.Models.Vacation
{
  public sealed class VacationExpense
  {
    public VacationExpense(string name, bool isPaid, decimal amount)
    {
      Name = name;
      IsPaid = isPaid;
      Amount = amount;
    }
    public string Name { get; }
    public bool IsPaid { get; }
    public decimal Amount { get; }
  }
}
