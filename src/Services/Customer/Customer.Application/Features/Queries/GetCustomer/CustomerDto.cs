namespace Customer.Application.Features.Queries
    {
    public class CustomerDto
        {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime Created { get; set; }
        }
    }