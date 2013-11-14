
namespace SmaugCS.Constants.Enums
{
    public enum GenderTypes
    {
        Neuter = 0,
        Male = 1,
        Female = 2
    }

    public static class GenderTypeExtensions
    {
        /// <summary>
        /// Converts the given Gender to a Subject Pronoun string
        /// </summary>
        /// <param name="type">The type of Gender</param>
        /// <returns>Returns a string representing the subject pronoun</returns>
        public static string SubjectPronoun(this GenderTypes type)
        {
            switch (type)
            {
                case GenderTypes.Male:
                    return "he";
                case GenderTypes.Female:
                    return "she";
                default:
                    return "it";
            }
        }

        /// <summary>
        /// Converts the given Gender to a Object Pronoun string
        /// </summary>
        /// <param name="type">The type of Gender</param>
        /// <returns>Returns a string representing the object pronoun</returns>
        public static string ObjectPronoun(this GenderTypes type)
        {
            switch (type)
            {
                case GenderTypes.Male:
                    return "him";
                case GenderTypes.Female:
                    return "her";
                default:
                    return "it";
            }
        }

        /// <summary>
        /// Converts the given Gender to a Possessive Pronoun string
        /// </summary>
        /// <param name="type">The type of Gender</param>
        /// <returns>Returns a string representing the possessive pronoun</returns>
        public static string PossessivePronoun(this GenderTypes type)
        {
            switch (type)
            {
                case GenderTypes.Male:
                    return "his";
                case GenderTypes.Female:
                    return "hers";
                default:
                    return "its";
            }
        }

        /// <summary>
        /// Converts the given Gender to a Reflexive Pronoun string
        /// </summary>
        /// <param name="type">The type of Gender</param>
        /// <returns>Returns a string representing the reflexive pronoun</returns>
        public static string ReflexivePronoun(this GenderTypes type)
        {
            switch (type)
            {
                case GenderTypes.Male:
                    return "himself";
                case GenderTypes.Female:
                    return "herself";
                default:
                    return "itself";
            }
        }
    }
}
