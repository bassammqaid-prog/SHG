using Al_Shami_AISS_.Data;
using Al_Shami_AISS_.Models;
using Al_Shami_AISS_.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_AISS_.Repositories.Implementations;

/// <summary>
/// EF Core repository responsible for contact message persistence.
/// The repository pattern keeps all database logic in one place.
/// </summary>
public class ContactMessageRepository : IContactMessageRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the repository with the shared DbContext.
    /// </summary>
    public ContactMessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ContactMessage>> GetAllAsync()
    {
        return await _context.ContactMessages
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ContactMessage?> GetByIdAsync(int id)
    {
        return await _context.ContactMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <inheritdoc />
    public async Task AddAsync(ContactMessage contactMessage)
    {
        await _context.ContactMessages.AddAsync(contactMessage);
    }

    /// <inheritdoc />
    public void Update(ContactMessage contactMessage)
    {
        _context.ContactMessages.Update(contactMessage);
    }

    /// <inheritdoc />
    public void Delete(ContactMessage contactMessage)
    {
        _context.ContactMessages.Remove(contactMessage);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
