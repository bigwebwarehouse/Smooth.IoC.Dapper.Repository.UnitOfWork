using Dapper.FastCrud;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smooth.IoC.Repository.UnitOfWork;

public abstract class Entity<TPk> : IEntity<TPk>
    where TPk : IComparable
{
    [Key]
    [DatabaseGeneratedDefaultValue]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TPk Id { get; set; }
}