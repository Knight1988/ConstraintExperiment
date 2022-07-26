﻿using ConstraintExperiment.Interfaces;
using ConstraintExperiment.Models.Constraint;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Repositories;

public class Customer2Repo: BaseRepo<Customer2>, ICustomer2Repo
{
    private readonly ConstraintContext _context;

    public Customer2Repo(ConstraintContext context) : base(context, context.Customers, "Customers")
    {
        _context = context;
    }
}