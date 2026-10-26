using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;

namespace Al_Shami_ِAISS_.Services;

/// <summary>
/// Business service for managing inbound contact messages.
/// This layer enforces business checks before data reaches the repository.
/// </summary>
public class ContactMessageService
{
    private readonly IContactMessageRepository _contactMessageRepository;

    /// <summary>
    /// Initializes the service with a repository dependency.
    /// </summary>
    public ContactMessageService(IContactMessageRepository contactMessageRepository)
    {
        _contactMessageRepository = contactMessageRepository;
    }

    /// <summary>
    /// Retrieves all contact messages sorted by date.
    /// </summary>
    public async Task<IEnumerable<ContactMessage>> GetAllAsync()
    {
        return await _contactMessageRepository.GetAllAsync();
    }

    /// <summary>
    /// Retrieves a single contact message by identifier.
    /// </summary>
    public async Task<ContactMessage?> GetByIdAsync(int id)
    {
        return await _contactMessageRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Creates a contact message after validation.
    /// </summary>
    public async Task<ContactMessage> AddAsync(ContactMessage contactMessage)
    {
        if (contactMessage == null)
        {
            throw new ArgumentNullException(nameof(contactMessage));
        }

        if (string.IsNullOrWhiteSpace(contactMessage.FullName))
        {
            throw new InvalidOperationException("Full name is required.");
        }

        if (string.IsNullOrWhiteSpace(contactMessage.Email))
        {
            throw new InvalidOperationException("Email is required.");
        }

        await _contactMessageRepository.AddAsync(contactMessage);
        await _contactMessageRepository.SaveChangesAsync();

        return contactMessage;
    }

    /// <summary>
    /// Updates an existing contact message.
    /// </summary>
    public async Task UpdateAsync(ContactMessage contactMessage)
    {
        if (contactMessage == null)
        {
            throw new ArgumentNullException(nameof(contactMessage));
        }

        if (string.IsNullOrWhiteSpace(contactMessage.FullName))
        {
            throw new InvalidOperationException("Full name is required.");
        }

        _contactMessageRepository.Update(contactMessage);
        await _contactMessageRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a contact message by identifier.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var message = await _contactMessageRepository.GetByIdAsync(id);

        if (message == null)
        {
            throw new KeyNotFoundException($"Contact message with id {id} was not found.");
        }

        _contactMessageRepository.Delete(message);
        await _contactMessageRepository.SaveChangesAsync();
    }
}
