using Realm.Library.Common.Attributes;

namespace SmaugCS.Common.Enumerations
{
    public enum ClimateTypes
    {
        Rainforest = 0,
        Savanna = 1,
        Desert = 2,
        Steppe = 3,
        Chapparal = 4,
        Grasslands = 5,

        [Name("Deciduous_Forest")]
        Deciduous = 6,
        Taiga = 7,
        Tundra = 8,
        Alpine = 9,
        Arctic = 10,
        Subarctic = 11,
        Coastal = 12,
        Humid = 13,
        Tropical = 14,
        Arid = 15
    }
}
