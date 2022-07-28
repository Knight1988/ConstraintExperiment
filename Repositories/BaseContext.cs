using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories;

public abstract class BaseContext : DbContext
{
    public static bool IsMySql { get; set; }
}