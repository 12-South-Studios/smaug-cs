using Xunit;

namespace Test.Common;

public static class CollectionDefinitions
{
    public const string NonParallelCollection = "Non-Parallel Collection";
}

[CollectionDefinition(CollectionDefinitions.NonParallelCollection, DisableParallelization = true)]
public class NonParallelCollectionDefinitionClass;