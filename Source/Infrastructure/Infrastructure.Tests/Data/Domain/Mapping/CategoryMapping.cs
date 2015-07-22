namespace Infrastructure.Tests.Data.Domain.Mapping
{
    public class CategoryMapping : EntityMappingBase<Category>
    {
        public CategoryMapping()
        {
            HasKey(x => x.Id);

            Property(x => x.Name).HasColumnName("Category Name");

            ToTable("Category");
        }
    }
}
