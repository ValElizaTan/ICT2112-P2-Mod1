namespace ProRental.Domain.Entities;
public class CreditCardPaymentDetails : PaymentMethodDetails
{
    private readonly string _cardNumber;
    private readonly DateOnly _expirationDate;
    private readonly int _securityCode;
    private readonly string _nameOnCard;

    public string CardNumber => _cardNumber;
    public DateOnly ExpirationDate => _expirationDate;
    public int SecurityCode => _securityCode;
    public string NameOnCard => _nameOnCard;

    // Constructor to initialize the properties
    public CreditCardPaymentDetails(string cardNumber, DateOnly expirationDate, int securityCode, string nameOnCard)
        : base(Enums.PaymentMethod.CREDIT_CARD)
    {
        _cardNumber = cardNumber;
        _expirationDate = expirationDate;
        _securityCode = securityCode;
        _nameOnCard = nameOnCard;
    }
}
