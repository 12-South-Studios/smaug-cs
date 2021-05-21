using System;

namespace Realm.Library.Common.Entities

{
    /// <summary>
    /// Defines the basic entity factory framework
    /// </summary>
    public interface IEntityFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool Register(Type type);

        /// <summary>
        ///
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object Create(IHelper<Type> helper, string type, params object[] args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object Create(Type type, params object[] args);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        T Create<T>(params object[] args) where T : IEntity;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        T Clone<T>(T source, params object[] args) where T : IEntity;
    }
}