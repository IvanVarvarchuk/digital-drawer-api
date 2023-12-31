﻿using DigitalDrawer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalDrawer.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<FileConversion> FileConversions { get; set; }
    public DbSet<UsersApiKey> ApiKeys { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}
