namespace SmaugCS.Data.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHasExtraDescriptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        void AddExtraDescription(string keywords, string description);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        bool DeleteExtraDescription(string keyword);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        ExtraDescriptionData GetExtraDescription(string keyword);
    }
}
