﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.Data.Extensions;

namespace Infrastructure.Data.Specification
{
    public class Specification<TEntity> : ISpecification<TEntity>
    {   
        public Specification(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }

        public Specification<TEntity> And(Specification<TEntity> specification)
        {
            return new Specification<TEntity>(Predicate.And(specification.Predicate));
        }

        public Specification<TEntity> And(Expression<Func<TEntity, bool>> predicate)
        {
            return new Specification<TEntity>(Predicate.And(predicate));
        }    

        public Specification<TEntity> Or(Specification<TEntity> specification)
        {
            return new Specification<TEntity>(Predicate.Or(specification.Predicate));
        }

        public Specification<TEntity> Or(Expression<Func<TEntity, bool>> predicate)
        {
            return new Specification<TEntity>(Predicate.Or(predicate));
        }

        public TEntity SatisfyingEntityFrom(IQueryable<TEntity> query)
        {
            return query.Where(Predicate).SingleOrDefault();
        }

        public IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(Predicate);
        }

        public readonly Expression<Func<TEntity, bool>> Predicate;
    }
}
