namespace ABC.Models.Domain
{
    public class Designation
    {

        // Primary key for the Designation
        public Guid Id { get; set; }

        // Name of the department (e.g., "Human Resources", "Finance")
        public string Name { get; set; }

        // Description of what the department does
        public string Description { get; set; }

        // Date when the department was established
        public DateTime CreatedDate { get; set; }

        // Status to determine if the department is active
        public bool IsActive { get; set; }


    }
}
