using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConstraintExperiment.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Models.NonConstraint;

public class Product : IBaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public int CategoryId { get; set; }
    [Unicode(false)]
    public string Name { get; set; }
    [Unicode(false)]
    public string Description { get; set; }
    public int Price { get; set; }
}