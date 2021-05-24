using Realm.Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;

namespace SmaugCS
{
    public sealed class CommandManager : GameSingleton
    {
        private static CommandManager _instance;
        private static readonly object Padlock = new object();

        private IRepositoryManager _dbManager;

        private CommandManager()
        {
        }

        public static CommandManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new CommandManager());
                }
            }
        }

        public void Initialize(IRepositoryManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void Execute(string commandName, CharacterInstance actor, string argument)
        {
            var command = _dbManager.GetEntity<CommandData>(commandName);
            if (command == null)
                throw new EntryNotFoundException();

            var attrib = command.DoFunction.Value.GetAttribute<CommandAttribute>(command.FunctionName);
            if (attrib != null)
            {
                if (CheckNpcAttribute(attrib, actor))
                    return;
            }

            command.DoFunction.Value.Invoke(actor, argument);
        }

        private static bool CheckNpcAttribute(CommandAttribute attrib, CharacterInstance actor)
        {
            return attrib.NoNpc && actor.IsNpc();
        }

        public CommandData FindCommand(string command)
        {
            return _dbManager.GetEntity<CommandData>(command);
        }
    }
}
