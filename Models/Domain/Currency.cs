namespace ABC.Models.Domain
{
    public class Currency
    {

        /// <summary>
        /// Unique identifier for the currency.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the currency (e.g., US Dollar, Euro).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ISO 4217 code of the currency (e.g., USD, EUR).
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Symbol representing the currency (e.g., $, €).
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Timestamp when the currency was added.
        /// </summary>
        public DateTime AddedOn { get; set; }

        /// <summary>
        /// Identifier for the user who added the currency.
        /// </summary>
        public string AddedBy { get; set; }

        // Constructor
        public Currency()
        {
            Id = Guid.NewGuid(); // Automatically generates a new UUID
            AddedOn = DateTime.UtcNow; // Sets the added timestamp to the current UTC time
        }
    }

}
