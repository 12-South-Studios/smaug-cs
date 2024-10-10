namespace SmaugCS.Repository;

public class GenericRepository<T> : Patterns.Repository.Repository<long, T> where T : class;