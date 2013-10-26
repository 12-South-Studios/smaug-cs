using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Objects;

namespace SmaugCS.Interfaces
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
        /// <returns></returns>
        ExtraDescriptionData Add(string keywords);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        bool Delete(string keywords);
    }
}
