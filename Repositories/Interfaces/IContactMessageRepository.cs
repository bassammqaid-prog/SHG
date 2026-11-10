using Al_Shami_AISS_.Models;

namespace Al_Shami_AISS_.Repositories.Interfaces;

/// <summary>
/// Defines the contract for contact message persistence.
/// The repository abstraction allows services to work with contacts without
/// depending on database-specific implementation details.
/// </summary>
public interface IContactMessageRepository
{
    /// <summary>
    /// Retrieves all contact messages.
    /// </summary>
    Task<IEnumerable<ContactMessage>> GetAllAsync();

    /// <summary>
    /// Retrieves a contact message by identifier.
    /// </summary>
    Task<ContactMessage?> GetByIdAsync(int id);

    /// <summary>
    /// Inserts a new contact message.
    /// </summary>
    Task AddAsync(ContactMessage contactMessage);

    /// <summary>
    /// Updates an existing contact message.
    /// </summary>
    void Update(ContactMessage contactMessage);

    /// <summary>
    /// Deletes a contact message.
    /// </summary>
    void Delete(ContactMessage contactMessage);

    /// <summary>
    /// Saves changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync();
}
