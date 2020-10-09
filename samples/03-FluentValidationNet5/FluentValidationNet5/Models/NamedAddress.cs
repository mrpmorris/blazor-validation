namespace FluentValidationNet5.Models
{
    public class NamedAddress
    {
        public string Name { get; set; }
        public Address Address { get; set; } = new Address();
    }
}
